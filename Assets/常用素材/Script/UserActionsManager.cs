using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UserActionsManager : MonoBehaviour
{

    int unitNumber;//��λ���
    int numberOfUnits;//��λ����
    //�洢�û���Ӫ
    Dictionary<string, string> userFactions = new Dictionary<string, string>();
    public static UserActionsManager instance;
    private void Awake()
    {
        instance = this;
    }
    // ��������...

    private void Start()
    {
        // ������ʱ���� WebSocketTest ���¼�
        // ������ʱ���� WebSocketTest ���¼�
        WebSocketTest webSocketTest = FindObjectOfType<WebSocketTest>();
        if (webSocketTest != null)
        {
            //webSocketTest.OnMessageReceived += HandleWebSocketMessage;
        }
    }

    // ����WebSocketTest����Ϣ
    private void HandleWebSocketMessage(string jsonMessage)
    {
        // �� JSON �ַ��������л�Ϊ GiftEventData ����
        GiftEventData giftMessage = JsonUtility.FromJson<GiftEventData>(jsonMessage);

        switch (giftMessage.type)
        {

            case 2:
                Debug.Log(giftMessage.name + " ����: " + giftMessage.msg);
                HandleUserMessage(giftMessage.name, giftMessage.msg, giftMessage.uid, giftMessage.url);
                break;
            case 3:
                Debug.Log(giftMessage.name + " ����" );
                Like(giftMessage.name, giftMessage.uid);
                break;
            case 4:
                Debug.Log(giftMessage.name + " ���� " + giftMessage.gift + " �� " + giftMessage.num);
                HandleUserGift(giftMessage.name, giftMessage.gift, giftMessage.num, giftMessage.uid);
                break;
            default:
                // ����������Ϣ����
                break;
        }
    }

    // �û�������Ϊ
    public void HandleUserMessage(string userName, string message, string uid, string url)
    {
        // ������ִ�д����û����Ե��߼�
        Debug.Log(userName + " ����: " + message);
        if (message == "1" || message == "2"||message == "8" || message == "9" || message == "��" || message == "��")
        {
            Debug.Log(userName + "�������");
            // ��������Ϊ


            if (GameManagers.Instance.joinedPlayers.Contains(uid))
            {
                Debug.Log("����Ѽ��������");
                return;
            }

            GameManagers.Instance.joinedPlayers.Add(uid);
            // ��PlayerName��UID��������
            GameManagers.Instance.playerNameToUID[userName] = uid;
            //����ҵ�ͷ��Ҳ�����ֵ䣬����ΪԿ�ף�HeadPortrait������Ϊֵ
            GameManagers.Instance.UserProfile[userName] = url;
            if (message == "1"|| message == "8" || message == "��")
            {
                Debug.Log(userName + "������ħ����Ӫ");
                GameManagers.Instance.bluePlayers.Add(userName);
                userFactions[uid] = "Blue"; // �洢�û���Ӫ��Ϣ

                Debug.Log(GameManagers.Instance.PlayerJoinText.name);
                GameObject obj = Instantiate(GameManagers.Instance.PlayerJoinText, GameManagers.Instance.PlayerJoinTextMainObject.transform);
                Debug.Log(userName + "���ɳɹ�");
                obj.transform.SetParent(GameManagers.Instance.PlayerJoinTextMainObject.transform);
                obj.GetComponent<Text>().text = userName + "������ħ����Ӫ";
                Destroy(obj, 2f);
                Debug.Log(userName + "���ͷ��");
                StartCoroutine(LoadImageAndStoreBluePlayerAvatar(url, userName));
                Debug.Log(userName + "�������");
            }
            else if (message == "2" || message == "9" || message == "��")
            {
                Debug.Log(userName + "����������Ӫ");
                GameManagers.Instance.redPlayers.Add(userName);
                userFactions[uid] = "Red"; // �洢�û���Ӫ��Ϣ
   

                GameObject obj = Instantiate(GameManagers.Instance.PlayerJoinText, GameManagers.Instance.PlayerJoinTextMainObject.transform);
                Debug.Log(userName + "���ɳɹ�");
                obj.transform.SetParent(GameManagers.Instance.PlayerJoinTextMainObject.transform);
                obj.GetComponent<Text>().text = userName + "����������Ӫ�ɹ�";
                Destroy(obj, 2f);
                Debug.Log(userName + "���ͷ��");
                StartCoroutine(LoadImageAndStoreRedPlayerAvatar(url, userName));
                Debug.Log(userName + "�������");
            }

        }
    }

    // �û�������Ϊ
    public void HandleUserGift(string userName, string giftType, int giftCount, string uid)
    {
        // ������ִ�д����û�������߼�
        Debug.Log(userName + " ���� " + giftCount + " �� " + giftType);
        if (GameManagers.Instance.joinedPlayers.Contains(uid))
        {
            // ����������Ϊ
            //Debug.Log("�յ���������Ϣ��" + giftMessage.msg);
            // ������ִ������Ҫ���߼�

            if (giftType.Contains("С����") || giftType.Contains("С����"))
            {
                unitNumber = 1;
                numberOfUnits = giftCount * 20;
            }
            else if (giftType.Contains("��ơ��") || giftType.Contains("��call") || giftType.Contains("��"))
            {
                unitNumber = 2;
                numberOfUnits = giftCount * 5;
            }
            else if (giftType.Contains("������") || giftType.Contains("����ä��"))
            {
                unitNumber = 3;
                numberOfUnits = giftCount * 30;
            }
            else if (giftType.Contains("����Ӵ") || giftType.Contains("�ɱ�"))
            {
                unitNumber = 4;
                numberOfUnits = giftCount * 20;
            }
            else if (giftType.Contains("����ֽ��") || giftType.Contains("�Ķ�ä��"))
            {
                unitNumber = 5;
                numberOfUnits = giftCount * 10;
            }
            else if (giftType.Contains("��Ͳ") || giftType.Contains("�Ǻ�����") || giftType.Contains("��Ů��"))
            {
                unitNumber = 6;
                numberOfUnits = giftCount * 5;
            }
            else if (giftType.Contains("������") || giftType.Contains("��ҫä��"))
            {
                unitNumber = 7;
                numberOfUnits = giftCount * 3;
            }
            print("���ɵĵ�λ���Ϊ" + unitNumber);
            print("���ɵĵ�λ����Ϊ" + numberOfUnits);
            //print("���ɵĵ�λͼƬΪ" + url);
            print("�ٻ��ߵ�����Ϊ" + userName);
            print("�ٻ��ߵ���ӪΪ" + userFactions[uid]);
            string userFaction = userFactions[uid];
            GameManagers.Instance.StartCoroutine(GameManagers.Instance.SpawnUnitAsync(unitNumber, numberOfUnits, /*giftMessage.head_img,*/ userName, userFaction));

        }
    }

    public void Like(string userName, string uid)
    {
        Debug.Log(userName + "���ޣ�");
        string userFaction = userFactions[uid];
        GameManagers.Instance.StartCoroutine(GameManagers.Instance.SpawnUnitAsync(UnityEngine.Random.Range(0, 2), 20, userName, userFaction));
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
                print("�ѻ�ȡ�����ͷ��");
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
