/**
 *	UI管理类 
 *	author:zhangjiasu
 *	date:2021年4月23日
 *	copyright: fungather.net
 ***/
using System;
using System.Collections.Generic;
using JSTools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;


namespace Managers
{
    public struct StrMapping
    {
        public const string UIManager = "UIManager";
        public const string UI = "UI";
        public const string UICamera = "UICamera";
        public const string UIRoot = "UIRoot";
        public const string SceneUI = "SceneUI";
        public const string Hud = "Hud";
        public const string Dialog = "Dialog";
        public const string Popup = "Popup";
        public const string Tip = "Tip";
        public const string Blocker = "Blocker";

        //第二层UI
        public const string UICameraAfter = "UICameraAfter";
        public const string UIRootAfter = "UIRootAfter";
    }

    public enum LayerSortingOrder
    {
        SceneUI = 0,
        Hud = 1000,
        Dialog = 2000,
        Popup = 3000,
        Tip = 4000,
        Blocker = 5000,
    }

    public class UIManager : BaseManager
    {
        private static UIManager uiManager;

        private static Camera uiCamera;
        private static Camera uiCameraAfter;
        private static GameObject UIManagerGo;
        private static GameObject canvasRoot;
        private static GameObject canvasRootAfter;
        private static EventSystem s_EventSystem;
        private static UILayerManager s_UILayerManager; //UI层级管理器
        private static UIAnimManager s_UIAnimManager;   //UI动画管理器
        private static UIStackManager s_UIStackManager; //UI栈管理器
        private static CamShotUtil s_CamShotUtil;
        public static AudioSource soundsEffectAudioSource;
        public static AudioSource dubAudioSource;
        public static AudioSource phoneSource = null;//手机

        // Dialog打开时隐藏了哪些PopUp（用于关闭时还原回去）
        private static Dictionary<UIWindowBase, List<UIWindowBase>> s_HidePopupUIListByDialog = new Dictionary<UIWindowBase, List<UIWindowBase>>();
        // 打开的UI
        static public Dictionary<UIType, Dictionary<string, List<UIWindowBase>>> s_UIs = new Dictionary<UIType, Dictionary<string, List<UIWindowBase>>>();
        // 隐藏的UI
        static public Dictionary<UIType, Dictionary<string, List<UIWindowBase>>> s_hideUIs = new Dictionary<UIType, Dictionary<string, List<UIWindowBase>>>();
        // 常驻的UI
        static public Dictionary<UIType, Dictionary<string, List<UIWindowBase>>> s_PermanentUIs = new Dictionary<UIType, Dictionary<string, List<UIWindowBase>>>();

        public static bool IsGlassBlurExcute = false;

        public static bool IsCutSceneing = false;

        public enum UICamShowType
        {
            Normal,
            Multi
        }

        #region 初始化
        public static UIManager GetInstance()
        {
            if (uiManager == null)
            {
                uiManager = new UIManager();
                Init();
            }

            GameObject.DontDestroyOnLoad(UIManagerGo);

            return uiManager;
        }

        public static void Init()
        {
            //创建UIManager Object
            UIManagerGo = new GameObject(StrMapping.UIManager);
            UIManagerGo.layer = LayerMask.NameToLayer(StrMapping.UI);

            s_UILayerManager = UIManagerGo.AddComponent<UILayerManager>();
            s_UIAnimManager = UIManagerGo.AddComponent<UIAnimManager>();
            s_UIStackManager = UIManagerGo.AddComponent<UIStackManager>();
            s_CamShotUtil = UIManagerGo.AddComponent<CamShotUtil>();
            s_EventSystem = UIManagerGo.GetComponentInChildren<EventSystem>();
            s_DelayUnloadAssetList.Clear();
            CreateUICamera(StrMapping.UIRoot);
            CreateUIAfterCamera(StrMapping.UIRootAfter);
            SetUICameraStack();
            SetUICameraShow();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            DestroyAllUI();

            UIStackManager.Clear();
        }

        public static void CreateUICamera(string key)
        {
            UILayerManager.UICameraData uICameraData = new UILayerManager.UICameraData();

            uICameraData.m_key = key;

            canvasRoot = new GameObject(key);
            RectTransform canvasRt = canvasRoot.AddComponent<RectTransform>();

            canvasRt.SetParent(UIManagerGo.transform);
            uICameraData.m_root = canvasRoot;

            //UIcamera
            GameObject cameraGo = new GameObject(StrMapping.UICamera);
            cameraGo.transform.SetParent(canvasRoot.transform);
            cameraGo.transform.localPosition = new Vector3(0, 0, -5000);
            uiCamera = cameraGo.AddComponent<Camera>();
            uiCamera.cullingMask = LayerMask.GetMask(StrMapping.UI);
            uiCamera.orthographic = true;
            uiCamera.clearFlags = CameraClearFlags.Depth;
            uiCamera.depth = 1;
            uiCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
            // uiCamera.GetUniversalAdditionalCameraData().gameCamType = UniversalAdditionalCameraData.GameCamType.UI;
            uICameraData.m_camera = uiCamera;

            //音效
            soundsEffectAudioSource = cameraGo.AddComponent<AudioSource>();
            dubAudioSource = cameraGo.AddComponent<AudioSource>();
            phoneSource = cameraGo.AddComponent<AudioSource>();

            SetCameraRender(1);

            //Canvas
            Canvas canvasComp = canvasRoot.AddComponent<Canvas>();
            canvasComp.renderMode = RenderMode.ScreenSpaceCamera;
            canvasComp.worldCamera = uiCamera;

            //CanvasScaler 竖屏 640 * 1136
            CanvasScaler scaler = canvasRoot.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(ScreenUtil.DESGIN_RESOLUTION_WIDTH, ScreenUtil.DESGIN_RESOLUTION_HEIGHT);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            //动态调整按宽还是高放缩
            float sWToH = scaler.referenceResolution.x * 1.0f / scaler.referenceResolution.y;
            float vWToH = Screen.width * 1.0f / Screen.height;
            if (sWToH > vWToH)
            {
                //匹配宽
                scaler.matchWidthOrHeight = 0;
            }
            else
            {
                //匹配高
                scaler.matchWidthOrHeight = 1;
            }

            //挂载点
            GameObject goTmp = null;
            RectTransform rtTmp = null;
            UILayerManager UILayerManager = UIManagerGo.GetComponent<UILayerManager>();
            uICameraData.m_uiLayerParents = new Dictionary<UIType, Transform>();

            InitUILayer(StrMapping.SceneUI, canvasRoot, UIType.SceneUI, (int)LayerSortingOrder.SceneUI, goTmp, rtTmp, uICameraData);
            InitUILayer(StrMapping.Hud, canvasRoot, UIType.Hud, (int)LayerSortingOrder.Hud, goTmp, rtTmp, uICameraData);
            InitUILayer(StrMapping.Dialog, canvasRoot, UIType.Dialog, (int)LayerSortingOrder.Dialog, goTmp, rtTmp, uICameraData);
            InitUILayer(StrMapping.Popup, canvasRoot, UIType.Popup, (int)LayerSortingOrder.Popup, goTmp, rtTmp, uICameraData);
            InitUILayer(StrMapping.Tip, canvasRoot, UIType.Tip, (int)LayerSortingOrder.Tip, goTmp, rtTmp, uICameraData);
            InitUILayer(StrMapping.Blocker, canvasRoot, UIType.Blocker, (int)LayerSortingOrder.Blocker, goTmp, rtTmp, uICameraData);

            UILayerManager.UICameraList.Add(uICameraData);

        }

