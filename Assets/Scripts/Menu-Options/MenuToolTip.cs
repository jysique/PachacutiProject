using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject toolTip;
    [SerializeField] string NewInfo;
    private TextMeshProUGUI info;
    private RectTransform background;
    private RectTransform canvasRectTransform;
    private bool preventStart = false;

    private bool canShow = false;
    private int temp = 0;
    private int timeToStart = 40;

    public void SetNewInfo(string info)
    {
        NewInfo = info;
    }

    private void Start()
    {
        NewInfo = NewInfo.Replace("\\n", "\n");
        canvasRectTransform = InGameMenuHandler.instance.canvas.GetComponent<RectTransform>();
        toolTip = InGameMenuHandler.instance.toolTip;
        preventStart = true;
        background = toolTip.transform.GetChild(0).GetComponent<RectTransform>();
        info = toolTip.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        toolTip.SetActive(true);
        canShow = true;
        info.SetText(NewInfo);
        info.ForceMeshUpdate();
        toolTip.SetActive(false);

    }
    private void ShowTooltip()
    {
        
        Vector2 textSize = info.GetRenderedValues(false);
//        print(info.text);

        Vector2 paddingSize = new Vector2(20, 20);
        background.sizeDelta = textSize + paddingSize;
        toolTip.SetActive(true);


    }
    private void FixedUpdate()
    {

        Vector3 mousePosCamera = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        toolTip.transform.position = new Vector3(mousePosCamera.x, mousePosCamera.y, toolTip.transform.position.z);

        if (canShow)
        {
           
            temp += 1;
            if(temp == timeToStart)
            {
                ShowTooltip();
                canShow = false;
                temp = 0;
            }
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        canShow = false;
        temp = 0;
        toolTip.SetActive(false);
    }
    private void OnDisable()
    {
        if(preventStart)toolTip.SetActive(false);
    }
}
