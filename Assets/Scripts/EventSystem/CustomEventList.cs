using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class CustomEventList
{
    public List<CustomEvent> CustomEvents = new List<CustomEvent>();

    public void AddCustomEvent(TimeSimulated _initTime,int _days)
    {
        CustomEvent _customEvent = new CustomEvent();
        _customEvent.TerritoryEvent = TerritoryManager.instance.GetTerritoryRandom(Territory.TYPEPLAYER.PLAYER).territoryStats.territory;
        _customEvent.GetCustomEvent(_initTime, _days);
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
    public void RemoveEventId(int id)
    {
        CustomEvents.Remove(CustomEvents[id]);
    }
    public void RemoveEvent(CustomEvent @event)
    {
        CustomEvents.Remove(@event);
    }
}
