using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public GameObject[] unitPrefabs; // 存储16种单位预制体
    public int poolSizePerPrefab = 20; // 每个预制体对象池的初始大小
    public static ObjectPoolManager instance;
    private Dictionary<GameObject, Queue<GameObject>> objectPools = new Dictionary<GameObject, Queue<GameObject>>();

    private void Awake()
    {
        instance = this;
        // 在 Awake 中为每个单位预制体创建对象池
        foreach (GameObject prefab in unitPrefabs)
        {
            CreateObjectPool(prefab);
        }
    }

    void Start()
    {
        //// 在启动时输出每个对象池的大小
        //foreach (var kvp in objectPools)
        //{
        //    Debug.Log($"Object pool for prefab '{kvp.Key.name}' has {kvp.Value.Count} objects.");
        //}
    }

    private void CreateObjectPool(GameObject prefab)
    {
        Queue<GameObject> objectPool = new Queue<GameObject>();

        int poolSize = poolSizePerPrefab;

        // 根据预制体类型设置不同的对象池大小
        if (prefab.name.Contains("史莱姆"))
        {
            poolSize = 5000;
        }
        else
        {
            poolSize = 0;
        }
        // 初始化对象池，创建指定数量的对象并加入队列
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        objectPools[prefab] = objectPool;
    }

    // 从对象池中获取指定预制体的单位对象
    public GameObject GetUnitFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (objectPools.ContainsKey(prefab) && objectPools[prefab].Count > 0)
        {
            // 从对象池中取出对象，设置位置和旋转，并激活
            GameObject obj = objectPools[prefab].Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);

            return obj;
        }
        else
        {
            // 如果对象池为空，实例化一个新的对象
            //Debug.LogWarning("单位不足，创建新的对象 " + prefab.name);
            return Instantiate(prefab, position, rotation);
        }
    }

    // 将单位对象返回到对象池中
    public void ReturnUnitToPool(GameObject prefab, GameObject obj)
    {
        // 将对象设置为非激活状态，并放回对象池中
        // 停止所有协程

        obj.SetActive(false);
        objectPools[prefab].Enqueue(obj);
        // 在启动时输出每个对象池的大小
        foreach (var kvp in objectPools)
        {
            //Debug.Log($"Object pool for prefab '{kvp.Key.name}' has {kvp.Value.Count} objects.");
        }
    }
}
