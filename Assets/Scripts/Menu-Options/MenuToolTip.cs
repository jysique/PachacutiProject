using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject toolTip;
    [SerializeField] string NewInfo;
    private Text Info;
    private void Start()
    {
        toolTip = InGameMenuHandler.instance.toolTip;
        Info = toolTip.transform.GetChild(0).GetComponent<Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Info.text = NewInfo;
        Vector3 pos = Camera.main.ScreenToWorldPoint(eventData.position);
        toolTip.GetComponent<RectTransform>().position = new Vector2(pos.x,pos.y);
        toolTip.SetActive(true);

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        toolTip.SetActive(false);
    }
    
}
