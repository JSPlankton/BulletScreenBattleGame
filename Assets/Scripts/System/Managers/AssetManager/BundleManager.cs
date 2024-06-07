using Managers;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnityEngine;

public static class BundleManager
{
    private sealed class AssetBundleInfo
    {
        public readonly AssetBundle m_AssetBundle;
        public int m_ReferencedCount;

        public AssetBundleInfo(AssetBundle assetBundle)
        {
            m_AssetBundle = assetBundle;
            m_ReferencedCount = 1;
        }
    }

    public delegate void ProcessCompleteEvent();

    static string m_DownloadingServerURL = "http://169.46.139.57:82/AssetBundles/";
    static string m_IndexFileName = "list.txt";
    static List<BundleItem> m_DownloadingList = new List<BundleItem>();
    static int m_TotalDownloadBytes = 0;
    static int m_CurrentDownloadIdx = 0;
    static int m_AlreadyDownloadBytes = 0;
    static WWW m_www;
    static string m_NewIndexContent;

    public static float downloadingProgress
    {
        get
        {
            int currentBytes = 0;
            if (m_www != null && m_CurrentDownloadIdx < m_DownloadingList.Count)
            {
                currentBytes = (int)(m_DownloadingList[m_CurrentDownloadIdx].filesize * m_www.progress);
            }
            if (m_TotalDownloadBytes > 0)
                return (float)(m_AlreadyDownloadBytes + currentBytes) / (float)m_TotalDownloadBytes;
            return 0;
        }
    }

    static AssetBundleManifest m_AssetBundleManifest = null;
    static AssetBundle m_AssetBundle = null;
    [ShowInInspector]
    static Dictionary<string, AssetBundleInfo> m_LoadedAssetBundles = new Dictionary<string, AssetBundleInfo>();
    static Dictionary<string, UnityEngine.Object> m_LoadedAsset = new Dictionary<string, UnityEngine.Object>();
    static Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();

    static string[] m_Variants = { };

    public static void UnAllAssetBundles()
    {
        List<string> allNames = new List<string>(m_LoadedAssetBundles.Count);
        foreach (var bundle in m_LoadedAssetBundles.Keys)
        {
            allNames.Add(bundle);
        }
        foreach (var bundle in allNames)
        {
            UnloadAssetBundle(bundle);
        }
        m_AssetBundleManifest = null;
    }

    public static string[] GetAllAssetBundle()
    {
        return m_AssetBundleManifest.GetAllAssetBundles();
    }

