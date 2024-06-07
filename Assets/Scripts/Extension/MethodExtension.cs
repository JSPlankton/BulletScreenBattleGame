using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public static class MethodExtension
{
    #region GameObject
    /// <summary>
    /// 递归改变子节点的Scale
    /// </summary>
    public static void SetScale(this Transform tr, Vector3 scale, bool recursion = true)
    {
        tr.localScale = scale;
        if (recursion)
        {
            foreach (Transform item in tr)
            {
                SetScale(item, scale);
            }
        }
    }

    /// <summary>
    /// 递归修改子节点的层级
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="layer"></param>
    public static void SetLayer(this GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform item in obj.transform)
        {
            item.gameObject.layer = layer;
            SetLayer(item.gameObject, layer);
        }
    }
    /// <summary>
    /// 优化的设置SetActive方法，可以节约重复设置Active的开销
    /// </summary>
    public static void SetActiveOptimize(this GameObject go, bool isActive)
    {
        if (go.activeSelf != isActive)
        {
            go.SetActive(isActive);
        }
    }

    /// <summary>
    /// 反转当前active的状态
    /// </summary>
    public static void ReverseActiveState(this GameObject go)
    {
        go?.SetActive(!go.activeSelf);
    }

    #endregion

    #region UI
    /// <summary>
    /// 更换图片texture
    /// </summary>
    /// <param name="img"></param>
    /// <param name="abName"></param>
    /// <param name="name"></param>
    /// <param name="is_nativesize"></param>
    public static void SetTexture(this Image img, string abName, string name, bool is_nativesize = false)
    {
        if (name == null)
        {
            LogUtil.Error("set_icon Image name 不能为 null !");
            return;
        }

        if (img == null)
        {
            LogUtil.Error("set_icon Image 不能为 null !");
            return;
        }
        try
        {
            Sprite assetData = AssetManager.Instance.LoadAsset<Sprite>(abName, name, ".png");
            if (assetData != null)
            {
                img.overrideSprite = assetData;
                img.sprite = img.overrideSprite;
            }
            else
            {
                img.overrideSprite = null;
                img.sprite = null;
            }

            if (is_nativesize)
                img.SetNativeSize();
        }
        catch (Exception e)
        {
            LogUtil.Exception("Image.Texture 加载失败，查看资源是否存在，图片格式是否正确:" + name, e);
        }
    }

    public static void SetTextColor(this Text uiText, string colorHex)
    {
        if (uiText == null) return;
        if (!string.IsNullOrEmpty(colorHex))
        {
            Color newColor = uiText.color;
            ColorUtility.TryParseHtmlString(colorHex, out newColor);
            uiText.color = newColor;
        }
    }

    /// <summary>
    /// 非空 且 已经Destroy
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    //public static bool IsDestroyed(this UnityEngine.Object self) { return !(self is null) && self == null; } wait for further usage

    /// <summary>
    /// 为空 或 已经Destroy
    /// </summary>
    /// <param name="self"></param>
    /// <returns></returns>
    //public static bool IsNullOrDestroyed(this UnityEngine.Object self) { return self == null; } wait for further usage
    #endregion

    #region Animator

    /// <summary>
    /// 可以重复调用的过渡动画
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="animName"></param>
    public static void CustomCrossFade(this Animator anim, string animName, int layer = 0, float time = 0.5f)
    {
        if (!anim.GetCurrentAnimatorStateInfo(layer).IsName(animName)
            && !anim.GetNextAnimatorStateInfo(layer).IsName(animName)
            )
        {
            anim.CrossFade(animName, time, layer);
        }

    }

    //获取切换进度
    public static float GetCrossFadeProgress(this Animator anim, int layer = 0)
    {
        if (anim.GetNextAnimatorStateInfo(layer).shortNameHash == 0)
        {
            return 1;
        }

        return anim.GetCurrentAnimatorStateInfo(layer).normalizedTime % 1;
    }

    #endregion

    #region ParticleSystem

    public static void RecursionPlay(this GameObject ps)
    {
        ParticleSystem[] list = ps.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < list.Length; i++)
        {
            list[i].Play();
        }
    }

    public static void RecursionStop(this GameObject ps)
    {
        ParticleSystem[] list = ps.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < list.Length; i++)
        {
            list[i].Stop();
        }
    }

    public static void RecursionPause(this GameObject ps)
    {
        ParticleSystem[] list = ps.GetComponentsInChildren<ParticleSystem>();

        for (int i = 0; i < list.Length; i++)
        {
            list[i].Pause();
        }
    }

    #endregion

    #region 向量

    //向量逆时针旋转
    public static Vector3 Vector3RotateInXZ(this Vector3 dir, float angle)
    {
        angle *= Mathf.Deg2Rad;
        float l_n_dirX = dir.x * Mathf.Cos(angle) - dir.z * Mathf.Sin(angle);
        float l_n_dirZ = dir.x * Mathf.Sin(angle) + dir.z * Mathf.Cos(angle);
        Vector3 l_dir = new Vector3(l_n_dirX, dir.y, l_n_dirZ);

        return l_dir;
    }

    //向量顺时针
    public static Vector3 Vector3RotateInXZ2(this Vector3 dir, float angle)
    {

        angle *= Mathf.Deg2Rad;
        float l_n_dirX = dir.x * Mathf.Cos(angle) + dir.z * Mathf.Sin(angle);
        float l_n_dirZ = -dir.x * Mathf.Sin(angle) + dir.z * Mathf.Cos(angle);

        Vector3 l_dir = new Vector3(l_n_dirX, dir.y, l_n_dirZ);

        return l_dir;
    }

    //位置绕点旋转顺时针，逆时针角度乘以-1即可
    public static Vector3 PostionRotateInXZ(this Vector3 pos, Vector3 center, float angle)
    {
        angle *= -Mathf.Deg2Rad;
        float x = (pos.x - center.x) * Mathf.Cos(angle) - (pos.z - center.z) * Mathf.Sin(angle) + center.x;
        float z = (pos.x - center.x) * Mathf.Sin(angle) + (pos.z - center.z) * Mathf.Cos(angle) + center.z;

        Vector3 newPos = new Vector3(x, pos.y, z);

        return newPos;
    }

    //获取一个顺时针夹角(需先标准化向量)
    public static float GetRotationAngle(this Vector3 dir, Vector3 aimDir)
    {
        //dir = dir.normalized;
        //aimDir = aimDir.normalized;

        float angle = (float)(Math.Acos(Vector3.Dot(dir, aimDir)) * 180 / Math.PI);

        if (angle != 180 && angle != 0)
        {
            float cross = dir.x * aimDir.z - aimDir.x * dir.z;
            if (cross < 0)
            {
                return angle;
            }
            else
            {
                return 360 - angle;
            }
        }

        return angle;
    }

    public static bool Approximately(this Quaternion quatA, Quaternion value, float acceptableRange)
    {
        return 1 - Mathf.Abs(Quaternion.Dot(quatA, value)) < acceptableRange;
    }

    #endregion

    #region ToSaveString
    public static string ToSaveString(this Vector3 v3)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(v3.x.ToString());
        sb.Append("|");
        sb.Append(v3.y.ToString());
        sb.Append("|");
        sb.Append(v3.z.ToString());

        return sb.ToString();
    }

    public static string ToSaveString(this Vector2 v2)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(v2.x.ToString());
        sb.Append("|");
        sb.Append(v2.y.ToString());

        return sb.ToString();
    }

    public static string ToSaveString(this Color color)
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(color.r.ToString());
        sb.Append("|");
        sb.Append(color.g.ToString());
        sb.Append("|");
        sb.Append(color.b.ToString());
        sb.Append("|");
        sb.Append(color.a.ToString());

        return sb.ToString();
    }

    public static string ToSaveString(this List<string> list)
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < list.Count; i++)
        {
            sb.Append(list[i]);

            if (i != list.Count - 1)
            {
                sb.Append("|");
            }
        }

        return sb.ToString();
    }

    public static string ToSaveString(this string[] list)
    {
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < list.Length; i++)
        {
            sb.Append(list[i]);

            if (i != list.Length - 1)
            {
                sb.Append("|");
            }
        }

        return sb.ToString();
    }

    #endregion

    #region Spine
    public static void DoFade(this SkeletonGraphic target,
    float alpha, float duration)
    {
        DOTween.To(() => target.color,
                   co => { target.color = co; },
                   new Color(target.color.r, target.color.g, target.color.b, alpha),
                   duration);

    }
    public static void DoColor(this SkeletonGraphic target,
    Color color, float duration)
    {
        DOTween.To(() => target.color,
                   co => { target.color = co; },
                   color,
                   duration);

    }
    #endregion

    #region RectTransform

    public static bool Overlaps(this RectTransform a, RectTransform b)
    {
        return a.WorldRect().Overlaps(b.WorldRect());
    }
    public static bool Overlaps(this RectTransform a, RectTransform b, bool allowInverse)
    {
        return a.WorldRect().Overlaps(b.WorldRect(), allowInverse);
    }

    public static Rect WorldRect(this RectTransform rectTransform)
    {
        Vector2 sizeDelta = rectTransform.sizeDelta;
        float rectTransformWidth = sizeDelta.x * rectTransform.lossyScale.x;
        float rectTransformHeight = sizeDelta.y * rectTransform.lossyScale.y;

        Vector3 position = rectTransform.position;
        return new Rect(position.x + rectTransformWidth * rectTransform.pivot.x, position.y - rectTransformHeight * rectTransform.pivot.y, rectTransformWidth, rectTransformHeight);
    }
    #endregion
}