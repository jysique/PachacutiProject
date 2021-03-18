using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuHandler : MonoBehaviour
{
    public static InGameMenuHandler instance;
    [SerializeField] private GameObject territoryName;
    [SerializeField] private GameObject militarWarriorsCount;
    private void Awake()
    {
        instance = this;
    }
    public void UpdateMenu()
    {
        Territory selectedTerritory = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territory;
        territoryName.GetComponent<Text>().text = selectedTerritory.name;
        
    }
    void Update()
    {
        Territory selectedTerritory = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territory;
        militarWarriorsCount.GetComponent<Text>().text = "Tropas: " + selectedTerritory.GetPopulation().ToString() + "/ 100 guerreros";
    }
}
