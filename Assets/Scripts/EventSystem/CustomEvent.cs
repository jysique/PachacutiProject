using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class CustomEvent
{
    [SerializeField] private string eventtype;
    private string messagge;
    //private string messaggeB;
    private Territory territoryEvent;
    private string element;
    private int acceptCostEvent;
    private int declineCostEvent;
    private string acceptMessageEvent;
    private string declineMessageEvent;
    [SerializeField]private TimeSimulated timeInit;
    [SerializeField]private TimeSimulated timeFinal;
    //private bool acceptEventBool = false;
    public STATUS statusEvent;
    public STATUS StatusEvent {
        get { return statusEvent; }
        set { statusEvent = value; }
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
    public string MessageEvent //sin tiempo
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

    public void GetCustomEvent(TimeSimulated _initTime,int days)
    {

        this.timeInit = new TimeSimulated(_initTime.Day, _initTime.Month, _initTime.Year);
        timeInit.PlusDays(days);

        this.timeFinal = new TimeSimulated(timeInit.Day, timeInit.Month, timeInit.Year);
        int r = UnityEngine.Random.Range(10, 15);
        timeFinal.PlusDays(days);

        EVENTTYPE _t = (EVENTTYPE)UnityEngine.Random.Range(0, Enum.GetNames(typeof(EVENTTYPE)).Length);
        this.eventtype = _t.ToString();
        this.statusEvent = STATUS.ANNOUNCE;
        this.acceptCostEvent = UnityEngine.Random.Range(3, InGameMenuHandler.instance.GoldPlayer / 2);
        this.declineCostEvent = UnityEngine.Random.Range(3, InGameMenuHandler.instance.GoldPlayer / 2);
        GetMessage();
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
    public void GetMessage()
    {
        string option = this.eventtype.ToString();
        switch (option)
        {
            case "REBELION":
                this.messagge = "In "+ territoryEvent.name+ " territory they plan to rebel against you in <DAYS> days, if you do not complete the following requirements, we will lose this territory. ";
                this.acceptMessageEvent = "-" + acceptCostEvent + " gold";
                this.declineMessageEvent = "-1 territorio";
                break;
            case "EVENT_PANDEMIC":
                this.messagge = "We have found a disease, if we don't do something we will lose half of our men. ";
                this.acceptMessageEvent = "-" + acceptCostEvent + " gold\n -" + acceptCostEvent + " food";
                this.declineMessageEvent = "-50% trops in all territories";
                break;
            case "EVENT_PANDEMIC2":
                this.messagge = "We have found a disease in "+ territoryEvent.name+ " territory, if we don't do something we will lose half of our men. ";
                this.acceptMessageEvent = "-" + acceptCostEvent + " gold\n -" + acceptCostEvent + " food";
                this.declineMessageEvent = "-50% trops in " + territoryEvent.name;
                break;
            case "EVENT_PLAGUE":
                this.messagge = "Our crops may be in danger from the plague. if we don't do something we will lose our food. ";
                this.acceptMessageEvent = "-" + acceptCostEvent + " gold\n -" + acceptCostEvent + " food";
                this.declineMessageEvent = "0.2 velocity in all territories";
                break;
            case "EVENT_PLAGUE2":
                this.messagge = "Our crops in "+ territoryEvent.name+ " territory may be in danger from the plague. if we don't do something we will lose the food of that place. ";
                this.acceptMessageEvent = "-" + acceptCostEvent + " gold\n -" + acceptCostEvent + " food";
                this.declineMessageEvent = "0.2 velocity in " + territoryEvent.name;
                break;
            case "PETITION_MIN":
                this.messagge = "In "+ territoryEvent.name+ " territory they want to ask you to improve the mine, they are somewhat short of resources. ";
                this.acceptMessageEvent = "-" + acceptCostEvent + " gold \n+ 2 mine gold level";
                this.declineMessageEvent = "-10 opinion" ;
                break;
            case "PETITION_FOR":
                this.messagge = "In "+ territoryEvent.name+ " territory they want to ask you to improve your defenses, they are very unprotected. ";
                this.acceptMessageEvent = "-" + acceptCostEvent + " gold \n+ 2 fortress";
                this.declineMessageEvent = "-10 opinion";
                break;
            case "GRACE_DIV":
                this.messagge = "A sanctuary has been found in "+ territoryEvent.name+ " territory. To appropriate them to our territories, meet the following requirements. ";
                this.acceptMessageEvent = "-" + acceptCostEvent + " gold \n+ 2 sacred place level";
                this.declineMessageEvent = "-10 opinion";
                break;
            case "GRACE_MIN":
                this.messagge = "An abandoned mine has been found in "+ territoryEvent.name+ " territory. To appropriate them to our territories, meet the following requirements. ";
                this.acceptMessageEvent = "-" + acceptCostEvent + " gold \n+ 3 gold mine level";
                this.declineMessageEvent = "+" + declineCostEvent + "gold";
                break;
            case "GRACE_FOOD":
                this.messagge = "We want to expand the seeding in "+ territoryEvent.name+ " territory. To run this, meet the following requirements. ";
                this.acceptMessageEvent = "-" + acceptCostEvent + " gold \n+ 3 vel of food";
                this.declineMessageEvent = "+" + declineCostEvent + "food";
                break;
            case null:
                break;
        }
    }
    public void AcceptEventAction()
    {
        statusEvent = STATUS.FINISH;
      //  acceptEventBool = true;
        switch (eventtype.ToString())
        {
            case "REBELION":
                InGameMenuHandler.instance.GoldPlayer -= acceptCostEvent;
                break;
            case "EVENT_PANDEMIC":
//                break;
            case "EVENT_PANDEMIC2":
                //break;
            case "EVENT_PLAGUE":
                break;
            case "EVENT_PLAGUE2":
                InGameMenuHandler.instance.GoldPlayer -= acceptCostEvent;
                InGameMenuHandler.instance.FoodPlayer -= acceptCostEvent;
                break;
            case "PETITION_MIN":
                InGameMenuHandler.instance.GoldPlayer -= acceptCostEvent;
                territoryEvent.GoldMineTerritory.ImproveBuilding(2);
                break;
            case "PETITION_FOR":
                InGameMenuHandler.instance.GoldPlayer -= acceptCostEvent;
                territoryEvent.FortressTerritory.ImproveBuilding(1);
                break;
            case "GRACE_DIV":
                InGameMenuHandler.instance.GoldPlayer -= acceptCostEvent;
                territoryEvent.SacredPlaceTerritory.ImproveBuilding(2);
                break;
            case "GRACE_MIN":
                InGameMenuHandler.instance.GoldPlayer -= acceptCostEvent;
                territoryEvent.GoldMineTerritory.ImproveBuilding(3);
                break;
            case "GRACE_FOOD":
                InGameMenuHandler.instance.GoldPlayer -= acceptCostEvent;
                territoryEvent.VelocityFood += 3;
                break;
            default:
                break;

        }
    }
    public void DeclineEventAction()
    {
        statusEvent = STATUS.FINISH;
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
                    list2[i].territoryStats.territory.VelocityFood = 0;
                }
                break;
            case "EVENT_PLAGUE2":
                territoryEvent.VelocityFood = 0;
                break;
            case "PETITION_MIN":

                //break;
            case "PETITION_FOR":
               // break;
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
