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
    public CustomEvent Custom
    {
        get { return custom; }
        set { custom = value; }
    }
    private void Start()
    {
        territoryNameTxt.text = custom.TerritoryEvent.name;
        typeEventTxt.text = custom.EventType;
        initTxt.text = custom.TimeInitEvent.PrintTimeSimulated();
        finishTxt.text = custom.TimeFinalEvent.PrintTimeSimulated();

    }
}
