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
    [SerializeField] Image Estado;
    private Button btn;
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
    void CustomEventApear()
    {
        if (!custom.TimeFinalEvent.EqualsDate(custom.TimeInitEvent))
        {
            InGameMenuHandler.instance.WarningEventAppearance(custom, TimeSystem.instance.DiferenceDays);
        }
    }
}
