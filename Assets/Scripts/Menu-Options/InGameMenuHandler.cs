using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuHandler : MonoBehaviour
{
    private int warriorsNumber;
    public static InGameMenuHandler instance;
    [Header("Menu militar")]
    [SerializeField] private GameObject menuConfirm;
    [SerializeField] private GameObject menuBlock;
    [SerializeField] private GameObject territoryName;
    [SerializeField] private GameObject militarWarriorsCount;
    [SerializeField] private GameObject militaryBossName;
    [SerializeField] private GameObject militaryBossPicture;
    [SerializeField] private GameObject militaryBossExperience;
    [SerializeField] private GameObject militaryBossEstrategy;
    [SerializeField] private GameObject militaryBossMilitary;
    [Header("Menu de movilizacion de tropas")]
    [SerializeField] private InputField warriorsCount;


    //perfil menu


    private void Awake()
    {
        instance = this;
        warriorsNumber = 0;
    }
   
    public void UpdateMenu()
    {

        Territory selectedTerritory = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territory;
        if (selectedTerritory.GetTypePlayer() == Territory.TypePlayer.PLAYER)
        {
            menuBlock.SetActive(false);
            MilitarBoss boss = selectedTerritory.GetMilitarBoss();
            territoryName.GetComponent<Text>().text = selectedTerritory.name;
            militaryBossName.GetComponent<Text>().text = "Nombre: " + boss.Name;
            militaryBossPicture.GetComponent<Image>().sprite = boss.Picture;
            militaryBossExperience.GetComponent<Text>().text = "Experiencia: " + boss.Experience;
            militaryBossEstrategy.GetComponent<Text>().text = "Estrategia: " + boss.Strategy;
            militaryBossMilitary.GetComponent<Text>().text = "Militar: " + boss.Military;
        }
        else
        {
            menuBlock.SetActive(true);
        }


        
        
        
    }
    void Update()
    {
        Territory selectedTerritory = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territory;
        militarWarriorsCount.GetComponent<Text>().text = "Tropas: " + selectedTerritory.GetPopulation().ToString() + "/ 100 guerreros";
    }


    //move warriors
    public void MoveWarriorsButton()
    {
        menuConfirm.SetActive(true);
        foreach (GameObject t in TerritoryManager.instance.territoryList)
        {
            t.GetComponent<TerritoryHandler>().state = 2;
        }
            
    }

    public void CloseWarriorsButton()
    {
        menuConfirm.SetActive(false);
        foreach (GameObject t in TerritoryManager.instance.territoryList)
        {
            t.GetComponent<TerritoryHandler>().state = 0;
        }
    }
    public void SelectTerritory()
    {
        warriorsNumber = int.Parse(warriorsCount.text);
        if (TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territoryStats.population - warriorsNumber > -1)
        {
            menuConfirm.SetActive(false);
            TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().ShowAdjacentTerritories();
        }
        
        
    }

    public void MoveWarriors(TerritoryHandler otherTerritory)
    {
        TerritoryHandler selected = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
        selected.territoryStats.population = selected.territoryStats.population - warriorsNumber;
        if(otherTerritory.territory.GetTypePlayer() == Territory.TypePlayer.PLAYER)
        {
            otherTerritory.territoryStats.population = otherTerritory.territoryStats.population + warriorsNumber;
        }
        else
        {
            otherTerritory.territoryStats.population = otherTerritory.territoryStats.population - warriorsNumber;
        }
        
    }

    


}
