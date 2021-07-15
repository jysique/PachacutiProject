using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Territory
{
   
    public string name;
    private bool isClaim = false;

    [SerializeField] private string region;
    [SerializeField] private TYPEPLAYER typePlayer;

    [SerializeField] private int gold;
    [SerializeField] private int food;
    [SerializeField] private int motivation;
    [SerializeField] private int opinion;
    [SerializeField] private bool selected;

    [SerializeField] private MilitarChief militarChiefTerritory = null;
    [SerializeField] private Swordsman swordsmen= new Swordsman();
    [SerializeField] private Lancer lancers = new Lancer();
    [SerializeField] private Archer archers  = new Archer();

    [SerializeField] private IrrigationChannel irrigationChannelTerritory = new IrrigationChannel();
    [SerializeField] private GoldMine goldMineTerritory = new GoldMine();
    [SerializeField] private Fortress fortressTerritory = new Fortress();
    [SerializeField] private Academy academyTerritory = new Academy();
    [SerializeField] private Barracks barracksTerritory = new Barracks();
    [SerializeField] private Archery archeryTerritory = new Archery();

    [SerializeField] private List<SquareType.TYPESQUARE> tiles = new List<SquareType.TYPESQUARE>();

    private int costImproveSpeedPopulation = 10;
    private int costImproveLimitPopulation = 10;
    private int addCost = 5;
    private int perPeople = 2;
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
    public Archer Archer
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
    public IrrigationChannel IrrigationChannelTerritory
    {
        get { return irrigationChannelTerritory; }
        set { irrigationChannelTerritory = value; }
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
    public List<SquareType.TYPESQUARE> Tiles
    {
        get { return tiles; }
    }
    public bool AllBuilds()
    {
        return goldMineTerritory.Level > 0
            && irrigationChannelTerritory.Level > 0
            && fortressTerritory.Level > 0
            && academyTerritory.Level > 0
            && barracksTerritory.Level > 0
            && archeryTerritory.Level >0;
    }
    public string RegionTerritory
    {
        get { return region; }
        set { region = value; }
    }
    
    public int Population
    {
        get { return lancers.NumbersUnit + archers.NumbersUnit+ swordsmen.NumbersUnit; }
        /*
        set
        {
            population = value;
        }
        */
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
    public void SetSelected(bool _selected) { selected = _selected; }
    public bool GetSelected() { return selected; }
    
    public float GetSpeed(UnitCombat unitCombat)
    {
        switch (unitCombat.GetType().ToString())
        {
            case "Swordsman":
                return this.academyTerritory.SpeedSwordsmen;
            case "Lancer":
                return this.barracksTerritory.SpeedLancer;
            case "Archer":
                return this.archeryTerritory.SpeedArchers;
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
            case "Archer":
                return this.archeryTerritory.LimitArchers;
            default:
                Debug.LogError("no se encontro edificio");
                break;
        }
        return 0;
    }

    /*
    public UnitCombat GetUnit(UnitCombat subordinate)
    {
        if (subordinate is Swordsman)
        {
            return swordsmen;
        }
        else if (subordinate is Lancer)
        {
            return lancers;
        }
        return archers;
    }
    */
    public void ResetAllBuilds()
    {
        irrigationChannelTerritory.ResetBuilding();
        goldMineTerritory.ResetBuilding();
        fortressTerritory.ResetBuilding();
        academyTerritory.ResetBuilding();
        barracksTerritory.ResetBuilding();
        archeryTerritory.ResetBuilding();
    }
    public enum REGION
    {
        NORTH_REGION,
        EAST_REGION,
        SOUTHERN_REGION,
        CENTRE_REGION,
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
        //        CLAIM
    }
}


