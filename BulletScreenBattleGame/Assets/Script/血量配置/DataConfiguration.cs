

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataConfiguration : MonoBehaviour
{
    public static DataConfiguration instance;
    public int hpMax;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject); return;
        }
        else
        {
            instance = this;
        }
    }

    public void HpPhase1()
    {
        hpMax = 9999;
        Level.instance.Level1();
    }
    public void HpPhase2()
    {
        hpMax = 29000;
        Level.instance.Level1();
    }
    public void HpPhase3()
    {
        hpMax = 99999;
        Level.instance.Level1();
    }
}
