using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CamShotUtil : MonoBehaviour
{
    public static bool takeShot = false;
    public static bool NewMat = false;
    public GameObject ShotImageGo;

    private void Awake()
    {
        takeShot = false;
    }

    RawImage posImg;
    public void OnCamShot(GameObject go, bool isNewMat = false)
    {
        ShotImageGo = go;
        NewMat = isNewMat;
        if (ShotImageGo != null)
        {
            posImg = ShotImageGo.GetComponent<RawImage>();
            if (posImg == null)
            {
                posImg = ShotImageGo.AddComponent<RawImage>();
            }
            posImg.raycastTarget = false;
            posImg.enabled = false;
            StartCoroutine(GetFrame(isNewMat));
        }
    }

    public void OnCamRealTimeShot(GameObject go)
    {
        ShotImageGo = go;
        if (ShotImageGo != null)
        {
            posImg = ShotImageGo.GetComponent<RawImage>();
            if (posImg == null)
            {
                posImg = ShotImageGo.AddComponent<RawImage>();
            }
            posImg.raycastTarget = false;
            posImg.material = AssetManager.Instance.Load("Settings/Renderer/RenderRes/RealTimeBlurM", typeof(Material)) as Material;
        }
    }

    public static RenderTexture tmpTex;

    IEnumerator GetFrame(bool isNewMat = false)
    {
        //取帧画面做模糊处理
        LogUtil.Log("JS_Blur_开始取帧缓存");
        if (isNewMat)
        {
            LogUtil.Log("JS_Blur_需要创建新的模糊RT");
            tmpTex = new RenderTexture(512, 512, 0);
        }
        takeShot = true;
        yield return null;
        DoGlassBlur(isNewMat);
    }

    void DoGlassBlur(bool isNewMat = false)
    {
        if (posImg != null)
        {
            if (isNewMat)
            {
                posImg.material = new Material(Shader.Find("JS/PostEffect/GlassBlur"));
                posImg.material.renderQueue = 3000;
                posImg.material.mainTexture = tmpTex;
            }
            else
            {
                posImg.material = AssetManager.Instance.Load("Settings/Renderer/RenderRes/GlassBlurM", typeof(Material)) as Material;
            }

            posImg.enabled = true;
            LogUtil.Log("JS_Blur_DoGlassBlur");
        }
    }

}
