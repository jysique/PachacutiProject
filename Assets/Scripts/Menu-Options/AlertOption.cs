using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AlertOption : MonoBehaviour
{
    [SerializeField] Button closeAlertBtn;
    [SerializeField] Button alertBtn;
    [SerializeField] TextMeshProUGUI tittleAlertText;
    [SerializeField] TextMeshProUGUI suggertAlertText;
    [SerializeField] GameObject container;
    private string type;
    Animator anim;
    TimeSimulated timeInit;
    TerritoryHandler focusTerritory;
    void Start()
    {
        anim = container.GetComponent<Animator>();
        timeInit = new TimeSimulated(TimeSystem.instance.TimeGame);
        timeInit.PlusDays(3);
        closeAlertBtn.onClick.AddListener(() => CloseAlertBtn());
        alertBtn.onClick.AddListener(() => OpenTabEvent());
    }
    public void Init(string type,string iconId,TerritoryHandler focus= null,string textToAdd = "")
    {
        this.type = type;
        tittleAlertText.text = GameMultiLang.GetTraduction(type + "_title");
        suggertAlertText.text = GameMultiLang.GetTraduction(type + "_sugg");
        if (textToAdd.Length >0 )
        {
            suggertAlertText.text = suggertAlertText.text.Replace("TERRITORY", textToAdd);
        }
        focusTerritory = focus;
    }
    void CloseAlertBtn()
    {
        anim.SetBool("Appeance", false);
        anim.speed = GlobalVariables.instance.timeModifier + 1.5f;
        Destroy(this.gameObject,GlobalVariables.instance.timeModifier);
    }
    void OpenTabEvent()
    {
        CloseAlertBtn();
        switch (type)
        {
            case "Alert_NewConq":
                MenuManager.instance.AccessTabEvent();
                EventManager.instance.SetNotificationEvent(false);
                GlobalVariables.instance.CenterCameraToTerritory(focusTerritory,true);
                break;
            case "Alert_NewEvent":
            case "Alert_EndEvent":
            case "Alert_NewExped":
                MenuManager.instance.AccessTabEvent();
                EventManager.instance.SetNotificationEvent(false);
                break;
            case "Alert_NewMission":
                MenuManager.instance.AccessTabMission();
                MissionManager.instance.ScrollToBottom();
                MissionManager.instance.SetNotificationMission(false);

                break;
            case "Alert_LostTerr":
                GlobalVariables.instance.CenterCameraToTerritory(focusTerritory,false);
                break;
            default:
                break;
        }
        
    }
    private void Update()
    {
        CloseAlertByTime();
        anim.speed = GlobalVariables.instance.timeModifier + 1.5f;
    }
    void CloseAlertByTime()
    {
        if (timeInit.EqualsDate(TimeSystem.instance.TimeGame))
        {
            CloseAlertBtn();
        }
    }

}
