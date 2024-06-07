using UnityEngine;

public class UIFollowBase : UIBase
{
    public Vector2 offset = Vector2.zero;
    public bool useTapEndListener;
    protected RectTransform rect;
    protected Transform guideTransform;
    protected Vector2 actualRect;
    protected Vector2 halfRect;

    private CallBack clickCallBack;
    private Transform checkTransform;
    private Transform scaleTransform;
    public virtual void AddEvents()
    {
        // AddOnEventListenr(null, EventType.Event_Camera_Scale_By_Finger, OnCameraScale);
        //
        // AddOnEventListenr(null, EventType.Event_Construction_Can_Tap, OnCanTap);
        //
        // if (useTapEndListener)
        //     AddOnEventListenr(null, EventType.Event_Construction_Tap_End, OnTapEnd);
    }

    private void OnCanTap(object[] args)
    {
        if (args.Length > 0)
            this.checkTransform = args[0] as Transform;
        else
            this.checkTransform = null;
    }

    private void OnCameraScale(object[] args)
    {
        RefreshPosition();
    }

    protected void SetFollowParams(RectTransform rect, Transform guideTransform, Transform scaleTransform, CallBack clickCallBack)
    {
        this.rect = rect;
        this.guideTransform = guideTransform;
        this.scaleTransform = scaleTransform;
        this.actualRect = ScreenUtil.GetActualRect(this.rect);
        this.halfRect = actualRect / 2;
        this.clickCallBack = clickCallBack;
    }

    private void OnTapEnd(object[] args)
    {
        if (args.Length < 2 || this.rect == null)
            return;
        // if (Global.inPlayingTimeline || this.gameObject == null || (!this.gameObject.activeInHierarchy))
        //     return;
        //
        // if (Global.isEnforceGuide)
        // {
        //     if (this.checkTransform != null && this.checkTransform == this.guideTransform)
        //         this.clickCallBack();
        //     return;
        // }

        Vector2 clickPosition = new Vector2((float)args[0], (float)args[1]);
        Vector2 localPoint;
        //将选中的点转换为Image区域内的本地点
        RectTransform scaleRect = this.scaleTransform.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(scaleRect, clickPosition, ScreenUtil.GetUICamera(), out localPoint);

        Vector2 pivot = scaleRect.pivot;
        Vector2 normalizedLocal = new Vector2(pivot.x + localPoint.x / scaleRect.sizeDelta.x, pivot.y + localPoint.y / scaleRect.sizeDelta.y);
        if (Managers.UIManager.GetCurDialogAndPopupUICount() == 0 &&
            (normalizedLocal.x >= 0 && normalizedLocal.x <= 1) && ((normalizedLocal.y >= 0 && normalizedLocal.y <= 1)))
        {
            this.clickCallBack();
        }
    }
    public virtual void RefreshPosition()
    {

    }
    public override void OnInit()
    {
        base.OnInit();
        if (InitObject.Length > 0 && InitObject[0] != null)
        {
            GameObject followObj = InitObject[0] as GameObject;
            if (followObj != null)
                ResetFollowObj(followObj);

        }
        AddEvents();
    }
    /// <summary>
    /// 获取跟随组件
    /// </summary>
    /// <returns></returns>
    private SceneUIComponent GetFollowCompent()
    {
        SceneUIComponent scenUIComponet = this.gameObject.GetComponent<SceneUIComponent>();
        if (scenUIComponet == null)
            scenUIComponet = this.gameObject.AddComponent<SceneUIComponent>();
        return scenUIComponet;
    }
    /// <summary>
    /// 重新设置跟随gameobject
    /// </summary>
    /// <param name="followObj"></param>
    public void ResetFollowObj(GameObject followObj)
    {
        SceneUIComponent scenUIComponet = GetFollowCompent();
        if (scenUIComponet != null && followObj != null)
            scenUIComponet.SetFollowObject(followObj, offset);

    }
    /// <summary>
    /// 设置跟随坐标
    /// </summary>
    /// <param name="worldPos"></param>
    public void SetFollowWorldPos(Vector3 worldPos)
    {
        SceneUIComponent scenUIComponet = GetFollowCompent();
        if (scenUIComponet != null)
            scenUIComponet.SetFollowWorldPos(worldPos);
    }
    /// <summary>
    /// 设置偏移量
    /// </summary>
    /// <param name="offset"></param>
    public void ChangeUIOffset(Vector2 offset)
    {
        this.offset = offset;
        SceneUIComponent scenUIComponet = GetFollowCompent();
        if (scenUIComponet != null)
            scenUIComponet.ChangePos(offset);

    }
    #region 滑动
    public bool isInDrag = false;
    private string btnName = "";
    public void AddButtonDragListener(string btnName)
    {
        this.btnName = btnName;
        //AddOnDragListener(btnName, OnDrag);
        //AddBeginDragListener(btnName, OnBeginDrag);
        //AddEndDragListener(btnName, OnEndDrag);
    }
    private void OnBeginDrag(InputUIOnBeginDragEvent inputEvent)
    {
        if (inputEvent.m_pointerDrag.Equals(GetGameObject(btnName)))
        {
            isInDrag = true;
            // EventManager.DispatchEvent(null, EventType.Event_Camera_Begin, new object[] { inputEvent.m_dragPosition, inputEvent.m_delta });
        }

    }

    private void OnDrag(InputUIOnDragEvent inputEvent)
    {
        if (!isInDrag) return;
        // EventManager.DispatchEvent(null, EventType.Event_Camera_Move, new object[] { inputEvent.m_dragPosition, inputEvent.m_delta });
    }

    private void OnEndDrag(InputUIOnEndDragEvent inputEvent)
    {
        if (!isInDrag) return;
        isInDrag = false;
        // EventManager.DispatchEvent(null, EventType.Event_Camera_End, new object[] { inputEvent.m_dragPosition, inputEvent.m_delta });
    }

    #endregion
    public void SetActive(bool isShow)
    {
        this.gameObject.SetActiveOptimize(isShow);
        if (!isShow) isInDrag = isShow;
    }
}
