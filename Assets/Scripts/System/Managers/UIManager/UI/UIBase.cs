using DG.Tweening;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Profiling;
using UnityEngine.UI;

public class UIBase : MonoBehaviour, UILifeCycleInterface
{
    private const string replaceStr = "(Clone)";
    [HideInInspector]
    public Canvas m_canvas;

    #region 重载方法
    public virtual void OnBeforeInit()
    {

    }
    //当UI第一次打开时调用OnInit方法，调用时机在OnOpen之前
    public virtual void OnInit()
    {
        // FontUtil.ReplaceFont(this.gameObject);
    }

    protected virtual void OnUIDestroy()
    {
    }

    #endregion

    #region 继承方法
    private int m_UIID = -1;
    protected object[] m_InitObject = { };

    public int UIID
    {
        get { return m_UIID; }
        set { m_UIID = value; }
    }

    public object[] InitObject
    {
        get { return m_InitObject; }
        set { m_InitObject = value; }
    }

    public string UIEventKey
    {
        get { return UIName + "@" + m_UIID; }
        //set { m_UIID = value; }
    }

    string m_UIName = null;
    public string UIName
    {
        get
        {
            if (m_UIName == null)
            {
                m_UIName = name;
            }

            return m_UIName;
        }
        set
        {
            m_UIName = value;
        }
    }

    string m_assetName = string.Empty;
    public string AssetName
    {
        get
        {
            if (string.IsNullOrEmpty(m_assetName))
                m_assetName = UIName;

            if (m_assetName.IndexOf(replaceStr) >= 0)
                m_assetName = m_assetName.Replace(replaceStr, string.Empty);

            return m_assetName;
        }
        set { m_assetName = value; }
    }


    public void Init(string UIEventKey, int id, params object[] objs)
    {
        if (UIEventKey != null)
        {
            UIName = null;
            UIName = UIEventKey + "_" + UIName;
        }

        m_UIID = id;
        m_canvas = GetComponent<Canvas>();
        m_InitObject = objs;
        EnsureObjectTable();
        OnBeforeInit();
        OnInit();
    }
    public void Dispose()
    {
        //ClearGuideModel();
        RemoveAllListener();
        RemoveAllOnEventListenr();
        RemoveAllTimerEvents();
        CleanItem();
        CleanModelShowCameraList();

        ClearLoadSprite();
        ClearUiBaseList();
        try
        {
            OnUIDestroy();
        }
        catch (Exception e)
        {
            LogUtil.Exception("UIBase Dispose Exception -> UIEventKey: " + UIEventKey, e);
        }

        ClearObjectTable();
        DisposeLifeComponent();
    }

    #endregion

    #region 获取对象

    public List<GameObject> m_objectList = new List<GameObject>();

    void CreateObjectTable()
    {
        Profiler.BeginSample("CreateObjectTable");
        objectCount = m_objectList.Count;
        ClearObjectTable();
        if (m_objectNameIndex == null)
        {
            m_objectNameIndex = new Dictionary<string, int>();
        }
        m_objectNameIndex.Clear();
        m_objects = s_GameObjectPool.RentIfNull(m_objects, objectCount);
        if (this == null)
        {
            LogUtil.Error($"{UIName} Try Call UIBase Method After UI Destroy!!!");
            return;
        }
        //TODO, objectNameIndex的长度是可以事先预知的, 这里的遍历扩容存在array空间浪费.
        int j = 0;
        for (int i = 0; i < m_objectList.Count; i++)
        {
            if (m_objectList[i] != null)
            {
                if (m_objectNameIndex.ContainsKey(m_objectList[i].name))
                {
                    LogUtil.Error("CreateObjectTable ContainsKey ->" + m_objectList[i].name + "<-");
                }
                else
                {
                    m_objectNameIndex.Add(m_objectList[i].name, j);
                    m_objects[j] = m_objectList[i];
                    j++;
                }
            }
            else
            {
                LogUtil.Warning(name + " m_objectList[" + i + "] is Null !");
            }
        }
        Profiler.EndSample();
    }

    private void ClearObjectTable()
    {
        m_uiBases = s_UIBasePool.SafeReturn(m_uiBases, objectCount);
        m_objects = s_GameObjectPool.SafeReturn(m_objects, objectCount);
        m_images = s_ImagePool.SafeReturn(m_images, objectCount);
        m_Sprites = s_SpritePool.SafeReturn(m_Sprites, objectCount);
        m_texts = s_TextPool.SafeReturn(m_texts, objectCount);
        m_textmeshs = s_TextMeshPool.SafeReturn(m_textmeshs, objectCount);
        m_buttons = s_ButtonPool.SafeReturn(m_buttons, objectCount);
        m_scrollRects = s_ScrollRectPool.SafeReturn(m_scrollRects, objectCount);
        m_reusingScrollRects = s_ReusingScrollRectPool.SafeReturn(m_reusingScrollRects, objectCount);
        m_reusingPageRects = s_ReusingPageRectPool.SafeReturn(m_reusingPageRects, objectCount);
        m_reusingPageEndRects = s_ReusingPageEndRectPool.SafeReturn(m_reusingPageEndRects, objectCount);
        m_reusingPage3DRects = s_ReusingPage3DRectPool.SafeReturn(m_reusingPage3DRects, objectCount);
        m_rawImages = s_RawImagePool.SafeReturn(m_rawImages, objectCount);
        m_rectTransforms = s_RectTransformPool.SafeReturn(m_rectTransforms, objectCount);
        m_inputFields = s_InputFieldPool.SafeReturn(m_inputFields, objectCount);
        m_Sliders = s_SliderPool.SafeReturn(m_Sliders, objectCount);
        m_longPressList = s_LongPressAcceptorPool.SafeReturn(m_longPressList, objectCount);
        m_Canvas = s_CanvasPool.SafeReturn(m_Canvas, objectCount);
        m_Toggle = s_TogglePool.SafeReturn(m_Toggle, objectCount);
        m_dragList = s_DragAcceptorPool.SafeReturn(m_dragList, objectCount);

    }

    private const int MAX_OBJECT_COUNT = 128;
    private const int MAX_COUNT_BUCKET = 50;
    private static ArrayPool<UIBase> s_UIBasePool;
    private static ArrayPool<GameObject> s_GameObjectPool;
    private static ArrayPool<Image> s_ImagePool;
    private static ArrayPool<Sprite> s_SpritePool;
    private static ArrayPool<Text> s_TextPool;
    private static ArrayPool<TextMesh> s_TextMeshPool;
    private static ArrayPool<Button> s_ButtonPool;
    private static ArrayPool<ScrollRect> s_ScrollRectPool;
    private static ArrayPool<ReusingScrollRect> s_ReusingScrollRectPool;
    private static ArrayPool<ReusingPageRect> s_ReusingPageRectPool;
    private static ArrayPool<ReusingPageEndRect> s_ReusingPageEndRectPool;
    private static ArrayPool<ResuingPage3DRect> s_ReusingPage3DRectPool;
    private static ArrayPool<RawImage> s_RawImagePool;
    private static ArrayPool<RectTransform> s_RectTransformPool;
    private static ArrayPool<InputField> s_InputFieldPool;
    private static ArrayPool<Slider> s_SliderPool;
    private static ArrayPool<Canvas> s_CanvasPool;
    private static ArrayPool<Toggle> s_TogglePool;
    private static ArrayPool<LongPressAcceptor> s_LongPressAcceptorPool;
    private static ArrayPool<DragAcceptor> s_DragAcceptorPool;

