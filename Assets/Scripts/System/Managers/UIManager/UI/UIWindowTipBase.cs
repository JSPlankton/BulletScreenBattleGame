using UnityEngine;
using UnityEngine.EventSystems;

public class UIWindowTipBase : UIWindowBase, IPointerDownHandler, IPointerUpHandler
{
    public bool IsAutoDispose = true;
    public bool IsIgnoreTimeScale = false;
    [Range(0, 10)]
    public float DestroyDelayTime = 2f;
    TimerEvent destroyEvent;
    public static UIType UIType
    {
        get { return UIType.Tip; }
    }

    public override void OnInit()
    {
        base.OnInit();
        LogUtil.Log("Tip Born");
        isInStack = false;
        isInStackHide = false;
        if (IsAutoDispose)
        {
            destroyEvent = Managers.TimerManager.DelayCallBack(DestroyDelayTime, IsIgnoreTimeScale, (objs) =>
            {
                LogUtil.Log("Tip Auto Destroy");
                Managers.UIManager.DestroyUI(this);
            }, null);
        }

    }

    protected override void OnUIDestroy()
    {
        if (destroyEvent != null)
        {
            Managers.TimerManager.RemoveTimer(destroyEvent);
            LogUtil.Log("Tip destroyEvent null");
            destroyEvent = null;
        }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {

    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {

    }
}