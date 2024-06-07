#if UNITY_5_5_OR_NEWER
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering.Universal;

public static class UIModelShowTool
{
    /// <summary>
    /// 每个UImodelCamera的间隔
    /// </summary>
    static Vector3 s_ShowSpace = new Vector3(20, 0, 0);

    static string s_defaultLayer = "UI";
    const bool c_defaultOrthographic = true;
    const float c_defaultOrthographicSize = 0.72f;
    const float c_defaultFOV = 60;
    static Color s_defaultBackgroundColor = new Color(0, 0, 0, 0f);
    static Vector3 s_StartPosition = new Vector3(1000f, 10f, 0);
    static Vector3 s_defaultLocationPosition = new Vector3(0, 0, 10);
    static Vector3 s_defaultEulerAngles = new Vector3(0, 0, 0);
    static Vector3 s_defaultLocalScale = Vector3.one;
    static Vector3 s_defaultTexSize = new Vector3(640, 1136, 100);
    static Vector2 s_clippingPlanes = new Vector2(0.2f, 40);

    static List<UIModelShowData> modelShowList = new List<UIModelShowData>();

    static void ResetModelShowPosition()
    {
        for (int i = 0; i < modelShowList.Count; i++)
        {
            if (modelShowList[i] != null)
            {
                if (modelShowList[i].top != null)
                {
                    modelShowList[i].top.transform.position = s_StartPosition + i * s_ShowSpace;
                }
            }
        }
    }


    public static void DisposeModelShow(UIModelShowData data)
    {
        data.Dispose();
        modelShowList.Remove(data);
        ResetModelShowPosition();
    }

