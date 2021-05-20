using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CustomEventOption : MonoBehaviour
{
    private CustomEvent custom;
    [SerializeField] Image Icon;
    [SerializeField] Text territoryNameTxt;
    [SerializeField] Text typeEventTxt;
    [SerializeField] Text initTxt;
    [SerializeField] Text finishTxt;
    [SerializeField] Image estado;
    private Button btn;
    int diferenceDays;
    public CustomEvent Custom
    {
        get { return custom; }
        set { custom = value; }
    }
    private void Start()
    {
        btn = transform.GetComponent<Button>();
        btn.onClick.AddListener(() => CustomEventApear());
        territoryNameTxt.text = custom.TerritoryEvent.name;
        typeEventTxt.text = custom.EventType;
        initTxt.text = custom.TimeInitEvent.PrintTimeSimulated();
        finishTxt.text = custom.TimeFinalEvent.PrintTimeSimulated();

    }
    private void Update()
    {
        diferenceDays = custom.TimeFinalEvent.DiferenceDays(TimeSystem.instance.TimeGame);
        if (custom.EventStatus == CustomEvent.STATUS.FINISH)
        {
            estado.color = new Color32(193, 39, 4,255);
            btn.interactable = false;
            TimeSystem.instance.listEvents.RemoveEvent(custom);
        }
    }
    void CustomEventApear()
    {
        InGameMenuHandler.instance.WarningEventAppearance(custom, diferenceDays);
    }
}
