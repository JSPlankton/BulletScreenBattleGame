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
    /// 发言行为
    /// </summary>
    ChatMessage,

    /// <summary>
    /// 发言行为
    /// </summary>
    LikeMessage,

    /// <summary>
    /// 发言行为
    /// </summary>
    GiftMessage,
}

public class websocket_01 : MonoBehaviour
{
    UserBehavior userBehavior;

    int unitNumber;//单位编号
    int numberOfUnits;//单位数量


    private void Start()
    {
        print("开启websocket");
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
        print("发送连接");
    }
    void OnOpen(WebSocket ws)
    {
        print("连接成功");
    }
    void OnMessageReceived(WebSocket ws, string msg)
    {
        Debug.Log(msg);
        GiftMessage giftMessage = JsonUtility.FromJson<GiftMessage>(msg);
        // 使用 switch 语句根据不同的行为类型执行不同的操作
        switch (giftMessage.type)
        {
            case "ChatMessage":
                if (giftMessage.content != "1" && giftMessage.content != "2")
                {
                    return;
                }
                // 处理发言行为
                Debug.Log("收到了发言消息：" + giftMessage.content);
                if (GameManagers.Instance.joinedPlayers.Contains(giftMessage.uid))
                {
                    Debug.Log("玩家已加入过队伍");
                    return;
                }

                //将玩家UID放入加入游戏的字典,名字关联UID，名字关联图片
                GameManagers.Instance.PlayerAddDictionary(giftMessage.name, giftMessage.uid, giftMessage.head_img);


                if (giftMessage.content == "1")
                {

                    GameManagers.Instance.Instances(giftMessage.uid, giftMessage.name, "加入了魔神阵营", "Blue", giftMessage.head_img);

                }
                else if (giftMessage.content == "2")
                {
                    GameManagers.Instance.Instances(giftMessage.uid, giftMessage.name, "加入了天使阵营", "Red", giftMessage.head_img);

                }

                break;

            case "LikeMessage":

                if (GameManagers.Instance.joinedPlayers.Contains(giftMessage.uid))
                {
                    // 处理点赞行为
                    Debug.Log("收到了点赞消息：" + giftMessage.content);

                    string userFaction = GameManagers.Instance.userFactions[giftMessage.uid];
                    GameManagers.Instance.StartCoroutine(GameManagers.Instance.SpawnUnitAsync(UnityEngine.Random.Range(0, 2), 20, giftMessage.name, userFaction));
                }
                break;

            case "GiftMessage":

                if (GameManagers.Instance.joinedPlayers.Contains(giftMessage.uid))
                {
                    // 处理礼物行为
                    Debug.Log("收到了礼物消息：" + giftMessage.content);
                    // 在这里执行你想要的逻辑

                    if (giftMessage.giftName.Contains("小心心") || giftMessage.giftName.Contains("小花花"))
                    {
                        unitNumber = 1;
                        numberOfUnits = int.Parse(giftMessage.giftCount) * 20;
                    }
                    else if (giftMessage.giftName.Contains("大啤酒") || giftMessage.giftName.Contains("打call") || giftMessage.giftName.Contains("打"))
                    {
                        unitNumber = 2;
                        numberOfUnits = int.Parse(giftMessage.giftCount) * 5;
                    }
                    //else if (giftMessage.giftName.Contains("棒棒糖") || giftMessage.giftName.Contains("星月盲盒"))
                    //{
                    //    unitNumber = 2;
                    //    numberOfUnits = int.Parse(giftMessage.giftCount) * 30;
                    //}
                    else if (giftMessage.giftName.Contains("爱你哟") || giftMessage.giftName.Contains("干杯"))
                    {
                        unitNumber = 3;
                        numberOfUnits = int.Parse(giftMessage.giftCount) * 20;
                    }
                    else if (giftMessage.giftName.Contains("爱的纸鹤") || giftMessage.giftName.Contains("心动盲盒"))
                    {
                        unitNumber = 4;
                        numberOfUnits = int.Parse(giftMessage.giftCount) * 10;
                    }
                    else if (giftMessage.giftName.Contains("礼花筒") || giftMessage.giftName.Contains("星河入梦") || giftMessage.giftName.Contains("仙女棒"))
                    {
                        unitNumber = 5;
                        numberOfUnits = int.Parse(giftMessage.giftCount) * 5;
                    }
                    else if (giftMessage.giftName == "热气球" || giftMessage.giftName.Contains("闪耀盲盒"))
                    {
                        unitNumber = 6;
                        numberOfUnits = int.Parse(giftMessage.giftCount) * 3;
                    }
                    print("生成的单位编号为" + unitNumber);
                    print("生成的单位数量为" + numberOfUnits);
                    print("生成的单位图片为" + giftMessage.head_img);
                    print("召唤者的名字为" + giftMessage.name);
                    print("召唤者的阵营为" + GameManagers.Instance.userFactions[giftMessage.uid]);
                    string userFaction = GameManagers.Instance.userFactions[giftMessage.uid];
                    GameManagers.Instance.StartCoroutine(GameManagers.Instance.SpawnUnitAsync(unitNumber, numberOfUnits, /*giftMessage.head_img,*/ giftMessage.name, userFaction));

                }

                break;

            default:
                Debug.Log("未知行为类型：" + giftMessage.type);
                break;
        }
        // 现在你可以访问反序列化对象的字段并进行操作
        //Debug.Log("消息类型：" + giftMessage.type);
        //Debug.Log("发送者名称：" + giftMessage.name);
        //Debug.Log("发送者唯一标识：" + giftMessage.uid);
        //Debug.Log("发送者头像链接：" + giftMessage.head_img);
        //Debug.Log("礼物ID：" + giftMessage.giftId);
        //Debug.Log("礼物数量：" + giftMessage.giftCount);
        //Debug.Log("礼物名称：" + giftMessage.giftName);
        //Debug.Log("消息内容：" + giftMessage.content);
        //Debug.Log("点赞数量：" + giftMessage.count);
        //Debug.Log("总点赞数量：" + giftMessage.total);

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
            Debug.Log("连接断开");
            webSocket.Close();
        }
    }
    void OnError(WebSocket ws, Exception ex)
    {
        Debug.Log("WebSocket出错:" + ex.Message);
        webSocket.Close();
    }
}