using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UserActionsManager : MonoBehaviour
{

    int unitNumber;//单位编号
    int numberOfUnits;//单位数量
    //存储用户阵营
    Dictionary<string, string> userFactions = new Dictionary<string, string>();
    public static UserActionsManager instance;
    private void Awake()
    {
        instance = this;
    }
    // 其他代码...

    private void Start()
    {
        // 在启动时订阅 WebSocketTest 的事件
        // 在启动时订阅 WebSocketTest 的事件
        WebSocketTest webSocketTest = FindObjectOfType<WebSocketTest>();
        if (webSocketTest != null)
        {
            //webSocketTest.OnMessageReceived += HandleWebSocketMessage;
        }
    }

    // 处理WebSocketTest的消息
    private void HandleWebSocketMessage(string jsonMessage)
    {
        // 将 JSON 字符串反序列化为 GiftEventData 对象
        GiftEventData giftMessage = JsonUtility.FromJson<GiftEventData>(jsonMessage);

        switch (giftMessage.type)
        {

            case 2:
                Debug.Log(giftMessage.name + " 发言: " + giftMessage.msg);
                HandleUserMessage(giftMessage.name, giftMessage.msg, giftMessage.uid, giftMessage.url);
                break;
            case 3:
                Debug.Log(giftMessage.name + " 点赞" );
                Like(giftMessage.name, giftMessage.uid);
                break;
            case 4:
                Debug.Log(giftMessage.name + " 送了 " + giftMessage.gift + " 个 " + giftMessage.num);
                HandleUserGift(giftMessage.name, giftMessage.gift, giftMessage.num, giftMessage.uid);
                break;
            default:
                // 处理其他消息类型
                break;
        }
    }

    // 用户发言行为
    public void HandleUserMessage(string userName, string message, string uid, string url)
    {
        // 在这里执行处理用户发言的逻辑
        Debug.Log(userName + " 发言: " + message);
        if (message == "1" || message == "2"||message == "8" || message == "9" || message == "蓝" || message == "橙")
        {
            Debug.Log(userName + "加入队伍");
            // 处理发言行为


            if (GameManagers.Instance.joinedPlayers.Contains(uid))
            {
                Debug.Log("玩家已加入过队伍");
                return;
            }

            GameManagers.Instance.joinedPlayers.Add(uid);
            // 将PlayerName和UID关联起来
            GameManagers.Instance.playerNameToUID[userName] = uid;
            //将玩家的头像也存入字典，名字为钥匙，HeadPortrait的索引为值
            GameManagers.Instance.UserProfile[userName] = url;
            if (message == "1"|| message == "8" || message == "蓝")
            {
                Debug.Log(userName + "加入了魔神阵营");
                GameManagers.Instance.bluePlayers.Add(userName);
                userFactions[uid] = "Blue"; // 存储用户阵营信息

                Debug.Log(GameManagers.Instance.PlayerJoinText.name);
                GameObject obj = Instantiate(GameManagers.Instance.PlayerJoinText, GameManagers.Instance.PlayerJoinTextMainObject.transform);
                Debug.Log(userName + "生成成功");
                obj.transform.SetParent(GameManagers.Instance.PlayerJoinTextMainObject.transform);
                obj.GetComponent<Text>().text = userName + "加入了魔神阵营";
                Destroy(obj, 2f);
                Debug.Log(userName + "添加头像");
                StartCoroutine(LoadImageAndStoreBluePlayerAvatar(url, userName));
                Debug.Log(userName + "加入完毕");
            }
            else if (message == "2" || message == "9" || message == "橙")
            {
                Debug.Log(userName + "加入了人阵营");
                GameManagers.Instance.redPlayers.Add(userName);
                userFactions[uid] = "Red"; // 存储用户阵营信息
   

                GameObject obj = Instantiate(GameManagers.Instance.PlayerJoinText, GameManagers.Instance.PlayerJoinTextMainObject.transform);
                Debug.Log(userName + "生成成功");
                obj.transform.SetParent(GameManagers.Instance.PlayerJoinTextMainObject.transform);
                obj.GetComponent<Text>().text = userName + "加入了人阵营成功";
                Destroy(obj, 2f);
                Debug.Log(userName + "添加头像");
                StartCoroutine(LoadImageAndStoreRedPlayerAvatar(url, userName));
                Debug.Log(userName + "加入完毕");
            }

        }
    }

    // 用户送礼行为
    public void HandleUserGift(string userName, string giftType, int giftCount, string uid)
    {
        // 在这里执行处理用户送礼的逻辑
        Debug.Log(userName + " 送了 " + giftCount + " 个 " + giftType);
        if (GameManagers.Instance.joinedPlayers.Contains(uid))
        {
            // 处理礼物行为
            //Debug.Log("收到了礼物消息：" + giftMessage.msg);
            // 在这里执行你想要的逻辑

            if (giftType.Contains("小心心") || giftType.Contains("小花花"))
            {
                unitNumber = 1;
                numberOfUnits = giftCount * 20;
            }
            else if (giftType.Contains("大啤酒") || giftType.Contains("打call") || giftType.Contains("打"))
            {
                unitNumber = 2;
                numberOfUnits = giftCount * 5;
            }
            else if (giftType.Contains("棒棒糖") || giftType.Contains("星月盲盒"))
            {
                unitNumber = 3;
                numberOfUnits = giftCount * 30;
            }
            else if (giftType.Contains("爱你哟") || giftType.Contains("干杯"))
            {
                unitNumber = 4;
                numberOfUnits = giftCount * 20;
            }
            else if (giftType.Contains("爱的纸鹤") || giftType.Contains("心动盲盒"))
            {
                unitNumber = 5;
                numberOfUnits = giftCount * 10;
            }
            else if (giftType.Contains("礼花筒") || giftType.Contains("星河入梦") || giftType.Contains("仙女棒"))
            {
                unitNumber = 6;
                numberOfUnits = giftCount * 5;
            }
            else if (giftType.Contains("热气球") || giftType.Contains("闪耀盲盒"))
            {
                unitNumber = 7;
                numberOfUnits = giftCount * 3;
            }
            print("生成的单位编号为" + unitNumber);
            print("生成的单位数量为" + numberOfUnits);
            //print("生成的单位图片为" + url);
            print("召唤者的名字为" + userName);
            print("召唤者的阵营为" + userFactions[uid]);
            string userFaction = userFactions[uid];
            GameManagers.Instance.StartCoroutine(GameManagers.Instance.SpawnUnitAsync(unitNumber, numberOfUnits, /*giftMessage.head_img,*/ userName, userFaction));

        }
    }

    public void Like(string userName, string uid)
    {
        Debug.Log(userName + "点赞：");
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
