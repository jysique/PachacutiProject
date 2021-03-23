using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumericButton : MonoBehaviour
{
    [SerializeField] private InputField inputField;

    public void IncreaseNumber()
    {
        int temporal = int.Parse(inputField.text) + 1;
        int limit = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territory.GetPopulation();
        if (temporal <= limit) inputField.text = temporal.ToString();
    }
    public void DecreaseNumber()
    {

        int temporal = int.Parse(inputField.text) - 1;
        if(temporal > 0) {
            inputField.text = temporal.ToString();
        }
        else {
            inputField.text = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territory.GetPopulation().ToString();
        }
    }
    
}
