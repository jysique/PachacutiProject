using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TimeSystem : MonoBehaviour
{
    public static TimeSystem instance;
    private TimeSimulated timeGame;
    private TimeSimulated timeGather;
    private TimeSimulated timeAddEvent;
    public CustomEventList listEvents;
    int indexListEvent = 0;

    public TimeSimulated TimeGame
    {
        get { return timeGame; }
    }
    public int IndexListEvent
    {
        get { return indexListEvent; }
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
        PlusDaysToTimeGather(3);
        listEvents = new CustomEventList();
        int rDaysToInitEvent = Random.Range(15,25);
        PlusDaysToAddEvent(rDaysToInitEvent);
    }
    void InitializeGameEvents()
    {
        GameEvents.instance.onGatherGoldTriggerEnter += onGatherGold;
        GameEvents.instance.onGatherFoodTriggerEnter += onGatherFood;
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
        CustomEventInTime();
    }

    void PlusDaysToTimeGather(int daysToPlus)
    {
        timeGather = new TimeSimulated(timeGame.Day, timeGame.Month, timeGame.Year);
        timeGather.PlusDays(daysToPlus);
        CalculateTime(timeGather);
    }
    void PlusDaysToAddEvent(int daysToFinishEvent)
    {
        timeAddEvent = new TimeSimulated(timeGame.Day, timeGame.Month, timeGame.Year);
        // every 5 days add new event
        int rAddPlusDays = Random.Range(15, 20);
        timeAddEvent.PlusDays(rAddPlusDays);
        //   Debug.LogWarning("Time to add new event: " + timeAddEvent.PrintTimeSimulated());
        listEvents.AddCustomEvent(timeAddEvent, daysToFinishEvent);
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
        if (timeGame.EqualsDate(timeAddEvent))
        {
            int rDays = Random.Range(15, 25);
            PlusDaysToAddEvent(rDays);
        }
        else
        {
            GameEvents.instance.CustomEventExit();
        }

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
    private void FinishCustomEvent(int id)
    {
        InGameMenuHandler.instance.FinishCustomEventAppearance(listEvents.CustomEvents[id]);


    }
    private void WarningCustomEvent(int id)
    {
        InGameMenuHandler.instance.InstantiateEventListOption(listEvents);
        InGameMenuHandler.instance.WarningEventAppearance(listEvents.CustomEvents[id], listEvents.CustomEvents[id].DifferenceToFinal);
    }

}