        public static void CreateUIAfterCamera(string key)
        {
            UILayerManager.UICameraData uICameraData = new UILayerManager.UICameraData();

            uICameraData.m_key = key;

            canvasRootAfter = new GameObject(key);
            RectTransform canvasRt = canvasRootAfter.AddComponent<RectTransform>();

            canvasRt.SetParent(UIManagerGo.transform);
            uICameraData.m_root = canvasRootAfter;

            //UIcamera
            GameObject cameraGo = new GameObject(StrMapping.UICameraAfter);
            cameraGo.transform.SetParent(canvasRootAfter.transform);
            cameraGo.transform.localPosition = new Vector3(0, 0, -5000);
            uiCameraAfter = cameraGo.AddComponent<Camera>();
            uiCameraAfter.cullingMask = LayerMask.GetMask(StrMapping.UI);
            uiCameraAfter.orthographic = true;
            uiCameraAfter.clearFlags = CameraClearFlags.Depth;
            uiCameraAfter.depth = 1;
            uiCameraAfter.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
            // uiCameraAfter.GetUniversalAdditionalCameraData().gameCamType = UniversalAdditionalCameraData.GameCamType.UI;
            uICameraData.m_camera = uiCameraAfter;

            SetCameraRender(1);

            //Canvas
            Canvas canvasComp = canvasRootAfter.AddComponent<Canvas>();
            canvasComp.renderMode = RenderMode.ScreenSpaceCamera;
            canvasComp.worldCamera = uiCameraAfter;

            //CanvasScaler 竖屏 640 * 1136
            CanvasScaler scaler = canvasRootAfter.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(ScreenUtil.DESGIN_RESOLUTION_WIDTH, ScreenUtil.DESGIN_RESOLUTION_HEIGHT);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            //动态调整按宽还是高放缩
            float sWToH = scaler.referenceResolution.x * 1.0f / scaler.referenceResolution.y;
            float vWToH = Screen.width * 1.0f / Screen.height;
            if (sWToH > vWToH)
            {
                //匹配宽
                scaler.matchWidthOrHeight = 0;
            }
            else
            {
                //匹配高
                scaler.matchWidthOrHeight = 1;
            }
            //挂载点
            GameObject goTmp = null;
            RectTransform rtTmp = null;
            UILayerManager UILayerManager = UIManagerGo.GetComponent<UILayerManager>();
            uICameraData.m_uiLayerParents = new Dictionary<UIType, Transform>();

            InitUILayer(StrMapping.SceneUI, canvasRootAfter, UIType.SceneUI, (int)LayerSortingOrder.SceneUI, goTmp, rtTmp, uICameraData);
            InitUILayer(StrMapping.Hud, canvasRootAfter, UIType.Hud, (int)LayerSortingOrder.Hud, goTmp, rtTmp, uICameraData);
            InitUILayer(StrMapping.Dialog, canvasRootAfter, UIType.Dialog, (int)LayerSortingOrder.Dialog, goTmp, rtTmp, uICameraData);
            InitUILayer(StrMapping.Popup, canvasRootAfter, UIType.Popup, (int)LayerSortingOrder.Popup, goTmp, rtTmp, uICameraData);
            InitUILayer(StrMapping.Tip, canvasRootAfter, UIType.Tip, (int)LayerSortingOrder.Tip, goTmp, rtTmp, uICameraData);
            InitUILayer(StrMapping.Blocker, canvasRootAfter, UIType.Blocker, (int)LayerSortingOrder.Blocker, goTmp, rtTmp, uICameraData);

            UILayerManager.UICameraList.Add(uICameraData);
        }

        public static void SetCameraRender(int index)
        {
            if (uiCamera != null)
            {
                uiCamera.GetUniversalAdditionalCameraData().SetRenderer(1);
            }
            if (uiCameraAfter != null)
            {
                uiCameraAfter.GetUniversalAdditionalCameraData().SetRenderer(3);
            }
        }

        private static void InitUILayer(String layerName, GameObject root, UIType uiType, int sortingOrder, GameObject goTmp, RectTransform rtTmp, UILayerManager.UICameraData uICameraData)
        {
            goTmp = new GameObject(layerName);
            goTmp.layer = LayerMask.NameToLayer(StrMapping.UI);
            goTmp.transform.SetParent(root.transform);
            goTmp.transform.localScale = Vector3.one;
            rtTmp = goTmp.AddComponent<RectTransform>();
            rtTmp.anchorMax = new Vector2(1, 1);
            rtTmp.anchorMin = new Vector2(0, 0);
            rtTmp.anchoredPosition3D = Vector3.zero;
            rtTmp.sizeDelta = Vector2.zero;

            uICameraData.m_uiLayerParents[uiType] = goTmp.transform;

            Canvas canvasUI = goTmp.AddComponent<Canvas>();
            canvasUI.overrideSorting = true;
            canvasUI.sortingLayerName = layerName;
            canvasUI.sortingOrder = sortingOrder;
            canvasUI.additionalShaderChannels |= AdditionalCanvasShaderChannels.TexCoord1;

            //UI Raycaster
            goTmp.AddComponent<GraphicRaycaster>();
        }

        public static void SetUICameraStack()
        {
            if (Camera.main != null)
            {
                if (uiCamera != null)
                {
                    uiCamera.GetUniversalAdditionalCameraData().renderType = UnityEngine.Rendering.Universal.CameraRenderType.Overlay;
                    if (!Camera.main.GetUniversalAdditionalCameraData().cameraStack.Contains(uiCamera))
                        Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(uiCamera);
                }
                if (uiCameraAfter != null)
                {
                    if (!Camera.main.GetUniversalAdditionalCameraData().cameraStack.Contains(uiCameraAfter))
                    {
                        uiCameraAfter.GetUniversalAdditionalCameraData().renderType = UnityEngine.Rendering.Universal.CameraRenderType.Overlay;
                        Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(uiCameraAfter);
                    }
                }
            }
        }

        public static void SetUICameraShow(UICamShowType type = UICamShowType.Normal)
        {
            switch (type)
            {
                case UICamShowType.Normal:
                    if (uiCamera != null)
                    {
                        canvasRootAfter.SetActiveOptimize(false);
                        s_UILayerManager?.SetCurUseCam(0);
                    }
                    break;
                case UICamShowType.Multi:
                    if (uiCamera != null && uiCameraAfter != null)
                    {
                        canvasRootAfter.SetActiveOptimize(true);
                        s_UILayerManager?.SetCurUseCam(1);
                    }
                    break;
            }

        }

        public static CanvasScaler GetCansScaler()
        {
            return canvasRoot?.GetComponent<CanvasScaler>();
        }

        public static Rect GetScreenRect()
        {
            Rect rect = new Rect(-Screen.width / 2,
                    -Screen.height / 2,
                    Screen.width,
                    Screen.height);

            CanvasScaler _canvasScaler = GetCansScaler();

            float scale = Math.Abs(_canvasScaler.matchWidthOrHeight - 1f) < 0.05f ?
                _canvasScaler.referenceResolution.y / (float)Screen.height :
                _canvasScaler.referenceResolution.x / (float)Screen.width;

            rect = new Rect(rect.x * scale,
                            rect.y * scale,
                            rect.width * scale,
                            rect.height * scale);

            return rect;

        }

        public static float GetScreenScale()
        {

            CanvasScaler _canvasScaler = GetCansScaler();
            if (_canvasScaler == null) return 1.0f;
            float scale = Math.Abs(_canvasScaler.matchWidthOrHeight - 1f) < 0.05f ?
                _canvasScaler.referenceResolution.y / (float)Screen.height :
                _canvasScaler.referenceResolution.x / (float)Screen.width;

            return scale;

        }

        /// <summary>
        /// 获取当前Canvas 的高度对比
        /// </summary>
        /// <returns></returns>
        public static float GetConstScale()
        {
            RectTransform root = canvasRoot.GetComponent<RectTransform>();
            CanvasScaler _canvasScaler = GetCansScaler();

            float scale = root.sizeDelta.y / (ScreenUtil.DESGIN_RESOLUTION_HEIGHT * 1f);

            return scale;
        }

