/**
 * 开启SoftMask说明：
 * 1. 将ScrollView的“Mask”组件通过右键菜单“Covnert To SoftMask”转换为“Soft Mask”组件。
 * 2. 将其子节点的Image组件上添加“Soft Maskable"脚本，此步骤可通过 “Soft Mask”组件 的“Fix”
 *    选项一键添加。
 * 3.Shader SoftMask支持：
 *      1）名称添加“Hidden/” 前缀和“ (SoftMaskable)”后缀；
 *      2）引用添加 SoftMask.cginc和pragma “SOFTMASK_EDITOR”
 *          #include "Packages/com.coffee.softmask-for-ugui/Shaders/SoftMask.cginc"
 *          #pragma shader_feature __ SOFTMASK_EDITOR
 *      3）在fragment中处理最终的Alpha值
 *          color.a *= SoftMask(IN.vertex, IN.worldPosition);
 *      4）示例参考：UI-Default-SoftMask.shader
 *
 * 4. 注意事项：
 *      1）Soft Mask不可嵌套使用，可与RectMask、Mask组件组合使用。
 *      2）不要使用ShaderGraph生成的Shader。
 **/
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRectInput : ScrollRect, UILifeCycleInterface
{
    private const string SOFTMASK_HORIZONTAL_RIGHT = "SoftMask_Horizontal_Right";
    private const string SOFTMASK_HORIZONTAL_LEFT = "SoftMask_Horizontal_Left";
    private const string SOFTMASK_HORIZONTAL = "SoftMask_Horizontal";

    private const string SOFTMASK_VERTICTAL_DOWN = "SoftMask_Vertictal_Down";
    private const string SOFTMASK_VERTICTAL_UP = "SoftMask_Vertictal_Up";
    private const string SOFTMASK_VERTICTAL = "SoftMask_Vertictal";

    private const string SOFTMASK_DEFAULT = "SoftMask_Alpha_One";

    private const float MIN_THRESHOLD = 0.01f;
    private const float MAX_THRESHOLD = 0.99f;

    public string m_UIEventKey;

    private bool useSoftMask;
    private Image maskImage;
    private Sprite maskSpriteLeftOrUp;
    private Sprite maskSpriteRightOrDown;
    private Sprite maskSprite;
    private Sprite maskDefaultSprite;

    private bool isSoftMaskDragEnd;


    InputEventRegisterInfo<InputUIOnScrollEvent> m_register;
    public enum Direction
    {
        Horizontal,
        Vertical
    }
    public virtual void Init(string UIEventKey, int id, params object[] objs)
    {
        m_UIEventKey = UIEventKey + "_" + this.GetInstanceID();
        m_register = InputUIEventProxy.GetOnScrollListener(m_UIEventKey, name, OnSetContentAnchoredPosition);

        this.maskImage = GetComponent<Image>();

        // SoftMask softMask = this.GetComponent<SoftMask>();
        // if (softMask != null && softMask.enabled)
        // {
        //     this.useSoftMask = true;
        //     this.isSoftMaskDragEnd = false;
        //     InitMaskSprite();
        // }

    }
    private void InitMaskSprite()
    {
        if (this.horizontal)
        {
            this.maskSpriteRightOrDown = Resources.Load<Sprite>(SOFTMASK_HORIZONTAL_RIGHT);
            this.maskSpriteLeftOrUp = Resources.Load<Sprite>(SOFTMASK_HORIZONTAL_LEFT);
            this.maskSprite = Resources.Load<Sprite>(SOFTMASK_HORIZONTAL);
        }
        else if (this.vertical)
        {
            this.maskSpriteRightOrDown = Resources.Load<Sprite>(SOFTMASK_VERTICTAL_DOWN);
            this.maskSpriteLeftOrUp = Resources.Load<Sprite>(SOFTMASK_VERTICTAL_UP);
            this.maskSprite = Resources.Load<Sprite>(SOFTMASK_VERTICTAL);
        }

        this.maskDefaultSprite = Resources.Load<Sprite>(SOFTMASK_DEFAULT);
    }

    public virtual void Dispose()
    {
        m_register?.RemoveListener();

        ReleaseMask(this.maskSpriteLeftOrUp);
        ReleaseMask(this.maskSpriteRightOrDown);
        ReleaseMask(this.maskSprite);
        ReleaseMask(this.maskDefaultSprite);
    }

    private void ReleaseMask(Sprite maskSprite)
    {
        if (maskSprite != null)
            Resources.UnloadAsset(maskSprite);
        maskSprite = null;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        OnValueChanged(this.normalizedPosition);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        SetEndDragMask();

        if (this.maskImage != null && this.useSoftMask)
            this.isSoftMaskDragEnd = true;
    }

    private void SetEndDragMask()
    {
        if (this.maskImage == null || !this.useSoftMask)
            return;

        this.contentSize = this.content.sizeDelta;

        if (this.horizontal)
        {
            if (this.scrollRect.width > this.contentSize.x)
                this.maskImage.sprite = this.maskDefaultSprite;

        }
        else if (this.vertical)
        {
            if (this.scrollRect.height > this.contentSize.y)
                this.maskImage.sprite = this.maskDefaultSprite;
        }
    }

    public void OnValueChanged(Vector2 position)
    {
        if (this.maskImage == null || !this.useSoftMask)
            return;

        if (this.horizontal && this.vertical)
        {
            return;
        }
        else if (this.horizontal)
        {
            //Vector(1.0,0.0), 左滑 ；Vector(0.0, 0.0), 右滑

            //右滑
            if (position.x > MAX_THRESHOLD)
                this.maskImage.sprite = this.maskSpriteRightOrDown;
            else if (position.x < MIN_THRESHOLD)//左滑
                this.maskImage.sprite = this.maskSpriteLeftOrUp;
            else
                this.maskImage.sprite = this.maskSprite;
        }
        else if (this.vertical)
        {
            //Vector(0.0,1.0),上滑；Vector(0.0,0.0) 下滑

            //上滑
            if (position.y > MAX_THRESHOLD)
                this.maskImage.sprite = this.maskSpriteLeftOrUp;
            else if (position.y < MIN_THRESHOLD)  //下滑
                this.maskImage.sprite = this.maskSpriteRightOrDown;
            else
                this.maskImage.sprite = this.maskSprite;
        }
    }

    protected override void LateUpdate()
    {
        base.LateUpdate();

        if (this.isSoftMaskDragEnd && ((this.velocity.x > -MIN_THRESHOLD && this.velocity.x < MIN_THRESHOLD) &&
            (this.velocity.y > -MIN_THRESHOLD && this.velocity.y < MIN_THRESHOLD)))
        {
            this.isSoftMaskDragEnd = false;

            OnValueChanged(this.normalizedPosition);
        }
    }

    public virtual void SetData(List<Dictionary<string, object>> data)
    {
        SetInitMask();
    }

    public virtual void SetData(List<Dictionary<string, object>> data, bool needDelay)
    {
        SetInitMask();
    }

    private Vector2 contentSize;
    private Rect scrollRect;
    private void SetInitMask()
    {
        if (this.maskImage == null || !this.useSoftMask)
            return;

        this.scrollRect = this.GetComponent<RectTransform>().rect;

        this.contentSize = this.content.sizeDelta;

        if (this.horizontal)
        {
            if (this.scrollRect.width > this.contentSize.x)
                this.maskImage.sprite = this.maskDefaultSprite;
            else
                this.maskImage.sprite = this.maskSpriteRightOrDown;

        }
        else if (this.vertical)
        {
            if (this.scrollRect.height > this.contentSize.y)
                this.maskImage.sprite = this.maskDefaultSprite;
            else
                this.maskImage.sprite = this.maskSpriteLeftOrUp;
        }
    }

    protected override void SetContentAnchoredPosition(Vector2 position)
    {
        InputUIEventProxy.DispatchScrollEvent(m_UIEventKey, name, "", position);
    }

    protected virtual void OnSetContentAnchoredPosition(InputUIOnScrollEvent e)
    {
        base.SetContentAnchoredPosition(e.m_pos);
    }

    /// <summary>
    /// 递归调用Init接口
    /// </summary>
    /// <param name="uiBase"></param>
    protected void RecursionInitUI(UIBase parentUI, UIBase uiBase)
    {
        if (uiBase.m_objectList.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < uiBase.m_objectList.Count; i++)
        {
            GameObject go = uiBase.m_objectList[i];

            if (go != null)
            {
                UIBase tmp = go.GetComponent<UIBase>();

                if (tmp != null)
                {
                    tmp.OnInit();
                    if (tmp.m_objectList.Count > 0)
                    {
                        RecursionInitUI(null, tmp);
                    }
                }
            }
            else
            {
                LogUtil.Warning("Init Scroll objectList[" + i + "] is null !: " + uiBase.UIEventKey);
            }
        }
    }

    /// <summary>
    /// 递归调用Dispose接口
    /// </summary>
    /// <param name="uiBase"></param>
    protected void RecursionDisposeUI(UIBase parentUI, UIBase uiBase)
    {
        if (uiBase.m_objectList.Count <= 0)
        {
            return;
        }
        for (int i = 0; i < uiBase.m_objectList.Count; i++)
        {
            GameObject go = uiBase.m_objectList[i];

            if (go != null)
            {
                UIBase tmp = go.GetComponent<UIBase>();

                if (tmp != null)
                {
                    tmp.Dispose();
                    if (tmp.m_objectList.Count > 0)
                    {
                        RecursionDisposeUI(null, tmp);
                    }
                }
            }
            else
            {
                LogUtil.Warning("Init Scroll objectList[" + i + "] is null !: " + uiBase.UIEventKey);
            }
        }
    }


    protected void DisposeItem(ReusingScrollItemBase item)
    {
        item.Dispose();
        RecursionDisposeUI(null, item);
        Destroy(item.gameObject);
    }
}
