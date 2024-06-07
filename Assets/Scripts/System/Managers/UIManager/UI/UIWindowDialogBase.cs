
using UnityEngine;

public class UIWindowDialogBase : UIWindowBase
{
    [Tooltip("是否屏蔽Android返回键")]
    public bool m_isDisableAndroidBackMenu = false;
    
    public static UIType UIType
    {
        get { return UIType.Dialog; }
    }

    //关联变量名称
    private const string CommonDialogTop = "CommonDialogTop";

    public override void OnInit()
    {
        base.OnInit();
        InitTitle();
    }

    protected override void OnUIDestroy()
    {
        base.OnUIDestroy();
        // if (HaveObject(CommonDialogTop))
        // {
        //     DestroyUIBase<CommonDialogTop>(CommonDialogTop);
        // }
    }

    /// <summary>
    /// 初始化
    /// </summary>
    protected void InitTitle()
    {
        if (!HaveObject(CommonDialogTop))
        {
            return;
        }
        // InitUIBase<CommonDialogTop>(CommonDialogTop, new object[] { this });
    }
    // protected CommonDialogTop GetCommonDialogTop()
    // {
    //     if (!HaveObject(CommonDialogTop))
    //     {
    //         return null;
    //     }
    //     return GetUIBase<CommonDialogTop>(CommonDialogTop);
    // }
    /// <summary>
    /// 设置标题
    /// </summary>
    /// <param name="text"></param>
    protected void SetTitleText(string text)
    {
        // CommonDialogTop top = GetCommonDialogTop();
        // if (null == top)
        // {
        //     return;
        // }
        // top.SetTitleText(text);
    }

    /// <summary>
    /// 设置钻石显示或隐藏
    /// </summary>
    /// <param name="isShow"></param>
    protected void ShowTitleDiamond(bool isShow)
    {
        // CommonDialogTop top = GetCommonDialogTop();
        // if (null == top)
        // {
        //     return;
        // }
        // top.ShowObjDiamond(isShow);
    }

    /// <summary>
    /// 设置图标
    /// </summary>
    /// <param name="picName"></param>
    /// <param name="picPath"></param>
    /// <param name="isNativeSize"></param>
    protected void SetDiamondImage(string picName, string picPath = "", bool isNativeSize = false)
    {
        // CommonDialogTop top = GetCommonDialogTop();
        // if (null == top)
        // {
        //     return;
        // }
        // top.SetDiamondImage(picName, picPath, isNativeSize);
    }
    /// <summary>
    /// 数量文本
    /// </summary>
    /// <param name="text"></param>
    protected void SetDiamondText(string text)
    {
        // CommonDialogTop top = GetCommonDialogTop();
        // if (null == top)
        // {
        //     return;
        // }
        // top.SetResText(text);
    }


    public virtual void OnAndroidBack()
    {
        Managers.UIManager.DestroyUI(this);
    }

    #region 引导

    public virtual Transform GetGuideTransform(string type, string key)
    {
        return null;
    }

    #endregion
}