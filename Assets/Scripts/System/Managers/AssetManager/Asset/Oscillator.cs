using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    private static Oscillator m_Instance;
    private WaitForSeconds m_WaitOneSecond = new WaitForSeconds(1f);
    private WaitForSeconds m_WaitThreeSecond = new WaitForSeconds(3f);

    public event Action<int> secondEvent;

    public event System.Action threeSecondEvent;

    public event Action<double> updateEvent;

    public void Dispose()
    {
        this.updateEvent = null;
        this.secondEvent = null;
        this.threeSecondEvent = null;
        base.StopAllCoroutines();
        UnityEngine.Object.Destroy(base.gameObject);
        m_Instance = null;
    }

    private void Start()
    {
        UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
        base.StartCoroutine(this.TriggerSecondEvent());
        base.StartCoroutine(this.TriggerThreeSecondEvent());
    }

    [DebuggerHidden]
    private IEnumerator TriggerSecondEvent()
    {
        return new c__Iterator62 { f__this = this };
    }

    [DebuggerHidden]
    private IEnumerator TriggerThreeSecondEvent()
    {
        return new c__Iterator63 { f__this = this };
    }

    private void Update()
    {
        if (this.updateEvent != null)
        {
            this.updateEvent(this.ServerTime.SmoothServerTime);
        }
    }

    public static Oscillator Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = UnityEngine.Object.FindObjectOfType(typeof(Oscillator)) as Oscillator;
                if (m_Instance == null)
                {
                    m_Instance = new GameObject("Oscillator").AddComponent<Oscillator>();
                }
            }
            return m_Instance;
        }
    }

    public static bool IsAvailable
    {
        get
        {
            return (m_Instance != null);
        }
    }

    public TimeUtil ServerTime
    {
        get
        {
            return TimeUtil.inst;
        }
    }


    private sealed class c__Iterator62 : IDisposable, IEnumerator, IEnumerator<object>
    {
        internal object current;
        internal int PC;
        internal Oscillator f__this;

        [DebuggerHidden]
        public void Dispose()
        {
            this.PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.PC;
            this.PC = -1;
            switch (num)
            {
                case 0:
                    break;

                case 1:
                    this.PC = -1;
                    goto Label_007B;

                default:
                    goto Label_007B;
            }
            if (this.f__this.secondEvent != null)
            {
                this.f__this.secondEvent((int) this.f__this.ServerTime.SmoothServerTime);
            }
            this.current = this.f__this.m_WaitOneSecond;
            this.PC = 1;
            return true;
        Label_007B:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.current;
            }
        }
    }

    
    private sealed class c__Iterator63 : IDisposable, IEnumerator, IEnumerator<object>
    {
        internal object current;
        internal int PC;
        internal Oscillator f__this;

        [DebuggerHidden]
        public void Dispose()
        {
            this.PC = -1;
        }

        public bool MoveNext()
        {
            uint num = (uint) this.PC;
            this.PC = -1;
            switch (num)
            {
                case 0:
                    break;

                case 1:
                    break;
                    this.PC = -1;
                    goto Label_006A;

                default:
                    goto Label_006A;
            }
            if (this.f__this.threeSecondEvent != null)
            {
                this.f__this.threeSecondEvent();
            }
            this.current = this.f__this.m_WaitThreeSecond;
            this.PC = 1;
            return true;
        Label_006A:
            return false;
        }

        [DebuggerHidden]
        public void Reset()
        {
            throw new NotSupportedException();
        }

        object IEnumerator<object>.Current
        {
            [DebuggerHidden]
            get
            {
                return this.current;
            }
        }

        object IEnumerator.Current
        {
            [DebuggerHidden]
            get
            {
                return this.current;
            }
        }
    }
}

