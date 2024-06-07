using System;
using System.Collections.Generic;
using Managers;

public class ExceptionReporter
{
    private const int SAME_EXCEPTION_REPORT_LIMIT = 10;
    private static readonly Dictionary<string, int> reportedExceptionCount = new Dictionary<string, int>();
    private static readonly Dictionary<Type, string> filteredExceptions = new Dictionary<Type, string>
    {
        // 添加需要过滤的异常类型和对应的异常信息
        // { typeof(FilteredExceptionType1), "FilteredExceptionType1 Message" },
    };

    public static void ReportException(string message, Exception e)
    {
        string exceptionKey = e.ToString(); // 生成异常的唯一标识
        ReportException(exceptionKey, message, e);
    }

    public static void ReportException(Exception e)
    {
        string exceptionKey = e.ToString(); // 生成异常的唯一标识
        ReportException(exceptionKey, null, e);
    }

    private static void ReportException(string exceptionKey, string message, Exception e)
    {
        if (IsExceptionFiltered(e))
        {
            return; // 过滤掉需要过滤的异常（根据异常类型和异常信息匹配）
        }

        if (!reportedExceptionCount.TryGetValue(exceptionKey, out int count) || count < SAME_EXCEPTION_REPORT_LIMIT)
        {
            // 上报firebase
            if (message == null)
            {
                LogUtil.Log("上报firebase..." + e);
                // SDKManager.Instance.Firebase.CrashlyticsLogException(e);
            }
            else
            {
                LogUtil.Log("上报firebase..." + message + "\n" + e);
                // SDKManager.Instance.Firebase.CrashlyticsLogException(message, e);
            }

            if (reportedExceptionCount.ContainsKey(exceptionKey))
            {
                reportedExceptionCount[exceptionKey] += 1;
            }
            else
            {
                reportedExceptionCount[exceptionKey] = 1;
            }
        }
    }

    private static bool IsExceptionFiltered(Exception e)
    {
        if (filteredExceptions == null || filteredExceptions.Count == 0)
            return false;
        
        Type exceptionType = e.GetType();
        if (filteredExceptions.TryGetValue(exceptionType, out var exception))
        {
            return exception == null || exception.Equals(e.Message);
        }
        return false;
    }

    // 添加需要过滤的异常类型及对应的异常信息
    public static void AddFilteredException(Type exceptionType, string expectedMessage)
    {
        filteredExceptions[exceptionType] = expectedMessage;
    }

    // 移除需要过滤的异常类型
    public static void RemoveFilteredException(Type exceptionType)
    {
        filteredExceptions.Remove(exceptionType);
    }
}
