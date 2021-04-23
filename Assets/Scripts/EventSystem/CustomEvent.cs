using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class CustomEvent
{
    [SerializeField] private string eventtype;
    [SerializeField] private string messagge;
    [SerializeField] private string territoryEvent;
    [SerializeField] private string element;
    [SerializeField] private int acceptCostEvent;
    [SerializeField] private int declineCostEvent;
    [SerializeField] private string acceptMessageEvent;
    [SerializeField] private string declineMessageEvent;
    [SerializeField] private TimeSimulated timeInit;
    [SerializeField] private TimeSimulated timeFinal;
    private int daysToFinal;
    private bool acceptEventBool;
    
    public string EventType
    {
        get { return eventtype; }
        set { eventtype = value; }
    }
    public string MessageEvent
    {
        get { return messagge; }
        set { messagge = value; }
    }
    public string TerritoryEvent
    {
        get { return territoryEvent; }
        set { territoryEvent = value; }
    }
    public string ElementEvent
    {
        get { return element; }
        set { element = value; }
    }
    public string AcceptMessageEvent
    {
        get { return acceptMessageEvent; }
        set { acceptMessageEvent = value; }
    }
    public string DeclineMessageEvent
    {
        get { return declineMessageEvent; }
        set { declineMessageEvent = value; }
    }
    public TimeSimulated TimeInitEvent
    {
        get { return timeInit; }
        set { timeInit = value; }
    }
    public TimeSimulated TimeFinalEvent
    {
        get { return timeFinal; }
        set { timeFinal = value; }
    }
    public int DaysToFinal
    {
        get { return daysToFinal; }
        set { daysToFinal = value; }
    }
    public bool AcceptEventBool
    {
        get { return acceptEventBool; }
    }
    public bool IsTimeEventType() {
        return timeInit.EqualsDate(timeFinal);
    }
    public void GetCustomEvent(TimeSimulated _initTime, TimeSimulated _finalTime, int _days)
    {
        this.timeInit = _initTime;
        this.timeFinal = _finalTime;
        this.daysToFinal = _days;
        this.acceptEventBool = false;
        EVENTTYPE _t = (EVENTTYPE)UnityEngine.Random.Range(0, 4);
        this.eventtype = _t.ToString();
        this.acceptCostEvent = UnityEngine.Random.Range(3, 20);
        this.declineCostEvent = UnityEngine.Random.Range(3, 20);
        GetMessage();
    }
    public void GetMessage()
    {
        string option = this.eventtype.ToString();
        switch (option)
        {
            case string a when a.Contains("REBELION"):
                this.messagge = "Nos rebelamos ante ti ";
                this.element = "se rebelaran contra ti.";
                this.declineMessageEvent = "-1 territorio";
                this.acceptMessageEvent = "-" + acceptCostEvent + " oro";
                break;
            case string a when a.Contains("DISCONTENT"):
                this.messagge = "Estamos muy disgustados";
                this.element = "estan muy disgustados.";
                declineCostEvent = acceptCostEvent*2;
                this.declineMessageEvent = "-"+ declineCostEvent +" oro";
                this.acceptMessageEvent = "-" + acceptCostEvent + " oro";
                break;
            case string a when a.Contains("PETITION"):
                this.messagge = "Queremos pedirte algo gobernante";
                this.element = "quieren pedirte algo.";
                switch (option)
                {
                    case string b when b.Contains("MIN"):
                        this.messagge += "\nEncontramos restos de oro";
                        this.declineMessageEvent = " Nada";
                        this.acceptMessageEvent = "-" + acceptCostEvent + " oro " + "\n +2 velocidad de mina";
                        break;
                    case string b when b.Contains("FOR"):
                        this.messagge += "\nQueremos mejorar nuesta defensa";
                        this.declineMessageEvent = " Nada";
                        this.acceptMessageEvent = "-" + acceptCostEvent + " oro " + "\n +2 defensa";
                        break;
                    default:
                        break;
                }
                break;
            case string a when a.Contains("GRACE"):
                this.messagge = "Encontramos algo que podria interesarte";
                this.element = "estan por encontrar algo interesante.";
                switch (option)
                {
                    case string b when b.Contains("DIV"):
                        this.messagge += "\nEncontramos un sitio sagrado";
                        this.declineMessageEvent = " Nada";
                        this.acceptMessageEvent = "+" + acceptCostEvent + " oro ";
                        break;
                    case string b when b.Contains("MIN"):
                        this.messagge += "\nEncontramos una mina";
                        this.declineMessageEvent = " Nada";
                        this.acceptMessageEvent = "+1 velocidad de mina";
                        break;
                    case string b when b.Contains("FOOD"):
                        this.messagge += "\nNuestro dios nos a favorecio";
                        this.declineMessageEvent = " Nada";
                        this.acceptMessageEvent = " +2 velocidad de comida";
                        break;
                    default:
                        break;
                }
                break;
            case null:
                break;
        }
    }
    public enum EVENTTYPE
    {
        REBELION,
        // mensaje : nos rebelamos antes ti 
        // territorioEvent: territorio propio
        // accept  : -oro
        // decline : -territorio
        DISCONTENT,
        // mensaje : estamos muy disgustados
        // territorioEvent: territorio propio
        // accept  : -oro
        // decline : -2xoro
        PETITION_MIN,
        // mensaje : queremos pedirte algo gobernante
        // territorioEvent: territorio propio
        // accept  : -oro +1 
        // decline : nada
        PETITION_FOR,
        // mensaje : queremos pedirte algo gobernante
        // territorioEvent: territorio propio
        // accept  :+1 defensa
        // decline :nada
        GRACE_DIV,
        // mensaje : encontramos algo que podria interesarte, 
        // territorioEvent: territorio propio
        // aceptar : +oro   => deberia ser fe pero para hacer pruebas
        // decline : nada
        GRACE_MIN,
        // mensaje : encontramos algo que podria interesarte, 
        // territorioEvent: territorio propio
        // aceptar : +1 velocidad de mina
        // decline : +oro
        GRACE_FOOD
        // mensaje : encontramos algo que podria interesarte, 
        // territorioEvent: territorio propio
        // aceptar : +1 velocidad de mina
        // decline : +oro
    }
    public void AcceptEventAction()
    {
        acceptEventBool = true;
        switch (eventtype.ToString())
        {
            case "REBELION":
                InGameMenuHandler.instance.GoldPlayer -= acceptCostEvent;
                break;
            case "DISCONTENT":
                InGameMenuHandler.instance.GoldPlayer -= acceptCostEvent;
                break;
            case "PETITION_MIN":
                InGameMenuHandler.instance.GoldPlayer -= acceptCostEvent;
                TerritoryManager.instance.SearchTerritoryByName(territoryEvent).territoryStats.territory.GoldMineTerritory.VelocityGold += 2;
                break;
            case "PETITION_FOR":
                InGameMenuHandler.instance.GoldPlayer -= acceptCostEvent;
                TerritoryManager.instance.SearchTerritoryByName(territoryEvent).territoryStats.territory.FortressTerritory.PlusDefense += 2;
                break;
            case "GRACE_DIV":
                InGameMenuHandler.instance.GoldPlayer += acceptCostEvent;
                break;
            case "GRACE_MIN":
                TerritoryManager.instance.SearchTerritoryByName(territoryEvent).territoryStats.territory.GoldMineTerritory.VelocityGold += 2;
                break;
            case "GRACE_FOOD":
                TerritoryManager.instance.SearchTerritoryByName(territoryEvent).territoryStats.territory.VelocityFood += 2;
                break;
            default:
                break;
        }
        if (InGameMenuHandler.instance.GoldPlayer < 0)
        {
            DeclineEventAction();
        }
    }
    public void DeclineEventAction()
    {
        switch (eventtype.ToString())
        {
            case "REBELION":
                TerritoryManager.instance.ChangeTerritoryToType(territoryEvent, Territory.TYPEPLAYER.NONE);
                break;
            case "DISCONTENT":
                InGameMenuHandler.instance.GoldPlayer -= declineCostEvent;
                break;
            case "PETITION_MIN":
            case "PETITION_FOR":
            case "GRACE_DIV":
            case "GRACE_MIN":
            case "GRACE_FOOD":
                break;
            default:
                break;
        }
        if(InGameMenuHandler.instance.GoldPlayer< 0)
        {
            InGameMenuHandler.instance.GoldPlayer = 0;
        }
    }

}
