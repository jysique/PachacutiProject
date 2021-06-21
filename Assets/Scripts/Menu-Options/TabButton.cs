using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private TabGroup tabGroup;

    public Image background;
    public TextMeshProUGUI text;

    public void OnPointerClick(PointerEventData eventData)
    {
        tabGroup.OnTabSelected(this);

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

    private void Update()
    {
        
    }
}
