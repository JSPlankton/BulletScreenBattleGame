using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using UnityEngine.EventSystems;
using Managers;
using DG.Tweening;
public class ScrollConstString
{
    public const string ItemPrefabKey = "itemPrefabIndex";
}

public class ReusingScrollRect : ScrollRectInput
{
    public string m_ItemName = "";

    //默认是从上到下，从左到右，勾上此选项则反向
    public bool m_isInversion = false;
    //是否接受操作
    public bool m_isReceiveControl = true;

    public List<Dictionary<string, object>> m_datas = new List<Dictionary<string, object>>();
    public List<ReusingScrollItemBase> m_items = new List<ReusingScrollItemBase>();
    public List<Dictionary<int, ReusingScrollItemBase>> m_itemCaches = new List<Dictionary<int, ReusingScrollItemBase>>();
    private List<ReusingScrollItemBase> m_tempItems = new List<ReusingScrollItemBase>();
    private List<TimerEvent> m_timerEventList = new List<TimerEvent>();

    //RectTransform m_rectTransform;

    private Bounds m_viewBounds;

    //新的逻辑
    public string[] m_ItemNames;
    public GameObject[] m_itemPrefabs;
    public Vector3[] m_itemSizes;

    public float delayTime = 0.02f;
    //m_items是否初始化完成（有动画时，m_items会延迟初始化，于是该属性应运而生）
    private bool m_showItemEnd = false;
    public bool haveAnim = false;

    #region 公共方法
    public void SetItem(string itemName)
    {
        m_ItemNames = new string[1];
        m_itemPrefabs = new GameObject[1];
        m_itemSizes = new Vector3[1];
        m_ItemNames[0] = itemName;

        Rebuild(CanvasUpdate.Layout);

        UpdateBound();
        SetLayout();

        for (int i = 0; i < m_ItemNames.Length; i++)
        {
            m_itemPrefabs[i] = (GameObject)AssetManager.Instance.Load(UIPath.GetPath(m_ItemNames[i]), typeof(GameObject));
            m_itemSizes[i] = m_itemPrefabs[i].GetComponent<RectTransform>().sizeDelta;
        }
    }

    public void SetItems(params string[] itemNames)
    {
        m_ItemNames = itemNames;
        m_itemPrefabs = new GameObject[m_ItemNames.Length];
        m_itemSizes = new Vector3[m_ItemNames.Length];

        Rebuild(CanvasUpdate.Layout);

        UpdateBound();
        SetLayout();

        for (int i = 0; i < m_ItemNames.Length; i++)
        {
            m_itemPrefabs[i] = (GameObject)AssetManager.Instance.Load(UIPath.GetPath(m_ItemNames[i]), typeof(GameObject));
            m_itemSizes[i] = m_itemPrefabs[i].GetComponent<RectTransform>().sizeDelta;
        }
    }

    private bool HasAsset(string asset)
    {
        for (int i = 0; i < m_ItemNames.Length; i++)
        {
            if (m_ItemNames[i].Equals(asset))
                return true;
        }
        return false;
    }

    private int GetAssetIndex(string asset)
    {
        for (int i = 0; i < m_ItemNames.Length; i++)
        {
            if (m_ItemNames[i].Equals(asset))
                return i;
        }
        return 0;
    }

    public void ClearDatas()
    {
        for (int i = 0; i < m_items.Count; i++)
        {
            //DisposeItem(m_items[i]);

            ReusingScrollItemBase reusingScrollItemBase = m_items[i];

            if (!HasAsset(reusingScrollItemBase.AssetName))
            {
                DisposeItem(reusingScrollItemBase);
                continue;
            }

            reusingScrollItemBase.m_index = -1;
            Dictionary<int, ReusingScrollItemBase> tmpData = new Dictionary<int, ReusingScrollItemBase>();
            tmpData.Add(GetAssetIndex(reusingScrollItemBase.AssetName), m_items[i]);
            m_items[i].gameObject.SetActive(false);
            m_itemCaches.Add(tmpData);
        }
        m_items.Clear();
    }