    public static IEnumerator LoadAllAssetAsync(string assetBundleName)
    {
#if UNITY_EDITOR
        yield return null;
#else
        LogUtil.Log("Load All Asset Async" + assetBundleName);
        AssetBundleInfo bundleInfo = null;
        if (m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundleInfo))
        {
            yield return bundleInfo.m_AssetBundle.LoadAllAssetsAsync();
        }
        else
        {
            AssetBundle bundle = LoadAssetBundle(assetBundleName);
            yield return bundle.LoadAllAssetsAsync();
        }
#endif

    }

    public static IEnumerator LoadAssetAsync(string assetBundleName, string assetName, Type type, CallBack<UnityEngine.Object, GameDefine.ResLoadState> callBack, AsyncLoadMode asyncMode = AsyncLoadMode.Default)
    {
        LogUtil.Log("LoadAssetAsync {0} {1}", assetBundleName, assetName);
        UnityEngine.Object obj = null;
        if (m_LoadedAsset.TryGetValue(assetName, out obj) && obj != null)
        {
            if (callBack != null)
                callBack(obj, GameDefine.ResLoadState.Success);
            yield break;
        }
        AssetBundleInfo bundleInfo = null;
        //动更系统可能在这里进行资源替换, 替换逻辑： 用Alias替换BundleName; Alias默认值等于BundleName
        AssetBundleRequest request = null;
        // DownloadManager3.GetInstance().TryGetBundleAlias(assetBundleName, assetName, out string assetBundleAlias);
        // assetBundleName = assetBundleAlias;
        if (m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundleInfo))
        {
            if (!bundleInfo.m_AssetBundle.isStreamedSceneAssetBundle)
            {
                request = bundleInfo.m_AssetBundle.LoadAssetAsync(assetName, type);
            }
        }
        else
        {
            AssetBundle bundle = null;
            if (asyncMode == AsyncLoadMode.Default)
            {
                bundle = LoadAssetBundle(assetBundleName);
            }
            else
            {
                yield return LoadAssetBundleAsync(assetBundleName);
                if (m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundleInfo))
                {
                    bundle = bundleInfo.m_AssetBundle;
                }
            }
            if (bundle != null && !bundle.isStreamedSceneAssetBundle)
            {
                request = bundle.LoadAssetAsync(assetName, type);
            }
        }

        if (request != null)
        {
            while (!request.isDone)
            {
                yield return false;
            }
            m_LoadedAsset[assetName] = request.asset;
            if (callBack != null)
                callBack(request.asset, GameDefine.ResLoadState.Success);
        }
        else
        {
            if (callBack != null)
                callBack(null, GameDefine.ResLoadState.Fail);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <param name="assetName"></param>
    /// <param name="type"></param>
    /// <param name="resDictCount"> 当前LoadAsset是否进行AssetManager 的 ResDict记录</param>
    /// <returns></returns>
    public static UnityEngine.Object LoadAsset(string assetBundleName, string assetName, Type type, bool resDictCount)
    {
        UnityEngine.Object obj = null;
        if (m_LoadedAsset.TryGetValue(assetName, out obj) && obj != null)
            return obj;
        AssetBundleInfo bundleInfo = null;
        //动更系统可能在这里进行资源替换, 替换逻辑： 用Alias替换BundleName; Alias默认值等于BundleName
        // DownloadManager3.GetInstance().TryGetBundleAlias(assetBundleName, assetName, out string assetBundleAlias);
        // assetBundleName = assetBundleAlias;
        string assetBundleAlias = "";
        if (m_LoadedAssetBundles.TryGetValue(assetBundleAlias, out bundleInfo))
        {
            if (!bundleInfo.m_AssetBundle.isStreamedSceneAssetBundle)
            {
                obj = bundleInfo.m_AssetBundle.LoadAsset(assetName, type);
                m_LoadedAsset[assetName] = obj;
                if (resDictCount)
                {

                    //出现一个Bundle导入多次Assets,这个时候，Dependency的计数也需要发生变化, 
                    bundleInfo.m_ReferencedCount++;
                    if (!m_Dependencies.TryGetValue(assetBundleAlias, out var dependencies))
                    {
                        dependencies = m_AssetBundleManifest.GetAllDependencies(assetBundleAlias);
                        // DownloadManager3.GetInstance().TryFillAssetBundleDependency(assetBundleAlias, ref dependencies);
                        if (dependencies.Length > 0)
                        {
                            for (int i = 0, n = dependencies.Length; i < n; i++)
                            {
                                dependencies[i] = RemapVariantName(dependencies[i]);
                            }

                            m_Dependencies.Add(assetBundleAlias, dependencies);
                        }
                    }
                    if (dependencies != null && dependencies.Length > 0)
                    {
                        for (int i = 0, n = dependencies.Length; i < n; i++)
                        {
                            if (m_LoadedAssetBundles.TryGetValue(dependencies[i], out var dependencyBundleInfo))
                            {
                                dependencyBundleInfo.m_ReferencedCount++;
                            }
                        }
                    }


                }
                return obj;
            }
            else
                return null;
        }
        var watch = Stopwatch.StartNew();
        AssetBundle bundle = LoadAssetBundle(assetBundleName);
        var watch1 = watch.ElapsedMilliseconds;
        if (bundle == null)
            return null;

        if (!bundle.isStreamedSceneAssetBundle)
        {
            obj = bundle.LoadAsset(assetName, type);
            m_LoadedAsset[assetName] = obj;
        }
#if UNITY_EDITOR
        LogUtil.Log("AssetLoad " + assetName + " UseTime " + watch1 + "  UseTime 2 : " +
        watch.ElapsedMilliseconds);
#endif
        watch.Stop();
        return obj;
    }

    /// <summary>
    /// Dangerous, Deprecate
    /// </summary>
    /// <param name="assetName"></param>
    //public static UnityEngine.Object[] LoadAllAssets(string assetBundleName, Type type)
    //{
    //    AssetBundleInfo bundleInfo = null;
    //    if (m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundleInfo))
    //    {
    //        return bundleInfo.m_AssetBundle.LoadAllAssets(type);
    //    }
    //    AssetBundle bundle = LoadAssetBundle(assetBundleName);
    //    return bundle.LoadAllAssets(type);
    //}


    public static void UnloadAsset(string assetName)
    {
        if (!m_LoadedAsset.ContainsKey(assetName)) return;

        if (m_LoadedAsset[assetName] == null)
        {
            m_LoadedAsset.Remove(assetName);
            return;
        }

        if (m_LoadedAsset[assetName] != null)
        {
            m_LoadedAsset[assetName] = null;
            m_LoadedAsset.Remove(assetName);
        }
    }

    public static AssetBundle LoadAssetBundle(string assetBundleName)
    {
        assetBundleName = RemapVariantName(assetBundleName);

        AssetBundleInfo bundle = null;
        if (m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle))
        {
            bundle.m_ReferencedCount++;
            return bundle.m_AssetBundle;
        }
        LoadDependencies(assetBundleName);
        bundle = LoadAssetBundleSingle(assetBundleName);

        if (bundle == null)
            return null;

        return bundle.m_AssetBundle;
    }

    public static IEnumerator LoadAssetBundleAsync(string assetBundleName)
    {
        assetBundleName = RemapVariantName(assetBundleName);

        AssetBundleInfo bundleInfo = null;
        if (m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundleInfo))
        {
            bundleInfo.m_ReferencedCount++;
            yield break;
        }
        else
        {
            yield return LoadDependenciesAsync(assetBundleName);
            yield return LoadAssetBundleSingleAsync(assetBundleName);
        }

    }


    private static Dictionary<string, byte[]> m_luaTables = new Dictionary<string, byte[]>();
