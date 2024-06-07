using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;

public interface IGolbalUpdate
{
    void GlobalUpdate();

    void RemoveGlobalUpdate();  //销毁时主动调用
}

public class UpdateUtils
{
    private static List<IGolbalUpdate> updateList = new List<IGolbalUpdate>();
    private static List<IGolbalUpdate> removeList = new List<IGolbalUpdate>();
    public static void AddUpdate(IGolbalUpdate update)
    {
        updateList.Add(update);
    }
    public static void RemoveUpdate(IGolbalUpdate update)
    {
        removeList.Add(update);
    }
    public static void RemoveUpdate()
    {
        while (removeList.Count > 0)
        {
            updateList.Remove(removeList[0]);
            removeList.RemoveAt(0);
        }
    }

    public static void Update()
    {
        RemoveUpdate();
        
        // if (!AppStartupManager.GetInstance().IsLoadingCompleted)
        //     return;

        for (int i = 0; i < updateList.Count; i++)
        {
            if (removeList.IndexOf(updateList[i]) == -1)
            {
                if (updateList[i] == null) continue;
                updateList[i].GlobalUpdate();
            }
        }
    }
}
