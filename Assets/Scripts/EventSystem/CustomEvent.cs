using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class CustomEvent
{
    [SerializeField] private string eventtype;
    [SerializeField] private string messaggeA;
    [SerializeField] private string messaggeB;
    //[SerializeField] private string territoryEvent;
    [SerializeField] private Territory territoryEvent;
    [SerializeField] private string element;
    [SerializeField] private int acceptCostEvent;
    [SerializeField] private int declineCostEvent;
    [SerializeField] private string acceptMessageEvent;
    [SerializeField] private string declineMessageEvent;
    [SerializeField] private TimeSimulated timeInit;
    [SerializeField] private TimeSimulated timeFinal;
 //   private int daysToFinal;
    private bool acceptEventBool = false;
    
    public string EventType
    {
        get { return eventtype; }
        set { eventtype = value; }
    }
    public string MessageEventA //sin tiempo
    {
        get { return messaggeA; }
        set { messaggeA = value; }
    }
    public string MessageEventB //con tiempo
    {
        get { return messaggeB; }
        set { messaggeB = value; }
    }
    public Territory TerritoryEvent
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
    /*
    public int DaysToFinal
    {
        get { return daysToFinal; }
        set { daysToFinal = value; }
    }
    */
    public bool AcceptEventBool
    {
        get { return acceptEventBool; }
        set { acceptEventBool = value; }
    }

    public void PrintEvent(int i)
    {
        Debug.Log(i + " eventType - " + EventType.ToString());
        Debug.Log(i + " territorio- " + TerritoryEvent.name);
        Debug.Log(i + " init time- " + TimeInitEvent.PrintTimeSimulated());
        Debug.Log(i + " final time- " + TimeFinalEvent.PrintTimeSimulated());
    }
    public void GetCustomEvent(TimeSimulated _initTime, TimeSimulated _finalTime)
    //public void GetCustomEvent(TimeSimulated _initTime, TimeSimulated _finalTime, int _days)
    {
        this.timeInit = _initTime;
        this.timeFinal = _finalTime;
     //   this.daysToFinal = _days;
        this.acceptEventBool = false;
        EVENTTYPE _t = (EVENTTYPE)UnityEngine.Random.Range(0, 8);
        this.eventtype = _t.ToString();
        this.acceptCostEvent = UnityEngine.Random.Range(3, InGameMenuHandler.instance.GoldPlayer /2);
        this.declineCostEvent = UnityEngine.Random.Range(3, InGameMenuHandler.instance.GoldPlayer / 2);
        GetMessage();
    }
    public void GetMessage()
    {
        string option = this.eventtype.ToString();
        switch (option)
        {
            case string a when a.Contains("REBELION"):
                this.messaggeA = "We rebel against you. ";
                this.messaggeB = "this territory will rebel against you.";
                this.acceptMessageEvent = "-" + acceptCostEvent + " gold";
                this.declineMessageEvent = "-1 territorio";
                break;
            case string a when a.Contains("PETITION"):
                this.messaggeA = "We want to request you";
                this.messaggeB = "this territory want to request you";
                switch (option)
                {
                    case string b when b.Contains("MIN"):
                        this.element += " to improve the gold mine.";
                        this.acceptMessageEvent = "-" + acceptCostEvent + " gold " + "\n +2 gold mine level";
                        this.declineMessageEvent = "Nothing";
                        break;
                    case string b when b.Contains("FOR"):
                        this.element += " to improve the fortress.";
                        this.acceptMessageEvent = "-" + acceptCostEvent + " gold " + "\n +2 fortress level";
                        this.declineMessageEvent = "Nothing";
                        break;
                    case string b when b.Contains("FOR"):
                        this.element += " to improve the fortress.";
                        this.acceptMessageEvent = "-" + acceptCostEvent + " gold " + "\n +2 fortress level";
                        this.declineMessageEvent = "Nothing";
                        break;
                    default:
                        break;
                }
                break;
            case string a when a.Contains("SUG_MB"):
                this.messaggeA = "The military chief suggests you change the type of strategy ";
                this.messaggeB = "the military chief of this territory will suggest changing the type of strategy.";
                switch (option)
                {
                    case string b when b.Contains("1"):
                        this.element += " to improve more defensive.";
                        this.acceptMessageEvent = "Strategy type to Defensive \n+3 fortrees levels";
                        this.declineMessageEvent = "Keep the same strategy";
                        break;
                    case string b when b.Contains("2"):
                        this.element += " to improve more aggresive";
                        this.acceptMessageEvent = "Strategy type to Aggressive \n+2 barracks levels";
                        this.declineMessageEvent = "Keep the same strategy";
                        break;
                    case string b when b.Contains("3"):
                        this.element += " to improve the dominance of the terrain.";
                        this.acceptMessageEvent = "Strategy type to Terrain Master \n +3 speed food";
                        this.declineMessageEvent = "Keep the same strategy";
                        break;
                    default:
                        break;
                }
                break;
            case string a when a.Contains("GRACE"):
                this.messaggeA = "We found";
                this.messaggeB = "this territory are to be found";
                switch (option)
                {
                    case string b when b.Contains("DIV"):
                        this.element += " an ancient sanctuary to add to our domains.";
                        this.acceptMessageEvent = "+" + acceptCostEvent + " gold ";
                        this.declineMessageEvent = "- 10 opinion";
                        break;
                    case string b when b.Contains("MIN"):
                        this.element += " a abandoned mine to add to our domains.";
                        this.acceptMessageEvent = "+2 gold mine level";
                        this.declineMessageEvent = "+" + declineCostEvent + " gold scraps";
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
        PETITION_MIN,
        // mensaje : queremos pedirte mejores la mina
        // territorioEvent: territorio propio
        // accept  : -oro +2 velocidad de oro
        // decline : -opinion
        PETITION_FOR,
        // mensaje : queremos pedirte mejores la fortaleza
        // territorioEvent: territorio propio
        // accept  : - oro +1 defensa
        // decline : -opinion
        SUG_MB1,
        SUG_MB2,
        SUG_MB3,
        GRACE_DIV,
        // mensaje : encontramos un santuario antiguo
        // territorioEvent: territorio propio
        // aceptar : +oro => +fe 
        // decline : -opinion
        GRACE_MIN,
        // mensaje : encontramos una mina abandonada, 
        // territorioEvent: territorio propio
        // aceptar : +1 velocidad de mina
        // decline : +oro
        GRACE_FOOD
        // mensaje : encontramos una granja abandonada, 
        // territorioEvent: territorio propio
        // aceptar : +1 velocidad de comida
        // decline : +comida
    }
    public void AcceptEventAction()
    {
        acceptEventBool = true;
        switch (eventtype.ToString())
        {
            case "REBELION":
                InGameMenuHandler.instance.GoldPlayer -= acceptCostEvent;
                break;
            case "PETITION_MIN":
                InGameMenuHandler.instance.GoldPlayer -= acceptCostEvent;
                territoryEvent.GoldMineTerritory.ImproveBuilding(2);
                break;
            case "PETITION_FOR":
                InGameMenuHandler.instance.GoldPlayer -= acceptCostEvent;
                territoryEvent.FortressTerritory.ImproveBuilding(2);
                break;
            case "SUG_MB1":
                territoryEvent.MilitarBossTerritory.StrategyType = MilitarBoss.TYPESTRAT.DEFENSIVE.ToString();
                territoryEvent.FortressTerritory.ImproveBuilding(3);
                break;
            case "SUG_MB2":
                territoryEvent.MilitarBossTerritory.StrategyType = MilitarBoss.TYPESTRAT.AGGRESSIVE.ToString();
                territoryEvent.BarracksTerritory.ImproveBuilding(2);
                break;
            case "SUG_MB3":
                territoryEvent.MilitarBossTerritory.StrategyType = MilitarBoss.TYPESTRAT.TERRAIN_MASTER.ToString();
                territoryEvent.VelocityFood += 3;
                break;
            case "GRACE_DIV":
                InGameMenuHandler.instance.GoldPlayer += acceptCostEvent;
                break;
            case "GRACE_MIN":

                territoryEvent.GoldMineTerritory.ImproveBuilding(2);
                break;
            case "GRACE_FOOD":
                territoryEvent.VelocityFood += 2;
                break;
            default:
                break;
        }
        /*
        if (InGameMenuHandler.instance.GoldPlayer < 0)
        {
            DeclineEventAction();
        }
        */
    }
    public void DeclineEventAction()
    {
        switch (eventtype.ToString())
        {
            case "REBELION":
                TerritoryEvent.TypePlayer = Territory.TYPEPLAYER.NONE;
                break;
            case "PETITION_MIN":
                //TerritoryEvent.GoldMineTerritory.VelocityGold -= 0.6f;
                break;
            case "PETITION_FOR":
                //TerritoryEvent.FortressTerritory.PlusDefense -= 0.2f;
                break;
            case "SUG_MB1":
                break;
            case "SUG_MB2":
                break;
            case "SUG_MB3":
                break;
            case "GRACE_DIV":
                TerritoryEvent.MotivationPeople -= 10;
                break;
            case "GRACE_MIN":
                InGameMenuHandler.instance.GoldPlayer += declineCostEvent;
                break;
            case "GRACE_FOOD":
                InGameMenuHandler.instance.FoodPlayer += declineCostEvent;
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