    public static UIModelShowData CreateModelData(string prefabName,
        string layerName = null,
        bool? orthographic = null,
        float? orthographicSize = null,
        bool? camAntialiasing = null,
        bool? camPostProcessing = null,
        Color? backgroundColor = null,
        Vector3? localPosition = null,
        Vector3? eulerAngles = null,
        Vector3? localScale = null,
        Vector3? texSize = null,
        float? nearClippingPlane = null,
        float? farClippingPlane = null,
        Vector3? cameraLocalPosition = null,
        Vector3? cameraEulerAngles = null,
        float? orthograthicSize = null,
        CallBack<UIModelShowTool.UIModelShowData> loadfinishFunc = null,
        bool? castShadow = null,
        bool? isAsyncLoad = null)
    {

        //默认值设置
        layerName = layerName ?? s_defaultLayer;
        Vector3 localPositionTmp = localPosition ?? s_defaultLocationPosition;
        Vector3 eulerAnglesTmp = eulerAngles ?? s_defaultEulerAngles;
        Vector3 texSizeTmp = texSize ?? s_defaultTexSize;
        Vector3 localScaleTmp = localScale ?? s_defaultLocalScale;
        Color backgroundColorTmp = backgroundColor ?? s_defaultBackgroundColor;
        float orthographicSizeTmp = orthographicSize ?? c_defaultOrthographicSize;
        bool orthographicTmp = orthographic ?? c_defaultOrthographic;
        float fieldOfView = orthographicSize ?? c_defaultFOV;
        float nearClippingPlaneTmp = nearClippingPlane ?? s_clippingPlanes.x;
        float farClippingPlaneTmp = farClippingPlane ?? s_clippingPlanes.y;
        float orthograthicSizeTmp = orthograthicSize ?? 16f;
        bool castShadowTmp = castShadow ?? false;
        bool camAntialiasingTmp = camAntialiasing ?? false;
        bool camPostProcessingTmp = camPostProcessing ?? true;
        bool asyncLoadTmp = isAsyncLoad ?? false;

        Vector3 cameraLocalPositionTmp = cameraLocalPosition ?? Vector3.zero;
        Vector3 cameraEulerAnglesTmp = cameraEulerAngles ?? Vector3.zero;

        //构造Camera
        UIModelShowData data = new UIModelShowData();
        GameObject uiModelShow = new GameObject("UIShowModelCamera");
        data.top = uiModelShow;

        GameObject camera = new GameObject("ModelCamera");

        camera.transform.SetParent(uiModelShow.transform);
        camera.transform.localPosition = Vector3.zero;
        Camera ca = camera.AddComponent<Camera>();
        data.camera = ca;

        ca.clearFlags = CameraClearFlags.SolidColor;
        ca.backgroundColor = backgroundColorTmp;
        ca.orthographic = orthographicTmp;
        ca.orthographicSize = orthographicSizeTmp;
        ca.fieldOfView = fieldOfView;
        ca.depth = 0;
        ca.nearClipPlane = nearClippingPlaneTmp;
        ca.farClipPlane = farClippingPlaneTmp;
        ca.cullingMask = 1 << LayerMask.NameToLayer(layerName);
        ca.tag = "ModelCamera";
        ca.transform.localEulerAngles = cameraEulerAnglesTmp;
        ca.transform.localPosition = cameraLocalPositionTmp;
        ca.orthographicSize = orthograthicSizeTmp;
        ca.enabled = false;

        var uac = ca.GetComponent<UniversalAdditionalCameraData>();
        if (uac == null)
        {
            uac = ca.gameObject.AddComponent<UniversalAdditionalCameraData>();
        }

        uac.renderPostProcessing = camPostProcessingTmp;
        uac.volumeLayerMask = 1 << LayerMask.NameToLayer(layerName);
        // uac.gameCamType = UniversalAdditionalCameraData.GameCamType.UI3D;

        if (camAntialiasingTmp)
        {
            uac.antialiasing = AntialiasingMode.SubpixelMorphologicalAntiAliasing;
        }

        GameObject root = new GameObject("Root");
        data.root = root;

        root.transform.SetParent(uiModelShow.transform);
        root.transform.localPosition = localPositionTmp;
        root.transform.eulerAngles = eulerAnglesTmp;
        root.transform.localScale = localScaleTmp;
        data.prefabName = prefabName;

        if (asyncLoadTmp)
        {
            //异步创建模型模型
            AssetManager.Instance.LoadAsync(prefabName, typeof(GameObject), (tmp, resLoadState) =>
            {
                if (tmp == null)
                {
                    LogUtil.Error("UIModelTool LoadAsync tmp Is Null");
                    return;
                }
                GameObject obj = UnityEngine.Object.Instantiate<GameObject>((GameObject)tmp);
                if (data == null)
                {
                    LogUtil.Error("UIModelTool LoadAsync Data Is Null");
                    return;
                }
                if (obj == null)
                {
                    LogUtil.Error("UIModelTool LoadAsync obj Inst Is Null");
                    return;
                }
                data.model = obj;
                if (root == null)
                {
                    LogUtil.Error("UIModelTool LoadAsync root Is Null");
                    return;
                }
                obj.transform.SetParent(root.transform);
                obj.transform.localPosition = new Vector3(0, 0, 0);
                obj.transform.localEulerAngles = Vector3.zero;
                obj.transform.localScale = Vector3.one;

                obj.SetLayer(LayerMask.NameToLayer(layerName));

                if (!castShadowTmp)
                {
                    SkinnedMeshRenderer[] mes = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
                    for (int i = 0; i < mes.Length; i++)
                    {
                        //mes[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                        //mes[i].receiveShadows = false;
                    }
                }

                //设置randerTexture
                RenderTexture tex = new RenderTexture((int)texSizeTmp.x, (int)texSizeTmp.y, (int)texSizeTmp.z);
                data.renderTexture = tex;

                tex.autoGenerateMips = false;
                tex.anisoLevel = 1;

                ca.targetTexture = tex;
                ca.enabled = true;
                modelShowList.Add(data);
                ResetModelShowPosition();

                if (loadfinishFunc != null && data.model != null && data.renderTexture != null)
                {
                    loadfinishFunc.Invoke(data);
                }
            });
        }
        else
        {
            //取消异步加载
            UnityEngine.Object tmp = AssetManager.Instance.Load(prefabName, typeof(GameObject));
            if (tmp == null)
            {
                LogUtil.Error("UIModelTool LoadAsync tmp Is Null");
                return data;
            }

            GameObject obj = UnityEngine.Object.Instantiate<GameObject>((GameObject)tmp);
            if (data == null)
            {
                LogUtil.Error("UIModelTool LoadAsync Data Is Null");
                return data;
            }
            if (obj == null)
            {
                LogUtil.Error("UIModelTool LoadAsync obj Inst Is Null");
                return data;
            }
            data.model = obj;
            if (root == null)
            {
                LogUtil.Error("UIModelTool LoadAsync root Is Null");
                return data;
            }
            obj.transform.SetParent(root.transform);
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.localEulerAngles = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            obj.SetLayer(LayerMask.NameToLayer(layerName));

            if (!castShadowTmp)
            {
                SkinnedMeshRenderer[] mes = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
                for (int i = 0; i < mes.Length; i++)
                {
                    //mes[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    //mes[i].receiveShadows = false;
                }
            }

            //设置randerTexture
            RenderTexture tex = new RenderTexture((int)texSizeTmp.x, (int)texSizeTmp.y, (int)texSizeTmp.z);
            data.renderTexture = tex;

            tex.autoGenerateMips = false;
            tex.anisoLevel = 1;

            ca.targetTexture = tex;
            ca.enabled = true;
            modelShowList.Add(data);
            ResetModelShowPosition();

            if (loadfinishFunc != null && data.model != null && data.renderTexture != null)
            {
                loadfinishFunc.Invoke(data);
            }
        }

        return data;
    }


