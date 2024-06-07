using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ReusingPageRect : ScrollRectInput
{
    public string m_ItemName = "";

    public float scrollVelocity = 1f;      //page的滑动速度
    public int paddingSpaceHead = 0;
    public int paddingSpaceTail = 0;

    //默认是从上到下，从左到右，勾上此选项则反向
    public bool m_isInversion = false;
    //是否接受操作
    public bool m_isReceiveControl = true;
    public bool m_isReceivePageChange = false;

    /// <summary>
    /// 是否可滑动
    /// </summary>
    public bool m_isCanSlidingPage = true;

    public List<Dictionary<string, object>> m_datas = new List<Dictionary<string, object>>();
    public List<ReusingScrollItemBase> m_items = new List<ReusingScrollItemBase>();
    public List<ReusingScrollItemBase> m_itemCaches = new List<ReusingScrollItemBase>();
    private List<ReusingScrollItemBase> m_tempItems = new List<ReusingScrollItemBase>();
    //RectTransform m_rectTransform;

    private Bounds m_viewBounds;
    private Bounds m_viewTopBounds;
    private Bounds m_viewBottomBounds;
    public GameObject m_itemPrefab;

    public Vector3 m_itemSize;
    //Page设置
    public int m_curPageNum;
    private float m_targetPos;
    private bool isDrag = false;
    private float m_smooting = 4;
    private bool m_stopMove = true;
    private float m_startTime;
    private List<float> m_pagePosList = new List<float>();
    private float m_startDragPos;
    private float m_sensitivity;

    public float Sensitivity { get => m_sensitivity; set => m_sensitivity = value; }

    private bool m_IsShowAllItem = false;
    /// <summary>
    /// 是否显示所有
    /// </summary>
    public bool IsShowAllItem
    {
        get => m_IsShowAllItem;
        set => m_IsShowAllItem = value;
    }
    /// <summary>
    /// 页个数
    /// </summary>
    public int PageItemCount => m_pagePosList.Count;

    #region 公共方法
    public void SetStartTime(float startTime)
    {
        this.m_startTime = startTime;
    }
    public void SetStopMove(bool stopMove)
    {
        this.m_stopMove = stopMove;
    }
    public void SetIsDrag(bool isDrag)
    {
        this.isDrag = isDrag;
    }

    public void SetItem(string itemName, bool needFixScreen = false)
    {
        m_ItemName = itemName;
        //m_rectTransform = GetComponent<RectTransform>();

        Rebuild(CanvasUpdate.Layout);

        UpdateBound();
        SetLayout();

        m_itemPrefab = (GameObject)AssetManager.Instance.Load(UIPath.GetPath(itemName), typeof(GameObject));
        Vector3 size = m_itemPrefab.GetComponent<RectTransform>().sizeDelta;

        if (needFixScreen)
        {
            float scaleFactor = ScreenUtil.GetDesignScreenRatio() / ScreenUtil.GetScreenRatio();
            if (horizontal && scaleFactor < 1)
            {
                size.x /= scaleFactor;
            }
            if (vertical && scaleFactor > 1)
            {
                size.y *= scaleFactor;
            }
        }

        m_itemSize = size;
    }

    public override void Dispose()
    {
        base.Dispose();

        for (int i = 0; i < m_items.Count; i++)
        {
            DisposeItem(m_items[i]);
        }
        m_items.Clear();

        for (int i = 0; i < m_itemCaches.Count; i++)
        {
            DisposeItem(m_itemCaches[i]);
        }
        m_itemCaches.Clear();

        if (m_itemPrefab != null)
        {
            m_itemPrefab = null;

            if (!string.IsNullOrEmpty(m_ItemName))
                AssetManager.Instance.Unload(UIPath.GetPath(m_ItemName));
            m_ItemName = null;
        }
    }

    public void Refresh()
    {
        for (int i = 0; i < m_items.Count; i++)
        {
            if (m_items[i].m_index == m_curPageNum)
            {
                m_items[i].isChoose = true;
            }
            else
            {
                m_items[i].isChoose = false;
            }
            m_items[i].SetContent(m_items[i].m_index, m_datas[m_items[i].m_index]);
        }

        SetItemDisplay();
    }

    public override void SetData(List<Dictionary<string, object>> data)
    {
        m_datas = data;

        UpdateIndexList(data.Count);
        UpdateConetntSize(data.Count);
        UpdatePagePos(data.Count);

        SetItemDisplay();

        base.SetData(data);
    }

    public void ClearDatas()
    {
        for (int i = 0; i < m_items.Count; i++)
        {
            DisposeItem(m_items[i]);
        }
        m_items.Clear();

        for (int i = 0; i < m_itemCaches.Count; i++)
        {
            DisposeItem(m_itemCaches[i]);
        }
        m_itemCaches.Clear();
    }

    public ReusingScrollItemBase GetItem(int index)
    {
        for (int i = 0; i < m_items.Count; i++)
        {
            if (m_items[i].m_index == index)
            {
                return m_items[i];
            }
        }

        return null;
    }

    public Vector3 GetItemAnchorPos(int index)
    {
        if (content == null)
        {
            throw new Exception("GetItemAnchorPos Exception: content is null !");
        }

        return GetItemPos(index) + GetRealItemOffset() + content.localPosition;
    }

    public void SetPos(Vector3 pos)
    {
        if (content == null)
        {
            throw new Exception("SetPos Exception: content is null !");
        }

        content.anchoredPosition3D = pos;

        SetItemDisplay();
    }

    #endregion

    #region 继承方法
    bool m_rebuild = false;
    public void Update()
    {
        if (m_rebuild)
        {
            m_rebuild = false;
            SetItemDisplay();
        }
        if (!isDrag && !m_stopMove)
        {
            m_startTime += Time.deltaTime;
            float t = m_startTime * m_smooting;
            if (horizontal)
            {
                this.horizontalNormalizedPosition = Mathf.Lerp(this.horizontalNormalizedPosition, m_targetPos, t);
            }
            else
            {
                if (m_isInversion)
                {
                    this.verticalNormalizedPosition = Mathf.Lerp(this.verticalNormalizedPosition, m_targetPos, t);
                }
                else
                {
                    this.verticalNormalizedPosition = 1 - (Mathf.Lerp(1 - this.verticalNormalizedPosition, m_targetPos, t));
                }

            }

            SetItemDisplay();
            if (t >= 1)
            {
                OnValueChanged(this.normalizedPosition);
                m_stopMove = true;
            }
        }
    }

    protected override void OnSetContentAnchoredPosition(InputUIOnScrollEvent e)
    {
        base.OnSetContentAnchoredPosition(e);
        SetItemDisplay();
    }

    protected override void Start()
    {
        base.Start();
        SetItemDisplay();
        m_Direction = vertical ? Direction.Vertical : Direction.Horizontal;
    }

    public override void Rebuild(CanvasUpdate executing)
    {
        base.Rebuild(executing);

        UpdateBound();
        UpdatePagePos(m_datas.Count);
        m_rebuild = true;
    }

    #endregion

    #region 私有方法

    void SetLayout()
    {
        content.anchorMin = GetMinAchors();
        content.anchorMax = GetMaxAchors();
        content.pivot = GetPivot();
        content.anchoredPosition3D = Vector3.zero;

    }

    void UpdateBound()
    {
        m_viewBounds = new Bounds(viewRect.rect.center, viewRect.rect.size);
        m_viewTopBounds = new Bounds(viewRect.rect.max - new Vector2(0, (viewRect.rect.size.y * 0.1f) * 0.5f - 5f), viewRect.rect.size * 0.1f);
    }

    void UpdateConetntSize(int count)
    {
        Vector3 size = m_itemSize;

        if (horizontal)
        {
            size.x *= (count + paddingSpaceHead + paddingSpaceTail);
            size.y = 0;
        }

        if (vertical)
        {
            size.y *= (count + paddingSpaceHead + paddingSpaceTail);
            size.x = 0;
        }

        content.sizeDelta = size;
    }

    void UpdateIndexList(int count)
    {
        m_indexList = new List<ReusingData>();
        for (int i = 0; i < count; i++)
        {
            ReusingData reusingTmp = null;
            if (m_indexList.Count > i)
            {
                reusingTmp = m_indexList[i];
            }
            else
            {
                reusingTmp = new ReusingData();
                m_indexList.Add(reusingTmp);
            }

            reusingTmp.index = i;
            reusingTmp.status = ReusingStatus.Hide;
        }
    }

    List<ReusingData> m_indexList = new List<ReusingData>();

    Vector2 localCuror;
    void SetItemDisplay(bool isRebuild = false)
    {
        if (content == null)
        {
            throw new Exception("SetItemDisplay Exception: content is null !");
        }

        //for (int i = 0; i < m_items.Count; i++)
        //{
        //    if (m_items[i].m_index == 0)
        //    {
        //        localCuror = Managers.UIManager.GetPosToOtherUI(content, m_items[i].RectTransform.position);
        //        LogUtil.Log("localCuror = " + localCuror.y + " index = " + m_items[i].m_index);
        //    }
        //}
        m_tempItems.Clear();
        //计算已显示的哪些需要隐藏
        for (int i = 0; i < m_items.Count; i++)
        {
            m_items[i].OnDrag();

            Bounds tempBounds = m_viewBounds;
            if (IsShowAllItem)
            {
                tempBounds = base.m_ContentBounds;
            }
            //已经完全离开了显示区域
            if (!tempBounds.Intersects(GetItemBounds(m_items[i].m_index)))
            {
                ReusingScrollItemBase itemTmp = m_items[i];
                m_tempItems.Add(itemTmp);
                itemTmp.OnHide();

                if (!isRebuild)
                {
                    //隐藏并移到缓存
                    itemTmp.gameObject.SetActive(false);
                }
                m_itemCaches.Add(itemTmp);
                if (m_indexList.Count > itemTmp.m_index)
                {
                    m_indexList[itemTmp.m_index].status = ReusingStatus.Hide;
                }
            }
        }

        for (int i = 0; i < m_tempItems.Count; i++)
        {
            m_items.Remove(m_tempItems[i]);
        }

        //计算出哪些需要显示
        //如果有未显示的则显示出来，从对象池取出对象
        bool m_clip = false;
        for (int i = 0; i < m_indexList.Count; i++)
        {
            if (m_indexList[i].status == ReusingStatus.Hide)
            {
                Bounds tempBounds = m_viewBounds;
                if (IsShowAllItem)
                {
                    tempBounds = base.m_ContentBounds;
                }
                if (tempBounds.Intersects(GetItemBounds(m_indexList[i].index)))
                {
                    ShowItem(i, isRebuild, m_datas[i]);


                    m_indexList[i].status = ReusingStatus.Show;
                    m_clip = true;
                }
                else
                {
                    if (m_clip)
                    {
                        break;
                    }
                }
            }
            else
            {
                m_clip = true;
            }
        }
    }

    void ShowItem(int index, bool isRebuild, Dictionary<string, object> data)
    {
        ReusingScrollItemBase itemTmp = GetItem();
        if (itemTmp == null) return;
        itemTmp.transform.SetParent(content);
        itemTmp.transform.localScale = Vector3.one;

        if (!isRebuild)
        {
            if (index == m_curPageNum)
            {
                itemTmp.isChoose = true;
            }
            else
            {
                itemTmp.isChoose = false;
            }
            itemTmp.SetContent(index, data);
        }

        itemTmp.RectTransform.pivot = GetPivot();
        itemTmp.RectTransform.anchorMin = GetMinAchors();
        itemTmp.RectTransform.anchorMax = GetMaxAchors();
        itemTmp.RectTransform.sizeDelta = GetItemSize();

        itemTmp.RectTransform.anchoredPosition3D = GetItemPos(index);

        itemTmp.m_index = index;
    }

    ReusingScrollItemBase GetItem()
    {
        ReusingScrollItemBase result = null;

        if (m_itemCaches.Count > 0)
        {
            result = m_itemCaches[0];
            result.gameObject.SetActive(true);
            result.OnShow();
            m_itemCaches.RemoveAt(0);

            m_items.Add(result);
            return result;
        }

        //result = GameObjectManager.CreateGameObjectByPool(m_itemPrefab).GetComponent<ReusingScrollItemBase>();
        if (m_itemPrefab == null) return null;
        result = GameObject.Instantiate(m_itemPrefab).GetComponent<ReusingScrollItemBase>();
        result.AssetName = m_ItemName;
        result.Init(m_UIEventKey, m_items.Count);
        RecursionInitUI(null, result);
        m_items.Add(result);

        return result;
    }

    Bounds GetItemBounds(int index)
    {
        return new Bounds(GetItemPos(index) + GetRealItemOffset() + content.localPosition, m_itemSize);
    }

    Vector3 GetItemPos(int index)
    {
        Vector3 offset = Vector3.zero;
        if (vertical)
        {
            offset = new Vector3(0, -m_itemSize.y, 0);
        }
        else
        {
            offset = new Vector3(m_itemSize.x, 0, 0);
        }

        if (m_isInversion)
        {
            offset *= -1;
        }

        offset *= index;

        if (vertical)
        {
            if (m_isInversion)
            {
                offset.y = offset.y + (m_itemSize.y * paddingSpaceHead);
            }
            else
            {
                offset.y = offset.y - (m_itemSize.y * paddingSpaceHead);
            }
        }
        else
        {
            offset.x = (paddingSpaceHead * m_itemSize.x) + offset.x;
        }

        return offset;
    }

    Vector3 GetRealItemOffset()
    {
        Vector3 offset;
        if (vertical)
        {

            offset = new Vector3(0, -m_itemSize.y / 2, 0);
        }
        else
        {
            offset = new Vector3(m_itemSize.x / 2, 0, 0);
        }

        if (m_isInversion)
        {
            offset *= -1;
        }

        return offset;
    }

    Vector2 GetPivot()
    {
        Vector2 pivot = new Vector2(0.5f, 0.5f);

        if (horizontal)
        {
            if (!m_isInversion)
            {
                pivot.x = 0;
            }
            else
            {
                pivot.x = 1;
            }
        }

        if (vertical)
        {
            if (!m_isInversion)
            {
                pivot.y = 1;
            }
            else
            {
                pivot.y = 0;
            }
        }

        return pivot;
    }

    Vector2 GetMinAchors()
    {
        Vector2 minAchors = new Vector2(0.5f, 0.5f);

        if (horizontal)
        {
            if (!m_isInversion)
            {
                minAchors.x = 0;
            }
            else
            {
                minAchors.x = 1;
            }

            minAchors.y = 0;
        }

        if (vertical)
        {
            if (!m_isInversion)
            {
                minAchors.y = 1;
            }
            else
            {
                minAchors.y = 0;
            }
            minAchors.x = 0;
        }

        return minAchors;
    }

    Vector2 GetMaxAchors()
    {
        Vector2 maxAchors = new Vector2(0.5f, 0.5f);

        if (horizontal)
        {
            if (!m_isInversion)
            {
                maxAchors.x = 0;
            }
            else
            {
                maxAchors.x = 1;
            }

            maxAchors.y = 1;
        }

        if (vertical)
        {
            if (!m_isInversion)
            {
                maxAchors.y = 1;
            }
            else
            {
                maxAchors.y = 0;
            }
            maxAchors.x = 1;
        }

        return maxAchors;
    }

    Vector2 GetItemSize()
    {
        Vector3 size = m_itemSize;

        if (horizontal)
        {
            size.y = 0;
        }

        if (vertical)
        {
            size.x = 0;
        }
        return size;
    }

    #endregion

    #region 动画

    public void StartEnterAnim()
    {
        m_isReceiveControl = false;
        StartCoroutine(EnterAnim());
    }

    public void StartExitAnim()
    {
        m_isReceiveControl = false;
        StartCoroutine(ExitAnim());
    }

    void EndEnterAnim()
    {
        m_isReceiveControl = true;
    }

    void EndExitAnim()
    {
        m_isReceiveControl = true;
    }

    public virtual IEnumerator EnterAnim()
    {
        return null;
    }

    public virtual IEnumerator ExitAnim()
    {
        return null;
    }

    #endregion

    #region 私有类和枚举

    class ReusingData
    {
        public int index;
        public ReusingStatus status;
    }

    enum ReusingStatus
    {
        Show,
        Hide
    }

    #endregion

    #region Page相关

    public void JumpPage(int pageNum)
    {
        if (m_curPageNum == pageNum)
        {
            return;
        }
        m_targetPos = m_pagePosList[pageNum];
        m_curPageNum = pageNum;
        if (m_isReceivePageChange)
        {
            EventManager.DispatchEvent(this, "PageRect_PageChange", new object[] { m_curPageNum });
        }
        //LogUtil.Log("当前页 = " + m_curPageNum + ", Pos = " + m_targetPos);
        isDrag = false;
        m_startTime = 0;
        m_stopMove = false;
    }

    public void ChangePage(int pageNum)
    {
        if (m_curPageNum == pageNum)
        {
            return;
        }
        m_curPageNum = pageNum;
        StartCoroutine(ChangePageWaitFrame());
    }

    private IEnumerator ChangePageWaitFrame()
    {
        yield return null;
        m_targetPos = m_pagePosList[m_curPageNum];
        if (horizontal)
        {
            this.horizontalNormalizedPosition = m_targetPos;
        }
        else if (vertical)
        {
            this.verticalNormalizedPosition = m_targetPos;
        }
        SetItemDisplay();
    }

    public void UpdatePagePos(int count)
    {
        float length = content.rect.width;
        if (horizontal)
        {
            length = content.rect.width - m_viewBounds.size.x;
        }
        else if (vertical)
        {
            length = content.rect.height - m_viewBounds.size.y;
        }
        m_pagePosList.Clear();
        for (int i = 0; i < count + (paddingSpaceTail + paddingSpaceHead); i++)
        {
            if (horizontal)
            {
                m_pagePosList.Add(m_itemSize.x * i / length);
            }
            else
            {
                m_pagePosList.Add(m_itemSize.y * i / length);
            }
        }
    }

    void SetPageSyn()
    {
        float posDiff;

        if (horizontal)
        {
            posDiff = this.horizontalNormalizedPosition;
        }
        else
        {
            if (m_isInversion)
            {
                posDiff = this.verticalNormalizedPosition;
            }
            else
            {
                posDiff = 1 - this.verticalNormalizedPosition;
            }
        }

        float move = (posDiff - m_startDragPos) * m_sensitivity;
        if (m_pagePosList.Count >= 2)
        {
            float delta = Mathf.Abs(m_pagePosList[1] - m_pagePosList[0]) / 2;
            if (move > 0)
            {
                move = Math.Min(move, delta);
            }
            else
            {
                move = Math.Max(move, -delta);
            }
        }

        posDiff += (move);

        posDiff = posDiff < 1 ? posDiff : 1;
        posDiff = posDiff > 0 ? posDiff : 0;

        int index = 0;

        float offset = Mathf.Abs(m_pagePosList[index] - posDiff);

        for (int i = 1; i < m_pagePosList.Count; i++)
        {
            float temp = Mathf.Abs(m_pagePosList[i] - posDiff);
            if (temp < offset)
            {
                index = i;
                offset = temp;
            }
        }

        JumpPage(index);
    }
    #region 重载方法
    public ScrollRect m_ParentScrollRect;
    //滑动方向
    private Direction m_Direction = Direction.Vertical;
    //当前操作方向
    private Direction m_BeginDragDirection = Direction.Vertical;

    public Direction scrollDirection { get => m_Direction; }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (!m_isCanSlidingPage)
            return;

        Transform parent = transform.parent;
        if (parent)
        {
            m_ParentScrollRect = parent.GetComponentInParent<ScrollRect>();
        }
        if (m_ParentScrollRect)
        {
            m_BeginDragDirection = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y) ? Direction.Horizontal : Direction.Vertical;
            if (m_BeginDragDirection != m_Direction)
            {
                //当前操作方向不等于滑动方向，将事件传给父对象
                ExecuteEvents.Execute(m_ParentScrollRect.gameObject, eventData, ExecuteEvents.beginDragHandler);
                return;
            }
        }
        base.OnBeginDrag(eventData);
        //更新两个包围盒的数据
        UpdateBounds();
        m_PointerStartLocalCursor2 = Vector2.zero;
        //记录由屏幕坐标转换为视图区域下的起始位置坐标
        RectTransformUtility.ScreenPointToLocalPointInRectangle(viewRect, eventData.position, eventData.pressEventCamera, out m_PointerStartLocalCursor2);

        isDrag = true;
        if (horizontal)
        {
            m_startDragPos = this.horizontalNormalizedPosition;
        }
        else
        {
            if (m_isInversion)
            {
                m_startDragPos = this.horizontalNormalizedPosition;
            }
            else
            {
                m_startDragPos = 1 - this.verticalNormalizedPosition;
            }
        }
    }

    Vector2 m_PointerStartLocalCursor2 = Vector2.zero;

    public override void OnDrag(PointerEventData eventData)
    {
        if (!m_isCanSlidingPage)
            return;

        if (m_ParentScrollRect)
        {
            if (m_BeginDragDirection != m_Direction)
            {
                //当前操作方向不等于滑动方向，将事件传给父对象
                ExecuteEvents.Execute(m_ParentScrollRect.gameObject, eventData, ExecuteEvents.dragHandler);
                return;
            }
        }
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        if (!IsActive())
            return;
        Vector2 localCursor;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(viewRect, eventData.position, eventData.pressEventCamera, out localCursor))
            return;
        UpdateBounds();
        //与起始坐标求插值
        var pointerDelta2 = localCursor - m_PointerStartLocalCursor2;
        //计算拖拽变化后的区域位置
        Vector2 position2 = m_ContentStartPosition + pointerDelta2 * scrollVelocity;

        // 计算内容区域在视图区域下是否需要偏移量,即是否有超出边界的情况
        Vector2 offset = CalculateOffset(position2 - content.anchoredPosition);
        position2 += offset;
        if (movementType == MovementType.Elastic)
        {
            //当处于弹性模式下时,会根据偏移量增加一个弹性势能让内容区域不会全部处于视图区域的外部
            if (offset.x != 0)
                position2.x = position2.x - RubberDelta(offset.x, m_viewBounds.size.x);//这里随着offset值越大势能越强
            if (offset.y != 0)
                position2.y = position2.y - RubberDelta(offset.y, m_viewBounds.size.y);
        }

        //更新内容区域坐标
        SetContentAnchoredPosition(position2);

        base.OnDrag(eventData);
    }

    private Vector2 CalculateOffset(Vector2 delta)
    {
        Vector2 offset = Vector2.zero;
        if (movementType == MovementType.Unrestricted)
            return offset;

        Vector2 min = m_ContentBounds.min;
        Vector2 max = m_ContentBounds.max;

        if (horizontal)
        {
            min.x += delta.x;
            max.x += delta.x;
            if (min.x > m_viewBounds.min.x)
                offset.x = m_viewBounds.min.x - min.x;
            else if (max.x < m_viewBounds.max.x)
                offset.x = m_viewBounds.max.x - max.x;
        }

        if (vertical)
        {
            min.y += delta.y;
            max.y += delta.y;
            if (max.y < m_viewBounds.max.y)
                offset.y = m_viewBounds.max.y - max.y;
            else if (min.y > m_viewBounds.min.y)
                offset.y = m_viewBounds.min.y - min.y;
        }

        return offset;
    }

    private static float RubberDelta(float overStretching, float viewSize)
    {
        return (1 - (1 / ((Mathf.Abs(overStretching) * 0.55f / viewSize) + 1))) * viewSize * Mathf.Sign(overStretching);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (!m_isCanSlidingPage)
            return;

        if (m_ParentScrollRect)
        {
            if (m_BeginDragDirection != m_Direction)
            {
                //当前操作方向不等于滑动方向，将事件传给父对象
                ExecuteEvents.Execute(m_ParentScrollRect.gameObject, eventData, ExecuteEvents.endDragHandler);
                return;
            }
        }
        base.OnEndDrag(eventData);
        if (m_datas.Count > 0)
        {
            SetPageSyn();
        }
        isDrag = false;
        m_startTime = 0;
        m_stopMove = false;

        base.OnEndDrag(eventData);
    }

    public override void OnScroll(PointerEventData data)
    {
        if (m_ParentScrollRect)
        {
            if (m_BeginDragDirection != m_Direction)
            {
                //当前操作方向不等于滑动方向，将事件传给父对象
                ExecuteEvents.Execute(m_ParentScrollRect.gameObject, data, ExecuteEvents.scrollHandler);
                return;
            }
        }
        base.OnScroll(data);
    }
    #endregion

    #endregion
}
