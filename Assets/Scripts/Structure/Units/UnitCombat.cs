using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class UnitCombat : Subordinate
{
    [SerializeField] private int quantity;
    [SerializeField] private string unitName;
    [SerializeField] private int attack;
    [SerializeField] private int precision;
    [SerializeField] private int range;
    [SerializeField] private string[] strongTo;
    [SerializeField] private string[] weakTo;

    public float _timeLeft = 0;
    public int _totalTime = 12;

    public int Quantity
    {
        get { return quantity; }
        set { quantity = value; }
    }
    public string UnitName
    {
        get { return unitName;}
        set { unitName = value; }
    }
    public int Attack
    {
        get { return attack; }
        set { attack = value; }
    }
    public int Precision
    {
        get { return precision; }
        set { precision = value; }
    }
    public int Range
    {
        get { return range; }
        set { range = value; }
    }
    public string[] StrongTo
    {
        get { return strongTo; }
        set { strongTo = value; }
    }
    public string[] WeakTo
    {
        get { return weakTo; }
        set { weakTo = value; }
    }
}