    public static GameObject Create(string prefabName, out RenderTexture tex)
    {
        GameObject temp0 = new GameObject("UIModelShow");
        GameObject temp1 = new GameObject("Camera");
        temp1.transform.SetParent(temp0.transform);
        temp1.transform.localPosition = new Vector3(0, 5000, 0);
        Camera ca = temp1.AddComponent<Camera>();
        ca.clearFlags = CameraClearFlags.SolidColor;
        ca.backgroundColor = new Color(0, 0, 0, 5 / 255f);
        ca.orthographic = true;
        ca.orthographicSize = 0.72f;
        ca.depth = 100;
        ca.cullingMask = 1 << LayerMask.NameToLayer("UI");

        GameObject root = new GameObject("Root");
        root.transform.SetParent(temp1.transform);
        root.transform.localPosition = new Vector3(0, 0, 100);
        root.transform.eulerAngles = new Vector3(0, 180, 0);

        //GameObject obj = AssetManager.Instance.Load(prefabName) as GameObject;
        UnityEngine.Object tmp = AssetManager.Instance.Load(prefabName, typeof(GameObject));
        GameObject obj = UnityEngine.Object.Instantiate<GameObject>((GameObject)tmp);
        obj.transform.SetParent(root.transform);
        obj.transform.localPosition = new Vector3(0, 0, 0);
        obj.transform.localEulerAngles = Vector3.zero;

        Transform[] trans = obj.GetComponentsInChildren<Transform>();
        for (int i = 0; i < trans.Length; i++)
        {
            trans[i].gameObject.layer = LayerMask.NameToLayer("UI");
        }

        SkinnedMeshRenderer[] mes = obj.GetComponentsInChildren<SkinnedMeshRenderer>();
        for (int i = 0; i < mes.Length; i++)
        {
            mes[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            mes[i].receiveShadows = false;
        }

        tex = new RenderTexture(512, 512, 100);
        tex.autoGenerateMips = false;
        tex.anisoLevel = 1;

        //  tex.antiAliasing = 2

        ca.targetTexture = tex;
        return obj;
    }

    private static EventTrigger.Entry GetEvent(EventTriggerType type, UnityAction<BaseEventData> eventFun)
    {
        UnityAction<BaseEventData> eventDrag = new UnityAction<BaseEventData>(eventFun);
        EventTrigger.Entry myclick = new EventTrigger.Entry();
        myclick.eventID = EventTriggerType.Drag;
        myclick.callback.AddListener(eventDrag);
        return myclick;
    }

    public class UIModelShowData
    {
        public GameObject top;
        public GameObject root;
        public GameObject model;
        public Camera camera;
        public RenderTexture renderTexture;
        public string prefabName;

        public void Dispose()
        {
            //销毁 or 缓存
            //GameObjectManager.DestroyGameObject(model);
            if (model != null)
            {
                GameObject.Destroy(model);
            }
            if (top != null)
            {
                GameObject.Destroy(top);
            }
            if (renderTexture != null)
            {
                GameObject.Destroy(renderTexture);
                renderTexture = null;
            }
            AssetManager.Instance.Unload(prefabName);
        }

        public void ChangeModel(string modelName, CallBack<UIModelShowTool.UIModelShowData> ChangeModelFinishCb = null)
        {
            int layer = LayerMask.NameToLayer("ModelLayer");
            if (model != null)
            {
                layer = model.layer;
            }

            GameObject.Destroy(model);

            //异步创建模型模型
            //AssetManager.Instance.LoadAsync(modelName, typeof(GameObject), (tmp, resLoadState) =>
            //{
            //    if (tmp == null)
            //    {
            //        LogUtil.Log("UIModelTool LoadAsync tmp Is Null");
            //        return;
            //    }
            //    model = UnityEngine.Object.Instantiate<GameObject>((GameObject)tmp);
            //    if (model == null)
            //    {
            //        LogUtil.Error("UIModelTool LoadAsync obj Inst Is Null");
            //        return;
            //    }
            //    model.transform.SetParent(root.transform);
            //    model.transform.localPosition = new Vector3(0, 0, 0);
            //    model.transform.localEulerAngles = Vector3.zero;
            //    model.transform.localScale = Vector3.one;

            //    model.SetLayer(layer);

            //    ChangeModelFinishCb?.Invoke(this);
            //});

            //取消异步加载
            UnityEngine.Object tmp = AssetManager.Instance.Load(modelName, typeof(GameObject));
            if (tmp == null)
            {
                LogUtil.Log("UIModelTool LoadAsync tmp Is Null");
                return;
            }
            model = UnityEngine.Object.Instantiate<GameObject>((GameObject)tmp);
            if (model == null)
            {
                LogUtil.Error("UIModelTool LoadAsync obj Inst Is Null");
                return;
            }
            model.transform.SetParent(root.transform);
            model.transform.localPosition = new Vector3(0, 0, 0);
            model.transform.localEulerAngles = Vector3.zero;
            model.transform.localScale = Vector3.one;

            model.SetLayer(layer);

            ChangeModelFinishCb?.Invoke(this);
        }
    }
}
#endif