using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using WebSocketSharp;


[System.Serializable]
public class GiftEventData
{
    public int type;      // ��Ϣ���ͣ�1��ʾ�����ˡ���2��ʾ�����ԡ���3��ʾ�����ޡ���4��ʾ�����񡱣�5��ʾ����ע��������
    public string uid;    // �û���Ψһ��ʶ��
    public string name;   // �û������ƣ��ǳƻ���ʾ���֣�
    public string url;    // �û���ͷ��URL
    public string msg;    // ��Ϣ���ݣ��û����͵��ı���Ϣ�����������Ϣ��
    public string gift;   // �������ͣ�ָʾ�ͳ����������ࣩ
    public int num;       // ����������ָʾ�ͳ�������������


}
public class WebSocketTest : MonoBehaviour
{

    WebSocket ws;
    int unitNumber;//��λ���
    int numberOfUnits;//��λ����
    //�洢�û���Ӫ
    Dictionary<string, string> userFactions = new Dictionary<string, string>();
    private void Awake()
    {

        Debug.Log(GameManagers.Instance.PlayerJoinText.name);
    }
    void Start()
    {
        ws = new WebSocket("ws://127.0.0.1:3000");
        ws.OnOpen += OnOpenHandler;
        ws.OnMessage += OnMessageHandler;
        ws.OnClose += OnCloseHandler;
        ws.OnError += OnErrorHandler;
        ws.Connect();
    }

    void OnOpenHandler(object sender, System.EventArgs e)
    {
        Debug.Log("WebSocket�����Ѵ�");
    }