    public static void InitPool()
    {
        s_UIBasePool = ArrayPool<UIBase>.Create(MAX_OBJECT_COUNT, MAX_COUNT_BUCKET);
        s_GameObjectPool = ArrayPool<GameObject>.Create(MAX_OBJECT_COUNT, MAX_COUNT_BUCKET);
        s_ImagePool = ArrayPool<Image>.Create(MAX_OBJECT_COUNT, MAX_COUNT_BUCKET);
        s_SpritePool = ArrayPool<Sprite>.Create(MAX_OBJECT_COUNT, MAX_COUNT_BUCKET);
        s_TextPool = ArrayPool<Text>.Create(MAX_OBJECT_COUNT, MAX_COUNT_BUCKET);
        s_TextMeshPool = ArrayPool<TextMesh>.Create(MAX_OBJECT_COUNT, MAX_COUNT_BUCKET);
        s_ButtonPool = ArrayPool<Button>.Create(MAX_OBJECT_COUNT, MAX_COUNT_BUCKET);
        s_ScrollRectPool = ArrayPool<ScrollRect>.Create(MAX_OBJECT_COUNT, MAX_COUNT_BUCKET);
        s_ReusingScrollRectPool = ArrayPool<ReusingScrollRect>.Create(MAX_OBJECT_COUNT, MAX_COUNT_BUCKET);
        s_ReusingPageRectPool = ArrayPool<ReusingPageRect>.Create(MAX_OBJECT_COUNT, MAX_COUNT_BUCKET);
        s_ReusingPageEndRectPool = ArrayPool<ReusingPageEndRect>.Create(MAX_OBJECT_COUNT, MAX_COUNT_BUCKET);
        s_ReusingPage3DRectPool = ArrayPool<ResuingPage3DRect>.Create(MAX_OBJECT_COUNT, MAX_COUNT_BUCKET);
        s_RawImagePool = ArrayPool<RawImage>.Create(MAX_OBJECT_COUNT, MAX_COUNT_BUCKET);
        s_RectTransformPool = ArrayPool<RectTransform>.Create(MAX_OBJECT_COUNT, MAX_COUNT_BUCKET);
        s_InputFieldPool = ArrayPool<InputField>.Create(MAX_OBJECT_COUNT, MAX_COUNT_BUCKET);
        s_SliderPool = ArrayPool<Slider>.Create(MAX_OBJECT_COUNT, MAX_COUNT_BUCKET);
        s_CanvasPool = ArrayPool<Canvas>.Create(MAX_OBJECT_COUNT, MAX_COUNT_BUCKET);
        s_TogglePool = ArrayPool<Toggle>.Create(MAX_OBJECT_COUNT, MAX_COUNT_BUCKET);
        s_LongPressAcceptorPool = ArrayPool<LongPressAcceptor>.Create(MAX_OBJECT_COUNT, MAX_COUNT_BUCKET);
        s_DragAcceptorPool = ArrayPool<DragAcceptor>.Create(MAX_OBJECT_COUNT, MAX_COUNT_BUCKET);
    }

    int objectCount = 0;
    private Dictionary<string, int> m_objectNameIndex;
    private UIBase[] m_uiBases = null;
    private GameObject[] m_objects = null;
    private Image[] m_images = null;
    private Sprite[] m_Sprites = null;
    private Text[] m_texts = null;
    private TextMesh[] m_textmeshs = null;
    private Button[] m_buttons = null;
    private ScrollRect[] m_scrollRects = null;
    private ReusingScrollRect[] m_reusingScrollRects = null;
    private ReusingPageRect[] m_reusingPageRects = null;
    private ReusingPageEndRect[] m_reusingPageEndRects = null;
    private ResuingPage3DRect[] m_reusingPage3DRects = null;
    private RawImage[] m_rawImages = null;
    private RectTransform[] m_rectTransforms = null;
    private InputField[] m_inputFields = null;
    private Slider[] m_Sliders = null;
    private Canvas[] m_Canvas = null;
    private Toggle[] m_Toggle = null;
    private LongPressAcceptor[] m_longPressList = null;
    private DragAcceptor[] m_dragList = null;

    public bool HaveObject(string name)
    {
        bool has = false;
        EnsureObjectTable();
        has = m_objectNameIndex.ContainsKey(name);
        return has;
    }

