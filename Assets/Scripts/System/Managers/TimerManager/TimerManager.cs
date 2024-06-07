/**
 *	计时任务管理类,处理计划执行的任务
 *	支持定时和延时任务。
 *	注意事项：
 *	1.使用计时器的地方需要手工销毁
 *	2.时间单位：秒
 *	3.增加Target,若target初始不为空，之后为is null为True，认为该计时器失效
 *	author:zhaojinlun
 *	date:2021年4月22日
 *	copyright: fungather.net
 *	
 *	示例：
 *	创建和销毁
 *	
    TimerEvent timerEvent = Managers.TimerManager.AddTimer(2f, true, 1, "test", (objs) =>
    {
        LogUtil.Log("2 test coallbakc..........");
    }, null);
    
    Managers.TimerManager.RemoveTimer(timerEvent);
    Managers.TimerManager.RemoveTimer("test");
 *	
 ***/
using UnityEngine;
using System.Collections.Generic;

namespace Managers
{
    public class TimerManager : BaseManager
    {
        private static TimerManager timerManager;
        public static TimerManager GetInstance()
        {
            if (timerManager == null)
            {
                timerManager = new TimerManager();
            }
            return timerManager;
        }

        public static List<TimerEvent> m_timers = new List<TimerEvent>();
        public override void OnLateUpdate()
        {
            for (int i = 0; i < m_timers.Count; i++)
            {
                TimerEvent e = m_timers[i];
                e.Update();

                if (e.m_isDone)
                {
                    e.CompleteTimer();

                    if (e.m_isDone)
                    {
                        m_timers.Remove(e);
                        e = null;
                        i--;
                    }
                }
            }
        }

        public static bool GetIsExistTimer(string timerName)
        {
            for (int i = 0; i < m_timers.Count; i++)
            {
                var e = m_timers[i];
                if (e.m_timerName == timerName)
                {
                    return true;
                }
            }

            return false;
        }

        public static TimerEvent GetTimer(string timerName)
        {
            for (int i = 0; i < m_timers.Count; i++)
            {
                var e = m_timers[i];
                if (e.m_timerName == timerName)
                {
                    return e;
                }
            }

            throw new System.Exception("Get Timer  Exception not find ->" + timerName + "<-");
        }

        /// <summary>
        /// 延迟调用
        /// </summary>
        /// <param name="delayTime">间隔时间</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="objs">回调函数的参数</param>
        /// <returns></returns>
        public static TimerEvent DelayCallBack(float delayTime, TimerCallBack callBack, params object[] objs)
        {
            return AddTimer(delayTime, false, 0, null, callBack, objs);
        }

        /// <summary>
        /// 延迟调用
        /// </summary>
        /// <param name="delayTime">间隔时间</param>
        /// <param name="isIgnoreTimeScale">是否忽略时间缩放</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="objs">回调函数的参数</param>
        /// <returns></returns>
        public static TimerEvent DelayCallBack(float delayTime, bool isIgnoreTimeScale, TimerCallBack callBack, params object[] objs)
        {
            return AddTimer(delayTime, isIgnoreTimeScale, 0, null, callBack, objs);
        }
        /// <summary>
        /// 延迟调用
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="delayTime">间隔时间</param>
        /// <param name="isIgnoreTimeScale">是否忽略时间缩放</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="objs">回调函数的参数</param>
        /// <returns></returns>
        public static TimerEvent DelayCallBack(UnityEngine.Object target, float delayTime, bool isIgnoreTimeScale, TimerCallBack callBack, params object[] objs)
        {
            return AddTimer(target, delayTime, isIgnoreTimeScale, 0, null, callBack, objs);
        }

        /// <summary>
        /// 间隔一定时间重复调用
        /// </summary>
        /// <param name="spaceTime">间隔时间</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="objs">回调函数的参数</param>
        /// <returns></returns>
        public static TimerEvent CallBackOfIntervalTimer(float spaceTime, TimerCallBack callBack, params object[] objs)
        {
            return AddTimer(spaceTime, false, -1, null, callBack, objs);
        }

        /// <summary>
        /// 间隔一定时间重复调用
        /// </summary>
        /// <param name="spaceTime">间隔时间</param>
        /// <param name="isIgnoreTimeScale">是否忽略时间缩放</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="objs">回调函数的参数</param>
        /// <returns></returns>
        public static TimerEvent CallBackOfIntervalTimer(float spaceTime, bool isIgnoreTimeScale, TimerCallBack callBack, params object[] objs)
        {
            return AddTimer(spaceTime, isIgnoreTimeScale, -1, null, callBack, objs);
        }

        /// <summary>
        /// 间隔一定时间重复调用
        /// </summary>
        /// <param name="spaceTime">间隔时间</param>
        /// <param name="isIgnoreTimeScale">是否忽略时间缩放</param>
        /// <param name="timerName">Timer的名字</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="objs">回调函数的参数</param>
        /// <returns></returns>
        public static TimerEvent CallBackOfIntervalTimer(float spaceTime, bool isIgnoreTimeScale, string timerName, TimerCallBack callBack, params object[] objs)
        {
            return AddTimer(spaceTime, isIgnoreTimeScale, -1, timerName, callBack, objs);
        }

        /// <summary>
        /// 间隔一定时间重复调用
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <param name="spaceTime">间隔时间</param>
        /// <param name="isIgnoreTimeScale">是否忽略时间缩放</param>
        /// <param name="timerName">Timer的名字</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="objs">回调函数的参数</param>
        /// <returns></returns>
        public static TimerEvent CallBackOfIntervalTimer(UnityEngine.Object target, float spaceTime, bool isIgnoreTimeScale, string timerName, TimerCallBack callBack, params object[] objs)
        {
            return AddTimer(target, spaceTime, isIgnoreTimeScale, -1, timerName, callBack, objs);
        }

