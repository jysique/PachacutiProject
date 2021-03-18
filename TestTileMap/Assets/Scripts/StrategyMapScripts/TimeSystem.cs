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
    private const int TIMESCALE = 10; // Tiempo escalable para la modificacion del ritmo del tiempo
    [SerializeField]private Text dayText;
    [SerializeField]private Text monthText;
    [SerializeField]private Text seasonText;
    [SerializeField]private Text yearText;
    [SerializeField] private Text weekText;
    bool isEvent = false;
    //public static double hour, day, month, year;
    public static TimeSimulated timeGame;

    void Start()
    {
        timeGame = new TimeSimulated(0, 28, 12, 1399);
        TextCallFunction();
    }

    void FixedUpdate()
    {
        CalculateTime();
        EventoRandom();
        
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
        yearText.text = timeGame.year.ToString();
        weekText.text = timeGame.week.ToString();
        seasonText.text = timeGame.season;

    }
    /*
    void CalculateSeason()
    {
        if (month == 12 || month == 1 || month == 2)
        {
            seasonText.text = "Temporada 1";
        }
        if (month == 3 || month == 4 || month == 5)
        {
            seasonText.text = "Temporada 2";
        }
        if (month == 6 || month == 7|| month == 8)
        {
            seasonText.text = "Temporada 3";
        }
        if (month == 9 || month == 10 || month == 11)
        {
            seasonText.text = "Temporada 4";
        }

    }
    
    void CalculateYear()
    {
        
        year++;
        month = 1;
        TextCallFunction();
    }
    void CalculateMonth()
    {
        month++;
        day = 1;
        TextCallFunction();
    }
    void CalculateDay()
    {
        hour = 1;
        day++;
        TextCallFunction();
    }
    */
    void CalculateTime()
    {
        //print("dia " + timeGame.day.ToString() + "mes: " + timeGame.month.ToString()+ "año: " + timeGame.year.ToString());
        //print("dia: " + timeGame.day.ToString()+ "hora: " + timeGame.hour.ToString());
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

    void EventoRandom()
    {
        TimeSimulated time1 = new TimeSimulated(0, 2, 1, 1400);
        if(timeGame.EqualsDate(time1))
        {
            Debug.Log("EVENTO RANDOM");
        }
        
    }
}
