using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIWindowPopupBase : UIWindowBase, IPointerDownHandler, IPointerUpHandler
{
    public GameObject m_bgPopContent;
    public GameObject m_bgPopClick;
    public bool m_isCanClose = true;
    public bool m_isCancelCb = false;
    [Tooltip("背景模糊时是否隐藏MainUI")]
    public bool m_isHideMainUIByBlur = false;
    [Tooltip("是否屏蔽Android返回键")]
    public bool m_isDisableAndroidBackMenu = false;
    public static UIType UIType
    {
        get { return UIType.Popup; }
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {

    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {
        if (m_bgPopClick != null && m_isCanClose && eventData.pointerEnter != null)
        {
            if (eventData.pointerEnter.Equals(m_bgPopClick))
            {

                Managers.UIManager.DestroyUI(this);
            }
        }
    }

    public virtual void OnAndroidBack()
    {
        Managers.UIManager.DestroyUI(this);
    }

    public override void OnBeforeInit()
    {
        isShowGlassBlur = true;
        if (isShowMask)
            isShowGlassBlur = false;
        base.OnBeforeInit();
    }

    public override void OnShow()
    {
        base.OnShow();
        this.gameObject.transform.SetAsLastSibling();
    }

    public override void OnInit()
    {
        base.OnInit();
        isInStackHide = false;
        if (m_bgPopContent != null)
        {
            if (m_bgPopContent.GetComponent<Image>() != null)
            {
                m_bgPopContent.GetComponent<Image>().raycastTarget = true;
            }
        }
        if (isShowMask && m_goPost != null)
        {
            Image mask = m_goPost.GetComponent<Image>();
            if (mask == null)
            {
                mask = m_goPost.AddComponent<Image>();
            }
            // mask.color = ColorUtil.GetColor(0, 0, 0, 220);
            mask.raycastTarget = false;
        }
    }
}