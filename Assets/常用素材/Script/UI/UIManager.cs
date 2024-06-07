using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

public class UIManager : MonoBehaviour
{
    public List<Text> redNameTexts; // 红方玩家名次文本
    public List<Image> redAvatarImages; // 红方玩家名次HeadPortrait图片
    public List<Text> blueNameTexts; // 蓝方玩家名次文本
    public List<Image> blueAvatarImages; // 蓝方玩家名次HeadPortrait图片

    public GameObject AllPlayerInformation; // 所有玩家名次文本

    public Dictionary<string, Sprite> playerAvatars = new Dictionary<string, Sprite>(); // 存放所有玩家HeadPortrait的字典

    public Color redTextColor; // 红队文本颜色
    public Color blueTextColor; // 蓝队文本颜色

    public GameObject redVictoryInterface;
    public GameObject blueVictoryInterface;
    public Image redRankingImage1;
    public Image redRankingImage2;
    public Image redRankingImage3;
    public Image blueRankingImage1;
    public Image blueRankingImage2;
    public Image blueRankingImage3;

    public Text redRankingText1;
    public Text redRankingText2;
    public Text redRankingText3;
    public Text blueRankingText1;
    public Text blueRankingText2;
    public Text blueRankingText3;

    public GameObject RankingBox;
    public static UIManager instance;
    private void Awake()
    {
        instance = this;
    }

    public void UpdateKillRankingDisplay(List<string> redPlayers, List<string> bluePlayers)
    {
        // 更新红队的Ranking
        UpdateRankingDisplay(redPlayers, redNameTexts, redAvatarImages, "Red");

        // 更新蓝队的Ranking
        UpdateRankingDisplay(bluePlayers, blueNameTexts, blueAvatarImages, "Blue");
    }

