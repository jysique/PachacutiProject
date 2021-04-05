using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        IncreaseNumber(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territory.GoldReward);
    }
    public void DecreaseNumberPopulation()
    {
        DecreaseNumber(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territory.Population);
    }
    public void DecreaseNumberGold()
    {
        DecreaseNumber(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territory.GoldReward);
    }

    private void IncreaseNumber(int limit)
    {
        int temporal = int.Parse(inputField.text) + 1;
        //int limit = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territory.Population;
        if (temporal <= limit) inputField.text = temporal.ToString();
    }
    private void DecreaseNumber(int limit)
    {

        int temporal = int.Parse(inputField.text) - 1;
        if(temporal > 0) {
            inputField.text = temporal.ToString();
        }
        else {
            //inputField.text = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territory.Population.ToString();
            inputField.text = limit.ToString();
        }
    }
    
}