        /// <summary>
        /// 有限次数的重复调用
        /// </summary>
        /// <param name="spaceTime">间隔时间</param>
        /// <param name="callBackCount">重复调用的次数</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="objs">回调函数的参数</param>
        /// <returns></returns>
        public static TimerEvent CallBackOfIntervalTimer(float spaceTime, int callBackCount, TimerCallBack callBack, params object[] objs)
        {
            return AddTimer(spaceTime, false, callBackCount, null, callBack, objs);
        }

        /// <summary>
        /// 有限次数的重复调用
        /// </summary>
        /// <param name="spaceTime">间隔时间</param>
        /// <param name="isIgnoreTimeScale">是否忽略时间缩放</param>
        /// <param name="callBackCount">重复调用的次数</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="objs">回调函数的参数</param>
        /// <returns></returns>
        public static TimerEvent CallBackOfIntervalTimer(float spaceTime, bool isIgnoreTimeScale, int callBackCount, TimerCallBack callBack, params object[] objs)
        {
            return AddTimer(spaceTime, isIgnoreTimeScale, callBackCount, null, callBack, objs); ;
        }

        /// <summary>
        /// 有限次数的重复调用
        /// </summary>
        /// <param name="spaceTime">间隔时间</param>
        /// <param name="isIgnoreTimeScale">是否忽略时间缩放</param>
        /// <param name="callBackCount">重复调用的次数</param>
        /// <param name="timerName">Timer的名字</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="objs">回调函数的参数</param>
        /// <returns></returns>
        public static TimerEvent CallBackOfIntervalTimer(float spaceTime, bool isIgnoreTimeScale, int callBackCount, string timerName, TimerCallBack callBack, params object[] objs)
        {
            return AddTimer(spaceTime, isIgnoreTimeScale, callBackCount, timerName, callBack, objs);
        }

        /// <summary>
        /// 逐帧调用
        /// </summary>
        /// <param name="callBack">回调函数</param>
        /// <param name="objs"></param>
        /// <returns></returns>
        public static TimerEvent CallBackOfFrameTimer(TimerCallBack callBack, params object[] objs)
        {
            return AddTimer(Time.fixedDeltaTime, false, -1, null, callBack, objs);
        }

        /// <summary>
        /// 添加一个Timer
        /// </summary>
        /// <param name="spaceTime">间隔时间</param>
        /// <param name="isIgnoreTimeScale">是否忽略时间缩放</param>
        /// <param name="callBackCount">重复调用的次数</param>
        /// <param name="timerName">Timer的名字</param>
        /// <param name="callBack">回调函数</param>
        /// <param name="objs">回调函数的参数</param>
        /// <returns></returns>
        public static TimerEvent AddTimer(float spaceTime, bool isIgnoreTimeScale, int callBackCount, string timerName, TimerCallBack callBack, params object[] objs)
        {
            return AddTimer(null, spaceTime, isIgnoreTimeScale, callBackCount, timerName, callBack, objs);
        }

        public static TimerEvent AddTimer(UnityEngine.Object target, float spaceTime, bool isIgnoreTimeScale, int callBackCount, string timerName, TimerCallBack callBack, params object[] objs)
        {
            TimerEvent te = new TimerEvent();
            te.m_target = target;
            te.m_timerName = timerName ?? te.GetHashCode().ToString();
            te.m_currentTimer = 0;
            te.m_timerSpace = spaceTime;

            te.m_callBack = callBack;
            te.m_objs = objs;

            te.m_isIgnoreTimeScale = isIgnoreTimeScale;
            te.m_repeatCount = callBackCount;

            m_timers.Add(te);

            return te;
        }
        public static void RemoveTimer(TimerEvent timer, bool isCallBack = false)
        {
            //LogUtil.Log("DestroyTimer " + timer.m_timerName + " isTest " + (timer == test));

            if (m_timers.Contains(timer))
            {
                if (isCallBack)
                {
                    timer.CallBackTimer();
                }

                m_timers.Remove(timer);
                timer = null;
            }
            else
            {
                LogUtil.Log("Timer DestroyTimer error: dont exist timer " + timer);
            }
        }

        public static void RemoveTimer(string timerName, bool isCallBack = false)
        {
            //LogUtil.Log("DestroyTimer2  ----TIMER " + timerName);
            for (int i = 0; i < m_timers.Count; i++)
            {
                TimerEvent te = m_timers[i];
                if (te.m_timerName.Equals(timerName))
                {
                    RemoveTimer(te, isCallBack);
                }
            }
        }

        public static void RemoveAllTimer(bool isCallBack = false)
        {
            for (int i = 0; i < m_timers.Count; i++)
            {
                if (isCallBack)
                {
                    m_timers[i].CallBackTimer();
                }
            }

            m_timers.Clear();
        }

        public static void ResetTimer(TimerEvent timer)
        {
            if (m_timers.Contains(timer))
            {
                timer.ResetTimer();
            }
            else
            {
                LogUtil.Error("Timer ResetTimer error: dont exist timer " + timer);
            }
        }

        public static void ResetTimer(string timerName)
        {
            for (int i = 0; i < m_timers.Count; i++)
            {
                var e = m_timers[i];

                if (e.m_timerName.Equals(timerName))
                {
                    ResetTimer(e);
                }
            }
        }
    }
}
