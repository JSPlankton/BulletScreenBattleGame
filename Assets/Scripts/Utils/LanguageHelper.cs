using System;
using System.Collections.Generic;

public class LanguageHelper
{
    public LanguageHelper()
    {
    }
    /// <summary>
    /// 获取多语言词条
    /// </summary>
    /// 词条id
    /// <param name="key"></param>
    /// <param name="isReplaceSpace"></param>
    /// <returns></returns>
    public static string GetValue(string key, bool isReplaceSpace = true)
    {
        // 使用LocalizationManager代替TxtParser
        string content = key;
        //string content = TxtParser.GetValue(key);
        if (string.IsNullOrEmpty(content))
        {
            return key;
        }
        //content.Replace("\\n", "\n");
        if (isReplaceSpace)
            content = ReplaceBlankChar(content);

        return content;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetValues(string key, params object[] args)
    {
        // 使用LocalizationManager代替TxtParser 
        string content = key;
        //string content = TxtParser.GetValue(key);
        if (string.IsNullOrEmpty(content))
        {
            return key;
        }
        //int count = args.Length;
        //content.Replace("\\n", "\n");
        //for (int i = 0; i < count; i++)
        //{
        //    string rep = "{" + i.ToString() + "}";
        //    content = content.Replace(rep, (string)args[i]);

        //}
        content = ReplaceBlankChar(content);

        return content;
    }

    public static string GetValueByNameParams(string name, string nameParams, bool isReplaceSpace = true)
    {
        string[] vec = nameParams.Split('|');
        for (int i = 0; i < vec.Length; i++)
        {
            string tempStr = vec[i];
            string[] tempVec = tempStr.Split(';');
            if (tempVec.Length >= 2)
            {
                switch (tempVec[0])
                {
                    case "1"://多语言，词条
                        vec[i] = LanguageHelper.GetValue(tempVec[1]);
                        break;
                    case "2"://原值
                        long temp = 0;
                        if (!tempVec[1].Contains("."))//有小数时就不能用千位分隔符了
                        {
                            temp = tempVec[1].ToLong();
                        }
                        if (0 >= temp)
                        {
                            vec[i] = tempVec[1];
                        }
                        else
                        {
                            vec[i] = temp.ToString("N0");
                        }
                        break;
                    case "3"://时间格式
                        vec[i] = DateUtil.TimeFormatSeconds(tempVec[1].ToLong());
                        break;
                    default:
                        vec[i] = tempVec[1];
                        break;
                }
            }
        }
        List<string> msgList = new List<string>();
        for (int k = 0; k < vec.Length; k++)
        {
            msgList.Add(vec[k]);
        }
        return LanguageHelper.GetValueByParaList(name, msgList, isReplaceSpace);
    }

    public static string GetValueByParaList(List<string> paraList, bool isReplaceSpace = true)
    {
        if (0 >= paraList.Count)
        {
            return "";
        }
        string key = paraList[0];
        paraList.RemoveAt(0);
        return GetValueByParaList(key, paraList, isReplaceSpace);
    }

    public static string GetValueByParaList(string key, List<string> paraList, bool isReplaceSpace = true)
    {
        string desc = "";
        if (string.IsNullOrEmpty(key))
        {
            return desc;
        }
        int count = paraList.Count;
        if (0 >= count)
        {
            desc = LanguageHelper.GetValue(key);
        }
        else if (0 < count)
        {
            desc = LanguageHelper.GetValues(key, paraList.ToArray());
        }

        if (isReplaceSpace)
            desc = ReplaceBlankChar(desc);

        return desc;
    }
    
    private static List<string> needReplaceBlankLangs = new List<string>(){
        "zh_CN", "zh_TW", "ja", "ko"
    };

    private static string ReplaceBlankChar(string text)
    {
        return needReplaceBlankLangs.Contains(null)
            ? text.Replace(" ", "\u00A0")
            : text;
    }
    
}
