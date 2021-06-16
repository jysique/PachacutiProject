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
    private Building building;
    
    
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
    public void GetCustomEvent(TimeSimulated _initTime)
    {
        this.isAcceptedEvent = false;

        //this.eventType = (EVENTTYPE)UnityEngine.Random.Range(3, 5);
        this.eventType = (EVENTTYPE)UnityEngine.Random.Range(0, Enum.GetNames(typeof(EVENTTYPE)).Length);
        this.eventStatus = STATUS.ANNOUNCE;
        this.costEvent = UnityEngine.Random.Range(3, InGameMenuHandler.instance.GoldPlayer / 2);
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
        // building.TimeFinish = new TimeSimulated(_initTime);
        // building.TimeFinish.PlusDays(building.DaysToBuild);
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
        Debug.LogError("canAccept|" + accept);
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
        int rDays1 = UnityEngine.Random.Range(TimeSystem.instance.MinDays, TimeSystem.instance.MaxDays);
        timeInit.PlusDays(rDays1);

        if (this.eventType != EVENTTYPE.EARTHQUAKER)
        {
            this.timeFinal = new TimeSimulated(timeInit);
            int rDays2 = UnityEngine.Random.Range(10, 15);
            timeFinal.PlusDays(rDays2);
        }
        else
        {
            this.timeFinal = new TimeSimulated(timeInit);
            timeFinal.PlusDays(1);
        }
    }
    /// <summary>
    /// Change messages of the custom event
    /// </summary>
    public void GetMessage()
    {
        switch (eventType)
        {
            case EVENTTYPE.EARTHQUAKER:
                this.message = "An earthquake has occurred in "+ TerritoryEvent.name+" territory";
                this.requirementMessageEvent = "No requirements";
                this.declineMessageEvent = "Reset builds in " + territoryEvent.name + " territory.";
                break;
            case EVENTTYPE.DROUGHT:
                this.message = "Drought continues in "+ TerritoryEvent.name +" territory. The villagers are going through a bad time. That is why they ask you for the following requirements";
                this.requirementMessageEvent = "-" + costEvent + " gold | -" + costEvent + " food.";
                this.acceptMessageEvent = "The residents can continue harvesting.";
                this.declineMessageEvent = "Conditions worsen for irrigation canals and troops in " + territoryEvent.name + " territory.";
                break;
            case EVENTTYPE.REBELION:
                this.message = "In "+ TerritoryEvent.name+ " territory they plan to rebel against you, if you do not complete the following requirements, we will lose this territory. ";
                this.requirementMessageEvent = "-" + costEvent + " gold.";
                this.acceptMessageEvent = "Keep " + TerritoryEvent.name + " territory.";
                this.declineMessageEvent = "-" +territoryEvent.name + " territory.";
                break;
            case EVENTTYPE.ALL_T_PANDEMIC:
                this.message = "We have found a disease in our territories, if we don't do something we will lose half of our men. ";
                this.requirementMessageEvent = "-" + costEvent + " gold | -" + costEvent + " food.";
                this.acceptMessageEvent = "Cure the desease in all territories.";
                this.declineMessageEvent = "-50% trops in all territories.";
                break;
            case EVENTTYPE.PANDEMIC:
                this.message = "We have found a disease in "+ territoryEvent.name+ " territory, if we don't do something we will lose half of our men. ";
                this.requirementMessageEvent = "-" + costEvent + " gold | -" + costEvent + " food.";
                this.acceptMessageEvent = "Cure the desease in "+ territoryEvent.name + " territory.";
                this.declineMessageEvent = "-50% trops in " + territoryEvent.name + " territory.";
                break;
            case EVENTTYPE.ALL_T_PLAGUE:
                this.message = "Our crops may be in danger from the ALL_T_PLAGUE. if we don't do something we will lose our food. ";
                this.requirementMessageEvent = "-" + costEvent + " gold | -" + costEvent + " food.";
                this.acceptMessageEvent = "Eliminate the ALL_T_PLAGUE in all territories.";
                this.declineMessageEvent = "Reduce to level 0 of irrigation channels in all territories.";
                break;
            case EVENTTYPE.PLAGUE:
                this.message = "Our crops in "+ territoryEvent.name+ " territory may be in danger from the ALL_T_PLAGUE. if we don't do something we will lose the food of that place. ";
                this.requirementMessageEvent = "-" + costEvent + " gold | -" + costEvent + " food.";
                this.acceptMessageEvent = "Eliminate the ALL_T_PLAGUE in " + territoryEvent.name + " territory.";
                this.declineMessageEvent = "Reduce to level 0 of irrigation channels in " + territoryEvent.name + " territory.";
                break;
            case EVENTTYPE.PETITION_MIN:
                this.message = "In "+ territoryEvent.name+ " territory they want to ask you to improve the mine, they are somewhat short of resources. ";
                this.requirementMessageEvent = "-" + costEvent + " gold.";
                this.acceptMessageEvent = "+2 mine gold level in " + territoryEvent.name + " territory.";
                this.declineMessageEvent = "-10 opinion in " + territoryEvent.name+ " territory.";
                break;
            case EVENTTYPE.PETITION_FOR:
                this.message = "In "+ territoryEvent.name+ " territory they want to ask you to improve your defenses, they are very unprotected. ";
                this.requirementMessageEvent = "-" + costEvent +" gold.";
                this.acceptMessageEvent = "+2 fortress level in " + territoryEvent.name + " territory.";
                this.declineMessageEvent = "-10 opinion in " + territoryEvent.name + " territory.";
                break;
            case EVENTTYPE.GRACE_DIV:
                this.message = "A sanctuary has been found in "+ territoryEvent.name+ " territory. To appropriate them to our territories, meet the following requirements. ";
                this.requirementMessageEvent = "-" + costEvent + " gold to repair";
                this.acceptMessageEvent = "+ 2 sacred place level in " + territoryEvent.name + " territory.";
                this.declineMessageEvent = "-10 opinion in " + territoryEvent.name + " territory.";
                break;
            case EVENTTYPE.GRACE_MIN:
                this.message = "An abandoned mine has been found in "+ territoryEvent.name+ " territory. To appropriate them to our territories, meet the following requirements. ";
                this.requirementMessageEvent = "-" + costEvent + " gold to repair";
                this.acceptMessageEvent = "+3 gold mine level in " + territoryEvent.name + " territory.";
                this.declineMessageEvent = "+15 gold.";
                break;
            case EVENTTYPE.GRACE_FOOD:
                this.message = "We want to expand the seeding in "+ territoryEvent.name+ " territory. To run this, meet the following requirements. ";
                this.requirementMessageEvent = "-" + costEvent + " gold to repair";
                this.acceptMessageEvent = "+3 irrigate chanel level in " + territoryEvent.name + " territory.";
                this.declineMessageEvent = "+15 food.";
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
        GRACE_FOOD
    }
    public enum STATUS
    {
        ANNOUNCE, // creado
        PROGRESS, // entre init time y finish time
        FINISH //finishTime
    }
}
