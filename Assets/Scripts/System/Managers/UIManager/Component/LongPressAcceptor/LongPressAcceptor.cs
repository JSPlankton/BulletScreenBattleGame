﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;

public class LongPressAcceptor : MonoBehaviour ,IPointerDownHandler,IPointerUpHandler
{
    /// <summary>
    /// 长按时间
    /// </summary>
    public float LongPressTime = 1f;

    float m_Timer = 0;

    bool isPress = false;
    bool isDispatch = false;
    bool isLongPress = false;

    public InputUIEventLongPressCallBack OnLongPress;
    //public InputEventHandle<InputUIOnClickEvent> OnPointerDown;
    //public InputEventHandle<InputUIOnClickEvent> OnPointerUp;

    void ResetAcceptor()
    {
        isPress = false;
        isDispatch = false;
        m_Timer = 0;
        isLongPress = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPress = true;

        if (OnLongPress != null)
        {
             OnLongPress(InputUIEventType.PressDown);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (OnLongPress != null)
        {
            OnLongPress(InputUIEventType.PressUp);

            if (!isLongPress)
            {
                OnLongPress(InputUIEventType.Click);
            }
        }
        ResetAcceptor();
    }

    void Update()
    {
        if (isPress && !isDispatch)
        {
            m_Timer += Time.deltaTime;
            if (m_Timer > LongPressTime)
            {
                //LogUtil.Log(" OnLongPress ");

                //派发长按事件
                isDispatch = true;
                if (OnLongPress != null)
                {
                    isLongPress = true;
                    OnLongPress(InputUIEventType.LongPress);
                }
            }
        }
    }
}

public delegate void InputUIEventLongPressCallBack(InputUIEventType type);
