/**
 *	name: 屏幕比率工具类 
 *	author:zhaojinlun
 *	date:2021年5月10日
 *	copyright: fungather.net
 ***/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenUtil
{
    public const int DESGIN_RESOLUTION_WIDTH = 640;
    public const int DESGIN_RESOLUTION_HEIGHT = 1136;
    public const string CANVAS_PATH = "UIManager/UIRoot";
    public const string CAMERA_PATH = CANVAS_PATH + "/UICamera";
    public const string SNEAK_FIGHT_CAMERA_PATH = "Environment/MyCamera/UICamera";
    private static float MIN_WIDTH_OR_HEIGHT = 1;
    private static CanvasScaler canvasScaler;
    private static Camera uiCamera;
    private static Rect rect;
    /// <summary>
    /// 获取物理屏幕的宽高比率
    /// </summary>
    /// <returns></returns>
    public static float GetScreenRatio()
    {
        return Screen.width * 1.0f / Screen.height;
    }

    /// <summary>
    /// 获取设计屏幕的宽高比率
    /// </summary>
    /// <returns></returns>
    public static float GetDesignScreenRatio()
    {
        return ScreenUtil.DESGIN_RESOLUTION_WIDTH * 1.0f / ScreenUtil.DESGIN_RESOLUTION_HEIGHT;
    }


    public static Camera GetUICamera()
    {
        if(uiCamera == null)
        {
            GameObject go = GameObject.Find(CAMERA_PATH);
            uiCamera = go.GetComponent<Camera>();
        }
        return uiCamera;
    }
    public static CanvasScaler GetCanvasScaler()
    {
        if (canvasScaler == null)
        {
            GameObject go = GameObject.Find(CANVAS_PATH);
            canvasScaler = go.GetComponent<CanvasScaler>();
        }
        return canvasScaler;
    }
    public static Rect GetCanvasRect()
    {
        if (rect.width < MIN_WIDTH_OR_HEIGHT || rect.height < MIN_WIDTH_OR_HEIGHT)
            rect = GetCanvasScaler().GetComponent<RectTransform>().rect;
        return rect;
    }

    /// <summary>
    /// 得到当前鼠标位置
    /// </summary>
    /// <param name="parentTrans"></param>
    /// <returns></returns>
    public static Vector2 CurrMousePosition(Transform parentTrans)
    {
        Vector2 vecMouse = Vector2.one;
        if (null == parentTrans)
        {
            return vecMouse;
        }
        RectTransform rectTrans = parentTrans.GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTrans, Input.mousePosition, GetUICamera(), out vecMouse);
        return vecMouse;
    }
    /// <summary>
    /// ui 转 ui 
    /// </summary>
    /// <param name="rectTransform"></param>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <returns></returns>
    public static Vector2 ConvertObjPosToUI(RectTransform rectTransform, float posX, float posY)
    {
        Vector2 screenPos = ConvertObjPosToScreen(posX, posY);
        Vector2 outVec;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPos, GetUICamera(), out outVec))
        {
            return outVec;
        }
       
        return Vector2.zero;
    }
    /// <summary>
    /// 战斗场景世界坐标转UI
    /// </summary>
    /// <param name="rectTransform"></param>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public static Vector2 ConvertAreaUnlockScenePosToUI(RectTransform rectTransform, Vector3 worldPos)
    {
        Vector2 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 outVec;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPos, GetUICamera(), out outVec))
        {
            return outVec;
        }

        return Vector2.zero;
    }
    /// <summary>
    /// 屏幕坐标转换到Scalar屏幕坐标
    /// </summary>
    /// <param name="screenPos"></param>
    /// <returns></returns>
    public static Vector2 ConvertScreenToScalar(Vector2 screenPos)
    {
        return ConvertScreenToScalar(screenPos.x, screenPos.y);
    }

    /// <summary>
    /// 屏幕坐标转换到Scalar屏幕坐标
    /// </summary>
    /// <param name="screenX">物理屏幕坐标X</param>
    /// <param name="screenY">物理屏幕坐标Y</param>
    /// <returns></returns>
    public static Vector2 ConvertScreenToScalar(float screenX, float screenY)
    {
        Rect canvasRect = GetCanvasRect();
        float screenXPercent = screenX / Screen.width;
        float screenYPercent = screenY / Screen.height;
        float scalarX = canvasRect.width * screenXPercent;
        float scalarY = canvasRect.height * screenYPercent;

        return new Vector2(scalarX, scalarY);
    }


    /// <summary>
    /// 组件坐标转物理屏幕坐标
    /// 注意事项：组件的坐标原点是左下角
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <returns></returns>
    public static Vector2 ConvertScalarToScreen(float posX, float posY)
    {
        Rect canvasRect = GetCanvasRect();
        float rectXPercent = posX / canvasRect.width;
        float rectYPercent = posY / canvasRect.height;
        float screenX = Screen.width * rectXPercent;
        float screenY = Screen.height * rectYPercent;

        return new Vector2(screenX, screenY);
    }

    /// <summary>
    /// UI局部坐标转物理屏幕坐标，
    /// 注意：易受到锚点的位置的影响
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public static Vector2 ConvertObjPosToScalar(Vector3 worldPos)
    {
        return ConvertObjPosToScalar(worldPos.x, worldPos.y);
    }

    /// <summary>
    /// UI局部坐标转物理屏幕坐标，
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <returns></returns>
    public static Vector2 ConvertObjPosToScalar(float posX, float posY)
    {
        Vector2 screenPos = ConvertObjPosToScreen(posX, posY);
        return ConvertScreenToScalar(screenPos);
    }

    /// <summary>
    /// UI局部坐标转物理屏幕坐标，
    /// 注意：易受到锚点的位置的影响
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <returns></returns>
    public static Vector2 ConvertObjPosToScreen(float posX, float posY)
    {
        return RectTransformUtility.WorldToScreenPoint(
            GetUICamera(), 
            new Vector3(posX, posY, 0));
    }
    /// <summary>
    /// UI局部坐标转物理屏幕坐标，
    /// 注意：易受到锚点的位置的影响
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public static Vector2 ConvertObjPosToScreen(Vector3 worldPos)
    {
        return ConvertObjPosToScreen(worldPos.x, worldPos.y);
    }
    /// <summary>
    /// 世界坐标转Scalar坐标
    /// </summary>
    /// <param name="worldPos"></param>
    /// <returns></returns>
    public static Vector2 ConvertWorldToScalar(Vector3 worldPos)
    {
        Vector3 screen = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 scalarPos = ScreenUtil.ConvertScreenToScalar(screen.x, screen.y);
        return scalarPos;
    }
    /// <summary>
    /// ui 屏幕坐标转世界坐标
    /// </summary>
    /// <param name="screenPos"></param>
    /// <returns></returns>
    public static Vector2 ConvertScreenToWorld(Vector3 screenPos)
    {
        return GetUICamera().ScreenToWorldPoint(screenPos);
    }

    /// <summary>
    /// 获取对象在屏幕中的真实大小
    /// </summary>
    /// <param name="rect">对象</param>
    /// <returns></returns>
    public static Vector2 GetActualRect(RectTransform rect)
    {
        return new Vector2(
            rect.rect.width * (Screen.width * 1.0f / ScreenUtil.DESGIN_RESOLUTION_WIDTH),
            rect.rect.height * (Screen.height * 1.0f / ScreenUtil.DESGIN_RESOLUTION_HEIGHT)
        );
    }

    /// <summary>
    /// 是否在对象内点击
    /// </summary>
    /// <param name="clickPosition">鼠标点击位置</param>
    /// <param name="leftDown">对象左下坐标</param>
    /// <param name="rightTop">对象右上坐标</param>
    /// <returns></returns>
    public static bool IsClickedInRect(Vector2 clickPosition, Vector2 leftDown, Vector2 rightTop)
    {
        if (clickPosition.x > leftDown.x && clickPosition.y > leftDown.y &&
            clickPosition.x < rightTop.x && clickPosition.y < rightTop.y)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 移动端使用
    /// </summary>
    /// <param name="screenPosition"></param>
    /// <returns></returns>
    private static bool IsPointerOverUIObject(Vector2 screenPosition)
    {
        //实例化点击事件
        UnityEngine.EventSystems.PointerEventData eventDataCurrentPosition = new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current);
        //将点击位置的屏幕坐标赋值给点击事件
        eventDataCurrentPosition.position = new Vector2(screenPosition.x, screenPosition.y);

        List<UnityEngine.EventSystems.RaycastResult> results = new List<UnityEngine.EventSystems.RaycastResult>();
        //向点击处发射射线
        UnityEngine.EventSystems.EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }
    /// <summary>
    /// 是否点击到UI上
    /// </summary>
    /// <returns></returns>
    public static bool IsPointerOverGameObject()
    {
        bool result = false;
#if UNITY_EDITOR
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            result = true;
#else
        foreach (Touch touch in Input.touches)
        {
            if(IsPointerOverUIObject(touch.position))
            {
                result = true;
                break;
            }

        }
#endif
        return result;
    }


    #region 潜行小游戏
    /// <summary>
    /// 潜行小游戏UI摄像机
    /// </summary>
    /// <returns></returns>
    private static Camera GetMiniGameUICamera()
    {
        GameObject go = GameObject.Find(SNEAK_FIGHT_CAMERA_PATH);
        if (go != null)
        {
            return go.GetComponent<Camera>();
        }
        return uiCamera;
    }
    public static CanvasScaler GetMiniGameCanvasScaler()
    {
        GameObject go = GameObject.Find("ScreenUIManager");
        if (go != null)
            return go.GetComponent<CanvasScaler>();

        return canvasScaler;
    }
    public static Rect GetMiniGameCanvasRect()
    {
        if (rect.width < MIN_WIDTH_OR_HEIGHT || rect.height < MIN_WIDTH_OR_HEIGHT)
            rect = GetMiniGameCanvasScaler().GetComponent<RectTransform>().rect;
        return rect;
    }
    /// <summary>
    /// 小游戏UI局部坐标转物理屏幕坐标，
    /// 注意：易受到锚点的位置的影响
    /// </summary>
    /// <param name="posX"></param>
    /// <param name="posY"></param>
    /// <returns></returns>
    public static Vector2 ConvertMiniGameObjPosToScreen(float posX, float posY)
    {
        return RectTransformUtility.WorldToScreenPoint(
            GetMiniGameUICamera(),
            new Vector3(posX, posY, 0));
    }
    /// <summary>
    /// 小游戏UI界面
    /// ui 转 ui 
    /// </summary>
    /// <param name="rectTransform"></param>
    /// <param name="objpos"></param>
    /// <returns></returns>
    public static Vector2 ConvertMiniGameObjPosToUI(RectTransform rectTransform, Vector3 objpos)
    {
        Vector2 screenPos = ConvertMiniGameObjPosToScreen(objpos.x, objpos.y);
        Vector2 outVec;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPos, GetUICamera(), out outVec))
        {
            return outVec;
        }
        return Vector2.zero;
    }
    /// <summary>
    /// 屏幕坐标转换到Scalar屏幕坐标
    /// </summary>
    /// <param name="screenX">物理屏幕坐标X</param>
    /// <param name="screenY">物理屏幕坐标Y</param>
    /// <returns></returns>
    public static Vector2 ConvertMiniGameScreenToScalar(float screenX, float screenY)
    {
        Rect canvasRect = GetMiniGameCanvasRect();
        float screenXPercent = screenX / Screen.width;
        float screenYPercent = screenY / Screen.height;
        float scalarX = canvasRect.width * screenXPercent;
        float scalarY = canvasRect.height * screenYPercent;

        return new Vector2(scalarX, scalarY);
    }
    #endregion
}