    private void UpdateRankingDisplay(List<string> players, List<Text> nameTexts, List<Image> avatarImages, string faction)
    {
        // 获取每个玩家的击杀数
        Dictionary<string, int> playerKillCounts = new Dictionary<string, int>();
        foreach (string player in players)
        {
            int killCount = GameManagers.Instance.GetUserKillCount(player);
            print(player + "的击杀数为" + killCount);
            playerKillCounts.Add(player, killCount);
        }

        // 根据击杀数排序并获取前三名玩家
        var sortedPlayers = playerKillCounts.OrderByDescending(pair => pair.Value)
                                            .Select(pair => pair.Key)
                                            .Take(3) // 获取前三名玩家
                                            .ToList();

        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            string playerName = sortedPlayers[i];
            nameTexts[i].text = playerName;
            print(nameTexts[i].text);
            Sprite playerAvatar = null;


            if (faction == "Red")
            {
                playerAvatar = GameManagers.Instance.GetRedPlayerAvatar(playerName);//获取红队玩家HeadPortrait
                avatarImages[i].sprite = playerAvatar;                              //更新列表
                redAvatarImages[i].sprite = avatarImages[i].sprite;                  //更新HeadPortrait
            }
            else if (faction == "Blue")
            {
                playerAvatar = GameManagers.Instance.GetBluePlayerAvatar(playerName);
                avatarImages[i].sprite = playerAvatar;
                blueAvatarImages[i].sprite = avatarImages[i].sprite;
            }
        }
    }






    public void UpdateKillRankingDisplays(List<string> redPlayers, List<string> bluePlayers)
    {


        foreach (var kvp in GameManagers.Instance.redPlayerAvatars)
        {
            playerAvatars.Add(kvp.Key, kvp.Value);
        }

        foreach (var kvp in GameManagers.Instance.bluePlayerAvatars)
        {
            playerAvatars.Add(kvp.Key, kvp.Value);
        }
        // 更新所有玩家的Ranking
        List<string> allPlayers = redPlayers.Concat(bluePlayers).ToList();
        if (redVictoryInterface.activeSelf == true)
        {
            AllPlayerInformation = redVictoryInterface.transform.GetChild(4).transform.GetChild(0).gameObject;
        }
        else
        {
            AllPlayerInformation = blueVictoryInterface.transform.GetChild(4).transform.GetChild(0).gameObject;
        }
        UpdateRankingDisplay(allPlayers, Color.white);
    }

    private void UpdateRankingDisplay(List<string> players, Color textColor)
    {

        // 获取每个玩家的击杀数
        Dictionary<string, int> playerKillCounts = new Dictionary<string, int>();
        foreach (string player in players)
        {
            int killCount = GameManagers.Instance.GetUserKillCount(player);
            print(player + "的击杀数为" + killCount);
            playerKillCounts.Add(player, killCount);
        }
        // 根据击杀数排序所有玩家
        var sortedPlayers = playerKillCounts.OrderByDescending(pair => pair.Value)
                                            .ToList();

        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            string playerName = sortedPlayers[i].Key;//取出第i个名字
            int killCount = sortedPlayers[i].Value;//取出第i个值

            //获取管理HeadPortrait名字的主物体
            //GameObject objs = Instantiate(RankingBox);
            //GameObject objs = allPlayerInformation.transform.GetChild(0).gameObject;
            GameObject obj = Instantiate(RankingBox);
            //obj.transform.SetParent(AllPlayerInformation.transform);
            print("要赋值给的主物体名字为" + AllPlayerInformation.name);
            obj.transform.parent = AllPlayerInformation.transform;
            obj.transform.GetChild(1).GetComponent<Text>().text = playerName;


            // 获取玩家的击败总数
            obj.transform.GetChild(2).GetComponent<Text>().text = "击败总数：" + playerKillCounts[playerName];


            // 根据Ranking显示文本颜色
            if (GetPlayerTeam(playerName) == "Red")
            {
                obj.transform.GetChild(1).GetComponent<Text>().color = redTextColor;
            }
            else if (GetPlayerTeam(playerName) == "Blue")
            {
                obj.transform.GetChild(1).GetComponent<Text>().color = blueTextColor;
            }

            // 设置玩家HeadPortrait
            Sprite playerAvatar = null;
            if (playerAvatars.ContainsKey(playerName))
            {
                playerAvatar = playerAvatars[playerName];
            }
            obj.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = playerAvatar;
            if (i == 0)
            {
                redRankingImage1.sprite = playerAvatar;
                blueRankingImage1.sprite = playerAvatar;
                redRankingText1.text = playerName;
                blueRankingText1.text = playerName;
            }
            else if (i == 1)
            {
                redRankingImage2.sprite = playerAvatar;
                blueRankingImage2.sprite = playerAvatar;
                redRankingText2.text = playerName;
                blueRankingText2.text = playerName;
            }
            else if (i == 2)
            {
                redRankingImage3.sprite = playerAvatar;
                blueRankingImage3.sprite = playerAvatar;
                redRankingText3.text = playerName;
                blueRankingText3.text = playerName;
            }
            // 游戏结束时调用此方法，并传递UID、PlayerName、玩家击杀数量和玩家Scores
            //string uniqueID = "玩家UID"; // 替换为实际的UID
            //         string uniqueIDs;
            //         if (OtherScript.instance != null)
            //         {
            //             long uniqueID = OtherScript.instance.playerNameToUID[playerName];
            //             uniqueIDs = uniqueID.ToString();
            //         }
            //else
            //         {
            string uniqueID = GameManagers.Instance.playerNameToUID[playerName];
            //uniqueIDs = uniqueID;
            //}



            //// 检查玩家是否已有Scores和击杀数数据
            int currentScore = 0;
            int currentKills = 0;
            ////如果有分，先赋值为原来的分
            if (GameManagers.Instance.playerScores.ContainsKey(playerName))
            {
                currentScore = GameManagers.Instance.playerScores[playerName];
            }

            if (GameManagers.Instance.userKillRecords.ContainsKey(playerName))
            {
                currentKills = GameManagers.Instance.userKillRecords[playerName];
            }
            print("上传服务器" + GameManagers.Instance.UserProfile[playerName]);
            MySQL.instance.SavePlayerData(uniqueID, playerName, GameManagers.Instance.UserProfile[playerName], currentKills, currentScore);

            int ranking = GetPlayerRankings(uniqueID);
            obj.transform.GetChild(3).GetComponent<Text>().text = ranking + "";
        }
    }
    public int GetPlayerRankings(string uniqueID)
    {
        // 定义查询所需的参数
        string tableName = "SixArmiesFiredInUnison"; // 假设表名为 SixArmiesFiredInUnison
        string[] cols = { "UID" };
        string[] operations = { "=" };
        string[] values = { uniqueID };
        Debug.Log("Unique ID: " + uniqueID);
        // 调用 SelectWhere 方法进行查询
        var ds = MySQL.mySqlTools.SelectWhere(tableName, cols, operations, values);

        // 检查是否找到了结果
        if (ds.Tables[0].Rows.Count > 0)
        {
            // 获取查询结果中的 Ranking 列值
            int ranking = 9999; // 默认排名，可以根据需要更改
            if (ds.Tables[0].Rows[0]["Ranking"] != DBNull.Value)
            {
                ranking = Convert.ToInt32(ds.Tables[0].Rows[0]["Ranking"]);
            }
            return ranking;
        }
        else
        {
            // 如果没有找到结果，返回默认值或者错误值，视情况而定
            return 9999; // 或者其他默认值
        }
    }

    public int GetPlayerRanking(string playerName)
    {
        // 定义查询所需的参数
        string tableName = "sixarmiesfiredinunison"; // 假设表名为 SixArmiesFiredInUnison
        string[] cols = { "PlayerName" };
        string[] operations = { "=" };
        string[] values = { playerName };

        // 调用 SelectWhere 方法进行查询
        var ds = MySQL.mySqlTools.SelectWhere(tableName, cols, operations, values);

        // 检查是否找到了结果
        if (ds.Tables[0].Rows.Count > 0)
        {
            // 获取查询结果中的 Ranking 列值
            int ranking = Convert.ToInt32(ds.Tables[0].Rows[0]["Ranking"]);

            return ranking;
        }
        else
        {
            // 如果没有找到结果，返回默认值或者错误值，视情况而定
            return 9999; // 或者其他默认值
        }
    }

    private string GetPlayerTeam(string playerName)
    {
        if (GameManagers.Instance.redPlayers.Contains(playerName))
        {
            return "Red";
        }
        else if (GameManagers.Instance.bluePlayers.Contains(playerName))
        {
            return "Blue";
        }
        return null;
    }
}


