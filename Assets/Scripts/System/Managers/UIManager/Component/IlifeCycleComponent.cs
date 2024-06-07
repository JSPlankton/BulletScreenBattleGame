using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface UILifeCycleInterface 
{
    void Init(string UIEventKey, int id = 0, params object[] objs);

    void Dispose();
}