    private void RemoveTimerEventList()
    {
        for (int i = 0; i < m_timerEventList.Count; i++)
        {
            if (m_timerEventList[i] != null)
            {
                TimerManager.RemoveTimer(m_timerEventList[i]);
                m_timerEventList[i] = null;
            }
        }
        m_timerEventList.Clear();

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
            foreach (var item in m_itemCaches[i])
            {
                DisposeItem(item.Value);
            }
        }
        m_itemCaches.Clear();

        for (int i = 0; i < m_itemPrefabs.Length; i++)
        {
            if (m_itemPrefabs[i] != null)
            {
                m_itemPrefabs[i] = null;

                if (!string.IsNullOrEmpty(m_ItemNames[i]))
                    AssetManager.Instance.Unload(UIPath.GetPath(m_ItemNames[i]));
                m_ItemNames[i] = null;
            }
        }

        RemoveTimerEventList();
    }

    public void Refresh(bool isShowDes = false)
    {
        for (int i = 0; i < m_items.Count; i++)
        {
            //m_items[i].SetContent(m_items[i].m_index, m_datas[m_items[i].m_index]);
            m_items[i].RectTransform.anchoredPosition3D = GetItemPos(m_items[i].m_index);
        }

        SetItemDisplay();
    }

    public void RefreshData(List<Dictionary<string, object>> data, bool isLayout = false)
    {
        if (isLayout)
        {
            SetLayout();
            if (m_items.Count > 0)
            {
                ClearDatas();
            }
        }

        SetData(data);
        Refresh();
    }

    public override void SetData(List<Dictionary<string, object>> data)
    {
        SetData(data, false);
    }

    public override void SetData(List<Dictionary<string, object>> data, bool needDelay)
    {
        m_showItemEnd = false;
        m_datas = data;

        UpdateIndexList(data);
        UpdateConetntSize(data);
        UpdateBound();
        SetItemDisplay(false, needDelay);

        base.SetData(data, needDelay);
    }

    public void ReSetData(List<Dictionary<string, object>> data, bool needDelay = false)
    {
        RemoveTimerEventList();

        if (m_items.Count > 0)
        {
            ClearDatas();
        }

        SetData(data, needDelay);
    }

    public void AddData(Dictionary<string, object> data, bool update = true)
    {
        m_datas.Add(data);
        AddIndexList(data);
        UpdateConetntSize(m_datas);

        if (update)
        {
            SetItemDisplay();
        }
    }

    public void InsertData(Dictionary<string, object> data, int index, bool update = true)
    {
        m_datas.Insert(index, data);
        InsertIndexList(data, index);
        UpdateItemIndex(index);
        UpdateConetntSize(m_datas);

        if (update)
        {
            SetItemDisplay();
        }
    }

    public void GetAllItemPos(Dictionary<int, Vector3> itemPosDict)
    {
        for (int i = 0; i < m_indexList.Count; i++)
        {
            Vector3 localPos = GetItemPos(i);
            localPos.y -= GetItemSize(m_datas[i]).y / 2;
            Vector3 worldpos = content.transform.TransformPoint(localPos);
            itemPosDict.Add(i, worldpos);
        }
    }

    public void RemoveDataByPrefabIndex(int prefabIndex)
    {
        int itemIndex = -1;

        for (int i = 0; i < m_indexList.Count; i++)
        {
            ReusingData reuTmp = m_indexList[i];
            if (reuTmp.resIndex == prefabIndex)
            {
                itemIndex = reuTmp.index;
            }
        }
        if (itemIndex < 0) return;

        m_datas.RemoveAt(itemIndex);
        m_indexList.RemoveAt(itemIndex);

        ReusingScrollItemBase tmp = null;
        for (int i = 0; i < m_items.Count; i++)
        {
            if (m_items[i].m_index == itemIndex)
            {
                tmp = m_items[i];
                break;
            }
        }
        if (tmp != null)
        {
            m_items.Remove(tmp);
            tmp.gameObject.SetActive(false);
            tmp.m_index = -1;
            Dictionary<int, ReusingScrollItemBase> tmpData = new Dictionary<int, ReusingScrollItemBase>();
            tmpData.Add(prefabIndex, tmp);
            m_itemCaches.Add(tmpData);

            for (int i = itemIndex; i < m_indexList.Count; i++)
            {
                ReusingData reusingTmp = m_indexList[i];
                reusingTmp.index = i;
            }
        }

        UpdateRemoveItemIndex(itemIndex);
        UpdateConetntSize(m_datas);
    }

    public void RemoveData(int index)
    {
        m_datas.RemoveAt(index);
        RemoveIndexList(index);
        UpdateRemoveItemIndex(index);
        UpdateConetntSize(m_datas);

        SetItemDisplay();
    }

    /// <summary>
    /// 托动事件
    /// </summary>
    private bool m_isDragEvent = false;
    /// <summary>
    /// 事件源
    /// </summary>
    private object m_eventSource = null;

    /// <summary>
    /// 设置
    /// </summary>
    /// <param name="isDragEvent"></param>
    /// <param name="eventSource"></param>
    public void SetDragEventInfo(bool isDragEvent, object eventSource)
    {
        m_isDragEvent = isDragEvent;
        m_eventSource = eventSource;
    }
    /// <summary>
    /// 根据数据的索引获取Item
    /// </summary>
    /// <param name="dataIndex">数据索引（非当前显示位置的索引）</param>
    /// <returns></returns>
    public ReusingScrollItemBase GetItemByDataIndex(int dataIndex)
    {
        for (int i = 0; i < m_items.Count; i++)
        {
            if (m_items[i].m_index == dataIndex)
            {
                return m_items[i];
            }
        }

        return null;
    }

    public void SetPos(Vector3 pos)
    {
        if (content == null)
        {
            throw new Exception("SetItemDisplay Exception: content is null !" + this.name);
        }

        content.anchoredPosition3D = pos;
        SetItemDisplay();
    }

    public void ScrollToPosUseAct(Vector3 pos, float actTime = 0.0f)
    {
        if (content == null)
        {
            throw new Exception("SetItemDisplay Exception: content is null !" + this.name);
        }

        //content.anchoredPosition3D = pos;
        content.DOAnchorPos3D(pos, actTime).onUpdate = () =>
        {
            SetItemDisplay();
        };

    }

    public void DisposeDatas()
    {
        for (int i = 0; i < m_items.Count; i++)
        {
            DisposeItem(m_items[i]);
        }
        m_items.Clear();
    }

    #endregion

    #region 继承方法
    bool m_rebuild = false;
    public void Update()
    {
        if (m_rebuild)
        {
            m_rebuild = false;
            SetItemDisplay(false, haveAnim);
        }
    }

    protected override void OnSetContentAnchoredPosition(InputUIOnScrollEvent e)
    {
        base.OnSetContentAnchoredPosition(e);
        SetItemDisplay(false, false, true);
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
    }

    void UpdateConetntSize(List<Dictionary<string, object>> datas)
    {
        Vector3 size = Vector3.zero;
        int prefabIdx = 0;
        if (m_itemPrefabs.Length > 1)
        {
            //多个itemPrefab
            for (int i = 0; i < datas.Count; i++)
            {
                var cellData = datas[i];
                if (cellData == null) continue;

                prefabIdx = GetItemPrefabIndex(cellData);
                if (horizontal)
                {
                    size.x += m_itemSizes[prefabIdx].x;
                    size.y = 0;
                }

                if (vertical)
                {
                    size.x = 0;
                    size.y += m_itemSizes[prefabIdx].y;
                }
            }
        }
        else
        {
            //单个itemPrefab
            if (horizontal)
            {
                size.x = m_itemSizes[0].x * datas.Count;
                size.y = 0;
            }

            if (vertical)
            {
                size.x = 0;
                size.y = m_itemSizes[0].y * datas.Count;
            }
        }

        content.sizeDelta = size;

    }

    void UpdateIndexList(List<Dictionary<string, object>> datas)
    {
        int count = datas.Count;
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
            reusingTmp.resIndex = GetItemPrefabIndex(datas[i]);
        }
    }

    void AddIndexList(Dictionary<string, object> data)
    {
        ReusingData reusingTmp = new ReusingData();
        reusingTmp.index = m_indexList.Count;
        reusingTmp.status = ReusingStatus.Hide;
        reusingTmp.resIndex = GetItemPrefabIndex(data);
        m_indexList.Add(reusingTmp);
    }

    void InsertIndexList(Dictionary<string, object> data, int index)
    {
        ReusingData reusingTmp = new ReusingData();
        reusingTmp.index = m_indexList.Count;
        reusingTmp.status = ReusingStatus.Hide;
        reusingTmp.resIndex = GetItemPrefabIndex(data);
        m_indexList.Insert(index, reusingTmp);
        for (int i = index; i < m_indexList.Count; i++)
        {
            ReusingData tmp = m_indexList[i];
            tmp.index = i;
        }
    }

    void RemoveIndexList(int index)
    {
        int removePrefaIndex = 0;
        ReusingData reusTmp = m_indexList[index];
        if (reusTmp != null)
        {
            removePrefaIndex = reusTmp.resIndex;
        }

        m_indexList.RemoveAt(index);
        ReusingScrollItemBase tmp = null;
        for (int i = 0; i < m_items.Count; i++)
        {
            if (m_items[i].m_index == index)
            {
                tmp = m_items[i];
                break;
            }
        }
        if (tmp != null)
        {
            m_items.Remove(tmp);
            tmp.gameObject.SetActive(false);
            tmp.m_index = -1;
            Dictionary<int, ReusingScrollItemBase> tmpData = new Dictionary<int, ReusingScrollItemBase>();
            tmpData.Add(removePrefaIndex, tmp);
            m_itemCaches.Add(tmpData);
        }

        for (int i = index; i < m_indexList.Count; i++)
        {
            ReusingData reusingTmp = m_indexList[i];
            reusingTmp.index = i;
        }
    }

    void UpdateItemIndex(int index)
    {
        for (int i = 0; i < m_items.Count; i++)
        {
            if (m_items[i].m_index >= index)
            {
                m_items[i].m_index = m_items[i].m_index + 1;
            }
        }
    }

    void UpdateRemoveItemIndex(int index)
    {
        for (int i = 0; i < m_items.Count; i++)
        {
            if (m_items[i].m_index >= index)
            {
                m_items[i].m_index = m_items[i].m_index - 1;
            }
        }
    }

    List<ReusingData> m_indexList = new List<ReusingData>();

    public void SetItemDisplay(bool isRebuild = false, bool needDelay = false, bool isDrag = false)
    {
        if (content == null)
        {
            throw new Exception("SetItemDisplay Exception: content is null !" + this.name);
        }

        m_tempItems.Clear();
        //计算已显示的哪些需要隐藏
        for (int i = 0; i < m_items.Count; i++)
        {
            m_items[i].OnDrag();

            //已经完全离开了显示区域
            if (!m_viewBounds.Intersects(GetItemBounds(m_items[i].m_index)))
            {
                ReusingScrollItemBase itemTmp = m_items[i];
                m_tempItems.Add(itemTmp);
                itemTmp.OnHide();

                if (!isRebuild)
                {
                    //隐藏并移到缓存
                    itemTmp.gameObject.SetActive(false);
                }
                Dictionary<int, ReusingScrollItemBase> tmpData = new Dictionary<int, ReusingScrollItemBase>();
                tmpData.Add(m_indexList[itemTmp.m_index].resIndex, itemTmp);
                m_itemCaches.Add(tmpData);

                m_indexList[itemTmp.m_index].status = ReusingStatus.Hide;
            }
        }

        for (int i = m_tempItems.Count - 1; i >= 0; i--)
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
                if (m_viewBounds.Intersects(GetItemBounds(i)))
                {
                    if (needDelay)
                    {
                        SetDelayAnim(isRebuild, i, m_datas[i]);
                    }
                    else
                    {
                        ShowItem(i, isRebuild, m_datas[i], isDrag);
                    }

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
    private void SetDelayAnim(bool isRebuild, int index, Dictionary<string, object> data)
    {
        TimerEvent timerEvent = TimerManager.DelayCallBack(delayTime * index, (ga) =>
        {
            ShowItem(index, isRebuild, data, true);
        });
        m_timerEventList.Add(timerEvent);
    }
    void UpdateItemPos()
    {
        for (int i = 0; i < m_items.Count; i++)
        {
            m_items[i].RectTransform.anchoredPosition3D = GetItemPos(i);
        }
    }

    void ShowItem(int index, bool isRebuild, Dictionary<string, object> data, bool needAnim = false)
    {
        ReusingScrollItemBase itemTmp = GetItem(data);
        if (itemTmp == null)
        {
            LogUtil.Error("ScrollRect Show Item Error, Reason : itemTmp is Null");
            return;
        }
        itemTmp.transform.SetParent(content);
        itemTmp.transform.localScale = Vector3.one;

        itemTmp.needAnim = needAnim;

        if (!isRebuild)
        {
            itemTmp.SetContent(index, data);
        }

        itemTmp.RectTransform.pivot = GetPivot();
        itemTmp.RectTransform.anchorMin = GetMinAchors();
        itemTmp.RectTransform.anchorMax = GetMaxAchors();
        itemTmp.RectTransform.sizeDelta = GetItemSize(data);

        itemTmp.RectTransform.anchoredPosition3D = GetItemPos(index);

        itemTmp.m_index = index;

        m_showItemEnd = true;
    }

    /// <summary>
    /// m_items是否初始化完成（有动画时，m_items会延迟初始化，于是该属性应运而生）
    /// </summary>
    /// <returns></returns>
    public bool isShowItemEnd()
    {
        return m_showItemEnd;
    }

    ReusingScrollItemBase GetItem(Dictionary<string, object> data)
    {
        ReusingScrollItemBase result = null;
        int prefabIndex = GetItemPrefabIndex(data);
        if (m_itemCaches.Count > 0)
        {
            int delIndex = 0;
            for (int i = 0; i < m_itemCaches.Count; i++)
            {
                if (m_itemCaches[i].ContainsKey(prefabIndex))
                {
                    result = m_itemCaches[i][prefabIndex];
                    delIndex = i;
                }
            }
            if (result != null)
            {
                result.OnResetInit();
                result.gameObject.SetActive(true);
                result.OnShow();
                m_itemCaches.RemoveAt(delIndex);

                m_items.Add(result);
                return result;
            }
        }

        if (m_itemPrefabs.Length > 0)
        {
            if (m_itemPrefabs.Length > 1)
            {
                if (m_itemPrefabs[prefabIndex] != null)
                {
                    //多个prefab
                    result = Instantiate(m_itemPrefabs[prefabIndex]).GetComponent<ReusingScrollItemBase>();
                    result.AssetName = m_ItemNames[prefabIndex];
                }
                else
                {
                    throw new Exception(string.Format("ScrollRect ItemPrefabs Is Null PrefabIndex = {0}, {1}, {2}",
                        prefabIndex, m_ItemNames[prefabIndex], this.name));
                }
            }
            else
            {
                if (m_itemPrefabs[0] != null)
                {
                    //单个prefab
                    result = Instantiate(m_itemPrefabs[0]).GetComponent<ReusingScrollItemBase>();
                    result.AssetName = m_ItemNames[0];
                }
                else
                {
                    throw new Exception(string.Format("ScrollRect Single ItemPrefab Is Null PrefabIndex = 0 , {0}, {1}",
                        m_ItemNames[0], this.name));
                }
            }
        }
        else
        {
            throw new Exception("ScrollRect Not Have ItemPrefabs!");
        }

        if (result != null)
        {
            result.Init(m_UIEventKey, m_items.Count);
            RecursionInitUI(null, result);
            m_items.Add(result);
        }

        return result;
    }

    private int GetItemPrefabIndex(Dictionary<string, object> data)
    {
        int prefabIdx = 0;
        if (data.ContainsKey(ScrollConstString.ItemPrefabKey))
        {
            int.TryParse(data[ScrollConstString.ItemPrefabKey].ToString(), out prefabIdx);
        }

        return prefabIdx;
    }

    Bounds GetItemBounds(int index)
    {
        if (m_datas.Count <= 0 || index >= m_datas.Count)
        {
            return new Bounds();
        }
        else
        {
            return new Bounds(GetItemPos(index) + GetRealItemOffset(index) + content.localPosition, GetItemSize(m_datas[index]));
        }
    }

    Vector3 GetItemSize(Dictionary<string, object> data)
    {
        Vector3 size = Vector3.zero;
        size = m_itemSizes[GetItemPrefabIndex(data)];
        if (vertical)
        {
            size.x = 0;
        }
        else
        {
            size.y = 0;
        }

        return size;
    }

    Vector3 GetItemPos(int index)
    {
        Vector3 offset = Vector3.zero;
        Vector3 recordOffset = Vector3.zero;

        if (m_isInversion)
        {
            offset *= -1;
        }

        if (m_itemPrefabs.Length > 1)
        {
            for (int i = 0; i < index; i++)
            {
                var cellData = m_datas[i];
                if (cellData == null) continue;
                int prefabIndex = GetItemPrefabIndex(cellData);
                if (vertical)
                {
                    recordOffset.y -= m_itemSizes[prefabIndex].y;
                }
                if (horizontal)
                {
                    recordOffset.x += m_itemSizes[prefabIndex].x;
                }
            }
            offset = recordOffset;
        }
        else
        {
            if (vertical)
            {
                offset.y = -m_itemSizes[0].y * index;
            }

            if (horizontal)
            {
                offset.x = m_itemSizes[0].x * index;
            }
        }


        return offset;
    }

    Vector3 GetRealItemOffset(int index)
    {
        if (m_datas.Count <= 0) return Vector3.zero;
        Vector3 offset;
        if (vertical)
        {
            offset = new Vector3(0, -GetItemSize(m_datas[index]).y / 2, 0);
        }
        else
        {
            offset = new Vector3(GetItemSize(m_datas[index]).x / 2, 0, 0);
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

    #endregion

    #region 重载方法
    public ScrollRect m_ParentScrollRect;
    //滑动方向
    private Direction m_Direction = Direction.Vertical;
    //当前操作方向
    private Direction m_BeginDragDirection = Direction.Vertical;
    public override void OnBeginDrag(PointerEventData eventData)
    {
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
        if (m_isDragEvent)
        {
            // EventManager.DispatchEvent(m_eventSource, EventType.Event_Reusing_Scroll_Rect_Begin_Drag, eventData);
        }
        base.OnBeginDrag(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (m_ParentScrollRect)
        {
            if (m_BeginDragDirection != m_Direction)
            {
                //当前操作方向不等于滑动方向，将事件传给父对象
                ExecuteEvents.Execute(m_ParentScrollRect.gameObject, eventData, ExecuteEvents.dragHandler);
                return;
            }
        }
        if (m_isDragEvent)
        {
            // EventManager.DispatchEvent(m_eventSource, EventType.Event_Reusing_Scroll_Rect_Drag, eventData);
        }
        base.OnDrag(eventData);
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (m_ParentScrollRect)
        {
            if (m_BeginDragDirection != m_Direction)
            {
                //当前操作方向不等于滑动方向，将事件传给父对象
                ExecuteEvents.Execute(m_ParentScrollRect.gameObject, eventData, ExecuteEvents.endDragHandler);
                return;
            }
        }
        if (m_isDragEvent)
        {
            // EventManager.DispatchEvent(m_eventSource, EventType.Event_Reusing_Scroll_Rect_End_Drag, eventData);
        }
        base.OnEndDrag(eventData);
    }

    public override void OnScroll(PointerEventData eventData)
    {
        if (m_ParentScrollRect)
        {
            if (m_BeginDragDirection != m_Direction)
            {
                //当前操作方向不等于滑动方向，将事件传给父对象
                ExecuteEvents.Execute(m_ParentScrollRect.gameObject, eventData, ExecuteEvents.scrollHandler);
                return;
            }
            if (m_isDragEvent)
            {
                //当前操作方向不等于滑动方向，将事件传给父对象
                ExecuteEvents.Execute(m_ParentScrollRect.gameObject, eventData, ExecuteEvents.beginDragHandler);
            }
        }
        base.OnScroll(eventData);
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
        public int resIndex;
        public ReusingStatus status;
    }

    enum ReusingStatus
    {
        Show,
        Hide,
    }

    #endregion
}
