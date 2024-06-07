using System.IO;
using UnityEngine;


class LoadPathUtil
{
    /// <summary>
    /// 返回 /Assets/Bundles/{key}
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetPathForLocalFile(string key)
    {
        string dataPath = Application.dataPath + "/";
        string projectPath = dataPath.Substring(0, dataPath.Length - 7);
        string path = Application.dataPath + "/Bundles/" + key;
        if (!File.Exists(path) && File.Exists(Application.dataPath + "/" + AssetConfig.BUNDLE_SPLIT_PATH + "/" + key))
        {
            path = Application.dataPath + "/" + AssetConfig.BUNDLE_SPLIT_PATH + "/" + key;
        }
        if (!File.Exists(path) && File.Exists(Application.dataPath + "/" + key))
        {
            path = Application.dataPath + "/" + key;
        }
        return path.Substring(projectPath.Length);
    }
}
