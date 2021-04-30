using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Territory
{
   
    public string name;
    public enum TRIBES
    {
        TRIBE_A,
        TRIBE_B,
        TRIBE_C,
        TRIBE_D,
    }
    public enum TYPEPLAYER
    {
        PLAYER,
        BOT,
        NONE
    }
    [SerializeField] private TRIBES tribe;
    [SerializeField] private TYPEPLAYER typePlayer;
    [SerializeField] private int population;
    [SerializeField] private int gold;
    [SerializeField] private int food;
    [SerializeField] private int limitPopulation;
    [SerializeField] private int motivation;

    [SerializeField] private float velocityPopulation;
    [SerializeField] private float velocityFood;

    [SerializeField] private bool selected;

    [SerializeField] private MilitarBoss militarBossTerritory = null;
    [SerializeField] private GoldMine goldMineTerritory = new GoldMine();
    [SerializeField] private SacredPlace sacredPlaceTerritory = new SacredPlace();
    [SerializeField] private Fortress fortressTerritory = new Fortress();
    [SerializeField] private Barracks barracksTerritory = new Barracks();

    /// <summary>
    /// variables for countDown timers in TerritoryMenu
    /// </summary>
    private float[] totalTime = new float[4] { 0, 0, 0, 0 };
    private bool[] canUpgrade = new bool[4] { true, true, true, true };

    public float[] TotalTime
    {
        get { return totalTime; }
        set { totalTime = value; }
    }
    public bool[] CanUpgrade
    {
        get { return canUpgrade; }
        set { canUpgrade = value; }
    }
    public MilitarBoss MilitarBossTerritory
    {
        get { return militarBossTerritory; }
        set { militarBossTerritory = value; }
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
    public Barracks BarracksTerritory
    {
        get { return barracksTerritory; }
        set { barracksTerritory = value; }
    }
    public TYPEPLAYER TypePlayer
    {
        get { return typePlayer; }
        set { typePlayer = value; }
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
    public float VelocityFood
    {
        get { return velocityFood; }
        set { velocityFood = value; }
    }
    public void SetSelected(bool _selected) { selected = _selected; }
    public bool GetSelected() { return selected; }


}

