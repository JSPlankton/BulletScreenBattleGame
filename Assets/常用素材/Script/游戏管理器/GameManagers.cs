//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManagers : MonoBehaviour
{
    // GameManager的单例实例
    public static GameManagers Instance;

    // 红色和蓝色阵营的单位预制体列表
    public List<GameObject> redUnitPrefabs;
    public List<GameObject> blueUnitPrefabs;


    // 存储红蓝双方的单位列表
    //public List<GameObject> redUnits = new List<GameObject>();
    //public List<GameObject> blueUnits = new List<GameObject>();

    // 存储红蓝双方的单位出场动画列表
    public List<GameObject> redUnitsAnimaton = new List<GameObject>();
    public List<GameObject> blueUnitsAnimaton = new List<GameObject>();

    public GameObject crystalRed;//红水晶
    public GameObject crystalBlue;//蓝水晶

    public GameObject[] redGenerativePoint;//红生成点
    public GameObject[] blueGenerativePoint;//蓝生成点


    //红击败数量展示
    public GameObject RedDefeatQuantityDisplay;

    //蓝击败数量展示
    public GameObject BlueDefeatQuantityDisplay;

    private Sprite unitImage;//用户HeadPortrait

    // 存储每个用户的击杀记录
    public Dictionary<string, int> userKillRecords = new Dictionary<string, int>();
    private Dictionary<string, int> userLastDisplayKill = new Dictionary<string, int>(); // 记录上次显示时的击杀数


    public List<string> redPlayers = new List<string>(); // 红队玩家列表
    public List<string> bluePlayers = new List<string>(); // 蓝队玩家列表
    public HashSet<string> joinedPlayers = new HashSet<string>(); // 已加入队伍的玩家集合

    //Scores
   public Dictionary<string, int> playerScores = new Dictionary<string, int>();



    //用户HeadPortrait
    public Dictionary<string, string> UserProfile = new Dictionary<string, string>();

    //玩家加入的信息
    public GameObject PlayerJoinText;
    //玩家加入信息的主物体
    public GameObject PlayerJoinTextMainObject;

    //UID
    public Dictionary<string, string> playerNameToUID = new Dictionary<string, string>();

    //是否自动出兵
    private bool isUnit = false;


    //存储名字和HeadPortrait
    public Dictionary<string, Sprite> redPlayerAvatars = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> bluePlayerAvatars = new Dictionary<string, Sprite>();

    //存储用户阵营
   public Dictionary<string, string> userFactions = new Dictionary<string, string>();

    void Awake()
    {
        // 确保GameManager只有一个实例
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {

        //GameManagers.Instance.redPlayers.Add("时光可待她安好");

        //GameManagers.Instance.bluePlayers.Add("修道荒千古");

        //StartCoroutine(OtherScript.instance. LoadImageAndStoreRedPlayerAvatar_Test(defaultAvatar1, "夕下"));
        //StartCoroutine(OtherScript.instance.LoadImageAndStoreBluePlayerAvatar_Test(defaultAvatar2, "秋风落叶"));

    }
    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.G))
        {
            //SpawnAllUnits(Random.Range(1, redUnitPrefabs.Count), Random.Range(5, 15), null, "夕下红2", "Red");
            StartCoroutine(SpawnUnitAsync(Random.Range(1, redUnitPrefabs.Count), Random.Range(5, 10), "时光可待她安好", "Red"));
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            //SpawnAllUnits(Random.Range(1, blueUnitPrefabs.Count), Random.Range(5, 15), null, "夕下蓝2", "Blue");
            StartCoroutine(SpawnUnitAsync(Random.Range(1, blueUnitPrefabs.Count), Random.Range(5, 10), "修道荒千古", "Blue"));
        }


        //maxUnitText.text = redUnits.Count + blueUnits.Count + "";
        if (Input.GetKeyDown(KeyCode.P))
        {
            isUnit = !isUnit; // 切换 isUnit 的状态，如果为 true，则设为 false；如果为 false，则设为 true。
            StartCoroutine(SpawnUnits(50));
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            isUnit = !isUnit; // 切换 isUnit 的状态，如果为 true，则设为 false；如果为 false，则设为 true。
            StartCoroutine(SpawnUnits(5));
        }
        */
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            //SpawnAllUnits(Random.Range(1, redUnitPrefabs.Count), Random.Range(5, 15), null, "夕下红2", "Red");
            StartCoroutine(SpawnUnitAsync(0, 1, "时光可待她安好", "Red"));
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            //SpawnAllUnits(Random.Range(1, blueUnitPrefabs.Count), Random.Range(5, 15), null, "夕下蓝2", "Blue");
            StartCoroutine(SpawnUnitAsync(0, 1, "修道荒千古", "Blue"));
        }
    }

    private IEnumerator SpawnUnits(int index)
    {
        while (isUnit)
        {
            SpawnAllUnits(0, index, "", "Red");
            SpawnAllUnits(0, index, "", "Blue");
            yield return new WaitForSeconds(1); // 等待 2 秒
            //yield return null; // Yield once to ensure the coroutine runs on the next frame

        }
        yield return null;
    }
  
    public IEnumerator SpawnUnitAsync(int unitNumber, int numberOfUnits, string unitName, string faction)
    {
        yield return null; // Yield once to ensure the coroutine runs on the next frame

        yield return SpawnUnit(unitNumber, numberOfUnits, unitName, faction);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="unitNumber">单位编号</param>
    /// <param name="numberOfUnits">生成数量</param>
    /// <param name="unitName">名字</param>
    /// <param name="faction">阵营</param>

    public IEnumerator SpawnUnit(int unitNumber, int numberOfUnits, string unitName, string faction)
    {
        List<GameObject> targetPrefabList;
        GameObject points; // 生成点

        if (faction == "Red")
        {
            targetPrefabList = redUnitPrefabs;
            // points = redGenerativePoint[Random.Range(0, redGenerativePoint.Length)];
            points = redGenerativePoint[0];
            if (unitNumber != 0)
            {
                // 创建动画对象
                GameObject unitAnimaton = Instantiate(redUnitsAnimaton[0], redUnitsAnimaton[0].transform.position, redUnitsAnimaton[0].transform.rotation);
                unitAnimaton.transform.SetParent(Camera.main.transform);
                unitAnimaton.SetActive(true);
                unitAnimaton.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = unitName;
            }
        }
        else if (faction == "Blue")
        {
            targetPrefabList = blueUnitPrefabs;
            // points = blueGenerativePoint[Random.Range(0, blueGenerativePoint.Length)];
            points = blueGenerativePoint[0];
            if (unitNumber != 0)
            {
                // 创建动画对象
                GameObject unitAnimaton = Instantiate(blueUnitsAnimaton[0], blueUnitsAnimaton[0].transform.position, blueUnitsAnimaton[0].transform.rotation);
                unitAnimaton.transform.SetParent(Camera.main.transform);
                unitAnimaton.SetActive(true);
                unitAnimaton.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = unitName;
            }
        }
        else
        {
            Debug.LogError("Invalid faction name!");
            yield break;
        }

        // 获取要生成的单位预制体
        GameObject unitPrefab = targetPrefabList[unitNumber];

        float unitSpacing = 5.0f; // 单位之间的横向间距
        float rowSpacing = 3.0f; // 行之间的纵向间距

        int middleUnitIndex = numberOfUnits / 2; // 找到本次生成的单位中的最中间的一个

        int unitsInCurrentRow = -1; // 每一行开始生成0个单位
        int currentRow = 0;
        Vector3[] spawnPositions = new Vector3[numberOfUnits];

        for (int i = 0; i < numberOfUnits; i++)
        {
            if (i >= unitsInCurrentRow)
            {
                unitsInCurrentRow += 2; // 每一排增加两个单位
                currentRow++;

                // 使用协程生成一行单位
                yield return StartCoroutine(SpawnRow(unitPrefab, points, unitName, faction, currentRow, unitsInCurrentRow, middleUnitIndex, unitSpacing, rowSpacing));
            }
        }
    }

    private IEnumerator SpawnRow(GameObject unitPrefab, GameObject points, string unitName, string faction, int currentRow, int unitsInCurrentRow, int middleUnitIndex, float unitSpacing, float rowSpacing)
    {

        Vector3 spawnPosition = Vector3.zero; // 初始生成位置

        for (int j = 0; j < unitsInCurrentRow; j++)
        {

            int col = j - unitsInCurrentRow / 2;

            if (faction == "Red")
            {
                // 如果是红方，反着排列
                spawnPosition = points.transform.position - currentRow * rowSpacing * points.transform.forward - col * unitSpacing * points.transform.right;
            }
            else
            {
                // 如果是蓝方，正常排列
                spawnPosition = points.transform.position - currentRow * rowSpacing * points.transform.forward + col * unitSpacing * points.transform.right;
            }

            GameObject unit = Instantiate(unitPrefab, spawnPosition, points.transform.rotation);

            Unit unitScript = unit.GetComponent<Unit>();

            if (unitName != "")
            {
                if (!userKillRecords.ContainsKey(unitName))
                {
                    userKillRecords[unitName] = 0;
                }
                if (!playerScores.ContainsKey(unitName))
                {
                    playerScores[unitName] = 0;
                }
                unitScript.SetSummoner(unitName);

                if (j == middleUnitIndex)
                {
                    unitScript.SetUnitName(unitName); // Set name for the middle unit
                }
            }

            yield return null; // 等待一帧后继续生成下一个单位
        }
    }

    private void SpawnAllUnits(int unitNumber, int numberOfUnits, /*string imageUrl,*/ string unitName, string faction)
    {
        //StartCoroutine(LoadImage(imageUrl));
        //List<GameObject> targetList;
        List<GameObject> targetPrefabList;
        GameObject points;//生成点

        // 根据阵营字符串选择要生成单位的列表和单位预制体的列表
        if (faction == "Red")
        {
            //targetList = redUnits;
            targetPrefabList = redUnitPrefabs;
            points = redGenerativePoint[Random.Range(0, redGenerativePoint.Length)];
            if (unitNumber != 0)
            {
                GameObject unitAnimaton = Instantiate(redUnitsAnimaton[unitNumber - 1], redUnitsAnimaton[unitNumber - 1].transform.position, redUnitsAnimaton[unitNumber - 1].transform.rotation);
                unitAnimaton.transform.SetParent(Camera.main.transform);
                unitAnimaton.SetActive(true);
                unitAnimaton.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = unitName;
            }

        }
        else if (faction == "Blue")
        {
            //targetList = blueUnits;
            targetPrefabList = blueUnitPrefabs;
            points = blueGenerativePoint[Random.Range(0, blueGenerativePoint.Length)];
            if (unitNumber != 0)
            {
                GameObject unitAnimaton = Instantiate(blueUnitsAnimaton[unitNumber - 1], blueUnitsAnimaton[unitNumber - 1].transform.position, blueUnitsAnimaton[unitNumber - 1].transform.rotation);
                unitAnimaton.transform.SetParent(Camera.main.transform);
                unitAnimaton.SetActive(true);
                unitAnimaton.transform.GetChild(0).transform.GetChild(1).GetComponent<Text>().text = unitName;
            }

        }
        else
        {
            Debug.LogError("Invalid faction name!");
            return;
        }

        // 获取要生成的单位预制体
        GameObject unitPrefab = targetPrefabList[unitNumber];

        Vector3 spawnPosition;
        //GameObject unit;

        int maxUnitsPerRow = 10; // 每行最多生成的单位数

        float unitSpacing = 5.0f; // 单位之间的横向间距
        float rowSpacing = 3.0f; // 行之间的纵向间距

        int middleUnitIndex = numberOfUnits / 2; // 找到本次生成的单位中的最中间的一个

        for (int i = 0; i < numberOfUnits; i++)
        {
            int row = i / maxUnitsPerRow;
            int col = i % maxUnitsPerRow;


            if (faction == "Red")
            {
                // 如果是红方，反着排列
                spawnPosition = points.transform.position - row * rowSpacing * points.transform.forward - col * unitSpacing * points.transform.right;
            }
            else
            {
                // 如果是蓝方，正常排列
                spawnPosition = points.transform.position -row * rowSpacing * points.transform.forward + col * unitSpacing * points.transform.right;
            }
            //spawnPosition = points.transform.position - row * rowSpacing * points.transform.forward - col * unitSpacing * points.transform.right;

            GameObject unit = ObjectPoolManager.instance.GetUnitFromPool(unitPrefab, spawnPosition, points.transform.rotation);
            //targetList.Add(unit);

            Unit unitScript = unit.GetComponent<Unit>();
            //unitScript.InitializeUnit(unitPrefab);

            if (unitName != "")
            {
                if (!userKillRecords.ContainsKey(unitName))
                {
                    userKillRecords[unitName] = 0;
                }

                unitScript.SetSummoner(unitName);

                if (i == middleUnitIndex)
                {
                    unitScript.SetUnitName(unitName); // Set name for the middle unit
                }
            }
        }

    }


    // 添加一个方法用于获取玩家的击杀数
    public int GetUserKillCount(string userName)
    {
        if (userKillRecords.ContainsKey(userName))
        {
            return userKillRecords[userName];
        }
        return 0;
    }

    // 处理击杀事件,传入击杀者的名字
    public void HandleKillEvent(string attackerName, string factions,int score)
    {
        //检查attackerName是否为空
        if (string.IsNullOrEmpty(attackerName))
        {
            //判断如果是系统单位，则不再往下执行
            return;
        }

        if (userKillRecords.ContainsKey(attackerName))
        {
          

            userKillRecords[attackerName]++;
            playerScores[attackerName]+= score;
            int requiredKills;
            // 在这里你可以检查是否满足显示条件


                requiredKills = userLastDisplayKill.ContainsKey(attackerName) ? userLastDisplayKill[attackerName] + 1000 : 888;


            int requiredKills2;
            // 在这里你可以检查是否满足显示条件
            requiredKills2 = userLastDisplayKill.ContainsKey(attackerName) ? userLastDisplayKill[attackerName] + 100 : 100;
            if (userKillRecords[attackerName] >= requiredKills2)
            {
                UIManager.instance.UpdateKillRankingDisplay(redPlayers, bluePlayers); // 传递红队和蓝队的玩家列表
            }


                int killCount = userKillRecords[attackerName];

            if (userKillRecords[attackerName] >= requiredKills)
            {
                //Debug.Log(summonerName + " 的击杀数达到 " + killCount + " 次！");
                userLastDisplayKill[attackerName] = userKillRecords[attackerName]; // 更新上次显示时的击杀数

                GameObject defeatQuantityDisplay;
                if (factions == "Red")
                {
                    defeatQuantityDisplay = RedDefeatQuantityDisplay;
                    defeatQuantityDisplay.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().sprite = GetRedPlayerAvatar(attackerName);//获取HeadPortrait显示给击杀弹窗
                    defeatQuantityDisplay.transform.GetChild(2).GetComponent<Text>().text = attackerName + "总击败：" + killCount;
                }
                else if (factions == "Blue")
                {
                    defeatQuantityDisplay = BlueDefeatQuantityDisplay;
                    defeatQuantityDisplay.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().sprite = GetBluePlayerAvatar(attackerName);//获取HeadPortrait显示给击杀弹窗
                    defeatQuantityDisplay.transform.GetChild(2).GetComponent<Text>().text = attackerName + "总击败：" + killCount;
                }
                else
                {
                    return; // 防止无效阵营 
                }

                UIManager.instance.UpdateKillRankingDisplay(redPlayers, bluePlayers); // 传递红队和蓝队的玩家列表
                StartCoroutine(OpenDefeatQuantityDisplay(defeatQuantityDisplay));

            }
        }
    }


    private IEnumerator OpenDefeatQuantityDisplay(GameObject display)
    {
        if (!display.activeSelf)
        {
            display.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            display.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(3.0f);
            display.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            display.SetActive(false);
        }
    }

    // 添加方法来存储红队玩家HeadPortrait
    public void AddRedPlayerAvatar(string playerName, Sprite avatar)
    {
        redPlayerAvatars[playerName] = avatar;
    }

    // 添加方法来存储蓝队玩家HeadPortrait
    public void AddBluePlayerAvatar(string playerName, Sprite avatar)
    {
        bluePlayerAvatars[playerName] = avatar;
    }

    // 添加方法来获取红队玩家HeadPortrait
    public Sprite GetRedPlayerAvatar(string playerName)
    {
        if (redPlayerAvatars.ContainsKey(playerName))
        {
            return redPlayerAvatars[playerName];
        }
        return null; // 返回默认HeadPortrait或其他处理
    }

    // 添加方法来获取蓝队玩家HeadPortrait
    public Sprite GetBluePlayerAvatar(string playerName)
    {
        if (bluePlayerAvatars.ContainsKey(playerName))
        {
            return bluePlayerAvatars[playerName];
        }
        return null; // 返回默认HeadPortrait或其他处理
    }


    //初始化玩家的入场信息
    public void Instances(string uid, string name, string text, string factions, string img)
    {

        if (factions == "Blue")
        {
            bluePlayers.Add(name);

        }
        else
        {
            redPlayers.Add(name);
        }
        userFactions[uid] = factions; // 存储用户阵营信息

        //生成出场文字
        GameObject obj = Instantiate(PlayerJoinText, PlayerJoinTextMainObject.transform);
        obj.transform.SetParent(PlayerJoinTextMainObject.transform);
        obj.GetComponent<Text>().text = name + text;


        Destroy(obj, 2f);


        //加载头像
        StartCoroutine(LoadImageAndStorePlayerAvatar(img, name, factions));
    }


    //初始化玩家的入场信息
    public void PlayerAddDictionary(string playerName, string playerUID, string playerImg)
    {
        //将UID添加列表表示玩家已在游戏中
        joinedPlayers.Add(playerUID);

        //将名字设置为uid
        playerNameToUID[playerName] = playerUID;


        //将玩家的头像也存入字典，名字为钥匙，HeadPortrait的索引为值
        UserProfile[playerName] = playerImg;
    }

    private IEnumerator LoadImageAndStorePlayerAvatar(string imageUrl, string playerName, string factions)
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
                if (factions == "Red")
                {
                    GameManagers.Instance.AddRedPlayerAvatar(playerName, sprite);//获取红方头像传入头像
                }
                  else
                {
                    GameManagers.Instance.AddBluePlayerAvatar(playerName, sprite);//获取红方头像传入头像
                }
            }
        }
    }
}
