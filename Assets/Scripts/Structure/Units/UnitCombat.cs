using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class UnitCombat : Subordinate
{
    [SerializeField] private int number;
    private string unitName;
    private int plus_damage;
    private int movement;
    
    public float _timeLeft = 0;
    public int _totalTime = 12;
    public int PlusDamage
    {
        get { return plus_damage; }
        set { plus_damage = value; }
    }
    public int Movement
    {
        get { return movement; }
        set { movement = value; }
    }
    public int NumbersUnit
    {
        get { return number; }
        set { number = value; }
    }
    public string UnitName
    {
        get { return unitName;}
        set { unitName = value; }
    }
}
