using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Territory
{
   
    public string name;
    
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
    private bool isClaim = false;

    // [SerializeField] private REGION region;
    [SerializeField] private string region;
    [SerializeField] private TYPEPLAYER typePlayer;
    [SerializeField] private int population;
    [SerializeField] private int gold;
    [SerializeField] private int food;
    [SerializeField] private int limitPopulation;
    [SerializeField] private int motivation;
    [SerializeField] private float velocityPopulation;

    private int costImproveSpeedPopulation = 10;
    private int addCost = 5;
    private int costImproveLimitPopulation = 10;
    private int perPeople = 2;
    private float improveSpeed = 0.3f;
    private int improveLimit = 10;
    [SerializeField] private bool selected;

    [SerializeField] private MilitarChief militarChiefTerritory = null;
    [SerializeField] private GoldMine goldMineTerritory = new GoldMine();
    [SerializeField] private SacredPlace sacredPlaceTerritory = new SacredPlace();
    [SerializeField] private Fortress fortressTerritory = new Fortress();
    [SerializeField] private Armory armoryTerritory = new Armory();
    [SerializeField] private IrrigationChannel irrigationChannelTerritory = new IrrigationChannel();

    public bool IsClaimed
    {
        get { return isClaim; }
        set { isClaim = value; }
    }
    public float ImproveSpeed
    {
        get { return improveSpeed; }
    }
    public int ImproveLimit
    {
        get { return improveLimit; }
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
    public GoldMine GoldMineTerritory
    {
        get { return goldMineTerritory; }
        set { goldMineTerritory = value; }
    }
    public SacredPlace SacredPlaceTerritory
    {
        get { return sacredPlaceTerritory; }
        set { sacredPlaceTerritory = value; }
    }
    public Fortress FortressTerritory
    {
        get { return fortressTerritory; }
        set { fortressTerritory = value; }
    }
    public Armory ArmoryTerritory
    {
        get { return armoryTerritory; }
        set { armoryTerritory = value; }
    }
    public IrrigationChannel IrrigationChannelTerritory
    {
        get { return irrigationChannelTerritory; }
        set { irrigationChannelTerritory = value; }
    }
    public TYPEPLAYER TypePlayer
    {
        get { return typePlayer; }
        set { typePlayer = value; }
    }
    public bool AllBuilds()
    {
        return goldMineTerritory.Level > 0 
            && sacredPlaceTerritory.Level > 0 
            && fortressTerritory.Level > 0 
            && armoryTerritory.Level >0 
            && irrigationChannelTerritory.Level > 0;
    }
    public string RegionTerritory
    {
        get { return region; }
        set { region = value; }
    }
    public int Population
    {
        get { return population; }
        set
        {
            population = value;
        }
    }
    public int MotivationPeople
    {
        get { return motivation; }
        set { motivation = value; }
    }
    public int LimitPopulation
    {
        get { return limitPopulation; }
        set { limitPopulation = value; }
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
    public float VelocityPopulation
    {
        get { return velocityPopulation; }
        set { velocityPopulation = value; }
    }
    public void SetSelected(bool _selected) { selected = _selected; }
    public bool GetSelected() { return selected; }
    public void ResetAllBuilds()
    {
        irrigationChannelTerritory.ResetBuilding();
        goldMineTerritory.ResetBuilding();
        sacredPlaceTerritory.ResetBuilding();
        FortressTerritory.ResetBuilding();
        ArmoryTerritory.ResetBuilding();
    }

}

