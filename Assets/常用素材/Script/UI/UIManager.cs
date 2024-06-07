using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using System;

public class UIManager : MonoBehaviour
{
    public List<Text> redNameTexts; // �췽��������ı�
    public List<Image> redAvatarImages; // �췽�������HeadPortraitͼƬ
    public List<Text> blueNameTexts; // ������������ı�
    public List<Image> blueAvatarImages; // �����������HeadPortraitͼƬ

    public GameObject AllPlayerInformation; // ������������ı�

    public Dictionary<string, Sprite> playerAvatars = new Dictionary<string, Sprite>(); // ����������HeadPortrait���ֵ�

    public Color redTextColor; // ����ı���ɫ
    public Color blueTextColor; // �����ı���ɫ

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
        // ���º�ӵ�Ranking
        UpdateRankingDisplay(redPlayers, redNameTexts, redAvatarImages, "Red");

        // �������ӵ�Ranking
        UpdateRankingDisplay(bluePlayers, blueNameTexts, blueAvatarImages, "Blue");
    }

    private void UpdateRankingDisplay(List<string> players, List<Text> nameTexts, List<Image> avatarImages, string faction)
    {
        // ��ȡÿ����ҵĻ�ɱ��
        Dictionary<string, int> playerKillCounts = new Dictionary<string, int>();
        foreach (string player in players)
        {
            int killCount = GameManagers.Instance.GetUserKillCount(player);
            print(player + "�Ļ�ɱ��Ϊ" + killCount);
            playerKillCounts.Add(player, killCount);
        }

        // ���ݻ�ɱ�����򲢻�ȡǰ�������
        var sortedPlayers = playerKillCounts.OrderByDescending(pair => pair.Value)
                                            .Select(pair => pair.Key)
                                            .Take(3) // ��ȡǰ�������
                                            .ToList();

        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            string playerName = sortedPlayers[i];
            nameTexts[i].text = playerName;
            print(nameTexts[i].text);
            Sprite playerAvatar = null;


            if (faction == "Red")
            {
                playerAvatar = GameManagers.Instance.GetRedPlayerAvatar(playerName);//��ȡ������HeadPortrait
                avatarImages[i].sprite = playerAvatar;                              //�����б�
                redAvatarImages[i].sprite = avatarImages[i].sprite;                  //����HeadPortrait
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
        // ����������ҵ�Ranking
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

        // ��ȡÿ����ҵĻ�ɱ��
        Dictionary<string, int> playerKillCounts = new Dictionary<string, int>();
        foreach (string player in players)
        {
            int killCount = GameManagers.Instance.GetUserKillCount(player);
            print(player + "�Ļ�ɱ��Ϊ" + killCount);
            playerKillCounts.Add(player, killCount);
        }
        // ���ݻ�ɱ�������������
        var sortedPlayers = playerKillCounts.OrderByDescending(pair => pair.Value)
                                            .ToList();

        for (int i = 0; i < sortedPlayers.Count; i++)
        {
            string playerName = sortedPlayers[i].Key;//ȡ����i������
            int killCount = sortedPlayers[i].Value;//ȡ����i��ֵ

            //��ȡ����HeadPortrait���ֵ�������
            //GameObject objs = Instantiate(RankingBox);
            //GameObject objs = allPlayerInformation.transform.GetChild(0).gameObject;
            GameObject obj = Instantiate(RankingBox);
            //obj.transform.SetParent(AllPlayerInformation.transform);
            print("Ҫ��ֵ��������������Ϊ" + AllPlayerInformation.name);
            obj.transform.parent = AllPlayerInformation.transform;
            obj.transform.GetChild(1).GetComponent<Text>().text = playerName;


            // ��ȡ��ҵĻ�������
            obj.transform.GetChild(2).GetComponent<Text>().text = "����������" + playerKillCounts[playerName];


            // ����Ranking��ʾ�ı���ɫ
            if (GetPlayerTeam(playerName) == "Red")
            {
                obj.transform.GetChild(1).GetComponent<Text>().color = redTextColor;
            }
            else if (GetPlayerTeam(playerName) == "Blue")
            {
                obj.transform.GetChild(1).GetComponent<Text>().color = blueTextColor;
            }

            // �������HeadPortrait
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
            // ��Ϸ����ʱ���ô˷�����������UID��PlayerName����һ�ɱ���������Scores
            //string uniqueID = "���UID"; // �滻Ϊʵ�ʵ�UID
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



            //// �������Ƿ�����Scores�ͻ�ɱ������
            int currentScore = 0;
            int currentKills = 0;
            ////����з֣��ȸ�ֵΪԭ���ķ�
            if (GameManagers.Instance.playerScores.ContainsKey(playerName))
            {
                currentScore = GameManagers.Instance.playerScores[playerName];
            }

            if (GameManagers.Instance.userKillRecords.ContainsKey(playerName))
            {
                currentKills = GameManagers.Instance.userKillRecords[playerName];
            }
            print("�ϴ�������" + GameManagers.Instance.UserProfile[playerName]);
            MySQL.instance.SavePlayerData(uniqueID, playerName, GameManagers.Instance.UserProfile[playerName], currentKills, currentScore);

            int ranking = GetPlayerRankings(uniqueID);
            obj.transform.GetChild(3).GetComponent<Text>().text = ranking + "";
        }
    }
    public int GetPlayerRankings(string uniqueID)
    {
        // �����ѯ����Ĳ���
        string tableName = "SixArmiesFiredInUnison"; // �������Ϊ SixArmiesFiredInUnison
        string[] cols = { "UID" };
        string[] operations = { "=" };
        string[] values = { uniqueID };
        Debug.Log("Unique ID: " + uniqueID);
        // ���� SelectWhere �������в�ѯ
        var ds = MySQL.mySqlTools.SelectWhere(tableName, cols, operations, values);

        // ����Ƿ��ҵ��˽��
        if (ds.Tables[0].Rows.Count > 0)
        {
            // ��ȡ��ѯ����е� Ranking ��ֵ
            int ranking = 9999; // Ĭ�����������Ը�����Ҫ����
            if (ds.Tables[0].Rows[0]["Ranking"] != DBNull.Value)
            {
                ranking = Convert.ToInt32(ds.Tables[0].Rows[0]["Ranking"]);
            }
            return ranking;
        }
        else
        {
            // ���û���ҵ����������Ĭ��ֵ���ߴ���ֵ�����������
            return 9999; // ��������Ĭ��ֵ
        }
    }

    public int GetPlayerRanking(string playerName)
    {
        // �����ѯ����Ĳ���
        string tableName = "sixarmiesfiredinunison"; // �������Ϊ SixArmiesFiredInUnison
        string[] cols = { "PlayerName" };
        string[] operations = { "=" };
        string[] values = { playerName };

        // ���� SelectWhere �������в�ѯ
        var ds = MySQL.mySqlTools.SelectWhere(tableName, cols, operations, values);

        // ����Ƿ��ҵ��˽��
        if (ds.Tables[0].Rows.Count > 0)
        {
            // ��ȡ��ѯ����е� Ranking ��ֵ
            int ranking = Convert.ToInt32(ds.Tables[0].Rows[0]["Ranking"]);

            return ranking;
        }
        else
        {
            // ���û���ҵ����������Ĭ��ֵ���ߴ���ֵ�����������
            return 9999; // ��������Ĭ��ֵ
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


