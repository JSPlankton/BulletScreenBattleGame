using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class OtherScript : MonoBehaviour
{

    int unitNumber;//单位编号
    long numberOfUnits;//单位数量
    string faction;
    //UID
  public   Dictionary<string, long> playerNameToUID = new Dictionary<string, long>();


    //存储用户阵营
    Dictionary<long, string> userFactions = new Dictionary<long, string>();

    public static OtherScript instance;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        ConnectViaCode connectViaCode = FindObjectOfType<ConnectViaCode>();

        // 订阅事件，指定事件处理方法
        //购买物品
        //connectViaCode.OnSuperChatEvent += HandleSuperChatEvent;
        //大航海
        //connectViaCode.OnGuardBuyEvent += HandleGuardBuyEvent;
        //送礼
        connectViaCode.OnGiftEvent += HandleGiftEvent;


        // 发言
        connectViaCode.OnDanmakuEvent += WebSocketBLiveClientOnDanmaku;


        connectViaCode.OnHeartBeatSucceedEvent += HandleHeartBeatSucceedEvent;
        connectViaCode.OnHeartBeatErrorEvent += HandleHeartBeatErrorEvent;
    }
    private void WebSocketBLiveClientOnDanmaku(string userName, string message, long uid, string userFace)
    {
        Debug.Log($"收到弹幕！用户：{userName}，内容：{message}");
        if (message == "1" || message == "2")
        {
            if (GameManagers.Instance.joinedPlayers.Contains(uid.ToString()))
            {
                Debug.Log("玩家已加入过队伍");
                return;
            }
            GameManagers.Instance.joinedPlayers.Add(uid.ToString());
            // 将PlayerName和UID关联起来
            playerNameToUID[userName] = uid;
            //将玩家的头像也存入字典，名字为钥匙，HeadPortrait的索引为值
           GameManagers.Instance. UserProfile[userName] = userFace;




            if (message == "1")
            {
                GameManagers.Instance.bluePlayers.Add(userName);
                userFactions[uid] = "Blue"; // 存储用户阵营信息
                GameObject obj = Instantiate(GameManagers.Instance.PlayerJoinText, GameManagers.Instance.PlayerJoinTextMainObject.transform);
                obj.transform.SetParent(GameManagers.Instance.PlayerJoinTextMainObject.transform);
                obj.GetComponent<Text>().text = userName + "加入了魔神阵营";
                Destroy(obj, 2f);

                StartCoroutine(LoadImageAndStoreBluePlayerAvatar(userFace, userName));
                int ranking = UIManager.instance.GetPlayerRanking(userName);
      
                if (ranking <= 10)
                {

                    FadeInAndOpenObject.instace.StartFadeIn(0, userName);
                }
   
            }
            else if (message == "2")
            {
                GameManagers.Instance.redPlayers.Add(userName);
                userFactions[uid] = "Red"; // 存储用户阵营信息
                GameObject obj = Instantiate(GameManagers.Instance.PlayerJoinText, GameManagers.Instance.PlayerJoinTextMainObject.transform);
                obj.transform.SetParent(GameManagers.Instance.PlayerJoinTextMainObject.transform);
                obj.GetComponent<Text>().text = userName + "加入了天使阵营";
                Destroy(obj, 2f);
                StartCoroutine(LoadImageAndStoreRedPlayerAvatar(userFace, userName));
                int ranking = UIManager.instance.GetPlayerRanking(userName);
         
                if (ranking <= 10)
                {
                    FadeInAndOpenObject.instace.StartFadeIn(1, userName);
                }

            }


        }

        if (message == "666")
        {
            if (GameManagers.Instance.joinedPlayers.Contains(uid.ToString()))
            {
                print("生成的单位编号为" + unitNumber);
                print("生成的单位数量为" + numberOfUnits);
                print("召唤者的名字为" + userName);
                print("召唤者的ID为" + uid);
                print("召唤者的HeadPortrait为" + uid);
                print("召唤者的阵营为" + userFactions[uid]);
                if (userFactions.ContainsKey(uid))
                {
                    string userFaction = userFactions[uid];
                    // 在这里使用userFaction来设置召唤单位的阵营
                    // 其他逻辑...
                    GameManagers.Instance.StartCoroutine(GameManagers.Instance.SpawnUnitAsync(Random.Range(0, 2), 20, userName, userFaction));
                }
               
            }
  
        }






    }
    // 事件处理方法，输出 SuperChat 数据
    //private void HandleSuperChatEvent(string userName, string message, float amount)
    //{
    //    Debug.Log($"收到SC！用户：{userName}，留言内容：{message}，金额：{amount}元");
    //}

    // 事件处理方法，输出 GuardBuy 数据
    //private void HandleGuardBuyEvent(string userName, string guardUnit)
    //{
    //    Debug.Log($"收到大航海！用户：{userName}，赠送了{guardUnit}");
    //}

    // 事件处理方法，输出 Gift 数据
    private void HandleGiftEvent(string userName, long giftNum, string giftName, long uid)
    {
        Debug.Log($"收到礼物！用户：{userName}，赠送了{giftNum}个{giftName}");


        if (GameManagers.Instance.joinedPlayers.Contains(uid.ToString()))
        {
            // 处理礼物行为
            Debug.Log($"收到礼物！用户：{userName}，赠送了{giftNum}个{giftName}");
            // 在这里执行你想要的逻辑

            if (giftName.Contains("魔力之石"))
            {
                unitNumber = 2;
                numberOfUnits = giftNum * 5;
            }
            else if (giftName.Contains("能量之星") )
            {
                unitNumber = 3;
                numberOfUnits = giftNum * 20;
            }
            else if (giftName.Contains("圣者法杖") )
            {
                unitNumber = 4;
                numberOfUnits = giftNum * 10;
            }
            else if (giftName.Contains("骑士权杖"))
            {
                unitNumber = 5;
                numberOfUnits = giftNum * 5;
            }
            else if (giftName == "魔法宝典")
            {
                unitNumber = 6;
                numberOfUnits = giftNum * 3;
            }
            string userFaction = userFactions[uid];
            print("生成的单位编号为" + unitNumber);
            print("生成的单位数量为" + numberOfUnits);
            print("召唤者的名字为" + userName);
            print("召唤者的UID为" + uid);
            print("召唤者的阵营为" + userFaction);

            GameManagers.Instance.StartCoroutine(GameManagers.Instance.SpawnUnitAsync(unitNumber, (int)numberOfUnits, userName, userFaction));

        }



    }

    // 事件处理方法，输出 HeartBeat 成功
    private void HandleHeartBeatSucceedEvent()
    {
        Debug.Log("心跳成功");
    }

    // 事件处理方法，输出 HeartBeat 错误信息
    private void HandleHeartBeatErrorEvent(string errorJson)
    {
        Debug.Log($"心跳失败 {errorJson}");
    }



    private IEnumerator LoadImageAndStoreRedPlayerAvatar(string imageUrl, string playerName)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                print("没有获取到玩家HeadPortrait");
            }
            else
            {
                print("已获取到玩家HeadPortrait"+ imageUrl);
                Texture2D texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                GameManagers.Instance.AddRedPlayerAvatar(playerName, sprite);//获取红方HeadPortrait传入HeadPortrait
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



    public IEnumerator LoadImageAndStoreRedPlayerAvatar_Test(string imageUrl, string playerName)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                print("没有获取到玩家HeadPortrait");
            }
            else
            {
                print("已获取到玩家HeadPortrait" + imageUrl);
                Texture2D texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
                GameManagers.Instance.AddRedPlayerAvatar(playerName, sprite);//获取红方HeadPortrait传入HeadPortrait
            }
        }
    }
    public IEnumerator LoadImageAndStoreBluePlayerAvatar_Test(string imageUrl, string playerName)
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
