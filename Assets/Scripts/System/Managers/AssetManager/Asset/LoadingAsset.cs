using System;
using UnityEngine;

public class LoadingAsset
{
    public AssetBundleRequest abr;
    public string bundlename;
    public Action<UnityEngine.Object, bool> callback;
    internal bool isLoading;
    public string name;
    private int refCount;
    public System.Type type;

    public LoadingAsset(string name, System.Type type, string bundlename, Action<UnityEngine.Object, bool> callback)
    {
        this.name = name;
        this.type = type;
        this.bundlename = bundlename;
        this.callback = callback;
    }

    public int RefCount
    {
        get
        {
            return this.refCount;
        }
        set
        {
            this.refCount = value;
            if (this.refCount == 0)
            {
                this.name = null;
            }
        }
    }
}

