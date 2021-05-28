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
    private int indexListEvent = 0;
    private int minDays = 15;
    private int maxDays = 20;
    private int daysToGather = 3;
    private int daysToProduce = 1;
    private int daysToConsume = 4;

    public CustomEventList listEvents;
    public TimeSimulated TimeGame
    {
        get { return timeGame; }
    }
    public int IndexListEvent
    {
        get { return indexListEvent; }
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
    void InitializeListEvents()
    {
        PlusDaysToTimeGather(daysToGather);
        PlusDaysToTimeProduction(daysToProduce);
        PlusDaysToTimeConsumption(daysToConsume);
        listEvents = new CustomEventList();
        AddEvent();
    }
    void InitializeGameEvents()
    {
        GameEvents.instance.onGatherResourceTriggerEnter += onGatherResources;
        GameEvents.instance.onProductionResourceTriggerEnter += onProductionResources;
        GameEvents.instance.onConsumptionResourceTriggerEnter += onConsumptionResources;
        GameEvents.instance.onCustomEventExit += onGatherExit;
    }
    
    public void CalculateTime(TimeSimulated _time)
    {
        _time.CalculateSeason();
        if (_time.Hour < 20)
        {
            _time.Hour += Time.deltaTime * GlobalVariables.instance.TimeScale; 
        }
        else
        {
            _time.CalculateDay();
            _time.Hour = 0;
        }
        _time.CalculateWeeks();
        if (_time.Day > 30)
        {
            _time.CalculateMonth();
        }
        if (_time.Month > 12)
        {
            _time.CalculateYear();
        }
    }
    void Update()
    {
        GatherResourceInTime();
        ProductionResourceInTime();
        ConsumptionResourceInTime();
        CustomEventInTime();
        CheckListCustomEvent();
    }

    void PlusDaysToTimeGather(int daysToPlus)
    {
        timeGather = new TimeSimulated(timeGame.Day, timeGame.Month, timeGame.Year);
        timeGather.PlusDays(daysToPlus);
        CalculateTime(timeGather);
    }

    void PlusDaysToTimeProduction(int daysToPlus)
    {
        timeProduction = new TimeSimulated(timeGame.Day, timeGame.Month, timeGame.Year);
        timeProduction.PlusDays(daysToPlus);
        CalculateTime(timeProduction);
    }

    void PlusDaysToTimeConsumption(int daysToPlus)
    {
        timeConsumption = new TimeSimulated(timeGame);
        timeConsumption.PlusDays(daysToPlus);
        CalculateTime(timeConsumption);
    }

    /// <summary>
    /// every randomAddPlusDays add a new Event
    /// </summary>
    /// <param name="daysToFinishEvent"></param>
    void AddEvent()
    {
        timeAddEvent = new TimeSimulated(timeGame.Day, timeGame.Month, timeGame.Year);
        // every 5 days add new event
        listEvents.AddCustomEvent(timeAddEvent);
        int rAddPlusDays = Random.Range(minDays, maxDays);
        timeAddEvent.PlusDays(rAddPlusDays);
        //   Debug.LogWarning("Time to add new event: " + timeAddEvent.PrintTimeSimulated());
        
    }
    /// <summary>
    /// Every 3 days automatically gather gold or food from all territories of the player
    /// </summary>
    private void GatherResourceInTime()
    {
        if (timeGame.EqualsDate(timeGather))
        {
            GameEvents.instance.GatherResourceTriggerEnter();
            PlusDaysToTimeGather(daysToGather);
        }
        else
        {
            GameEvents.instance.CustomEventExit();
        }
    }

    private void ProductionResourceInTime()
    {
        if (timeGame.EqualsDate(timeProduction))
        {
            GameEvents.instance.ProductionResourceTriggerEnter();
            PlusDaysToTimeProduction(daysToProduce);
        }
        else
        {
            GameEvents.instance.CustomEventExit();
        }
    }
    private void ConsumptionResourceInTime()
    {
        if (timeGame.EqualsDate(timeConsumption))
        {
            GameEvents.instance.ConsumptionResourceTriggerEnter();
            PlusDaysToTimeConsumption(daysToConsume);
        }
        else
        {
            GameEvents.instance.CustomEventExit();
        }
    }

    /// <summary>
    /// Every random between 15 and 25 days automatically add a event to the list
    /// </summary>
    private void CustomEventInTime()
    {
        if (timeGame.EqualsDate(timeAddEvent))
        {
            int rDays = Random.Range(15, 25);
            AddEvent();
        }
        else
        {
            GameEvents.instance.CustomEventExit();
        }
    }
    /// <summary>
    /// check the status of the events in the list according 
    /// to the time-simulated of the event comparing with the time game 
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
    private void onGatherExit()
    {
    }
    private void onGatherResources()
    {
        InGameMenuHandler.instance.GatherGoldResourceButton();
        InGameMenuHandler.instance.GatherFoodResourceButton();
    }
    private void onProductionResources()
    {
        List<TerritoryHandler> t = TerritoryManager.instance.GetAllTerritoriesHanlder();
        for (int i = 0; i < t.Count; i++)
        {
            TerritoryStats territoryStats = t[i].territoryStats;
            territoryStats.IncresementGold();
            territoryStats.IncresementFood();
            if (territoryStats.territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
            {
                InGameMenuHandler.instance.ShowFloatingText("+" + territoryStats.territory.GoldMineTerritory.WorkersMine / territoryStats.territory.PerPeople + "gold", "TextMesh", t[i].transform, new Color32(0, 19, 152, 255));
                InGameMenuHandler.instance.ShowFloatingText("+" + territoryStats.territory.IrrigationChannelTerritory.WorkersChannel / territoryStats.territory.PerPeople + "food", "TextMesh", t[i].transform, new Color32(0, 19, 152, 255), posY: -0.25f);
            }
            //InGameMenuHandler.instance.ShowFloatingText("+" + TerritoryManager.instance.GetOveralRateResource(Territory.TYPEPLAYER.PLAYER, "goldmine"), "TextFloating", ResourceTableHandler.instance.GoldAnimation, Color.white);
            //InGameMenuHandler.instance.ShowFloatingText("+" + TerritoryManager.instance.GetOveralRateResource(Territory.TYPEPLAYER.PLAYER, "channel"), "TextFloating", ResourceTableHandler.instance.FoodAnimation, Color.white);
        }

    }
    private void onConsumptionResources()
    {
        int foodConsume = 0;
        List<TerritoryHandler> t = TerritoryManager.instance.GetAllTerritoriesHanlder();
        for (int i = 0; i < t.Count; i++)
        {
            TerritoryStats territoryStats = t[i].territoryStats;
            if (territoryStats.territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
            {
                foodConsume += territoryStats.territory.Population / territoryStats.territory.PerPeople;
            }
        }
        if (InGameMenuHandler.instance.FoodPlayer >= foodConsume)
        {
            InGameMenuHandler.instance.FoodPlayer -= foodConsume;
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
    /// Appears the Event Menu if it is in warning status
    /// </summary>
    /// <param name="id"></param>
    private void WarningCustomEvent(int id)
    {
        EventManager.instance.InstantiateEventListOption(listEvents);
        EventManager.instance.WarningEventAppearance(listEvents.CustomEvents[id], listEvents.CustomEvents[id].DifferenceToFinal);
    }

}
