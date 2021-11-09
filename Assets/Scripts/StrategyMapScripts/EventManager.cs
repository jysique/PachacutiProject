using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI TitleTextCustomEvent;
    [SerializeField] private TextMeshProUGUI DetailsTextCustomEvent;
    [SerializeField] private TextMeshProUGUI MotivationTxt;
    [SerializeField] private TextMeshProUGUI OpinionTxt;
    private TextMeshProUGUI FirstOptionTxt;
    private TextMeshProUGUI SecondOptionTxt;
    private TextMeshProUGUI ThirdOptionTxt;

    [Header("Buttons")]
    [SerializeField] private Button FirstOptionButton;
    [SerializeField] private Button SecondOptionButton;
    [SerializeField] private Button ThirdOptionButton;
    [SerializeField] private Button goToBtn;

    [Header("GameObjects")]
    [SerializeField] private GameObject CustomEventSelection;
    [SerializeField] private GameObject notificationEvent;
    [SerializeField] private GameObject CustomEventList;
    [SerializeField] private GameObject TerritoryInfo;

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
        CheckListCreatingUnits();
    }
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        FirstOptionTxt = FirstOptionButton.GetComponentInChildren<TextMeshProUGUI>();
        SecondOptionTxt = SecondOptionButton.GetComponentInChildren<TextMeshProUGUI>();
        ThirdOptionTxt = ThirdOptionButton.GetComponentInChildren<TextMeshProUGUI>();
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
        FirstOptionButton.gameObject.SetActive(false);
        SecondOptionButton.gameObject.SetActive(false);
        ThirdOptionTxt.text = "Close";
        TitleTextCustomEvent.text = GameMultiLang.GetTraduction("ResultEvent") ;
        custom.ResultsEvent();
        DetailsTextCustomEvent.text = custom.ResultMessageEvent + "\n" + GameMultiLang.GetTraduction("Results") + "\n" + custom.Effect;
        if (!custom.IsBattle)
        {
            DateTableHandler.instance.PauseTime();
        }
        else
        {
            ThirdOptionButton.onClick.RemoveAllListeners();
            ThirdOptionButton.onClick.AddListener(() => CloseInEvent(custom));
            ThirdOptionButton.onClick.AddListener(() => Close());
        } 
    }
    private void CloseInEvent(CustomEvent custom)
    {
        custom.CloseEventAction();
    }
    public void WarningEventAppearance(CustomEvent custom, int daysToFinal)
    {
        InitCustomEvent(custom);
        DateTableHandler.instance.PauseTime();
        TitleTextCustomEvent.text = custom.NameEvent;
        DetailsTextCustomEvent.text = custom.Cause + "\n"+GameMultiLang.GetTraduction("TimeRemaining").Replace("%", daysToFinal.ToString()) + "\n"
            + custom.RequirementMessageEvent;
        MotivationTxt.text = ":" + custom.TerritoryEvent.Territory.MotivationTerritory;
        OpinionTxt.text = ":" + custom.TerritoryEvent.Territory.OpinionTerritory;
        goToBtn.onClick.AddListener(() => GoToButton(custom));
    }
    public void WarningEventAppearanceInBattle(CustomEvent custom)
    {
        InitCustomEvent(custom);
        TitleTextCustomEvent.text = custom.NameEvent;
        DetailsTextCustomEvent.text = custom.Cause;
    }
    private void GoToButton(CustomEvent custom)
    {
        ThirdOptionCustomEventButton(custom);
        GlobalVariables.instance.CenterCameraToTerritory(custom.TerritoryEvent,false);
    }
    
    public void InitCustomEvent(CustomEvent custom)
    {
        CustomEventSelection.gameObject.SetActive(true);
        ResetTextCustomEvent();
        FirstOptionTxt.text = custom.Decision1;
        SecondOptionTxt.text = custom.Decision2;
        ThirdOptionTxt.text = custom.Decision3;

        FirstOptionButton.interactable = custom.GetAcceptButton();
        if (custom.IsBattle)
        {
            TerritoryInfo.SetActive(false);
        }
        else
        {
            TerritoryInfo.SetActive(true);
        }
        if (custom.EventType == CustomEvent.EVENTTYPE.WASTE)
        {
            FirstOptionButton.gameObject.SetActive(false);
            FirstOptionButton.gameObject.SetActive(false);
        }
        FirstOptionButton.onClick.RemoveAllListeners();
        SecondOptionButton.onClick.RemoveAllListeners();
        ThirdOptionButton.onClick.RemoveAllListeners();
        FirstOptionButton.onClick.AddListener(() => AcceptCustomEventButton(custom));
        SecondOptionButton.onClick.AddListener(() => DeclineCustomEventButton(custom));
        ThirdOptionButton.onClick.AddListener(() => ThirdOptionCustomEventButton(custom));
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
    public void Close()
    {
        CustomEventSelection.gameObject.SetActive(false);
    }
    public void ThirdOptionCustomEventButton(CustomEvent custom)
    {
        Close();
        if (!custom.IsBattle)
        {
          //  print("1");
            DateTableHandler.instance.ResumeTime();
            InGameMenuHandler.instance.UpdateMenu();
            ResetTextCustomEvent();
            UpdateEventListOption();
        }
        else
        {
            FinishCustomEventAppearance(custom);
            custom.AdicitionalEventAction();
        }

    }
    private void ResetTextCustomEvent()
    {
        FirstOptionButton.gameObject.SetActive(true);
        SecondOptionButton.gameObject.SetActive(true);
        ThirdOptionButton.gameObject.SetActive(true);
        DetailsTextCustomEvent.text = " ";
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

    public void AddEvent(TerritoryHandler territoryHandler, UnitCombat unitCombat)
    {
        listEvents.AddUnitEvent(TimeSystem.instance.TimeGame, territoryHandler, unitCombat);
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
    public void AddBattleEvents(Troop playerTroop, Troop enemyTroop, TerritoryHandler _playerTerritory, TerritoryHandler _enemyTerritory, bool isPlayerTerritory)
    {
        if (Random.Range(0, 100) >= 30)
            listEvents.AddBattleEvent("INIT", playerTroop, enemyTroop, _playerTerritory, _enemyTerritory, isPlayerTerritory);
        if (Random.Range(0, 100) >= 30)
            listEvents.AddBattleEvent("MIDDLE", playerTroop, enemyTroop, _playerTerritory, _enemyTerritory, isPlayerTerritory);
        if (Random.Range(0, 100) >= 30)
            listEvents.AddBattleEvent("FINAL", playerTroop, enemyTroop, _playerTerritory, _enemyTerritory, isPlayerTerritory);
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
            if (listEvents.CustomEvents[i].TerritoryEvent.Territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
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
            if (diferenceDays == listEvents.BuildingsEvents[i].BuildingEvent.DaysToBuild)
            {
                listEvents.BuildingsEvents[i].FinishUpgradeBuilding();
                listEvents.RemoveEvent(listEvents.BuildingsEvents[i]);
            }
            else
            {
                listEvents.BuildingsEvents[i].InProggressUpgrade();
            }
        }
    }
    private void CheckListCreatingUnits()
    {
        for (int i = 0; i < listEvents.UnitsEvents.Count; i++)
        {
            int diferenceDays = TimeSystem.instance.TimeGame.DiferenceDays(listEvents.UnitsEvents[i].TimeInitEvent);
            if (diferenceDays == listEvents.UnitsEvents[i].DaysToCreate)
            {
                listEvents.UnitsEvents[i].FinishAddingUnit();
                listEvents.RemoveEvent(listEvents.UnitsEvents[i]);
            }
            else
            {
                listEvents.UnitsEvents[i].AddingUnit();
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
