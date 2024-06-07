using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollListMaskActive : MonoBehaviour
{
    public ScrollRect scrollRect;
    public GameObject imgTop;
    public GameObject imgBottom;

    private bool onEnd = false;
    private bool onStart = true;
    private bool needMask = true;

    private bool isFirst = true;

    private float offSetLength = 0.005f;

    private void Update()
    {
        if (scrollRect == null || imgTop == null && imgBottom == null || !needMask)
            return;

        if (isFirst)
        {
            isFirst = false;
            CheckNeedMask();
        }
        else
        {
            CheckShowMask();
        }
    }

    private void CheckNeedMask()
    {
        float contentLenth = scrollRect.content.GetComponent<RectTransform>().rect.height;
        if(contentLenth <=0)
        {
            isFirst = true;
            return;
        }
        float listLength = this.gameObject.GetComponent<RectTransform>().rect.height;
        if (listLength >= contentLenth)
        {
            needMask = false;
            onStart = true;
            onEnd = true;
            SetImgActive();
        }
    }
    private void CheckShowMask()
    {
        if (scrollRect.verticalNormalizedPosition - offSetLength <= 0)
        {
            onEnd = true;
        }
        else
        {
            onEnd = false;
        }

        if (scrollRect.verticalNormalizedPosition + offSetLength >= 1)
        {
            onStart = true;
        }
        else
        {
            onStart = false;
        }
        SetImgActive();
    }

    private void SetImgActive()
    {
        if (imgBottom != null)
        {
            if (!onEnd && !imgBottom.activeSelf)
            {
                imgBottom.SetActive(true);
            }
            else if (onEnd && imgBottom.activeSelf)
            {
                imgBottom.SetActive(false);
            }
        }


        if(imgTop != null)
        {
            if (!onStart && !imgTop.activeSelf)
            {
                imgTop.SetActive(true);
            }
            else if (onStart && imgTop.activeSelf)
            {
                imgTop.SetActive(false);
            }
        }
    }
}
