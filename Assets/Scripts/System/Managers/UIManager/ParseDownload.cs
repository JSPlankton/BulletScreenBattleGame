//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Threading;
//using UnityEngine;
//using UnityEngine.Networking;
//using Utils;

//public class ParseDownload : MonoBehaviour
//{
//    private struct DownloadParams
//    {
//        public UnityWebRequest request;
//        public Managers.WebRequestManager.OnWebRequestCallback calback;
//        public object userdata;
//    }

//    private int m_maxDownloadThreads = 3;

//    private List<Thread> m_childThreadList = new List<Thread>();

//    private Dictionary<string, DownloadParams> m_paramsList = new Dictionary<string, DownloadParams>();

//    private static ParseDownload _current;
//    public static ParseDownload Current
//    {
//        get
//        {
//            Initialize();
//            return _current;
//        }
//    }

//    void Awake()
//    {
//        _current = this;
//        initialized = true;
//    }

//    void OnDisable()
//    {
//        if (_current == this)
//        {
//            _current = null;
//        }
//        ClearThreads();
//    }

//    static bool initialized = false;

//    static void Initialize()
//    {
//        if (!initialized)
//        {
//            if (!Application.isPlaying)
//            {
//                return;
//            }
//            initialized = true;
//            var g = new GameObject("ParseDownload");
//            _current = g.AddComponent<ParseDownload>();
//        }
//    }

//    /// <summary>
//    /// 清除下载信息
//    /// </summary>
//    public void ClearDownloadInfo()
//    {
//        m_paramsList.Clear();
//    }
//    /// <summary>
//    /// 添加下载信息
//    /// </summary>
//    /// <param name="uri"></param>
//    /// <param name="callback"></param>
//    /// <param name="userdata"></param>
//    public void AddDownloadInfo(string bundle, string uri, Managers.WebRequestManager.OnWebRequestCallback callback, object userdata)
//    {
//        Initialize();
//        if (!m_paramsList.ContainsKey(bundle))
//        {
//            UnityWebRequest request = UnityWebRequest.Get(uri);
//            m_paramsList.Add(bundle, new DownloadParams { request = request, calback = callback, userdata = userdata });
//        }
//    }

//    /// <summary>
//    /// 开始下载
//    /// </summary>
//    public void StartDownload()
//    {
//        try
//        {
//            ClearThreads();
//            for (int i = 0; i < m_maxDownloadThreads; ++i)
//            {
//                Thread thread = new Thread(new ParameterizedThreadStart(OnStartDownloadFile));
//                m_childThreadList.Add(thread);
//            }
//        }
//        catch (Exception e)
//        {
//            LogUtil.Warning("StartDownload--error={0}--", e.Message + e.StackTrace);
//        }
//    }

//    public void OnStartDownload()
//    {
//        try
//        {
//            int index = 0;
//            List<DownloadParams>[] paramsList = new List<DownloadParams>[m_maxDownloadThreads];
//            Dictionary<string, DownloadParams>.Enumerator enumerator = m_paramsList.GetEnumerator();
//            while (enumerator.MoveNext())
//            {
//                int i = index % m_maxDownloadThreads;
//                if (null == paramsList[i])
//                {
//                    paramsList[i] = new List<DownloadParams>();
//                }
//                paramsList[i].Add(enumerator.Current.Value);
//                ++index;
//            }
//            for (int i = 0; i < m_childThreadList.Count; ++i)
//            {
//                m_childThreadList[i].Start(paramsList[i]);
//            }
//            LogUtil.Log("OnStartDownload--end--");
//        }
//        catch (Exception e)
//        {
//            LogUtil.Warning("OnStartDownload--error={0}--", e.Message + e.StackTrace);
//        }
//    }

//    private void ClearThreads()
//    {
//        for (int i = 0; i < m_childThreadList.Count; ++i)
//        {
//            m_childThreadList[i].Abort();
//            m_childThreadList[i] = null;
//        }
//        m_childThreadList.Clear();
//    }

//    private void OnStartDownloadFile(object obj)
//    {
//        LogUtil.Log("OnStartDownload--start--");
//        List<DownloadParams> paramsList = (List<DownloadParams>)obj;
//        for (int i = 0; i < paramsList.Count; ++i)
//        {
//            DownloadParams param = paramsList[i];
//            try
//            {
//                UnityWebRequest request = param.request;
//                UnityWebRequestAsyncOperation operation = request.SendWebRequest();
//                while (!operation.isDone)
//                {
//                    Thread.Sleep(1);
//                }

//                bool isError = false;
//                if (request.result == UnityWebRequest.Result.ProtocolError ||
//                    request.result == UnityWebRequest.Result.ConnectionError ||
//                    request.result == UnityWebRequest.Result.DataProcessingError ||
//                    request.responseCode != 200)
//                {
//                    isError = true;
//                }
//                else
//                {
//                    AssetDownloadInfo info = (AssetDownloadInfo)param.userdata;
//                    string pathFileName = Path.Combine(Application.temporaryCachePath, FileUtil.BundleName, info.bundle);
//                    Managers.DownloadManager.CreateDownloadFile(pathFileName, request.downloadHandler.data);
//                    if (!pathFileName.EndsWith(FileExtensionUtil.manifest))
//                    {
//                        string newMd5 = HashEncryption.MD5FileHash(pathFileName);
//                        if (!string.IsNullOrEmpty(info.md5) && newMd5 != info.md5)
//                        {
//                            FileUtil.DeleteFile(pathFileName);
//                        }
//                        else
//                        {
//                            if (info.bundle.EndsWith(FileExtensionUtil.zip))
//                            {
//                                Managers.DownloadManager.UnzipDownloadFile(pathFileName);
//                            }
//                        }
//                    }
//                }
//                Managers.DownloadManager.QueueOnMainThread(() => {
//                    param.calback?.Invoke(request, isError, param.userdata);
//                });
//                Thread.Sleep(2);
//            }
//            catch (Exception e)
//            {
//                LogUtil.Warning("OnStartDownload--error={0}--", e.Message + e.StackTrace);
//            }
//        }
//    }
//}
