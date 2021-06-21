using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlertManager : MonoBehaviour
{
    private static AlertManager instance;
    [SerializeField] private TabButton tabEvent;
    [SerializeField] private TabButton tabMission;
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

    public static void NewAlert(string alerttextId, string alertIconId = "", GameObject focusObject = null)
    {
        string alertTitle = GameMultiLang.GetTraduction(alerttextId+ "_TITLE");
        string alertSuggest = GameMultiLang.GetTraduction(alerttextId + "_SUGG");
        GameObject alertGo =  (GameObject)Instantiate(instance.alertOption);
        alertGo.transform.SetParent(instance.alertContainer.transform);
        alertGo.GetComponent<AlertOption>().Init(alertTitle, alertSuggest,alerttextId);
        alertGo.transform.localScale= Vector3.one;
    }
    public static void NewAlert(string alerttextID,GameObject focusObject)
    {
        NewAlert(alerttextID,"",focusObject);
    }
    public static void AlertEvent()
    {
        NewAlert("ALERT1");
    }
    public static void AlerMission()
    {
        NewAlert("ALERT2");
    }
    public static void AlertConquered()
    {
        NewAlert("ALERT3");
    }
    public static void AlertLost()
    {
        NewAlert("ALERT4");
    }
    public static void TabEventMenu()
    {
        instance.tabEvent.AccessToMenu();
    }
    public static void TabMissionMenu()
    {
        instance.tabMission.AccessToMenu();
    }
}
