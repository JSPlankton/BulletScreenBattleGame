using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public List<Text> redNameTexts; // �췽��������ı�
    public List<Image> redAvatarImages; // �췽�������ͷ��ͼƬ
    public List<Text> blueNameTexts; // ������������ı�
    public List<Image> blueAvatarImages; // �����������ͷ��ͼƬ

    public GameObject AllPlayerInformation; // ������������ı�

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
    public static UIManager instance;
    private void Awake()
    {
        instance = this;
    }

    public void UpdateKillRankingDisplay(List<string> redPlayers, List<string> bluePlayers)
    {
        // ���º�ӵ�����
        UpdateRankingDisplay(redPlayers, redNameTexts, redAvatarImages, "Red");

        // �������ӵ�����
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
                playerAvatar = GameManagers.Instance.GetRedPlayerAvatar(playerName);//��ȡ������ͷ��
                avatarImages[i].sprite = playerAvatar;                              //�����б�
                redAvatarImages[i].sprite = avatarImages[i].sprite;                  //����ͷ��
            }
            else if (faction == "Blue")
            {
                playerAvatar = GameManagers.Instance.GetBluePlayerAvatar(playerName);
                avatarImages[i].sprite = playerAvatar;
                blueAvatarImages[i].sprite = avatarImages[i].sprite;
            }
        }
    }


   

    private Dictionary<string, Sprite> playerAvatars = new Dictionary<string, Sprite>(); // ����������ͷ����ֵ�

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
        // ����������ҵ�����
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

        // ��ȡÿ����ҵĻ�ɱ��
        Dictionary<string, int> playerKillCounts = new Dictionary<string, int>();
        foreach (string player in players)
        {
            int killCount = GameManagers.Instance.GetUserKillCount(player);
            print(player + "�Ļ�ɱ��Ϊ" + killCount);
            playerKillCounts.Add(player, killCount);
        }
        // ���ݻ�ɱ�����򲢻�ȡǰʮ�����
        var sortedPlayers = playerKillCounts.OrderByDescending(pair => pair.Value)
                                        .Select(pair => pair.Key)
                                        .Take(10) // ��ȡǰʮ�����
                                        .ToList();

        for (int i = 0; i < sortedPlayers.Count; i++)
        {

            string playerName = sortedPlayers[i];
            GameObject obj = allPlayerInformation.transform.GetChild(i).gameObject;
            obj.transform.GetChild(1).GetComponent<Text>().text = playerName;
            // ��ȡ��ҵĻ�������

            obj.transform.GetChild(2).GetComponent<Text>().text = "����������" + playerKillCounts[playerName];

            //nameTexts[i].text = playerName;
            //nameTexts[i].color = textColor; // �����ı���ɫ

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


