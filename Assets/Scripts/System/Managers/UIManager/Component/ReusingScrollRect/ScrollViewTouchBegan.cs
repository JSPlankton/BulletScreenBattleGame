
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewTouchBegan : MonoBehaviour
{
    public ScrollRect scrollRect;

    bool IsTouchBegin()
    {
        if (Input.GetMouseButtonDown(0))
            return true;

        if(Input.touchCount> 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (Input.GetTouch(i).phase == TouchPhase.Began)
                    return true;
            }
        }

        return false;
    }

    void Update()
    {
        if (scrollRect == null)
            return;

        if(IsTouchBegin())
            scrollRect.StopMovement();
    }
}
