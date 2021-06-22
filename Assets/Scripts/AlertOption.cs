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
        anim.speed = GlobalVariables.instance.timeModifier;
        timeInit = new TimeSimulated(TimeSystem.instance.TimeGame);
        timeInit.PlusDays(3);
        closeAlertBtn.onClick.AddListener(() => CloseAlertBtn());
        alertBtn.onClick.AddListener(() => OpenTabEvent());
    }
    public void Init(string type,string iconId,TerritoryHandler focus= null)
    {
        this.type = type;
        tittleAlertText.text = GameMultiLang.GetTraduction(type + "_TITLE");
        suggertAlertText.text = GameMultiLang.GetTraduction(type + "_SUGG");
        focusTerritory = focus;
    }
    void CloseAlertBtn()
    {
        anim.SetBool("Appeance", false);
        anim.speed = GlobalVariables.instance.timeModifier;
        Destroy(this.gameObject,1+GlobalVariables.instance.timeModifier);
    }
    void OpenTabEvent()
    {
        CloseAlertBtn();
        switch (type)
        {
            case "ALERT1":
            case "ALERT2":
            case "ALERT3":
                AlertManager.TabEventMenu();
                EventManager.instance.SetNotificationEvent(false);
                break;
            case "ALERT4":
                AlertManager.TabMissionMenu();
                MissionManager.instance.SetNotificationMission(false);
                break;
            case "ALERT5":
                GlobalVariables.instance.CenterCameraToTerritory(focusTerritory);
                focusTerritory.MakeOutline();
                AlertManager.TabEventMenu();
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
