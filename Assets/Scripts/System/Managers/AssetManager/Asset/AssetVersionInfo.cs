using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AssetVersionList
{
    public List<AssetVersionData> data = new List<AssetVersionData>();
    public static AssetVersionList FromJson(string json)
    {
        return JsonUtility.FromJson<AssetVersionList>(json);
    }

    public void AddData(AssetVersionData aData)
    {
        data.Add(aData);
    }
}
[System.Serializable]
public class AssetVersionData
{
    public string name;
    public string md5;
    public long size;
}