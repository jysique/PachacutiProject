using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CustomEvent
{
    [SerializeField] protected string message;
    [SerializeField] protected int costEvent;
    [SerializeField] protected string acceptMessageEvent;
    [SerializeField] protected string declineMessageEvent;
    [SerializeField] protected string requirementMessageEvent;
    [SerializeField] protected string resultMessageEvent;
    [SerializeField] protected bool w = false;
    [SerializeField] protected TerritoryHandler territoryEvent;
    [SerializeField] protected TimeSimulated timeInit;
    [SerializeField] protected TimeSimulated timeFinal;
    [SerializeField] protected STATUS eventStatus;
    [SerializeField] protected EVENTTYPE eventType;
    [SerializeField] protected bool isAcceptedEvent;

    
    public bool W
    {
        get { return w; }
        set { w = value; }
    }
    public STATUS EventStatus {
        get { return eventStatus; }
        set { eventStatus = value; }
    }
    public bool IsAccepted
    {
        get { return isAcceptedEvent; }
        set { isAcceptedEvent = value; }
    }
    public int DifferenceToFinal
    {
        get {return timeFinal.DiferenceDays(timeInit); }
    }
    public EVENTTYPE EventType
    {
        get { return eventType; }
        set { eventType = value; }
    }
    public string MessageEvent
    {
        get { return message; }
        set { message = value; }
    }
    public TerritoryHandler TerritoryEvent
    {
        get { return territoryEvent; }
        set { territoryEvent = value; }
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
    public string ResultMessagetEvent
    {
        get { return resultMessageEvent; }
        set { resultMessageEvent = value; }
    }
    public string RequirementMessageEvent
    {
        get { return requirementMessageEvent; }
        set { requirementMessageEvent = value; }
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
    public void PrintEvent(int i)
    {
        Debug.Log(i + " eventType - " + eventType.ToString());
        Debug.Log(i + " territorio- " + TerritoryEvent.name);
        Debug.Log(i + " init time- " + TimeInitEvent.PrintTimeSimulated());
        Debug.Log(i + " final time- " + TimeFinalEvent.PrintTimeSimulated());
    }
    public CustomEvent()
    {

    }

    public CustomEvent(TimeSimulated _initTime, TerritoryHandler territory)
    {
        this.isAcceptedEvent = false;
        if (territory != null)
        {
            this.eventType = EVENTTYPE.CONQUIST;
            this.territoryEvent = territory;
        }
        else
        {
            this.eventType = (EVENTTYPE)UnityEngine.Random.Range(0, Enum.GetNames(typeof(EVENTTYPE)).Length - 2);
            this.territoryEvent = TerritoryManager.instance.GetTerritoryRandom(Territory.TYPEPLAYER.PLAYER);
        }

        this.eventStatus = STATUS.ANNOUNCE;
        this.costEvent = UnityEngine.Random.Range(1, InGameMenuHandler.instance.GoldPlayer / 2);
        GetTimesEvents(_initTime );
        GetMessage();
        GetRequirement();
    }
    protected void GetTimesEvents(TimeSimulated _initTime)
    {
        this.timeInit = new TimeSimulated(_initTime);
        int rDays1 = UnityEngine.Random.Range(EventManager.instance.MinDays, EventManager.instance.MaxDays);
        timeInit.PlusDays(rDays1);

        if (this.eventType == EVENTTYPE.EARTHQUAKER)
        {
            this.timeFinal = new TimeSimulated(timeInit);
            timeFinal.PlusDays(1);
        }else if (this.eventType == EVENTTYPE.CONQUIST)
        {
            this.timeFinal = new TimeSimulated(timeInit);
            timeFinal.PlusDays(1000);
            this.eventStatus = STATUS.PROGRESS;
        }
        else
        {
            this.timeFinal = new TimeSimulated(timeInit);
            int rDays2 = UnityEngine.Random.Range(10, 15);
            timeFinal.PlusDays(rDays2);
        }
    }
    /// <summary>
    /// Change messages of the custom event
    /// </summary>
    protected void GetMessage()
    {
        this.message = GameMultiLang.GetTraduction(eventType.ToString() + "M").Replace("TERRITORYEVENT", TerritoryEvent.name);
        this.acceptMessageEvent = GameMultiLang.GetTraduction(eventType.ToString() + "A").Replace("TERRITORYEVENT", TerritoryEvent.name);
        this.declineMessageEvent = GameMultiLang.GetTraduction(eventType.ToString() + "D").Replace("TERRITORYEVENT", TerritoryEvent.name);
    }
    protected void GetRequirement()
    {
        switch (eventType)
        {
            case EVENTTYPE.EARTHQUAKER:
                this.requirementMessageEvent = "";
                break;
            case EVENTTYPE.DROUGHT:
            case EVENTTYPE.ALL_T_PANDEMIC:
            case EVENTTYPE.PANDEMIC:
            case EVENTTYPE.ALL_T_PLAGUE:
            case EVENTTYPE.PLAGUE:
                this.requirementMessageEvent = GameMultiLang.GetTraduction("Requirements") + "\n" + GameMultiLang.GetTraduction("REQ1").Replace("COSTEVENT", costEvent.ToString());
                break;
            case EVENTTYPE.PETITION_MIN:
            case EVENTTYPE.PETITION_FOR:
            case EVENTTYPE.REBELION:
                this.requirementMessageEvent = GameMultiLang.GetTraduction("Requirements") + "\n" + GameMultiLang.GetTraduction("REQ2").Replace("COSTEVENT", costEvent.ToString());
                break;
            case EVENTTYPE.GRACE_DIV:
            case EVENTTYPE.GRACE_MIN:
            case EVENTTYPE.GRACE_FOOD:
                this.requirementMessageEvent = GameMultiLang.GetTraduction("Requirements") + "\n" + GameMultiLang.GetTraduction("REQ3").Replace("COSTEVENT", costEvent.ToString());
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Retuns if is posible to accept the custom event
    /// </summary>
    /// <returns></returns>
    public bool GetAcceptButton()
    {
        bool accept = false;
        switch (eventType)
        {
            case EVENTTYPE.DROUGHT:
            case EVENTTYPE.ALL_T_PANDEMIC:
            case EVENTTYPE.PANDEMIC:
            case EVENTTYPE.ALL_T_PLAGUE:
            case EVENTTYPE.PLAGUE:
                if (InGameMenuHandler.instance.GoldPlayer >= costEvent && InGameMenuHandler.instance.FoodPlayer >= costEvent)
                {
                    accept = true;
                }
                break;
            case EVENTTYPE.REBELION:
            case EVENTTYPE.PETITION_MIN:
            case EVENTTYPE.PETITION_FOR:
            case EVENTTYPE.GRACE_DIV:
            case EVENTTYPE.GRACE_MIN:
            case EVENTTYPE.GRACE_FOOD:
                if (InGameMenuHandler.instance.GoldPlayer >= costEvent)
                {
                    accept = true;
                }
                break;
            default:
                break;
        }
        //Debug.LogError("canAccept|" + accept);
        return accept;
    }
    /// <summary>
    /// Accept Action event
    /// </summary>
    public void AcceptEventAction()
    {
        this.eventStatus = STATUS.FINISH;
        this.isAcceptedEvent = true;
        switch (eventType)
        {
            case EVENTTYPE.REBELION:
                InGameMenuHandler.instance.GoldPlayer -= costEvent;
                break;
            case EVENTTYPE.DROUGHT:
            case EVENTTYPE.ALL_T_PANDEMIC:
            case EVENTTYPE.PANDEMIC:
            case EVENTTYPE.ALL_T_PLAGUE:
            case EVENTTYPE.PLAGUE:
                InGameMenuHandler.instance.GoldPlayer -= costEvent;
                InGameMenuHandler.instance.FoodPlayer -= costEvent;
                break;
            case EVENTTYPE.PETITION_MIN:
                InGameMenuHandler.instance.GoldPlayer -= costEvent;
                TerritoryEvent.TerritoryStats.Territory.GoldMineTerritory.ImproveBuilding(2);
                break;
            case EVENTTYPE.PETITION_FOR:
                InGameMenuHandler.instance.GoldPlayer -= costEvent;
                TerritoryEvent.TerritoryStats.Territory.FortressTerritory.ImproveBuilding(1);
                break;
            case EVENTTYPE.GRACE_DIV:
                InGameMenuHandler.instance.GoldPlayer -= costEvent;
                TerritoryEvent.TerritoryStats.Territory.OpinionTerritory += 10;
                break;
            case EVENTTYPE.GRACE_MIN:
                InGameMenuHandler.instance.GoldPlayer -= costEvent;
                TerritoryEvent.TerritoryStats.Territory.GoldMineTerritory.ImproveBuilding(3);
                break;
            case EVENTTYPE.GRACE_FOOD:
                InGameMenuHandler.instance.GoldPlayer -= costEvent;
                TerritoryEvent.TerritoryStats.Territory.FarmTerritory.ImproveBuilding(3);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Decline Action event
    /// </summary>
    public void DeclineEventAction()
    {
        this.eventStatus= STATUS.FINISH;
        this.isAcceptedEvent = false;
        switch (eventType)
        {
            case EVENTTYPE.EARTHQUAKER:
                TerritoryEvent.TerritoryStats.Territory.ResetAllBuilds();
                break;
            case EVENTTYPE.REBELION:
                TerritoryEvent.TerritoryStats.Territory.TypePlayer = Territory.TYPEPLAYER.NONE;
                //    Debug.LogError("perdimos el territorio");
                break;
            case EVENTTYPE.DROUGHT:
                TerritoryEvent.TerritoryStats.Territory.FarmTerritory.ResetBuilding();
                TerritoryEvent.TerritoryStats.Territory.Lancers.NumbersUnit /= 2;
                TerritoryEvent.TerritoryStats.Territory.Swordsmen.NumbersUnit /= 2;
                TerritoryEvent.TerritoryStats.Territory.Axemen.NumbersUnit /= 2;
                TerritoryEvent.TerritoryStats.Territory.Scouts.NumbersUnit /= 2;
                break;
            case EVENTTYPE.ALL_T_PANDEMIC:
                List<TerritoryHandler> list = TerritoryManager.instance.GetTerritoriesHandlerByTypePlayer(Territory.TYPEPLAYER.PLAYER);
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].TerritoryStats.Territory.Lancers.NumbersUnit /= 2;
                    list[i].TerritoryStats.Territory.Swordsmen.NumbersUnit /= 2;
                    list[i].TerritoryStats.Territory.Axemen.NumbersUnit /= 2;
                    list[i].TerritoryStats.Territory.Scouts.NumbersUnit /= 2;
                }
                break;
            case EVENTTYPE.PANDEMIC:
                TerritoryEvent.TerritoryStats.Territory.Lancers.NumbersUnit /= 2;
                TerritoryEvent.TerritoryStats.Territory.Swordsmen.NumbersUnit /= 2;
                TerritoryEvent.TerritoryStats.Territory.Axemen.NumbersUnit /= 2;
                TerritoryEvent.TerritoryStats.Territory.Scouts.NumbersUnit /= 2;
                break;
            case EVENTTYPE.ALL_T_PLAGUE:
                List<TerritoryHandler> list2 = TerritoryManager.instance.GetTerritoriesHandlerByTypePlayer(Territory.TYPEPLAYER.PLAYER);
                for (int i = 0; i < list2.Count; i++)
                {
                    list2[i].TerritoryStats.Territory.FarmTerritory.ResetBuilding();
                }
                break;
            case EVENTTYPE.PLAGUE:
                TerritoryEvent.TerritoryStats.Territory.FarmTerritory.ResetBuilding();
                break;
            case EVENTTYPE.PETITION_MIN:
                break;
            case EVENTTYPE.PETITION_FOR:
                break;
            case EVENTTYPE.GRACE_DIV:
                TerritoryEvent.TerritoryStats.Territory.OpinionTerritory -= 10;
                break;
            case EVENTTYPE.GRACE_MIN:
                InGameMenuHandler.instance.GoldPlayer += 15;
                break;
            case EVENTTYPE.GRACE_FOOD:
                InGameMenuHandler.instance.FoodPlayer += 15;
                break;
            default:
                break;
        }
        if(InGameMenuHandler.instance.GoldPlayer< 0)
        {
            InGameMenuHandler.instance.GoldPlayer = 0;
        }
    }
    /// <summary>
    /// Returns the message if is accepted of decline
    /// </summary>
    /// <returns></returns>
    public string ResultsEvent()
    {
        if (isAcceptedEvent)
        {
            this.resultMessageEvent = GameMultiLang.GetTraduction("CompleteResults").Replace("TERRITORYEVENT", TerritoryEvent.name);
            return this.acceptMessageEvent;
        }
        else {
            this.resultMessageEvent = GameMultiLang.GetTraduction("IncompleteResults").Replace("TERRITORYEVENT", TerritoryEvent.name);
            if (eventType == EVENTTYPE.EARTHQUAKER)
            {
                this.resultMessageEvent = GameMultiLang.GetTraduction("EarthquarkeResults").Replace("TERRITORYEVENT", TerritoryEvent.name);
            }
            if (eventType == EVENTTYPE.EXPLORATION)
            {
                this.resultMessageEvent = GameMultiLang.GetTraduction("ExplorationResults").Replace("TERRITORYEVENT", TerritoryEvent.name);
            }
            return this.declineMessageEvent;
        }
    }
    public enum EVENTTYPE
    {
        EARTHQUAKER,
        REBELION,
        DROUGHT,
        ALL_T_PANDEMIC,
        PANDEMIC,
        ALL_T_PLAGUE,
        PLAGUE,
        PETITION_MIN,
        PETITION_FOR,
        GRACE_DIV,
        GRACE_MIN,
        GRACE_FOOD,
        CONQUIST,
        EXPLORATION
    }
    public enum STATUS
    {
        ANNOUNCE, // creado
        PROGRESS, // entre init time y finish time
        FINISH //finishTime
    }
}
[Serializable]
public class CustomBuilding : CustomEvent
{
    [SerializeField] private Building building;
    public Building BuildingEvent
    {
        get { return building; }
        set { building = value; }
    }
    public CustomBuilding(TimeSimulated _initTime, TerritoryHandler territory, Building _building)
    {
        this.territoryEvent = territory;
        building = territoryEvent.TerritoryStats.Territory.GetBuilding(_building);
        building.TimeInit = new TimeSimulated(_initTime);
        timeInit = new TimeSimulated(_initTime);
    }
    public void FinishUpgradeBuilding(int levels)
    {
        building.CanUpdrade = true;
        building.DaysTotal = 0;
        building.ImproveBuilding(levels);
        building.ImproveCostUpgrade(levels);
    }
}

[Serializable]
public class CustomExpedition : CustomEvent
{
    [SerializeField] private int sword;
    [SerializeField] private int lance;
    [SerializeField] private int axe;
    [SerializeField] private TerritoryHandler attacker;
    public int SwordEvent
    {
        get { return sword; }
        set { sword = value; }
    }
    public int LanceEvent
    {
        get { return lance; }
        set { lance = value; }
    }
    public int AxeEvent
    {
        get { return axe; }
        set { axe = value; }
    }
    public CustomExpedition(TimeSimulated _initTime, int a, int b, int c, TerritoryHandler attackerterritory, TerritoryHandler wasterterritory)
    {
        this.sword = a;
        this.lance = b;
        this.axe = c;
        this.isAcceptedEvent = false;
        this.attacker = attackerterritory;
        this.territoryEvent = wasterterritory;
        this.eventType = EVENTTYPE.EXPLORATION;
        this.eventStatus = STATUS.PROGRESS;
        this.requirementMessageEvent = " ";
      //  this.costEvent = UnityEngine.Random.Range(1, InGameMenuHandler.instance.GoldPlayer / 2);
        GetTimesExpeditionEvents(_initTime);
        GetMessage();
    }
    void GetTimesExpeditionEvents(TimeSimulated _initTime)
    {
        this.timeInit = new TimeSimulated(_initTime);
        this.timeFinal = new TimeSimulated(timeInit);
        int rDays2 = UnityEngine.Random.Range(10, 15);
        timeFinal.PlusDays(rDays2);
    }
    public void ReturnUnits()
    {
        Troop troop = new Troop(sword, lance, axe);
        WarManager.instance.SendWarriors(territoryEvent, attacker, troop);
    }
}