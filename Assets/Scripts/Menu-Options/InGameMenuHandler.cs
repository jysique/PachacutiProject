using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuHandler : MonoBehaviour
{
    public static InGameMenuHandler instance;
    //military menu
    [SerializeField] private GameObject menuBlock;
    [SerializeField] private GameObject territoryName;
    [SerializeField] private GameObject militarWarriorsCount;
    [SerializeField] private GameObject militaryBossName;
    [SerializeField] private GameObject militaryBossPicture;
    [SerializeField] private GameObject militaryBossExperience;
    [SerializeField] private GameObject militaryBossEstrategy;
    [SerializeField] private GameObject militaryBossMilitary;


    private void Awake()
    {
        instance = this;
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
}
