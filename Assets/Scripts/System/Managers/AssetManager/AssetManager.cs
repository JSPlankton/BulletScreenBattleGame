using Managers;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using WebSocketSharp;

public class AssetData
{
    public int refCount = 1;
    public UnityEngine.Object asset;
    public string name;
}

public enum AsyncLoadMode
{
    Default,    // load assset async, load bundle sync
    BundleAsync, // load asset and bundle async, bundle use smart fake async
    BundleStrictAsync, //load asset and bundle async, bundle force async TODO implement
}

public class AssetManager : MonoSingleton<AssetManager>
{
    private static AssetManager assetManager;
    public static bool IsLoadAssetFromBundle = true;
    [ReadOnly]
    [ShowInInspector]
    private Dictionary<string, LoadedAsset> loadedAssets = new Dictionary<string, LoadedAsset>();
    private List<LoadingAsset> loadingAssets = new List<LoadingAsset>();
#if UNITY_EDITOR
    [Button("BundleManager"), GUIColor(0.4f, 0.8f, 1f)]
    [PropertyOrder(-10)]
    private void InspectBundleManager()
    {
        Sirenix.OdinInspector.Editor.StaticInspectorWindow.InspectType(typeof(BundleManager));
    }
#endif

    public void ClearTempData()
    {
        this.loadingAssets.Clear();
    }


    public void Dispose()
    {
    }


    private void DisposeLoadedAssets()
    {
        foreach (LoadedAsset asset in this.loadedAssets.Values)
        {
            asset.RefCount = 0;
        }
    }


    private string GetAssetNameFromPath(string fullname)
    {
        return fullname.Replace("/", "+").ToLower();
    }


    private string GetShortname(string assetname)
    {
        char[] separator = new char[] { '+' };
        string[] strArray = assetname.Split(separator);
        return strArray[strArray.Length - 1];
    }

    public void LoadAsync(string fullname, System.Type type = null, CallBack<UnityEngine.Object, GameDefine.ResLoadState> callBack = null, AsyncLoadMode asyncMode = AsyncLoadMode.Default)
    {
        StartCoroutine(LoadAssetAsync(fullname, type, callBack, asyncMode));
    }
    public IEnumerator LoadAsyncCO(string fullname, System.Type type = null, CallBack<UnityEngine.Object, GameDefine.ResLoadState> callBack = null, AsyncLoadMode asyncMode = AsyncLoadMode.Default)
    {
        return LoadAssetAsync(fullname, type, callBack, asyncMode);
    }

    public void LoadAllAsync(string filename)
    {
        StartCoroutine(LoadAssetAllAsync(filename));
    }


    public bool IsFileExists(string pathName, Type type)
    {
        // if (Global.useAssetBundle)
        // {
        //     var data = WebRequestManager.GetUnityWebRequest($"{BuildConfig.BUILD_FOLDER_PATH}/{GetNameWithFileExtention(pathName, type)}");
        //     if (null == data)
        //     {
        //         return false;
        //     }
        // }
        // else
        // {
        //     string fileName = LoadPathUtil.GetPathForLocalFile($"{GetNameWithFileExtention(pathName, type)}");
        //     if (!System.IO.File.Exists(fileName))
        //     {
        //         return false;
        //     }
        // }
        return true;
    }

    /// <summary>
    /// 直接返回IEnumerator, 场景Scene加载不需要LoadAsset,只需要完成AssetBundle加载即可
    /// Editor模式下不起作用
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    public IEnumerator LoadSceneAsyncCO(string sceneName)
    {
        // if (!Global.useAssetBundle)
        // {
        //     return null;
        // }
        return LoadAssetAsync(sceneName, typeof(UnityEngine.SceneManagement.Scene), null, AsyncLoadMode.BundleAsync);
    }

    IEnumerator LoadAssetAllAsync(string filename)
    {
        yield return BundleManager.LoadAllAssetAsync(filename + FileExtensionUtil.bundle);
    }

    public string[] GetAllAssetBundleName()
    {
        return BundleManager.GetAllAssetBundle();
    }

    [ReadOnly]
    [ShowInInspector]
    Dictionary<string, AssetData> resDict = new Dictionary<string, AssetData>();

