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
        anim.speed = GlobalVariables.instance.timeModifier+1.5f;
        timeInit = new TimeSimulated(TimeSystem.instance.TimeGame);
        timeInit.PlusDays(3);
        closeAlertBtn.onClick.AddListener(() => CloseAlertBtn());
        alertBtn.onClick.AddListener(() => OpenTabEvent());
    }
    public void Init(string type,string iconId,TerritoryHandler focus= null)
    {
        this.type = type;
        tittleAlertText.text = GameMultiLang.GetTraduction(type + "_title");
        suggertAlertText.text = GameMultiLang.GetTraduction(type + "_sugg");
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
            case "Alert_NewEvent":
            case "Alert_EndEvent":
                AlertManager.TabEventMenu();
                EventManager.instance.SetNotificationEvent(false);
                break;
            case "Alert_NewMission":
                AlertManager.TabMissionMenu();
                MissionManager.instance.SetNotificationMission(false);
                break;
            case "Alert_NewConq":
                AlertManager.TabEventMenu();
                EventManager.instance.SetNotificationEvent(false);
                GlobalVariables.instance.CenterCameraToTerritory(focusTerritory);
                focusTerritory.MakeOutline();
                break;
            case "Alert_LostTerr":
                GlobalVariables.instance.CenterCameraToTerritory(focusTerritory);
                break;
            default:
                break;
        }
        
    }
    private void Update()
    {
        CloseAlertByTime();
    }
    void CloseAlertByTime()
    {
        if (timeInit.EqualsDate(TimeSystem.instance.TimeGame))
        {
            CloseAlertBtn();
        }
    }

}
