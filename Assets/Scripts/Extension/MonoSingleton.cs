/**
 *	name: 单例模板类 
 *	author:zhaojinlun
 *	date:2021年4月20日
 *	copyright: fungather.net
 ***/

using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T mInstance;

    public static T Instance
    {
        get
        {
            if (mInstance == null)
            {
                string _name = "[" + typeof(T).Name + "]";
                GameObject obj = null;
                if (!Application.isPlaying)
                {
                    obj = GameObject.Find(_name);
                    if (obj)
                    {
                        mInstance = obj.GetComponent<T>();
                    }
                }
                if (obj == null)
                    obj = new GameObject(_name);
                if (mInstance == null)
                    mInstance = obj.AddComponent<T>();
                mInstance.Init();
            }

            return mInstance;
        }
    }

    public static bool HasInstance()
    {
        return mInstance != null;
    }
    protected virtual void Init() { }
    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }

    private void Awake()
    {
        if (mInstance == null)
        {
            if (gameObject != null)
            {
                gameObject.name = "[" + typeof(T).Name + "]";
                mInstance = GetComponent<T>();
            }
        }
        OnAwake();
    }
    private void Start()
    {
        OnStart();
    }
    public void DestroyThis()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        mInstance = null;
    }
}