using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonCustomize : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject label;
    public TYPECUSTOM typecustom;
    private void Start()
    {
        typecustom = gameObject.name.Contains("Buttom") ? TYPECUSTOM.BUTTON : TYPECUSTOM.ICON;
        label.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        label.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        label.SetActive(false);
    }
    private void Update()
    {
        InstantiateFloatingText();
    }
    private void InstantiateFloatingText()
    {
        if (typecustom == TYPECUSTOM.BUTTON)
        {
            if (InGameMenuHandler.instance.GoldPlayer >= InGameMenuHandler.instance.GoldNeedSpeed)
            {
                label.GetComponent<Text>().text = "Costo -"+ InGameMenuHandler.instance.GoldNeedSpeed + "g";
            }
            else
            {
                label.GetComponent<Text>().text = "No disponible";
            }
        }
    }
    public enum TYPECUSTOM
    {
        BUTTON,
        ICON
    }
}
