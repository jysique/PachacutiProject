using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuHandler : MonoBehaviour
{
    private int warriorsNumber;
    public static InGameMenuHandler instance;
    [SerializeField] private GameObject warriorsPrefab;
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
            MilitarBoss boss = selectedTerritory.MilitarBoss;
            territoryName.GetComponent<Text>().text = selectedTerritory.name;
            militaryBossName.GetComponent<Text>().text = "Nombre: " + boss.CharacterName;
            //militaryBossPicture.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/TemporalAssets/" + boss.CharacIcon + "/" + boss.);
            //militaryBossPicture.GetComponent<Image>().sprite = Resources.Load<Sprite>("Textures/TemporalAssets/Military/02");
            militaryBossPicture.GetComponent<Image>().sprite = boss.Picture;
            militaryBossExperience.GetComponent<Text>().text = "Experiencia: " + boss.Experience;
            militaryBossEstrategy.GetComponent<Text>().text = "Estrategia: " + boss.StrategyLevel;
            militaryBossMilitary.GetComponent<Text>().text = "Influencia: " + boss.Influence;
           

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
        warriorsCount.text = "1";
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
        if (TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territoryStats.population - warriorsNumber > 0)
        {
            menuConfirm.SetActive(false);
            TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().ShowAdjacentTerritories();
        }
        
        
    }

    public void SendWarriors(TerritoryHandler otherTerritory)
    {
        TerritoryHandler selected = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
        selected.territoryStats.population = selected.territoryStats.population - warriorsNumber;
        warriorsPrefab.GetComponent<WarriorsMoving>().target = otherTerritory.gameObject;
        warriorsPrefab.transform.GetChild(0).GetComponent<TextMeshPro>().text = warriorsNumber.ToString();
        Instantiate(warriorsPrefab, TerritoryManager.instance.territorySelected.transform.position , Quaternion.identity);
    }
    public void MoveWarriors(TerritoryHandler otherTerritory)
    {
        
        if(otherTerritory.territory.GetTypePlayer() == Territory.TypePlayer.PLAYER)
        {
            otherTerritory.territoryStats.population = otherTerritory.territoryStats.population + warriorsNumber;

        }
        else
        {
            int survivors = otherTerritory.territoryStats.population - warriorsNumber;
            otherTerritory.territoryStats.population = otherTerritory.territoryStats.population - warriorsNumber;
            if(survivors < 0)
            {
                otherTerritory.territory.SetTypePlayer(Territory.TypePlayer.PLAYER);
                otherTerritory.territoryStats.population = otherTerritory.territoryStats.population * -1;
            }
        }
        
    }

    


}
