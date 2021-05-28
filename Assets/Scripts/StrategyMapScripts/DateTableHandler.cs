using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DateTableHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI monthText;
    [SerializeField] private TextMeshProUGUI seasonText;
    [SerializeField] private TextMeshProUGUI yearText;
    [SerializeField] private TextMeshProUGUI weekText;
    private TimeSimulated timeGame;
    void Start()
    {
        timeGame = TimeSystem.instance.TimeGame;
        TextCallFunction();
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


    void Update()
    {
        TextCallFunction();
        TimeSystem.instance.CalculateTime(timeGame);
    }
}
