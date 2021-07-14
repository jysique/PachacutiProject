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
    }
    void Start()
    { 
        if(alertContainer == null)
        {
            alertContainer = this.gameObject;
        }
    }
    public static void NewAlert(string alerttextId, string alertIconId = "", TerritoryHandler focusTerritory= null)
    {
      //  print("A|"+alerttextId);
        GameObject alertGo = (GameObject)Instantiate(instance.alertOption);
        alertGo.transform.SetParent(instance.alertContainer.transform);
        alertGo.GetComponent<AlertOption>().Init(alerttextId,alertIconId,focusTerritory);
        alertGo.transform.localScale = Vector3.one;
    }
    public static void NewAlert(string alerttextID)
    {
        NewAlert("Alert_"+ alerttextID,"");
    }
    public static void NewAlert(string alerttextID, TerritoryHandler focusObject)
    {
        NewAlert("Alert_" + alerttextID, "", focusObject);
    }
    public static void AlertEvent()
    {
        NewAlert("NewEvent");
    }
    public static void AlertExpedition()
    {
        NewAlert("NewExped");
    }
    public static void AlertConquered(TerritoryHandler territoryfocus)
    {
        NewAlert("NewConq", territoryfocus);
    }
    public static void AlertEventEnd()
    {
        NewAlert("EndEvent");
    }
    public static void AlertMission()
    {
        NewAlert("NewMission");
    }
    public static void AlertLost(TerritoryHandler territoryfocus)
    {
        NewAlert("LostTerr", territoryfocus);
    }
}
