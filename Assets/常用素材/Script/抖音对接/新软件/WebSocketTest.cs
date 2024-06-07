using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using WebSocketSharp;


[System.Serializable]
public class GiftEventData
{
    public int type;      // 消息类型（1表示“来了”，2表示“发言”，3表示“点赞”，4表示“送礼”，5表示“关注主播”）
    public string uid;    // 用户的唯一标识符
    public string name;   // 用户的名称（昵称或显示名字）
    public string url;    // 用户的头像URL
    public string msg;    // 消息内容（用户发送的文本消息或其他相关信息）
    public string gift;   // 礼物类型（指示送出的礼物种类）
    public int num;       // 礼物数量（指示送出的礼物数量）


}
public class WebSocketTest : MonoBehaviour
{

    WebSocket ws;
    int unitNumber;//单位编号
    int numberOfUnits;//单位数量
    //存储用户阵营
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
        Debug.Log("WebSocket连接已打开");
    }

    void OnMessageHandler(object sender, MessageEventArgs e)
    {
        //Debug.Log("收到消息：" + e.Data);

        //// 将JSON字符串反序列化为GiftEventData对象
        //GiftEventData giftMessage = JsonUtility.FromJson<GiftEventData>(e.Data);

        //// 根据消息类型进行处理
        //switch (giftMessage.type)
        //{
        //    case 1:
        //        //Debug.Log("来了：" + giftMessage.msg);
        //        break;
        //    case 2:
   
        //        Debug.Log(giftMessage.name + "发送了消息：" + giftMessage.msg);
        //        if (giftMessage.msg == "1" || giftMessage.msg == "2")
        //        {
        //            Debug.Log(giftMessage.name+"加入队伍");
        //            // 处理发言行为


        //            if (GameManagers.Instance.joinedPlayers.Contains(giftMessage.uid))
        //            {
        //                Debug.Log("玩家已加入过队伍");
        //                return;
        //            }
              
        //            GameManagers.Instance.joinedPlayers.Add(giftMessage.uid);
        //            // 将PlayerName和UID关联起来
        //            GameManagers.Instance.playerNameToUID[giftMessage.name] = giftMessage.uid;
        //            //将玩家的头像也存入字典，名字为钥匙，HeadPortrait的索引为值
        //            GameManagers.Instance.UserProfile[giftMessage.name] = giftMessage.url;
        //            if (giftMessage.msg == "1")
        //            {
        //                Debug.Log(giftMessage.name + "加入了魔神阵营");
        //                Debug.Log(giftMessage.name + "添加头像");
        //                StartCoroutine(LoadImageAndStoreBluePlayerAvatar(giftMessage.url, giftMessage.name));
        //                Debug.Log(giftMessage.name + "加入完毕");
        //                GameManagers.Instance.bluePlayers.Add(giftMessage.name);
        //                userFactions[giftMessage.uid] = "Blue"; // 存储用户阵营信息

        //                GameManagers.Instance.Instances(/*giftMessage.name + */"加入了魔神阵营成功");
        //                Debug.Log(giftMessage.name + "加入完毕2");

        //            }
        //            else if (giftMessage.msg == "2")
        //            {
        //                Debug.Log(giftMessage.name + "加入了人阵营");
        //                Debug.Log(giftMessage.name + "添加头像");
        //                StartCoroutine(LoadImageAndStoreRedPlayerAvatar(giftMessage.url, giftMessage.name));
        //                Debug.Log(giftMessage.name + "加入完毕");
        //                GameManagers.Instance.redPlayers.Add(giftMessage.name);
        //                userFactions[giftMessage.uid] = "Red"; // 存储用户阵营信息

        //                GameManagers.Instance.Instances(/*giftMessage.name + */"加入了人阵营成功");
        //                Debug.Log(giftMessage.name + "加入完毕2");



        //            }
            
        //        }
        //        break;
        //    case 3:
        //        Debug.Log("要加载的文本的名字" + GameObject.Find("GameManager").name);
        //        Debug.Log(giftMessage.name + "点赞：" + giftMessage.msg);
          
        //        if (GameManagers.Instance.joinedPlayers.Contains(giftMessage.uid))
        //        {
        //            // 处理点赞行为
        //            //Debug.Log("收到了点赞消息：" + giftMessage.msg);
        //            //GameManagers.Instance.SpawnUnit(0, 10,/* giftMessage.head_img,*/ giftMessage.name, faction);
        //            //GameManagers.Instance.StartCoroutine(GameManagers.Instance.SpawnUnit(0, 10, /*giftMessage.head_img,*/ giftMessage.name, userFactions[giftMessage.uid]));
        //            string userFaction = userFactions[giftMessage.uid];
        //            GameManagers.Instance.StartCoroutine(GameManagers.Instance.SpawnUnitAsync(UnityEngine.Random.Range(0, 2), 20, giftMessage.name, userFaction));
        //        }
        //        break;
        //    case 4:
        //        Debug.Log("送礼用户：" + giftMessage.name);
        //        Debug.Log("礼物类型：" + giftMessage.gift);
        //        Debug.Log("礼物数量：" + giftMessage.num);

        //        if (GameManagers.Instance.joinedPlayers.Contains(giftMessage.uid))
        //        {
        //            // 处理礼物行为
        //            //Debug.Log("收到了礼物消息：" + giftMessage.msg);
        //            // 在这里执行你想要的逻辑

        //            if (giftMessage.gift.Contains("小心心") || giftMessage.gift.Contains("小花花"))
        //            {
        //                unitNumber = 1;
        //                numberOfUnits = giftMessage.num * 20;
        //            }
        //            else if (giftMessage.gift.Contains("大啤酒") || giftMessage.gift.Contains("打call") || giftMessage.gift.Contains("打"))
        //            {
        //                unitNumber = 2;
        //                numberOfUnits = giftMessage.num * 5;
        //            }
        //            else if (giftMessage.gift.Contains("棒棒糖") || giftMessage.gift.Contains("星月盲盒"))
        //            {
        //                unitNumber = 3;
        //                numberOfUnits = giftMessage.num * 30;
        //            }
        //            else if (giftMessage.gift.Contains("爱你哟") || giftMessage.gift.Contains("干杯"))
        //            {
        //                unitNumber = 4;
        //                numberOfUnits = giftMessage.num * 20;
        //            }
        //            else if (giftMessage.gift.Contains("爱的纸鹤") || giftMessage.gift.Contains("心动盲盒"))
        //            {
        //                unitNumber = 5;
        //                numberOfUnits = giftMessage.num * 10;
        //            }
        //            else if (giftMessage.gift.Contains("礼花筒") || giftMessage.gift.Contains("星河入梦") || giftMessage.gift.Contains("仙女棒"))
        //            {
        //                unitNumber = 6;
        //                numberOfUnits = giftMessage.num * 5;
        //            }
        //            else if (giftMessage.gift.Contains ( "热气球") || giftMessage.gift.Contains("闪耀盲盒"))
        //            {
        //                unitNumber = 7;
        //                numberOfUnits = giftMessage.num * 3;
        //            }
        //            print("生成的单位编号为" + unitNumber);
        //            print("生成的单位数量为" + numberOfUnits);
        //            print("生成的单位图片为" + giftMessage.url);
        //            print("召唤者的名字为" + giftMessage.name);
        //            print("召唤者的阵营为" + userFactions[giftMessage.uid]);
        //            string userFaction = userFactions[giftMessage.uid];
        //            GameManagers.Instance.StartCoroutine(GameManagers.Instance.SpawnUnitAsync(unitNumber, numberOfUnits, /*giftMessage.head_img,*/ giftMessage.name, userFaction));

        //        }
        //        break;
        //    case 5:
        //        Debug.Log("关注主播：" + giftMessage.msg);
        //        break;
        //    default:
        //        Debug.Log("未知消息类型：" + giftMessage.type);
        //        break;
        //}
    }

    void OnCloseHandler(object sender, CloseEventArgs e)
    {
        Debug.Log("WebSocket连接已关闭");
    }

    void OnErrorHandler(object sender, ErrorEventArgs e)
    {
        //Debug.Log("WebSocket连接发生错误：" + e.Message);
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
                print("没有获取到玩家头像");
            }
            else
            {
                print("已获取到玩家头像");
                Texture2D texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                GameManagers.Instance.AddRedPlayerAvatar(playerName, sprite);//获取红方头像传入头像
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