    public int ResCount { get => resDict.Count(); }
    IEnumerator LoadAssetAsync(string fullname, System.Type type, CallBack<UnityEngine.Object, GameDefine.ResLoadState> callBack, AsyncLoadMode asyncMode = AsyncLoadMode.Default)
    {
        fullname = fullname.ToLower();
        if (type == null)
        {
            type = typeof(UnityEngine.Object);
        }

        fullname = GetNameWithFileExtention(fullname, type);

        if (resDict.ContainsKey(fullname) && resDict[fullname].asset != null)
        {
            if (callBack != null)
            {
                //LogUtil.Log("asset refCount async ++ " + fullname +", " + resDict[fullname].refCount);
                callBack(resDict[fullname].asset, GameDefine.ResLoadState.Success);
                resDict[fullname].refCount++;
            }

            yield return null;
        }
        else
        {
            if (false)
            {
                string bundleName = fullname + FileExtensionUtil.bundle;

                CallBack<UnityEngine.Object, GameDefine.ResLoadState> loadCallback = (gameObject, resLoadState) =>
                {
                    AssetData assetData = new AssetData();
                    assetData.name = fullname;
                    assetData.asset = gameObject;
                    resDict[fullname] = assetData;
                    callBack?.Invoke(gameObject, resLoadState);
                };
                yield return BundleManager.LoadAssetAsync(bundleName.ToLower(), fullname, type, loadCallback, asyncMode);
            }
            else
            {
                // string path = LoadPathUtil.GetPathForLocalFile(fullname);
#if UNITY_EDITOR
                // UnityEngine.Object tempObj = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                //
                // if (!resDict.ContainsKey(fullname))
                // {
                //     AssetData assetData = new AssetData();
                //     assetData.name = fullname;
                //     resDict.Add(fullname, assetData);
                // }
                // resDict[fullname].asset = tempObj;
                //
                //
                // if (callBack != null)
                //     callBack(resDict[fullname].asset, GameDefine.ResLoadState.Success);
#endif
                yield return null;
            }
        }
    }

    /// <summary>
    /// 加载配置文件
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public string LoadConfig(string fileName)
    {
        string filePath = FileUtil.GetBundleFilePath(fileName);

        if (filePath.IndexOf(Application.persistentDataPath) >= 0)
        {
            if (File.Exists(filePath))
            {
                LogUtil.Log($"AssetManager Load Config {fileName} {filePath}");
                return File.ReadAllText(filePath);
            }
        }
        TextAsset obj = Load(fileName, typeof(TextAsset)) as TextAsset;
        var ret = obj == null ? null : obj.text;
        Unload(fileName, typeof(TextAsset));
        return ret;

    }

    public byte[] LoadConfigData(string fileName)
    {
        string filePath = FileUtil.GetBundleFilePath(fileName);

        if (filePath.IndexOf(Application.persistentDataPath) >= 0)
        {
            if (File.Exists(filePath))
            {
                LogUtil.Log($"AssetManager Load Config {fileName} {filePath}");
                return File.ReadAllBytes(filePath);
            }
        }
        TextAsset obj = Load(fileName, typeof(TextAsset)) as TextAsset;
        var xmlBytes = obj == null ? null : obj.bytes;
        Unload(fileName, typeof(TextAsset));
        return xmlBytes;
    }

    public UnityEngine.Object Load(string fullname, System.Type type = null)
    {
        if (fullname == null)
        {
            return null;
        }
        if (type == null)
        {
            type = typeof(UnityEngine.Object);
        }
        fullname = fullname.ToLower();
        //if (type == typeof(SkeletonDataAsset)) {
        //	if (!fullname.Contains (".asset"))
        //		fullname += ".asset";

        //} else 
        fullname = GetNameWithFileExtention(fullname, type);

        if (resDict.ContainsKey(fullname) && resDict[fullname].asset != null)
        {
            LogUtil.Log("asset refCount ++ " + fullname + ", " + resDict[fullname].refCount);
            resDict[fullname].refCount++;

            return resDict[fullname].asset;
        }
        UnityEngine.Object tempObj = null;
        bool disableEditorBundleLoad = false;
#if UNITY_EDITOR
        if (type == typeof(RenderPipelineAsset))
            disableEditorBundleLoad = true;
#endif
//         if (Global.useAssetBundle && !disableEditorBundleLoad)
//         {
//             string bundleName = fullname + FileExtensionUtil.bundle;
//             tempObj = BundleManager.LoadAsset(bundleName.ToLower(), fullname, type, true) as UnityEngine.Object;
//         }
//         else
//         {
// #if UNITY_EDITOR
//             string path = LoadPathUtil.GetPathForLocalFile(fullname);
//             tempObj = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
// #endif
//         }

        if (tempObj == null && type != typeof(UnityEngine.SceneManagement.Scene))
        {
            return null;
        }

        if (!resDict.ContainsKey(fullname))
        {
            AssetData assetData = new AssetData();
            assetData.name = fullname;
            resDict[fullname] = assetData;
        }
        resDict[fullname].asset = tempObj;

        return tempObj;
    }

