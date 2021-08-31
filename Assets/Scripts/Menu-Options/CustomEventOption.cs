using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CustomEventOption : MonoBehaviour
{
    private CustomEvent custom;
    [SerializeField] Image Icon;
    [SerializeField] TextMeshProUGUI descriptionTxt;
    [SerializeField] TextMeshProUGUI territoryNameTxt;
    [SerializeField] TextMeshProUGUI typeEventTxt;
    [SerializeField] TextMeshProUGUI initTxt;
    [SerializeField] TextMeshProUGUI finishTxt;
    [SerializeField] Image estado;
    private Button btn;
    int diferenceDays;
    public CustomEvent Custom
    {
        get { return custom; }
        set { custom = value; }
    }
    private void Start()
    {
        btn = transform.GetComponent<Button>();
        btn.onClick.AddListener(() => OnClickCustomEvent());
        territoryNameTxt.text = custom.TerritoryEvent.name;
        typeEventTxt.text = custom.EventType.ToString().ToLower().Replace("_", " ");
      //  initTxt.text = custom.TimeInitEvent.PrintTimeSimulated();
      //  finishTxt.text = custom.TimeFinalEvent.PrintTimeSimulated();
        string[] splitArray = custom.MessageEvent.Split(char.Parse("."));
        descriptionTxt.text = splitArray[0];
        
    }
    private void Update()
    {
        if (custom.EventStatus == CustomEvent.STATUS.FINISH)
        {
            estado.color = new Color32(193, 39, 4,255);
            btn.interactable = false;
            EventManager.instance.listEvents.RemoveEvent(custom);
        }
        if (custom.EventType == CustomEvent.EVENTTYPE.EARTHQUAKER)
        {
            btn.interactable = false;
        }
    }
    void OnClickCustomEvent()
    {
        diferenceDays = custom.TimeFinalEvent.DiferenceDays(TimeSystem.instance.TimeGame);
        if (custom.EventType == CustomEvent.EVENTTYPE.CONQUIST)
        {
            GlobalVariables.instance.CenterCameraToTerritory(custom.TerritoryEvent,true);
            //custom.TerritoryEvent.MakeOutline();
            MenuManager.instance.AccessTabMilitar();
            InGameMenuHandler.instance.UpdateMenu();
            if (!MenuManager.instance.IsOpen)
            {
                MenuManager.instance.OpenMenu();
            }
        }
        else
        {
            EventManager.instance.WarningEventAppearance(custom, diferenceDays);
        }
        
    }
}
