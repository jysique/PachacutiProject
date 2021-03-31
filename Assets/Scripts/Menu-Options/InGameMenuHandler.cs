using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuHandler : MonoBehaviour
{
    public int warriorsNumber;
    public static InGameMenuHandler instance;
    [SerializeField] private GameObject warriorsPrefab;
    [SerializeField] private Territory selectedTerritory;
    [Header("Menu personaje")]
    [SerializeField] private Text governorName;
    [SerializeField] private Text governorAge;
    [SerializeField] private Text governorOrigin;
    [SerializeField] private Image governorPicture;
    [SerializeField] private Text governorDiplomacy;
    [SerializeField] private Text governorMilitancy;
    [SerializeField] private Text governorManagement;
    [SerializeField] private Text governorPrestige;
    [SerializeField] private Text governorPiety;



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


    [Header("Recursos")]
    [SerializeField] private Text goldGenerated;
    [SerializeField] private Text foodGenerated;
    [SerializeField] private Text susecionSizeTxt;
    [SerializeField] private Text scoreTxt;
    //perfil menu


    private void Awake()
    {
        instance = this;
        warriorsNumber = 0;
    }
   public void UpdateResourceTable()
    {
        goldGenerated.text = TerritoryManager.instance.GetGolds(Territory.TYPEPLAYER.PLAYER).ToString();
        foodGenerated.text = TerritoryManager.instance.GetFood(Territory.TYPEPLAYER.PLAYER).ToString();
        susecionSizeTxt.text = "0";
        scoreTxt.text = TerritoryManager.instance.CountTerrytorry(Territory.TYPEPLAYER.PLAYER).ToString();
    }
    public void UpdateMilitarMenu()
    {
        
        if (selectedTerritory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
        {
            menuBlock.SetActive(false);
            MilitarBoss boss = selectedTerritory.MilitarBoss;
            territoryName.GetComponent<Text>().text = selectedTerritory.name;
            militaryBossName.GetComponent<Text>().text = "Nombre: " + boss.CharacterName;
            militaryBossPicture.GetComponent<Image>().sprite = boss.Picture;
            militaryBossExperience.GetComponent<Text>().text = "Experiencia: " + boss.Experience;
            militaryBossEstrategy.GetComponent<Text>().text = "Estrategia: " + boss.Type;
            militaryBossMilitary.GetComponent<Text>().text = "Influencia: " + boss.Influence;
        }
        else
        {
            menuBlock.SetActive(true);
        }
    }
    public void UpdateProfileMenu()
    {
        Governor temp = CharacterManager.instance.Governor;
        governorName.text = "Nombre: " + temp.CharacterName;
        governorAge.text = "Edad: " + temp.Age.ToString();
        governorPicture.sprite = temp.Picture;
        governorOrigin.text = "Origen: " + temp.Origin;
        governorDiplomacy.text = "Diplomacia: " + temp.Diplomacy;
        governorMilitancy.text = "Militar: " + temp.Militancy;
        governorManagement.text = "Administracion: " +temp.Managment;
        governorPrestige.text = "Prestigio: " + temp.Prestige;
        governorPiety.text = "Piedad: " + temp.Piety;


    }
    public void UpdateMenu()
    {
        selectedTerritory = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territory;
        UpdateMilitarMenu();
        
    }
    void Update()
    {
        
        militarWarriorsCount.GetComponent<Text>().text = "Tropas: " + selectedTerritory.Population.ToString() + "/ 100 guerreros";
        UpdateResourceTable();
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
        if (TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territoryStats.population - warriorsNumber >= 0)
        {
            menuConfirm.SetActive(false);
            TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().ShowAdjacentTerritories();
        }
    }

    public void SendWarriors(TerritoryHandler selected, TerritoryHandler otherTerritory, int _warriorsNumber)
    {
        
        selected.territoryStats.population = selected.territoryStats.population - _warriorsNumber;
        warriorsPrefab.GetComponent<WarriorsMoving>().SetAttack(otherTerritory.gameObject, 1, _warriorsNumber, selected.territory.TypePlayer, selected.territory.MilitarBoss);
        
        Instantiate(warriorsPrefab, selected.transform.position , Quaternion.identity);
    }
    public void MoveWarriors(TerritoryHandler otherTerritory, int attackPower, Territory.TYPEPLAYER type)
    {
        
        if (otherTerritory.territory.TypePlayer == type)
        {
            otherTerritory.territoryStats.population = otherTerritory.territoryStats.population + attackPower;

        }
        else
        {
            int survivors = otherTerritory.territoryStats.population - attackPower;
            otherTerritory.territoryStats.population = otherTerritory.territoryStats.population - attackPower;
            if(survivors < 0)
            {
                otherTerritory.territory.TypePlayer = type;
                otherTerritory.territoryStats.population = otherTerritory.territoryStats.population * -1;
            }
        }

        
    }

    


}
