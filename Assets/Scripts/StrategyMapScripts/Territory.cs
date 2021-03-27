﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private float velocityPopulation;
    [SerializeField] private float velocityGold;
    [SerializeField] private float velocityFood;

    [SerializeField] private bool selected;
    [SerializeField] private MilitarBoss militarBoss;

    public MilitarBoss MilitarBoss
    {
        get { return militarBoss; }
        set { militarBoss = value; }
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
            Debug.Log("change population");
            population = value;
        }
    }
    public int GoldReward
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
    }
    public float VelocityGold
    {
        get { return velocityGold; }
    }
    public float VelocityFood
    {
        get { return velocityFood; }
    }

    public void SetStats(TerritoryStats territoryStats)
    {
        gold = territoryStats.gold;
        population = territoryStats.population;
        food = territoryStats.food;

        velocityGold = territoryStats.velocityGold;
        velocityPopulation = territoryStats.velocityPopulation;
        velocityFood = territoryStats.velocityFood;
    }

    public void SetSelected(bool _selected) { selected = _selected; }
    public bool GetSelected() { return selected; }


}
