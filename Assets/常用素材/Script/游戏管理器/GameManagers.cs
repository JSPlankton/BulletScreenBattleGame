//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class GameManagers : MonoBehaviour
{
    // GameManager�ĵ���ʵ��
    public static GameManagers Instance;

    // ��ɫ����ɫ��Ӫ�ĵ�λԤ�����б�
    public List<GameObject> redUnitPrefabs;
    public List<GameObject> blueUnitPrefabs;


    // �洢����˫���ĵ�λ�б�
    //public List<GameObject> redUnits = new List<GameObject>();
    //public List<GameObject> blueUnits = new List<GameObject>();

    // �洢����˫���ĵ�λ���������б�
    public List<GameObject> redUnitsAnimaton = new List<GameObject>();
    public List<GameObject> blueUnitsAnimaton = new List<GameObject>();

    public GameObject crystalRed;//��ˮ��
    public GameObject crystalBlue;//��ˮ��

    public GameObject[] redGenerativePoint;//�����ɵ�
    public GameObject[] blueGenerativePoint;//�����ɵ�


    //���������չʾ
    public GameObject RedDefeatQuantityDisplay;

    //����������չʾ
    public GameObject BlueDefeatQuantityDisplay;

    private Sprite unitImage;//�û�HeadPortrait

    // �洢ÿ���û��Ļ�ɱ��¼
    public Dictionary<string, int> userKillRecords = new Dictionary<string, int>();
    private Dictionary<string, int> userLastDisplayKill = new Dictionary<string, int>(); // ��¼�ϴ���ʾʱ�Ļ�ɱ��


    public List<string> redPlayers = new List<string>(); // �������б�
    public List<string> bluePlayers = new List<string>(); // ��������б�
    public HashSet<string> joinedPlayers = new HashSet<string>(); // �Ѽ���������Ҽ���

    //Scores
   public Dictionary<string, int> playerScores = new Dictionary<string, int>();



    //�û�HeadPortrait
    public Dictionary<string, string> UserProfile = new Dictionary<string, string>();

    //��Ҽ������Ϣ
    public GameObject PlayerJoinText;
    //��Ҽ�����Ϣ��������
    public GameObject PlayerJoinTextMainObject;

    //UID
    public Dictionary<string, string> playerNameToUID = new Dictionary<string, string>();

    //�Ƿ��Զ�����
    private bool isUnit = false;


    //�洢���ֺ�HeadPortrait
    public Dictionary<string, Sprite> redPlayerAvatars = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> bluePlayerAvatars = new Dictionary<string, Sprite>();

    //�洢�û���Ӫ
   public Dictionary<string, string> userFactions = new Dictionary<string, string>();

    void Awake()
    {
        // ȷ��GameManagerֻ��һ��ʵ��
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

        //GameManagers.Instance.redPlayers.Add("ʱ��ɴ�������");

        //GameManagers.Instance.bluePlayers.Add("�޵���ǧ��");

        //StartCoroutine(OtherScript.instance. LoadImageAndStoreRedPlayerAvatar_Test(defaultAvatar1, "Ϧ��"));
        //StartCoroutine(OtherScript.instance.LoadImageAndStoreBluePlayerAvatar_Test(defaultAvatar2, "�����Ҷ"));

    }
    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.G))
        {
            //SpawnAllUnits(Random.Range(1, redUnitPrefabs.Count), Random.Range(5, 15), null, "Ϧ�º�2", "Red");
            StartCoroutine(SpawnUnitAsync(Random.Range(1, redUnitPrefabs.Count), Random.Range(5, 10), "ʱ��ɴ�������", "Red"));
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            //SpawnAllUnits(Random.Range(1, blueUnitPrefabs.Count), Random.Range(5, 15), null, "Ϧ����2", "Blue");
            StartCoroutine(SpawnUnitAsync(Random.Range(1, blueUnitPrefabs.Count), Random.Range(5, 10), "�޵���ǧ��", "Blue"));
        }


        //maxUnitText.text = redUnits.Count + blueUnits.Count + "";
        if (Input.GetKeyDown(KeyCode.P))
        {
            isUnit = !isUnit; // �л� isUnit ��״̬�����Ϊ true������Ϊ false�����Ϊ false������Ϊ true��
            StartCoroutine(SpawnUnits(50));
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            isUnit = !isUnit; // �л� isUnit ��״̬�����Ϊ true������Ϊ false�����Ϊ false������Ϊ true��
            StartCoroutine(SpawnUnits(5));
        }
        */
        
        if (Input.GetKeyDown(KeyCode.G))
        {
            //SpawnAllUnits(Random.Range(1, redUnitPrefabs.Count), Random.Range(5, 15), null, "Ϧ�º�2", "Red");
            StartCoroutine(SpawnUnitAsync(0, 1, "ʱ��ɴ�������", "Red"));
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            //SpawnAllUnits(Random.Range(1, blueUnitPrefabs.Count), Random.Range(5, 15), null, "Ϧ����2", "Blue");
            StartCoroutine(SpawnUnitAsync(0, 1, "�޵���ǧ��", "Blue"));
        }
    }

    private IEnumerator SpawnUnits(int index)
    {
        while (isUnit)
        {
            SpawnAllUnits(0, index, "", "Red");
            SpawnAllUnits(0, index, "", "Blue");
            yield return new WaitForSeconds(1); // �ȴ� 2 ��
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
    /// <param name="unitNumber">��λ���</param>
    /// <param name="numberOfUnits">��������</param>
    /// <param name="unitName">����</param>
    /// <param name="faction">��Ӫ</param>

    public IEnumerator SpawnUnit(int unitNumber, int numberOfUnits, string unitName, string faction)
    {
        List<GameObject> targetPrefabList;
        GameObject points; // ���ɵ�

        if (faction == "Red")
        {
            targetPrefabList = redUnitPrefabs;
            // points = redGenerativePoint[Random.Range(0, redGenerativePoint.Length)];
            points = redGenerativePoint[0];
            if (unitNumber != 0)
            {
                // ������������
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
                // ������������
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

        // ��ȡҪ���ɵĵ�λԤ����
        GameObject unitPrefab = targetPrefabList[unitNumber];

        float unitSpacing = 5.0f; // ��λ֮��ĺ�����
        float rowSpacing = 3.0f; // ��֮���������

        int middleUnitIndex = numberOfUnits / 2; // �ҵ��������ɵĵ�λ�е����м��һ��

        int unitsInCurrentRow = -1; // ÿһ�п�ʼ����0����λ
        int currentRow = 0;
        Vector3[] spawnPositions = new Vector3[numberOfUnits];

        for (int i = 0; i < numberOfUnits; i++)
        {
            if (i >= unitsInCurrentRow)
            {
                unitsInCurrentRow += 2; // ÿһ������������λ
                currentRow++;

                // ʹ��Э������һ�е�λ
                yield return StartCoroutine(SpawnRow(unitPrefab, points, unitName, faction, currentRow, unitsInCurrentRow, middleUnitIndex, unitSpacing, rowSpacing));
            }
        }
    }

    private IEnumerator SpawnRow(GameObject unitPrefab, GameObject points, string unitName, string faction, int currentRow, int unitsInCurrentRow, int middleUnitIndex, float unitSpacing, float rowSpacing)
    {

        Vector3 spawnPosition = Vector3.zero; // ��ʼ����λ��

        for (int j = 0; j < unitsInCurrentRow; j++)
        {

            int col = j - unitsInCurrentRow / 2;

            if (faction == "Red")
            {
                // ����Ǻ췽����������
                spawnPosition = points.transform.position - currentRow * rowSpacing * points.transform.forward - col * unitSpacing * points.transform.right;
            }
            else
            {
                // �������������������
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

            yield return null; // �ȴ�һ֡�����������һ����λ
        }
    }

    private void SpawnAllUnits(int unitNumber, int numberOfUnits, /*string imageUrl,*/ string unitName, string faction)
    {
        //StartCoroutine(LoadImage(imageUrl));
        //List<GameObject> targetList;
        List<GameObject> targetPrefabList;
        GameObject points;//���ɵ�

        // ������Ӫ�ַ���ѡ��Ҫ���ɵ�λ���б�͵�λԤ������б�
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

        // ��ȡҪ���ɵĵ�λԤ����
        GameObject unitPrefab = targetPrefabList[unitNumber];

        Vector3 spawnPosition;
        //GameObject unit;

        int maxUnitsPerRow = 10; // ÿ��������ɵĵ�λ��

        float unitSpacing = 5.0f; // ��λ֮��ĺ�����
        float rowSpacing = 3.0f; // ��֮���������

        int middleUnitIndex = numberOfUnits / 2; // �ҵ��������ɵĵ�λ�е����м��һ��

        for (int i = 0; i < numberOfUnits; i++)
        {
            int row = i / maxUnitsPerRow;
            int col = i % maxUnitsPerRow;


            if (faction == "Red")
            {
                // ����Ǻ췽����������
                spawnPosition = points.transform.position - row * rowSpacing * points.transform.forward - col * unitSpacing * points.transform.right;
            }
            else
            {
                // �������������������
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


    // ���һ���������ڻ�ȡ��ҵĻ�ɱ��
    public int GetUserKillCount(string userName)
    {
        if (userKillRecords.ContainsKey(userName))
        {
            return userKillRecords[userName];
        }
        return 0;
    }

    // �����ɱ�¼�,�����ɱ�ߵ�����
    public void HandleKillEvent(string attackerName, string factions,int score)
    {
        //���attackerName�Ƿ�Ϊ��
        if (string.IsNullOrEmpty(attackerName))
        {
            //�ж������ϵͳ��λ����������ִ��
            return;
        }

        if (userKillRecords.ContainsKey(attackerName))
        {
          

            userKillRecords[attackerName]++;
            playerScores[attackerName]+= score;
            int requiredKills;
            // ����������Լ���Ƿ�������ʾ����


                requiredKills = userLastDisplayKill.ContainsKey(attackerName) ? userLastDisplayKill[attackerName] + 1000 : 888;


            int requiredKills2;
            // ����������Լ���Ƿ�������ʾ����
            requiredKills2 = userLastDisplayKill.ContainsKey(attackerName) ? userLastDisplayKill[attackerName] + 100 : 100;
            if (userKillRecords[attackerName] >= requiredKills2)
            {
                UIManager.instance.UpdateKillRankingDisplay(redPlayers, bluePlayers); // ���ݺ�Ӻ����ӵ�����б�
            }


                int killCount = userKillRecords[attackerName];

            if (userKillRecords[attackerName] >= requiredKills)
            {
                //Debug.Log(summonerName + " �Ļ�ɱ���ﵽ " + killCount + " �Σ�");
                userLastDisplayKill[attackerName] = userKillRecords[attackerName]; // �����ϴ���ʾʱ�Ļ�ɱ��

                GameObject defeatQuantityDisplay;
                if (factions == "Red")
                {
                    defeatQuantityDisplay = RedDefeatQuantityDisplay;
                    defeatQuantityDisplay.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().sprite = GetRedPlayerAvatar(attackerName);//��ȡHeadPortrait��ʾ����ɱ����
                    defeatQuantityDisplay.transform.GetChild(2).GetComponent<Text>().text = attackerName + "�ܻ��ܣ�" + killCount;
                }
                else if (factions == "Blue")
                {
                    defeatQuantityDisplay = BlueDefeatQuantityDisplay;
                    defeatQuantityDisplay.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().sprite = GetBluePlayerAvatar(attackerName);//��ȡHeadPortrait��ʾ����ɱ����
                    defeatQuantityDisplay.transform.GetChild(2).GetComponent<Text>().text = attackerName + "�ܻ��ܣ�" + killCount;
                }
                else
                {
                    return; // ��ֹ��Ч��Ӫ 
                }

                UIManager.instance.UpdateKillRankingDisplay(redPlayers, bluePlayers); // ���ݺ�Ӻ����ӵ�����б�
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

    // ��ӷ������洢������HeadPortrait
    public void AddRedPlayerAvatar(string playerName, Sprite avatar)
    {
        redPlayerAvatars[playerName] = avatar;
    }

    // ��ӷ������洢�������HeadPortrait
    public void AddBluePlayerAvatar(string playerName, Sprite avatar)
    {
        bluePlayerAvatars[playerName] = avatar;
    }

    // ��ӷ�������ȡ������HeadPortrait
    public Sprite GetRedPlayerAvatar(string playerName)
    {
        if (redPlayerAvatars.ContainsKey(playerName))
        {
            return redPlayerAvatars[playerName];
        }
        return null; // ����Ĭ��HeadPortrait����������
    }

    // ��ӷ�������ȡ�������HeadPortrait
    public Sprite GetBluePlayerAvatar(string playerName)
    {
        if (bluePlayerAvatars.ContainsKey(playerName))
        {
            return bluePlayerAvatars[playerName];
        }
        return null; // ����Ĭ��HeadPortrait����������
    }


    //��ʼ����ҵ��볡��Ϣ
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
        userFactions[uid] = factions; // �洢�û���Ӫ��Ϣ

        //���ɳ�������
        GameObject obj = Instantiate(PlayerJoinText, PlayerJoinTextMainObject.transform);
        obj.transform.SetParent(PlayerJoinTextMainObject.transform);
        obj.GetComponent<Text>().text = name + text;


        Destroy(obj, 2f);


        //����ͷ��
        StartCoroutine(LoadImageAndStorePlayerAvatar(img, name, factions));
    }


    //��ʼ����ҵ��볡��Ϣ
    public void PlayerAddDictionary(string playerName, string playerUID, string playerImg)
    {
        //��UID����б��ʾ���������Ϸ��
        joinedPlayers.Add(playerUID);

        //����������Ϊuid
        playerNameToUID[playerName] = playerUID;


        //����ҵ�ͷ��Ҳ�����ֵ䣬����ΪԿ�ף�HeadPortrait������Ϊֵ
        UserProfile[playerName] = playerImg;
    }

    private IEnumerator LoadImageAndStorePlayerAvatar(string imageUrl, string playerName, string factions)
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
                if (factions == "Red")
                {
                    GameManagers.Instance.AddRedPlayerAvatar(playerName, sprite);//��ȡ�췽ͷ����ͷ��
                }
                  else
                {
                    GameManagers.Instance.AddBluePlayerAvatar(playerName, sprite);//��ȡ�췽ͷ����ͷ��
                }
            }
        }
    }
}
