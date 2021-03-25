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
    [SerializeField] private MilitarBoss militarBoss;

    public MilitarBoss MilitarBoss
    {
        get { return militarBoss; }
        set { militarBoss = value; }
    }
    /*
    public MilitarBoss GetMilitarBoss()
    {
        return militarBoss;
    }
    */
    public TypePlayer GetTypePlayer()
    {
        return typePlayer;
    }
    public int GetPopulation()
    {
        return population;
    }
    public void SetPopulation(int value)
    {
        Debug.Log("change population");
        population = value;
    }
    public float GetVelocity()
    {
        return velocity;
    }

    public void SetTypePlayer(TypePlayer _typePlayer)
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
    public bool GetSelected() { return selected; }


}
