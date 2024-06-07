using System;
using System.Collections.Generic;

public class AssetBundleData
{
    //读取的AB配置文件
    public static AssetBundleConfigList abConfig;

    //Asset-Bundle 缓存结构
    public static Dictionary<string, string> m_ABPathDic;

    //Bundle - FullPath 缓存结构
    public static Dictionary<string, string> m_BundleFullPathDic;

    public static void InitData()
    {
        abConfig = new AssetBundleConfigList();
        m_ABPathDic = new Dictionary<string, string>();
        m_BundleFullPathDic = new Dictionary<string, string>();
    }


    /// <summary>
    /// 根据Asset名称获取Bundle
    /// </summary>
    /// <param name="assetName"></param>
    /// <returns></returns>
    public static string GetBundleName(string assetName)
    {
        if (string.IsNullOrEmpty(assetName))
        {
            return String.Empty;
        }

        string bundleName = string.Empty;
        m_ABPathDic.TryGetValue(assetName, out bundleName);
        return bundleName;
    }
}