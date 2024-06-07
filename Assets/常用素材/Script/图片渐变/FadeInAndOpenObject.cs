
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
            // ���ڵ�����
            float elapsedTime = Time.time - startTime;
            float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, alpha);

            if (elapsedTime >= fadeDuration)
            {
                // �������
                isFading = false;
                //imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, 1f); // ȷ����ȫ��͸��
                imageToFade.color = new Color(imageToFade.color.r, imageToFade.color.g, imageToFade.color.b, 0f); // ��ʼʱ͸��
                imageToFade.gameObject.SetActive(false);
                GameObject obj = Instantiate(objectToOpen[index]);
                // ��ʼ���ű�

                obj.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).GetComponent<Text>().text = playerName + "";
                int ranking = UIManager.instance.GetPlayerRanking(playerName);
                Debug.Log("�Ѳ�ѯ�������");
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
                // ����һ��Э�̣���8�����������
     

            }
        }
    }
    IEnumerator OpenCameraAfterDelay(float delay)
    {
        // �ȴ�ָ�����ӳ�ʱ��
        yield return new WaitForSeconds(delay);


            // ���������
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
