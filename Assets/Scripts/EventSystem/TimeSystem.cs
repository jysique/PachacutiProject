using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TimeSystem : MonoBehaviour
{
    public static TimeSystem instance;
    private TimeSimulated timeGame;
    private TimeSimulated timeGather;
    private TimeSimulated timeProduction;
    private TimeSimulated timeConsumption;
    
    private int daysToGather = 3;
    private int daysToProduce = 1;
    private int daysToConsume = 2;

    public TimeSimulated TimeGame
    {
        get { return timeGame; }
    }
    private void Awake()
    {
        instance = this;
        timeGame = new TimeSimulated(29, 12, 1399);
    }
    void Start()
    {
        InitializeGameEvents();
        InitializeEvents();
    }
    void Update()
    {
        GatherResourceInTime();
        ProductionResourceInTime();
        ConsumptionResourceInTime();
    }
    /// <summary>
    /// Initialize the time to gather, time to produce and time to consume
    /// Initialize the list of events and updgradeBuilds events
    /// Also add a new event
    /// </summary>
    void InitializeEvents()
    {

        timeGather = PlusDaysToTimeSimulated(daysToGather);
        timeProduction = PlusDaysToTimeSimulated(daysToProduce);
        timeConsumption = PlusDaysToTimeSimulated(daysToConsume);
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
        for (int i = 0; i < BotManager.instance.bots.Count; i++)
        {
            BotManager.instance.bots[i].GetResources();
        }
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
            Territory territory = t[i].Territory;
            territory.IncresementGold();
            territory.IncresementFood();
            if (territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
            //  if (territory.TypePlayer == Territory.TYPEPLAYER.PLAYER || territory.TypePlayer == Territory.TYPEPLAYER.CLAIM)
            {
                InGameMenuHandler.instance.ShowFloatingText("+" + territory.GoldMineTerritory.LimitUnits / territory.PerPeople + "gold", "TextMesh", t[i].transform, new Color32(0, 19, 152, 255));
                InGameMenuHandler.instance.ShowFloatingText("+" + territory.FarmTerritory.LimitUnits / territory.PerPeople + "food", "TextMesh", t[i].transform, new Color32(0, 19, 152, 255), posY: -0.25f);
            }
        }
    }
    /// <summary>
    /// Event to consume food resources from all territories of a player depending to the population
    /// </summary>
    private void onConsumptionResources()
    {
        timeConsumption = PlusDaysToTimeSimulated(daysToConsume);
        float foodConsumePlayer = GetConsumeByPlayer(Territory.TYPEPLAYER.PLAYER);
        if (InGameMenuHandler.instance.FoodPlayer >= foodConsumePlayer)
        {
            InGameMenuHandler.instance.FoodPlayer -= (int)foodConsumePlayer;
            InGameMenuHandler.instance.ShowFloatingText("-" + foodConsumePlayer, "TextFloating", ResourceTableHandler.instance.FoodAnimation, Color.white, posY: -10f);
        }
        else
        {
            InGameMenuHandler.instance.FoodPlayer = 0;
        }

        for (int i = 0; i < BotManager.instance.bots.Count; i++)
        {
            float foodConsumeBot = GetConsumeByPlayer(BotManager.instance.bots[i].TypeBot);
            if (BotManager.instance.bots[i].FoodBot >= foodConsumeBot)
            {
                BotManager.instance.bots[i].FoodBot -= (int)foodConsumeBot;
            }
            else
            {
                BotManager.instance.bots[i].FoodBot = 0;
            }
        }
    }

    private float GetConsumeByPlayer(Territory.TYPEPLAYER typePlayer)
    {
        float foodConsume = 0;
        List<Territory> t = TerritoryManager.instance.GetTerritoriesByTypePlayer(typePlayer);
        for (int i = 0; i < t.Count; i++)
        {
            foodConsume += Mathf.Ceil((float)t[i].Population / (float)t[i].PerPeople);
        }
        return foodConsume;
    }
}
