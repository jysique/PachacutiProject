using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DateTableHandler : MonoBehaviour
{
    public static DateTableHandler instance;
    [SerializeField] private GameObject pauseGO;
    [SerializeField] private Button PauseBtn;
    [SerializeField] private Button PlayBtn;
    [SerializeField] private Button Quicknessx1Btn;
    [SerializeField] private Button Quicknessx2Btn;

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
        PlayBtn.onClick.AddListener(() => PlayButton());
        Quicknessx1Btn.onClick.AddListener(() => Quicknessx1Button());
        Quicknessx2Btn.onClick.AddListener(() => Quicknessx2Button());
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
        pauseGO.SetActive(true);
        if (!isMenuPaused && !isTimePaused)
        {

            PauseTime();
            isTimePaused = true;
        }
    }
    public void PlayButton()
    {
        if (GlobalVariables.instance.timeModifier==0)
        {
            pauseGO.SetActive(false);
            if (!isMenuPaused && isTimePaused)
            {
                ResumeTime();
                isTimePaused = false;
            }
        }
        else
        {
            GlobalVariables.instance.timeModifier = 1;
        }
        
    }
    public void Quicknessx1Button()
    {
        if (!isTimePaused)
        {
            GlobalVariables.instance.timeModifier = 1.5f;
        } 
    }
    public void Quicknessx2Button()
    {
        if (!isTimePaused)
        {
            GlobalVariables.instance.timeModifier = 2f;
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
        // GlobalVariables.instance.timeModifier = temporalTime;
        GlobalVariables.instance.timeModifier = 1;
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
