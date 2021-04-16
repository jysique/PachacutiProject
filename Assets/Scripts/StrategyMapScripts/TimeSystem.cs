using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using System.Linq;
using System.Text;
public class TimeSystem : MonoBehaviour
{
    public static TimeSystem instance;
    private int TIMESCALE; 
    private TimeSimulated timeGame;
    private TimeSimulated timeAdd;

    [SerializeField] private Text dayText;
    [SerializeField] private Text monthText;
    [SerializeField] private Text seasonText;
    [SerializeField] private Text yearText;
    [SerializeField] private Text weekText;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
        timeGame = new TimeSimulated(28, 12, 1399);
        TIMESCALE = 6;
        if (GlobalVariables.instance != null)
        {
            timeGame = GlobalVariables.instance.GovernorChoose.TimeInit;
            TIMESCALE = GlobalVariables.instance.TimeScale;
        }
        TextCallFunction();
        PlusDaysToTimeAdd();
        GameEvents.instance.onGatherGoldTriggerEnter += onGatherGold;
        GameEvents.instance.onGatherGoldTriggerExit += onGatherExit;
      //  GameEvents.instance.onGatherFoodTriggerEnter += onGatherFood;
      //  GameEvents.instance.onGatherFoodTriggerExit += onGatherExit;
    }
    private void TextCallFunction()
    {
        if(timeGame.day < 9)
        {
            dayText.text = "0" + timeGame.day.ToString();
        }
        else
        {
            dayText.text = timeGame.day.ToString();
        }
        if (timeGame.month < 9)
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
    void CalculateTime(TimeSimulated _time)
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
        GatherGoldTime();
    }

    void PlusDaysToTimeAdd()
    {
        timeAdd = new TimeSimulated(timeGame.day,timeGame.month,timeGame.year);
        timeAdd.PlusDays(3);
        CalculateTime(timeAdd);
    }
    private void GatherGoldTime()
    {
        if (timeGame.EqualsDate(timeAdd))
        {
            GameEvents.instance.GatherGoldTriggerEnter();
           // GameEvents.instance.GatherFoodTriggerEnter();
            PlusDaysToTimeAdd();
        }
        else
        {
            GameEvents.instance.GatherGoldTriggerExit();
           // GameEvents.instance.GatherFoodTriggerExit();
        }
    }

    private void onGatherExit()
    {
    }
    private void onGatherGold()
    {
        InGameMenuHandler.instance.GatherGoldResource();
    }
    private void onGatherFood()
    {
      //  InGameMenuHandler.instance.GatherFoodResource();
    }
}
