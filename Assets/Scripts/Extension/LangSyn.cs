using Managers;
using UnityEngine;
using UnityEngine.UI;

public class LangSyn : MonoBehaviour
{
    public string langKey = "";
    Text langText;
    private bool EventAdded;
    [HideInInspector]
    public bool isInit;
    private void Awake()
    {
        TryGetComponent<Text>(out langText);
        EventAdded = false;
        isInit = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        // if (!LocalizationManager.LangInitDone)
        // {
        //     EventAdded = true;
        //     langText.text = "";
        //     EventManager.AddEvent(null, EventType.On_Language_File_Init_Complete, RefreshText);
        //     return;
        // }

        RefreshText(null);
    }

    public void RefreshText(object[] args)
    {
        if (!string.IsNullOrEmpty(langKey) && langText != null)
        {
            // langText.text = LanguageHelper.GetValue(langKey).Replace("\\n", "\n");
            isInit = true;
        }
    }

    public void ManualRefreshText()
    {
        TryGetComponent<Text>(out langText);
        RefreshText(null);
    }

    private void OnDestroy()
    {
        // if (EventAdded)
            // EventManager.RemoveEvent(null, EventType.On_Language_File_Init_Complete, RefreshText);
    }
}
