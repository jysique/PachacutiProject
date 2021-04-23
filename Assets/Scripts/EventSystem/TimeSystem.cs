using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class TimeSystem : MonoBehaviour
{
    public static TimeSystem instance;
    private int TIMESCALE; 
    private TimeSimulated timeGame;
    private TimeSimulated timeGather;
    private TimeSimulated timeEvent;
    public CustomEventList listEvents;
    int indexListEvent = 0;
    [SerializeField] private Text dayText;
    [SerializeField] private Text monthText;
    [SerializeField] private Text seasonText;
    [SerializeField] private Text yearText;
    [SerializeField] private Text weekText;
    public TimeSimulated TimeGame { 
        get { return timeGame; }
    }

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        timeGame = new TimeSimulated(28, 12, 1399);
        //timeGame = new TimeSimulated(1, 1, 1399);
        TIMESCALE = 6;
        if (GlobalVariables.instance != null)
        {
            timeGame = GlobalVariables.instance.GovernorChoose.TimeInit;
            TIMESCALE = GlobalVariables.instance.TimeScale;
        }
        TextCallFunction();
        InitializeGameEvents();
        InitializeListEvents();
    }
    void InitializeListEvents()
    {
        PlusDaysToTimeGather(3);
        listEvents = new CustomEventList();
        PlusDaysToTimeInitEvent(3);
    }
    void InitializeGameEvents()
    {
        GameEvents.instance.onGatherGoldTriggerEnter += onGatherGold;
        GameEvents.instance.onGatherFoodTriggerEnter += onGatherFood;
        GameEvents.instance.onCustomEventEnter += onCustomEvent;
        GameEvents.instance.onCustomWarningEventEnter += onWarningEvent;
        GameEvents.instance.onCustomEventExit += onGatherExit;
    }
    private void TextCallFunction()
    {
        if(timeGame.day <= 9)
        {
            dayText.text = "0" + timeGame.day.ToString();
        }
        else
        {
            dayText.text = timeGame.day.ToString();
        }
        if (timeGame.month <= 9)
        {
            monthText.text = "0" + timeGame.month.ToString();
        }
        else
        {
            monthText.text = timeGame.month.ToString();
        }
        weekText.text = timeGame.week.ToString();
        yearText.text = timeGame.year.ToString();
        seasonText.text = timeGame.season;
    }
    public void CalculateTime(TimeSimulated _time)
    {
        _time.CalculateSeason();
        if(_time.hour < 20)
        {
            _time.hour += Time.deltaTime * TIMESCALE;
        }
        else
        {
            _time.CalculateDay();
            _time.hour = 0;
        }
        _time.CalculateWeeks();
        if (_time.day > 30)
        {
            _time.CalculateMonth();  
        }
        if (_time.month > 12)
        {
            _time.CalculateYear();
        }
    }
    void Update()
    {
        CalculateTime(timeGame);
        TextCallFunction();
        GatherResourceInTime();
        CustomEventInTime();
    }

    void PlusDaysToTimeGather(int daysToPlus)
    {
        timeGather = new TimeSimulated(timeGame.day,timeGame.month,timeGame.year);
        timeGather.PlusDays(daysToPlus);
        CalculateTime(timeGather);
    }
    void PlusDaysToTimeInitEvent(int daysToPlus)
    {
        TimeSimulated timeFinalEvent;
        timeEvent = new TimeSimulated(timeGame.day, timeGame.month, timeGame.year);
        timeEvent.PlusDays(daysToPlus);
        CalculateTime(timeEvent);
        int rDays = Random.Range(3, 13);
        if (rDays >= 8)
        {
            timeFinalEvent = new TimeSimulated(timeEvent.day, timeEvent.month, timeEvent.year);
            timeFinalEvent.PlusDays(rDays);
            CalculateTime(timeFinalEvent);
        }
        else
        {
            timeFinalEvent = timeEvent;
        }
        listEvents.AddCustomEvent(timeEvent,timeFinalEvent,rDays);

        //listEvents.PrintList();
    }

    private void GatherResourceInTime()
    {
        if (timeGame.EqualsDate(timeGather))
        {
            GameEvents.instance.GatherGoldTriggerEnter();
            GameEvents.instance.GatherFoodTriggerEnter();
            PlusDaysToTimeGather(3);
        }
        else
        {
            GameEvents.instance.CustomEventExit();
        }
    }
    private void CustomEventInTime()
    {
        if (timeGame.EqualsDate(timeEvent))
        {
            if(timeGame.EqualsDate(listEvents.CustomEvents[indexListEvent].TimeFinalEvent))
            {
                GameEvents.instance.CustomEventEnter();
                int randomDays = Random.Range(3, 10);
                PlusDaysToTimeInitEvent(randomDays);
            }
            else
            {
                GameEvents.instance.CustomWarningEventEnter();
            }
        }
        else
        {
            GameEvents.instance.CustomEventExit();
        }
    }
    private void onGatherExit()
    {
    }
    private void onGatherGold()
    {
        InGameMenuHandler.instance.GatherGoldResourceButton();
    }
    private void onGatherFood()
    {
        InGameMenuHandler.instance.GatherFoodResourceButton();
    }
    private void onCustomEvent()
    {
        List<TerritoryHandler> list = TerritoryManager.instance.GetTerritoriesByTypePlayer(Territory.TYPEPLAYER.PLAYER);
        int r = Random.Range(0, list.Count);
        listEvents.CustomEvents[indexListEvent].TerritoryEvent = list[r].territoryStats.territory.name;
        InGameMenuHandler.instance.CustomEventAppearance(listEvents.CustomEvents[indexListEvent]);
        indexListEvent++;
    }

    private void onWarningEvent()
    {
        List<TerritoryHandler> list = TerritoryManager.instance.GetTerritoriesByTypePlayer(Territory.TYPEPLAYER.PLAYER);
        int r = Random.Range(0, list.Count);
        listEvents.CustomEvents[indexListEvent].TerritoryEvent = list[r].territoryStats.territory.name;
        InGameMenuHandler.instance.WarningEventAppearance(listEvents.CustomEvents[indexListEvent]);
        timeEvent = listEvents.CustomEvents[indexListEvent].TimeFinalEvent;
//        Debug.LogError("timeEvent " + timeEvent.PrintTimeSimulated());
    }
}