    private string GetNameWithFileExtention(string name, Type type)
    {
        if (type == null)
        {
            type = typeof(UnityEngine.Object);
        }

        if (type == typeof(Material))
        {
            if (!name.Contains(FileExtensionUtil.mat))
                name += FileExtensionUtil.mat;
        }
        else if (type == typeof(TextAsset))
        {
            if (!name.Contains(FileExtensionUtil.txt) && !name.Contains(FileExtensionUtil.xml))
                name += FileExtensionUtil.txt;
        }
        else if (type == typeof(Scene))
        {
            if (!name.Contains(FileExtensionUtil.unity))
                name += FileExtensionUtil.unity;
        }
        else if (type == typeof(Shader))
        {
            if (!name.Contains(FileExtensionUtil.shader))
                name += FileExtensionUtil.shader;
        }
        else if (type == typeof(VideoClip))
        {
            if (!name.Contains(FileExtensionUtil.mov))
                name += FileExtensionUtil.mov;
        }
        else if (type == typeof(RenderTexture))
        {
            if (!name.Contains(FileExtensionUtil.renderTexture))
                name += FileExtensionUtil.renderTexture;
        }
        else if (type == typeof(AudioClip))
        {
            if (!name.Contains(FileExtensionUtil.ogg))
                name += FileExtensionUtil.ogg;
        }
        else if (type == typeof(Font))
        {
        }
        else if (type == typeof(RenderPipelineAsset))
        {
            if (!name.Contains(FileExtensionUtil.asset))
                name += FileExtensionUtil.asset;
        }
        else
        {
            if (!name.Contains(FileExtensionUtil.prefab))
                name += FileExtensionUtil.prefab;
        }

        return name;
    }

    public UnityEngine.GameObject LoadAndInstantiate(string fullName, System.Type type = null, Transform parent = null)
    {
        UnityEngine.Object obj = Load(fullName, type);
        if (obj == null)
            return null;

        GameObject gameObject;

        if (parent != null)
            gameObject = (GameObject)GameObject.Instantiate(obj, parent);
        else
            gameObject = (GameObject)GameObject.Instantiate(obj);

        return gameObject;
    }

    //public UnityEngine.Object Load (string fullname, System.Type type = null)
    //{
    //	if (fullname == null) {
    //		return null;
    //	}
    //	if (type == null) {
    //		type = typeof(UnityEngine.Object);
    //	}
    //	if (!IsLoadAssetFromBundle) {
    //		return Resources.Load (fullname, type);
    //	}
    //	string assetNameFromPath = this.GetAssetNameFromPath (fullname);
    //	if (this.GetBundleNameFromAsset (assetNameFromPath) == null) {
    //		return Resources.Load (fullname, type);
    //	}
    //	return this.LoadAssetFromBundle (assetNameFromPath, type);
    //}

    public void LoadAssetFromBundleAsync(LoadingBundle lb, LoadingAsset la)
    {
        if (la.bundlename == lb.name)
        {
            string shortname = this.GetShortname(la.name);
            AssetBundleRequest request = lb.ab.LoadAssetAsync(shortname, la.type);
            la.abr = request;
            la.isLoading = true;
        }
    }

    #region 卸载Asset
    public void UnloadAsset(string assetName)
    {
        assetName = assetName.ToLower();
        if (resDict.ContainsKey(assetName))
        {
            resDict[assetName].asset = null;
            resDict.Remove(assetName);
        }
        // if (!Global.useAssetBundle) return;
        BundleManager.UnloadAsset(assetName.ToLower());
    }

