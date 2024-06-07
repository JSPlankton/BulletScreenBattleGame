using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;

public class ReusingPageEndRect : ReusingPageRect
{
    //当前操作方向
    private Direction m_DragDirection = Direction.Vertical;

    private float offsetDistance = 10f;
    Vector2 m_PointerStartLocalPos = Vector2.zero;
    Vector2 m_PointerEndLocalPos = Vector2.zero;

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (!m_isCanSlidingPage)
            return;

        Transform parent = transform.parent;
        if (parent)
        {
            m_ParentScrollRect = parent.GetComponentInParent<ScrollRect>();
        }
        if (m_ParentScrollRect)
        {
            m_DragDirection = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y) ? Direction.Horizontal : Direction.Vertical;
            if (m_DragDirection != scrollDirection)
            {
                //当前操作方向不等于滑动方向，将事件传给父对象
                ExecuteEvents.Execute(m_ParentScrollRect.gameObject, eventData, ExecuteEvents.beginDragHandler);
                return;
            }
        }
        base.OnBeginDrag(eventData);
        //更新两个包围盒的数据
        UpdateBounds();
        //记录由屏幕坐标转换为视图区域下的起始位置坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(viewRect, eventData.position, eventData.pressEventCamera, out m_PointerStartLocalPos);

        SetIsDrag(true);
    }
    public override void OnDrag(PointerEventData eventData)
    {

    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!m_isCanSlidingPage)
            return;

        if (m_ParentScrollRect)
        {
            if (m_DragDirection != scrollDirection)
            {
                //当前操作方向不等于滑动方向，将事件传给父对象
                ExecuteEvents.Execute(m_ParentScrollRect.gameObject, eventData, ExecuteEvents.endDragHandler);
                return;
            }
        }
        base.OnEndDrag(eventData);

        //记录由屏幕坐标转换为视图区域下的起始位置坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(viewRect, eventData.position, eventData.pressEventCamera, out m_PointerEndLocalPos);
        if (m_datas.Count > 0)
        {
            SetPageSyn();
        }
        SetIsDrag(false);
        SetStartTime(0);
        SetStopMove(false);
    }

    void SetPageSyn()
    {
        int nextPage = m_curPageNum;

        //与起始坐标求插值
        Vector2 offsetPos = m_PointerEndLocalPos - m_PointerStartLocalPos;

        if (horizontal)
        {
            if (offsetPos.x > offsetDistance)
            {
                nextPage--;
            }
            else if(offsetPos.x < 0 - offsetDistance)
            {
                nextPage++;
            }
        }
        else
        {
            if (offsetPos.y > offsetDistance)
            {
                nextPage++;
            }
            else if (offsetPos.y < 0 - offsetDistance)
            {
                nextPage--;
            }
        }
        nextPage = nextPage < 0 ? 0 : nextPage;
        nextPage = nextPage > PageItemCount - 1 ? PageItemCount - 1 : nextPage;

        JumpPage(nextPage);
    }
}
