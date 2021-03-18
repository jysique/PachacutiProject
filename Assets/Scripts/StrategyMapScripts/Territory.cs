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

    [SerializeField] private Tribes tribe;
    [SerializeField] private TypePlayer typePlayer;
    [SerializeField] private int moneyRewards;
    [SerializeField] private int expRewards;
    [SerializeField] private int population;
    [SerializeField] private float velocity;
    [SerializeField] private bool selected;

    public TypePlayer getTypePlayer()
    {
        return typePlayer;
    }
    public int getPopulation()
    {
        return population;
    }
    public float getVelocity()
    {
        return velocity;
    }

    public void setTypePlayer(TypePlayer _typePlayer)
    {
        typePlayer = _typePlayer;
    }
    public void SetStats(int _moneyRewards, int _expRewards,int _population, float _velocity)
    {
        moneyRewards = _moneyRewards;
        expRewards = _expRewards;
        population = _population;
        velocity = _velocity;
    }

    public void SetSelected(bool _selected) { selected = _selected; }

}