    public void UnloadAllAssets()
    {
        BundleManager.UnAllAssetBundles();
    }
    /// <summary>
    /// 卸载资源
    /// 1.清空ResDict中的引用
    /// 2.卸载相应的assetBundle和引用资源
    /// </summary>
    /// <param name="assetName"></param>
    public void Unload(string assetName, System.Type assetType = null, bool unload = false)
    {
        assetName = assetName.ToLower();

        if (assetType == null)
            assetType = typeof(System.Object);
        assetName = GetNameWithFileExtention(assetName, assetType);
        if (!resDict.ContainsKey(assetName)) return;
        //LogUtil.Log("asset refCount -- " + assetName + ", " + resDict[assetName].refCount);

        if (unload) resDict[assetName].refCount = 1;

        if (--resDict[assetName].refCount > 0) return;

        UnloadAsset(assetName);
        UnloadBundle(assetName + FileExtensionUtil.bundle, assetName);
    }

    public void Unload<T>(Dictionary<T, string> assets)
    {
        foreach (var asset in assets)
        {
            AssetManager.Instance.Unload(asset.Value, null, false);
        }
    }
    #endregion

    #region 卸载bundle
    public static void UnloadBundle(string name, string assetName = "")
    {
        // if (!Global.useAssetBundle) return;
        BundleManager.UnloadAssetBundle(name.ToLower(), assetName.ToLower());
    }

    public static AssetBundle LoadBundle(string name)
    {
        return BundleManager.LoadAssetBundle(name.ToLower());
    }
    #endregion

    private void OnAssetUnload(string name)
    {
        if (this.loadedAssets.ContainsKey(name))
        {
            this.loadedAssets.Remove(name);
        }
    }


    public void OnBundleLoadFinished(LoadingBundle lb)
    {
        for (int i = 0; i < this.loadingAssets.Count; i++)
        {
            LoadingAsset la = this.loadingAssets[i];
            if (!la.isLoading)
            {
                this.LoadAssetFromBundleAsync(lb, la);
            }
        }
    }


    private void OnLoadAssetFromBundleFailed(string bundlename, string assetname)
    {
    }


    public void PrintContent()
    {
        foreach (LoadingAsset asset in this.loadingAssets)
        {
        }
        foreach (KeyValuePair<string, LoadedAsset> pair in this.loadedAssets)
        {
            LoadedAsset asset2 = pair.Value;
        }
    }


    private void Start()
    {
        assetManager = this;
    }


    public void UnLoadUnusedRes()
    {

        //List<string> assetNames = new List<string>();
        //foreach (var res in resDict)
        //{
        //    assetNames.Add(res.Key);
        //    Debug.Log(string.Format("{0}: count {1}", res.Key, res.Value.refCount));
        //}

        //for (int i = 0; i < assetNames.Count; i++)
        //{
        //    string assetName = assetNames[i];
        //    if (resDict.ContainsKey(assetName) && resDict[assetName] != null)
        //    {
        //        resDict[assetName].asset = null;
        //        BundleManager.UnloadAsset(assetName);
        //    }

        //    string assetNameWithExtension = FileExtensionUtil.bundle;
        //    if (resDict.ContainsKey(assetNameWithExtension) && resDict[assetNameWithExtension] != null)
        //    {
        //        resDict[assetNameWithExtension].asset = null;
        //        BundleManager.UnloadAsset(assetNameWithExtension);
        //    }
        //}

        //assetNames.Clear();
        //resDict.Clear();

        //Resources.UnloadUnusedAssets();
    }

    public void LoadTextureAssetBundle(string assetName, string fileExtention = FileExtensionUtil.png)
    {
        // if (!Global.useAssetBundle) return;

        var assetBundleName = assetName.ToLower() + fileExtention + FileExtensionUtil.bundle;
        // DownloadManager3.GetInstance().TryGetBundleAlias(assetBundleName, "", out var assetBundleAlias);
        // assetBundleName = assetBundleAlias;
        BundleManager.LoadAssetBundle(assetBundleName);
    }

    public void UnLoadTextureAssetBundle(string assetName)
    {
        // if (!Global.useAssetBundle) return;

        assetName = assetName.ToLower();

        UnloadBundle(assetName + FileExtensionUtil.png + FileExtensionUtil.bundle);

        List<string> textures = new List<string>();
        foreach (var item in assetDict)
        {
            textures.Add(item.Key);
        }

        for (int i = 0; i < textures.Count; i++)
        {
            string texture = textures[i];

            if (texture.IndexOf(assetName) >= 0 && assetDict.ContainsKey(texture))
            {
                if (assetDict[texture] != null && assetDict[texture].asset == null)
                {
                    assetDict[texture].asset = null;
                    BundleManager.UnloadAsset(texture + FileExtensionUtil.png);
                    assetDict[texture] = null;
                    assetDict.Remove(texture);
                }
            }
        }
        textures.Clear();

    }

