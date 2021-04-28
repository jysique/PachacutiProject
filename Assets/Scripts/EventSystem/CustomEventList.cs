using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class CustomEventList
{
    public List<CustomEvent> CustomEvents = new List<CustomEvent>();

    public void AddCustomEvent(TimeSimulated _initTime,TimeSimulated _finalTime,int _days)
    {
        CustomEvent _customEvent = new CustomEvent();
        _customEvent.TerritoryEvent = TerritoryManager.instance.GetTerritoryRandom().territoryStats.territory;
        if (_customEvent.TerritoryEvent.MotivationPeople <= 20)
        {
            int random = Random.Range(0, 100);
            if (random >= 50)
            {
                Debug.LogError("Oh no cambio a rebelion");
                _customEvent.EventType = CustomEvent.EVENTTYPE.REBELION.ToString();
            }
        }
        _customEvent.GetCustomEvent(_initTime,_finalTime,_days);
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
