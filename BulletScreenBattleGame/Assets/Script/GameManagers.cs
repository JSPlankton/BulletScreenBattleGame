//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    
    public Text texts;

    //���������չʾ
    public GameObject RedDefeatQuantityDisplay;

    //����������չʾ
    public GameObject BlueDefeatQuantityDisplay;

    private Sprite unitImage;//�û�ͷ��

    // �洢ÿ���û��Ļ�ɱ��¼
    private Dictionary<string, int> userKillRecords = new Dictionary<string, int>();
    private Dictionary<string, int> userLastDisplayKill = new Dictionary<string, int>(); // ��¼�ϴ���ʾʱ�Ļ�ɱ��


    public List<string> redPlayers = new List<string>(); // �������б�
    public List<string> bluePlayers = new List<string>(); // ��������б�
    public HashSet<string> joinedPlayers = new HashSet<string>(); // �Ѽ���������Ҽ���

    public GameObject PlayerJoinText;//��Ҽ������Ϣ
    public GameObject PlayerJoinTextMainObject;//��Ҽ�����Ϣ��������


    public Sprite blueImage;
    public Sprite redImage;


    private bool isUnit = false;
    // �洢�����Ϣ�Ľṹ��
    private struct PlayerInfo
    {
        public string name;
        public string headImage;
    }
    //�洢���ֺ�ͷ��
    public Dictionary<string, Sprite> redPlayerAvatars = new Dictionary<string, Sprite>();
    public Dictionary<string, Sprite> bluePlayerAvatars = new Dictionary<string, Sprite>();

    public Text maxUnitText;

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
    //��λ���        //��������        //ͼƬ·��          //����       //��Ӫ
    //  1                   5             path_to_sprite      Unit1         Red
    //  2                   3             null                Unit2         Blue



    private void Start()
    {

        GameManagers.Instance.redPlayers.Add("Ϧ��");

        GameManagers.Instance.bluePlayers.Add("�����Ҷ"); 
        //GameManagers.Instance.redPlayers.Add("Ϧ�º�2");

        //GameManagers.Instance.bluePlayers.Add("Ϧ����2");
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //SpawnAllUnits(Random.Range(1, redUnitPrefabs.Count), Random.Range(5, 15), null, "Ϧ�º�2", "Red");
            StartCoroutine(SpawnUnitAsync(Random.Range(1, redUnitPrefabs.Count), Random.Range(5, 10), "Ϧ��", "Red"));
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            //SpawnAllUnits(Random.Range(1, blueUnitPrefabs.Count), Random.Range(5, 15), null, "Ϧ����2", "Blue");
            StartCoroutine(SpawnUnitAsync(Random.Range(1, blueUnitPrefabs.Count), Random.Range(5, 10), "�����Ҷ", "Blue"));
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            //SpawnAllUnits(0, 200, null, "Ϧ����", "Blue");
            StartCoroutine(SpawnUnitAsync(0, 200, "", "Blue"));

        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            //SpawnAllUnits(0, 200, null, "Ϧ�º�", "Red");
            StartCoroutine(SpawnUnitAsync(0, 200,  "", "Red"));
        }
        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    StartCoroutine(SpawnUnitAsync(0, 1000, null, "Ϧ����", "Blue"));
        //}
        //if (Input.GetKeyDown(KeyCode.C))
        //{
        //    StartCoroutine(SpawnUnitAsync(0, 1000, null, "Ϧ�º�", "Red"));
        //}

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
  
    private IEnumerator SpawnUnitAsync(int unitNumber, int numberOfUnits, string unitName, string faction)
    {
        yield return null; // Yield once to ensure the coroutine runs on the next frame

        yield return SpawnUnit(unitNumber, numberOfUnits, unitName, faction);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="unitNumber">��λ���</param>
    /// <param name="numberOfUnits">��������</param>
    /// <param name="unitImage">ͼƬ</param>
    /// <param name="unitName">����</param>
    /// <param name="faction">��Ӫ</param>
    // ���ɵ�λ�ĺ���        //��λ���       //��������                                //ͼƬ            //����          //��Ӫ

    public IEnumerator SpawnUnit(int unitNumber, int numberOfUnits, /*string imageUrl,*/ string unitName, string faction)
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
            yield break;
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
                spawnPosition = points.transform.position - row * rowSpacing * points.transform.forward + col * unitSpacing * points.transform.right;
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
        yield break;
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


    // Э�̣���������ͼƬ
    //public IEnumerator LoadImage(string imageUrl)
    //{
    //    unitImage = null;
    //    using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageUrl))
    //    {
    //        yield return webRequest.SendWebRequest();

    //        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
    //        {
    //            Debug.LogError("Error loading image: " + webRequest.error);
    //        }
    //        else
    //        {
    //            Texture2D texture = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
    //            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
    //            unitImage = sprite;
    //            // ���� SpawnUnit����������ͼƬ�� Sprite

    //        }
    //    }
    //}

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
    public void HandleKillEvent(string summonerName, string factions)
    {
        if (string.IsNullOrEmpty(summonerName))
        {
            return;
        }

        if (userKillRecords.ContainsKey(summonerName))
        {

            if (summonerName == "")
            {
                return;
            }
            userKillRecords[summonerName]++;
            int requiredKills;
            // ����������Լ���Ƿ�������ʾ����


                requiredKills = userLastDisplayKill.ContainsKey(summonerName) ? userLastDisplayKill[summonerName] + 1000 : 888;


            int requiredKills2;
            // ����������Լ���Ƿ�������ʾ����
            requiredKills2 = userLastDisplayKill.ContainsKey(summonerName) ? userLastDisplayKill[summonerName] + 20 : 20;
            if (userKillRecords[summonerName] >= requiredKills2)
            {
                UIManager.instance.UpdateKillRankingDisplay(redPlayers, bluePlayers); // ���ݺ�Ӻ����ӵ�����б�
            }


                int killCount = userKillRecords[summonerName];

            if (userKillRecords[summonerName] >= requiredKills)
            {
                //Debug.Log(summonerName + " �Ļ�ɱ���ﵽ " + killCount + " �Σ�");
                userLastDisplayKill[summonerName] = userKillRecords[summonerName]; // �����ϴ���ʾʱ�Ļ�ɱ��

                GameObject defeatQuantityDisplay;
                if (factions == "Red")
                {
                    defeatQuantityDisplay = RedDefeatQuantityDisplay;
                    defeatQuantityDisplay.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().sprite = GetRedPlayerAvatar(summonerName);//��ȡͷ����ʾ����ɱ����
                    defeatQuantityDisplay.transform.GetChild(2).GetComponent<Text>().text = summonerName + "�ܻ��ܣ�" + killCount;
                }
                else if (factions == "Blue")
                {
                    defeatQuantityDisplay = BlueDefeatQuantityDisplay;
                    defeatQuantityDisplay.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().sprite = GetBluePlayerAvatar(summonerName);//��ȡͷ����ʾ����ɱ����
                    defeatQuantityDisplay.transform.GetChild(2).GetComponent<Text>().text = summonerName + "�ܻ��ܣ�" + killCount;
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

    // ��ӷ������洢������ͷ��
    public void AddRedPlayerAvatar(string playerName, Sprite avatar)
    {
        redPlayerAvatars[playerName] = avatar;
    }

    // ��ӷ������洢�������ͷ��
    public void AddBluePlayerAvatar(string playerName, Sprite avatar)
    {
        bluePlayerAvatars[playerName] = avatar;
    }

    // ��ӷ�������ȡ������ͷ��
    public Sprite GetRedPlayerAvatar(string playerName)
    {
        if (redPlayerAvatars.ContainsKey(playerName))
        {
            return redPlayerAvatars[playerName];
        }
        return null; // ����Ĭ��ͷ�����������
    }

    // ��ӷ�������ȡ�������ͷ��
    public Sprite GetBluePlayerAvatar(string playerName)
    {
        if (bluePlayerAvatars.ContainsKey(playerName))
        {
            return bluePlayerAvatars[playerName];
        }
        return null; // ����Ĭ��ͷ�����������
    }
}