    private void Update()
    {
        if (this.loadingAssets.Count == 0)
        {
            return;
        }
        for (int i = 0; i < this.loadingAssets.Count; i++)
        {
            LoadingAsset item = this.loadingAssets[i];
            if (item.abr != null)
            {
                if (item.abr.isDone)
                {
                    UnityEngine.Object obj = null;
                    try
                    {
                        obj = item.abr.asset;
                    }
                    catch
                    {
                        this.OnLoadAssetFromBundleFailed(item.bundlename, item.name);
                        break;
                    }
                    if (null == obj)
                    {
                        this.OnLoadAssetFromBundleFailed(item.bundlename, item.name);
                        return;
                    }
                    try
                    {
                        if (item.callback != null)
                        {
                            LoadedAsset loadedAsset = new LoadedAsset(item.name, item.type, obj);
                            loadedAsset.onAssetUnLoad += new Action<string>(this.OnAssetUnload);
                            loadedAsset.RefCount = item.RefCount;
                            if (this.loadedAssets.ContainsKey(item.name))
                            {
                                LogUtil.Error(string.Concat("~~~~~already has key in loaded assets2 ", item.name),
                                    new object[0]);
                            }
                            else
                            {
                                this.loadedAssets.Add(item.name, loadedAsset);
                            }
                            item.callback(obj, true);
                        }
                        this.loadingAssets.RemoveAt(i);
                        i--;
                    }
                    catch (NullReferenceException nullReferenceException)
                    {
                        this.loadingAssets.RemoveAt(i);
                        i--;
                    }
                }
            }
        }
    }

    #region 资源加载

    public Dictionary<string, AssetData> assetDict = new Dictionary<string, AssetData>();

    private AssetData LoadAssetData<T>(string assetPath, string assetName, string suffix) where T : UnityEngine.Object
    {
        string path = assetPath + "/" + assetName;

        if (assetDict.ContainsKey(path) && assetDict[path].asset != null)
        {
            return assetDict[path];
        }

        return null;
//         else
//         {
//             AssetData assetData = new AssetData();
//             UnityEngine.Object asset = null;
//             if (Global.useAssetBundle)
//             {
//                 if (!string.IsNullOrEmpty(suffix) &&
//                     !suffix.Equals(FileExtensionUtil.png) &&
//                     FileExtensionUtil.Textures.Contains(suffix))
//                     assetPath += FileExtensionUtil.png;
//                 else
//                     assetPath += suffix;
//
//                 asset = BundleManager.LoadAsset(
//                     $"{assetPath}{FileExtensionUtil.bundle}".ToLower(),
//                     $"{path}{suffix}", typeof(T), false) as UnityEngine.Object;
//             }
//             else
//             {
// #if UNITY_EDITOR
//                 asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(LoadPathUtil.GetPathForLocalFile($"{path}{suffix}"));
// #endif
//             }
//
//             if (asset == null) return null;
//             assetData.asset = asset;
//             assetData.name = name;
//             assetDict[path] = assetData;
//             return assetData;
//         }
    }

    /// <summary>
    /// 此方法只允许加载图片时使用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="assetPath"></param>
    /// <param name="assetName"></param>
    /// <param name="suffix"></param>
    /// <returns></returns>
    public T LoadAsset<T>(string assetPath, string assetName, string suffix) where T : UnityEngine.Object
    {
        AssetData assetData;
        if (false)
        {
            assetData = LoadAssetData<T>(assetPath.ToLower(), assetName, suffix);
        }
        else
        {
            //先从Dynamic下加载，有就直接使用
            string newPath = "dynamic/" + assetPath.ToLower();
            string pathFileName = Path.Combine(Application.persistentDataPath, FileUtil.BundleName, newPath + suffix + ".ab.manifest");
            if (!File.Exists(pathFileName))
            {
                assetData = LoadAssetData<T>(assetPath.ToLower(), assetName, suffix);
            }
            else
            {
                assetData = LoadAssetData<T>(newPath, assetName, suffix);
                if (null == assetData)
                {
                    assetData = LoadAssetData<T>(assetPath.ToLower(), assetName, suffix);
                }
            }
        }
        if (null == assetData)
        {
            return null;
        }
        return (T)assetData.asset;
    }

