using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using BestHTTP.WebSocket;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using UnityEngine.Networking;

public enum UserBehavior
{
    /// <summary>
    /// ������Ϊ
    /// </summary>
    ChatMessage,

    /// <summary>
    /// ������Ϊ
    /// </summary>
    LikeMessage,

    /// <summary>
    /// ������Ϊ
    /// </summary>
    GiftMessage,
}

public class websocket_01 : MonoBehaviour
{
    UserBehavior userBehavior;

    int unitNumber;//��λ���
    int numberOfUnits;//��λ����


    private void Start()
    {
        print("����websocket");
        Init();
        Connect();
    }
    //public string url = "ws://127.0.0.1:9999";
    public string url = "ws://127.0.0.1:9999";
    private WebSocket webSocket;

    private void Init()
    {
        webSocket = new WebSocket(new Uri(url));
        webSocket.OnOpen += OnOpen;
        webSocket.OnMessage += OnMessageReceived;
        webSocket.OnError += OnError;
        webSocket.OnClosed += OnClosed;
    }
    public void Connect()
    {
        webSocket.Open();
        print("��������");
    }
    void OnOpen(WebSocket ws)
    {
        print("���ӳɹ�");
    }
    void OnMessageReceived(WebSocket ws, string msg)
    {
        Debug.Log(msg);
        GiftMessage giftMessage = JsonUtility.FromJson<GiftMessage>(msg);
        // ʹ�� switch �����ݲ�ͬ����Ϊ����ִ�в�ͬ�Ĳ���
        switch (giftMessage.type)
        {
            case "ChatMessage":
                if (giftMessage.content != "1" && giftMessage.content != "2")
                {
                    return;
                }
                // ��������Ϊ
                Debug.Log("�յ��˷�����Ϣ��" + giftMessage.content);
                if (GameManagers.Instance.joinedPlayers.Contains(giftMessage.uid))
                {
                    Debug.Log("����Ѽ��������");
                    return;
                }

                //�����UID���������Ϸ���ֵ�,���ֹ���UID�����ֹ���ͼƬ
                GameManagers.Instance.PlayerAddDictionary(giftMessage.name, giftMessage.uid, giftMessage.head_img);


                if (giftMessage.content == "1")
                {

                    GameManagers.Instance.Instances(giftMessage.uid, giftMessage.name, "������ħ����Ӫ", "Blue", giftMessage.head_img);

                }
                else if (giftMessage.content == "2")
                {
                    GameManagers.Instance.Instances(giftMessage.uid, giftMessage.name, "��������ʹ��Ӫ", "Red", giftMessage.head_img);

                }

                break;

            case "LikeMessage":

                if (GameManagers.Instance.joinedPlayers.Contains(giftMessage.uid))
                {
                    // ���������Ϊ
                    Debug.Log("�յ��˵�����Ϣ��" + giftMessage.content);

                    string userFaction = GameManagers.Instance.userFactions[giftMessage.uid];
                    GameManagers.Instance.StartCoroutine(GameManagers.Instance.SpawnUnitAsync(UnityEngine.Random.Range(0, 2), 20, giftMessage.name, userFaction));
                }
                break;

            case "GiftMessage":

                if (GameManagers.Instance.joinedPlayers.Contains(giftMessage.uid))
                {
                    // ����������Ϊ
                    Debug.Log("�յ���������Ϣ��" + giftMessage.content);
                    // ������ִ������Ҫ���߼�

                    if (giftMessage.giftName.Contains("С����") || giftMessage.giftName.Contains("С����"))
                    {
                        unitNumber = 1;
                        numberOfUnits = int.Parse(giftMessage.giftCount) * 20;
                    }
                    else if (giftMessage.giftName.Contains("��ơ��") || giftMessage.giftName.Contains("��call") || giftMessage.giftName.Contains("��"))
                    {
                        unitNumber = 2;
                        numberOfUnits = int.Parse(giftMessage.giftCount) * 5;
                    }
                    //else if (giftMessage.giftName.Contains("������") || giftMessage.giftName.Contains("����ä��"))
                    //{
                    //    unitNumber = 2;
                    //    numberOfUnits = int.Parse(giftMessage.giftCount) * 30;
                    //}
                    else if (giftMessage.giftName.Contains("����Ӵ") || giftMessage.giftName.Contains("�ɱ�"))
                    {
                        unitNumber = 3;
                        numberOfUnits = int.Parse(giftMessage.giftCount) * 20;
                    }
                    else if (giftMessage.giftName.Contains("����ֽ��") || giftMessage.giftName.Contains("�Ķ�ä��"))
                    {
                        unitNumber = 4;
                        numberOfUnits = int.Parse(giftMessage.giftCount) * 10;
                    }
                    else if (giftMessage.giftName.Contains("��Ͳ") || giftMessage.giftName.Contains("�Ǻ�����") || giftMessage.giftName.Contains("��Ů��"))
                    {
                        unitNumber = 5;
                        numberOfUnits = int.Parse(giftMessage.giftCount) * 5;
                    }
                    else if (giftMessage.giftName == "������" || giftMessage.giftName.Contains("��ҫä��"))
                    {
                        unitNumber = 6;
                        numberOfUnits = int.Parse(giftMessage.giftCount) * 3;
                    }
                    print("���ɵĵ�λ���Ϊ" + unitNumber);
                    print("���ɵĵ�λ����Ϊ" + numberOfUnits);
                    print("���ɵĵ�λͼƬΪ" + giftMessage.head_img);
                    print("�ٻ��ߵ�����Ϊ" + giftMessage.name);
                    print("�ٻ��ߵ���ӪΪ" + GameManagers.Instance.userFactions[giftMessage.uid]);
                    string userFaction = GameManagers.Instance.userFactions[giftMessage.uid];
                    GameManagers.Instance.StartCoroutine(GameManagers.Instance.SpawnUnitAsync(unitNumber, numberOfUnits, /*giftMessage.head_img,*/ giftMessage.name, userFaction));

                }

                break;

            default:
                Debug.Log("δ֪��Ϊ���ͣ�" + giftMessage.type);
                break;
        }
        // ��������Է��ʷ����л�������ֶβ����в���
        //Debug.Log("��Ϣ���ͣ�" + giftMessage.type);
        //Debug.Log("���������ƣ�" + giftMessage.name);
        //Debug.Log("������Ψһ��ʶ��" + giftMessage.uid);
        //Debug.Log("������ͷ�����ӣ�" + giftMessage.head_img);
        //Debug.Log("����ID��" + giftMessage.giftId);
        //Debug.Log("����������" + giftMessage.giftCount);
        //Debug.Log("�������ƣ�" + giftMessage.giftName);
        //Debug.Log("��Ϣ���ݣ�" + giftMessage.content);
        //Debug.Log("����������" + giftMessage.count);
        //Debug.Log("�ܵ���������" + giftMessage.total);

    }

   
    void OnClosed(WebSocket ws, UInt16 code, string message)
    {
        Debug.Log(message);
        webSocket.Close();
    }
    private void OnDestroy()
    {
        if (webSocket != null && webSocket.IsOpen)
        {
            Debug.Log("���ӶϿ�");
            webSocket.Close();
        }
    }
    void OnError(WebSocket ws, Exception ex)
    {
        Debug.Log("WebSocket����:" + ex.Message);
        webSocket.Close();
    }
}