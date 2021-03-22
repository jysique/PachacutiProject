using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabGroup : MonoBehaviour
{
    [SerializeField] private List<TabButton> tabButtons;
    private Color tabHoverColor;
    private Color tabSelectedColor;
    private Color tabIdleColor;
    [SerializeField] private TabButton selectedTab;
    [SerializeField] private List<GameObject> objectsToSwap;
    private void Start()
    {
        tabHoverColor = Color.gray;
        tabIdleColor = Color.white;
        tabSelectedColor = Color.cyan;
        selectedTab.background.color = tabSelectedColor;
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
        }
    }

}
