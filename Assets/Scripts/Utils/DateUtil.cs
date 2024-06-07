using System;
using UnityEngine;

/// <summary>
/// 等待建筑类型接入
/// </summary>
public static class DateUtil
{
    /// <summary>
    /// 毫秒转秒
    /// </summary>
    /// <param name="timespan"></param>
    /// <returns></returns>
    public static long TimeFormat(long timespan)
    {
        return timespan / 1000;
    }

    public static string TimeFormatSeconds(int second)
    {
        TimeSpan ts = new TimeSpan(0, 0, second);
        return GetTime(ts);
    }

    [Obsolete("不需要double类型的时间戳转换")]
    public static string TimeFormatSeconds(double second)
    {
        TimeSpan ts = new TimeSpan(0, 0, (int)second);
        return GetTime(ts);
    }

    public static string TimeFormatSeconds(long second)
    {
        TimeSpan ts = new TimeSpan(0, 0, (int)second);
        return GetTime(ts);
    }

    public static string TimeFormatMSeconds(long millisecond)
    {
        TimeSpan ts = new TimeSpan(0, 0, (int)TimeFormat(millisecond));
        return GetTime(ts);
    }

    /// <summary>
    /// 只返回小时和分钟
    /// </summary>
    /// <param name="second"></param>
    /// <returns></returns>
    public static string TimeFormatMinuteAndSecond(long millisecond)
    {
        return TimeFormatMinuteAndSecondBySecond(TimeFormat(millisecond));
    }
    /// <summary>
    /// 只返回小时、分钟、秒，不返回天
    /// </summary>
    /// <param name="second"></param>
    /// <returns></returns>
    public static string TimeFormatHourMinuteAndSecond(long second)
    {
        TimeSpan timeSpan = new TimeSpan(0, 0, (int)(second / 1000));

        System.Text.StringBuilder builder = new System.Text.StringBuilder();

        int day = timeSpan.Days;
        int hours = timeSpan.Hours + day * 24;

        builder.Append(hours.ToString("00"));
        builder.Append(":");
        builder.Append(timeSpan.Minutes.ToString("00"));
        builder.Append(":");
        builder.Append(timeSpan.Seconds.ToString("00"));

        return builder.ToString();
    }

    /// </summary>
    /// <param name="second"></param>
    /// <returns></returns>
    public static string TimeFormatMinuteAndSecondBySecond(long second)
    {
        TimeSpan timeSpan = new TimeSpan(0, 0, (int)second);

        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        builder.Append(timeSpan.Minutes.ToString("00"));
        builder.Append(":");
        builder.Append(timeSpan.Seconds.ToString("00"));

        return builder.ToString();
    }

    private static string GetTime(TimeSpan timeSpan)
    {
        return GetFormatTimeStr(timeSpan);
    }
    /// <summary>
    /// 最多显示三个时间段
    /// </summary>
    /// <param name="millisecond"></param>
    /// <returns></returns>
    public static string TimeFormatMaxThreeTimeByMSeconds(long millisecond)
    {
        TimeSpan ts = new TimeSpan(0, 0, (int)TimeFormat(millisecond));
        return GetFormatTimeStr(ts, false);
    }
    public static string TimeFormatMaxThreeTimeBySeconds(int second)
    {
        TimeSpan ts = new TimeSpan(0, 0, second);
        return GetFormatTimeStr(ts, false);
    }
    /// <summary>
    /// 获取时间字符串
    /// 1.时间长度大于1天的,返回 dd:hh:mm
    /// 2.时间长度小于1天的,返回 hh:mm:ss
    /// </summary>
    /// <param name="timeSpan"></param>
    /// <returns></returns>
    private static string GetFormatTimeStr(TimeSpan timeSpan, bool showSecond = true)
    {
        System.Text.StringBuilder builder = new System.Text.StringBuilder();

        if (timeSpan.Days > 0)
        {
            builder.Append(LanguageHelper.GetValues("Desc_Time_Day_Para", timeSpan.Days.ToString()));
            builder.Append(" ");
            builder.Append(timeSpan.Hours.ToString("00"));
            builder.Append(":");
            builder.Append(timeSpan.Minutes.ToString("00"));

            if (showSecond)
            {
                builder.Append(":");
                builder.Append(timeSpan.Seconds.ToString("00"));
            }
        }
        else
        {
            builder.Append(timeSpan.Hours.ToString("00"));
            builder.Append(":");
            builder.Append(timeSpan.Minutes.ToString("00"));
            builder.Append(":");
            builder.Append(timeSpan.Seconds.ToString("00"));
        }

        return builder.ToString();
    }