    void OnMessageHandler(object sender, MessageEventArgs e)
    {
        //Debug.Log("�յ���Ϣ��" + e.Data);

        //// ��JSON�ַ��������л�ΪGiftEventData����
        //GiftEventData giftMessage = JsonUtility.FromJson<GiftEventData>(e.Data);

        //// ������Ϣ���ͽ��д���
        //switch (giftMessage.type)
        //{
        //    case 1:
        //        //Debug.Log("���ˣ�" + giftMessage.msg);
        //        break;
        //    case 2:
   
        //        Debug.Log(giftMessage.name + "��������Ϣ��" + giftMessage.msg);
        //        if (giftMessage.msg == "1" || giftMessage.msg == "2")
        //        {
        //            Debug.Log(giftMessage.name+"�������");
        //            // ��������Ϊ


        //            if (GameManagers.Instance.joinedPlayers.Contains(giftMessage.uid))
        //            {
        //                Debug.Log("����Ѽ��������");
        //                return;
        //            }
              
        //            GameManagers.Instance.joinedPlayers.Add(giftMessage.uid);
        //            // ��PlayerName��UID��������
        //            GameManagers.Instance.playerNameToUID[giftMessage.name] = giftMessage.uid;
        //            //����ҵ�ͷ��Ҳ�����ֵ䣬����ΪԿ�ף�HeadPortrait������Ϊֵ
        //            GameManagers.Instance.UserProfile[giftMessage.name] = giftMessage.url;
        //            if (giftMessage.msg == "1")
        //            {
        //                Debug.Log(giftMessage.name + "������ħ����Ӫ");
        //                Debug.Log(giftMessage.name + "���ͷ��");
        //                StartCoroutine(LoadImageAndStoreBluePlayerAvatar(giftMessage.url, giftMessage.name));
        //                Debug.Log(giftMessage.name + "�������");
        //                GameManagers.Instance.bluePlayers.Add(giftMessage.name);
        //                userFactions[giftMessage.uid] = "Blue"; // �洢�û���Ӫ��Ϣ

        //                GameManagers.Instance.Instances(/*giftMessage.name + */"������ħ����Ӫ�ɹ�");
        //                Debug.Log(giftMessage.name + "�������2");

        //            }
        //            else if (giftMessage.msg == "2")
        //            {
        //                Debug.Log(giftMessage.name + "����������Ӫ");
        //                Debug.Log(giftMessage.name + "���ͷ��");
        //                StartCoroutine(LoadImageAndStoreRedPlayerAvatar(giftMessage.url, giftMessage.name));
        //                Debug.Log(giftMessage.name + "�������");
        //                GameManagers.Instance.redPlayers.Add(giftMessage.name);
        //                userFactions[giftMessage.uid] = "Red"; // �洢�û���Ӫ��Ϣ

        //                GameManagers.Instance.Instances(/*giftMessage.name + */"����������Ӫ�ɹ�");
        //                Debug.Log(giftMessage.name + "�������2");



        //            }
            
        //        }
        //        break;
        //    case 3:
        //        Debug.Log("Ҫ���ص��ı�������" + GameObject.Find("GameManager").name);
        //        Debug.Log(giftMessage.name + "���ޣ�" + giftMessage.msg);
          
        //        if (GameManagers.Instance.joinedPlayers.Contains(giftMessage.uid))
        //        {
        //            // ���������Ϊ
        //            //Debug.Log("�յ��˵�����Ϣ��" + giftMessage.msg);
        //            //GameManagers.Instance.SpawnUnit(0, 10,/* giftMessage.head_img,*/ giftMessage.name, faction);
        //            //GameManagers.Instance.StartCoroutine(GameManagers.Instance.SpawnUnit(0, 10, /*giftMessage.head_img,*/ giftMessage.name, userFactions[giftMessage.uid]));
        //            string userFaction = userFactions[giftMessage.uid];
        //            GameManagers.Instance.StartCoroutine(GameManagers.Instance.SpawnUnitAsync(UnityEngine.Random.Range(0, 2), 20, giftMessage.name, userFaction));
        //        }
        //        break;
        //    case 4:
        //        Debug.Log("�����û���" + giftMessage.name);
        //        Debug.Log("�������ͣ�" + giftMessage.gift);
        //        Debug.Log("����������" + giftMessage.num);

        //        if (GameManagers.Instance.joinedPlayers.Contains(giftMessage.uid))
        //        {
        //            // ����������Ϊ
        //            //Debug.Log("�յ���������Ϣ��" + giftMessage.msg);
        //            // ������ִ������Ҫ���߼�

        //            if (giftMessage.gift.Contains("С����") || giftMessage.gift.Contains("С����"))
        //            {
        //                unitNumber = 1;
        //                numberOfUnits = giftMessage.num * 20;
        //            }
        //            else if (giftMessage.gift.Contains("��ơ��") || giftMessage.gift.Contains("��call") || giftMessage.gift.Contains("��"))
        //            {
        //                unitNumber = 2;
        //                numberOfUnits = giftMessage.num * 5;
        //            }
        //            else if (giftMessage.gift.Contains("������") || giftMessage.gift.Contains("����ä��"))
        //            {
        //                unitNumber = 3;
        //                numberOfUnits = giftMessage.num * 30;
        //            }
        //            else if (giftMessage.gift.Contains("����Ӵ") || giftMessage.gift.Contains("�ɱ�"))
        //            {
        //                unitNumber = 4;
        //                numberOfUnits = giftMessage.num * 20;
        //            }
        //            else if (giftMessage.gift.Contains("����ֽ��") || giftMessage.gift.Contains("�Ķ�ä��"))
        //            {
        //                unitNumber = 5;
        //                numberOfUnits = giftMessage.num * 10;
        //            }
        //            else if (giftMessage.gift.Contains("��Ͳ") || giftMessage.gift.Contains("�Ǻ�����") || giftMessage.gift.Contains("��Ů��"))
        //            {
        //                unitNumber = 6;
        //                numberOfUnits = giftMessage.num * 5;
        //            }
        //            else if (giftMessage.gift.Contains ( "������") || giftMessage.gift.Contains("��ҫä��"))
        //            {
        //                unitNumber = 7;
        //                numberOfUnits = giftMessage.num * 3;
        //            }
        //            print("���ɵĵ�λ���Ϊ" + unitNumber);
        //            print("���ɵĵ�λ����Ϊ" + numberOfUnits);
        //            print("���ɵĵ�λͼƬΪ" + giftMessage.url);
        //            print("�ٻ��ߵ�����Ϊ" + giftMessage.name);
        //            print("�ٻ��ߵ���ӪΪ" + userFactions[giftMessage.uid]);
        //            string userFaction = userFactions[giftMessage.uid];
        //            GameManagers.Instance.StartCoroutine(GameManagers.Instance.SpawnUnitAsync(unitNumber, numberOfUnits, /*giftMessage.head_img,*/ giftMessage.name, userFaction));

        //        }
        //        break;
        //    case 5:
        //        Debug.Log("��ע������" + giftMessage.msg);
        //        break;
        //    default:
        //        Debug.Log("δ֪��Ϣ���ͣ�" + giftMessage.type);
        //        break;
        //}
    }

    void OnCloseHandler(object sender, CloseEventArgs e)
    {
        Debug.Log("WebSocket�����ѹر�");
    }

    void OnErrorHandler(object sender, ErrorEventArgs e)
    {
        //Debug.Log("WebSocket���ӷ�������" + e.Message);
    }

    void OnDestroy()
    {
        //ws.Close();
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