/**
*	工程生成路径类
*	author:zhaojinlun
*	date:2021年4月15日
*	copyright: fungather.net
***/
using UnityEngine;
public class BuildConfig
{
    public static string APP_VERSION_FILE_PATH = (Application.streamingAssetsPath + "/version.json");
    public static string RES_VERSION_FILE_PATH_STREAMING = (Application.streamingAssetsPath + "/versions.txt");
    public static string CONFIG_VERSION_FILE_PATH_STREAMING = (Application.streamingAssetsPath + "/conf_version.txt");
    public static string RES_VERSION_FILE_PATH_PERSISTENT = (Application.persistentDataPath + "/" + FileUtil.BundleName + "/versions.txt");
    public static string ASSETS_CONFIG_PATH = (BUILD_BUNDLE_FOLDER_PATH + "/assetsconfig.json");
    public static string BUILD_BUNDLE_FOLDER_PATH = (BUILD_TEMP_FOLDER + "/build/bundle");


    /// <summary>
    /// AssetBundle Config生成目录 Application.streamingAssetsPath + "/AssetBundleConfig"
    /// </summary>
    public static string BUILD_FOLDER_AB_CONFIG = Application.streamingAssetsPath + "/AssetBundleConfig";

    public const string BUILD_FOLDER = "build";
    /// <summary>
    /// AssetBunldle输出目录
    /// </summary>
    public static string BUILD_FOLDER_PATH = (Application.streamingAssetsPath + "/AssetBundles");
    public static string BUILD_FOLDER_CONFIG_PATH = (BUILD_FOLDER_PATH + "/configs");
    public static string SHADER_ASSET_BUNDLE_NAME = "single_shader";

    //项目根路径
    public static string Root = PathUtil.ToUnixPath(System.Environment.CurrentDirectory);

    public static string BUILD_TEMP_FOLDER = (Application.streamingAssetsPath + "/Temp");
    public static string BUNDLE_CACHE_FOLDER = (Application.streamingAssetsPath + "/build/cache");
    public static string BUNDLE_CAPCITY_PATH = (BUNDLE_CACHE_FOLDER + "/bundlecapcity.json");
    public static string BUNDLE_ORIGIN_CAPCITY_PATH = (BUNDLE_CACHE_FOLDER + "/bundleorigincapcity.json");
    public static string BUNDLE_TYPE_G_FOLDER = (BUILD_TEMP_FOLDER + "/G");
    public static string BUNDLE_TYPE_R_FOLDER = (BUILD_TEMP_FOLDER + "/R");
    public static string BUNDLE_VERSION_LIST_PATH = (BUNDLE_CACHE_FOLDER + "/VersionList.json");
    public static string CONFIG_CACHE_FOLDER = (Application.streamingAssetsPath + "/build/config");
    public static string CONFIG_FOLDER = (BUILD_TEMP_FOLDER + "/config");
    public static string DEPLOY_ASSETS_FOLDER = (Application.dataPath + "/_BUILD/DeployAssets");
    public static string MANIFEST_PATH = (BUILD_BUNDLE_FOLDER_PATH + "/bundle.manifest");
    public static string ZIPED_BUNDLE_FOLDER = (BUILD_TEMP_FOLDER + "/ZipedBundle");

    //singleAB资源相关
    public static string BUILD_EXTRA_FOLDER = Application.dataPath + "/../BuildExtra";
    public static string SINGLE_AB_BUILD_FOLDER_PATH = (BUILD_EXTRA_FOLDER + "/SingleAssetBundles");

    public const string SINGLE_AB_DIR_PATH = "/singleab";
    public const string SINGLE_AB_NAME = "singleab/{0}";
    public const string SINGLE_AB_ATLAS_NAME = "singleab_atlas_{0}";
    public const string ATLAS_FILE_PATH = "Assets/Atlas/{0}.spriteatlas";

    //res download相关
    public static string ASSET_VERSION_FILE = "asset_version.json";

    //assetbundle split 相关

    public static string BUNDLE_SPLIT_COMPRESS_FOLDER = Application.dataPath + "/../AssetBundleSplitCompress";
    public static string BUNDLE_SPLIT_FLAG_FILE = "split_flag";
    public static string BUNDLE_SPLIT_INFO_FILE = "split_info";
    public static string BUNDLE_SPLIT_VERSION_FILE = Application.streamingAssetsPath + "/bundle_split.json";

    public static string BUNDLE_SPLIT_BUILD_FOLDER_PATH = (Application.streamingAssetsPath + "/AssetBundles/bundlessplits");
    public static string BUNDLE_SPLIT_BUILD_EXTRA_FOLDER = Application.dataPath + "/../BundleExtra";
}

