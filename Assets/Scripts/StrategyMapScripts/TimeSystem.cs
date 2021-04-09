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
    private int TIMESCALE; // Tiempo escalable para la modificacion del ritmo del tiempo
    [SerializeField]private Text dayText;
    [SerializeField]private Text monthText;
    [SerializeField]private Text seasonText;
    [SerializeField]private Text yearText;
    [SerializeField] private Text weekText;
    public bool isPause = false;
    //bool isEvent = false;
    //public static double hour, day, month, year;
    public static TimeSimulated timeGame;
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
            timeGame = GlobalVariables.instance.governorChoosen.TimeInit;
            TIMESCALE = 6 * (GlobalVariables.instance.velocityGame + 1);
        }
        TextCallFunction();
    }

    void Update()
    {
        CalculateTime();
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

    void CalculateTime()
    {
        TextCallFunction();
        timeGame.CalculateSeason();
        if(timeGame.hour < 10)
        {
            timeGame.hour += Time.deltaTime * TIMESCALE;
        }
        else
        {
            timeGame.CalculateDay();
            timeGame.hour = 0;
        }
        timeGame.CalculateWeeks();
        if (timeGame.day > 30)
        {
            timeGame.CalculateMonth();  
        }
        if (timeGame.month > 12)
        {
            timeGame.CalculateYear();
        }
    }

    void EventoRandom(TimeSimulated _time)
    {
        if(timeGame.EqualsDate(_time))
        {
            //Debug.Log("EVENTO RANDOM");
        }
    }
}
