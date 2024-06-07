using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OtherScript : MonoBehaviour
{

    int unitNumber;//��λ���
    long numberOfUnits;//��λ����
    string faction;

    Dictionary<long, string> userFactions = new Dictionary<long, string>();
    private void Start()
    {
        ConnectViaCode connectViaCode = FindObjectOfType<ConnectViaCode>();

        // �����¼���ָ���¼�������
        //������Ʒ
        //connectViaCode.OnSuperChatEvent += HandleSuperChatEvent;
        //�󺽺�
        //connectViaCode.OnGuardBuyEvent += HandleGuardBuyEvent;
        //����
        connectViaCode.OnGiftEvent += HandleGiftEvent;


        // ����
        connectViaCode.OnDanmakuEvent += WebSocketBLiveClientOnDanmaku;


        connectViaCode.OnHeartBeatSucceedEvent += HandleHeartBeatSucceedEvent;
        connectViaCode.OnHeartBeatErrorEvent += HandleHeartBeatErrorEvent;
    }
    private void WebSocketBLiveClientOnDanmaku(string userName, string message, long uid, string userFace)
    {
        Debug.Log($"�յ���Ļ���û���{userName}�����ݣ�{message}");
        if (message == "1" || message == "2")
        {
            if (GameManagers.Instance.joinedPlayers.Contains(uid.ToString()))
            {
                Debug.Log("����Ѽ��������");
                return;
            }
            GameManagers.Instance.joinedPlayers.Add(uid.ToString());


            if (message == "1")
            {
                GameManagers.Instance.bluePlayers.Add(userName);
                userFactions[uid] = "Blue"; // �洢�û���Ӫ��Ϣ
                GameObject obj = Instantiate(GameManagers.Instance.PlayerJoinText, GameManagers.Instance.PlayerJoinTextMainObject.transform);
                obj.transform.SetParent(GameManagers.Instance.PlayerJoinTextMainObject.transform);
                obj.GetComponent<Text>().text = userName + "������ħ����Ӫ";
                Destroy(obj, 2f);
                StartCoroutine(LoadImageAndStoreBluePlayerAvatar(userFace, userName));
            }
            else if (message == "2")
            {
                GameManagers.Instance.redPlayers.Add(userName);
                userFactions[uid] = "Red"; // �洢�û���Ӫ��Ϣ
                GameObject obj = Instantiate(GameManagers.Instance.PlayerJoinText, GameManagers.Instance.PlayerJoinTextMainObject.transform);
                obj.transform.SetParent(GameManagers.Instance.PlayerJoinTextMainObject.transform);
                obj.GetComponent<Text>().text = userName + "����������Ӫ";
                Destroy(obj, 2f);
                StartCoroutine(LoadImageAndStoreRedPlayerAvatar(userFace, userName));
            }
        }

        if (message == "666")
        {
            if (GameManagers.Instance.joinedPlayers.Contains(uid.ToString()))
            {
                print("���ɵĵ�λ���Ϊ" + unitNumber);
                print("���ɵĵ�λ����Ϊ" + numberOfUnits);
                print("�ٻ��ߵ�����Ϊ" + userName);
                print("�ٻ��ߵ�����Ϊ" + uid);
                print("�ٻ��ߵ���ӪΪ" + faction);
                if (userFactions.ContainsKey(uid))
                {
                    string userFaction = userFactions[uid];
                    // ������ʹ��userFaction�������ٻ���λ����Ӫ
                    // �����߼�...
                    GameManagers.Instance.StartCoroutine(GameManagers.Instance.SpawnUnit(Random.Range(0, 2), 20, userName, userFaction));
                }
               
            }
  
        }






    }
    // �¼������������ SuperChat ����
    //private void HandleSuperChatEvent(string userName, string message, float amount)
    //{
    //    Debug.Log($"�յ�SC���û���{userName}���������ݣ�{message}����{amount}Ԫ");
    //}

    // �¼������������ GuardBuy ����
    //private void HandleGuardBuyEvent(string userName, string guardUnit)
    //{
    //    Debug.Log($"�յ��󺽺����û���{userName}��������{guardUnit}");
    //}

    // �¼������������ Gift ����
    private void HandleGiftEvent(string userName, long giftNum, string giftName, long uid)
    {
        Debug.Log($"�յ�����û���{userName}��������{giftNum}��{giftName}");


        if (GameManagers.Instance.joinedPlayers.Contains(uid.ToString()))
        {
            // ����������Ϊ
            Debug.Log($"�յ�����û���{userName}��������{giftNum}��{giftName}");
            // ������ִ������Ҫ���߼�

            if (giftName.Contains("ħ��֮ʯ"))
            {
                unitNumber = 2;
                numberOfUnits = giftNum * 5;
            }
            else if (giftName.Contains("����֮��") )
            {
                unitNumber = 3;
                numberOfUnits = giftNum * 20;
            }
            else if (giftName.Contains("ʥ�߷���") )
            {
                unitNumber = 4;
                numberOfUnits = giftNum * 10;
            }
            else if (giftName.Contains("��ʿȨ��"))
            {
                unitNumber = 5;
                numberOfUnits = giftNum * 5;
            }
            else if (giftName == "ħ������")
            {
                unitNumber = 6;
                numberOfUnits = giftNum * 3;
            }
            string userFaction = userFactions[uid];
            print("���ɵĵ�λ���Ϊ" + unitNumber);
            print("���ɵĵ�λ����Ϊ" + numberOfUnits);
            print("�ٻ��ߵ�����Ϊ" + userName);
            print("�ٻ��ߵ�UIDΪ" + uid);
            print("�ٻ��ߵ���ӪΪ" + userFaction);

            GameManagers.Instance.StartCoroutine(GameManagers.Instance.SpawnUnit(unitNumber, (int)numberOfUnits, userName, userFaction));

        }



    }

    // �¼������������ HeartBeat �ɹ�
    private void HandleHeartBeatSucceedEvent()
    {
        Debug.Log("�����ɹ�");
    }

    // �¼������������ HeartBeat ������Ϣ
    private void HandleHeartBeatErrorEvent(string errorJson)
    {
        Debug.Log($"����ʧ�� {errorJson}");
    }



    private IEnumerator LoadImageAndStoreRedPlayerAvatar(string imageUrl, string playerName)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                print("û�л�ȡ�����ͷ��");
            }
            else
            {
                print("�ѻ�ȡ�����ͷ��"+ imageUrl);
                Texture2D texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                GameManagers.Instance.AddRedPlayerAvatar(playerName, sprite);//��ȡ�췽ͷ����ͷ��
            }
        }
    }
    private IEnumerator LoadImageAndStoreBluePlayerAvatar(string imageUrl, string playerName)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error loading image: " + webRequest.error);
            }
            else
            {
                Texture2D texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                GameManagers.Instance.AddBluePlayerAvatar(playerName, sprite);
            }
        }
    }
}
