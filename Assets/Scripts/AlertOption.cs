using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AlertOption : MonoBehaviour
{
    [SerializeField] Button closeAlertBtn;
    [SerializeField] Button alertBtn;
    [SerializeField] TextMeshProUGUI tittleAlertText;
    [SerializeField] TextMeshProUGUI suggertAlertText;
    [SerializeField] GameObject container;
   // public float timeLearping;
   // public float learpTime;
    private Vector2 startPosition;
    private Vector2 endPosition ;
    private string type;
    Animator anim;
    RectTransform a;
    TimeSimulated timeInit;
    void Start()
    {
        anim = container.GetComponent<Animator>();
        timeInit = new TimeSimulated(TimeSystem.instance.TimeGame);
        timeInit.PlusDays(3);
        //a.anchoredPosition= new Vector2(392,0);

        startPosition = new Vector3(392, 0);
        endPosition= new Vector3(0, 0, 0);
        closeAlertBtn.onClick.AddListener(() => CloseAlertBtn());
        alertBtn.onClick.AddListener(() => OpenTabEvent());
       // timeLearping = Time.time;

        a = container.GetComponent<RectTransform>();
        
    }
    public void Init(string tittle,string suggest,string type)
    {
        tittleAlertText.text = tittle;
        suggertAlertText.text = suggest;
        this.type = type;
    }
    void CloseAlertBtn()
    {
        anim.SetBool("Appeance", false);
        Destroy(this.gameObject,2.5f);
    }
    void OpenTabEvent()
    {
        CloseAlertBtn();
        if (type== "ALERT2")
        {
            AlertManager.TabEventMenu();
        }
        else
        {
            AlertManager.TabMissionMenu();
        }
        
    }
    private void Update()
    {
        CloseAlertByTime();
    }
    void CloseAlertByTime()
    {
        if (timeInit.EqualsDate(TimeSystem.instance.TimeGame))
        {
            CloseAlertBtn();
        }
    }
    /*
    private void Update()
    {
        a.localPosition = Learp(startPosition, endPosition, timeLearping, learpTime);
    }

    private Vector2 Learp(Vector3 start, Vector3 end, float _timeStarted, float learpTime = 1)
    {
        float _time = Time.time - _timeStarted;
        float percentage = _time / learpTime;
        print(percentage);
        var result = Vector3.Lerp(start, end, percentage);
        return result;
    }*/
}