#if !UNITY_EDITOR
	private static bool hasluaLoad = false;
#endif
    public static byte[] GetLuaBytes(string name)
    {
        var subName = name.Substring(name.LastIndexOf('/') + 1);
        if (m_luaTables.ContainsKey(subName))
        {
            return m_luaTables[subName];
        }

        if (m_luaTables.ContainsKey(name))
        {
            return m_luaTables[name];
        }

        byte[] target = null;
#if UNITY_EDITOR
        try
        {
            var fileInfo = File.ReadAllText(Application.dataPath + "/LuaFiles/Resources/" + name + ".lua");
            target = Encoding.UTF8.GetBytes(fileInfo);
        }
        catch (Exception e)
        {
            TextAsset asset = (TextAsset)Resources.Load(name);
            if (asset == null)
                target = null;
            target = asset.bytes;
            if (target != null)
                m_luaTables[name] = target;
            else
                LogUtil.Error("Lua File Not Found : " + name);
        }
#else
        if (!hasluaLoad)
        {
			var luaBundle = new AssetBundleInfo(AssetBundle.LoadFromFile(FileUtil.GetBundleFilePath("lua.unity3d")));
            var list = luaBundle.m_AssetBundle.LoadAllAssets();
            TextAsset text = null;
            foreach (var assetObj in list)
            {
                text = assetObj as TextAsset;
                if (text)
                {
                    m_luaTables[text.name] = text.bytes;
                }
            }
            hasluaLoad = true;
            if (m_luaTables.ContainsKey(subName))
                target = m_luaTables[subName];
            UnloadAssetBundleSingle("lua.unity3d");
        }

        if(target == null)
        {
            TextAsset asset = (TextAsset)Resources.Load(name);
            if (asset == null)
                target = null;
            target = asset.bytes;
            if (target != null)
                m_luaTables[name] = target;
            else
                LogUtil.Error("Lua File Not Found : " + name);
        }
#endif
        return target;
    }

    public static void UnloadAssetBundle(string assetBundleName, string assetName = "")
    {
        // DownloadManager3.GetInstance().TryGetBundleAlias(assetBundleName, assetName, out string assetBundleAlias);
        // assetBundleName = assetBundleAlias;
        UnloadAssetBundleSingle(assetBundleName);
        UnloadDependencies(assetBundleName);
    }

    private static AssetBundleInfo LoadAssetBundleSingle(string assetBundleName)
    {
        AssetBundleInfo bundle = null;
        if (m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle))
        {
            bundle.m_ReferencedCount++;
            return bundle;
        }

        string uri = FileUtil.GetBundleFilePath(assetBundleName);
        AssetBundle assetBundle = AssetBundle.LoadFromFile(uri);

        LogUtil.Log($"LoadAssetBundleSingle..........{assetBundleName}, {assetBundle != null}, {uri}");

        if (assetBundle == null)
            return null;
        bundle = new AssetBundleInfo(assetBundle);
        m_LoadedAssetBundles.Add(assetBundleName, bundle);
        return bundle;
    }

    public static void UnloadAssetBundleSingle(string assetBundleName)
    {
        AssetBundleInfo bundle = null;
        if (!m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundle))
        {
            return;
        }

        if (--bundle.m_ReferencedCount <= 0)
        {
            LogUtil.Log("Unload Asset Bundle......" + assetBundleName);

            bundle.m_AssetBundle.Unload(true);
            m_LoadedAssetBundles.Remove(assetBundleName);
        }
    }

    private static void LoadDependencies(string assetBundleName)
    {
        string[] dependencies = m_AssetBundleManifest.GetAllDependencies(assetBundleName);
        // DownloadManager3.GetInstance().TryFillAssetBundleDependency(assetBundleName, ref dependencies);
        if (dependencies.Length == 0)
        {
            return;
        }

        for (int i = 0, n = dependencies.Length; i < n; i++)
        {
            dependencies[i] = RemapVariantName(dependencies[i]);
        }

        if (!m_Dependencies.ContainsKey(assetBundleName))
            m_Dependencies.Add(assetBundleName, dependencies);

        for (int i = 0, n = dependencies.Length; i < n; i++)
        {
            LoadAssetBundleSingle(dependencies[i]);
        }
    }

    private static void UnloadDependencies(string assetBundleName)
    {
        string[] dependencies = null;
        if (!m_Dependencies.TryGetValue(assetBundleName, out dependencies))
        {
            return;
        }

        for (int i = 0, n = dependencies.Length; i < n; i++)
        {
            UnloadAssetBundleSingle(dependencies[i]);
        }

        m_Dependencies.Remove(assetBundleName);
    }

    private static IEnumerator LoadAssetBundleSingleAsync(string assetBundleName)
    {
        AssetBundleInfo bundleInfo;
        if (m_LoadedAssetBundles.TryGetValue(assetBundleName, out bundleInfo))
        {
            bundleInfo.m_ReferencedCount++;
            yield return null;
        }
        else
        {
            string uri = FileUtil.GetBundleFilePath(assetBundleName);
            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(uri);

            yield return request;


            LogUtil.Log($"LoadAssetBundleSingle..........{assetBundleName}, {request.assetBundle != null}, {uri}");
            if (request.assetBundle == null)
                yield return null;
            else
            {
                bundleInfo = new AssetBundleInfo(request.assetBundle);
                m_LoadedAssetBundles.Add(assetBundleName, bundleInfo);
                yield return null;
            }
        }


    }

    private const float TARGET_MIN_TIME = 0.2f;  //尽量让帧率不要低于5
    private static int _loadTimeCounter;    //实际帧加载数量
    private static int _loadTimeCount;
    private const int DEFAULT_ROUND_COUNTER = 10;

    /// <summary>
    /// 特殊异步多Assetbundle处理，目前Assetbundle文件数量过多，如果完全使用async会造成高端机帧浪费严重，所以这里面使用动态async，即每一帧同步执行1个或者多个加载，加载速度根据上一帧完成情况动态调整
    /// </summary>
    /// <param name="assetBundleName"></param>
    /// <returns></returns>
    private static IEnumerator LoadDependenciesAsync(string assetBundleName)
    {
        string[] dependencies = m_AssetBundleManifest.GetAllDependencies(assetBundleName);
        if (dependencies.Length == 0)
        {
            yield return null;
        }
        else
        {
            for (int i = 0, n = dependencies.Length; i < n; i++)
            {
                dependencies[i] = RemapVariantName(dependencies[i]);
            }

            if (!m_Dependencies.ContainsKey(assetBundleName))
                m_Dependencies.Add(assetBundleName, dependencies);

            _loadTimeCounter = _loadTimeCount = DEFAULT_ROUND_COUNTER;
            for (int i = 0, n = dependencies.Length; i < n; i++)
            {
                LoadAssetBundleSingle(dependencies[i]);
                _loadTimeCounter--;
                if (_loadTimeCounter <= 0)
                {
                    yield return null;
                    var _timeMultiple = TARGET_MIN_TIME / Time.unscaledDeltaTime;
                    _loadTimeCount = (int)Mathf.Max(2, _loadTimeCount * _timeMultiple);
                    _loadTimeCounter = _loadTimeCount;
                    LogUtil.Log($"Target Frame Load {_loadTimeCount}, {_timeMultiple}");
                }
            }
        }
    }

    private static string RemapVariantName(string assetBundleName)
    {
        string[] bundlesWithVariant = m_AssetBundleManifest.GetAllAssetBundlesWithVariant();

        if (System.Array.IndexOf(bundlesWithVariant, assetBundleName) < 0)
        {
            return assetBundleName;
        }

        string[] split = assetBundleName.Split('.');

        int bestFit = int.MaxValue;
        int bestFitIndex = -1;
        // Loop all the assetBundles with variant to find the best fit variant assetBundle.
        for (int i = 0; i < bundlesWithVariant.Length; i++)
        {
            string[] curSplit = bundlesWithVariant[i].Split('.');
            if (curSplit[0] != split[0])
                continue;

            int found = System.Array.IndexOf(m_Variants, curSplit[1]);
            if (found != -1 && found < bestFit)
            {
                bestFit = found;
                bestFitIndex = i;
            }
        }
        if (bestFitIndex != -1)
        {
            return bundlesWithVariant[bestFitIndex];
        }
        else
        {
            return assetBundleName;
        }
    }

    /// <summary>
    /// 初始化MainfestXml
    /// </summary>
    public static void InitMainfestXml()
    {
        if (false)
        {
            string manifeatName = FileUtil.GetBundleFilePath(FileUtil.BundleName);

            if (m_AssetBundle != null)
            {
                m_AssetBundle.Unload(true);
            }
            // 初始化mainfestBundle
            m_AssetBundle = AssetBundle.LoadFromFile(manifeatName);
            LogUtil.Log($"Load Bundle Manifest File From {manifeatName} {m_AssetBundle != null}");

            if (null != BundleManager.m_AssetBundleManifest)
            {
                UnAllAssetBundles();
            }
            if (BundleManager.m_AssetBundleManifest == null)
            {
                BundleManager.m_AssetBundleManifest = m_AssetBundle.LoadAsset("AssetBundleManifest", typeof(AssetBundleManifest)) as AssetBundleManifest;
            }

        }
    }
}
