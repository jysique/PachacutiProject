using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NumericButton : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    public void IncreaseNumberPopulation()
    {
        IncreaseNumber(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territory.Population);
    }

    public void IncreaseNumberGold()
    {
        IncreaseNumber(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territory.Gold);
    }
    public void DecreaseNumberPopulation()
    {
        DecreaseNumber(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territory.Population);
    }
    public void DecreaseNumberGold()
    {
        DecreaseNumber(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territory.Gold);
    }

    private void IncreaseNumber(int limit)
    {
        int temporal = int.Parse(inputField.text) + 1;
        if (temporal <= limit) inputField.text = temporal.ToString();
    }
    private void DecreaseNumber(int limit)
    {

        int temporal = int.Parse(inputField.text) - 1;
        if(temporal > 0) {
            inputField.text = temporal.ToString();
        }
        else {
            inputField.text = limit.ToString();
        }
    }
    
}