        /// <summary>
        /// 获取当前Canvas 的宽度对比
        /// </summary>
        /// <returns></returns>
        public static float GetConstScaleX()
        {
            RectTransform root = canvasRoot.GetComponent<RectTransform>();
            CanvasScaler _canvasScaler = GetCansScaler();

            float scale = root.sizeDelta.x / (ScreenUtil.DESGIN_RESOLUTION_WIDTH * 1f);

            return scale;
        }

        public static UILayerManager UILayerManager
        {
            get
            {
                if (s_UILayerManager == null)
                {
                    Init();
                }
                return s_UILayerManager;
            }

            set
            {
                s_UILayerManager = value;
            }
        }

        public static UIAnimManager UIAnimManager
        {
            get
            {
                if (s_UILayerManager == null)
                {
                    Init();
                }
                return s_UIAnimManager;
            }

            set
            {
                s_UIAnimManager = value;
            }
        }

        public static UIStackManager UIStackManager
        {
            get
            {
                if (s_UIStackManager == null)
                {
                    Init();
                }
                return s_UIStackManager;
            }

            set
            {

                s_UIStackManager = value;
            }
        }

        public static EventSystem EventSystem
        {
            get
            {
                if (s_EventSystem == null)
                {
                    Init();
                }
                return s_EventSystem;
            }

            set
            {
                s_EventSystem = value;
            }
        }

        public static Camera UICamera
        {
            get
            {
                return uiCamera;
            }
        }

        #endregion

        #region 继承
        public override void OnUpdate()
        {
            base.OnUpdate();
            if (s_DelayUnloadAssetList.Count > 0)
            {
                foreach (var item in s_DelayUnloadAssetList)
                {
                    AssetManager.Instance.Unload(item);
                }
                s_DelayUnloadAssetList.Clear();
            }

            if (s_UIStackManager == null) return;
            if (IsCutSceneing) return;

            // if (GameDefine.SceneType.World == ScenesManager.Instance.GetCurrentScene())
            // {
            //     Managers.WorldManager.wWorldCamera.OnCamDraw(!(s_UIStackManager.GetAllActiveUICount(UIType.Dialog, true) > 0));
            // }
            // else if (GameDefine.SceneType.City == ScenesManager.Instance.GetCurrentScene())
            // {
            //     if (CameraUtil.GetMainCamera() != null)
            //     {
            //         if (s_UIStackManager.GetAllActiveUICount(UIType.Dialog, true) > 0)
            //         {
            //             if (CameraUtil.GetMainCamera().cullingMask != 0)
            //             {
            //                 LogUtil.Log("城市相机不渲染");
            //                 CameraUtil.GetMainCamera().cullingMask = 0;
            //                 s_UILayerManager?.SetLayerState(UIType.SceneUI, false);
            //                 s_UILayerManager?.SetLayerState(UIType.Hud, false);
            //                 UIManager.GetUI<SunLight>(UIType.SceneUI)?.Move(true);
            //             }
            //         }
            //         else
            //         {
            //             if (CameraUtil.GetMainCamera().cullingMask != -1)
            //             {
            //                 LogUtil.Log("城市相机渲染");
            //                 CameraUtil.GetMainCamera().cullingMask = -1;
            //                 s_UILayerManager?.SetLayerState(UIType.SceneUI, true);
            //                 s_UILayerManager?.SetLayerState(UIType.Hud, true);
            //                 UIManager.GetUI<SunLight>(UIType.SceneUI)?.Move(false);
            //             }
            //         }
            //     }
            // }
        }
        #endregion

        #region EventSystem

        public static void SetEventSystemEnable(bool enable)
        {
            if (EventSystem != null)
            {
                EventSystem.enabled = enable;
            }
            else
            {
                LogUtil.Error("EventSystem.current is null !");
            }
        }

        #endregion

        #region UI的打开与关闭方法

        /// <summary>
        /// 创建uibase模板
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dialog"></param>
        /// <param name="prefabsName"></param>
        /// <param name="id"></param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static T CreateUiBase<T>(GameObject dialog, string uiEventKey, params object[] objs) where T : UIBase
        {
            return CreateUiBase<T>(dialog, "", uiEventKey, 0, objs);
        }
        public static T CreateUiBase<T>(GameObject dialog, string prefabsName, string uiEventKey, int id, params object[] objs) where T : UIBase
        {
            if (string.IsNullOrEmpty(prefabsName))
            {
                Type t = typeof(T);
                prefabsName = t.Name;
            }
            if (null == dialog)
            {
                LogUtil.Error("CreateUiBase--prefabsName={0}--dialog=null.", prefabsName);
                return null;
            }
            //LogUtil.LogKeyValue(LogUtil.OPENUIB, prefabsName);
            GameObject itemPrefab = AssetManager.Instance.LoadAndInstantiate(UIPath.GetPath(prefabsName), typeof(GameObject), dialog.transform);
            T uiBase = itemPrefab.GetComponent<T>();
            uiBase.AssetName = prefabsName;

            if (null == uiBase)
            {
                LogUtil.Error("CreateUiBase--prefabsName={0}--uiBase=null.", prefabsName);
                return null;
            }
            uiBase.Init(uiEventKey, id, objs);
            return uiBase;
        }

