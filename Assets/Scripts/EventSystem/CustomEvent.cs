using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class CustomEvent
{
  //  [SerializeField] private string eventtype;
    private string message;
    [SerializeField] private TerritoryHandler territoryEvent;
    private string element;
    private int costEvent;
    private string requirementMessageEvent;
    private string acceptMessageEvent;
    private string declineMessageEvent;
    private string resultMessageEvent;
    [SerializeField] private TimeSimulated timeInit;
    [SerializeField] private TimeSimulated timeFinal;
    [SerializeField] private STATUS eventStatus;
    [SerializeField] private EVENTTYPE eventType;
    private bool isAcceptedEvent;
    private bool w = false;
    private Building building;
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
    public Building Building
    {
        get { return building; }
        set { building = value; }
    }
    public void PrintEvent(int i)
    {
        Debug.Log(i + " eventType - " + eventType.ToString());
        Debug.Log(i + " territorio- " + TerritoryEvent.name);
        Debug.Log(i + " init time- " + TimeInitEvent.PrintTimeSimulated());
        Debug.Log(i + " final time- " + TimeFinalEvent.PrintTimeSimulated());
    }
    /// <summary>
    /// Initialize custom event
    /// </summary>
    /// <param name="_initTime"></param>
    /// <param name="days"></param>
    public void GetCustomEvent(TimeSimulated _initTime,TerritoryHandler territory)
    {
        this.isAcceptedEvent = false;

        //this.eventType = (EVENTTYPE)UnityEngine.Random.Range(0, 3);
        if (territory!=null)
        {
            this.eventType = EVENTTYPE.CONQUIST;
            this.territoryEvent = territory;
            
        }
        else
        {
            this.eventType = (EVENTTYPE)UnityEngine.Random.Range(0, Enum.GetNames(typeof(EVENTTYPE)).Length - 1);
            //this.eventType = EVENTTYPE.EARTHQUAKER;
            this.territoryEvent = TerritoryManager.instance.GetTerritoryRandom(Territory.TYPEPLAYER.PLAYER);
        }
        
        this.eventStatus = STATUS.ANNOUNCE;
        this.costEvent = UnityEngine.Random.Range(1, InGameMenuHandler.instance.GoldPlayer / 2);
        GetTimesEvents(_initTime);
        GetMessage();
    }
    /// <summary>
    ///  Initialize improve build event
    /// </summary>
    /// <param name="_initTime"></param>
    /// <param name="territory"></param>
    /// <param name="_building"></param>
    public void GetCustomEvent(TimeSimulated _initTime,TerritoryHandler territory, Building _building)
    {
        territoryEvent = territory;
        building = territoryEvent.GetBuilding(_building);
        building.TimeInit = new TimeSimulated(_initTime);
        timeInit = new TimeSimulated(_initTime);
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
    /*
        if (this.eventType != EVENTTYPE.EARTHQUAKER)
        {
            this.timeInit = new TimeSimulated(_initTime.Day, _initTime.Month, _initTime.Year);
            int rDays1 = UnityEngine.Random.Range(TimeSystem.instance.MinDays, TimeSystem.instance.MaxDays);
            timeInit.PlusDays(rDays1);

            this.timeFinal = new TimeSimulated(timeInit.Day, timeInit.Month, timeInit.Year);
            int rDays2 = UnityEngine.Random.Range(10, 15);
            timeFinal.PlusDays(rDays2);
            this.eventStatus = STATUS.ANNOUNCE;
        }
        else
        {
            this.timeFinal = new TimeSimulated(_initTime);
            int rDays2 = UnityEngine.Random.Range(10, 15);
            timeFinal.PlusDays(rDays2);
            this.eventStatus = STATUS.PROGRESS;
            this.isAcceptedEvent = false;
        }
        */
    private void GetTimesEvents(TimeSimulated _initTime)
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
            timeFinal.PlusDays(200);
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
    public void GetMessage()
    {
        this.message = GameMultiLang.GetTraduction(eventType.ToString() + "M").Replace("TERRITORYEVENT", TerritoryEvent.name);
        this.acceptMessageEvent = GameMultiLang.GetTraduction(eventType.ToString() + "A").Replace("TERRITORYEVENT", TerritoryEvent.name);
        this.declineMessageEvent = GameMultiLang.GetTraduction(eventType.ToString() + "D").Replace("TERRITORYEVENT", TerritoryEvent.name);
        switch (eventType)
        {
            case EVENTTYPE.EARTHQUAKER:
                this.requirementMessageEvent = GameMultiLang.GetTraduction("NoRequirements");
                break;
            case EVENTTYPE.DROUGHT:
            case EVENTTYPE.ALL_T_PANDEMIC:
            case EVENTTYPE.PANDEMIC:
            case EVENTTYPE.ALL_T_PLAGUE:
            case EVENTTYPE.PLAGUE:
                this.requirementMessageEvent = GameMultiLang.GetTraduction("REQ1").Replace("COSTEVENT", costEvent.ToString());
                break;
            case EVENTTYPE.PETITION_MIN:
            case EVENTTYPE.PETITION_FOR:
            case EVENTTYPE.REBELION:
                this.requirementMessageEvent = GameMultiLang.GetTraduction("REQ2").Replace("COSTEVENT", costEvent.ToString());
                break;
            case EVENTTYPE.GRACE_DIV:
            case EVENTTYPE.GRACE_MIN:
            case EVENTTYPE.GRACE_FOOD:
                this.requirementMessageEvent = GameMultiLang.GetTraduction("REQ3").Replace("COSTEVENT", costEvent.ToString());
                break;
            default:
                break;
        }
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
                TerritoryEvent.territoryStats.territory.GoldMineTerritory.ImproveBuilding(2);
                break;
            case EVENTTYPE.PETITION_FOR:
                InGameMenuHandler.instance.GoldPlayer -= costEvent;
                TerritoryEvent.territoryStats.territory.FortressTerritory.ImproveBuilding(1);
                break;
            case EVENTTYPE.GRACE_DIV:
                InGameMenuHandler.instance.GoldPlayer -= costEvent;
                TerritoryEvent.territoryStats.territory.SacredPlaceTerritory.ImproveBuilding(2);
                break;
            case EVENTTYPE.GRACE_MIN:
                InGameMenuHandler.instance.GoldPlayer -= costEvent;
                TerritoryEvent.territoryStats.territory.GoldMineTerritory.ImproveBuilding(3);
                break;
            case EVENTTYPE.GRACE_FOOD:
                InGameMenuHandler.instance.GoldPlayer -= costEvent;
                TerritoryEvent.territoryStats.territory.IrrigationChannelTerritory.ImproveBuilding(3);
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
                TerritoryEvent.territoryStats.territory.ResetAllBuilds();
                break;
            case EVENTTYPE.REBELION:
                TerritoryEvent.territoryStats.territory.TypePlayer = Territory.TYPEPLAYER.NONE;
                //    Debug.LogError("perdimos el territorio");
                break;
            case EVENTTYPE.DROUGHT:
                TerritoryEvent.territoryStats.territory.IrrigationChannelTerritory.ResetBuilding();
                TerritoryEvent.territoryStats.territory.Population /= 2;
                break;
            case EVENTTYPE.ALL_T_PANDEMIC:
                List<TerritoryHandler> list = TerritoryManager.instance.GetTerritoriesHandlerByTypePlayer(Territory.TYPEPLAYER.PLAYER);
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].territoryStats.territory.Population /= 2;
                }
                break;
            case EVENTTYPE.PANDEMIC:
                TerritoryEvent.territoryStats.territory.Population /= 2;
                break;
            case EVENTTYPE.ALL_T_PLAGUE:
                List<TerritoryHandler> list2 = TerritoryManager.instance.GetTerritoriesHandlerByTypePlayer(Territory.TYPEPLAYER.PLAYER);
                for (int i = 0; i < list2.Count; i++)
                {
                    list2[i].territoryStats.territory.IrrigationChannelTerritory.ResetBuilding();
                }
                break;
            case EVENTTYPE.PLAGUE:
                TerritoryEvent.territoryStats.territory.IrrigationChannelTerritory.ResetBuilding();
                break;
            case EVENTTYPE.PETITION_MIN:
                break;
            case EVENTTYPE.PETITION_FOR:
                break;
            case EVENTTYPE.GRACE_DIV:
                TerritoryEvent.territoryStats.territory.MotivationPeople -= 10;
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
            this.resultMessageEvent = "You complete the requirements of the " + TerritoryEvent.name + " territory petition.";
            return this.acceptMessageEvent;
        }
        else {
            this.resultMessageEvent = "You were unable to complete the requirements of the " + TerritoryEvent.name + " territory petition.";
            if(eventType == EVENTTYPE.EARTHQUAKER)
            {
                this.resultMessageEvent = "The results from the event in "+ TerritoryEvent.name + "territory.";
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
        CONQUIST
    }
    public enum STATUS
    {
        ANNOUNCE, // creado
        PROGRESS, // entre init time y finish time
        FINISH //finishTime
    }
}
