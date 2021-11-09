﻿using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Territory
{
    private float width;
    private float height;
    public int numberOfBuildings = 0;
    public string name;
    [SerializeField] private bool isClaim = false;

    [SerializeField] private REGION region;
    [SerializeField] private TYPEPLAYER typePlayer;

    [SerializeField] private int gold;
    [SerializeField] private int food;
    [SerializeField] private int motivation;
    [SerializeField] private int opinion;
    [SerializeField] private bool selected;

    [SerializeField] private MilitarChief militarChiefTerritory = null;


    [SerializeField] private Troop listUnitCombat = new Troop();
    [SerializeField] private Farm farmTerritory = new Farm();
    [SerializeField] private GoldMine goldMineTerritory = new GoldMine();
    [SerializeField] private Fortress fortressTerritory = new Fortress();

    [SerializeField] private Academy academyTerritory = new Academy();
    [SerializeField] private Barracks barracksTerritory = new Barracks();
    [SerializeField] private Castle castleTerritory = new Castle();
    [SerializeField] private Stable stableTerritory = new Stable();
    [SerializeField] private Archery archeryTerritory = new Archery();

    // 0,1,2,4   8,9,10,11  16,17,18,19
    [SerializeField] private List<Terrain> terrainDefendList = new List<Terrain>();
    // 4,5,6,7  12,13,14,15     20,21,22,23
    [SerializeField] private List<Terrain> terrainAttackList = new List<Terrain>();
    [SerializeField] private Civilization civilization = new Civilization();
    private int costImproveSpeedPopulation = 10;
    private int costImproveLimitPopulation = 10;
    private int addCost = 5;
    private int perPeople = 2;


    public Civilization Civilization
    {
        get { return civilization; }
        set { civilization = value; }
    }
    public float Width
    {
        get { return width; }
        set { width = value; }
    }
    public float Height
    {
        get { return height; }
        set { height = value; }
    }
    public Troop ListUnitCombat
    {
        get { return listUnitCombat; }
        set { listUnitCombat = value; }
    }

    public bool IsClaimed
    {
        get { return isClaim; }
        set { isClaim = value; }
    }
    public int PerPeople
    {
        get { return perPeople; }
    }
    public int AddCost
    {
        get { return addCost; }
        set { addCost = value; }
    }
    public int CostSpeedPopulation
    {
        get { return costImproveSpeedPopulation; }
        set { costImproveSpeedPopulation = value; }
    }
    public int CostLimitPopulation
    {
        get { return costImproveLimitPopulation; }
        set { costImproveLimitPopulation = value; }
    }
    public MilitarChief MilitarChiefTerritory
    {
        get { return militarChiefTerritory; }
        set { militarChiefTerritory = value; }
    }
    public Farm FarmTerritory
    {
        get { return farmTerritory; }
        set { farmTerritory = value; }
    }
    public GoldMine GoldMineTerritory
    {
        get { return goldMineTerritory; }
        set { goldMineTerritory = value; }
    }

    public Fortress FortressTerritory
    {
        get { return fortressTerritory; }
        set { fortressTerritory = value; }
    }
    public Academy AcademyTerritory
    {
        get { return academyTerritory; }
        set { academyTerritory = value; }
    }
    public Barracks BarracksTerritory
    {
        get { return barracksTerritory; }
        set { barracksTerritory = value; }
    }
    public Castle CastleTerritory
    {
        get { return castleTerritory; }
        set { castleTerritory = value; }
    }
    public Stable StableTerritory
    {
        get { return stableTerritory; }
        set { stableTerritory = value; }
    }
    public Archery ArcheryTerritory
    {
        get { return archeryTerritory; }
        set { archeryTerritory = value; }
    }
    public TYPEPLAYER TypePlayer
    {
        get { return typePlayer; }
        set { typePlayer = value; }
    }
    
    public List<Terrain> TerrainAttackList
    {
        get { return terrainAttackList; }
        set { terrainAttackList = value; }
    }
    public List<Terrain> TerrainDeffendList
    {
        get { return terrainDefendList; }
        set { terrainDefendList = value; }
    }

    public bool AllBuildsLevels()
    {
        return goldMineTerritory.Level > 0
            && FarmTerritory.Level > 0
            && fortressTerritory.Level > 0
            && academyTerritory.Level > 0
            && barracksTerritory.Level > 0
            && castleTerritory.Level > 0
            && stableTerritory.Level > 0
            && archeryTerritory.Level >0;
    }

    public REGION RegionTerritory
    {
        get { return region; }
        set { region = value; }
    }
    
    public int Population
    {
        get { return listUnitCombat.GetPopulation(); }
    }
    public int LimitPopulation
    {
        get {
            return academyTerritory.LimitUnits +
          barracksTerritory.LimitUnits +
          castleTerritory.LimitUnits +
          archeryTerritory.LimitUnits +
          stableTerritory.LimitUnits;
        } 
    }

    public int MotivationTerritory
    {
        get { return motivation; }
        set { motivation = value; }
    }

    public int OpinionTerritory
    {
        get { return opinion; }
        set { opinion = value; }
    }
    public int Gold
    {
        get { return gold; }
        set { gold = value; }
    }
    public int FoodReward
    {
        get { return food; }
        set { food = value; }
    }
    public bool Selected
    {
        get { return selected; }
        set { selected = value; }
    }
    public Building GetBuildingByUnit(UnitCombat unit)
    {
        switch (unit.UnitType)
        {
            case "Sword":
                return this.academyTerritory;
            case "Lancer":
                return this.barracksTerritory;
            case "Axe":
                return this.castleTerritory;
            case "Scout":
                return this.stableTerritory;
            case "Archer":
                return this.archeryTerritory;
            default:
                Debug.LogError("no se encontro unidad de combate");
                return null;
        }
    }
    public Building GetBuilding(Building building)
    {
        if (building is Farm)
        {
            return this.farmTerritory;
        }
        else if (building is GoldMine)
        {
            return this.goldMineTerritory;
        }
        else if (building is Fortress)
        {
            return this.fortressTerritory;
        }
        else if (building is Academy)
        {
            return this.academyTerritory;
        }
        else if (building is Barracks)
        {
            return this.barracksTerritory;
        }
        else if(building is Castle)
        {
            return this.castleTerritory;
        }
        else if (building is Stable)
        {
            return this.stableTerritory;
        }
        else if (building is Archery)
        {
            return this.archeryTerritory;
        }
        else
        {
            return null;
        }
    }
    public Building GetBuilding(string building)
    {
        if (building == "Farm")
        {
            return farmTerritory;
        }
        else if (building == "GoldMine")
        {
            return goldMineTerritory;
        }
        else if (building == "Fortress")
        {
            return fortressTerritory;
        }
        else if (building == "Academy")
        {
            return academyTerritory;
        }
        else if (building == "Barracks")
        {
            return barracksTerritory;
        }
        else if (building == "Castle")
        {
            return castleTerritory;
        }
        else if (building == "Stable")
        {
            return stableTerritory;
        }
        else if (building == "Archery")
        {
            return archeryTerritory;
        }
        else
        {
            return null;
        }
    }
    public void MoveUnits(Troop troop)
    {
        for (int i = 0; i < troop.UnitCombats.Count; i++)
        {
            listUnitCombat.DeleteUnitCombat(troop.UnitCombats[i]);
        }
    }
    public Building GetBuildingRandom()
    {
        int random = Random.Range(0, Utils.instance.Buildings_string.Count);
        return GetBuilding(Utils.instance.Buildings_string[random]);
    }
    public void IncresementGold()
    {
        gold += goldMineTerritory.LimitUnits / perPeople;
    }
    public void IncresementFood()
    {
        food += farmTerritory.LimitUnits / perPeople;
    }
    public void GatherTerritoryGold()
    {
        int gather = gold;
        gold -= gather;
    }
    public void GatherTerritoryFood()
    {
        int gather = food;
        food -= gather;
    }
    public void ResetAllBuilds()
    {
        farmTerritory.ResetBuilding(this);
        goldMineTerritory.ResetBuilding(this);
        fortressTerritory.ResetBuilding(this);
        academyTerritory.ResetBuilding(this);
        barracksTerritory.ResetBuilding(this);
        castleTerritory.ResetBuilding(this);
        stableTerritory.ResetBuilding(this);
        archeryTerritory.ResetBuilding(this);
    }
    public enum REGION
    {
        NORTH,
        EAST,
        SOUTHERN,
        CENTRE,
        NONE
    }
    public enum TYPEPLAYER
    {
        PLAYER,
        BOT,
        BOT2,
        BOT3,
        BOT4,
        NONE,
        WASTE
    }
}