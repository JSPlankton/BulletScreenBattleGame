/**
 *	name: 场景UI组件类
 *	      使场景UI对齐建筑
 *	author:zhaojinlun
 *	date:2021年4月13日
 *	copyright: fungather.net
 ***/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneUIComponent : MonoBehaviour
{
    public GameObject followGameObject;
    public Vector2 offset;
    private Vector2 screenPos;
    private Vector2 scalarOffset;
    private Vector3 followWorldPos = default;
    private void Start()
    {
        SetPosition();
    }

    private void LateUpdate()
    {
        if (Camera.main == null || (this.followGameObject == null && followWorldPos == default)) return;
        if (followGameObject == null)
        {
            screenPos = ScreenUtil.ConvertWorldToScalar(followWorldPos);
        }
        else
        {
            screenPos = ScreenUtil.ConvertWorldToScalar(this.followGameObject.transform.position);
        }
        
        screenPos -= scalarOffset;
        screenPos += offset;
        this.gameObject.transform.localPosition = screenPos;
    }

    private void SetPosition()
    {
        if (Camera.main == null || (this.followGameObject == null && followWorldPos == default)) return;
        
        Rect canvasRect = ScreenUtil.GetCanvasRect();
        scalarOffset = new Vector2(canvasRect.width / 2, canvasRect.height / 2);
        if (followGameObject == null)
        {
            screenPos = ScreenUtil.ConvertWorldToScalar(followWorldPos);
        }
        else
        {
            screenPos = ScreenUtil.ConvertWorldToScalar(this.followGameObject.transform.position);
        }
        screenPos -= scalarOffset;
        screenPos += offset;
        this.gameObject.transform.localPosition = screenPos;
    }

    public void SetFollowObject(GameObject followGameObject, Vector2 offset)
    {
        this.followGameObject = followGameObject;
        this.offset = offset;
    }

    public void ChangePos(Vector2 offPos)
    {
        offset = offPos;
        SetPosition();
    }

    #region FollowWorldPos
    public void SetFollowWorldPos(Vector3 worldPos)
    {
        followWorldPos = worldPos;
    }
    #endregion

}
