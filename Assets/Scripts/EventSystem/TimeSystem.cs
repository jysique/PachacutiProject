using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TimeSystem : MonoBehaviour
{
    public static TimeSystem instance;


    [SerializeField] private GameObject notificationEvent;
    private TimeSimulated timeGame;
    private TimeSimulated timeGather;
    private TimeSimulated timeProduction;
    private TimeSimulated timeConsumption;
    private TimeSimulated timeAddEvent;
    private int minDays = 15;
    private int maxDays = 20;
    private int daysToGather = 3;
    private int daysToProduce = 1;
    private int daysToConsume = 4;

    public CustomEventList listEvents;
    public CustomEventList listUpgradesBuilds;
    public TimeSimulated TimeGame
    {
        get { return timeGame; }
    }
    public int MinDays
    {
        get { return minDays; }
    }
    public int MaxDays
    {
        get { return maxDays; }
    }
    private void Awake()
    {
        instance = this;
        timeGame = new TimeSimulated(29, 12, 1399);
        //timeGame = new TimeSimulated(1, 1, 1399);
    }
    void Start()
    {
        InitializeGameEvents();
        InitializeListEvents();
    }
    /// <summary>
    /// Initialize the time to gather, time to produce and time to consume
    /// Initialize the list of events and updgradeBuilds events
    /// Also add a new event
    /// </summary>
    void InitializeListEvents()
    {
        timeGather = PlusDaysToTimeSimulated(daysToGather);
        timeProduction = PlusDaysToTimeSimulated(daysToProduce);
        timeConsumption = PlusDaysToTimeSimulated(daysToConsume);

        listEvents = new CustomEventList();
        listUpgradesBuilds = new CustomEventList();
        AddEvent();
    }
    /// <summary>
    /// Initialize all events (gather, produce, consume and exit event)
    /// </summary>
    void InitializeGameEvents()
    {
        GameEvents.instance.onGatherResourceTriggerEnter += onGatherResources;
        GameEvents.instance.onProductionResourceTriggerEnter += onProductionResources;
        GameEvents.instance.onConsumptionResourceTriggerEnter += onConsumptionResources;
        GameEvents.instance.onCustomEventExit += onGatherExit;
    }
    void Update()
    {
        GatherResourceInTime();
        ProductionResourceInTime();
        ConsumptionResourceInTime();
        CustomEventInTime();

        CheckListCustomEvent();
        CheckListBuildingsUpgrade();
    }
    /// <summary>
    /// Returns a time-simulated according to the timeGame
    /// </summary>
    /// <param name="daysToPlus">That days to add</param>
    /// <returns></returns>
    private TimeSimulated PlusDaysToTimeSimulated(int daysToPlus )
    {
        TimeSimulated time = new TimeSimulated(timeGame);
        time.PlusDays(daysToPlus);
        return time;
    }
    /// <summary>
    /// Add a new Event to the list of events
    /// </summary>
    private void AddEvent()
    {
        timeAddEvent = new TimeSimulated(timeGame);
        listEvents.AddCustomEvent(timeAddEvent);
        int rAddPlusDays = Random.Range(minDays, maxDays);
        timeAddEvent.PlusDays(rAddPlusDays);
    }
    /// <summary>
    /// Add a new UpgradeBuilding Event to the list
    /// </summary>
    /// <param name="territoryHandler">That territory improvement</param>
    /// <param name="building">That building improve</param>
    public void AddEvent(TerritoryHandler territoryHandler, Building building)
    {
        listUpgradesBuilds.AddCustomEvent(timeGame, territoryHandler, building);
    }
    public void AddEvent(TerritoryHandler territoryHandler)
    {
        listEvents.AddCustomEvent(timeGame, territoryHandler);
    }
    /// <summary>
    /// Check the timeGame with the time of gather resources
    /// if is the same active the event of gather resources
    /// </summary>
    private void GatherResourceInTime()
    {
        if (timeGame.EqualsDate(timeGather))
        {
            GameEvents.instance.GatherResourceTriggerEnter();
        }
        else
        {
            GameEvents.instance.CustomEventExit();
        }
    }
    /// <summary>
    /// Check the timeGame with the time of production resources
    /// if is the same active the event of production resources
    /// </summary>
    private void ProductionResourceInTime()
    {
        if (timeGame.EqualsDate(timeProduction))
        {
            GameEvents.instance.ProductionResourceTriggerEnter();
        }
        else
        {
            GameEvents.instance.CustomEventExit();
        }
    }
    /// <summary>
    /// Check the timeGame with the time of consume resources
    /// if is the same active the event of consume resources
    /// </summary>
    private void ConsumptionResourceInTime()
    {
        if (timeGame.EqualsDate(timeConsumption))
        {
            GameEvents.instance.ConsumptionResourceTriggerEnter();
        }
        else
        {
            GameEvents.instance.CustomEventExit();
        }
    }
    /// <summary>
    /// Check the timeGame with the timeAddEvent
    /// if is the same active the event to add a new custom event
    /// </summary>
    private void CustomEventInTime()
    {
        if (timeGame.EqualsDate(timeAddEvent))
        {
            AddEvent();
        }
        else
        {
            GameEvents.instance.CustomEventExit();
        }
    }
    /// <summary>
    /// check the status of the events in the LIST-EVENTS according 
    /// to the TIME-INIT and TIME-FINISH of every event comparing with the timeGame
    /// state one - if is ANNOUNCE and timeGame is equals to timeInit of enent -> PROGRESS (warning event)
    /// state two - if is in PROGRESS and timeGame is equales to timeFinish of event -> FINISH (finish event)
    /// Also corroborate notification status
    /// </summary>
    private void CheckListCustomEvent()
    {
        int numberActiveEvent = 0;
        for (int i = 0; i < listEvents.CustomEvents.Count; i++)
        {
            if (listEvents.CustomEvents[i].EventStatus == CustomEvent.STATUS.ANNOUNCE)
            {
                if (timeGame.EqualsDate(listEvents.CustomEvents[i].TimeInitEvent))
                {
                    listEvents.CustomEvents[i].EventStatus = CustomEvent.STATUS.PROGRESS;
                    WarningCustomEvent(i);
                }
            }
            else if (timeGame.EqualsDate(listEvents.CustomEvents[i].TimeFinalEvent) && listEvents.CustomEvents[i].EventStatus == CustomEvent.STATUS.PROGRESS)
            {
                listEvents.CustomEvents[i].EventStatus = CustomEvent.STATUS.FINISH;
                FinishCustomEvent(i);
            }
            if (listEvents.CustomEvents[i].EventStatus == CustomEvent.STATUS.PROGRESS)
            {
                numberActiveEvent++;
            }
        }
        if (numberActiveEvent >= 1)
        {
            notificationEvent.SetActive(true);
        }
        else
        {
            notificationEvent.SetActive(false);
        }
    }
    /// <summary>
    /// check the diference days of the UPGRADE-BUILDS events in the list according
    /// to the TIME-INIT of the upgrade of every event comparing with the timeGame
    /// if is the same update the daysTotal to 0, can Upgrade the builds again, improve the 
    /// building and remove the same event
    /// </summary>
    private void CheckListBuildingsUpgrade()
    {
        for (int i = 0; i < listUpgradesBuilds.CustomEvents.Count; i++)
        {
            int diferenceDays = timeGame.DiferenceDays(listUpgradesBuilds.CustomEvents[i].TimeInitEvent);
            listUpgradesBuilds.CustomEvents[i].Building.DaysTotal = diferenceDays;
            if (diferenceDays == listUpgradesBuilds.CustomEvents[i].Building.DaysToBuild)
            {
//                print("hola"); 
                listUpgradesBuilds.CustomEvents[i].Building.CanUpdrade = true;
                listUpgradesBuilds.CustomEvents[i].Building.DaysTotal = 0;
                listUpgradesBuilds.CustomEvents[i].Building.ImproveBuilding(1);
                listUpgradesBuilds.RemoveEvent(listUpgradesBuilds.CustomEvents[i]);
            }
            else
            {
                listUpgradesBuilds.CustomEvents[i].Building.CanUpdrade = false;
            }
        }
    }
    /// <summary>
    /// Event to do notthing
    /// </summary>
    private void onGatherExit()
    {
    }
    /// <summary>
    /// Event to gather gold and food resources from all territories of a player
    /// </summary>
    private void onGatherResources()
    {
        timeGather = PlusDaysToTimeSimulated(daysToGather);
        InGameMenuHandler.instance.GatherGoldResourceButton();
        InGameMenuHandler.instance.GatherFoodResourceButton();
    }
    /// <summary>
    /// Event to produce gold and food resources from all territories of a player
    /// </summary>
    private void onProductionResources()
    {
        timeProduction = PlusDaysToTimeSimulated(daysToProduce);
        List<TerritoryHandler> t = TerritoryManager.instance.GetAllTerritoriesHanlder();
        for (int i = 0; i < t.Count; i++)
        {
            TerritoryStats territoryStats = t[i].territoryStats;
            territoryStats.IncresementGold();
            territoryStats.IncresementFood();
            if (territoryStats.territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
            //  if (territoryStats.territory.TypePlayer == Territory.TYPEPLAYER.PLAYER || territoryStats.territory.TypePlayer == Territory.TYPEPLAYER.CLAIM)
            {
                InGameMenuHandler.instance.ShowFloatingText("+" + territoryStats.territory.GoldMineTerritory.WorkersMine / territoryStats.territory.PerPeople + "gold", "TextMesh", t[i].transform, new Color32(0, 19, 152, 255));
                InGameMenuHandler.instance.ShowFloatingText("+" + territoryStats.territory.IrrigationChannelTerritory.WorkersChannel / territoryStats.territory.PerPeople + "food", "TextMesh", t[i].transform, new Color32(0, 19, 152, 255), posY: -0.25f);
            }
        }
    }
    /// <summary>
    /// Event to consume food resources from all territories of a player depending to the population
    /// </summary>
    private void onConsumptionResources()
    {
        timeConsumption = PlusDaysToTimeSimulated(daysToConsume);
        float foodConsume = 0;
        List<TerritoryHandler> t = TerritoryManager.instance.GetAllTerritoriesHanlder();
        for (int i = 0; i < t.Count; i++)
        {
            TerritoryStats territoryStats = t[i].territoryStats;
            // if (territoryStats.territory.TypePlayer == Territory.TYPEPLAYER.PLAYER || territoryStats.territory.TypePlayer == Territory.TYPEPLAYER.CLAIM)
            if (territoryStats.territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
            {
                foodConsume += Mathf.Ceil((float)territoryStats.territory.Population / (float)territoryStats.territory.PerPeople);
            }
        }
        if (InGameMenuHandler.instance.FoodPlayer >= foodConsume)
        {
            InGameMenuHandler.instance.FoodPlayer -= (int)foodConsume;
            InGameMenuHandler.instance.ShowFloatingText("-" + foodConsume, "TextFloating", ResourceTableHandler.instance.FoodAnimation, Color.white,posY:-10f);
        }
        else
        {
            InGameMenuHandler.instance.FoodPlayer = 0;
            print("no le alcanza comida");
        }
    }
    /// <summary>
    /// Appears the Event Menu if it is in finish status
    /// </summary>
    /// <param name="id"></param>
    private void FinishCustomEvent(int id)
    {
        EventManager.instance.FinishCustomEventAppearance(listEvents.CustomEvents[id]);
        listEvents.CustomEvents[id].DeclineEventAction();
    }
    /// <summary>
    /// Appears the Event Menu if it is in warning of init status
    /// </summary>
    /// <param name="id"></param>
    private void WarningCustomEvent(int id)
    {
        //EventManager.instance.InstantiateEventListOption(listEvents);
        EventManager.instance.InstantiateEventListOption();
        AlertManager.AlertEvent();
        
       // EventManager.instance.WarningEventAppearance(listEvents.CustomEvents[id], listEvents.CustomEvents[id].DifferenceToFinal);
    }
    public bool GetIsTerritorieIsInPandemic(TerritoryHandler territoryEvent)
    {
        for (int i = 0; i < listEvents.CustomEvents.Count; i++)
        {
            if (listEvents.CustomEvents[i].EventStatus == CustomEvent.STATUS.PROGRESS && listEvents.CustomEvents[i].EventType == CustomEvent.EVENTTYPE.PANDEMIC && listEvents.CustomEvents[i].TerritoryEvent == territoryEvent)
            {
               // print("is in pandemic");
                return true;
            }
        }
        return false;
    }
    public bool GetIsTerritorieIsInPandemic()
    {
        for (int i = 0; i < listEvents.CustomEvents.Count; i++)
        {
            if (listEvents.CustomEvents[i].EventStatus == CustomEvent.STATUS.PROGRESS && listEvents.CustomEvents[i].EventType == CustomEvent.EVENTTYPE.ALL_T_PANDEMIC)
            {
               // print("is in all pandemic");
                return true;
            }
        }
        return false;
    }
}
