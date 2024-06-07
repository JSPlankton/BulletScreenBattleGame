using UnityEngine;
/// <summary>
/// 约定，第一个参数为跟随GameObject
/// </summary>
public class UIWindowSceneUIBase : UIWindowBase
{
    public Vector2 offset = Vector2.zero;
    public static UIType UIType
    {
        get { return UIType.SceneUI; }
    }

    public override void OnInit()
    {
        base.OnInit();
        isInStack = false;
        if (this.InitObject.Length >= 1 && this.InitObject[0] != null)
        {
            GameObject followGameObject = (GameObject)this.InitObject[0];
            SetFollowObject(followGameObject);
        }
    }
    protected void SetFollowObject(GameObject followGameObject)
    {
        if (null == followGameObject)
        {
            return;
        }
        SceneUIComponent scenUIComponet = this.gameObject.AddComponent<SceneUIComponent>();
        scenUIComponet.SetFollowObject(followGameObject, offset);
    }
    public void SetFollowWorldPos(Vector3 worldPos)
    {
        SceneUIComponent scenUIComponet = this.gameObject.GetComponent<SceneUIComponent>();
        if (scenUIComponet == null)
        {
            scenUIComponet = this.gameObject.AddComponent<SceneUIComponent>();
        }
        if (scenUIComponet != null)
        {
            scenUIComponet.SetFollowWorldPos(worldPos);
        }
    }
    public void ChangeUIOffset(Vector2 offset)
    {
        this.offset = offset;
        SceneUIComponent scenUIComponet = this.gameObject.GetComponent<SceneUIComponent>();
        scenUIComponet.ChangePos(offset);
    }

    public void ResetFollowObj(GameObject followObj)
    {
        SceneUIComponent scenUIComponet = this.gameObject.GetComponent<SceneUIComponent>();
        if (scenUIComponet != null && followObj != null)
        {
            scenUIComponet.SetFollowObject(followObj, offset);
        }
    }
    #region 滑动
    public bool isInDrag = false;
    public void AddButtonDragListener(string btnName)
    {
        AddOnDragListener(btnName, OnDrag);
        AddBeginDragListener(btnName, OnBeginDrag);
        AddEndDragListener(btnName, OnEndDrag);
    }
    private void OnBeginDrag(InputUIOnBeginDragEvent inputEvent)
    {
        isInDrag = true;
        // EventManager.DispatchEvent(null, EventType.Event_Camera_Begin, new object[] { inputEvent.m_dragPosition, inputEvent.m_delta });
    }

    private void OnDrag(InputUIOnDragEvent inputEvent)
    {
        if (!isInDrag) isInDrag = true;
        // EventManager.DispatchEvent(null, EventType.Event_Camera_Move, new object[] { inputEvent.m_dragPosition, inputEvent.m_delta });
    }

    private void OnEndDrag(InputUIOnEndDragEvent inputEvent)
    {
        isInDrag = false;
        // EventManager.DispatchEvent(null, EventType.Event_Camera_End, new object[] { inputEvent.m_dragPosition, inputEvent.m_delta });
    }

    #endregion

    public virtual Transform GetGuideTransform()
    {
        return null;
    }
}