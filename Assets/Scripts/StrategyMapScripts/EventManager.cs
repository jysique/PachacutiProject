using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    [Header("Event Menu")]
    [SerializeField] private GameObject CustomEventSelection;
    [SerializeField] private TextMeshProUGUI TitleTextCustomEvent;
    [SerializeField] private TextMeshProUGUI DetailsTextCustomEvent;
    [SerializeField] private TextMeshProUGUI ResultsTextEvent;
    [SerializeField] private Button AcceptEventButton;
    [SerializeField] private Button CloseEventButton;
    [SerializeField] private Button DeclineEventButton;
    [SerializeField] private GameObject ResultsEvent;
    [SerializeField] private GameObject notificationEvent;
    [SerializeField] private GameObject CustomEventList;

    [SerializeField] private TextMeshProUGUI MotivationTxt;
    [SerializeField] private TextMeshProUGUI OpinionTxt;
    [SerializeField] private Button goToBtn;

    [Header("Listas de Eventos")]
    public CustomEventList listEvents;
    private TimeSimulated timeAddEvent;

    private int minDays = 10;
    private int maxDays = 15;
    private bool init = false;
    //public int current = 0;
    public void SetNotificationEvent(bool active)
    {
        notificationEvent.SetActive(active);
    }
    public int MinDays
    {
        get { return minDays; }
    }
    public int MaxDays
    {
        get { return maxDays; }
    }
    private void Update()
    {
        if (init)
        {
            CustomEventInTime();
            CheckListCustomEvent();
        }
        CheckListBuildingsUpgrade();
    }
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        CloseEventButton.onClick.AddListener(() => CloseCustomEventButton());
        listEvents = new CustomEventList();
        SetNotificationEvent(false);
    }
    public void InitEvents()
    {
        AddEvent();
        init = true;
    }
    /// <summary>
    /// Add a new Event to the list of events
    /// </summary>
    private void AddEvent()
    {
        timeAddEvent = new TimeSimulated(TimeSystem.instance.TimeGame);
        listEvents.AddCustomEvent(timeAddEvent);
        int rAddPlusDays = Random.Range(minDays, maxDays);
        timeAddEvent.PlusDays(rAddPlusDays);
    }
    public void UpdateEventListOption()
    {
        Transform gridLayout = CustomEventList.transform.Find("ScrollArea/ScrollContainer/GridLayout").transform;
        foreach (Transform child in gridLayout.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (CustomEvent customEvent in listEvents.CustomEvents)
        {
            InstantiateCustomEventOption(customEvent, gridLayout);

        }
        foreach (CustomExpedition customEvent in listEvents.ExpedicionEvents)
        {
            InstantiateCustomEventOption(customEvent, gridLayout);
        }
    }
    public void InstantiateCustomEventOption(CustomEvent _customEvent, Transform _gridLayout)
    {
        GameObject customEventOption = Instantiate(Resources.Load("Prefabs/MenuPrefabs/CustomEventOption")) as GameObject;
        customEventOption.transform.SetParent(_gridLayout.transform, false);
        customEventOption.GetComponent<CustomEventOption>().Custom = _customEvent;
        if (_customEvent.EventStatus == CustomEvent.STATUS.ANNOUNCE)
        {
            DestroyImmediate(customEventOption);
        }
        else if (_customEvent.EventStatus == CustomEvent.STATUS.FINISH)
        {
            Destroy(customEventOption, 1);
        }
    }

    public void FinishCustomEventAppearance(CustomEvent custom)
    {
        InitCustomEvent(custom);
        AcceptEventButton.gameObject.SetActive(false);
        DeclineEventButton.gameObject.SetActive(false);
        TitleTextCustomEvent.text = GameMultiLang.GetTraduction("ResultEvent");
        ResultsEvent.SetActive(true);
        ResultsTextEvent.text = GameMultiLang.GetTraduction("Results")+"\n" + custom.ResultsEvent();
        DetailsTextCustomEvent.text = custom.ResultMessagetEvent;
        DateTableHandler.instance.PauseTime();
    }
    public void WarningEventAppearance(CustomEvent custom, int daysToFinal)
    {
        InitCustomEvent(custom);
        DateTableHandler.instance.PauseTime();
        CloseEventButton.gameObject.SetActive(true);
        DetailsTextCustomEvent.text = custom.MessageEvent + "\n"+GameMultiLang.GetTraduction("TimeRemaining").Replace("%", daysToFinal.ToString());
        MotivationTxt.text = ":" + custom.TerritoryEvent.TerritoryStats.Territory.MotivationTerritory;
        OpinionTxt.text = ":" + custom.TerritoryEvent.TerritoryStats.Territory.OpinionTerritory;
        goToBtn.onClick.AddListener(() => GoToButton(custom));
    }
    private void GoToButton(CustomEvent custom)
    {
        CloseCustomEventButton();
        GlobalVariables.instance.CenterCameraToTerritory(custom.TerritoryEvent,false);
    }
    public void InitCustomEvent(CustomEvent custom)
    {
        CustomEventSelection.gameObject.SetActive(true);
        ResetTextCustomEvent();
        if (!custom.GetAcceptButton())
        {
            AcceptEventButton.interactable = false;
        }
        else
        {
            AcceptEventButton.interactable = true;
        }
        if (custom.EventType == CustomEvent.EVENTTYPE.EXPLORATION)
        {
            DeclineEventButton.gameObject.SetActive(false);
            AcceptEventButton.gameObject.SetActive(false);
        }
        ResultsTextEvent.text = custom.RequirementMessageEvent;
        AcceptEventButton.onClick.AddListener(() => AcceptCustomEventButton(custom));
        DeclineEventButton.onClick.AddListener(() => DeclineCustomEventButton(custom));
    }
    public void AcceptCustomEventButton(CustomEvent custom)
    {
        custom.AcceptEventAction();
        InGameMenuHandler.instance.UpdateMenu();
        FinishCustomEventAppearance(custom);
    }
    public void DeclineCustomEventButton(CustomEvent custom)
    {
        custom.DeclineEventAction();
        InGameMenuHandler.instance.UpdateMenu();
        FinishCustomEventAppearance(custom);
    }
    public void CloseCustomEventButton()
    {
        DateTableHandler.instance.ResumeTime();
        CustomEventSelection.gameObject.SetActive(false);
        ResetTextCustomEvent();
        UpdateEventListOption();
    }
    private void ResetTextCustomEvent()
    {
        AcceptEventButton.gameObject.SetActive(true);
        DeclineEventButton.gameObject.SetActive(true);
        CloseEventButton.gameObject.SetActive(true);
        DeclineEventButton.gameObject.SetActive(true);
        AcceptEventButton.gameObject.SetActive(true);
        DetailsTextCustomEvent.text = " ";
        TitleTextCustomEvent.text = GameMultiLang.GetTraduction("EventLabel");
    }
    
    /// <summary>
    /// Add a new UpgradeBuilding Event to the list
    /// </summary>
    /// <param name="territoryHandler">That territory improvement</param>
    /// <param name="building">That building improve</param>
    public void AddEvent(TerritoryHandler territoryHandler, Building building)
    {
        listEvents.AddBuildingEvent(TimeSystem.instance.TimeGame, territoryHandler, building);
    }
    public void AddEvent(TerritoryHandler territoryHandler)
    {
        listEvents.AddCustomEvent(TimeSystem.instance.TimeGame, territoryHandler);
    }
    public void AddExpedicionEvent(TerritoryHandler attacker , TerritoryHandler wasteTerritory, Troop troopToWaste)
    {
        listEvents.AddExpedicionEvent(TimeSystem.instance.TimeGame,troopToWaste, attacker, wasteTerritory);
        SetNotificationEvent(true);
    }
    /// <summary>
    /// Check the timeGame with the timeAddEvent
    /// if is the same active the event to add a new custom event
    /// </summary>
    private void CustomEventInTime()
    {
        if (TimeSystem.instance.TimeGame.EqualsDate(timeAddEvent))
        {
            AddEvent();
        }
        else
        {
            GameEvents.instance.CustomEventExit();
        }
    }

    /// <summary>
    /// check the status of the events in the LIST-EVENTS according 
    /// to the TIME-INIT and TIME-FINISH of every event comparing with the timeGame
    /// state one - if is ANNOUNCE and timeGame is equals to timeInit of enent -> PROGRESS (warning event)
    /// state two - if is in PROGRESS and timeGame is equales to timeFinish of event -> FINISH (finish event)
    /// Also corroborate notification status
    /// </summary>
    private void CheckListCustomEvent()
    {
        for (int i = 0; i < listEvents.CustomEvents.Count; i++)
        {
            if (listEvents.CustomEvents[i].TerritoryEvent.TerritoryStats.Territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
            {

                if (listEvents.CustomEvents[i].EventStatus == CustomEvent.STATUS.ANNOUNCE)
                {
                    if (TimeSystem.instance.TimeGame.EqualsDate(listEvents.CustomEvents[i].TimeInitEvent))
                    {
                        listEvents.CustomEvents[i].EventStatus = CustomEvent.STATUS.PROGRESS;
                        WarningCustomEvent();
                    }
                }
                else if (TimeSystem.instance.TimeGame.EqualsDate(listEvents.CustomEvents[i].TimeFinalEvent) && listEvents.CustomEvents[i].EventStatus == CustomEvent.STATUS.PROGRESS)
                {
                    FinishCustomEvent(listEvents.CustomEvents[i]);
                }
                if (listEvents.CustomEvents[i].TimeFinalEvent.DiferenceDays(TimeSystem.instance.TimeGame) == 1 && !listEvents.CustomEvents[i].W)
                {
                    listEvents.CustomEvents[i].W = true;
                    AlertManager.AlertEventEnd();
                }
            }
            else
            {
                listEvents.RemoveEvent(listEvents.CustomEvents[i]);
                UpdateEventListOption();
            }
        }

        for (int i = 0; i < listEvents.ExpedicionEvents.Count; i++)
        {
            if (TimeSystem.instance.TimeGame.EqualsDate(listEvents.ExpedicionEvents[i].TimeFinalEvent) && listEvents.ExpedicionEvents[i].EventStatus == CustomEvent.STATUS.PROGRESS)
            {
                FinishCustomEvent(listEvents.ExpedicionEvents[i]);
                listEvents.ExpedicionEvents[i].ReturnUnits();
            }
        }
    }
    
    /// <summary>
    /// check the diference days of the UPGRADE-BUILDS events in the list according
    /// to the TIME-INIT of the upgrade of every event comparing with the timeGame
    /// if is the same update the daysTotal to 0, can Upgrade the builds again, improve the 
    /// building and remove the same event
    /// </summary>
    /// 
    private void CheckListBuildingsUpgrade()
    {
        for (int i = 0; i < listEvents.BuildingsEvents.Count; i++)
        {
            int diferenceDays = TimeSystem.instance.TimeGame.DiferenceDays(listEvents.BuildingsEvents[i].TimeInitEvent);
            listEvents.BuildingsEvents[i].BuildingEvent.DaysTotal = diferenceDays;
            if (diferenceDays == listEvents.BuildingsEvents[i].BuildingEvent.DaysToBuild)
            {
                listEvents.BuildingsEvents[i].FinishUpgradeBuilding(1);
                listEvents.RemoveEvent(listEvents.BuildingsEvents[i]);
            }
            else
            {
                listEvents.BuildingsEvents[i].BuildingEvent.CanUpdrade = false;
            }
        }
    }
    /// <summary>
    /// Appears the Event Menu if it is in finish status
    /// </summary>
    /// <param name="id"></param>
    private void FinishCustomEvent(CustomEvent custom)
    {
        custom.EventStatus = CustomEvent.STATUS.FINISH;
        FinishCustomEventAppearance(custom);
        custom.DeclineEventAction();
    }
    /// <summary>
    /// Appears the Event Menu if it is in warning of init status
    /// </summary>
    private void WarningCustomEvent()
    {
        UpdateEventListOption();
        SetNotificationEvent(true);
        AlertManager.AlertEvent();
    }
    public bool GetIsTerritoriesIsInPandemic(TerritoryHandler territoryEvent)
    {
        for (int i = 0; i < listEvents.CustomEvents.Count; i++)
        {
            if (listEvents.CustomEvents[i].EventStatus == CustomEvent.STATUS.PROGRESS && listEvents.CustomEvents[i].EventType == CustomEvent.EVENTTYPE.PANDEMIC && listEvents.CustomEvents[i].TerritoryEvent == territoryEvent)
            {
                return true;
            }
        }
        return false;
    }
    public bool GetIsTerritoriesIsInPandemic()
    {
        for (int i = 0; i < listEvents.CustomEvents.Count; i++)
        {
            if (listEvents.CustomEvents[i].EventStatus == CustomEvent.STATUS.PROGRESS && listEvents.CustomEvents[i].EventType == CustomEvent.EVENTTYPE.ALL_T_PANDEMIC)
            {
                return true;
            }
        }
        return false;
    }
}
