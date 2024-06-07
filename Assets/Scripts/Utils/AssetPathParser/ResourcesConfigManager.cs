using Managers;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Profiling;
using WebSocketSharp;

public static class ResourcesConfigManager
{
    public static AssetsLoadType assetsloadType = AssetsLoadType.Resources;
    public const string c_ManifestFileName = "ResourcesManifest";
    public const string c_PathKey = "Path";

    static DataTable s_config;
    static Dictionary<string, string> s_addtionalConfig = new Dictionary<string, string>();
    static bool s_isInit = false;

    static void Initialize()
    {
        s_isInit = true;
#if UNITY_EDITOR
        assetsloadType = AssetsLoadType.Resources;
#else
        assetsloadType = AssetsLoadType.AssetBundle;
#endif
        LoadResourceConfig();
    }

    public static void ClearConfig()
    {
        s_isInit = false;
    }

    public static bool GetIsExitRes(string resName)
    {
        if (string.IsNullOrEmpty(resName))
        {
            return false;
        }
        resName = resName.ToLower();

        if (!s_isInit || s_config == null)
        {
            Initialize();
        }

        return (s_config.ContainsKey(resName) || s_addtionalConfig.ContainsKey(resName));
    }

    public static string GetResourcePath(string bundleName)
    {
        bundleName = bundleName.ToLower();

        if (!s_isInit)
        {
            Initialize();
        }
        var ret = "";
        if (s_config.ContainsKey(bundleName))
        {
            ret = s_config[bundleName].GetString(c_PathKey);
        }
        if (s_addtionalConfig.ContainsKey(bundleName))
        {
            ret = s_addtionalConfig[bundleName];
        }
        if (ret == "" || ret == null)
            throw new Exception("RecourcesConfigManager can't find ->" + bundleName + "<-");
        return ret;
    }
    /// <summary>
    /// 根据AssetsLoadType获取加载路径
    /// </summary>
    /// <param name="bundleName"></param>
    /// <returns></returns>
    public static string GetLoadPath(string name)
    {
        string path = GetResourcePath(name);
        if (assetsloadType == AssetsLoadType.Resources)
            return path;
        else
        {
            return path;
        }
    }

    public static void LoadResourceConfig()
    {
        Profiler.BeginSample("LoadResourceConfig");
        string data = "";
        if (assetsloadType == AssetsLoadType.Resources)
        {
            data = ResourceIOTool.ReadStringByResource($"{AssetConfig.SPECIAL}/{c_ManifestFileName}.txt");
        }
        else
        {
            string configPath = $"{AssetConfig.SPECIAL}/{c_ManifestFileName}.txt";
            UnityEngine.Object obj = AssetManager.Instance.Load(configPath, typeof(TextAsset));
            TextAsset text = (TextAsset)obj;
            data = text.text;
        }
        s_config = DataTable.Analysis(data);
        Profiler.EndSample();
    }

    public static void ResetAddtiional()
    {
        if (s_addtionalConfig != null)
        {
            s_addtionalConfig.Clear();
        }
    }

    /// <summary>
    /// 将额外资源列表添加到ResourceConfigManger中,
    /// 注意，如果出现同名，额外资源将覆盖原始资源信息.
    /// </summary>
    /// <param name="fillAssetList"></param>
    /// <param name="byPassPrefix"></param>
    public static void FillDownloadManagerAssetData(List<string> fillAssetList, string byPassPrefix = "")
    {
        if (!s_isInit)
        {
            Initialize();
        }
        foreach (var assetPath in fillAssetList)
        {
            if (assetPath.EndsWith(FileExtensionUtil.png))
            {
                continue;
            }
            var realPath = assetPath.ToLower();
            var assetName = Path.GetFileNameWithoutExtension(realPath);
            if (assetName.IsNullOrEmpty())
            {
                continue;
            }
            var prefixcheckPath = realPath;
            if (!byPassPrefix.IsNullOrEmpty() && prefixcheckPath.StartsWith(byPassPrefix))
            {
                prefixcheckPath = prefixcheckPath.Substring(byPassPrefix.Length + 1);
            }
            if (prefixcheckPath.StartsWith("prefabs") ||
                prefixcheckPath.StartsWith("music") ||
                prefixcheckPath.StartsWith("dynamic") ||
                prefixcheckPath.StartsWith("extrapack"))

            {
                if (realPath.Contains("dynamic"))
                {
                    if (!realPath.Contains("dynamic/prefabs") && !realPath.Contains("dynamic/music"))
                    {
                        continue;
                    }
                }
                if (realPath.Contains("extraPack"))
                {
                    if (!realPath.Contains("extraPack/prefabs") && !realPath.Contains("extraPack/music"))
                    {
                        continue;
                    }
                }
                var path = Path.ChangeExtension(assetPath, null);
                if (s_config.ContainsKey(assetName.ToLower()))
                {
                    LogUtil.Warning($"FillDownloadManagerAssetData warning 重名覆盖!! {assetName} {path}");
                }
                if (s_addtionalConfig.ContainsKey(assetName))
                {
                    LogUtil.Warning($"FillDownloadManagerAssetData warning 重名覆盖!! {assetName} {path}");
                }
                // if (DownloadManager3.LogDetails)
                //     LogUtil.Log($"FillDownloadManagerAssetData Add {assetName} {path}");
                s_addtionalConfig[assetName] = path;
            }


        }
    }

#if UNITY_EDITOR

