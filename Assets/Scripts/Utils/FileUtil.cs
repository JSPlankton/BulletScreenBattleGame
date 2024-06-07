using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class FileUtil : MonoBehaviour
{

    static string SREAMING_ASSETS_PATH = String.Format("{0}/", Application.streamingAssetsPath);

    /// <summary>
    /// 获取真实路径
    /// </summary>
    /// <returns>真实文件路径</returns>
    /// <param name="file">相对文件路径</param>
    public static string getPath(string file)
    {
        return SREAMING_ASSETS_PATH + file;
    }

    /// <summary>
    /// 获取本地路径
    /// </summary>
    /// <returns>The local path.</returns>
    /// <param name="file">File.</param>
    public static string getLocalPath(string file)
    {
        return file;
    }
    /// <summary>
    /// 是否存在此文件
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static bool Exists(string filePath)
    {
        return File.Exists(filePath);
    }

    /// <summary>
    /// 是否是指定扩展名的文件
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="extensions"></param>
    /// <returns></returns>
    public static bool isExtension(string filePath, params string[] extensions)
    {
        if (string.IsNullOrEmpty(filePath) || extensions.Length <= 0)
        {
            return false;
        }
        for (int index = 0; index < extensions.Length; ++index)
        {
            if (filePath.EndsWith(extensions[index], StringComparison.CurrentCultureIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 根据路径和扩展名获取到相关所有文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="fileList"></param>
    /// <param name="extensions"></param>
    public static void GetAllFiles(string path, List<string> fileList, params string[] extensions)
    {
        if (false == Directory.Exists(path))
        {
            return;
        }

        FileInfo[] allFiles = new DirectoryInfo(path).GetFiles("*", SearchOption.AllDirectories);
        for (int index = 0; index < allFiles.Length; ++index)
        {
            string fileFullName = PathUtil.ToUnixPath(allFiles[index].FullName);
            if (extensions.Length <= 0 || isExtension(allFiles[index].FullName, extensions))
            {
                if (fileList.Contains(fileFullName))
                {
                    LogUtil.Log(fileFullName + "is exist");
                }
                else
                {
                    fileList.Add(fileFullName);
                }
            }
        }
    }

    /// <summary>
    /// 根据路径获取文件流
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static FileStream GetFileStream(string filePath)
    {
        if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
        {
            return null;
        }
        return new FileStream(filePath, FileMode.Open, FileAccess.Read);
    }

    public static bool CreateFile(string destPath, byte[] bytes)
    {
        if (bytes == null || bytes.Length == 0)
        {
            LogUtil.Error("bytes Error {0}", destPath);
            return false;
        }
        CreateFilePath(destPath);
        File.WriteAllBytes(destPath, bytes);
        return true;
    }
    public static void DeleteFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    public static void CreateFilePath(string filePath)
    {
        if (File.Exists(filePath) == false)
        {
            string directoryName = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryName))
            {
                Directory.CreateDirectory(directoryName);
            }
        }
    }

    /// <summary>
    /// 将Asset-Bundle信息写入到XML
    /// </summary>
    /// <param name="outPath"></param>
    /// <param name="resPathDic"></param>
    public static void WriteABXmlData(string outPath, Dictionary<string, string> resPathDic)
    {
        if (resPathDic == null || resPathDic.Count <= 0)
        {
            return;
        }
        AssetBundleConfigList ABconfig = new AssetBundleConfigList();
        ABconfig.ABList = new List<BaseAssetBundle>();
        foreach (string path in resPathDic.Keys)
        {
            BaseAssetBundle abBase = new BaseAssetBundle();
            abBase.AssetName = path;
            abBase.BundleName = resPathDic[path];
            ABconfig.ABList.Add(abBase);
        }
        //写入xml
        string xmlPath = outPath + "/AssetBundleConfig.xml";
        if (File.Exists(xmlPath))
        {
            File.Delete(xmlPath);
        }
        FileStream fileStream = new FileStream(xmlPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
        StreamWriter sw = new StreamWriter(fileStream, System.Text.Encoding.UTF8);
        XmlSerializer xs = new XmlSerializer(ABconfig.GetType());
        xs.Serialize(sw, ABconfig);
        sw.Close();
        fileStream.Close();
    }

    /// <summary>
    /// 读取AssetBundle XML数据配置文件
    /// </summary>
    /// <param name="abPath"></param>
    /// <returns></returns>
    public static AssetBundleConfigList ReadABXmlData(string abPath)
    {
        if (string.IsNullOrEmpty(abPath))
        {
            return null;
        }
        FileStream fs = new FileStream(abPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
        System.Xml.Serialization.XmlSerializer xs = new XmlSerializer(typeof(AssetBundleConfigList));
        AssetBundleConfigList ABconfig = (AssetBundleConfigList)xs.Deserialize(fs);
        fs.Close();
        return ABconfig;
    }

    public static string BundleName
    {
        get
        {
            //#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            //            return "Windows";
            //#elif UNITY_ANDROID
            //				return "Android";
            //#elif UNITY_IPHONE
            //				return "IOS";
            //#else
            //				return "Windows";
            //#endif
            return "AssetBundles";
        }
    }


    /// <summary>
    /// 读取Bundle文件位置，DownloadManager可能修改次地址
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string GetBundleFilePath(string fileName)
    {
        var filePath = SREAMING_ASSETS_PATH + BundleName + "/" + fileName;
        // Managers.DownloadManager3.GetInstance().TryGetBundleFilePath(fileName, ref filePath);
        return filePath;
    }

    #region zjs add by 2021.05.19
    //移除拓展名
    public static string RemoveExpandName(string name)
    {
        if (Path.HasExtension(name))
            name = Path.ChangeExtension(name, null);
        return name;
    }

    //取出一个路径下的文件名
    public static string GetFileNameByPath(string path)
    {
        FileInfo fi = new FileInfo(path);
        return fi.Name; // text.txt
    }
    #endregion

    public class AssetCacheInfo
    {
        public string asset;
        public string bundle;
        public string version;
        public string md5;
        public long size = 0;
        public string saveVersion = "";
    }

    /// <summary>
    /// 将资源清单/Asset-Bundle信息写入到txt
    /// </summary>
    /// <param name="outFile"></param>
    /// <param name="resInfoDic"></param>
    public static void WriteResVersionsTxt(string outFile, Dictionary<string, AssetCacheInfo> resInfoDic)
    {
        if (resInfoDic == null || resInfoDic.Count <= 0)
        {
            return;
        }

        LogUtil.Log("WriteResVersionsTxt {0} {1}", outFile, resInfoDic.Count);
        if (File.Exists(outFile))
        {
            File.Delete(outFile);
        }

        File.WriteAllText(outFile, "");

    }

    public static long GetFileSize(string path)
    {
        FileInfo info = new FileInfo(path);
        return info.Length;
    }

}
