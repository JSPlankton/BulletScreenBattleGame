using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public List<Text> redNameTexts; // 红方玩家名次文本
    public List<Image> redAvatarImages; // 红方玩家名次头像图片
    public List<Text> blueNameTexts; // 蓝方玩家名次文本
    public List<Image> blueAvatarImages; // 蓝方玩家名次头像图片

    public GameObject AllPlayerInformation; // 所有玩家名次文本

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
    public static UIManager instance;
    private void Awake()
    {
        instance = this;
    }

    public void UpdateKillRankingDisplay(List<string> redPlayers, List<string> bluePlayers)
    {
        // 更新红队的排名
        UpdateRankingDisplay(redPlayers, redNameTexts, redAvatarImages, "Red");

        // 更新蓝队的排名
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
                playerAvatar = GameManagers.Instance.GetRedPlayerAvatar(playerName);//获取红队玩家头像
                avatarImages[i].sprite = playerAvatar;                              //更新列表
                redAvatarImages[i].sprite = avatarImages[i].sprite;                  //更新头像
            }
            else if (faction == "Blue")
            {
                playerAvatar = GameManagers.Instance.GetBluePlayerAvatar(playerName);
                avatarImages[i].sprite = playerAvatar;
                blueAvatarImages[i].sprite = avatarImages[i].sprite;
            }
        }
    }


   

    private Dictionary<string, Sprite> playerAvatars = new Dictionary<string, Sprite>(); // 存放所有玩家头像的字典

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
        // 更新所有玩家的排名
        List<string> allPlayers = redPlayers.Concat(bluePlayers).ToList();
        if (redVictoryInterface.activeSelf == true)
        {
            AllPlayerInformation = redVictoryInterface.transform.GetChild(4).gameObject;
        }
        else
        {
            AllPlayerInformation = blueVictoryInterface.transform.GetChild(4).gameObject;
        }
        UpdateRankingDisplay(allPlayers, AllPlayerInformation, Color.white);
    }

    private void UpdateRankingDisplay(List<string> players, GameObject allPlayerInformation, Color textColor)
    {

        // 获取每个玩家的击杀数
        Dictionary<string, int> playerKillCounts = new Dictionary<string, int>();
        foreach (string player in players)
        {
            int killCount = GameManagers.Instance.GetUserKillCount(player);
            print(player + "的击杀数为" + killCount);
            playerKillCounts.Add(player, killCount);
        }
        // 根据击杀数排序并获取前十名玩家
        var sortedPlayers = playerKillCounts.OrderByDescending(pair => pair.Value)
                                        .Select(pair => pair.Key)
                                        .Take(10) // 获取前十名玩家
                                        .ToList();

        for (int i = 0; i < sortedPlayers.Count; i++)
        {

            string playerName = sortedPlayers[i];
            GameObject obj = allPlayerInformation.transform.GetChild(i).gameObject;
            obj.transform.GetChild(1).GetComponent<Text>().text = playerName;
            // 获取玩家的击败总数

            obj.transform.GetChild(2).GetComponent<Text>().text = "击败总数：" + playerKillCounts[playerName];

            //nameTexts[i].text = playerName;
            //nameTexts[i].color = textColor; // 设置文本颜色

            if (GetPlayerTeam(playerName) == "Red")
            {
                obj.transform.GetChild(1).GetComponent<Text>().color = redTextColor;
            }
            else if (GetPlayerTeam(playerName) == "Blue")
            {
                obj.transform.GetChild(1).GetComponent<Text>().color = blueTextColor;
            }


            Sprite playerAvatar = null;
            if (playerAvatars.ContainsKey(playerName))
            {
                playerAvatar = playerAvatars[playerName];
            }
            obj.transform.GetChild(0).GetComponent<Image>().sprite = playerAvatar;
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


