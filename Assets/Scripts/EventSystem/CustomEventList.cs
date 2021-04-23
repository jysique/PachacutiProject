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
            Debug.Log(i + " eventType - " + CustomEvents[i].EventType.ToString());
          //  Debug.Log(i + " territorio- " + CustomEvents[i].TerritoryEvent);
            Debug.Log(i + " init time- " + CustomEvents[i].TimeInitEvent.PrintTimeSimulated());
            Debug.Log(i + " final time- " + CustomEvents[i].TimeFinalEvent.PrintTimeSimulated());
           // Debug.Log("================");
        }
    }
}
