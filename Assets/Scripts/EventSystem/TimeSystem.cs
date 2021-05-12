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
    [SerializeField] private Text dayText;
    [SerializeField] private Text monthText;
    [SerializeField] private Text seasonText;
    [SerializeField] private Text yearText;
    [SerializeField] private Text weekText;
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
    }
    void Start()
    {
        timeGame = new TimeSimulated(29, 12, 1399);
        //timeGame = new TimeSimulated(1, 1, 1399);
    
        TextCallFunction();
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
    private void TextCallFunction()
    {
        if (timeGame.Day <= 9)
        {
            dayText.text = "0" + timeGame.Day.ToString();
        }
        else
        {
            dayText.text = timeGame.Day.ToString();
        }
        if (timeGame.Month <= 9)
        {
            monthText.text = "0" + timeGame.Month.ToString();
        }
        else
        {
            monthText.text = timeGame.Month.ToString();
        }
        weekText.text = timeGame.Week.ToString();
        yearText.text = timeGame.Year.ToString();
        seasonText.text = timeGame.Season;
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
        CalculateTime(timeGame);
        TextCallFunction();
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
        timeAddEvent.PlusDays(5);
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
            if (listEvents.CustomEvents[i].StatusEvent == CustomEvent.STATUS.ANNOUNCE)
            {
                if (timeGame.EqualsDate(listEvents.CustomEvents[i].TimeInitEvent))
                {
                    listEvents.CustomEvents[i].StatusEvent = CustomEvent.STATUS.PROGRESS;
                    WarningCustomEvent(i);

                }
            }

            else if (timeGame.EqualsDate(listEvents.CustomEvents[i].TimeFinalEvent) && listEvents.CustomEvents[i].StatusEvent == CustomEvent.STATUS.PROGRESS)
            {
                listEvents.CustomEvents[i].StatusEvent = CustomEvent.STATUS.FINISH;
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
