using System.Collections.Generic;
using UnityEngine;

public class UIStackManager : MonoBehaviour
{
    public List<UIWindowBase> m_sceneUIStack = new List<UIWindowBase>();
    public List<UIWindowBase> m_hudStack = new List<UIWindowBase>();
    public List<UIWindowBase> m_dialogStack = new List<UIWindowBase>();
    public List<UIWindowBase> m_popupStack = new List<UIWindowBase>();
    public List<UIWindowBase> m_tipStack = new List<UIWindowBase>();
    public List<UIWindowBase> m_blockerStack = new List<UIWindowBase>();

    public void OnUIOpen(UIWindowBase ui)
    {
        switch (ui.m_UIType)
        {
            case UIType.SceneUI: m_sceneUIStack.Add(ui); break;
            case UIType.Hud: m_hudStack.Add(ui); break;
            case UIType.Dialog: m_dialogStack.Add(ui); break;
            case UIType.Popup: m_popupStack.Add(ui); break;
            case UIType.Tip: m_tipStack.Add(ui); break;
            case UIType.Blocker: m_blockerStack.Add(ui); break;
        }
    }

    public void OnUIClose(UIWindowBase ui)
    {
        switch (ui.m_UIType)
        {
            case UIType.SceneUI: m_sceneUIStack.Remove(ui); break;
            case UIType.Hud: m_hudStack.Remove(ui); break;
            case UIType.Dialog: m_dialogStack.Remove(ui); break;
            case UIType.Popup: m_popupStack.Remove(ui); break;
            case UIType.Tip: m_tipStack.Remove(ui); break;
            case UIType.Blocker: m_blockerStack.Remove(ui); break;
        }
    }

    public UIWindowBase GetLastUI(UIType uiType)
    {
        switch (uiType)
        {
            case UIType.SceneUI:
                if (m_sceneUIStack.Count > 0)
                    return m_sceneUIStack[m_sceneUIStack.Count - 1];
                else
                    return null;
            case UIType.Hud:
                if (m_hudStack.Count > 0)
                    return m_hudStack[m_hudStack.Count - 1];
                else
                    return null;
            case UIType.Dialog:
                if (m_dialogStack.Count > 0)
                    return m_dialogStack[m_dialogStack.Count - 1];
                else
                    return null;
            case UIType.Popup:
                if (m_popupStack.Count > 0)
                    return m_popupStack[m_popupStack.Count - 1];
                else
                    return null;
            case UIType.Tip:
                if (m_tipStack.Count > 0)
                    return m_tipStack[m_tipStack.Count - 1];
                else
                    return null;
            case UIType.Blocker:
                if (m_blockerStack.Count > 0)
                    return m_blockerStack[m_blockerStack.Count - 1];
                else
                    return null;
        }

        throw new System.Exception("CloseLastUIWindow does not support GameUI");
    }

    public List<UIWindowBase> GetAllUI(UIType uiType)
    {
        List<UIWindowBase> uiList;
        switch (uiType)
        {
            case UIType.SceneUI:
                uiList = m_sceneUIStack;
                break;
            case UIType.Hud:
                uiList = m_hudStack;
                break;
            case UIType.Dialog:
                uiList = m_dialogStack;
                break;
            case UIType.Popup:
                uiList = m_popupStack;
                break;
            case UIType.Tip:
                uiList = m_tipStack;
                break;
            case UIType.Blocker:
                uiList = m_blockerStack;
                break;
            default:
                uiList = new List<UIWindowBase>();
                break;
        }

        return uiList;
    }

    public int GetAllDialogUICount()
    {
        return m_dialogStack.Count;
    }

    public int GetAllPopupUICount()
    {
        return m_popupStack.Count;
    }

    int count = 0;
    public int GetAllActiveUICount(UIType uiType, bool isCheckFullScreen = false)
    {
        count = 0;
        switch (uiType)
        {
            case UIType.SceneUI:
                if (m_sceneUIStack == null) return count;
                if (m_sceneUIStack.Count > 0)
                {
                    for (int i = 0; i < m_sceneUIStack.Count; i++)
                    {
                        if (m_sceneUIStack[i] == null) continue;
                        if (m_sceneUIStack[i].gameObject.activeSelf) count++;
                    }
                    return count;
                }
                break;
            case UIType.Hud:
                if (m_hudStack == null) return count;
                if (m_hudStack.Count > 0)
                {
                    for (int i = 0; i < m_hudStack.Count; i++)
                    {
                        if (m_hudStack[i] == null) continue;
                        if (m_hudStack[i].gameObject.activeSelf) count++;
                    }
                    return count;
                }
                break;
            case UIType.Dialog:
                if (m_dialogStack == null) return count;
                if (m_dialogStack.Count > 0)
                {
                    for (int i = 0; i < m_dialogStack.Count; i++)
                    {
                        if (m_dialogStack[i] == null) continue;
                        if (m_dialogStack[i].gameObject == null) continue;
                        if (m_dialogStack[i].gameObject.activeSelf)
                        {
                            if (isCheckFullScreen)
                            {
                                if (m_dialogStack[i].isFullScreen)
                                {
                                    count++;
                                }
                            }
                        }
                    }
                    return count;
                }
                break;
            case UIType.Popup:
                if (m_popupStack == null) return count;
                if (m_popupStack.Count > 0)
                {
                    for (int i = 0; i < m_popupStack.Count; i++)
                    {
                        if (m_popupStack[i] == null) continue;
                        if (m_popupStack[i].gameObject.activeSelf) count++;
                    }
                    return count;
                }
                break;
            case UIType.Tip:
                if (m_tipStack == null) return count;
                if (m_tipStack.Count > 0)
                {
                    for (int i = 0; i < m_tipStack.Count; i++)
                    {
                        if (m_tipStack[i] == null) continue;
                        if (m_tipStack[i].gameObject.activeSelf) count++;
                    }
                    return count;
                }
                break;
            case UIType.Blocker:
                if (m_blockerStack == null) return count;
                if (m_blockerStack.Count > 0)
                {
                    for (int i = 0; i < m_blockerStack.Count; i++)
                    {
                        if (m_blockerStack[i] == null) continue;
                        if (m_blockerStack[i].gameObject.activeSelf) count++;
                    }
                    return count;
                }
                break;
            default:
                break;
        }

        return count;
    }

    public void Clear()
    {
        m_sceneUIStack.Clear();
        m_hudStack.Clear();
        m_dialogStack.Clear();
        m_popupStack.Clear();
        m_tipStack.Clear();
        m_blockerStack.Clear();
    }
}
