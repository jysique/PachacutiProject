using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerritoryStats : MonoBehaviour
{
    [SerializeField] public GameObject container;

   [SerializeField] private Text populationTxt;
    [SerializeField] private Text nameTerritoryTxt;
    [SerializeField] private Image imageTerritory;
    [SerializeField] private GameObject iconsContainer;

    [Header("New system")]

    private Territory territory;

    public Territory Territory
    {
        get { return territory; }
        set { territory = value; }
    }
    private void Start()
    {
       
        nameTerritoryTxt.text = territory.name;
        if (territory.TypePlayer == Territory.TYPEPLAYER.WASTE)
        {
            populationTxt.enabled = false;
        }

    }
    public void CanPopulate()
    {
        // populationTxt.text = territory.Population + "/" + (territory.AcademyTerritory.LimitSwordsmen + territory.BarracksTerritory.LimitLancer + territory.CastleTerritory.LimitAxemen);
        populationTxt.text = territory.Population + "/" + 
            (territory.AcademyTerritory.LimitUnits + 
            territory.BarracksTerritory.LimitUnits + 
            territory.CastleTerritory.LimitUnits +
            territory.ArcheryTerritory.LimitUnits +
            territory.StableTerritory.LimitUnits
            );

    }
    private void FixedUpdate()
    {
        CanPopulate();
    }
 
    public void IncresementGold()
    {
        // territory.Gold+= territory.GoldMineTerritory.WorkersMine / territory.PerPeople;
        territory.Gold += territory.GoldMineTerritory.LimitUnits / territory.PerPeople;
    }
    public void IncresementFood()
    {
        territory.FoodReward+= territory.FarmTerritory.LimitUnits / territory.PerPeople;
    }
   
}

