using System;
using UnityEngine;

public class TimeUtil
{
    private static TimeUtil _instance;
    private static float _lastUpdateTime = -1f;
    private static double _serverTime = -1.0;
    private static double _smoothServerTime = -1.0;
    private static bool m_update;
    private static double m_UpdateTime;
    private const int SYNC_TIME_INTERVAL = 60;
    private const int S_TO_MS = 1000;
    private static long _nextZeroClock;
    private static long _nextWeekZeroClock;

    /// <summary>
    /// 一天的毫秒数
    /// </summary>
    public static long ONE_DAY_MILLISECONDS = 86400000L;
    /// <summary>
    /// 一小时的毫秒数
    /// </summary>
    public static long ONE_HOUR_MILLISECONDS = 3600000L;
    /// <summary>
    /// 一分钟的毫秒数
    /// </summary>
    public static long ONE_MINUTE_MILLISECONDS = 60000L;
    /// <summary>
    /// 一秒的毫秒数
    /// </summary>
    public static long ONE_SECOND_MILLISECONDS = 1000L;

    public bool IsToday(long time)
    {
        DateTime toDay = DateUtil.ConvertTimeToDateTime(ToDayZeroTime);
        DateTime currentTime = DateUtil.ConvertTimeToDateTime(time);
        TimeSpan span = currentTime.Subtract(toDay);
        return span.TotalDays > 0 && span.TotalDays <= 1;
    }
    public bool IsToday(double time)
    {
        return ((Now / 0x15180) == (((int)time) / 0x15180));
    }
    public bool IsServerTimeToday(long time)
    {
        if (time > ToDayZeroTime)
        {
            return true;
        }
        return false;
    }

    public double LerpTime(double originTime, double targetTime, double lerpFactor = 0.95f)
    {
        if (originTime >= targetTime)
        {
            return originTime;
        }
        return (originTime + ((targetTime - originTime) * lerpFactor));
    }

    private static void Process(int timestamp)
    {
        if (m_update && ((timestamp - ((int)_serverTime)) > 60))
        {
            m_update = false;
        }
    }

    private void ResetSmoothTime()
    {
        _smoothServerTime = -1.0;
        _lastUpdateTime = -1f;
    }

    public void SetServerTime(double serverTime)
    {
        if (serverTime > _serverTime)
        {
            _serverTime = serverTime;
            m_UpdateTime = RealTimeSinceStartUp;
            m_update = true;
        }
    }
    public void SetNextZeroClock(long time)
    {
        _nextZeroClock = time;
    }
    public void SetNextWeekZeroClock(long time)
    {
        _nextWeekZeroClock = time;
    }
    /// <summary>
    /// 服务器时间是否已经同步了
    /// </summary>
    public bool IsServerTimeInited => _serverTime > 0;

    public void StartSyncTime()
    {
        this.ResetSmoothTime();
        Oscillator.Instance.secondEvent += new Action<int>(Process);
    }

    public void StopSyncTime()
    {
        if (Oscillator.IsAvailable)
        {
            Oscillator.Instance.secondEvent -= new Action<int>(Process);
        }
        this.ResetSmoothTime();
    }

    public int TodayLeftTimeUTC()
    {
        return ((int)(0x15180 - (Now % 0x15180)));
    }

    private void UpdateSmoothServerTime()
    {
        double updateTime = UpdateTime;
        if (_smoothServerTime == -1.0)
        {
            _smoothServerTime = updateTime;
        }
        _smoothServerTime = this.LerpTime(_smoothServerTime, updateTime, 0.949999988079071);
    }

    public static TimeUtil inst
    {
        get
        {
            if (_instance == null)
            {
                _instance = new TimeUtil();
            }
            return _instance;
        }
    }

    public int LocalTimestamp
    {
        get
        {
            return (int)((DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks) / 0x989680L);
        }
    }

    public double LocalUpdateTime
    {
        get
        {
            return (((double)(DateTime.UtcNow.Ticks - new DateTime(1970, 1, 1, 0, 0, 0).Ticks)) / 10000000.0);
        }
    }

    public static long Now
    {
        get
        {
            return (long)(_serverTime + RealTimeSinceStartUp - m_UpdateTime);
        }
    }
    public static long NextWeekClock0 => _nextWeekZeroClock;
    public static long TomorrowClock0 => _nextZeroClock;
    public static long ToDayZeroTime { get { return TomorrowClock0 - (long)86400000; } }
    /// <summary>
    /// 变更天刷新，注这个数值一定要大于 NextDayRefreshPeriod
    /// </summary>
    public static long NextDayRefresh { get; private set; } = 30 * ONE_SECOND_MILLISECONDS;
    /// <summary>
    /// 延迟变更天毫秒数
    /// </summary>
    public static long NextDayRefreshPeriod { get; private set; } = 3 * ONE_SECOND_MILLISECONDS;
    
    public double SmoothServerTime
    {
        get
        {
            if (_lastUpdateTime != Time.time)
            {
                this.UpdateSmoothServerTime();
                _lastUpdateTime = Time.time;
            }
            return _smoothServerTime;
        }
    }

    public static double UpdateTime
    {
        get
        {
            return ((RealTimeSinceStartUp - m_UpdateTime) + _serverTime);
        }
    }

    public static long RealTimeSinceStartUp
    {
        get
        {
            return (long)(Time.realtimeSinceStartup * S_TO_MS);
        }
    }

}

