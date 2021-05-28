using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour
{
    public static EventManager instance;

    [Header("Evento")]
    [SerializeField] private GameObject CustomEventSelection;
    [SerializeField] private TextMeshProUGUI TitleTextCustomEvent;
    [SerializeField] private TextMeshProUGUI DetailsTextCustomEvent;
    [SerializeField] private TextMeshProUGUI ResultsTextEvent;
    [SerializeField] private Button AcceptEventButton;
    [SerializeField] private Button CloseEventButton;
    [SerializeField] private Button DeclineEventButton;
    [SerializeField] private GameObject ResultsEvent;
    private CustomEvent currentCustomEvent;
    

    [SerializeField] private GameObject CustomEventList;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitEventsButtons();
    }

    public void InstantiateEventListOption(CustomEventList customlist)
    {
        Transform gridLayout = CustomEventList.transform.Find("ScrollArea/ScrollContainer/GridLayout").transform;
        foreach (Transform child in gridLayout.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (CustomEvent customEvent in customlist.CustomEvents)
        {
            GameObject customEventOption = Instantiate(Resources.Load("Prefabs/MenuPrefabs/CustomEventOption")) as GameObject;
            customEventOption.transform.SetParent(gridLayout.transform, false);
            customEventOption.GetComponent<CustomEventOption>().Custom = customEvent;
            if (customEvent.EventStatus == CustomEvent.STATUS.ANNOUNCE)
            {
                DestroyImmediate(customEventOption);
            }
            else if (customEvent.EventStatus == CustomEvent.STATUS.FINISH)
            {
                Destroy(customEventOption, 1);
            }
        }

    }
    public void FinishCustomEventAppearance(CustomEvent custom)
    {
        InitCustomEvent(custom);
        AcceptEventButton.gameObject.SetActive(false);
        DeclineEventButton.gameObject.SetActive(false);
        TitleTextCustomEvent.text = "Results Event";
        if (custom.IsAccepted)
        {
            DetailsTextCustomEvent.text = "You complete the requirements of the " + custom.TerritoryEvent.name + " territory petition.";
        }
        else
        {
            DetailsTextCustomEvent.text = "You were unable to complete the requirements of the " + custom.TerritoryEvent.name + " territory petition.";
        }
        ResultsEvent.SetActive(true);
        ResultsTextEvent.text = "Results:\n" + custom.ResultsEvent();
        MenuManager.instance.PauseGame();
    }

    public void WarningEventAppearance(CustomEvent custom, int daysToFinal)
    {
        InitCustomEvent(custom);
        MenuManager.instance.PauseGame();
        CloseEventButton.gameObject.SetActive(true);
        DetailsTextCustomEvent.text = custom.MessageEvent + "\nTime remaining: " + daysToFinal + " days.";
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
        ResultsTextEvent.text = "Requirements:\n" + custom.RequirementMessageEvent;
        currentCustomEvent = custom;
    }
    public void AcceptCustomEventButton()
    {
        currentCustomEvent.AcceptEventAction();
        InGameMenuHandler.instance.UpdateMenu();
        FinishCustomEventAppearance(currentCustomEvent);
    }
    public void DeclineCustomEventButton()
    {
        currentCustomEvent.DeclineEventAction();
        InGameMenuHandler.instance.UpdateMenu();
        FinishCustomEventAppearance(currentCustomEvent);
    }
    public void CloseCustomEventButton()
    {
        MenuManager.instance.ResumeGame();
        CustomEventSelection.gameObject.SetActive(false);
        ResetTextCustomEvent();
        InstantiateEventListOption(TimeSystem.instance.listEvents);
    }
    private void ResetTextCustomEvent()
    {
        AcceptEventButton.gameObject.SetActive(true);
        DeclineEventButton.gameObject.SetActive(true);
        CloseEventButton.gameObject.SetActive(true);
        DetailsTextCustomEvent.text = " ";
        TitleTextCustomEvent.text = "Event";
    }
    void InitEventsButtons()
    {
        AcceptEventButton.onClick.AddListener(() => AcceptCustomEventButton());
        DeclineEventButton.onClick.AddListener(() => DeclineCustomEventButton());
        CloseEventButton.onClick.AddListener(() => CloseCustomEventButton());
    }
}
