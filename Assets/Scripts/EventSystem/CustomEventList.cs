using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CustomEventList
{
    public List<CustomEvent> CustomEvents = new List<CustomEvent>();

    public void AddCustomEvent(TimeSimulated _initTime,TimeSimulated _finalTime)
    {
        CustomEvent _customEvent = new CustomEvent();
        _customEvent.TerritoryEvent = TerritoryManager.instance.GetTerritoryRandom().territoryStats.territory;
        //_customEvent.GetCustomEvent(_initTime,_finalTime,_days);
        _customEvent.GetCustomEvent(_initTime, _finalTime);
        CustomEvents.Add(_customEvent);
    }
    public void AddCustomEvent(CustomEvent _customEvent)
    {
        CustomEvents.Add(_customEvent);
    }
    public void PrintList()
    {
        for (int i = 0; i < CustomEvents.Count; i++)
        {
            CustomEvents[i].PrintEvent(i);
        }
    }
}
