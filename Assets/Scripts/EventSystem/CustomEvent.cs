using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class CustomEvent
{
    [SerializeField] private string eventtype;
    private string messagge;
    private Territory territoryEvent;
    private string element;
    private int costEvent;
    private string requirementMessageEvent;
    private string acceptMessageEvent;
    private string declineMessageEvent;
    [SerializeField]private TimeSimulated timeInit;
    [SerializeField]private TimeSimulated timeFinal;
    public STATUS eventStatus;
    private bool isAcceptedEvent;
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
        Debug.Log(i + " eventType - " + EventType.ToString());
        Debug.Log(i + " territorio- " + TerritoryEvent.name);
        Debug.Log(i + " init time- " + TimeInitEvent.PrintTimeSimulated());
        Debug.Log(i + " final time- " + TimeFinalEvent.PrintTimeSimulated());
    }
    /// <summary>
    /// Initialize custom event
    /// </summary>
    /// <param name="_initTime"></param>
    /// <param name="days"></param>
    public void GetCustomEvent(TimeSimulated _initTime,int days)
    {
        this.isAcceptedEvent = false;

        this.timeInit = new TimeSimulated(_initTime.Day, _initTime.Month, _initTime.Year);
        timeInit.PlusDays(days);

        this.timeFinal = new TimeSimulated(timeInit.Day, timeInit.Month, timeInit.Year);
        int rDays = UnityEngine.Random.Range(10, 15);
        timeFinal.PlusDays(rDays);
        EVENTTYPE _t = (EVENTTYPE)UnityEngine.Random.Range(0, Enum.GetNames(typeof(EVENTTYPE)).Length);
        this.eventtype = _t.ToString();
        this.eventStatus = STATUS.ANNOUNCE;
        this.costEvent = UnityEngine.Random.Range(3, InGameMenuHandler.instance.GoldPlayer / 2);
        GetMessage();
    }
    
    /// <summary>
    /// Retuns if is posible to accept the custom event
    /// </summary>
    /// <returns></returns>
    public bool GetAcceptButton()
    {
        string option = this.eventtype.ToString();
        switch (option)
        {
            case "EVENT_PANDEMIC":
            case "EVENT_PANDEMIC2":
            case "EVENT_PLAGUE":
            case "EVENT_PLAGUE2":
                if (InGameMenuHandler.instance.GoldPlayer >= costEvent && InGameMenuHandler.instance.FoodPlayer >= costEvent)
                {
                    return true;
                }
                break;
            case "REBELION":
            case "PETITION_MIN":
            case "PETITION_FOR":
            case "GRACE_DIV":
            case "GRACE_MIN":
            case "GRACE_FOOD":
                if (InGameMenuHandler.instance.GoldPlayer >= costEvent)
                {
                    return true;
                }
                break;
            default:
                break;
        }
        return false;
    }
    /// <summary>
    /// Change messages of the custom event
    /// </summary>
    public void GetMessage()
    {
        string option = this.eventtype.ToString();
        switch (option)
        {
            case "REBELION":
                this.messagge = "In "+ territoryEvent.name+ " territory they plan to rebel against you, if you do not complete the following requirements, we will lose this territory. ";
                this.requirementMessageEvent = "-" + costEvent + " gold.";
                this.acceptMessageEvent = "Keep " + territoryEvent.name + " territory.";
                this.declineMessageEvent = "-" +territoryEvent.name + " territory.";
                break;
            case "EVENT_PANDEMIC":
                this.messagge = "We have found a disease in our territories, if we don't do something we will lose half of our men. ";
                this.requirementMessageEvent = "-" + costEvent + " gold | -" + costEvent + " food.";
                this.acceptMessageEvent = "Cure the desease in all territories.";
                this.declineMessageEvent = "-50% trops in all territories.";
                break;
            case "EVENT_PANDEMIC2":
                this.messagge = "We have found a disease in "+ territoryEvent.name+ " territory, if we don't do something we will lose half of our men. ";
                this.requirementMessageEvent = "-" + costEvent + " gold | -" + costEvent + " food.";
                this.acceptMessageEvent = "Cure the desease in "+ territoryEvent.name + " territory.";
                this.declineMessageEvent = "-50% trops in " + territoryEvent.name + " territory.";
                break;
            case "EVENT_PLAGUE":
                this.messagge = "Our crops may be in danger from the plague. if we don't do something we will lose our food. ";
                this.requirementMessageEvent = "-" + costEvent + " gold | -" + costEvent + " food.";
                this.acceptMessageEvent = "Eliminate the plague in all territories.";
                this.declineMessageEvent = "Reduce to level 0 of irrigation channels in all territories.";
                break;
            case "EVENT_PLAGUE2":
                this.messagge = "Our crops in "+ territoryEvent.name+ " territory may be in danger from the plague. if we don't do something we will lose the food of that place. ";
                this.requirementMessageEvent = "-" + costEvent + " gold | -" + costEvent + " food.";
                this.acceptMessageEvent = "Eliminate the plague in " + territoryEvent.name + " territory.";
                this.declineMessageEvent = "Reduce to level 0 of irrigation channels in " + territoryEvent.name + " territory.";
                break;
            case "PETITION_MIN":
                this.messagge = "In "+ territoryEvent.name+ " territory they want to ask you to improve the mine, they are somewhat short of resources. ";
                this.requirementMessageEvent = "-" + costEvent + " gold.";
                this.acceptMessageEvent = "+2 mine gold level in " + territoryEvent.name + " territory.";
                this.declineMessageEvent = "-10 opinion in " + territoryEvent.name+ " territory.";
                break;
            case "PETITION_FOR":
                this.messagge = "In "+ territoryEvent.name+ " territory they want to ask you to improve your defenses, they are very unprotected. ";
                this.requirementMessageEvent = "-" + costEvent +" gold.";
                this.acceptMessageEvent = "+2 fortress level in " + territoryEvent.name + " territory.";
                this.declineMessageEvent = "-10 opinion in " + territoryEvent.name + " territory.";
                break;
            case "GRACE_DIV":
                this.messagge = "A sanctuary has been found in "+ territoryEvent.name+ " territory. To appropriate them to our territories, meet the following requirements. ";
                this.requirementMessageEvent = "-" + costEvent + " gold to repair";
                this.acceptMessageEvent = "+ 2 sacred place level in " + territoryEvent.name + " territory.";
                this.declineMessageEvent = "-10 opinion in " + territoryEvent.name + " territory.";
                break;
            case "GRACE_MIN":
                this.messagge = "An abandoned mine has been found in "+ territoryEvent.name+ " territory. To appropriate them to our territories, meet the following requirements. ";
                this.requirementMessageEvent = "-" + costEvent + " gold to repair";
                this.acceptMessageEvent = "+3 gold mine level in " + territoryEvent.name + " territory.";
                this.declineMessageEvent = "+15 gold.";
                break;
            case "GRACE_FOOD":
                this.messagge = "We want to expand the seeding in "+ territoryEvent.name+ " territory. To run this, meet the following requirements. ";
                this.requirementMessageEvent = "-" + costEvent + " gold to repair";
                this.acceptMessageEvent = "+3 irrigate chanel level in " + territoryEvent.name + " territory.";
                this.declineMessageEvent = "+15 food.";
                break;
            case null:
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
        switch (eventtype.ToString())
        {
            case "REBELION":
                InGameMenuHandler.instance.GoldPlayer -= costEvent;
                break;
            case "EVENT_PANDEMIC":
//                break;
            case "EVENT_PANDEMIC2":
                //break;
            case "EVENT_PLAGUE":
                //break;
            case "EVENT_PLAGUE2":
                InGameMenuHandler.instance.GoldPlayer -= costEvent;
                InGameMenuHandler.instance.FoodPlayer -= costEvent;
                break;
            case "PETITION_MIN":
                InGameMenuHandler.instance.GoldPlayer -= costEvent;
                territoryEvent.GoldMineTerritory.ImproveBuilding(2);
                break;
            case "PETITION_FOR":
                InGameMenuHandler.instance.GoldPlayer -= costEvent;
                territoryEvent.FortressTerritory.ImproveBuilding(1);
                break;
            case "GRACE_DIV":
                InGameMenuHandler.instance.GoldPlayer -= costEvent;
                territoryEvent.SacredPlaceTerritory.ImproveBuilding(2);
                break;
            case "GRACE_MIN":
                InGameMenuHandler.instance.GoldPlayer -= costEvent;
                territoryEvent.GoldMineTerritory.ImproveBuilding(3);
                break;
            case "GRACE_FOOD":
                InGameMenuHandler.instance.GoldPlayer -= costEvent;
                territoryEvent.IrrigationChannelTerritory.VelocityFood += 3;
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
        switch (eventtype.ToString())
        {
            case "REBELION":
                TerritoryEvent.TypePlayer = Territory.TYPEPLAYER.NONE;
                break;
            case "EVENT_PANDEMIC":
                List<TerritoryHandler> list = TerritoryManager.instance.GetTerritoriesByTypePlayer(Territory.TYPEPLAYER.PLAYER);
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].territoryStats.territory.Population /= 2;
                }
                break;
            case "EVENT_PANDEMIC2":
                territoryEvent.Population /= 2;
                break;
            case "EVENT_PLAGUE":
                List<TerritoryHandler> list2 = TerritoryManager.instance.GetTerritoriesByTypePlayer(Territory.TYPEPLAYER.PLAYER);
                for (int i = 0; i < list2.Count; i++)
                {
                    list2[i].territoryStats.territory.IrrigationChannelTerritory.Level = 0;
                }
                break;
            case "EVENT_PLAGUE2":
                territoryEvent.IrrigationChannelTerritory.Level = 0;
                break;
            case "PETITION_MIN":

                //break;
            case "PETITION_FOR":
               // break;
            case "GRACE_DIV":
                TerritoryEvent.MotivationPeople -= 10;
                break;
            case "GRACE_MIN":
                InGameMenuHandler.instance.GoldPlayer += 15;
                break;
            case "GRACE_FOOD":
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
            return this.acceptMessageEvent;
        }
        else {
            return this.declineMessageEvent;
        }
    }
    public enum EVENTTYPE
    {
        REBELION,
        EVENT_PANDEMIC,
        EVENT_PANDEMIC2,
        EVENT_PLAGUE,
        EVENT_PLAGUE2,
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
