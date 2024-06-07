using UnityEngine;
using System.Collections;

public class NormalUIComponet : MonoBehaviour
{
    public GameObject followGameObject;
    public Vector2 offset;
    private Vector2 screenPos;
    private Vector2 scalarOffset;
    private bool ignoreHalfScreen = true;
    public bool IgnoreHalfScreen { set => ignoreHalfScreen = value; }
    private void Start()
    {
        SetPosition();
    }

    private void LateUpdate()
    {
        if (Camera.main == null || this.followGameObject == null)
            return;

        screenPos = ScreenUtil.ConvertObjPosToScalar(this.followGameObject.transform.position);
        screenPos += offset;
        if (!ignoreHalfScreen)
            screenPos -= scalarOffset;

        this.gameObject.transform.localPosition = screenPos;
    }

    private void SetPosition()
    {
        if (Camera.main == null || this.followGameObject == null)
            return;
        Rect canvasRect = ScreenUtil.GetCanvasRect();
        scalarOffset = new Vector2(canvasRect.width / 2, canvasRect.height / 2);
        screenPos = ScreenUtil.ConvertObjPosToScalar(this.followGameObject.transform.position);
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

}