    public bool HasGameObject(string name)
    {
        if (this == null)
            return false;   //already destroy
        EnsureObjectTable();

        if (m_objectNameIndex.ContainsKey(name))
        {
            GameObject go = m_objects[m_objectNameIndex[name]];

            if (go == null)
            {
                return false;
            }

            return true;
        }
        else
        {
            return false;
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void EnsureObjectTable()
    {
        if (m_objects == null)
        {
            CreateObjectTable();
        }
    }

    public GameObject GetGameObject(string name)
    {
        EnsureObjectTable();

        if (m_objectNameIndex.ContainsKey(name))
        {
            GameObject go = m_objects[m_objectNameIndex[name]];

            if (go == null)
            {
                throw new Exception("UIWindowBase GetGameObject error: " + UIName + " m_objects[" + name + "] is null !!");
            }

            return go;
        }
        else
        {
            throw new Exception("UIWindowBase GetGameObject error: " + UIName + " dont find ->" + name + "<-");
        }
    }


    /// <summary>
    /// 注意 Rent的Array可能比预想中的大， 这个函数使用前必须确保index是当前Array的真实范围
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private GameObject GetGameObject(int index)
    {
        EnsureObjectTable();
        return m_objects[index];
    }


    public RectTransform GetRectTransform(string name)
    {
        EnsureObjectTable();
        int index;
        if (!m_objectNameIndex.TryGetValue(name, out index))
        {
            throw new Exception(this.UIName + " GetRectTransform ->" + name + "<- is Null !");
        }
        m_rectTransforms = s_RectTransformPool.RentIfNull(m_rectTransforms, objectCount);
        if (m_rectTransforms[index] != null)
        {
            return m_rectTransforms[index];
        }

        RectTransform tmp = GetGameObject(index).GetComponent<RectTransform>();


        if (tmp == null)
        {
            throw new Exception(this.UIName + " GetRectTransform ->" + name + "<- is Null !");
        }

        m_rectTransforms[index] = tmp;
        return tmp;
    }

    public UIBase GetUIBase(string name)
    {
        EnsureObjectTable();
        int index;
        if (!m_objectNameIndex.TryGetValue(name, out index))
        {
            throw new Exception(this.UIName + " GetUIBase ->" + name + "<- is Null !");
        }
        m_uiBases = s_UIBasePool.RentIfNull(m_uiBases, objectCount);
        if (m_uiBases[index] != null)
        {
            return m_uiBases[index];
        }

        UIBase tmp = GetGameObject(index).GetComponent<UIBase>();

        if (tmp == null)
        {
            throw new Exception(this.UIName + " GetUIBase ->" + name + "<- is Null !");
        }

        m_uiBases[index] = tmp;
        return tmp;
    }
    public Sprite GetSprite(string name)
    {
        EnsureObjectTable();
        int index;
        if (!m_objectNameIndex.TryGetValue(name, out index))
        {
            throw new Exception(this.UIName + " GetSprite ->" + name + "<- is Null !");
        }
        m_Sprites = s_SpritePool.RentIfNull(m_Sprites, objectCount);
        if (m_Sprites[index] != null)
        {
            return m_Sprites[index];
        }

        Sprite tmp = GetGameObject(index).GetComponent<Sprite>();

        if (tmp == null)
        {
            throw new Exception(this.UIName + " GetImage ->" + name + "<- is Null !");
        }

        m_Sprites[index] = tmp;
        return tmp;
    }
    public Image GetImage(string name)
    {
        EnsureObjectTable();
        int index;
        if (!m_objectNameIndex.TryGetValue(name, out index))
        {
            throw new Exception(this.UIName + " GetImage ->" + name + "<- is Null !");
        }
        m_images = s_ImagePool.RentIfNull(m_images, objectCount);
        if (m_images[index])
        {
            return m_images[index];
        }

        Image tmp = GetGameObject(index).GetComponent<Image>();

        if (tmp == null)
        {
            throw new Exception(this.UIName + " GetImage ->" + name + "<- is Null !");
        }

        m_images[index] = tmp;
        return tmp;
    }
    public TextMesh GetTextMesh(string name)
    {
        EnsureObjectTable();
        int index;
        if (!m_objectNameIndex.TryGetValue(name, out index))
        {
            throw new Exception(this.UIName + " GetText ->" + name + "<- is Null !");
        }
        m_textmeshs = s_TextMeshPool.RentIfNull(m_textmeshs, objectCount);
        if (m_textmeshs[index] != null)
        {
            return m_textmeshs[index];
        }

        TextMesh tmp = GetGameObject(index).GetComponent<TextMesh>();



        if (tmp == null)
        {
            throw new Exception(this.UIName + " GetText ->" + name + "<- is Null !");
        }

        m_textmeshs[index] = tmp;
        return tmp;
    }
    public Text GetText(string name)
    {
        EnsureObjectTable();
        int index;
        if (!m_objectNameIndex.TryGetValue(name, out index))
        {
            throw new Exception(this.UIName + " GetText ->" + name + "<- is Null !");
        }
        m_texts = s_TextPool.RentIfNull(m_texts, objectCount);
        if (m_texts[index] != null)
        {
            return m_texts[index];
        }

        Text tmp = GetGameObject(index).GetComponent<Text>();

        if (tmp == null)
        {
            throw new Exception(this.UIName + " GetText ->" + name + "<- is Null !");
        }

        m_texts[index] = tmp;
        return tmp;
    }
    public Toggle GetToggle(string name)
    {
        EnsureObjectTable();
        int index;
        if (!m_objectNameIndex.TryGetValue(name, out index))
        {
            throw new Exception(this.UIName + " GetToggle ->" + name + "<- is Null !");
        }
        m_Toggle = s_TogglePool.RentIfNull(m_Toggle, objectCount);
        if (m_Toggle[index])
        {
            return m_Toggle[index];
        }

        Toggle tmp = GetGameObject(index).GetComponent<Toggle>();

        if (tmp == null)
        {
            throw new Exception(this.UIName + " GetText ->" + name + "<- is Null !");
        }

        m_Toggle[index] = tmp;
        return tmp;
    }

    public Button GetButton(string name)
    {
        EnsureObjectTable();
        int index;
        if (!m_objectNameIndex.TryGetValue(name, out index))
        {
            throw new Exception(this.UIName + " GetToggle ->" + name + "<- is Null !");
        }
        m_buttons = s_ButtonPool.RentIfNull(m_buttons, objectCount);
        if (m_buttons[index] != null)
        {
            return m_buttons[index];
        }

        Button tmp = GetGameObject(index).GetComponent<Button>();

        if (tmp == null)
        {
            throw new Exception(this.UIName + " GetButton ->" + name + "<- is Null !");
        }

        m_buttons[index] = tmp;
        return tmp;
    }

    public InputField GetInputField(string name)
    {
        EnsureObjectTable();
        int index;
        if (!m_objectNameIndex.TryGetValue(name, out index))
        {
            throw new Exception(this.UIName + " GetInputField ->" + name + "<- is Null !");
        }
        m_inputFields = s_InputFieldPool.RentIfNull(m_inputFields, objectCount);
        if (m_inputFields[index])
        {
            return m_inputFields[index];
        }

        InputField tmp = GetGameObject(index).GetComponent<InputField>();

        if (tmp == null)
        {
            throw new Exception(this.UIName + " GetInputField ->" + name + "<- is Null !");
        }

        m_inputFields[index] = tmp;
        return tmp;
    }

    public ScrollRect GetScrollRect(string name)
    {
        EnsureObjectTable();
        int index;
        if (!m_objectNameIndex.TryGetValue(name, out index))
        {
            throw new Exception(this.UIName + " GetScrollRect ->" + name + "<- is Null !");
        }
        m_scrollRects = s_ScrollRectPool.RentIfNull(m_scrollRects, objectCount);
        if (m_scrollRects[index] != null)
        {
            return m_scrollRects[index];
        }

        ScrollRect tmp = GetGameObject(index).GetComponent<ScrollRect>();

        if (tmp == null)
        {
            throw new Exception(this.UIName + " GetScrollRect ->" + name + "<- is Null !");
        }

        m_scrollRects[index] = tmp;
        return tmp;
    }

    public RawImage GetRawImage(string name)
    {
        EnsureObjectTable();
        int index;
        if (!m_objectNameIndex.TryGetValue(name, out index))
        {
            throw new Exception(this.UIName + " GetRawImage ->" + name + "<- is Null !");
        }
        m_rawImages = s_RawImagePool.RentIfNull(m_rawImages, objectCount);
        if (m_rawImages[index] != null)
        {
            return m_rawImages[index];
        }

        RawImage tmp = GetGameObject(index).GetComponent<RawImage>();

        if (tmp == null)
        {
            throw new Exception(this.UIName + " GetRawImage ->" + name + "<- is Null !");
        }

        m_rawImages[index] = tmp;
        return tmp;
    }

    public Slider GetSlider(string name)
    {
        EnsureObjectTable();
        int index;
        if (!m_objectNameIndex.TryGetValue(name, out index))
        {
            throw new Exception(this.UIName + " GetSlider ->" + name + "<- is Null !");
        }
        m_Sliders = s_SliderPool.RentIfNull(m_Sliders, objectCount);
        if (m_Sliders[index] != null)
        {
            return m_Sliders[index];
        }

        Slider tmp = GetGameObject(index).GetComponent<Slider>();

        if (tmp == null)
        {
            throw new Exception(this.UIName + " GetSlider ->" + name + "<- is Null !");
        }

        m_Sliders[index] = tmp;
        return tmp;
    }

    public Canvas GetCanvas(string name)
    {
        EnsureObjectTable();
        int index;
        if (!m_objectNameIndex.TryGetValue(name, out index))
        {
            throw new Exception(this.UIName + " GetCanvas ->" + name + "<- is Null !");
        }
        m_Canvas = s_CanvasPool.RentIfNull(m_Canvas, objectCount);
        if (m_Canvas[index] != null)
        {
            return m_Canvas[index];
        }

        Canvas tmp = GetGameObject(index).GetComponent<Canvas>();

        if (tmp == null)
        {
            throw new Exception(this.UIName + " GetCanvas ->" + name + "<- is Null !");
        }

        m_Canvas[index] = tmp;
        return tmp;
    }

    public Vector3 GetPosition(string name, bool islocal)
    {
        Vector3 tmp = Vector3.zero;
        GameObject go = GetGameObject(name);
        if (go != null)
        {
            if (islocal)
                tmp = GetGameObject(name).transform.localPosition;
            else
                tmp = GetGameObject(name).transform.position;
        }
        return tmp;
    }

    private RectTransform m_rectTransform;
    public RectTransform RectTransform
    {
        get
        {
            if (m_rectTransform == null)
            {
                m_rectTransform = GetComponent<RectTransform>();
            }

            return m_rectTransform;
        }
        set { m_rectTransform = value; }
    }

    public void SetSizeDelta(float w, float h)
    {
        RectTransform.sizeDelta = new Vector2(w, h);
    }
    #region 自定义组件

    public ReusingScrollRect GetReusingScrollRect(string name)
    {
        EnsureObjectTable();
        int index;
        if (!m_objectNameIndex.TryGetValue(name, out index))
        {
            throw new Exception(this.UIName + " GetReusingScrollRect ->" + name + "<- is Null !");
        }
        m_reusingScrollRects = s_ReusingScrollRectPool.RentIfNull(m_reusingScrollRects, objectCount);
        if (m_reusingScrollRects[index])
        {
            return m_reusingScrollRects[index];
        }

        ReusingScrollRect tmp = GetGameObject(name).GetComponent<ReusingScrollRect>();

        if (tmp == null)
        {
            throw new Exception(this.UIName + " GetReusingScrollRect ->" + name + "<- is Null !");
        }

        m_reusingScrollRects[index] = tmp;
        return tmp;
    }

    public ReusingPageRect GetReusingPageRect(string name)
    {
        EnsureObjectTable();
        int index;
        if (!m_objectNameIndex.TryGetValue(name, out index))
        {
            throw new Exception(this.UIName + " GetReusingPageRect ->" + name + "<- is Null !");
        }
        m_reusingPageRects = s_ReusingPageRectPool.RentIfNull(m_reusingPageRects, objectCount);
        if (m_reusingPageRects[index])
        {
            return m_reusingPageRects[index];
        }

        ReusingPageRect tmp = GetGameObject(name).GetComponent<ReusingPageRect>();

        if (tmp == null)
        {
            throw new Exception(this.UIName + " GetReusingPageRect ->" + name + "<- is Null !");
        }

        m_reusingPageRects[index] = tmp;
        return tmp;
    }
    public ReusingPageEndRect GetReusingPageEndRect(string name)
    {
        EnsureObjectTable();
        int index;
        if (!m_objectNameIndex.TryGetValue(name, out index))
        {
            throw new Exception(this.UIName + " GetReusingPageEndRect ->" + name + "<- is Null !");
        }
        m_reusingPageEndRects = s_ReusingPageEndRectPool.RentIfNull(m_reusingPageEndRects, objectCount);
        if (m_reusingPageEndRects[index] != null)
        {
            return m_reusingPageEndRects[index];
        }

        ReusingPageEndRect tmp = GetGameObject(name).GetComponent<ReusingPageEndRect>();

        if (tmp == null)
        {
            throw new Exception(this.UIName + " GetReusingPageEndRect ->" + name + "<- is Null !");
        }

        m_reusingPageEndRects[index] = tmp;
        return tmp;
    }

    public ResuingPage3DRect GetReusingPage3DRect(string name)
    {
        EnsureObjectTable();
        int index;
        if (!m_objectNameIndex.TryGetValue(name, out index))
        {
            throw new Exception(this.UIName + " GetReusingPage3DRect ->" + name + "<- is Null !");
        }
        m_reusingPage3DRects = s_ReusingPage3DRectPool.RentIfNull(m_reusingPage3DRects, objectCount);
        if (m_reusingPage3DRects[index])
        {
            return m_reusingPage3DRects[index];
        }

        ResuingPage3DRect tmp = GetGameObject(name).GetComponent<ResuingPage3DRect>();

        if (tmp == null)
        {
            throw new Exception(this.UIName + " GetReusingPage3DRect ->" + name + "<- is Null !");
        }

        m_reusingPage3DRects[index] = tmp;
        return tmp;
    }

    public LongPressAcceptor GetLongPressComp(string name)
    {
        EnsureObjectTable();
        int index;
        if (!m_objectNameIndex.TryGetValue(name, out index))
        {
            throw new Exception(this.UIName + " GetLongPressComp ->" + name + "<- is Null !");
        }
        m_longPressList = s_LongPressAcceptorPool.RentIfNull(m_longPressList, objectCount);
        if (m_longPressList[index])
        {
            return m_longPressList[index];
        }

        LongPressAcceptor tmp = GetGameObject(name).GetComponent<LongPressAcceptor>();

        if (tmp == null)
        {
            throw new Exception(this.UIName + " GetLongPressComp ->" + name + "<- is Null !");
        }

        m_longPressList[index] = tmp;
        return tmp;
    }

    public DragAcceptor GetDragComp(string name)
    {
        EnsureObjectTable();
        int index;
        if (!m_objectNameIndex.TryGetValue(name, out index))
        {
            throw new Exception(this.UIName + " GetDragComp ->" + name + "<- is Null !");
        }
        m_dragList = s_DragAcceptorPool.RentIfNull(m_dragList, objectCount);
        if (m_dragList[index] != null)
        {
            return m_dragList[index];
        }

        DragAcceptor tmp = GetGameObject(name).GetComponent<DragAcceptor>();

        if (tmp == null)
        {
            throw new Exception(this.UIName + " GetDragComp ->" + name + "<- is Null !");
        }

        m_dragList[index] = tmp;
        return tmp;
    }

    #endregion

    #endregion

    #region 注册监听

    //protected List<Enum> m_EventNames = new List<Enum>(); GlobalEvent使用
    protected List<String> m_EventNames = new List<String>();

    protected List<InputEventRegisterInfo> m_OnClickEvents = new List<InputEventRegisterInfo>();
    protected List<InputEventRegisterInfo> m_LongPressEvents = new List<InputEventRegisterInfo>();
    protected List<InputEventRegisterInfo> m_DragEvents = new List<InputEventRegisterInfo>();
    protected List<InputEventRegisterInfo> m_BeginDragEvents = new List<InputEventRegisterInfo>();
    protected List<InputEventRegisterInfo> m_EndDragEvents = new List<InputEventRegisterInfo>();


    protected List<EventData> m_UIEventDatas = new List<EventData>();
    protected List<EventData> m_UITmpEventDatas = new List<EventData>();
    protected List<TimerEvent> m_UITimerEvents = new List<TimerEvent>();

    public void CopyUIEvent()
    {
        m_UITmpEventDatas.Clear();
        m_UITmpEventDatas = new List<EventData>(m_UIEventDatas);
    }

    public virtual void RemoveAllListener()
    {

        for (int i = 0; i < m_OnClickEvents.Count; i++)
        {
            m_OnClickEvents[i].RemoveListener();
        }
        m_OnClickEvents.Clear();

        for (int i = 0; i < m_LongPressEvents.Count; i++)
        {
            m_LongPressEvents[i].RemoveListener();
        }
        m_LongPressEvents.Clear();

        #region 拖动事件
        for (int i = 0; i < m_DragEvents.Count; i++)
        {
            m_DragEvents[i].RemoveListener();
        }
        m_DragEvents.Clear();

        for (int i = 0; i < m_BeginDragEvents.Count; i++)
        {
            m_BeginDragEvents[i].RemoveListener();
        }
        m_BeginDragEvents.Clear();

        for (int i = 0; i < m_EndDragEvents.Count; i++)
        {
            m_EndDragEvents[i].RemoveListener();
        }
        m_EndDragEvents.Clear();
        #endregion
    }

    #region 添加监听

    bool GetRegister(List<InputEventRegisterInfo> list, string eventKey)
    {
        int registerCount = 0;
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i].eventKey == eventKey)
            {
                registerCount++;
            }
        }

