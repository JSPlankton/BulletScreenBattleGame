using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeInImages : MonoBehaviour
{
    public float delayBeforeFadeIn = 2f; // 延迟开始淡入的时间
    public float fadeInDuration = 2f; // 淡入持续时间
    public Image[] imageComponents; // 存储Image组件的数组
    public Text[] textComponents; // 存储Text组件的数组

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

        // 获取并记录每个Image组件和Text组件的初始颜色和目标颜色
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

            // 使用Lerp逐渐改变每个Image组件的颜色透明度
            for (int i = 0; i < imageComponents.Length; i++)
            {
                float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
                imageComponents[i].color = new Color(imageTargetColors[i].r, imageTargetColors[i].g, imageTargetColors[i].b, alpha);
            }

            // 使用Lerp逐渐改变每个Text组件的颜色透明度
            for (int i = 0; i < textComponents.Length; i++)
            {
                float alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeInDuration);
                textComponents[i].color = new Color(textTargetColors[i].r, textTargetColors[i].g, textTargetColors[i].b, alpha);
            }

            yield return null;
        }

        // 确保所有Image组件和Text组件完全不透明
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