    /// <summary>
    /// 将时间戳转换为时间
    /// </summary>
    /// <param name="timestamp"></param>
    /// <returns></returns>
    public static DateTime ConvertTimeToDateTime(long timestamp)
    {
        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0));
        DateTime createTime = startTime.AddMilliseconds(timestamp);
        return createTime;
    }
    public static DateTime ConvertTimeToWorldDateTime(long timestamp)
    {
        DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime createTime = startTime.AddMilliseconds(timestamp);
        DateTime worldTime = createTime.AddHours(-2).ToUniversalTime();
        return worldTime;
    }
    /// <summary>
    /// 将时间转换为时间戳
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static long ConvertTimeToLong(DateTime dateTime)
    {
        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0));
        TimeSpan toNow = dateTime.Subtract(startTime);
        long timeStamp = toNow.Ticks;
        timeStamp = long.Parse(timeStamp.ToString().Substring(0, timeStamp.ToString().Length - 4));
        return timeStamp;
    }
    /// <summary>
    /// 将时间戳转换为字符
    /// </summary>
    /// <param name="timestamp"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static string ConvertDateTimeByFormatProvider(long timestamp, string format = "yyyy/MM/dd HH:mm")
    {
        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0));
        DateTime createTime = startTime.AddMilliseconds(timestamp);
        return createTime.ToString(format, System.Globalization.DateTimeFormatInfo.InvariantInfo);
    }

    /// <summary>
    /// 将时间戳转换为字符串
    /// </summary>
    /// <param name="timestamp"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static string ConvertDateTimeToString(long timestamp, string format = "yyyy/MM/dd HH:mm")
    {
        DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0));
        DateTime createTime = startTime.AddMilliseconds(timestamp);
        return createTime.ToString(format);
    }
    /// <summary>
    /// 将时间戳转换为世界时间
    /// </summary>
    /// <param name="timestamp"></param>
    /// <param name="format"></param>
    /// <returns></returns>
    public static string ConvertDateTimeToWorldTimeToString(long timestamp, string format = "yyyy/MM/dd HH:mm")
    {
        DateTime startTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime createTime = startTime.AddMilliseconds(timestamp);
        DateTime worldTime = createTime.AddHours(-2).ToUniversalTime();
        return worldTime.ToString(format);
    }
    /// <summary>
    /// 只保留一个时间段
    /// </summary>
    /// <param name="ms"></param>
    /// <returns></returns>
    public static string TimeFormatMSecondsToOneOfDHMS(long ms)
    {
        TimeSpan timeSpan = new TimeSpan(0, 0, (int)TimeFormat(ms));
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        if (0 < timeSpan.Days)
        {
            builder.Append(LanguageHelper.GetValues("Desc_Time_Day_Para", timeSpan.Days.ToString("")));
        }
        else if (0 < timeSpan.Hours)
        {
            builder.Append(LanguageHelper.GetValues("Desc_Time_Hour_Para", timeSpan.Hours.ToString("")));
        }
        else if (0 < timeSpan.Minutes)
        {
            builder.Append(LanguageHelper.GetValues("Desc_Time_Minute_Para", timeSpan.Minutes.ToString("")));
        }
        else if (0 < timeSpan.Seconds)
        {
            builder.Append(LanguageHelper.GetValues("Desc_Time_Second_Para", timeSpan.Seconds.ToString("")));
        }
        return builder.ToString();
    }
    public static string TimeFormatMSecondsToTwoOfDHMS(long ms)
    {
        int seconds = (int)TimeFormat(ms);
        return TimeFormatSecondsToTwoOfDHMS(seconds);
    }

    public static string TimeFormatSecondsToTwoOfDHMS(int seconds)
    {
        string timeStr = "";
        if (seconds >= 60)
        {
            //超过一分钟
            int hours = seconds / (60 * 60);
            if (hours >= 24)
            {
                int days = hours / 24;
                if (days >= 1)
                {
                    //超过一天 
                    string dayStr = LanguageHelper.GetValues("Desc_Time_Day_Para", days.ToString());
                    int hour = hours % 24;
                    if (hour > 0)
                    {
                        timeStr = dayStr + " " + LanguageHelper.GetValues("Desc_Time_Hour_Para", hour.ToString());
                    }
                    else
                    {
                        timeStr = dayStr;
                    }
                }
            }
            else
            {
                if (hours >= 1)
                {
                    //超过一小时
                    string hourStr = LanguageHelper.GetValues("Desc_Time_Hour_Para", hours.ToString());
                    int minutes = seconds % (60 * 60) / 60;
                    if (minutes > 0)
                    {
                        timeStr = hourStr + " " + LanguageHelper.GetValues("Desc_Time_Minute_Para", minutes.ToString());
                    }
                    else
                    {
                        timeStr = hourStr;
                    }
                }
                else
                {
                    //不超过一小时
                    int minutes = seconds / 60;
                    int second = seconds % 60;
                    string minStr = LanguageHelper.GetValues("Desc_Time_Minute_Para", minutes.ToString());
                    if (second > 0)
                    {
                        timeStr = minStr + " " + LanguageHelper.GetValues("Desc_Time_Second_Para", second.ToString());
                    }
                    else
                    {
                        timeStr = minStr;
                    }
                }

            }
        }
        else
        {
            //秒
            timeStr = LanguageHelper.GetValues("Desc_Time_Second_Para", seconds.ToString());
        }

        return timeStr;
    }

    public static string GetDayOfWeekLanguage(DayOfWeek dayOfWeek)
    {
        string str = dayOfWeek switch
        {
            DayOfWeek.Monday => LanguageHelper.GetValue("Desc_Event_Calendar_Mon"),
            DayOfWeek.Tuesday => LanguageHelper.GetValue("Desc_Event_Calendar_Tue"),
            DayOfWeek.Wednesday => LanguageHelper.GetValue("Desc_Event_Calendar_Wed"),
            DayOfWeek.Thursday => LanguageHelper.GetValue("Desc_Event_Calendar_Thu"),
            DayOfWeek.Friday => LanguageHelper.GetValue("Desc_Event_Calendar_Fri"),
            DayOfWeek.Saturday => LanguageHelper.GetValue("Desc_Event_Calendar_Sat"),
            _ => LanguageHelper.GetValue("Desc_Event_Calendar_Sun"),
        };
        return str;
    }

    /// <summary>
    /// 得到时间格式0d 00h 00m 00s，如果是0直接舍去
    /// </summary>
    /// <param name="mseconds"></param>
    /// <returns></returns>
    public static string TimeFormatTimeByMSeconds(long mseconds)
    {
        int seconds = (int)TimeFormat(mseconds);
        return TimeFormatTimeBySeconds(seconds);
    }

    public static string TimeFormatTimeBySeconds(int seconds)
    {
        bool isDays = false;
        TimeSpan timeSpan = new TimeSpan(0, 0, seconds);
        System.Text.StringBuilder builder = new System.Text.StringBuilder();
        if (0 < timeSpan.Days)
        {
            isDays = true;
            builder.Append(LanguageHelper.GetValues("Desc_Time_Day_Para", timeSpan.Days.ToString("")));
        }
        if (0 < timeSpan.Seconds)
        {
            if (isDays)
            {
                builder.Append(" ");
            }
            builder.Append(LanguageHelper.GetValues("Desc_Time_Hour_Para", timeSpan.Hours.ToString("")));
            builder.Append(LanguageHelper.GetValues("Desc_Time_Minute_Para", timeSpan.Minutes.ToString("")));
            builder.Append(LanguageHelper.GetValues("Desc_Time_Second_Para", timeSpan.Seconds.ToString("")));
        }
        else if (0 < timeSpan.Minutes)
        {
            if (isDays)
            {
                builder.Append(" ");
            }
            builder.Append(LanguageHelper.GetValues("Desc_Time_Hour_Para", timeSpan.Hours.ToString("")));
            builder.Append(LanguageHelper.GetValues("Desc_Time_Minute_Para", timeSpan.Minutes.ToString("")));
        }
        else if (0 < timeSpan.Hours)
        {
            if (isDays)
            {
                builder.Append(" ");
            }
            builder.Append(LanguageHelper.GetValues("Desc_Time_Hour_Para", timeSpan.Hours.ToString("")));
        }
        return builder.ToString();
    }
    /// <summary>
    /// Turns a float (expressed in seconds) into a string displaying hours, minutes, seconds and hundredths optionnally
    /// </summary>
    /// <param name="t"></param>
    /// <param name="displayHours"></param>
    /// <param name="displayMinutes"></param>
    /// <param name="displaySeconds"></param>
    /// <param name="displayHundredths"></param>
    /// <returns></returns>
    public static string FloatToTimeString(float t, bool displayHours = false, bool displayMinutes = true, bool displaySeconds = true, bool displayMilliseconds = false)
    {
        int intTime = (int)t;
        int hours = intTime / 3600;
        int minutes = intTime / 60;
        int seconds = intTime % 60;
        int milliseconds = Mathf.FloorToInt((t * 1000) % 1000);

        if (displayHours && displayMinutes && displaySeconds && displayMilliseconds)
        {
            return string.Format("{0:00}:{1:00}:{2:00}.{3:D3}", hours, minutes, seconds, milliseconds);
        }
        if (!displayHours && displayMinutes && displaySeconds && displayMilliseconds)
        {
            return string.Format("{0:00}:{1:00}.{2:D3}", minutes, seconds, milliseconds);
        }
        if (!displayHours && !displayMinutes && displaySeconds && displayMilliseconds)
        {
            return string.Format("{0:D2}.{1:D3}", seconds, milliseconds);
        }
        if (!displayHours && !displayMinutes && displaySeconds && !displayMilliseconds)
        {
            return string.Format("{0:00}", seconds);
        }
        if (displayHours && displayMinutes && displaySeconds && !displayMilliseconds)
        {
            return string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
        }
        if (!displayHours && displayMinutes && displaySeconds && !displayMilliseconds)
        {
            return string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        return null;

    }
}
