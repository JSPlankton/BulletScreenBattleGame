
/**
 *	路径工具类 
 *	author:zhaojinlun
 *	date:2021年4月15日
 *	copyright: fungather.net
 ***/
using System;
using System.IO;
using System.Text;
using UnityEngine;

public class PathUtil
{



    /// <summary>
    /// 获取当前平台下assets的全路径 (替换前缀)
    /// </summary>
    /// <param name="assetPath">新路径名</param>
    /// <param name="preStr">替换前缀</param>
    /// <returns></returns>
    public static string ToFullAssetPath(string assetPath, string preStr)
    {
        if (assetPath.StartsWith(preStr))
        {
            return Application.dataPath + assetPath.Substring(preStr.Length);
        }
        return assetPath;
    }

    /// <summary>
    /// 获取当前平台下的assets的全路径（无需替换前缀）
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string ToFullAssetPath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            LogUtil.Error("path is null");
            return path;
        }
        return Application.dataPath + "/" + path;
    }

    /// <summary>
    /// 获取Bundle模式下的assets的全路径
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string ToFullBundlePath(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            LogUtil.Error("path is null");
            return path;
        }
        return BuildConfig.Root + "/" + path;
    }

    public static string ToUnixPath(string fullPath)
    {
        if (string.IsNullOrEmpty(fullPath))
        {
            return String.Empty;
        }
        return fullPath.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
    }

    public static string ToWindowPath(string fullPath)
    {
        if (string.IsNullOrEmpty(fullPath))
        {
            return string.Empty;
        }
        return fullPath.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);
    }

    /// <summary>
    /// 移除路径所带的扩展名
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string RemoveExtension(string path)
    {
        return Path.HasExtension(path) ? path.Substring(0, path.LastIndexOf(".")) : path;
    }

    public static string ToAssetPath(string path)
    {
        return path.Replace(Application.dataPath, AssetConfig.ASSETS);
    }

    /// <summary>
    /// 资源全路径 -> Bundles/BundlesSplits/Assets资源相对路径
    /// </summary>
    /// <param name="assetPath"></param>
    /// <returns></returns>
    public static string AssetPathToAssetName(string assetPath)
    {
        if (assetPath.StartsWith(AssetConfig.ASSETS_BUNDLES_PATH + "/"))
            return assetPath.Remove(0, AssetConfig.ASSETS_BUNDLES_PATH.Length + 1);
        if (assetPath.StartsWith(AssetConfig.ASSETS_BUNDLES_SPLIT_PATH + "/"))
            return assetPath.Remove(0, AssetConfig.ASSETS_BUNDLES_SPLIT_PATH.Length + 1);
        if (assetPath.StartsWith(AssetConfig.ASSETS + "/"))
            return assetPath.Remove(0, AssetConfig.ASSETS.Length + 1);
        return assetPath;
    }

    /// <summary>
    /// 资源全路径 -> Bundles/Assets资源相对路径
    /// Assets/Bundle/XXX.ab ==> XXX.ab
    /// Assets/BundlesSplits/XXX.ab => BundlesSplits/XXX.ab
    /// Assets/XXX.ab => XXX.ab
    /// </summary>
    /// <param name="assetPath"></param>
    /// <returns></returns>
    public static string AssetPathToBundleName(string assetPath)
    {
        if (assetPath.StartsWith(AssetConfig.ASSETS_BUNDLES_PATH + "/"))
            return assetPath.Remove(0, AssetConfig.ASSETS_BUNDLES_PATH.Length + 1);
        if (assetPath.StartsWith(AssetConfig.ASSETS + "/"))
            return assetPath.Remove(0, AssetConfig.ASSETS.Length + 1);
        return assetPath;
    }

    public static bool IsBundleAssetPath(string assetPath)
    {
        return assetPath.StartsWith(AssetConfig.ASSETS_BUNDLES_PATH + "/");
    }
    public static bool IsBundleSplitAssetPath(string assetPath)
    {
        return assetPath.StartsWith(AssetConfig.ASSETS_BUNDLES_SPLIT_PATH + "/");
    }

    public static string BundleSplitNameForAssetPath(string assetPath)
    {
        if (!assetPath.StartsWith(AssetConfig.ASSETS_BUNDLES_SPLIT_PATH + "/"))
            return "";
        assetPath = assetPath.Remove(0, AssetConfig.ASSETS_BUNDLES_SPLIT_PATH.Length + 1);
        if (assetPath.IndexOf("/") >= 0)
        {
            return assetPath.Substring(0, assetPath.IndexOf("/"));
        }
        else
        {
            return assetPath;
        }

    }


    static StringBuilder builder = new StringBuilder(1024, 4096);
    /// <summary>
    /// 链接一个不以"/"结尾的路径
    /// </summary>
    /// <param name="paths"></param>
    /// <returns></returns>
    public static string CombinePath(params string[] paths)
    {
        if (paths == null)
        {
            LogUtil.Error("paths  is  null");
            return string.Empty;
        }
        if (builder.Length > 0)
        {
            LogUtil.Error("StringBuilder not released");
            return string.Empty;
        }
        int i = 0;
        do
        {
            builder.Append(paths[i++]);
            if (i < paths.Length)
            {
                builder.Append(Path.AltDirectorySeparatorChar);
            }
        } while (i < paths.Length);
        string str = builder.ToString();
        builder.Clear();
        return str;
    }

    /// <summary>
    /// 根据路径信息, 获得父层级路径, 返回值Unix格式，以/结尾
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetParentDirectoryNameUnix(string path)
    {
        var parentPath = ToUnixPath(path);
        if (parentPath.EndsWith(Path.AltDirectorySeparatorChar.ToString()))
        {
            //文件夹路径, 去除最后一个
            parentPath = parentPath.TrimEnd(Path.AltDirectorySeparatorChar);
        }
        if (parentPath.LastIndexOf(Path.AltDirectorySeparatorChar) == -1)
        {
            //根路径
            return Path.AltDirectorySeparatorChar.ToString();
        }
        else
        {
            return parentPath.Substring(0, parentPath.LastIndexOf(Path.AltDirectorySeparatorChar) + 1);
        }
    }
}
