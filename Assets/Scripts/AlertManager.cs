using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlertManager : MonoBehaviour
{
    private static AlertManager instance;
    [SerializeField] private TabButton tabEvent;
    [SerializeField] private TabButton tabMission;
    [SerializeField] private TabButton tabMilitar;
    [SerializeField] private GameObject alertOption;
    [SerializeField] private GameObject alertContainer;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    { 
        if(alertContainer == null)
        {
            alertContainer = this.gameObject;
        }
    }
    /*
    public static void NewAlert(string alerttextId, string alertIconId = "")
    {
        string alertTitle = GameMultiLang.GetTraduction(alerttextId+ "_TITLE");
        string alertSuggest = GameMultiLang.GetTraduction(alerttextId + "_SUGG");
        GameObject alertGo =  (GameObject)Instantiate(instance.alertOption);
        alertGo.transform.SetParent(instance.alertContainer.transform);
        alertGo.GetComponent<AlertOption>().Init(alertTitle, alertSuggest,alerttextId);
        alertGo.transform.localScale= Vector3.one;
    }
    */
    public static void NewAlert(string alerttextId, string alertIconId = "", TerritoryHandler focusTerritory= null)
    {
        string alertTitle = GameMultiLang.GetTraduction(alerttextId + "_TITLE");
        string alertSuggest = GameMultiLang.GetTraduction(alerttextId + "_SUGG");
        GameObject alertGo = (GameObject)Instantiate(instance.alertOption);
        alertGo.transform.SetParent(instance.alertContainer.transform);
        alertGo.GetComponent<AlertOption>().Init(alertTitle, alertSuggest, alerttextId,focusTerritory);
        alertGo.transform.localScale = Vector3.one;
    }


    public static void NewAlert(string alerttextID)
    {
        NewAlert(alerttextID,"");
    }
    public static void NewAlert(string alerttextID, TerritoryHandler focusObject)
    {
        NewAlert(alerttextID, "", focusObject);
    }
    public static void AlertEvent()
    {
        NewAlert("ALERT1");
    }
    public static void AlertMission()
    {
        NewAlert("ALERT2");
    }
    public static void AlertConquered(TerritoryHandler territoryfocus)
    {
        NewAlert("ALERT3",territoryfocus);
    }
    public static void AlertLost(TerritoryHandler territoryfocus)
    {
        NewAlert("ALERT4", territoryfocus);
    }
    public static void TabEventMenu()
    {
        instance.tabEvent.AccessToMenu();
    }
    public static void TabMissionMenu()
    {
        instance.tabMission.AccessToMenu();
    }
    public static void TabMilitarMenu()
    {
        instance.tabMilitar.AccessToMenu();
    }
}
