using UnityEngine;

public class AssetConfig
{
    public const string APP_VERSION_FIEL_NAME = "version.json";
    public const string ASSET_BUNDLE_MANIFEST_NAME = "assetbundlemanifest";
    public const string ASSETS_CONFIG_NAME = "assetsconfig.json";
    public const string ASSETS_CONFIG_NAME_G = "assetsconfig_g.json";
    public const string ASSETS_CONFIG_NAME_R = "assetsconfig_r.json";
    public const string BUNDLE_CAPCITY_NAME = "bundlecapcity.json";
    public const string BUNDLE_DYNAMIC_FLAG = "dyna+";
    public const string BUNDLE_EX = ".assetbundle";
    public const string BUNDLE_ORIGIN_CAPCITY_NAME = "bundleorigincapcity.json";
    public const string BUNDLE_OTA_FLAG = "ota+";
    public const string BUNDLE_PATH = "iPhone/SD";
    public const string BUNDLE_STATIC_FLAG = "static+";
    public const string BUNDLE_VERSION_LIST_NAME = "BundleVersionList.json";
    public const string BUNDLE_VERSION_LIST_NAME_G = "BundleVersionList_G.json";
    public const string BUNDLE_VERSION_LIST_NAME_R = "BundleVersionList_R.json";
    public const char CacheSymbol = '=';
    public const string CONFIG_VERSION_LIST_NAME = "ConfigVersionList.json";
    public const string CONFIG_ZIP_FILE_NAME = "config.zip";
    public const string LOCAL_CONFIG_VERSION_NAME = "local_config_version";
    public const string LOCALIZE_FLAG = "localize_";
    public static string localPath = Application.persistentDataPath;
    public static bool USE_COMPRESS = true;
    public const string VERSION_LIST_NAME = "VersionList.json";
    /// <summary>
    /// 需要构建bundle的目录
    /// </summary>
    public const string BUNDLE_SRC_PATH = "Bundles";

    /// <summary>
    /// Assets目录
    /// </summary>
    public static string ASSETS = "Assets";

    /// <summary>
    /// Configs目录
    /// </summary>
    public static string CONFIGS = "Configs";
    /// <summary>
    /// Special目录，存放特殊文件例：需要打包带出去并且热更
    /// </summary>
    public static string SPECIAL = "Special";

    public static string FONTS = "Fonts";
    public static string FONT_PATH = BUNDLE_SRC_PATH + "/" + FONTS;

    /// <summary>
    /// bundles相对路径
    /// </summary>
    public static string ASSETS_BUNDLES_PATH = ASSETS + "/" + BUNDLE_SRC_PATH;

    /// <summary>
    /// Application.persistentDataPath
    /// </summary>
    public static string BundleCacheDir
    {
        get
        {
            return localPath;
        }
    }

    // Asset 拆包Const
    public static string BUNDLE_SPLIT_PATH = "BundlesSplits";
    public static string ASSETS_BUNDLES_SPLIT_PATH = ASSETS + "/" + BUNDLE_SPLIT_PATH;
}

