
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInAndOpenObject : MonoBehaviour
{
    public Image imageToFade;
    public GameObject[] objectToOpen;
    public float fadeDuration = 1.0f;

    private bool isFading = false;
    private float startTime;
    public static FadeInAndOpenObject instace;



    private string playerName;
    private int index=0;
    Camera mainCamera;

    private void Awake()
    {
        instace = this;
        mainCamera = Camera.main;
    }
    private void Update()
    {
        if (isFading)
        {
            // 正在淡入中
            float elapsedTime = Time.time - startTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, alpha);

            if (elapsedTime >= fadeDuration)
            {
                // 淡入完成
                isFading = false;
                //imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, 1f); // 确保完全不透明
                imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, 0f); // 初始时透明
                imageToFade.gameObject.SetActive(false);
                GameObject obj = Instantiate(objectToOpen[index]);
                // 初始化脚本

                obj.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = playerName + "";
                int ranking = UIManager.instance.GetPlayerRanking(playerName);
                Debug.Log("已查询完毕排名");
                obj.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = ranking + "";
    
                if (index == 0)
                {
                    obj.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = GameManagers.Instance.GetBluePlayerAvatar(playerName);
                    print(obj.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject.name);
                }
              else
                {
                    obj.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = GameManagers.Instance.GetRedPlayerAvatar(playerName);
                    print(obj.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).gameObject.name);
                }

                StartCoroutine(OpenCameraAfterDelay(8.8f));
                Destroy(obj, 9);
                // 启动一个协程，在8秒后打开主摄像机
     

            }
        }
    }
    IEnumerator OpenCameraAfterDelay(float delay)
    {
        // 等待指定的延迟时间
        yield return new WaitForSeconds(delay);


            // 打开主摄像机
            mainCamera.enabled = true;
     
    }
    public void StartFadeIn(int indexs, string playerNames)
    {

        imageToFade.gameObject.SetActive(true);
              isFading = true;
        mainCamera.enabled = false;
        startTime = Time.time;
        index = indexs;

        playerName = playerNames;
 
    }
}
