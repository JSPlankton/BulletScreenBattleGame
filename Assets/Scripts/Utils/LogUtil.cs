/**
 *	自定义日志类，实现不同格式日志的显示 
 *	author:zhaojinlun
 *	date:2021年4月13日
 *	copyright: fungather.net
 ***/
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public class CustomLogHandler : ILogHandler
{
    private static ILogHandler m_DefaultLogHandler = Debug.unityLogger.logHandler;
    public CustomLogHandler()
    {
    }
    [HideInCallstack]
    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        if (LogUtil.CanPrintLog())
            m_DefaultLogHandler.LogFormat(logType, context, format, args);
    }
    [HideInCallstack]
    public void LogException(Exception exception, UnityEngine.Object context)
    {
        if (LogUtil.CanPrintLog())
            m_DefaultLogHandler.LogException(exception, context);
    }
}


public class CustomUnityLogHandler : ILogHandler
{
    private static ILogHandler m_DefaultLogHandler = Debug.unityLogger.logHandler;
    public CustomUnityLogHandler()
    {
    }
    [HideInCallstack]
    public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args)
    {
        if (LogUtil.CanPrintSystemLog(logType))
            m_DefaultLogHandler.LogFormat(logType, context, format, args);
    }
    [HideInCallstack]
    public void LogException(Exception exception, UnityEngine.Object context)
    {
        if (LogUtil.CanPrintSystemLog(LogType.Exception))
            m_DefaultLogHandler.LogException(exception, context);
        //if (LogUtil.CanFireBaseSystemException())
        //{
        //    //if (SDKManager.Instance != null && SDKManager.Instance.Firebase.IsReady && Crashlytics.IsCrashlyticsCollectionEnabled)
        //    //{
        //    Crashlytics.LogException(exception);
        //    //}
        //}
    }
}

public class LogUtil
{
    private static Logger userlogger = new Logger(new CustomLogHandler());
    private static ILogHandler systemlogHandler = new CustomUnityLogHandler();

    public static List<string> LogByGameUids = new List<string>()
    {
        "78418207000001",
        "15885000006",
        "1816138425000001",
        "513251262000007",
        "263828208000002",
    };

    /// <summary>
    /// 静态初始化，重置Unitylogger信息。
    /// </summary>
    public static void InitLoggerUtils()
    {
        Debug.unityLogger.logHandler = systemlogHandler;
        LogKeyValue(GAMESTART, "Start");
        SetupLogFilter(LogType.Log);
    }

    public static void SetupLogFilter(LogType logType)
    {
        userlogger.filterLogType = logType;
    }