        /// <summary>
        /// 销毁一个uiBase
        /// </summary>
        /// <param name="uIBase"></param>
        public static void DestroyUIBase(UIBase uIBase)
        {
            if (null == uIBase)
                return;
            uIBase.Dispose();
            uIBase.transform.SetParent(null);
            ReleaseUI(uIBase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parentObject"></param>
        public static void DestroyChildrenUIBase(GameObject parentObject)
        {
            if (null == parentObject)
            {
                return;
            }
            int childCount = parentObject.transform.childCount;
            for (int i = 0; i < childCount; i++)
            {
                UIBase cell = parentObject.transform.GetChild(0).gameObject.GetComponent<UIBase>();
                Managers.UIManager.DestroyUIBase(cell);
            }
        }

        /// <summary>
        /// 创建UI,如果不打开则存放在Hide列表中
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateUIWindow<T>(UICallBack callback = null, params object[] objs) where T : UIWindowBase
        {
            return (T)CreateUIWindow(typeof(T).Name, callback, objs);
        }
        /// <summary>
        /// 创建UI
        /// </summary>
        /// <param name="UIName"></param>
        /// <param name="callback"></param>
        /// <param name="objs">第一个参数固定为界面类型，比如CommonUI，类型1对应CommonUI_1.prefab,没有类型有参数：{0, 参数},没有类型无参数：{},有类型无参数：{类型}</param>
        /// <returns></returns>
        public static UIWindowBase CreateUIWindow(string UIName, UICallBack callback = null, params object[] objs)
        {
            //LogUtil.Log("CreateUIWindow " + UIName);
            LogUtil.LogKeyValue(LogUtil.OPENUIW, UIName);
            Transform transform = UIManagerGo == null ? null : UIManagerGo.transform;
            string uiPath = UIPath.GetPath(UIName);

            GameObject UITmp = AssetManager.Instance.LoadAndInstantiate(uiPath, typeof(GameObject), transform);
            string name = UIName.Substring(UIName.LastIndexOf("/") + 1);
            UITmp.name = name;
            UITmp.GetComponent<UIWindowBase>().AssetName = name;
            UIWindowBase UIWIndowBase = UITmp.GetComponent<UIWindowBase>();
            EventManager.DispatchEvent(null, UIEvent.OnInit.ToString(), new object[] { });

            UIWIndowBase.windowStatus = UIWindowBase.WindowStatus.Create;

            try
            {
                UIWIndowBase.InitWindow(GetUIID(UIWIndowBase.m_UIType, UIName), objs);
            }
            catch (Exception e)
            {
                LogUtil.Exception(UIName + " OnInit Exception", e);
            }

            AddHideUI(UIWIndowBase);

            if (UIWIndowBase.isShowGlassBlur)
            {
                UIWindowPopupBase popupBase = UIWIndowBase as UIWindowPopupBase;
                if (popupBase != null)
                {
                    if (popupBase.m_isHideMainUIByBlur && GetCurrentDialogCount() == 0)
                    {
                        // if (ScenesManager.Instance.GetCurrentScene() == GameDefine.SceneType.City || ScenesManager.Instance.GetCurrentScene() == GameDefine.SceneType.World)
                        // {
                        //     EventManager.DispatchEvent(null, EventType.Event_Main_UI_Hide);
                        // }
                    }
                }

                s_CamShotUtil.OnCamShot(UIWIndowBase.m_goPost, UIWIndowBase.isNewGlassBlurMat);
            }
            else if (UIWIndowBase.isShowRealTimeBlur)
            {
                if (canvasRootAfter.gameObject.activeSelf)
                {
                    s_CamShotUtil.OnCamRealTimeShot(UIWIndowBase.m_goPost);
                }
            }

            return UIWIndowBase;
        }

        /// <summary>
        /// 打开UI
        /// </summary>
        /// <param name="UIName">UI名</param>
        /// <param name="callback">动画播放完毕回调</param>
        /// <param name="objs">回调传参</param>`
        /// <returns>返回打开的UI</returns>
        public static UIWindowBase OpenUIWindow(string UIName, UIType uiType, UICallBack callback = null, params object[] objs)
        {
            if (uiType == UIType.Dialog || uiType == UIType.Popup)
            {
                //界面变化
                // EventManager.DispatchEvent(null, EventType.Event_View_Open);
            }
            UIWindowBase UIbase = GetHideUI(UIName, uiType);

            if (UIbase == null)
            {
                UIbase = CreateUIWindow(UIName, callback, objs);
            }

            RemoveHideUI(UIbase);
            AddUI(UIbase);

            //栈顶UI
            UIWindowBase UItop = s_UIStackManager.GetLastUI(UIbase.m_UIType);
            if (UItop != null)
            {
                if (UIbase.isInStackHide)
                {
                    UItop.Hide();
                    UItop.OnHide();
                }
            }

            if (UIbase.isInStack)
            {
                UIStackManager.OnUIOpen(UIbase);
            }

            // 打开Dialog时，把所有Popup都隐藏并且记录，待关闭这个Dialog时再还原
            if (UIbase.m_UIType == UIType.Dialog)
            {
                List<UIWindowBase> popupUIList = GetAllUI(UIType.Popup);
                if (popupUIList != null && popupUIList.Count > 0)
                {
                    foreach (var popupUi in popupUIList)
                    {
                        if (popupUi != null && popupUi.gameObject != null && popupUi.gameObject.activeSelf)
                        {
                            popupUi.Hide();
                            if (!s_HidePopupUIListByDialog.ContainsKey(UIbase))
                            {
                                s_HidePopupUIListByDialog.Add(UIbase, new List<UIWindowBase>());
                            }
                            s_HidePopupUIListByDialog[UIbase].Add(popupUi);
                        }
                    }
                }
            }
            else if (UIbase.m_UIType == UIType.Popup)
            {
                if (!IsIgnoreUI(UIbase))
                {
                    // Managers.MusicManager.GetInstance().PlaySoundsEffect(GameDefineString.SoundEffect.SoundEffect_1085);
                }
            }

            if (canvasRootAfter.gameObject.activeSelf)
            {
                //第二相机处于激活状态，默认加载到第二相机
                UILayerManager.SetCurUseCam(1);
                //SceneUI和Hud使用底层相机
                if (UIType.SceneUI == uiType || UIType.Hud == uiType)
                {
                    UILayerManager.SetCurUseCam(0);
                }
            }

            //设置层级
            if (UIbase.isShowGlassBlur)
            {
                ActionSequenceManager.Create()
                    .Then(new WaitForEndOfFrame())
                    .Then(() => UILayerManager.SetLayer(UIbase))
                    .Run();
            }
            else
            {
                UILayerManager.SetLayer(UIbase);
            }

            UIbase.windowStatus = UIWindowBase.WindowStatus.OpenAnim;

            EventManager.DispatchEvent(null, UIEvent.OnOpen.ToString(), new object[] { });

            try
            {
                UIbase.OnOpen();
                UIbase.OnShow();
            }
            catch (Exception e)
            {
                LogUtil.Exception(UIName + " OnOpen Exception", e);
            }

            EventManager.DispatchEvent(null, UIEvent.OnOpened.ToString(), new object[] { });

            UIAnimManager.StartEnterAnim(UIbase, callback, objs); //播放动画
            if (UIType.Dialog == uiType || UIType.Popup == uiType)
            {
                //关闭一个已经打开的窗口
                //Managers.UIManager.CloseUIWindow<SoldierGetTip>();
                // TipManager.Instance.CheckCloseTipAnnouncement();
            }
            return UIbase;
        }
        public static T OpenUIWindow<T>() where T : UIWindowBase
        {
            Type t = typeof(T);
            Type baseT = t.BaseType;
            if (baseT != null)
            {
                UIType _uiType = (UIType)baseT.GetProperty("UIType").GetValue(null, null);
                return (T)OpenUIWindow(t.Name, _uiType);
            }

            return (T)OpenUIWindow(t.Name, UIType.Dialog);
        }
        /// <summary>
        /// 关闭一个已经打开的窗口
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static bool CloseUIWindow<T>() where T : UIWindowBase
        {
            Type t = typeof(T);
            Type baseT = t.BaseType;
            if (null == baseT)
            {
                return false;
            }
            UIType uiType = (UIType)baseT.GetProperty("UIType").GetValue(null, null);
            T ui = Managers.UIManager.GetUI<T>(uiType);
            if (null != ui)
            {
                Managers.UIManager.DestroyUI(ui);
            }
            return null != ui;
        }

        public static T OpenUIWindow<T>(string prefabsName, params object[] objs) where T : UIWindowBase
        {
            Type t = typeof(T);
            Type baseT = t.BaseType;
            if (string.IsNullOrEmpty(prefabsName))
            {
                prefabsName = t.Name;
            }
            if (baseT != null)
            {
                UIType _uiType = (UIType)baseT.GetProperty("UIType").GetValue(null, null);
                return (T)OpenUIWindow(prefabsName, _uiType, null, objs);
            }

            return (T)OpenUIWindow(prefabsName, UIType.Dialog, null, objs);
        }

        /// <summary>
        /// 关闭UI
        /// <param name="UI">目标UI</param>
        /// <param name="isPlayAnim">是否播放关闭动画</param>
        /// <param name="callback">动画播放完毕回调</param>
        /// <param name="objs">回调传参</param>
        public static void CloseUIWindow(UIWindowBase UI, UIType uiType, bool isPlayAnim = true, UICallBack callback = null, params object[] objs)
        {
            RemoveUI(UI);        //移除UI引用
            UI.RemoveAllListener();

            if (isPlayAnim)
            {
                //动画播放完毕删除UI
                if (callback != null)
                {
                    callback += CloseUIWindowCallBack;
                }
                else
                {
                    callback = CloseUIWindowCallBack;
                }
                UI.windowStatus = UIWindowBase.WindowStatus.CloseAnim;
                UIAnimManager.StartExitAnim(UI, callback, objs);
            }
            else
            {
                CloseUIWindowCallBack(UI, objs);
            }
        }
        static void CloseUIWindowCallBack(UIWindowBase UI, params object[] objs)
        {
            UI.windowStatus = UIWindowBase.WindowStatus.Close;
            EventManager.DispatchEvent(null, UIEvent.OnClose.ToString(), new object[] { });

            try
            {
                UI.OnClose();
            }
            catch (Exception e)
            {
                LogUtil.Exception(UI.UIName + " OnClose Exception", e);
            }

            UIStackManager.OnUIClose(UI);
            AddHideUI(UI);

            EventManager.DispatchEvent(null, UIEvent.OnClosed.ToString(), new object[] { });
        }
        public static void CloseUIWindow(string UIname, UIType uiType, bool isPlayAnim = true, UICallBack callback = null, params object[] objs)
        {
            UIWindowBase ui = GetUI(UIname, uiType);

            if (ui == null)
            {
                LogUtil.Error("CloseUIWindow Error UI ->" + UIname + "<-  not Exist!");
            }
            else
            {
                CloseUIWindow(GetUI(UIname, uiType), uiType, isPlayAnim, callback, objs);
            }
        }

        public static void CloseUIWindow<T>(bool isPlayAnim = true, UIType uiType = UIType.Dialog, UICallBack callback = null, params object[] objs) where T : UIWindowBase
        {
            CloseUIWindow(typeof(T).Name, uiType, isPlayAnim, callback, objs);
        }

        public static UIWindowBase ShowUI(string UIname, UIType uiType)
        {
            UIWindowBase ui = GetUI(UIname, uiType);
            return ShowUI(ui);
        }

        public static UIWindowBase ShowUI(UIWindowBase ui)
        {
            ui.windowStatus = UIWindowBase.WindowStatus.Open;
            EventManager.DispatchEvent(null, UIEvent.OnShow.ToString(), new object[] { });

            try
            {
                ui.Show();
                ui.OnShow();
            }
            catch (Exception e)
            {
                LogUtil.Exception(ui.UIName + " OnShow Exception", e);
            }

            return ui;
        }

        public static UIWindowBase HideUI(string UIname, UIType uiType)
        {
            UIWindowBase ui = GetUI(UIname, uiType);
            return HideUI(ui);
        }

        public static UIWindowBase HideUI(UIWindowBase ui)
        {
            ui.windowStatus = UIWindowBase.WindowStatus.Hide;
            EventManager.DispatchEvent(null, UIEvent.OnHide.ToString(), new object[] { });

            try
            {
                ui.Hide();
                ui.OnHide();
            }
            catch (Exception e)
            {
                LogUtil.Exception(ui.UIName + " OnShow Exception", e);
            }

            return ui;
        }
        #endregion
        // private static bool IsDialogTop(UIBase uiBase)
        // {
        //     return uiBase is DialogTop;
        // }

        #region UI内存管理

        public static void DestroyUI(UIWindowBase UI, bool delayUnloadAsset = false)
        {
            //LogUtil.Log("UIManager DestroyUI " + UI.name);
            if (UI == null)
            {
                return;
            }
            if (GetIsExitsHide(UI))
            {
                RemoveHideUI(UI);
            }
            else if (GetIsExits(UI))
            {
                RemoveUI(UI);
            }

            UIStackManager.OnUIClose(UI);
            if (UIType.Dialog == UI.m_UIType)
            {
                HideResourcesTop();
                if (!IsIgnoreUI(UI))
                {
                    // Managers.MusicManager.GetInstance().PlaySoundsEffect(GameDefineString.SoundEffect.Return);
                }

            }
            else if (UIType.Popup == UI.m_UIType)
            {
                if (!IsIgnoreUI(UI))
                {
                    // Managers.MusicManager.GetInstance().PlaySoundsEffect(GameDefineString.SoundEffect.Close);
                }
                UIWindowPopupBase popupBase = UI as UIWindowPopupBase;
                if (popupBase != null)
                {
                    if (popupBase.m_isHideMainUIByBlur && GetCurrentDialogCount() == 0)
                    {
                        // if (ScenesManager.Instance.GetCurrentScene() == GameDefine.SceneType.City || ScenesManager.Instance.GetCurrentScene() == GameDefine.SceneType.World)
                        // {
                        //     EventManager.DispatchEvent(null, EventType.Event_Main_UI_Show);
                        // }
                    }
                }
            }

            if (s_HidePopupUIListByDialog.ContainsKey(UI) && s_HidePopupUIListByDialog[UI].Count > 0)
            {
                var popupUIList = s_HidePopupUIListByDialog[UI];
                foreach (var popupUI in popupUIList)
                {
                    if (popupUI != null && popupUI.gameObject != null && !popupUI.gameObject.activeSelf)
                    {
                        popupUI.Show();
                    }
                }
                popupUIList.Clear();
                s_HidePopupUIListByDialog.Remove(UI);
            }

            //栈顶UI
            UIWindowBase UItop = s_UIStackManager.GetLastUI(UI.m_UIType);
            if (UItop != null)
            {
                if (UItop.isInStackHide)
                {
                    UItop.Show();
                }
                UItop.OnShow();
                //同步Order
                UpdateUIRenderOrder(UI, UItop.m_recordUIRenderOrder);
                ReductionRenderOrder(UItop, null, UItop.m_recordUIRenderOrder);
            }
            else
            {
                UILayerManager.ResetUIOrder(UI);
                if (UIType.Dialog == UI.m_UIType)
                {
                    // DialogTop ui = (DialogTop)GetUI(DialogTop, UIType.Dialog);
                    // if ((ui != null) && (ui.mType == DialogTopType.MainCity))
                    // {
                    //     EventManager.DispatchEvent(null, EventType.Event_Main_UI_Show_Form_Dialog);
                    // }
                    // //Dialog栈中已经没有UI了
                    // ShowResourcesTop(DialogTopType.MainCity);
                }

                if (UI.isShowGlassBlur && UI.isNewGlassBlurMat)
                {
                    if (UI.m_goPost != null)
                    {
                        if (UI.m_goPost.GetComponent<RawImage>() != null)
                        {
                            RenderTexture rt = UI.m_goPost.GetComponent<RawImage>().material.mainTexture as RenderTexture;
                            if (rt != null)
                            {
                                rt.Release();
                                GameObject.Destroy(rt);
                                LogUtil.Log("JS_Blur_Clear UI: {0} GlassBlur RT", UI.name);
                            }
                        }
                    }
                }
            }

            EventManager.DispatchEvent(null, UIEvent.OnDestroy.ToString(), new object[] { });

            try
            {
                UI.Dispose();
            }
            catch (Exception e)
            {
                LogUtil.Exception("OnDestroy Exception", e);
            }
            ReleaseUI(UI, delayUnloadAsset);
            if (UI.m_UIType == UIType.Dialog || UI.m_UIType == UIType.Popup)
            {
                CheckViewChangeToMainUI();
            }

            if (UIType.Dialog == UI.m_UIType || UIType.Popup == UI.m_UIType)
            {
                //尝试弹出跑马灯
                // Managers.TipManager.Instance.ShowTipAnnouncement();
            }
        }

        private static bool IsIgnoreUI(UIWindowBase UI)
        {
            // if (UI is LoadingView
            //     || UI is PopupPlot
            //     || UI is PopupSidePlot
            //     || UI is DialogStoryBoard
            //     || UI is PopupGuideVideo
            //     || UI is DialogChapterAnimationNew
            //     || UI is PopupCommonText
            //     || UI is CommonBoxTip
            //     || UI is PutBuildEditView
            //     )
            // {
            //     return true;
            // }
            return false;
        }
        private static List<string> s_DelayUnloadAssetList = new List<string>();
        // 释放UI相关资源
        private static void UnLoadUIResources(UIBase UI, bool delayUnloadAsset = false)
        {
            if (delayUnloadAsset)
            {
                s_DelayUnloadAssetList.Add(UIPath.GetPath(UI.AssetName));
            }
            else
            {

                AssetManager.Instance.Unload(UIPath.GetPath(UI.AssetName));
            }
        }

        public static void DestroyAllUI()
        {
            DestroyAllActiveUI();
            DestroyAllHideUI();

        }

        public static void DestroyAllUI(UIType uiType)
        {
            if (s_UIs.ContainsKey(uiType))
            {
                DestroyUIs(s_UIs[uiType]);
            }
            if (s_hideUIs.ContainsKey(uiType))
            {
                DestroyUIs(s_hideUIs[uiType]);
            }
        }

        public static void DestroyAllDialogAndPopupUI()
        {
            DestroyAllUI(UIType.Dialog);
            DestroyAllUI(UIType.Popup);
            // if (Managers.ScenesManager.Instance.GetCurrentScene() == GameDefine.SceneType.City || Managers.ScenesManager.Instance.GetCurrentScene() == GameDefine.SceneType.World)
            // {
            //     EventManager.DispatchEvent(null, EventType.Event_Main_UI_Show);
            // }
        }

        private static void DestroyUIs(Dictionary<string, List<UIWindowBase>> allUis)
        {
            foreach (List<UIWindowBase> uis in allUis.Values)
            {
                foreach (var ui in uis)
                {
                    UIStackManager.OnUIClose(ui);

                    if (s_HidePopupUIListByDialog.ContainsKey(ui))
                    {
                        // 批量删除时应该不需要还原（如果以后有需求再根据需求扩展）
                        s_HidePopupUIListByDialog[ui].Clear();
                        s_HidePopupUIListByDialog.Remove(ui);
                    }
                    EventManager.DispatchEvent(null, UIEvent.OnDestroy.ToString(), new object[] { });
                    try
                    {
                        ui.Dispose();
                    }
                    catch (Exception e)
                    {
                        LogUtil.Exception("OnDestroy Exception", e);
                    }
                    ReleaseUI(ui);
                }
                uis.Clear();
            }

            allUis.Clear();
        }

        private static void ReleaseUI(UIBase ui, bool delayUnloadAsset = false)
        {
            if (ui == null)
                return;

            if (ui.gameObject != null)
                GameObject.Destroy(ui.gameObject);

            UnLoadUIResources(ui, delayUnloadAsset);
        }

        #endregion

        #region 打开UI列表的管理

        /// <summary>
        /// 删除所有打开的UI
        /// </summary>
        public static void DestroyAllActiveUI()
        {
            foreach (Dictionary<string, List<UIWindowBase>> typeUIs in s_UIs.Values)
            {
                foreach (List<UIWindowBase> uis in typeUIs.Values)
                {
                    for (int i = 0; i < uis.Count; i++)
                    {
                        if (s_HidePopupUIListByDialog.ContainsKey(uis[i]))
                        {
                            // 批量删除时应该不需要还原（如果以后有需求再根据需求扩展）
                            s_HidePopupUIListByDialog[uis[i]].Clear();
                            s_HidePopupUIListByDialog.Remove(uis[i]);
                        }
                        EventManager.DispatchEvent(null, UIEvent.OnDestroy.ToString(), new object[] { });
                        try
                        {
                            uis[i].Dispose();
                        }
                        catch (Exception e)
                        {
                            LogUtil.Exception("OnDestroy Exception", e);
                        }
                        ReleaseUI(uis[i]);
                    }
                }

                typeUIs.Clear();
            }

            s_UIs.Clear();
        }

        public static T GetUI<T>(UIType uiType) where T : UIWindowBase
        {
            return (T)GetUI(typeof(T).Name, uiType);
        }
        public static void CheckUIReturn(string UIname, UIType uiType, ref bool isUIReturn, ref bool isPUIReturn)
        {
            if (!s_UIs.ContainsKey(uiType))
            {
                isUIReturn = true;
            }
            else if (!s_UIs[uiType].ContainsKey(UIname))
            {
                isUIReturn = true;
            }
            else if (s_UIs[uiType][UIname].Count == 0)
            {
                isUIReturn = true;
            }

            if (!s_PermanentUIs.ContainsKey(uiType))
            {
                isPUIReturn = true;
            }
            else if (!s_PermanentUIs[uiType].ContainsKey(UIname))
            {
                isPUIReturn = true;
            }
            else if (s_PermanentUIs[uiType][UIname].Count == 0)
            {
                isPUIReturn = true;
            }

        }
        public static UIWindowBase GetUI(string UIname, UIType uiType)
        {
            bool isUIReturn = false;
            bool isPUIReturn = false;

            CheckUIReturn(UIname, uiType, ref isUIReturn, ref isPUIReturn);

            if (isUIReturn && isPUIReturn) return null;

            if (!isUIReturn)
            {
                return s_UIs[uiType][UIname][s_UIs[uiType][UIname].Count - 1];
            }
            else if (!isPUIReturn)
            {
                return s_PermanentUIs[uiType][UIname][s_PermanentUIs[uiType][UIname].Count - 1];
            }

            return null;
        }

        public static List<UIWindowBase> GetAllSameNameSceneUI(string UIname)
        {
            if (!s_UIs.ContainsKey(UIType.SceneUI))
            {
                return null;
            }
            if (!s_UIs[UIType.SceneUI].ContainsKey(UIname))
            {
                return null;
            }
            return s_UIs[UIType.SceneUI][UIname];
        }

        public static List<UIWindowBase> GetAllUI(UIType uiType)
        {
            return s_UIStackManager.GetAllUI(uiType);
        }

        static bool GetIsExits(UIWindowBase UI)
        {
            if (!s_UIs.ContainsKey(UI.m_UIType))
            {
                return false;
            }
            if (!s_UIs[UI.m_UIType].ContainsKey(UI.name))
            {
                return false;
            }
            else
            {
                return s_UIs[UI.m_UIType][UI.name].Contains(UI);
            }
        }

        static void AddUI(UIWindowBase UI)
        {
            if (UI.isPermanent)
            {
                if (!s_PermanentUIs.ContainsKey(UI.m_UIType))
                {
                    s_PermanentUIs[UI.m_UIType] = new Dictionary<string, List<UIWindowBase>>();
                }
                if (!s_PermanentUIs[UI.m_UIType].ContainsKey(UI.name))
                {
                    s_PermanentUIs[UI.m_UIType].Add(UI.name, new List<UIWindowBase>());
                }
                s_PermanentUIs[UI.m_UIType][UI.name].Add(UI);
            }
            else
            {
                if (!s_UIs.ContainsKey(UI.m_UIType))
                {
                    s_UIs[UI.m_UIType] = new Dictionary<string, List<UIWindowBase>>();
                }
                if (!s_UIs[UI.m_UIType].ContainsKey(UI.name))
                {
                    s_UIs[UI.m_UIType].Add(UI.name, new List<UIWindowBase>());
                }
                s_UIs[UI.m_UIType][UI.name].Add(UI);
            }

            UI.Show();
        }

        static void RemoveUI(UIWindowBase UI)
        {
            if (UI == null)
            {
                throw new Exception("UIManager: RemoveUI error UI is null: !");
            }

            if (!s_UIs[UI.m_UIType].ContainsKey(UI.name))
            {
                throw new Exception("UIManager: RemoveUI error dont find UI name: ->" + UI.name + "<-  " + UI);
            }

            if (!s_UIs[UI.m_UIType][UI.name].Contains(UI))
            {
                throw new Exception("UIManager: RemoveUI error dont find UI: ->" + UI.name + "<-  " + UI);
            }
            else
            {
                s_UIs[UI.m_UIType][UI.name].Remove(UI);
            }
        }

        static int GetUIID(UIType uiType, string UIname)
        {
            bool isUIReturn = false;
            bool isPUIReturn = false;

            CheckUIReturn(UIname, uiType, ref isUIReturn, ref isPUIReturn);

            if (isUIReturn && isPUIReturn) return 0;

            if (!isUIReturn)
            {
                int id = s_UIs[uiType][UIname].Count;

                for (int i = 0; i < s_UIs[uiType][UIname].Count; i++)
                {
                    if (s_UIs[uiType][UIname][i].UIID == id)
                    {
                        id++;
                        i = 0;
                    }
                }
                return id;
            }
            else if (!isPUIReturn)
            {
                int id = s_PermanentUIs[uiType][UIname].Count;

                for (int i = 0; i < s_PermanentUIs[uiType][UIname].Count; i++)
                {
                    if (s_PermanentUIs[uiType][UIname][i].UIID == id)
                    {
                        id++;
                        i = 0;
                    }
                }
                return id;
            }

            return 0;
        }

        public static int GetNormalUICount()
        {
            //return UIStackManager.m_normalStack.Count;
            return 0;
        }

        public static UIWindowBase GetCurrentUI(UIType uiType)
        {
            return s_UIStackManager.GetLastUI(uiType);
        }

        #endregion

        #region 隐藏UI列表的管理

        /// <summary>
        /// 删除所有隐藏的UI
        /// </summary>
        public static void DestroyAllHideUI()
        {
            foreach (Dictionary<string, List<UIWindowBase>> typeUIs in s_hideUIs.Values)
            {
                foreach (List<UIWindowBase> uis in typeUIs.Values)
                {
                    for (int i = 0; i < uis.Count; i++)
                    {
                        if (s_HidePopupUIListByDialog.ContainsKey(uis[i]))
                        {
                            // 批量删除时应该不需要还原（如果以后有需求再根据需求扩展）
                            s_HidePopupUIListByDialog[uis[i]].Clear();
                            s_HidePopupUIListByDialog.Remove(uis[i]);
                        }
                        EventManager.DispatchEvent(null, UIEvent.OnDestroy.ToString(), new object[] { });
                        try
                        {
                            uis[i].Dispose();
                        }
                        catch (Exception e)
                        {
                            LogUtil.Exception("OnDestroy Exception", e);
                        }
                        ReleaseUI(uis[i]);
                    }
                }

                typeUIs.Clear();
            }

            s_hideUIs.Clear();
        }

        public static T GetHideUI<T>(UIType uiType) where T : UIWindowBase
        {
            string UIname = typeof(T).Name;
            return (T)GetHideUI(UIname, uiType);
        }

        /// <summary>
        /// 获取一个隐藏的UI,如果有多个同名UI，则返回最后创建的那一个
        /// </summary>
        /// <param name="UIname">UI名</param>
        /// <returns></returns>
        public static UIWindowBase GetHideUI(string UIname, UIType uiType)
        {
            if (!s_hideUIs.ContainsKey(uiType))
            {
                return null;
            }
            if (!s_hideUIs[uiType].ContainsKey(UIname))
            {
                return null;
            }
            else
            {
                if (s_hideUIs[uiType][UIname].Count == 0)
                {
                    return null;
                }
                else
                {
                    UIWindowBase ui = s_hideUIs[uiType][UIname][s_hideUIs[uiType][UIname].Count - 1];
                    //默认返回最后创建的那一个
                    return ui;
                }
            }
        }

        static bool GetIsExitsHide(UIWindowBase UI)
        {
            if (!s_hideUIs.ContainsKey(UI.m_UIType))
            {
                return false;
            }
            if (!s_hideUIs[UI.m_UIType].ContainsKey(UI.name))
            {
                return false;
            }
            else
            {
                return s_hideUIs[UI.m_UIType][UI.name].Contains(UI);
            }
        }

        static void AddHideUI(UIWindowBase UI)
        {
            if (!s_hideUIs.ContainsKey(UI.m_UIType))
            {
                s_hideUIs[UI.m_UIType] = new Dictionary<string, List<UIWindowBase>>();
            }
            if (!s_hideUIs[UI.m_UIType].ContainsKey(UI.name))
            {
                s_hideUIs[UI.m_UIType].Add(UI.name, new List<UIWindowBase>());
            }

            s_hideUIs[UI.m_UIType][UI.name].Add(UI);

            UI.Hide();
        }


        static void RemoveHideUI(UIWindowBase UI)
        {
            if (UI == null)
            {
                throw new Exception("UIManager: RemoveUI error l_UI is null: !");
            }

            if (!s_hideUIs[UI.m_UIType].ContainsKey(UI.name))
            {
                throw new Exception("UIManager: RemoveUI error dont find: " + UI.name + "  " + UI);
            }

            if (!s_hideUIs[UI.m_UIType][UI.name].Contains(UI))
            {
                throw new Exception("UIManager: RemoveUI error dont find: " + UI.name + "  " + UI);
            }
            else
            {
                s_hideUIs[UI.m_UIType][UI.name].Remove(UI);
            }
        }

        #endregion

        #region UI渲染层级管理 暂时手动调用等对象池接入后再同一加入 关闭/释放/压栈逻辑
        public static void ReductionRenderOrder(UIWindowBase ui, string cameraKey = null, int order = -1)
        {
            //关闭UI时还原被更改的Order
            if (s_UILayerManager != null)
            {
                s_UILayerManager.ReductionRenderOrder(ui, cameraKey, order);
            }
        }

        public static void UpdateUIRenderOrder(UIWindowBase ui, int order = -1)
        {
            //打开新UI时对Order的同步
            if (s_UILayerManager != null)
            {
                s_UILayerManager.UpdateUIRenderOrder(ui, order);
            }
        }

        #endregion

        public static Vector2 GetPosToOtherUI(RectTransform toTransform, Vector2 pos)
        {
            Vector2 toPos;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(toTransform as RectTransform, pos, uiCamera, out toPos))
            {
                return toPos;
            }
            return Vector2.zero;
        }

        private const string DialogTop = "DialogTop";
        /// <summary>
        /// dialog里的资源UI
        /// </summary>
        /// <param name="dType"></param>
        /// <param name="uiName"></param>
        /// <param name="closeCb"></param>
        // public static void ShowResourcesTop(DialogTopType dType, string uiName = "", UICallBack closeCb = null, params object[] objs)
        // {
        //     if (IsCutSceneing)
        //     {
        //         return;
        //     }
        //
        //     if (ScenesManager.Instance.GetCurrentScene() == GameDefine.SceneType.World &&
        //          Controllers.WorldData.WMapType == WorldMapType.BattleMap)
        //     {
        //         if (Controllers.FactionFight.IsWatchMem())
        //         {
        //             return;
        //         }
        //     }
        //
        //     DialogTop ui = (DialogTop)GetUI(DialogTop, UIType.Dialog);
        //     if (ui != null)
        //     {
        //         ui.CloseCallBack = closeCb;
        //         ui.OnTopShow(dType, uiName, objs);
        //     }
        // }

        public static void HideResourcesTop()
        {
            // DialogTop ui = (DialogTop)GetUI(DialogTop, UIType.Dialog);
            // if (ui != null)
            // {
            //     ui.OnTopHide();
            // }
        }
        #region sceneui
        public static void CreateSceneUI()
        {
            // //倒计时层
            // UIManager.OpenUIWindow<SceneUITimeBar>();
            // //气泡层
            // UIManager.OpenUIWindow<SceneUIBubble>();
            // //飘资源等
            // UIManager.OpenUIWindow<SceneUIFly>();
            // //sceneui最高层级
            // UIManager.OpenUIWindow<SceneUICommon>();
        }

        /*
        public static SceneUITimeBar GetSceneUITimeBar()
        {
            SceneUITimeBar sceneUITimeBar = UIManager.GetUI<SceneUITimeBar>(SceneUITimeBar.UIType);
            if (sceneUITimeBar == null)
            {
                sceneUITimeBar = UIManager.OpenUIWindow<SceneUITimeBar>();
            }
            return sceneUITimeBar;
        }

        public static GameObject GetSceneUITimeBarRoot()
        {
            SceneUITimeBar sceneUITimeBar = GetSceneUITimeBar();
            return sceneUITimeBar.GetRootGameObj();
        }
        #region 气泡
        public static SceneUIBubble GetSceneUIBubble()
        {
            SceneUIBubble sceneUIBubble = UIManager.GetUI<SceneUIBubble>(SceneUIBubble.UIType);
            if (sceneUIBubble == null)
            {
                sceneUIBubble = UIManager.OpenUIWindow<SceneUIBubble>();
            }
            return sceneUIBubble;
        }

        public static GameObject GetSceneUIBubbleRoot()
        {
            SceneUIBubble sceneUIBubble = GetSceneUIBubble();
            return sceneUIBubble.GetRootGameObj();
        }
        #endregion

        #region 飘动画
        public static SceneUIFly GetSceneUIFly()
        {
            SceneUIFly sceneUIBubble = UIManager.GetUI<SceneUIFly>(SceneUIFly.UIType);
            if (sceneUIBubble == null)
            {
                sceneUIBubble = UIManager.OpenUIWindow<SceneUIFly>();
            }
            return sceneUIBubble;
        }

        public static GameObject GetSceneUIFlyRoot()
        {
            SceneUIFly sceneUIFly = GetSceneUIFly();
            return sceneUIFly.GetRootGameObj();
        }
        #endregion

        #region 最高层
        public static SceneUICommon GetSceneUICommon()
        {
            SceneUICommon sceneUICommon = UIManager.GetUI<SceneUICommon>(SceneUICommon.UIType);
            if (sceneUICommon == null)
            {
                sceneUICommon = UIManager.OpenUIWindow<SceneUICommon>();
            }
            return sceneUICommon;
        }

        public static GameObject GetSceneUICommonRoot()
        {
            SceneUICommon sceneUICommon = GetSceneUICommon();
            return sceneUICommon.GetRootGameObj();
        }
        #endregion
        #endregion
        


        public static TipUICommon GetTipUICommon()
        {
            TipUICommon tipUICommon = UIManager.GetUI<TipUICommon>(TipUICommon.UIType);
            if (tipUICommon == null)
            {
                tipUICommon = UIManager.OpenUIWindow<TipUICommon>();
            }
            return tipUICommon;
        }

        public static GameObject GetTipUICommonRoot()
        {
            TipUICommon tipUICommon = UIManager.GetUI<TipUICommon>(TipUICommon.UIType);
            if (tipUICommon == null)
            {
                tipUICommon = UIManager.OpenUIWindow<TipUICommon>();
            }
            return tipUICommon.GetRootGameObj();
        }
        
               
        /// <summary>
        /// 点击特效
        /// </summary>
        public static void CreateClickEffect()
        {
            ClickEffect clickEffect = UIManager.GetUI<ClickEffect>(UIType.Blocker);
            if (clickEffect == null)
            {
                UIManager.OpenUIWindow<ClickEffect>();
            }
        }
        
         */
        /// <summary>
        /// 获取当前打开的dialog,popup界面总和
        /// </summary>
        /// <returns></returns>
        public static int GetCurDialogAndPopupUICount()
        {
            return UIStackManager.GetAllDialogUICount() + UIStackManager.GetAllPopupUICount();
        }

        public static int GetCurrentDialogCount()
        {
            return UIStackManager.GetAllDialogUICount();
        }

        /// <summary>
        /// 检测一级一级退出时是否退到了主城(不包括一下删除所有界面，那种操作一般都会紧跟下一步操作)
        /// </summary>
        private static void CheckViewChangeToMainUI()
        {
            if (GetCurDialogAndPopupUICount() <= 0)
            {
                // EventManager.DispatchEvent(null, EventType.Event_View_Change_Main);
            }
        }

        public static bool CheckGuiRaycastGameObject(GameObject gameObject)
        {
            if (null == EventSystem.current || null == gameObject)
            {
                return false;
            }

            PointerEventData eventData = new PointerEventData(EventSystem.current);
#if UNITY_EDITOR
            eventData.pressPosition = Input.mousePosition;
            eventData.position = Input.mousePosition;
#endif
#if UNITY_ANDROID || UNITY_IPHONE
            if (Input.touchCount > 0)
            {
                eventData.pressPosition = Input.GetTouch(0).position;
                eventData.position = Input.GetTouch(0).position;
            }
#endif
            List<RaycastResult> list = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, list);
            for (int i = 0; i < list.Count; ++i)
            {
                if (gameObject == list[i].gameObject)
                {
                    return true;
                }
            }
            return false;
        }


