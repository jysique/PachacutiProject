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
    public List<CustomUnitCombat> UnitsEvents = new List<CustomUnitCombat>();
    public List<CustomBattle> BattleEvents = new List<CustomBattle>();
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

    public void AddUnitEvent(TimeSimulated _initTime, TerritoryHandler territory, UnitCombat unitCombat)
    {
        CustomUnitCombat _customEvent = new CustomUnitCombat(_initTime, territory, unitCombat);
        UnitsEvents.Add(_customEvent);
    }

    public void AddBattleEvent(string _battleType, Troop playerTroop, Troop enemyTroop, TerritoryHandler _playerTerritory, TerritoryHandler _enemyTerritory, bool isPlayerTerritory)
    {
        CustomBattle _customBattle;
        if (isPlayerTerritory)
        {
            _customBattle = new CustomBattle(_battleType, enemyTroop, playerTroop, _enemyTerritory,_playerTerritory);
        }
        else
        {
            _customBattle = new CustomBattle(_battleType, playerTroop,enemyTroop, _playerTerritory,_enemyTerritory);
            
        }
        
        BattleEvents.Add(_customBattle);
    }
    public void PrintList()
    {
        for (int i = 0; i < CustomEvents.Count; i++)
        {
            CustomEvents[i].PrintEvent(i);
        }
    }
    public void ResetAllBattleEvents()
    {
        BattleEvents.Clear();
    }

    public void RemoveEvent(CustomEvent @event)
    {
        CustomEvents.Remove(@event);
    }

    public void RemoveEvent(CustomBattle @event)
    {
        BattleEvents.Remove(@event);
    }
    public void RemoveEvent(CustomExpedition @event)
    {
        ExpedicionEvents.Remove(@event);
    }
    public void RemoveEvent(CustomBuilding @event)
    {
        BuildingsEvents.Remove(@event);
    }
    public void RemoveEvent(CustomUnitCombat @event)
    {
        UnitsEvents.Remove(@event);
    }
    public CustomEvent GetCustomEventByTerritory(Territory @territory, CustomEvent.EVENTTYPE  @type)
    {
        CustomEvent @event = null;
        for (int i = 0; i < CustomEvents.Count; i++)
        {
            if (CustomEvents[i].TerritoryEvent.Territory == territory && CustomEvents[i].EventType == @type)
            {
                Debug.Log("encontre");
                @event = CustomEvents[i];
            }
        }
        return @event;
    }

}
