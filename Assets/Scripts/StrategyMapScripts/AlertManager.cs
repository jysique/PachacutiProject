using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlertManager : MonoBehaviour
{
    private static AlertManager instance;
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
        if (alertContainer == null)
        {
            alertContainer = this.gameObject;
        }
    }
    void Start()
    { 

    }
    public static void NewAlert(string alerttextId, string alertIconId = "", TerritoryHandler focusTerritory= null,string textToAdd=null)
    {
      //  print("A|"+alerttextId);
        GameObject alertGo = (GameObject)Instantiate(instance.alertOption);
        alertGo.transform.SetParent(instance.alertContainer.transform);
        alertGo.GetComponent<AlertOption>().Init(alerttextId,alertIconId,focusTerritory);
        alertGo.transform.localScale = Vector3.one;
    }
    public static void NewAlertText(string alerttextID,string textToAdd = null)
    {
        NewAlert("Alert_"+ alerttextID,"",null,textToAdd);
    }
    public static void NewAlertTerrytoryHandler(string alerttextID, TerritoryHandler focusObject)
    {
        NewAlert("Alert_" + alerttextID, "", focusObject);
    }
    public static void AlertEvent()
    {
        NewAlertText("NewEvent");
    }
    public static void AlertExpedition()
    {
        NewAlertText("NewExped");
    }
    public static void AlertConquered(TerritoryHandler territoryfocus)
    {
        NewAlertTerrytoryHandler("NewConq", territoryfocus);
    }
    public static void AlertEventEnd()
    {
        NewAlertText("EndEvent");
    }

    public static void AlertLimitBuilding(string territory)
    {
        NewAlertText("LimitBuilding",territory);
    }
    public static void AlertMission()
    {
        NewAlertText("NewMission");
    }
    public static void AlertLost(TerritoryHandler territoryfocus)
    {
        NewAlertTerrytoryHandler("LostTerr", territoryfocus);
    }
}
