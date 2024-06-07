using UnityEngine;
using System.Collections;
using System;

public class UIAnimManager : MonoBehaviour 
{
    //开始调用进入动画
    public void StartEnterAnim(UIWindowBase UIbase, UICallBack callBack, params object[] objs)
    {
        StartCoroutine(UIbase.EnterAnim(EndEnterAnim, callBack, objs));
    }

    //进入动画播放完毕回调
    public void EndEnterAnim(UIWindowBase UIbase, UICallBack callBack, params object[] objs)
    {
        UIbase.OnCompleteEnterAnim();
        UIbase.windowStatus = UIWindowBase.WindowStatus.Open;

        try
        {
            if (callBack!= null)
            {
                callBack(UIbase, objs);
            }
        }
        catch (Exception e)
        {
            LogUtil.Exception(e);
        }
    }

    //开始调用退出动画
    public void StartExitAnim(UIWindowBase UIbase, UICallBack callBack, params object[] objs)
    {
        StartCoroutine(UIbase.ExitAnim(EndExitAnim, callBack, objs));
    }

    //退出动画播放完毕回调
    public void EndExitAnim(UIWindowBase UIbase, UICallBack callBack, params object[] objs)
    {
        UIbase.OnCompleteExitAnim();
        UIbase.windowStatus = UIWindowBase.WindowStatus.Close;

        try
        {
            if (callBack != null)
            {
                callBack(UIbase, objs);
            }
        }
        catch(Exception e)
        {
            LogUtil.Error(e.ToString());
        }
    }
}