        return registerCount == 0;
    }

    public TimerEvent AddCallBackTimer(float spaceTime, bool isIgnoreTimeScale, TimerCallBack callBack, params object[] objs)
    {
        TimerEvent timerEvent = Managers.TimerManager.CallBackOfIntervalTimer(this, spaceTime, isIgnoreTimeScale, null, callBack, objs);
        this.m_UITimerEvents.Add(timerEvent);
        return timerEvent;
    }

    public TimerEvent AddCallBackTimer(float spaceTime, TimerCallBack callBack, params object[] objs)
    {
        return AddCallBackTimer(spaceTime, true, callBack, objs);
    }

    public TimerEvent AddDelayCallBack(float delayTime, bool isIgnoreTimeScale, TimerCallBack callBack, params object[] objs)
    {
        TimerEvent timerEvent = Managers.TimerManager.DelayCallBack(this, delayTime, isIgnoreTimeScale, callBack, objs);
        this.m_UITimerEvents.Add(timerEvent);
        return timerEvent;
    }
    public TimerEvent AddDelayCallBack(float delayTime, TimerCallBack callBack, params object[] objs)
    {
        return AddDelayCallBack(delayTime, true, callBack, objs);
    }

    public void RemoveTimer(TimerEvent timerEvent)
    {
        if (this.m_UITimerEvents.Contains(timerEvent))
        {
            DelteTimer(timerEvent);
        }
    }

    private void DelteTimer(TimerEvent timerEvent)
    {
        Managers.TimerManager.RemoveTimer(timerEvent);
        this.m_UITimerEvents.Remove(timerEvent);
        timerEvent = null;
    }

    public void RemoveAllTimerEvents()
    {
        for (int i = 0; i < this.m_UITimerEvents.Count; i++)
        {
            TimerEvent timerEvent = this.m_UITimerEvents[i];
            DelteTimer(timerEvent);
        }
        this.m_UITimerEvents.Clear();
    }

    public void AddOnEventListenr(object source, string eventName, EventHandle handle, bool isUseOnce = false)
    {
        //当前UI添加事件监听
        EventManager.AddEvent(source, eventName, handle, isUseOnce);
        m_UIEventDatas.Add(EventData.GetOnUIEvent(source, eventName, handle, isUseOnce));
    }

    public void DispatchOnEventListenr(object source, string eventName, EventHandle handle, bool isUseOnce = false)
    {
        //当前UI派发监听事件
        EventManager.DispatchEvent(source, eventName, handle, isUseOnce);
    }

    public void RemoveAllOnEventListenr()
    {
        //当前UI清除所有监听事件
        for (int i = 0; i < m_UIEventDatas.Count; i++)
        {
            EventData uIEventData = (EventData)m_UIEventDatas[i];
            EventManager.RemoveEvent(uIEventData.source, uIEventData.eventName, uIEventData.handle);
        }

        m_UIEventDatas.Clear();
        //LogUtil.Log("UI {0} Hide/Destroy, 移除所有监听, Tmp数量:{1}, Event数量:{2}", gameObject.name, m_UITmpEventDatas.Count, m_UIEventDatas.Count);
    }

    public void ResumeAllOnEventListenr()
    {
        if (m_UITmpEventDatas.Count <= 0)
        {
            return;
        }
        //当前UI清除所有监听事件
        for (int i = 0; i < m_UITmpEventDatas.Count; i++)
        {
            EventData uIEventData = (EventData)m_UITmpEventDatas[i];
            EventManager.AddEvent(uIEventData.source, uIEventData.eventName, uIEventData.handle, uIEventData.isUseOnce);
            m_UIEventDatas.Add(EventData.GetOnUIEvent(uIEventData.source, uIEventData.eventName, uIEventData.handle, uIEventData.isUseOnce));
        }

        m_UITmpEventDatas.Clear();
        //LogUtil.Log("UI {0} Show, 恢复所有监听, Tmp数量:{1}, Event数量:{2}", gameObject.name, m_UITmpEventDatas.Count, m_UIEventDatas.Count);
    }

    public void AddButtonClickListener(string buttonName, UnityAction callback, string parm = null)
    {
        //先删除
        GetButton(buttonName).onClick.RemoveAllListeners();
        GetButton(buttonName).onClick.AddListener(callback);
    }


    public void AddOnClickListener(string buttonName, InputEventHandle<InputUIOnClickEvent> callback, string parm = null)
    {
        InputButtonClickRegisterInfo info = InputUIEventProxy.GetOnClickListener(GetButton(buttonName), UIEventKey, buttonName, parm, callback);
        info.AddListener();
        m_OnClickEvents.Add(info);
    }

    public void AddOnClickListenerByCreate(Button button, string compName, InputEventHandle<InputUIOnClickEvent> callback, string parm = null)
    {
        InputButtonClickRegisterInfo info = InputUIEventProxy.GetOnClickListener(button, UIEventKey, compName, parm, callback);
        info.AddListener();
        m_OnClickEvents.Add(info);
    }

    public void AddLongPressListener(string compName, InputEventHandle<InputUILongPressEvent> callback, string parm = null)
    {
        InputEventRegisterInfo<InputUILongPressEvent> info = InputUIEventProxy.GetLongPressListener(GetLongPressComp(compName), UIEventKey, compName, parm, callback);
        info.AddListener();
        m_LongPressEvents.Add(info);
    }

    public void AddBeginDragListener(string compName, InputEventHandle<InputUIOnBeginDragEvent> callback, string parm = null)
    {
        InputEventRegisterInfo<InputUIOnBeginDragEvent> info = InputUIEventProxy.GetOnBeginDragListener(GetDragComp(compName), UIEventKey, compName, parm, callback);
        info.AddListener();
        m_BeginDragEvents.Add(info);
    }

    public void AddEndDragListener(string compName, InputEventHandle<InputUIOnEndDragEvent> callback, string parm = null)
    {
        InputEventRegisterInfo<InputUIOnEndDragEvent> info = InputUIEventProxy.GetOnEndDragListener(GetDragComp(compName), UIEventKey, compName, parm, callback);
        info.AddListener();
        m_EndDragEvents.Add(info);
    }

    public void AddOnDragListener(string compName, InputEventHandle<InputUIOnDragEvent> callback, string parm = null)
    {
        InputEventRegisterInfo<InputUIOnDragEvent> info = InputUIEventProxy.GetOnDragListener(GetDragComp(compName), UIEventKey, compName, parm, callback);
        info.AddListener();
        m_DragEvents.Add(info);
    }

    #endregion

    #region 移除监听

    //TODO 逐步添加所有的移除监听方法

    public InputButtonClickRegisterInfo GetClickRegisterInfo(string buttonName, InputEventHandle<InputUIOnClickEvent> callback, string parm)
    {
        string eventKey = InputUIOnClickEvent.GetEventKey(UIEventKey, buttonName, parm);
        for (int i = 0; i < m_OnClickEvents.Count; i++)
        {
            InputButtonClickRegisterInfo info = (InputButtonClickRegisterInfo)m_OnClickEvents[i];
            if (info.eventKey == eventKey
                && info.callBack == callback)
            {
                return info;
            }
        }

        throw new Exception("GetClickRegisterInfo Exception not find RegisterInfo by " + buttonName + " parm " + parm);
    }

    public void RemoveOnClickListener(string buttonName, InputEventHandle<InputUIOnClickEvent> callback, string parm = null)
    {
        InputButtonClickRegisterInfo info = GetClickRegisterInfo(buttonName, callback, parm);
        m_OnClickEvents.Remove(info);
        info.RemoveListener();
    }

    public void RemoveLongPressListener(string compName, InputEventHandle<InputUILongPressEvent> callback, string parm = null)
    {
        InputEventRegisterInfo<InputUILongPressEvent> info = GetLongPressRegisterInfo(compName, callback, parm);
        m_LongPressEvents.Remove(info);
        info.RemoveListener();
    }

    public InputEventRegisterInfo<InputUILongPressEvent> GetLongPressRegisterInfo(string compName, InputEventHandle<InputUILongPressEvent> callback, string parm)
    {
        string eventKey = InputUILongPressEvent.GetEventKey(UIName, compName, parm);
        for (int i = 0; i < m_LongPressEvents.Count; i++)
        {
            InputEventRegisterInfo<InputUILongPressEvent> info = (InputEventRegisterInfo<InputUILongPressEvent>)m_LongPressEvents[i];
            if (info.eventKey == eventKey
                && info.callBack == callback)
            {
                return info;
            }
        }

        throw new Exception("GetLongPressRegisterInfo Exception not find RegisterInfo by " + compName + " parm " + parm);
    }

    #endregion

    #endregion

    #region 创建对象<等待对象池模块/资源管理模块...>

    protected List<UIBase> m_ChildList = new List<UIBase>();
    int m_childUIIndex = 0;
    public UIBase CreateItem(string itemName, GameObject parent, bool isActive)
    {
        Transform transform = parent == null ? null : parent.transform;
        string uiPath = UIPath.GetPath(itemName);
        UnityEngine.Object obj = AssetManager.Instance.Load(uiPath, typeof(GameObject));
        GameObject item = UnityEngine.Object.Instantiate<GameObject>((GameObject)obj, transform);

        item.SetActive(true);
        if (parent == null)
        {
            item.transform.SetParent(null);
        }
        else
        {
            item.transform.SetParent(parent.transform);
        }
        return SetItem(item);
    }

    public UIBase CreateItem(string itemName, string prantName, bool isActive)
    {
        GameObject parent = GetGameObject(prantName);
        return CreateItem(itemName, parent, isActive);
    }
    public UIBase CreateItem(GameObject itemObj, GameObject parent, bool isActive)
    {
        //GameObject item = GameObjectManager.CreateGameObjectByPool(itemObj, parent, isActive);
        Transform transform = parent == null ? null : parent.transform;
        GameObject item = GameObject.Instantiate(itemObj, transform);
        item.SetActive(true);
        item.name = itemObj.name;
        return SetItem(item);
    }
    public UIBase CreateItem(string itemName, string prantName)
    {
        return CreateItem(itemName, prantName, true);
    }
    private UIBase SetItem(GameObject item)
    {
        item.transform.localScale = Vector3.one;
        item.transform.localPosition = Vector3.zero;
        UIBase UIItem = item.GetComponent<UIBase>();

        if (UIItem == null)
        {
            throw new Exception("CreateItem Error : ->" + item.name + "<- don't have UIBase Component!");
        }

        UIItem.Init(UIEventKey, m_childUIIndex++);
        UIItem.UIName = UIEventKey + "_" + UIItem.UIName;

        m_ChildList.Add(UIItem);

        return UIItem;
    }
    public void DestroyItem(UIBase item)
    {
        DestroyItem(item, true);
    }

    public void DestroyItem(UIBase item, bool isActive)
    {
        if (m_ChildList.Contains(item))
        {
            m_ChildList.Remove(item);
            item.Dispose();
            //GameObjectManager.DestroyGameObjectByPool(item.gameObject, isActive);
            Vector3 s_OutOfRange = new Vector3(9000, 9000, 9000);
            GameObject go = item.gameObject;
            if (!isActive)
                go.SetActive(false);
            else
            {
                go.transform.position = s_OutOfRange;
            }
        }
    }

    public void DestroyItem(UIBase item, float t)
    {
        if (m_ChildList.Contains(item))
        {
            m_ChildList.Remove(item);
            item.Dispose();
            //GameObjectManager.DestroyGameObjectByPool(item.gameObject, t);

        }
    }

    public void CleanItem()
    {
        CleanItem(true);
    }

    public void CleanItem(bool isActive)
    {
        for (int i = 0; i < m_ChildList.Count; i++)
        {
            try
            {
                m_ChildList[i].Dispose();
                //GameObjectManager.DestroyGameObjectByPool(m_ChildList[i].gameObject, isActive);
                Vector3 s_OutOfRange = new Vector3(9000, 9000, 9000);
                GameObject go = m_ChildList[i].gameObject;
                if (!isActive)
                    go.SetActive(false);
                else
                {
                    go.transform.position = s_OutOfRange;
                }
            }
            catch (Exception e)
            {
                LogUtil.Exception("CleanItem Error! UIName " + UIName, e);
            }
        }

        m_ChildList.Clear();
        m_childUIIndex = 0;
    }


    public UIBase GetItemByIndex(string itemName, int index)
    {
        for (int i = 0; i < m_ChildList.Count; i++)
        {
            if (m_ChildList[i].name == itemName)
            {
                //LogUtil.Log("GetItemByIndex " + index, m_ChildList[i]);

                index--;
                if (index == 0)
                {
                    return m_ChildList[i];
                }
            }
        }

        throw new Exception(UIName + " GetItem Exception Dont find Item: " + itemName);
    }

    public UIBase GetItemByKey(string uiEvenyKey)
    {
        for (int i = 0; i < m_ChildList.Count; i++)
        {
            if (m_ChildList[i].UIEventKey == uiEvenyKey)
            {
                return m_ChildList[i];
            }
        }

        throw new Exception(UIName + " GetItemByKey Exception Dont find Item: " + uiEvenyKey);
    }

    public bool GetItemIsExist(string itemName)
    {
        for (int i = 0; i < m_ChildList.Count; i++)
        {
            if (m_ChildList[i].name == itemName)
            {
                return true;
            }
        }

        return false;
    }

    #endregion

    #region 赋值方法

    public void SetText(string TextID, string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            GetText(TextID).text = content;
        }
        else
        {
            GetText(TextID).text = content.Replace("\\n", "\n");
        }
    }

    public void SetImageColor(string ImageID, Color color)
    {
        GetImage(ImageID).color = color;
    }

    public void SetImageColor(string ImageID, string colorHex, float alpha = 1f)
    {

        if (!string.IsNullOrEmpty(colorHex))
        {
            Color newColor;
            ColorUtility.TryParseHtmlString(colorHex, out newColor);
            newColor.a = alpha;
            GetImage(ImageID).color = newColor;
        }
    }

    public void SetImageFillAmount(string ImageID, float value)
    {
        GetImage(ImageID).fillAmount = value;
    }

    public void SetTextColor(string TextID, Color color)
    {
        GetText(TextID).color = color;
    }

    public void SetTextColor(string TextID, string colorHex)
    {
        SetTextColor(GetText(TextID), colorHex);
    }

    public void SetTextColor(Text text, string colorHex)
    {
        if (!string.IsNullOrEmpty(colorHex))
        {
            Color newColor = text.color;
            ColorUtility.TryParseHtmlString(colorHex, out newColor);
            text.color = newColor;
        }
    }

    public void SetTextGray(string TextID, bool isGray)
    {
        Text text = GetText(TextID);
        SetTextGray(text, isGray);
    }

    public void SetTextGray(Text text, bool isGray)
    {
        if (!isGray)
        {
            text.material = AssetManager.Instance.Load("Materials/Normal_Mat", typeof(Material)) as Material;
        }
        else
        {
            text.material = AssetManager.Instance.Load("Materials/TurnGray_Mat", typeof(Material)) as Material;
        }
    }

    public void SetImageAlpha(string ImageID, float alpha)
    {
        Color col = GetImage(ImageID).color;
        col.a = alpha;
        GetImage(ImageID).color = col;
    }

    public void SetImageGray(string ImageID, bool isGray)
    {
        Image img = GetImage(ImageID);
        SetImageGray(img, isGray);
    }

    public void SetImageGray(Image img, bool isGray)
    {
        if (!isGray)
        {
            img.material = AssetManager.Instance.Load("Materials/Normal_Mat", typeof(Material)) as Material;
        }
        else
        {
            img.material = AssetManager.Instance.Load("Materials/TurnGray_Mat", typeof(Material)) as Material;
        }
    }

    public void SetInputText(string TextID, string content)
    {
        GetInputField(TextID).text = content;
    }

    void SetTextStyle(string textID, string work)
    {

    }

    public void SetSlider(string sliderID, float value)
    {
        GetSlider(sliderID).value = value;
    }

    /// <summary>
    /// 优化的设置SetActive方法，可以节约重复设置Active的开销
    /// </summary>
    /// <param name="gameObjectID"></param>
    /// <param name="isShow"></param>
    public void SetActive(string gameObjectID, bool isShow)
    {
        var go = GetGameObject(gameObjectID);

        if (go == null) return;

        if (isShow && !go.activeSelf)
        {
            go.SetActive(true);
        }
        else if (!isShow && go.activeSelf)
        {
            go.SetActive(false);
        }
    }
    /// <summary>
    /// Only Button
    /// </summary>
    public void SetEnabeled(string ID, bool enable, string enableName = "", string unenableName = "")
    {
        Button button = GetButton(ID);
        if (null != button)
        {
            button.enabled = enable;
            if (!string.IsNullOrEmpty(enableName) && !string.IsNullOrEmpty(unenableName))
            {
                string picName = button.enabled ? enableName : unenableName;
                Sprite sprite = AssetManager.Instance.LoadAsset<Sprite>("", picName, ".png");
                if (null != sprite)
                {
                    button.GetComponent<Image>().sprite = sprite;
                }
            }
        }
    }

    public void SetTempUnEnabeled(string ID, float delayTime = 0.5f, string enableName = "", string unenableName = "")
    {
        SetEnabeled(ID, false, enableName, unenableName);
        Sequence quenceDispose = DOTween.Sequence();
        quenceDispose.SetDelay(delayTime).OnComplete(() =>
        {
            SetEnabeled(ID, true, enableName, unenableName);
        });
    }
    /// <summary>
    /// 设置控件高度
    /// </summary>
    /// <param name="contrlId"></param>
    /// <param name="height"></param>
    protected void SetRectHeight(string contrlId, float height)
    {
        RectTransform transform = GetRectTransform(contrlId);
        if (null == transform)
        {
            return;
        }
        transform.sizeDelta = new Vector2(transform.sizeDelta.x, height);
    }

    protected void SetRectWidth(string contrlId, float width)
    {
        RectTransform transform = GetRectTransform(contrlId);
        if (null == transform)
        {
            return;
        }
        transform.sizeDelta = new Vector2(width, transform.sizeDelta.y);
    }

    /// <summary>
    /// 增加控件的高度
    /// </summary>
    /// <param name="contrlId"></param>
    /// <param name="height"></param>
    protected void AddRectHeight(string contrlId, float height)
    {
        RectTransform transform = GetRectTransform(contrlId);
        if (null == transform)
        {
            return;
        }
        transform.sizeDelta = new Vector2(transform.sizeDelta.x, transform.sizeDelta.y + height);
    }

    /// <summary>
    /// 是否能点击
    /// </summary>
    /// <param name="buttonName"></param>
    /// <returns></returns>
    protected bool IsCanClickButton(string buttonName)
    {
        if (!HaveObject(buttonName))
        {
            return false;
        }
        if (!GetButton(buttonName).enabled)
        {
            return false;
        }
        SetEnabeled(buttonName, false);
        return true;
    }

    /// <summary>
    /// 设置按钮可点击
    /// </summary>
    /// <param name="buttonName"></param>
    protected void SetButtonClick(string buttonName)
    {
        if (!HaveObject(buttonName))
        {
            return;
        }
        SetEnabeled(buttonName, true);
    }

    /// <summary>
    /// Only Button
    /// </summary>
    public void SetButtonInteractable(string ID, bool enable)
    {
        GetButton(ID).interactable = enable;
    }

    public void SetRectWidth(string TextID, float value, float height)
    {
        GetRectTransform(TextID).sizeDelta = Vector2.right * -value * 2 + Vector2.up * height;
    }

    public void SetWidth(string TextID, float width, float height)
    {
        GetRectTransform(TextID).sizeDelta = Vector2.right * width + Vector2.up * height;
    }

    public void SetPosition(string TextID, float x, float y, float z, bool islocal)
    {
        if (islocal)
            GetRectTransform(TextID).localPosition = Vector3.right * x + Vector3.up * y + Vector3.forward * z;
        else
            GetRectTransform(TextID).position = Vector3.right * x + Vector3.up * y + Vector3.forward * z;

    }

    public void SetAnchoredPosition(string ID, float x, float y)
    {
        GetRectTransform(ID).anchoredPosition = Vector2.right * x + Vector2.up * y;
    }

    public void SetAnchoredPosition(string ID, Vector2 pos)
    {
        SetAnchoredPosition(ID, pos.x, pos.y);
    }

    public void SetScale(string TextID, float x, float y, float z)
    {
        GetGameObject(TextID).transform.localScale = Vector3.right * x + Vector3.up * y + Vector3.forward * z;
    }

    public void SetScale(string TextID, Vector3 scaleVec3)
    {
        GetGameObject(TextID).transform.localScale = Vector3.right * scaleVec3.x + Vector3.up * scaleVec3.y + Vector3.forward * scaleVec3.z;
    }

    public void SetMeshText(string TextID, string txt)
    {
        GetTextMesh(TextID).text = txt;
    }

    #endregion

    #region 动态加载Sprite赋值
    private Dictionary<string, int> loadSpriteNames = new Dictionary<string, int>();
    public void SetImageSprite(string imgName, string abName, string name, bool is_nativesize = false)
    {
        Image img = GetImage(imgName);
        SetImageSprite(img, abName, name, is_nativesize);
    }

    public void SetImageSprite(Image img, string abName, string name, bool is_nativesize = false)
    {
        if (null == img)
        {
            return;
        }
        Sprite assetData = AssetManager.Instance.LoadAsset<Sprite>(abName, name, ".png");
        if (assetData != null)
        {
            img.overrideSprite = assetData;
            img.sprite = img.overrideSprite;
        }
        else
        {
            img.overrideSprite = null;
            img.sprite = null;
        }

        if (is_nativesize)
            img.SetNativeSize();
    }

    public void SetImageSprite(string imgName, string pathName, string name, string defName, bool isNativeSize = false)
    {
        Image image = GetImage(imgName);
        if (null == image)
        {
            LogUtil.Warning("error--{0}--SetImageSprite--image=null.", this.ToString());
            return;
        }
        Sprite sprite = AssetManager.Instance.LoadAsset<Sprite>(pathName, name, ".png");
        if (null == sprite && !string.IsNullOrEmpty(defName))
        {
            sprite = AssetManager.Instance.LoadAsset<Sprite>(pathName, defName, ".png");
        }
        if (null == sprite)
        {
            LogUtil.Warning("error--{0}--SetImageSprite--name={1}--defName={2}--.", this.ToString(), name, defName);
            return;
        }
        image.overrideSprite = sprite;
        image.sprite = image.overrideSprite;
        if (isNativeSize)
        {
            image.SetNativeSize();
        }
    }

    private void ClearLoadSprite()
    {
        ////LogUtil.Log("===>> ClearLoadSprite");
        //foreach (var item in loadSpriteNames)
        //{
        //    int num = item.Value;
        //    //if (item.Key == "CLT_border_TagBg_Hunter")
        //    //    LogUtil.Log("UIBase 回收图片：" + item.Key + ":" + num);
        //    ResourceManager.DestoryAssetsCounter(item.Key, num);


        //}
        //loadSpriteNames.Clear();
    }
    #endregion

    #region RawImageCamera

    List<UIModelShowTool.UIModelShowData> modelList = new List<UIModelShowTool.UIModelShowData>();

    public UIModelShowTool.UIModelShowData SetRawImageByModelShowCamera(string rawimageName, string modelName,
        string layerName = null,
        bool? orthographic = null,
        float? orthographicSize = null,
        Color? backgroundColor = null,
        Vector3? localPosition = null,
        Vector3? localScale = null,
        Vector3? eulerAngles = null,
        Vector3? texSize = null,
        float? nearClippingPlane = null,
        float? farClippingPlane = null,
        Vector3? cameraLocalPosition = null,
        Vector3? cameraEulerAngles = null,
        float? orthograthicSize = null,
        bool? camAntialiasing = null,
        bool? camPostProcessing = null,
        CallBack<UIModelShowTool.UIModelShowData> loadfinishFunc = null,
        bool? castShadow = null,
        bool? isAsyncLoad = null)
    {
        var model = CreateModelShow(modelName, layerName, orthographic, orthographicSize, camAntialiasing, camPostProcessing, backgroundColor, localPosition, localScale, eulerAngles, texSize, nearClippingPlane, farClippingPlane, cameraLocalPosition, cameraEulerAngles, orthograthicSize, loadfinishFunc, castShadow, isAsyncLoad);
        return model;
    }

    public void CleanModelShowCameraList()
    {
        for (int i = 0; i < modelList.Count; i++)
        {
            if (modelList[i] != null)
            {
                UIModelShowTool.DisposeModelShow(modelList[i]);
            }
        }

        modelList.Clear();
    }

    public UIModelShowTool.UIModelShowData CreateModelShow(string modelName,
        string layerName = null,
        bool? orthographic = null,
        float? orthographicSize = null,
        bool? camAntialiasing = null,
        bool? camPostProcessing = null,
        Color? backgroundColor = null,
        Vector3? localPosition = null,
        Vector3? localScale = null,
        Vector3? eulerAngles = null,
        Vector3? texSize = null,
        float? nearClippingPlane = null,
        float? farClippingPlane = null,
        Vector3? cameraLocalPosition = null,
        Vector3? cameraEulerAngles = null,
        float? orthograthicSize = null,
        CallBack<UIModelShowTool.UIModelShowData> loadfinishFunc = null,
        bool? castShadow = null,
        bool? isAsyncLoad = null)
    {
        var model = UIModelShowTool.CreateModelData(modelName, layerName, orthographic, orthographicSize, camAntialiasing, camPostProcessing, backgroundColor, localPosition, eulerAngles, localScale, texSize, nearClippingPlane, farClippingPlane, cameraLocalPosition, cameraEulerAngles, orthograthicSize, loadfinishFunc, castShadow, isAsyncLoad);
        modelList.Add(model);
        return model;
    }

    public void RemoveModelShowCamera(UIModelShowTool.UIModelShowData data)
    {
        modelList.Remove(data);
        UIModelShowTool.DisposeModelShow(data);
    }

    #endregion

    List<UIBase> m_uiBaseList = null;
    Dictionary<string, List<UIBase>> m_mapUiBaseList = null;

    protected void ClearUiBaseList()
    {
        ClearUiBaseList(ref m_uiBaseList);
        if (null != m_mapUiBaseList)
        {
            Dictionary<string, List<UIBase>>.Enumerator enumerator = m_mapUiBaseList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                List<UIBase> uiBaseList = enumerator.Current.Value;
                ClearUiBaseList(ref uiBaseList);
            }
            m_mapUiBaseList.Clear();
            m_mapUiBaseList = null;
        }
    }

    private void ClearUiBaseList(ref List<UIBase> uiBaseList)
    {
        if (null == uiBaseList)
        {
            return;
        }
        for (int i = 0; i < uiBaseList.Count; ++i)
        {
            if (uiBaseList[i])
            {
                uiBaseList[i].Dispose();
            }
        }
        uiBaseList.Clear();
        uiBaseList = null;
    }
    /// <summary>
    /// 得到类型是T的数据
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rootName"></param>
    /// <returns></returns>
    protected T GetUIBase<T>(string rootName) where T : UIBase
    {
        if (!HasGameObject(rootName))
        {
            return null;
        }
        GameObject root = GetGameObject(rootName);
        if (null == root)
        {
            return null;
        }
        T item = root.GetComponent<T>();
        return item;
    }
    protected T[] GetUIBasesInChildren<T>(string rootName = "") where T : UIBase
    {
        GameObject root;
        if (string.IsNullOrEmpty(rootName))
        {
            root = this.gameObject;
        }
        else
        {
            if (!HaveObject(rootName))
            {
                return null;
            }
            root = GetGameObject(rootName);
        }
        if (null == root)
        {
            return null;
        }
        T[] items = root.GetComponentsInChildren<T>();
        return items;
    }

    /// <summary>
    /// 初始化uibase
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rootName"></param>
    /// <param name="objs"></param>
    /// <returns></returns>
    protected T InitUIBase<T>(string rootName, params object[] objs) where T : UIBase
    {
        if (!string.IsNullOrEmpty(rootName) && !HasGameObject(rootName))
        {
            return null;
        }
        T item = GetUIBase<T>(rootName);
        if (null != item)
        {
            item.Init(UIEventKey, UIID, objs);
        }
        return item;
    }

    /// <summary>
    /// 自动释放
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rootName"></param>
    protected void InitUIBasesInChildren<T>(string rootName = "") where T : UIBase
    {
        T[] uIBases = GetUIBasesInChildren<T>(rootName);
        if (null == uIBases)
        {
            return;
        }
        List<UIBase> uiBaseList;
        if (string.IsNullOrEmpty(rootName))
        {
            if (null == m_uiBaseList)
            {
                m_uiBaseList = new List<UIBase>();
            }
            uiBaseList = m_uiBaseList;
        }
        else
        {
            if (null == m_mapUiBaseList)
            {
                m_mapUiBaseList = new Dictionary<string, List<UIBase>>();
            }
            if (m_mapUiBaseList.ContainsKey(rootName))
            {
                uiBaseList = m_mapUiBaseList[rootName];
            }
            else
            {
                m_mapUiBaseList[rootName] = new List<UIBase>();
                uiBaseList = m_mapUiBaseList[rootName];
            }
        }
        for (int i = 0; i < uIBases.Length; ++i)
        {
            T uiBase = uIBases[i];
            if (null != uiBase)
            {
                uiBase.Init(UIEventKey, i + 1, this);
                uiBaseList.Add(uiBase);
            }
        }
    }

    /// <summary>
    /// 销毁UIBase
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="rootName"></param>
    protected void DestroyUIBase<T>(string rootName) where T : UIBase
    {
        if (!string.IsNullOrEmpty(rootName) && !HasGameObject(rootName))
        {
            return;
        }
        T item = GetUIBase<T>(rootName);
        if (null != item)
        {
            item.Dispose();
        }
    }


    #region 生命周期管理 

    protected List<UILifeCycleInterface> m_lifeComponent = new List<UILifeCycleInterface>();

    public void AddLifeCycleComponent(UILifeCycleInterface comp)
    {
        comp.Init(UIEventKey, m_lifeComponent.Count);
        m_lifeComponent.Add(comp);
    }

    void DisposeLifeComponent()
    {
        for (int i = 0; i < m_lifeComponent.Count; i++)
        {
            try
            {
                m_lifeComponent[i].Dispose();
            }
            catch (Exception e)
            {
                LogUtil.Exception("UIBase DisposeLifeComponent Exception -> UIEventKey: " + UIEventKey, e);
            }

        }

        m_lifeComponent.Clear();
    }

    #endregion

    #region 工具方法

    [ContextMenu("ObjectList 去重")]
    public void ClearObject()
    {
        List<GameObject> ls = new List<GameObject>();
        int len = m_objectList.Count;
        for (int i = 0; i < len; i++)
        {
            GameObject go = m_objectList[i];
            if (go != null)
            {
                if (!ls.Contains(go)) ls.Add(go);
            }
        }

        ls.Sort((a, b) =>
        {
            return a.name.CompareTo(b.name);
        });

        m_objectList = ls;
    }


    //将世界坐标转换为 UI 坐标系中的位置
    public Vector3 WorldPosToUIPos(Vector3 worldPos, string cameraKey)
    {
        Vector3 scale = Managers.UIManager.UILayerManager.GetUICameraDataByKey(cameraKey).m_root.GetComponent<RectTransform>().localScale;
        Vector3 UIPos = new Vector3(worldPos.x / scale.x, worldPos.y / scale.y, worldPos.z / scale.z);
        return UIPos;
    }

    //将 UI 坐标系中的位置 转换为 世界坐标
    public Vector3 UIPosToWorldPos(Vector3 UIPos, string cameraKey)
    {
        Vector3 scale = Managers.UIManager.UILayerManager.GetUICameraDataByKey(cameraKey).m_root.GetComponent<RectTransform>().localScale;
        Vector3 worldPos = new Vector3(UIPos.x * scale.x, UIPos.y * scale.y, UIPos.z * scale.z);
        return worldPos;
    }

    protected void SetBgSizeByCanvasHeight(Image image)
    {
        RectTransform rectTransform = image.transform.GetComponent<RectTransform>();

        Vector2 imageSize = rectTransform.sizeDelta;
        Rect rectScalar = ScreenUtil.GetCanvasRect();
        float p = imageSize.y / rectScalar.height;
        imageSize = imageSize / p;
        rectTransform.sizeDelta = imageSize;
    }

    #endregion
}
