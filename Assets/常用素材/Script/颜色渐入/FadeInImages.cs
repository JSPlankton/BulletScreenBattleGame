using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInImages : MonoBehaviour
{
    public float delayBeforeFadeIn = 2f; // �ӳٿ�ʼ�����ʱ��
    public float fadeInDuration = 2f; // �������ʱ��
    public Image[] imageComponents; // �洢Image���������
    public Text[] textComponents; // �洢Text���������

    private Color[] imageTargetColors;
    private Color[] imageInitialColors;
    private Color[] textTargetColors;
    private Color[] textInitialColors;
    private float elapsedTime = 0f;
    private bool hasStartedFading = false;
    private bool isDelayFinished = false;

    private void Start()
    {
        imageTargetColors = new Color[imageComponents.Length];
        imageInitialColors = new Color[imageComponents.Length];
        textTargetColors = new Color[textComponents.Length];
        textInitialColors = new Color[textComponents.Length];

        // ��ȡ����¼ÿ��Image�����Text����ĳ�ʼ��ɫ��Ŀ����ɫ
        for (int i = 0; i < imageComponents.Length; i++)
        {
            imageInitialColors[i] = imageComponents[i].color;
            imageTargetColors[i] = new Color(imageInitialColors[i].r, imageInitialColors[i].g, imageInitialColors[i].b, 1f);
            imageComponents[i].color = new Color(imageInitialColors[i].r, imageInitialColors[i].g, imageInitialColors[i].b, 0f);
        }

        for (int i = 0; i < textComponents.Length; i++)
        {
            textInitialColors[i] = textComponents[i].color;
            textTargetColors[i] = new Color(textInitialColors[i].r, textInitialColors[i].g, textInitialColors[i].b, 1f);
            textComponents[i].color = new Color(textInitialColors[i].r, textInitialColors[i].g, textInitialColors[i].b, 0f);
        }
        StartCoroutine(FadeIn(delayBeforeFadeIn));
    }


    private IEnumerator FadeIn(float delayBeforeFadeIns)
    {
        yield return new WaitForSeconds(delayBeforeFadeIns);
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;

            // ʹ��Lerp�𽥸ı�ÿ��Image�������ɫ͸����
            for (int i = 0; i < imageComponents.Length; i++)
            {
                float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
                imageComponents[i].color = new Color(imageTargetColors[i].r, imageTargetColors[i].g, imageTargetColors[i].b, alpha);
            }

            // ʹ��Lerp�𽥸ı�ÿ��Text�������ɫ͸����
            for (int i = 0; i < textComponents.Length; i++)
            {
                float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
                textComponents[i].color = new Color(textTargetColors[i].r, textTargetColors[i].g, textTargetColors[i].b, alpha);
            }

            yield return null;
        }

        // ȷ������Image�����Text�����ȫ��͸��
        for (int i = 0; i < imageComponents.Length; i++)
        {
            imageComponents[i].color = imageTargetColors[i];
        }

        for (int i = 0; i < textComponents.Length; i++)
        {
            textComponents[i].color = textTargetColors[i];
        }
    }
}
