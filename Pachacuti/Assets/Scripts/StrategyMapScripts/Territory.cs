using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Territory
{
    public string name;
    public enum Tribes
    {
        TRIBE_A,
        TRIBE_B,
        TRIBE_C,
        TRIBE_D,
    }

    public enum TypePlayer
    {
        PLAYER,
        BOT,
        NONE
    }

    public Tribes tribe;
    public TypePlayer typePlayer;
    public int moneyRewards;
    public int expRewards;
    public int population;
    public float velocity;

    public void SetStats(int _moneyRewards, int _expRewards,int _population, float _velocity)
    {
        moneyRewards = _moneyRewards;
        expRewards = _expRewards;
        population = _population;
        velocity = _velocity;
    }
}
