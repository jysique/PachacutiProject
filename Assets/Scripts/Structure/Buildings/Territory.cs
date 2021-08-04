using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [SerializeField] private Swordsman swordsmen= new Swordsman();
    [SerializeField] private Lancer lancers = new Lancer();
    [SerializeField] private Axeman axemen  = new Axeman();
    [SerializeField] private Scout scouts = new Scout();
    [SerializeField] private Archer archers = new Archer();

    [SerializeField] private Farm farmTerritory = new Farm();
    [SerializeField] private GoldMine goldMineTerritory = new GoldMine();
    [SerializeField] private Fortress fortressTerritory = new Fortress();

    [SerializeField] private Academy academyTerritory = new Academy();
    [SerializeField] private Barracks barracksTerritory = new Barracks();
    [SerializeField] private Castle castleTerritory = new Castle();
    [SerializeField] private Stable stableTerritory = new Stable();
    [SerializeField] private Archery archeryTerritory = new Archery();

    [SerializeField] private List<Terrain> terrainList = new List<Terrain>();
    private int costImproveSpeedPopulation = 10;
    private int costImproveLimitPopulation = 10;
    private int addCost = 5;
    private int perPeople = 2;
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
    public Swordsman Swordsmen
    {
        get { return swordsmen; }
        set { swordsmen = value; }
    }
    public Lancer Lancers
    {
        get { return lancers; }
        set { lancers = value; }
    }
    public Axeman Axemen
    {
        get { return axemen; }
        set { axemen = value; }
    }
    public Scout Scouts
    {
        get { return scouts; }
        set { scouts = value; }
    }
    public Archer Archers
    {
        get { return archers; }
        set { archers = value; }
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
    public List<Terrain> TerrainList
    {
        get { return terrainList; }
        set { terrainList = value; }
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
        get { return lancers.Quantity 
                + axemen.Quantity
                + swordsmen.Quantity 
                + scouts.Quantity
                + archers.Quantity; }
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
    public Building GetBuilding(UnitCombat unit)
    {
//        Debug.LogError(unit.GetType().ToString());
        if (unit is Swordsman)
        {
            return this.academyTerritory;
        }
        else if (unit is Lancer)
        {
            return this.barracksTerritory;
        }
        else if (unit is Axeman)
        {
            return this.castleTerritory;
        }
        else if (unit is Scout)
        {
            return this.stableTerritory;
        }
        else if (unit is Archer)
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
    public float GetSpeed(UnitCombat unitCombat)
    {
        switch (unitCombat.GetType().ToString())
        {
            case "Swordsman":
                return this.academyTerritory.SpeedSwordsmen;
            case "Lancer":
                return this.barracksTerritory.SpeedLancer;
            case "Axeman":
                return this.castleTerritory.SpeedAxemen;
            case "Scout":
                return this.stableTerritory.SpeedScouts;
            case "Archer":
                return this.archeryTerritory.SpeedArcher;
            default:
                Debug.LogError("no se encontro unidad de combate");
                break;
        }
        return 0;
    }
    public int GetLimit(UnitCombat unitCombat)
    {
        switch (unitCombat.GetType().ToString())
        {
            case "Swordsman":
                return this.academyTerritory.LimitSwordsmen;
            case "Lancer":
                return this.barracksTerritory.LimitLancer;
            case "Axeman":
                return this.castleTerritory.LimitAxemen;
            case "Scout":
                return this.stableTerritory.LimitScouts;
            case "Archer":
                return this.archeryTerritory.LimitArcher;
            default:
                Debug.LogError("no se encontro edificio");
                break;
        }
        return 0;
    }
    public UnitCombat GetUnit(string unit)
    {

        if (unit == "Swordsman")
        {
            return swordsmen;
        }
        else if (unit == "Lancer")
        {
            return lancers;
        }
        else if (unit == "Axeman")
        {
            return axemen;
        }
        else if(unit == "Scout")
        {
            return scouts;
        }
        else if (unit == "Archer")
        {
            return archers;
        }
        else
        {
            return null;
        }
    }
    public void ResetAllBuilds()
    {
        farmTerritory.ResetBuilding();
        goldMineTerritory.ResetBuilding();
        fortressTerritory.ResetBuilding();
        academyTerritory.ResetBuilding();
        barracksTerritory.ResetBuilding();
        castleTerritory.ResetBuilding();
        stableTerritory.ResetBuilding();
        archeryTerritory.ResetBuilding();
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
    [SerializeField] List<UnitCombat> troopDefend = new List<UnitCombat>();
    public List<UnitCombat> TroopDefending
    {
        get { return troopDefend; }
        set { troopDefend = value; }
    }
}