using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReusingScrollItemBase : UIBase
{
    public int m_index = 0;
    public bool isChoose = false;
    public bool needAnim = false;

    public override void OnInit()
    {
        base.OnInit();
    }

    public virtual void OnResetInit()
    {

    }

    public virtual void OnShow()
    {

    }

    public virtual void OnHide()
    {

    }

    public virtual void SetContent(int index, Dictionary<string, object> data)
    {

    }

    public virtual void OnDrag()
    {

    }

    public virtual void OnUpdateItem(Bounds viewBounds, Bounds itemBounds)
    {
        
    }
}
