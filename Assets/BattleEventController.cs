using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BattleEventController : MonoBehaviour
{
    public static BattleEventController instance { get; private set; }
    private float timerCountDown1;
    private float timerCountDown2;
    [SerializeField] private Button FirstButtonEvent;
    [SerializeField] private Button SecondButtonEvent;
    [SerializeField] private Button ThirdButtonEvent;
    [SerializeField] private GameObject buttonsGO;
    [SerializeField] private int progress = 0;
    [SerializeField] private List<string> data = new List<string>();

    TextMeshProUGUI firstButtonTxt;
    TextMeshProUGUI secondButtonTxt;
    TextMeshProUGUI thirdButtonTxt;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        firstButtonTxt = FirstButtonEvent.transform.GetComponentInChildren<TextMeshProUGUI>();
        secondButtonTxt = SecondButtonEvent.transform.GetComponentInChildren<TextMeshProUGUI>();
        thirdButtonTxt = ThirdButtonEvent.transform.GetComponentInChildren<TextMeshProUGUI>();
    }



    public void Init(CustomBattle _customBattle)
    {
        waitTurnOff = false;
        waitButton = true;
        timerCountDown1 = 2.5f;
        timerCountDown2 = 2.5f;
        data.Clear();
        progress = 0;
        ChapterController.instance.speechSystemButtons.SetActive(false);
        firstButtonTxt.text = _customBattle.Button1;
        secondButtonTxt.text = _customBattle.Button2;
        thirdButtonTxt.text = _customBattle.Button3;
        FirstButtonEvent.onClick.RemoveAllListeners();
        SecondButtonEvent.onClick.RemoveAllListeners();
        ThirdButtonEvent.onClick.RemoveAllListeners();
        FirstButtonEvent.onClick.AddListener(() => AcceptCustomEventButton(_customBattle));
        SecondButtonEvent.onClick.AddListener(() => DeclineCustomEventButton(_customBattle));
        ThirdButtonEvent.onClick.AddListener(() => ThirdOptionCustomEventButton(_customBattle));
        data.Add("Pachacuti \"" + _customBattle.MessageEvent + "\"");
        
        HandleLineInBattleEvent();
    }

    void AcceptCustomEventButton(CustomBattle _customBattle)
    {
        _customBattle.AcceptEventAction();
        FinishCustomEventAppearance(_customBattle);
    }
    void DeclineCustomEventButton(CustomBattle _customBattle)
    {
        _customBattle.AcceptEventAction();
        FinishCustomEventAppearance(_customBattle);
    }
    void ThirdOptionCustomEventButton(CustomBattle _customBattle)
    {
        _customBattle.AcceptEventAction();
        FinishCustomEventAppearance(_customBattle);
    }
    bool waitTurnOff;
    bool waitButton;
    void FinishCustomEventAppearance(CustomBattle _customBattle)
    {        
        data.Add("Pachacuti \"" + _customBattle.ResultsEvent() + "\"");
        progress++;
        ChapterController.instance.speechSystemButtons.SetActive(false);
        HandleLineInBattleEvent();
        waitTurnOff = true;
        waitButton = false;
    }
    void HandleLineInBattleEvent()
    {
        ChapterController.instance.speechSystemRoot.SetActive(true);
        ChapterController.instance.HandleLine(data[progress]);
    }
    private void Update()
    {
        if (waitButton)
        {
            if (timerCountDown1 > 0)
            {
                timerCountDown1 -= Time.deltaTime;
            }
            else
            {
                ChapterController.instance.speechSystemButtons.SetActive(true);
            }
        }

        if (waitTurnOff)
        {
            if (timerCountDown2 > 0)
            {
                timerCountDown2 -= Time.deltaTime;
            }
            else
            {
                ChapterController.instance.speechSystemRoot.SetActive(false);
            }
        }
    }

}


