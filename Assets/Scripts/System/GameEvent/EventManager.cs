/**
 *	事件管理类 
 *	当listner为空时，为全局事件，所有注册监听的地方都可收到对应信息
 *	
 *	部分测试用例
 *	
 *	测试结构
 *	public class Data
    {
        public string name = "";
        public string sex = "";
    }
 *  1.添加监听
 *	EventManager.AddEvent(null, EventType, Function);
 *	EventManager.AddEvent(null, EventType, Function, UseOnce);
 *	EventManager.AddEvent(source,EvenType, Function);
 *	EventManager.AddEvent(source,EvenType, Function, UseOnce);
 *	
 *	2.移除监听
 *	EventManager.RemoveEvent(null, EventType, Function);
 *	EventManager.RemoveEvent(source, EventType, Function);
 *	
 *	3.移除所有监听
 *	EventManager.RemoveAllEvent();
 *	
 *	4.分发事件
 *	EventManager.DispatchEvent(source, EventType, params);
 *	
 *	5.示例
 *	EventManager.AddEvent(null, EventType.DataEvent_Update, OnDataAdd, true);
 *  EventManager.AddEvent(data1, EventType.DataEvent_Update, OnData1Update);
 *  EventManager.AddEvent(data1, EventType.DataEvent_Update, OnData2Update, true);
 *  EventManager.AddEvent(data2, EventType.DataEvent_Update, OnData1Update);
 *  
 *  EventManager.RemoveEvent(null, EventType.DataEvent_Update, OnDataAdd);
 *  EventManager.RemoveEvent(data1, EventType.DataEvent_Update, OnData1Update);
 *  
 *  EventManager.DispatchEvent(null, EventType.DataEvent_Update, new object[] { "name........." });
 *  EventManager.DispatchEvent(data1, EventType.DataEvent_Update, new object[] { data1.name });
 *  
 ***/
using UnityEngine;
using System.Collections.Generic;
using System;

public delegate void EventHandle(params object[] args);
public delegate void EventHandle<T>(T e, params object[] args);

public class EventData
{
    public object source;
    public string eventName = "";
    public EventHandle handle;
    public bool isUseOnce = false;

    public static EventData GetOnUIEvent(object source, string eventName, EventHandle handle, bool isUseOnce = false)
    {
        EventData data = HeapObjectPool<EventData>.GetObject();

        data.source = source;
        data.eventName = eventName;
        data.handle = handle;
        data.isUseOnce = isUseOnce;

        return data;
    }
}

/// <summary>
/// 事件管理类
/// 通过监听事件名称，来接收事件源的对应变化，并调用相关处理函数
/// </summary>
public class EventManager
{
    #region 以String为Key的事件派发
    private const string GLOBAL = "Global";

    private static Dictionary<String, Dictionary<object, Dictionary<int, EventHandle>>> mEventDic = new Dictionary<String, Dictionary<object, Dictionary<int, EventHandle>>>();
    private static Dictionary<String, List<EventHandle>> mUseOnceEventDic = new Dictionary<String, List<EventHandle>>();

    /// <summary>
    /// 添加事件及回调
    /// </summary>
    /// <param name="source">事件源</param>
    /// <param name="eventName">事件枚举</param>
    /// <param name="handle">回调</param>
    /// <param name="isUseOnce">单次触发</param>
    public static void AddEvent(object source, String eventName, EventHandle handle, bool isUseOnce = false)
    {
        if (source == null) source = GLOBAL;

        if (isUseOnce)
        {
            if (mUseOnceEventDic.ContainsKey(eventName))
            {
                if (!mUseOnceEventDic[eventName].Contains(handle))
                    mUseOnceEventDic[eventName].Add(handle);
                else
                    LogUtil.Warning("already existing EventType: " + eventName + " handle: " + handle);
            }
            else
            {
                List<EventHandle> temp = new List<EventHandle>();
                temp.Add(handle);
                mUseOnceEventDic.Add(eventName, temp);
            }
        }
        else
        {
            if (mEventDic.ContainsKey(eventName))
            {
                if (mEventDic[eventName].ContainsKey(source))
                    mEventDic[eventName][source][handle.GetHashCode()] = handle;
                else
                {
                    Dictionary<int, EventHandle> dictHandler = new Dictionary<int, EventHandle>();
                    dictHandler[handle.GetHashCode()] = handle;
                    mEventDic[eventName][source] = dictHandler;
                }
            }
            else
            {
                Dictionary<int, EventHandle> dictHandle = new Dictionary<int, EventHandle>();
                dictHandle[handle.GetHashCode()] = handle;
                Dictionary<object, Dictionary<int, EventHandle>> dict = new Dictionary<object, Dictionary<int, EventHandle>>();
                dict[source] = dictHandle;
                mEventDic[eventName] = dict;
            }
        }
    }

