using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NativeWebSocket;
using Newtonsoft.Json;
using OpenBLive.Runtime;
using OpenBLive.Runtime.Data;
using OpenBLive.Runtime.Utilities;
using UnityEngine;

public class ConnectViaCode : MonoBehaviour
{

    //public static ConnectViaCode Instance
    //{
    //    get
    //    {
    //        if (instance == null)
    //        {
    //            instance = FindObjectOfType<ConnectViaCode>();
    //        }
    //        return instance;
    //    }
    //}

    public static ConnectViaCode Instance;
    private void Awake()
    {
        Instance = this;

    }
    // Start is called before the first frame update
    private WebSocketBLiveClient m_WebSocketBLiveClient;
    private InteractivePlayHeartBeat m_PlayHeartBeat;
    private string gameId;
    public string accessKeySecret;
    public string accessKeyId;
    public string appId;

    public Action ConnectSuccess;
    public Action ConnectFailure;

    // 定义委托类型
    /// <summary>
    /// 购买物品
    /// </summary>
    /// <param name="userName">用户名字</param>
    /// <param name="message">留言内容</param>
    /// <param name="amount">支付金额</param>
    public delegate void SuperChatEventHandler(string userName, string message, float amount);

    /// <summary>
    /// 大航海
    /// </summary>
    /// <param name="userName">用户名字</param>
    /// <param name="guardUnit">购买大航海的日期/月</param>
    public delegate void GuardBuyEventHandler(string userName, string guardUnit);

    /// <summary>
    /// 收到礼物
    /// </summary>
    /// <param name="userName">用户名字</param>
    /// <param name="giftNum">送礼数量</param>
    /// <param name="giftName">送礼名字</param>
    /// <param name="uid">玩家UID</param>
    public delegate void GiftEventHandler(string userName, long giftNum, string giftName, long uid);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="userName">用户名字</param>
    /// <param name="giftName">发送的内容</param>
    /// <param name="uid">UID</param>
    /// <param name="userFace">HeadPortrait</param>
    public delegate void UserSpeech(string userName, string giftName, long uid, string userFace);

    /// <summary>
    /// 点赞/心跳成功
    /// </summary>
    public delegate void HeartBeatSucceedEventHandler();
    /// <summary>
    /// 点赞/心跳失败
    /// </summary>
    public delegate void HeartBeatErrorEventHandler(string json);

    //// 声明事件，使用对应的委托类型
    ///// <summary>
    ///// 用户购买
    ///// </summary>
    //public event SuperChatEventHandler OnSuperChatEvent;
    ///// <summary>
    ///// 大航海
    ///// </summary>
    //public event GuardBuyEventHandler OnGuardBuyEvent;

    /// <summary>
    /// 收到礼物
    /// </summary>
    public event GiftEventHandler OnGiftEvent;

    /// <summary>
    /// 用户发言
    /// </summary>
    public event UserSpeech OnDanmakuEvent;

    /// <summary>
    /// 
    /// </summary>
    public event HeartBeatSucceedEventHandler OnHeartBeatSucceedEvent;
    public event HeartBeatErrorEventHandler OnHeartBeatErrorEvent;

    public async void LinkStart(string code)
    {
        //测试的密钥
        SignUtility.accessKeySecret = accessKeySecret;
        //测试的ID
        SignUtility.accessKeyId = accessKeyId;
        var ret = await BApi.StartInteractivePlay(code, appId);
        //打印到控制台日志
        var gameIdResObj = JsonConvert.DeserializeObject<AppStartInfo>(ret);
        if (gameIdResObj.Code != 0)
        {
            Debug.LogError(gameIdResObj.Message);
            ConnectFailure?.Invoke();
            return;
        }

        m_WebSocketBLiveClient = new WebSocketBLiveClient(gameIdResObj.GetWssLink(), gameIdResObj.GetAuthBody());
        m_WebSocketBLiveClient.OnDanmaku += WebSocketBLiveClientOnDanmaku;
        m_WebSocketBLiveClient.OnGift += WebSocketBLiveClientOnGift;
        //用户购买
        //m_WebSocketBLiveClient.OnGuardBuy += WebSocketBLiveClientOnGuardBuy;
        //大航海
        //m_WebSocketBLiveClient.OnSuperChat += WebSocketBLiveClientOnSuperChat;

        try
        {
            m_WebSocketBLiveClient.Connect(TimeSpan.FromSeconds(1), 1000000);
            ConnectSuccess?.Invoke();
            Debug.Log("连接成功");
        }
        catch (Exception)
        {
            ConnectFailure?.Invoke();
            Debug.Log("连接失败");
            throw;
        }

        gameId = gameIdResObj.GetGameId();
        m_PlayHeartBeat = new InteractivePlayHeartBeat(gameId);
        m_PlayHeartBeat.HeartBeatError += M_PlayHeartBeat_HeartBeatError;
        m_PlayHeartBeat.HeartBeatSucceed += M_PlayHeartBeat_HeartBeatSucceed;
        m_PlayHeartBeat.Start();


    }


