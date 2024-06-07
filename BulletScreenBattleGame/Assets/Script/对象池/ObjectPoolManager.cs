using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public GameObject[] unitPrefabs; // �洢16�ֵ�λԤ����
    public int poolSizePerPrefab = 20; // ÿ��Ԥ�������صĳ�ʼ��С
    public static ObjectPoolManager instance;
    private Dictionary<GameObject, Queue<GameObject>> objectPools = new Dictionary<GameObject, Queue<GameObject>>();

    private void Awake()
    {
        instance = this;
        // �� Awake ��Ϊÿ����λԤ���崴�������
        foreach (GameObject prefab in unitPrefabs)
        {
            CreateObjectPool(prefab);
        }
    }

    void Start()
    {
        //// ������ʱ���ÿ������صĴ�С
        //foreach (var kvp in objectPools)
        //{
        //    Debug.Log($"Object pool for prefab '{kvp.Key.name}' has {kvp.Value.Count} objects.");
        //}
    }

    private void CreateObjectPool(GameObject prefab)
    {
        Queue<GameObject> objectPool = new Queue<GameObject>();

        int poolSize = poolSizePerPrefab;

        // ����Ԥ�����������ò�ͬ�Ķ���ش�С
        if (prefab.name.Contains("ʷ��ķ"))
        {
            poolSize = 5000;
        }
        else
        {
            poolSize = 0;
        }
        // ��ʼ������أ�����ָ�������Ķ��󲢼������
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            objectPool.Enqueue(obj);
        }

        objectPools[prefab] = objectPool;
    }

    // �Ӷ�����л�ȡָ��Ԥ����ĵ�λ����
    public GameObject GetUnitFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (objectPools.ContainsKey(prefab) && objectPools[prefab].Count > 0)
        {
            // �Ӷ������ȡ����������λ�ú���ת��������
            GameObject obj = objectPools[prefab].Dequeue();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.SetActive(true);

            return obj;
        }
        else
        {
            // ��������Ϊ�գ�ʵ����һ���µĶ���
            //Debug.LogWarning("��λ���㣬�����µĶ��� " + prefab.name);
            return Instantiate(prefab, position, rotation);
        }
    }

    // ����λ���󷵻ص��������
    public void ReturnUnitToPool(GameObject prefab, GameObject obj)
    {
        // ����������Ϊ�Ǽ���״̬�����Żض������
        // ֹͣ����Э��

        obj.SetActive(false);
        objectPools[prefab].Enqueue(obj);
        // ������ʱ���ÿ������صĴ�С
        foreach (var kvp in objectPools)
        {
            //Debug.Log($"Object pool for prefab '{kvp.Key.name}' has {kvp.Value.Count} objects.");
        }
    }
}