        public static bool CheckGuiRaycastObjects()
        {
            if (EventSystem.current == null)
            {
                return false;
            }

            PointerEventData eventData = new PointerEventData(EventSystem.current);
#if UNITY_EDITOR
            eventData.pressPosition = Input.mousePosition;
            eventData.position = Input.mousePosition;
#endif
#if UNITY_ANDROID || UNITY_IPHONE
            if (Input.touchCount > 0)
            {
                eventData.pressPosition = Input.GetTouch(0).position;
                eventData.position = Input.GetTouch(0).position;
            }
#endif
            List<RaycastResult> list = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, list);
            return list.Count > 0;
        }

        public static bool CheckGuiRaycastObjects(Vector2 mousePos)
        {
            if (EventSystem.current == null)
            {
                return false;
            }

            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.pressPosition = mousePos;
            eventData.position = mousePos;
            List<RaycastResult> list = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, list);

            return list.Count > 0;
        }

        public static bool CheckGuiRaycastObjectsByTag(Vector2 mousePos, string tag)
        {
            if (EventSystem.current == null)
            {
                return false;
            }

            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.pressPosition = mousePos;
            eventData.position = mousePos;
            List<RaycastResult> list = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, list);

            for (int i = 0; i < list.Count; i++)
            {
                GameObject retGo = list[i].gameObject;
                if (retGo == null) continue;
                if (retGo.tag.Equals(tag))
                {
                    return true;
                }
            }

            return false;
        }

        public static void SetMainUI(bool bState)
        {
            // GameMainUI mainUI = GetUI<GameMainUI>(UIType.Hud);
            // if (mainUI != null)
            // {
            //     mainUI.gameObject.SetActive(bState);
            //     if (bState)
            //     {
            //         ShowResourcesTop(DialogTopType.MainCity);
            //     }
            //     else
            //     {
            //         HideResourcesTop();
            //     }
            // }
        }

        #endregion

        #region UI统计&日志

        private GameObject _lastLoggedObject;
        private GameObject _tempGO;
        public override void OnLateUpdate()
        {
            base.OnLateUpdate();
            InputLog();

        }

        private void InputLog()
        {
            if (EventSystem.current == null)
                return;
            if (EventSystem.current.currentSelectedGameObject != null)
                _tempGO = EventSystem.current.currentSelectedGameObject;
            else if (EventSystem.current.firstSelectedGameObject != null)
                _tempGO = EventSystem.current.firstSelectedGameObject;
            if (_tempGO != null && _tempGO != _lastLoggedObject)
            {
                _lastLoggedObject = _tempGO;
                LogUtil.LogKeyValue(LogUtil.INPUTGO, _lastLoggedObject.name);
            }
        }

        #endregion
    }

}

#region UI事件 代理 枚举

/// <summary>
/// UI回调
/// </summary>
/// <param name="objs"></param>
public delegate void UICallBack(UIWindowBase UI, params object[] objs);
public delegate void UIAnimCallBack(UIWindowBase UIbase, UICallBack callBack, params object[] objs);

public enum UIType
{
    SceneUI = 0,

    Hud = 1,
    Dialog = 2,
    Popup = 3,
    Tip = 4,
    Blocker = 5,
}

public enum UIModeType
{
    Common = 0,
    MainCity = 1,
    Bag = 2,
    Hero = 3
}

public enum UIEffectOrder
{
    Order_SceneUI = 0,

    Order_Hud = 1000,
    Order_Dialog = 2000,
    Order_Popup = 3000,
    Order_Tip = 4000,
    Order_Blocker = 5000
}

public enum UIEvent
{
    OnOpen,
    OnOpened,

    OnClose,
    OnClosed,

    OnHide,
    OnShow,

    OnInit,
    OnDestroy,

    OnRefresh,

    OnStartEnterAnim,
    OnCompleteEnterAnim,

    OnStartExitAnim,
    OnCompleteExitAnim,
}
#endregion