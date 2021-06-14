using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DateTableHandler : MonoBehaviour
{
    public static DateTableHandler instance;
    [SerializeField] private Button PauseBtn;
    [SerializeField] private Button QuicknessBtn;
    [SerializeField] private Button SlownessBtn;

    [SerializeField] private TextMeshProUGUI dayText;
    [SerializeField] private TextMeshProUGUI monthText;
    [SerializeField] private TextMeshProUGUI seasonText;
    [SerializeField] private TextMeshProUGUI yearText;
    [SerializeField] private TextMeshProUGUI weekText;
    [SerializeField] private Image counterDay;
    private TimeSimulated timeGame;
    public float temporalTime;
    private bool isTimePaused = false;
    private bool isMenuPaused = false;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        timeGame = TimeSystem.instance.TimeGame;
        TextCallFunction();
        PauseBtn.onClick.AddListener(() => PauseButton());
        QuicknessBtn.onClick.AddListener(() => QuicknessButton());
        SlownessBtn.onClick.AddListener(() => SlownessButton());
    }
    void Update()
    {
        counterDay.fillAmount = (float)timeGame.Hour / 24.0f;
        TextCallFunction();
        CalculateTimeInUpdate(timeGame);
        MenuEscapeGame();
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
    public void CalculateTimeInUpdate(TimeSimulated _time)
    {

        if (_time.Hour < 24)
        {
            _time.Hour += Time.deltaTime * GlobalVariables.instance.TimeScale;
        }
        else
        {
            _time.CalculateDay();
            _time.Hour = 0;
        }
        _time.CalculateSeason();
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

    public void PauseButton()
    {
        if (!isMenuPaused)
        {
            if (!isTimePaused)
            {
                PauseTime();
                isTimePaused = true;
            }
            else
            {
                ResumeTime();
                isTimePaused = false;
            }
        }
    }
    public void QuicknessButton()
    {
        if (GlobalVariables.instance.timeModifier <= 10 && !isTimePaused)
        {
            GlobalVariables.instance.timeModifier += 0.2f;
        }
        
    }
    public void SlownessButton()
    {
        if (GlobalVariables.instance.timeModifier >= 0.6 && !isTimePaused)
        {
            print(isTimePaused);
            GlobalVariables.instance.timeModifier -= 0.2f;
        }
    }

    public void PauseTime()
    {
        MenuManager.instance.turnOffMenus();
        if (GlobalVariables.instance.timeModifier != 0)
            temporalTime = GlobalVariables.instance.timeModifier;
        GlobalVariables.instance.timeModifier = 0;
    }
    public void ResumeTime()
    {
        GlobalVariables.instance.timeModifier = temporalTime;
    }

    public void MenuEscapeGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isTimePaused)
            {
                MenuManager.instance.ResumeMenuGame();
                ResumeTime();
                isTimePaused = false;
                isMenuPaused = false;
            }
            else
            {
                MenuManager.instance.PauseMenuGame();
                PauseTime();
                isTimePaused = true;
                isMenuPaused = true;
            }
        }
    }
}
