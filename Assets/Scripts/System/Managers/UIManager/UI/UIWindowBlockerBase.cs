using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UIWindowBlockerBase : UIWindowBase
{
    public static UIType UIType
    {
        get { return UIType.Blocker; }
    }

    public override void OnInit()
    {
        base.OnInit();
        isInStackHide = false;
    }
}