    /// <summary>
    /// 加载Sprite
    /// </summary>
    /// <param name="assetPath"></param>
    /// <param name="assetName"></param>
    /// <param name="suffix"></param>
    /// <returns></returns>
    public Sprite LoadSpriteAsset(string assetPath, string assetName, string suffix)
    {
        if (string.IsNullOrEmpty(assetPath) || string.IsNullOrEmpty(assetName))
        {
            return null;
        }
        AssetData assetData = LoadAssetData<Sprite>(assetPath.ToLower(), assetName.ToLower(), suffix);
        if (null == assetData)
        {
            return null;
        }
        return (Sprite)assetData.asset;
    }
    /// <summary>
    /// 加载spine,spineName为空或不存在，使用默认defSpineName
    /// </summary>
    /// <param name="spineName">spine名称</param>
    /// <param name="spineParent">spine父</param>
    /// <param name="defSpineName">默认spine名称</param>
    /// <returns></returns>
    public GameObject LoadSpine(string spineName, GameObject spineParent, string defSpineName = "")
    {
        if (string.IsNullOrEmpty(spineName) || !ResourcesConfigManager.GetIsExitRes(spineName))
        {
            if (string.IsNullOrEmpty(defSpineName) || !ResourcesConfigManager.GetIsExitRes(defSpineName))
            {
                LogUtil.Error("{0}--LoadSpine--spineName={1}--defSpineName={2}.--", this, spineName, defSpineName);
                return null;
            }
            spineName = defSpineName;
        }
        GameObject spine = (GameObject)AssetManager.Instance.Load(ResourcesConfigManager.GetLoadPath(spineName), typeof(GameObject));
        if (null == spine)
        {
            LogUtil.Error("{0}--LoadSpine--spineName={1}--spine=null.--", this, spineName);
            return null;
        }
        return GameObject.Instantiate(spine, spineParent.transform) as GameObject;
    }

    #endregion


    #region Bundle拆分资源工具类
    /// <summary>
    /// FullName，即 BundleSplits/XXX... 中的XXX...路径
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fullname"></param>
    /// <returns></returns>
    public bool IsBundleSplitFullName<T>(string fullname)
    {
        return IsBundleSplitFullName(fullname, typeof(T));
    }
    public bool IsBundleSplitFullName(string fullname, Type type)
    {
        fullname = GetNameWithFileExtention(fullname, type);
        // return DownloadManager3.GetInstance().GetBundleSplitShouldExistFullPath(fullname);
        return false;
    }
    public bool IsBundleSplitFullName<T>(string fullname, out string downloadName)
    {
        return IsBundleSplitFullName(fullname, typeof(T), out downloadName);
    }
    public bool IsBundleSplitFullName(string fullname, Type type, out string downloadName)
    {
        fullname = GetNameWithFileExtention(fullname, type);
        bool exist = false;
        if (exist)
        {
            downloadName = GetSplitDownloadNameForFullName(fullname);
        }
        else
        {
            downloadName = "";
        }
        return exist;
    }

    public string GetSplitDownloadNameForFullName(string fullname)
    {
        return "";
    }

    public bool IsBundleSplitTextureFullName(string assetName, string fileExtention = FileExtensionUtil.png)
    {
        return false;
    }

    /// <summary>
    /// AssetName, BundleSplits/XXX/AAA.... 中的AAA...路径，
    /// 多个资源包并存时，返回第一个命中的 Packname
    /// 注意！！！ 谨慎使用，AAA...可能存在重名，特别是Texture图集资源
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="fullname"></param>
    /// <param name="downloadPackName"></param>
    /// <returns></returns>
    public bool IsBundleSplitAssetName<T>(string fullname, out string downloadPackName)
    {
        return IsBundleSplitAssetName(fullname, typeof(T), out downloadPackName);
    }
    /// <summary>
    /// AssetName, BundleSplits/XXX/AAA.... 中的AAA...路径，
    /// 多个资源包并存时，返回第一个命中的 Packname
    /// 注意！！！ 谨慎使用，AAA...可能存在重名，特别是Texture图集资源
    /// </summary>
    /// <param name="fullname"></param>
    /// <param name="type"></param>
    /// <param name="downloadName"></param>
    /// <returns></returns>
    public bool IsBundleSplitAssetName(string fullname, Type type, out string downloadName)
    {
        fullname = GetNameWithFileExtention(fullname, type);
        downloadName = "";
        return false;
    }


    #endregion
}