using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class CustomEventList
{
    public List<CustomEvent> CustomEvents = new List<CustomEvent>();
    /// <summary>
    /// Add a event to list with a time simulated init and days to finish event
    /// </summary>
    /// <param name="_initTime"></param>
    /// <param name="_days"></param>
    public void AddCustomEvent(TimeSimulated _initTime)
    {
        CustomEvent _customEvent = new CustomEvent();
        _customEvent.TerritoryEvent = TerritoryManager.instance.GetTerritoryRandom(Territory.TYPEPLAYER.PLAYER);
        _customEvent.GetCustomEvent(_initTime);
        CustomEvents.Add(_customEvent);
    }
    public void AddCustomEvent2(TimeSimulated _initTime,TerritoryHandler territory, Building building)
    {
        CustomEvent _customEvent = new CustomEvent();
        _customEvent.GetCustomEvent(_initTime,territory, building);
        CustomEvents.Add(_customEvent);
    }
    public void PrintList()
    {
        for (int i = 0; i < CustomEvents.Count; i++)
        {
            CustomEvents[i].PrintEvent(i);
        }
    }
    public void RemoveEvent(CustomEvent @event)
    {
        CustomEvents.Remove(@event);
    }
}
