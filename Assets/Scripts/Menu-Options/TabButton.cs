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

    public Image background;
    public TextMeshProUGUI text;
    GameObject notification;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ReadAndPlaySFX("menu_click");
        }
        tabGroup.OnTabSelected(this);
        if (notification!=null)
        {
            notification.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabGroup.OnTabEnter(this);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
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
    public GameObject Notification
    {
        get{return notification; }
        set { notification = value; }
    }

}
