using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private TabGroup tabGroup;
    private bool isOpen;
    public Image background;
    public TextMeshProUGUI text;

    public bool IsOpen
    {
        get { return isOpen; }
        set { isOpen = value; }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ReadAndPlaySFX("menu_click");
            
        }
        InGameMenuHandler.instance.UpdateMenu();
        tabGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);
        isOpen = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
        isOpen = false;
    }

    private void Awake()
    {
        background = GetComponent<Image>();
        text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        tabGroup.Subscribe(this);
    }

    public void AccessToMenu()
    {
        tabGroup.OnTabSelected(this);
        tabGroup.OnTabEnter(this);
    }

}