    public const string c_ResourceParentPath = "/Bundles/";
    public const string c_ResourceManifestPath = "Assets/Bundles/Special/";
    public const string c_MainKey = "Res";
    static int direIndex = 0;

    public static bool GetIsExistResources()
    {
        string resourcePath = Application.dataPath + c_ResourceParentPath;
        return Directory.Exists(resourcePath);
    }

    public static void CreateResourcesConfig()
    {
        string content = DataTable.Serialize(GenerateResourcesConfig());
        string path = c_ResourceManifestPath + c_ManifestFileName + ".txt";

        ResourceIOTool.WriteStringByFile(path, content);
    }

    /// <summary>
    /// 生成资源清单路径，仅在Editor下可以调用
    /// </summary>
    /// <returns></returns>
    public static DataTable GenerateResourcesConfig()
    {
        DataTable data = new DataTable();

        data.TableKeys.Add(c_MainKey);

        data.TableKeys.Add(c_PathKey);
        // data.SetDefault(c_PathKey, "资源相对路径");
        // data.SetFieldType(c_PathKey, FieldType.String, null);

        string resourcePath = Application.dataPath + c_ResourceParentPath;
        direIndex = resourcePath.LastIndexOf(c_ResourceParentPath);
        direIndex += c_ResourceParentPath.Length;

        RecursionAddResouces(data, resourcePath);

        return data;
    }

    static void RecursionAddResouces(DataTable data, string path)
    {
        if (!Directory.Exists(path))
        {
            FileUtil.CreateFilePath(path);
        }

        string[] dires = Directory.GetDirectories(path);

        for (int i = 0; i < dires.Length; i++)
        {
            if (dires[i].Contains("Prefabs") || dires[i].Contains("Music")
                                             || dires[i].Contains("Dynamic")
                                             || dires[i].Contains("ExtraPack")
               )
            {
                RecursionAddResouces(data, dires[i]);
            }

        }

        string[] files = Directory.GetFiles(path);

        for (int i = 0; i < files.Length; i++)
        {
            string fileName = FileUtil.RemoveExpandName(FileUtil.GetFileNameByPath(files[i]));
            string relativePath = files[i].Substring(direIndex);
            if (relativePath.EndsWith(".meta") || relativePath.EndsWith(".DS_Store") || relativePath.EndsWith(".git"))
                continue;
            else
            {
                relativePath = FileUtil.RemoveExpandName(relativePath).Replace("\\", "/");
                if (relativePath.Contains("Dynamic"))
                {
                    if (!relativePath.Contains("Dynamic/Prefabs") && !relativePath.Contains("Dynamic/Music"))
                    {
                        continue;
                    }
                }
                if (relativePath.Contains("ExtraPack"))
                {
                    if (!relativePath.Contains("ExtraPack/Prefabs") && !relativePath.Contains("ExtraPack/Music"))
                    {
                        continue;
                    }
                }
                SingleData sd = new SingleData();
                sd.Add(c_MainKey, fileName.ToLower());
                sd.Add(c_PathKey, relativePath.ToLower());

                if (fileName.EndsWith(" "))
                {
                    LogUtil.Warning("文件名尾部中有空格！ ->" + fileName + "<-");
                }
                else
                {
                    if (!data.ContainsKey(fileName.ToLower()))
                    {
                        data.AddData(sd);
                    }
                    else
                    {
                        LogUtil.Warning("GenerateResourcesConfig error 存在重名文件！" + relativePath);
                    }
                }
            }
        }
    }
#endif
}

#region 枚举
public enum AssetsLoadType
{
    /// <summary>
    /// Resources文件夹下的资源
    /// </summary>
    Resources = 0,
    /// <summary>
    ///AssetBundle下的资源
    /// </summary>
    AssetBundle = 1,

}

public enum FieldType
{
    String,
    Bool,
    Int,
    Float,
    Vector2,
    Vector3,
    Color,
    Enum,

    StringArray,
    IntArray,
    FloatArray,
    BoolArray,
    Vector2Array,
    Vector3Array,
}
#endregion