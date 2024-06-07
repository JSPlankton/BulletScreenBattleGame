using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPath : MonoBehaviour
{
    static public Dictionary<string, string> m_uiPaths = new Dictionary<string, string>()
    {
        { "MainUI", "Main/MainUI" },
        { "HeroUIView", "Hero/HeroUIView" },
        { "HeroUIView_HeroItem", "Hero/HeroUIView_HeroItem" },
        { "BagUIView", "Bag/BagUIView" },
        { "BagUIView_BagItem", "Bag/BagUIView_BagItem" },
        { "BagItem", "Bag/BagItem" },
        { "GameMainUI", "Main/GameMainUI" },
        { "GameMainUITop", "Main/GameMainUITop" },
        { "GameMainUIRight", "Main/GameMainUIRight" },
        { "GameMainUILeft", "Main/GameMainUILeft" },
        { "GameMainUIBottom", "Main/GameMainUIBottom" },
        { "LoadingView", "Loading/LoadingView" },
        { "SoldierView", "Soldier/SoldierView" },
        { "SoldierHeadItem", "Soldier/SoldierHeadItem" },
        { "PutBuildTypesView", "Building/PutBuildTypesView" },
        { "PutBuildTypesInfoCell", "Building/PutBuildTypesInfoCell" },
        { "PutBuildTypesCell", "Building/PutBuildTypesCell" },
        { "PutBuildListView", "Building/PutBuildListView" },
        { "PutBuildListCell", "Building/PutBuildListCell" },
        { "PutBuildEditView", "Building/PutBuildEditView" },
        { "BuildingUpgradeView", "Building/BuildingUpgrade" },
        { "BuildingAccelerate", "Building/BuildingAccelerate" },
        { "BuildingActivityItem", "Building/BuildingActivityItem" },
        { "TileCircleMenu", "Menu/TileCircleMenu" },
        { "TileEditArrowNode", "Building/TileEditArrowNode" },
        { "SevenDaysHappy", "Activity/SevenDaysHappy" },
        { "FirstMonthActivity", "Activity/FirstMonthActivity/FirstMonthActivity" }
    };

    /*
    public static string GetPath(string uiname)
    {
        if (m_uiPaths.ContainsKey(uiname))
        {
            return "Prefabs/UI/" + m_uiPaths[uiname];
        }

        return "";
    }
    */
    public static string GetPath(string uiname)
    {
        string path = ResourcesConfigManager.GetLoadPath(uiname);
        return path;
    }

}
