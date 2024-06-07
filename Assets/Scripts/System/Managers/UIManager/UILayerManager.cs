using UnityEngine;
using System;
using System.Collections.Generic;

public class UILayerManager : MonoBehaviour 
{
    public List<UICameraData> UICameraList = new List<UICameraData>();
    public int[] m_UIEffectOrderNums = new int[6];
    public UICameraData uiCamData;

    public void Awake()
    {
        for (int i = 0; i < UICameraList.Count; i++)
        {
            UICameraData data = UICameraList[i];

            //data.m_root.transform.localPosition = new Vector3(0, 0, i * -2000);

            if (data.m_root == null)
            {
                LogUtil.Error("UILayerManager :Root is null! " + " key : " + data.m_key + " index : " + i);
            }

            if (data.m_camera == null)
            {
                LogUtil.Error("UILayerManager :Camera is null! " + " key : " + data.m_key + " index : " + i);
            }
        }

        int enumIdx = 0;
        foreach (int myCode in Enum.GetValues(typeof(UIEffectOrder)))
        {
            m_UIEffectOrderNums[enumIdx] = myCode;
            enumIdx++;
        }

    }

    public void SetCurUseCam(int index)
    {
        uiCamData = UICameraList[index];
    }

    public void SetLayerState(UIType uiType, bool state)
    {
        if (uiCamData.m_uiLayerParents[uiType] != null)
        {
            //uiCamData.m_uiLayerParents[uiType].gameObject.SetActiveOptimize(state);
            uiCamData.m_uiLayerParents[uiType].gameObject.GetComponent<Canvas>().enabled = state;
        }
    }

	public void SetLayer(UIWindowBase ui,string cameraKey = null)
    {
        RectTransform rt = ui.GetComponent<RectTransform>();

        if (uiCamData.m_uiLayerParents[ui.m_UIType] != null)
        {
            ui.transform.SetParent(uiCamData.m_uiLayerParents[ui.m_UIType]);
        }

        if (m_UIEffectOrderNums[(int)ui.m_UIType] != ui.m_recordUIRenderOrder)
        {
            ui.m_recordUIRenderOrder = m_UIEffectOrderNums[(int)ui.m_UIType];
            ReductionRenderOrder(ui, cameraKey);
        }

        rt.localScale = Vector3.one;
        rt.sizeDelta = Vector2.zero;
        if (ui.m_UIType != UIType.SceneUI)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector3.one;

            rt.sizeDelta = Vector2.zero;
            rt.transform.localPosition = new Vector3(0, 0, ui.m_PosZ);
            rt.anchoredPosition3D = new Vector3(0, 0, ui.m_PosZ);
            rt.SetAsLastSibling();
        }
        else
        {
            rt.sizeDelta = Vector2.zero;
            rt.transform.localPosition = new Vector3(0, 0, ui.m_PosZ);
            rt.anchoredPosition3D = new Vector3(0, 0, ui.m_PosZ);
            rt.SetAsLastSibling();
        }
    }

    public void ResetUIOrder(UIWindowBase ui)
    {
        int enumIdx = 0;
        foreach (int order in Enum.GetValues(typeof(UIEffectOrder)))
        {
            if (enumIdx == (int)ui.m_UIType)
            {
                m_UIEffectOrderNums[(int)ui.m_UIType] = order;
                ReductionRenderOrder(ui, null, order);
                break;
            }
            enumIdx++;
        }
    }

    public void UpdateUIRenderOrder(UIWindowBase ui, int order = -1)
    {
        if (order > 0)
        {
            m_UIEffectOrderNums[(int)ui.m_UIType] = order;
        }
    }

    public void ReductionRenderOrder(UIWindowBase ui, string cameraKey = null, int order = -1)
    {

        Canvas canvas = uiCamData.m_uiLayerParents[ui.m_UIType].GetComponent<Canvas>();

        if (canvas != null)
        {
            if (!canvas.overrideSorting) canvas.overrideSorting = true;
            if (order > 0)
            {
                canvas.sortingOrder = order;
            }
            else
            {
                canvas.sortingOrder = ui.m_recordUIRenderOrder;
            }
        }
        
    }

    public UICameraData GetUICameraDataByKey(string key)
    {
        if(key == null || key == "")
        {
            if(UICameraList.Count > 0)
            {
                return UICameraList[0];
            }
            else
            {
                throw new System.Exception("UICameraList is null ! " + key);
            }
        }

        for (int i = 0; i < UICameraList.Count; i++)
        {
            if(UICameraList[i].m_key == key)
            {
                return UICameraList[i];
            }
        }

        throw new System.Exception("Dont Find UILayerData by " + key);
    }

    [System.Serializable]
    public struct UICameraData
    {
        public string m_key;
        public GameObject m_root;
        public Camera m_camera;
        public Dictionary<UIType, Transform> m_uiLayerParents;
    }
}