    public async Task LinkEnd()
    {
        m_WebSocketBLiveClient.Dispose();
        m_PlayHeartBeat.Dispose();
        await BApi.EndInteractivePlay(appId, gameId);
        Debug.Log("游戏关闭");
    }
    /// <summary>
    /// 用户购买
    /// </summary>
    /// <param name="superChat"></param>
    //private void WebSocketBLiveClientOnSuperChat(SuperChat superChat)
    //{
    //    string userName = superChat.userName;
    //    string message = superChat.message;
    //    float rmb = superChat.rmb;

    //    // 触发事件，传递信息
    //    OnSuperChatEvent?.Invoke(userName, message, rmb);

    //    // ... 其他处理逻辑
    //}
    /// <summary>
    /// 用户大航海
    /// </summary>
    //private void WebSocketBLiveClientOnGuardBuy(Guard guard)
    //{
    //    //StringBuilder sb = new StringBuilder("收到大航海!");
    //    //sb.AppendLine();
    //    //sb.Append("来自用户：");
    //    //sb.AppendLine(guard.userInfo.userName);
    //    //sb.Append("赠送了");
    //    //sb.Append(guard.guardUnit);
    //    //Debug.Log(sb);
    //    string userName = guard.userInfo.userName;
    //    string guardUnit = guard.guardUnit;

    //    // 触发事件，传递信息
    //    OnGuardBuyEvent?.Invoke(userName, guardUnit);
    //}

    private void WebSocketBLiveClientOnGift(SendGift sendGift)
    {
        //StringBuilder sb = new StringBuilder("收到礼物!");
        //sb.AppendLine();
        //sb.Append("来自用户：");
        //sb.AppendLine(sendGift.userName);
        //sb.Append("赠送了");
        //sb.Append(sendGift.giftNum);
        //sb.Append("个");
        //sb.Append(sendGift.giftName);
        //Debug.Log(sb);
        string userName = sendGift.userName;//送礼的角色
        long giftNum = sendGift.giftNum;//送礼的数量
        string giftName = sendGift.giftName;//送礼物的名字
        long uid = sendGift.uid;//送礼物的玩家的uid
        //string userFace = sendGift.userFace;//送礼物的HeadPortrait

        // 触发事件，传递信息
        OnGiftEvent?.Invoke(userName, giftNum, giftName, uid);

    }

    /// <summary>
    /// 用户发言
    /// </summary>
    /// <param name="dm"></param>
    public void WebSocketBLiveClientOnDanmaku(Dm dm)
    {

        string userName = dm.userName;
        string message = dm.msg;
        long uid = dm.uid;
        string userFace = dm.userFace;

        // 触发事件，传递用户名和弹幕内容
        OnDanmakuEvent?.Invoke(userName, message, uid, userFace);
    }


    private static void M_PlayHeartBeat_HeartBeatSucceed()
    {
        Debug.Log("心跳成功");
    }

    private static void M_PlayHeartBeat_HeartBeatError(string json)
    {
        Debug.Log("心跳失败" + json);
    }


    private void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if (m_WebSocketBLiveClient is { ws: { State: WebSocketState.Open } })
        {
            m_WebSocketBLiveClient.ws.DispatchMessageQueue();
        }
#endif
    }

    private void OnDestroy()
    {
        if (m_WebSocketBLiveClient == null)
            return;

        m_PlayHeartBeat.Dispose();
        m_WebSocketBLiveClient.Dispose();
    }
}