    /// <summary>
    /// 注意, LogUtil部分函数使用ConditionalAttribute, 如果修改CanPrintLog里面的返回值, 需要同步修改ConditionalAttribute的值
    /// </summary>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CanPrintLog()
    {
#if UNITY_EDITOR || DEVELOP || DEBUG_INFO_OUTPUT
        return true;
#else
        return false;
        //string gameUid = PlayerPrefs.GetString(SettingKeys.GAME_UID, "");
        //if (string.IsNullOrEmpty(gameUid) ||
        //    !LogByGameUids.Contains(gameUid))
        //    return false;
        //return true;
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CanPrintSystemLog(LogType logType)
    {
#if UNITY_EDITOR || DEVELOP || DEBUG_INFO_OUTPUT
        return true;
#else
        if (logType == LogType.Exception)
            return true;
        return false;
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CanFireBaseSystemException()
    {
#if UNITY_EDITOR || DEVELOP || DEBUG_INFO_OUTPUT
        return false;
#else
        return true;
#endif
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool CanFireBaseKeyValueLog()
    {
#if UNITY_EDITOR || DEVELOP || DEBUG_INFO_OUTPUT
        return false;
#else
        return true;
#endif
    }

    [HideInCallstack, System.Diagnostics.Conditional("UNITY_EDITOR"), System.Diagnostics.Conditional("DEVELOP"), System.Diagnostics.Conditional("DEBUG_INFO_OUTPUT")]
    public static void LogFormat(string message, params object[] args)
    {
        userlogger.LogFormat(LogType.Log, message, args);
    }

    [HideInCallstack, System.Diagnostics.Conditional("UNITY_EDITOR"), System.Diagnostics.Conditional("DEVELOP"), System.Diagnostics.Conditional("DEBUG_INFO_OUTPUT")]
    public static void LogFormat(UnityEngine.Object context, string format, params object[] args)
    {
        userlogger.LogFormat(LogType.Log, context, format, args);
    }

    [HideInCallstack, System.Diagnostics.Conditional("UNITY_EDITOR"), System.Diagnostics.Conditional("DEVELOP"), System.Diagnostics.Conditional("DEBUG_INFO_OUTPUT")]
    public static void Log(string message)
    {
        userlogger.Log(message);
    }
    [HideInCallstack, System.Diagnostics.Conditional("UNITY_EDITOR"), System.Diagnostics.Conditional("DEVELOP"), System.Diagnostics.Conditional("DEBUG_INFO_OUTPUT")]
    public static void Log(object message)
    {
        userlogger.Log(message);
    }

    [HideInCallstack, System.Diagnostics.Conditional("UNITY_EDITOR"), System.Diagnostics.Conditional("DEVELOP"), System.Diagnostics.Conditional("DEBUG_INFO_OUTPUT")]
    public static void Log(string message, params object[] args)
    {
        userlogger.LogFormat(LogType.Log, message, args);
    }

    [HideInCallstack, System.Diagnostics.Conditional("UNITY_EDITOR"), System.Diagnostics.Conditional("DEVELOP"), System.Diagnostics.Conditional("DEBUG_INFO_OUTPUT")]
    public static void Log(UnityEngine.Object context, string message, params object[] args)
    {
        userlogger.LogFormat(LogType.Log, context, message, args);
    }
    [HideInCallstack]
    public static void Exception(Exception e)
    {
        ExceptionReporter.ReportException(e);
        userlogger.LogException(e);
    }
    [HideInCallstack]
    public static void Exception(string message, Exception e)
    {
        ExceptionReporter.ReportException(message, e);
        userlogger.LogFormat(LogType.Exception, message + "\n" + e);
    }

    [HideInCallstack, System.Diagnostics.Conditional("UNITY_EDITOR"), System.Diagnostics.Conditional("DEVELOP"), System.Diagnostics.Conditional("DEBUG_INFO_OUTPUT")]
    public static void Error(string message)
    {
        userlogger.LogError("Error", message);
    }
    [HideInCallstack, System.Diagnostics.Conditional("UNITY_EDITOR"), System.Diagnostics.Conditional("DEVELOP"), System.Diagnostics.Conditional("DEBUG_INFO_OUTPUT")]
    public static void Error(string format, params object[] args)
    {
        userlogger.LogFormat(LogType.Error, format, args);
    }

    [HideInCallstack, System.Diagnostics.Conditional("UNITY_EDITOR"), System.Diagnostics.Conditional("DEVELOP"), System.Diagnostics.Conditional("DEBUG_INFO_OUTPUT")]
    public static void Error(UnityEngine.Object context, string format, params object[] args)
    {
        userlogger.LogFormat(LogType.Error, context, format, args);
    }
    [HideInCallstack, System.Diagnostics.Conditional("UNITY_EDITOR"), System.Diagnostics.Conditional("DEVELOP"), System.Diagnostics.Conditional("DEBUG_INFO_OUTPUT")]
    public static void Warning(string format, params object[] args)
    {
        userlogger.LogFormat(LogType.Warning, format, args);
    }
    [HideInCallstack, System.Diagnostics.Conditional("UNITY_EDITOR"), System.Diagnostics.Conditional("DEVELOP"), System.Diagnostics.Conditional("DEBUG_INFO_OUTPUT")]

    public static void Warning(UnityEngine.Object context, string format, params object[] args)
    {
        userlogger.LogFormat(LogType.Warning, context, format, args);
    }

    public static void Send(string eventName, long duration, int callIntervalCount)
    {
        // if (GameMessage.CocLog.Instance == null)
        //     return;
        //
        // Sfs2X.Entities.Data.ISFSObject sfsObj = Sfs2X.Entities.Data.SFSObject.NewInstance();
        // sfsObj.PutUtfString("logType", eventName);
        // sfsObj.PutLong("duration", duration);
        // sfsObj.PutInt("callIntervalCount", callIntervalCount);
        // GameMessage.CocLog.Instance.Send(sfsObj);

    }

    public static void GmSend(string eventName, long duration, int callIntervalCount)
    {

        // if (Controllers.Player == null || Controllers.Player.SelfLordData == null ||
        //     !Controllers.Player.SelfLordData.IsHaveRoleByRoleType(RoleType.Role_Smoke))
        // {
        //     return;
        // }

        Send(eventName, duration, callIntervalCount);
    }

    public const string OPENUIW = "Open_UI_W";
    public const string OPENUIB = "Open_UI_B";
    public const string INPUTGO = "Input_Go";
    public const string RELOAD = "Reload";
    public const string GAMESTART = "GameStart";
    public const string SCENECHANGE = "Scene_Change";
    public const string APPFOCUS = "APP_Fucus";
    public const string APPAUSE = "APP_Pause";
    public const string CMDHANDLE = "Cmd_Handle";
    public const string CMDSEND = "Cmd_Send";
    public const string LONGCMDHANDLE = "Long_Cmd_Handle";
    public static void LogKeyValue(string key, string value)
    {
        // if (CanFireBaseKeyValueLog() && SDKManager.HasInstance) //avoid create sdkmanager
        // {
        //     //Debug.Log($"LogKeyValue {key}, {Time.unscaledTime}_{value}");
        //     SDKManager.Instance.Firebase.CrashlyticsKeyValue(key, $"{Time.unscaledTime}_{value}");
        // }
    }

    public static void LogFBLog(string key, string data)
    {
        // if (CanFireBaseKeyValueLog() && SDKManager.HasInstance) //avoid create sdkmanager
        // {
        //     //Debug.Log($"LogData , {key}_{Time.unscaledTime}_{data}");
        //     SDKManager.Instance.Firebase.CrashlyticsLogStr($"{key}_{Time.unscaledTime}_{data}");
        // }
    }

}


/// <summary>
/// Unity外部调试时显示Log用。
/// 其中，开关 AppManager.Instance.UseDebugLog 由外部NativeProject在版本发布时控制。
/// </summary>
public class LogUtilForNativeProject
{
    public static void LogFormat(string message, params object[] args)
    {
        if (false)
        {
            Debug.LogFormat(message, args);
        }
        else
        {
            LogUtil.LogFormat(message, args);
        }
    }

    public static void Log(string message)
    {
        if (false)
        {
            Debug.Log(message);
        }
        else
        {
            LogUtil.Log(message);
        }
    }

    public static void Log(string message, params object[] args)
    {
        if (false)
        {
            Debug.LogFormat(message, args);
        }
        else
        {
            LogUtil.LogFormat(message, args);
        }
    }

    public static void Exception(System.Exception e)
    {
        if (false)
        {
            Debug.LogException(e);
        }
        else
        {
            LogUtil.Exception(e);
        }

    }

    public static void Error(string message)
    {
        if (false)
        {
            Debug.LogError(message);
        }
        else
        {
            LogUtil.Error(message);
        }
    }
    public static void Error(string format, params object[] args)
    {
        if (false)
        {
            Debug.LogErrorFormat(format, args);
        }
        else
        {
            LogUtil.Error(format, args);
        }
    }
    public static void Warning(string format, params object[] args)
    {
        if (false)
        {
            Debug.LogWarningFormat(format, args);
        }
        else
        {
            LogUtil.Warning(format, args);
        }
    }
}