    /// <summary>
    /// 移除事件源的某类事件的一个回调
    /// </summary>
    /// <param name="source">事件源</param> 
    /// <param name="eventName">事件名称</param> 
    /// <param name="handle">事件处理函数</param>
    public static void RemoveEvent(object source, String eventName, EventHandle handle)
    {
        if (source == null) source = GLOBAL;

        if (mUseOnceEventDic.ContainsKey(eventName))
        {
            if (mUseOnceEventDic[eventName].Contains(handle))
            {
                mUseOnceEventDic[eventName].Remove(handle);
                if (mUseOnceEventDic[eventName].Count == 0)
                {
                    mUseOnceEventDic.Remove(eventName);
                }
            }
        }

        if (mEventDic.ContainsKey(eventName))
        {
            Dictionary<object, Dictionary<int, EventHandle>> dict = mEventDic[eventName];

            if (!dict.ContainsKey(source) || dict[source] == null) return;

            if (!dict[source].ContainsKey(handle.GetHashCode())) return;

            dict[source].Remove(handle.GetHashCode());

            if (dict[source] == null)
                mEventDic[eventName].Remove(source);
        }
    }

    /// <summary>
    /// 移除某类事件
    /// </summary>
    /// <param name="type"></param>
    public static void RemoveEvent(String type)
    {
        RemoveUseOnceEvent(type);

        if (mEventDic.ContainsKey(type))
        {
            mEventDic.Remove(type);
        }
    }
    /// <summary>
    /// 移除使用一次的事件
    /// </summary>
    /// <param name="type"></param>
    private static void RemoveUseOnceEvent(String type)
    {
        if (mUseOnceEventDic.ContainsKey(type))
        {
            mUseOnceEventDic.Remove(type);
        }
    }

    /// <summary>
    /// 移除所有事件
    /// </summary>
    public static void RemoveAllEvent()
    {
        mUseOnceEventDic.Clear();

        mEventDic.Clear();
    }

    /// <summary>
    /// 触发事件
    /// </summary>
    /// <param name="source">事件源</param>
    /// <param name="eventName">事件名称</param>
    /// <param name="args"></param>
    public static void DispatchEvent(object source, String eventName, params object[] args)
    {
        if (source == null) source = GLOBAL;

        if (mEventDic.ContainsKey(eventName))
        {
            try
            {
                if (mEventDic.ContainsKey(eventName) && mEventDic[eventName].ContainsKey(source))
                {
                    foreach (var handler in mEventDic[eventName][source].ValueToArray())
                    {
                        handler(args);
                    }
                }

            }
            catch (Exception e)
            {
                LogUtil.Exception(e);
            }
        }

        if (mUseOnceEventDic.ContainsKey(eventName))
        {
            for (int i = 0; i < mUseOnceEventDic[eventName].Count; i++)
            {
                //遍历委托链表
                foreach (EventHandle callBack in mUseOnceEventDic[eventName][i].GetInvocationList())
                {
                    try
                    {
                        callBack(args);
                    }
                    catch (Exception e)
                    {
                        LogUtil.Exception(e);
                    }
                }
            }
            RemoveUseOnceEvent(eventName);
        }
    }

    #endregion
}
