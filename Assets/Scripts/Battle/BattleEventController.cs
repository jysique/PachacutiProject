using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BattleEventController : MonoBehaviour
{
    public static BattleEventController instance { get; private set; }
    [SerializeField] private Button FirstButtonEvent;
    [SerializeField] private Button SecondButtonEvent;
    [SerializeField] private Button ThirdButtonEvent;

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
        firstButtonTxt.text = _customBattle.Decision1;
        secondButtonTxt.text = _customBattle.Decision2;
        thirdButtonTxt.text = _customBattle.Decision3;
        FirstButtonEvent.onClick.RemoveAllListeners();
        SecondButtonEvent.onClick.RemoveAllListeners();
        ThirdButtonEvent.onClick.RemoveAllListeners();
        FirstButtonEvent.onClick.AddListener(() => AcceptCustomEventButton(_customBattle));
        SecondButtonEvent.onClick.AddListener(() => DeclineCustomEventButton(_customBattle));
        ThirdButtonEvent.onClick.AddListener(() => ThirdOptionCustomEventButton(_customBattle));

        ChapterController.instance.speechSystemRoot.SetActive(true);
        ChapterController.instance.backgroundRoot.SetActive(true);
        ChapterController.instance.speechSystemButtons.SetActive(false);
        string setbackground = "setBackground("+_customBattle.Background+",false,10)";
        ChapterController.instance.HandleLine(setbackground);
        ChapterController.instance.HandleLine("Pachacuti \"" + _customBattle.Cause + "\"");
        StartCoroutine(WaitingText());
    }

    IEnumerator WaitingText()
    {
        yield return new WaitForSeconds(4f);
        ChapterController.instance.speechSystemButtons.SetActive(true);
    }
    void AcceptCustomEventButton(CustomBattle _customBattle)
    {
        _customBattle.AcceptEventAction();
        StartCoroutine(IndicentEvent(_customBattle,true));
    }
    void DeclineCustomEventButton(CustomBattle _customBattle)
    {
        _customBattle.DeclineEventAction();
        StartCoroutine(IndicentEvent(_customBattle,true));
    }
    void ThirdOptionCustomEventButton(CustomBattle _customBattle)
    {
        _customBattle.AdicitionalEventAction();
        StartCoroutine(IndicentEvent(_customBattle,false));
    }
    IEnumerator IndicentEvent(CustomBattle _customBattle, bool isEffect)
    {
        ChapterController.instance.speechSystemButtons.SetActive(false);
        ChapterController.instance.HandleLine("Pachacuti \"" + _customBattle.Incident + "\"");
        yield return new WaitForSeconds(5.0f);
        if (isEffect)
        {
            ChapterController.instance.speechSystemRoot.SetActive(false);
            ChapterController.instance.HandleLine("setBackground(after,false,10)");
            yield return new WaitForSeconds(1f);
            ChapterController.instance.speechSystemRoot.SetActive(true);
            ChapterController.instance.HandleLine("setBackground(" + _customBattle.Background + ",false,10)");
            ChapterController.instance.HandleLine("Pachacuti \"" + _customBattle.Effect + "\"");
            yield return new WaitForSeconds(5.0f);
        }
        EndEvent(_customBattle);
    }
    void EndEvent(CustomBattle customBattle)
    {
        ChapterController.instance.speechSystemRoot.SetActive(false);
        ChapterController.instance.speechSystemButtons.SetActive(false);
        ChapterController.instance.backgroundRoot.SetActive(false);
        customBattle.CloseEventAction();
    }
}


