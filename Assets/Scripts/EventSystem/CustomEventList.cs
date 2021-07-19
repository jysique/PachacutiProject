using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class CustomEventList
{
    public List<CustomEvent> CustomEvents = new List<CustomEvent>();
    public List<CustomExpedition> ExpedicionEvents = new List<CustomExpedition>();
    public List<CustomBuilding> BuildingsEvents = new List<CustomBuilding>();
    /// <summary>
    /// Add a event to list with a time simulated init and days to finish event
    /// </summary>
    /// <param name="_initTime"></param>
    /// <param name="_days"></param>
    public void AddCustomEvent(TimeSimulated _initTime,TerritoryHandler territory = null)
    {
        CustomEvent _customEvent = new CustomEvent(_initTime, territory);
        CustomEvents.Add(_customEvent);
    }

    public void AddExpedicionEvent(TimeSimulated _initTime,Troop troopToWaste, TerritoryHandler attackerTerritory = null, TerritoryHandler wasteTerritory = null)
    {
        CustomExpedition _customEvent = new CustomExpedition(_initTime,troopToWaste,attackerTerritory, wasteTerritory);
        ExpedicionEvents.Add(_customEvent);
    }

    public void AddBuildingEvent(TimeSimulated _initTime,TerritoryHandler territory, Building building)
    {
        CustomBuilding _customEvent = new CustomBuilding(_initTime, territory, building);
        BuildingsEvents.Add(_customEvent);
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
    public void RemoveEvent(CustomExpedition @event)
    {
        ExpedicionEvents.Remove(@event);
    }
    public void RemoveEvent(CustomBuilding @event)
    {
        BuildingsEvents.Remove(@event);
    }
    public CustomEvent GetCustomEventByTerritory(Territory @territory)
    {
        CustomEvent @event = new CustomEvent();
        for (int i = 0; i < CustomEvents.Count; i++)
        {
            if (CustomEvents[i].TerritoryEvent.TerritoryStats.Territory == territory && CustomEvents[i].EventType == CustomEvent.EVENTTYPE.CONQUIST)
            {
                @event = CustomEvents[i];
            }
        }
        return @event;
    }

}
