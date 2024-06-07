using System;
using UnityEngine;

public class LoadedAsset
{
    public string name;
    private UnityEngine.Object objRef;
    public Action<string> onAssetUnLoad;
    private int refCount;
    public System.Type type;

    public LoadedAsset(string name, System.Type type, UnityEngine.Object obj)
    {
        this.name = name;
        this.type = type;
        this.objRef = obj;
    }

    private bool IsPrefab(System.Type type)
    {
        return (type == typeof(GameObject));
    }

    private bool NeedUnloadAsset(System.Type assetType)
    {
        if ((assetType != typeof(Texture)) && (assetType != typeof(Texture2D)))
        {
            return false;
        }
        return true;
    }

    public UnityEngine.Object ObjRef
    {
        get
        {
            return this.objRef;
        }
        set
        {
            this.objRef = value;
        }
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
                if (this.NeedUnloadAsset(this.type))
                {
                    Resources.UnloadAsset(this.objRef);
                }
                this.objRef = null;
                this.onAssetUnLoad(this.name);
                this.name = null;
            }
        }
    }
}

