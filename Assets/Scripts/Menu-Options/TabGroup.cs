using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    [SerializeField] private List<TabButton> tabButtons;
    private Color tabHoverColor;
    private Color tabSelectedColor;
    private Color tabIdleColor;
    private Color textColor;
    [SerializeField] private TabButton selectedTab;
    [SerializeField] private List<GameObject> objectsToSwap;
    private void Start()
    {
        tabHoverColor = new Color32(66, 65, 61, 255);
        tabIdleColor = new Color32(204, 178, 126, 255);
        tabSelectedColor = new Color32(66, 65, 61, 255);
        textColor = new Color32(50,50,50,255);
        selectedTab.background.color = tabSelectedColor;
        selectedTab.text.color = Color.white;
        int index = selectedTab.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if (i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }

        }
    }

    public void Subscribe(TabButton button)
    {
        if(tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }
        tabButtons.Add(button);
    }
    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if(selectedTab == null||button != selectedTab)
        {
            button.background.color = tabHoverColor;
        }
        
    }
    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }
    public void OnTabSelected(TabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.background.color = tabSelectedColor;
        button.text.color = Color.white;
        int index = button.transform.GetSiblingIndex();
        for (int i = 0; i < objectsToSwap.Count; i++)
        {
            if(i == index)
            {
                objectsToSwap[i].SetActive(true);
            }
            else
            {
                objectsToSwap[i].SetActive(false);
            }
            
        }
    }

    public void ResetTabs()
    {
        foreach(TabButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab) { continue; }
            button.background.color = tabIdleColor;
            button.text.color = textColor;
        }
    }

}
