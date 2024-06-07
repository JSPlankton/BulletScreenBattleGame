using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWindowBase : UIBase
{
    public int m_recordUIRenderOrder;   //记录当前UI创建时的OrderId，关闭UI时将Order值同步回去
    [HideInInspector]
    public string cameraKey;
    public UIType m_UIType;
    [HideInInspector]
    public bool isInStack = true;  //是否入栈
    [HideInInspector]
    public bool isInStackHide = true;  //被压栈后是否隐藏
    [HideInInspector]
    public bool isPermanent = false;    //是否常驻内存，不收统一删除影响，需要主动关闭
    [Tooltip("打开此选项,UI实现背景当前帧模糊,需配合固定节点使用")]
    public bool isShowGlassBlur = false;
    [Tooltip("打开此选项,UI实现背景实时模糊,只有在多相机模式下才会生效")]
    public bool isShowRealTimeBlur = false;
    [Tooltip("打开此选项,UI实现背景当前帧模糊重新创建材质球和RT，与UI绑定。不用RenderRes下共用的Mat")]
    public bool isNewGlassBlurMat = false;
    [Tooltip("打开此选项,UI实现背景遮罩")]
    public bool isShowMask = false;
    [Tooltip("如果Dialog层关闭此选项，代表为非全屏遮挡的Window")]
    public bool isFullScreen = true;
    [HideInInspector]
    [Tooltip("打开此选项,UI非激活清空下在OnHide清除监听事件,OnShow恢复监听事件")]
    public bool isListerUnActive = false;
    [HideInInspector]
    [Tooltip("压栈后是否active script")]
    public bool isActiveScriptInStack = true;

    [HideInInspector]
    public WindowStatus windowStatus;

    public GameObject m_bgMask;
    public GameObject m_uiRoot;
    public GameObject m_goPost;

    public float m_PosZ; //Z轴偏移

    #region 重载方法

    public virtual void OnOpen()
    {

    }

    public virtual void OnClose()
    {

    }

    public virtual void OnHide()
    {
        if (isListerUnActive)
        {
            CopyUIEvent();//临时存储UI事件监听
            RemoveAllOnEventListenr();//移除当前UI事件监听
        }
    }

    public virtual void OnShow()
    {
        if (isListerUnActive)
        {
            ResumeAllOnEventListenr();       //恢复当前UI的所有事件监听 
        }
    }

    public virtual void OnRefresh()
    {

    }

    public virtual IEnumerator EnterAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        //默认无动画
        animComplete(this, callBack, objs);

        yield break;
    }

    public virtual void OnCompleteEnterAnim()
    {
    }

    public virtual IEnumerator ExitAnim(UIAnimCallBack animComplete, UICallBack callBack, params object[] objs)
    {
        //默认无动画
        animComplete(this, callBack, objs);

        yield break;
    }

    public virtual void OnCompleteExitAnim()
    {
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
        if (!isActiveScriptInStack)
        {
            this.enabled = true;
        }
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
        if (!isActiveScriptInStack)
        {
            this.enabled = false;
        }
    }

    #endregion

    #region 继承方法
    public override void OnBeforeInit()
    {

    }

    public void InitWindow(int id, params object[] objs)
    {
        List<UILifeCycleInterface> list = new List<UILifeCycleInterface>();
        Init(null, id, objs);
        RecursionInitUI(null, this, id, list);
    }

    /// <summary>
    /// 递归初始化UI
    /// </summary>
    /// <param name="uiBase"></param>
    public void RecursionInitUI(UIBase parentUI, UIBase uiBase, int id, List<UILifeCycleInterface> UIList)
    {
        int childIndex = 0;
        for (int i = 0; i < uiBase.m_objectList.Count; i++)
        {
            GameObject go = uiBase.m_objectList[i];

            if (go != null)
            {
                UILifeCycleInterface tmp = go.GetComponent<UILifeCycleInterface>();

                if (tmp != null)
                {
                    if (!UIList.Contains(tmp))
                    {
                        uiBase.AddLifeCycleComponent(tmp);

                        UIList.Add(tmp);

                        UIBase subUI = uiBase.m_objectList[i].GetComponent<UIBase>();
                        if (subUI != null)
                        {
                            RecursionInitUI(uiBase, subUI, childIndex++, UIList);
                        }
                    }
                    else
                    {
                        LogUtil.Error("InitWindow 重复的引用 " + uiBase.UIEventKey + " " + uiBase.m_objectList[i].name);
                    }

                }
            }
            else
            {
                LogUtil.Error("InitWindow objectList[" + i + "] is null !: " + uiBase.UIEventKey);
            }
        }
    }

    /// <summary>
    /// 调用当前UIBase下的所有子UIBase的Dispose方法
    /// </summary>
    public void RecursionUIDispose(UIBase uiBase)
    {
        if (uiBase.m_objectList.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < uiBase.m_objectList.Count; i++)
        {
            GameObject go = uiBase.m_objectList[i];

            if (go != null)
            {
                UIBase tmp = go.GetComponent<UIBase>();

                if (tmp != null)
                {
                    LogUtil.Log("Dispose UI {0} ", tmp.UIName);
                    tmp.Dispose();
                    if (tmp.m_objectList.Count > 0)
                    {
                        RecursionUIDispose(tmp);
                    }
                }
            }
            else
            {
                LogUtil.Warning("Dispose UI objectList[" + i + "] is null !: " + uiBase.UIEventKey);
            }
        }
    }

    //刷新是主动调用
    public void Refresh(params object[] args)
    {
        EventManager.DispatchEvent(null, UIEvent.OnRefresh.ToString(), args);
        OnRefresh();
    }

    /* GlobalEvent 使用
    public void AddEventListener(Enum l_Event)
    {
        if (!m_EventNames.Contains(l_Event))
        {
            m_EventNames.Add(l_Event);
            GlobalEvent.AddEvent(l_Event, Refresh);
        }
    }
    */

    public void AddEventListener(String eventName)
    {
        if (!m_EventNames.Contains(eventName))
        {
            m_EventNames.Add(eventName);
            EventManager.AddEvent(null, eventName, Refresh);
        }
    }

    #region 引导

    protected string m_guideChangeKey = "";

    public virtual Transform GetGuideTransform(params string[] objs)
    {
        return null;
    }

    protected void SetGuideChangeKey(params string[] objs)
    {
        if (null == objs)
        {
            return;
        }
        m_guideChangeKey = string.Join("_", objs);
    }

    protected void OnEventGuideChange()
    {
        if (string.IsNullOrEmpty(m_guideChangeKey))
        {
            return;
        }
        // EventManager.DispatchEvent(null, EventType.Event_Guide_Change, new object[] { m_guideChangeKey });
        m_guideChangeKey = "";
    }

    #endregion

    #endregion

    public enum WindowStatus
    {
        Create,
        Open,
        Close,
        OpenAnim,
        CloseAnim,
        Hide,
    }
}