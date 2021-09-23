using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class UnitCombat : Subordinate
{
    [SerializeField] private int in_progress;
    [SerializeField] private int quantity;
    [SerializeField] private string unitName;
    [SerializeField] private int attack;
    [SerializeField] private int defense;
    [SerializeField] private int evasion;
    [SerializeField] private int precision;
    [SerializeField] private int range;
    [SerializeField] private string[] strongTo;
    [SerializeField] private string[] weakTo;
    [SerializeField] private int position;
    [SerializeField] protected int typeArm;
    private float daysToCreate;
    private float daysTotal = 0;

    [SerializeField] private TimeSimulated init;
    [SerializeField] private bool isAvailable = false;
    public bool IsAvailable
    {
        get { return isAvailable; }
        set { isAvailable = value; }
    }
    public TimeSimulated TimeInit
    {
        get { return init; }
        set { init = value; }
    }

    public float DaysToCreate
    {
        get { return daysToCreate; }
        set { daysToCreate = value; }
    }
    public float DaysTotal
    {
        get { return daysTotal; }
        set { daysTotal = value; }
    }
    public int TypeArm
    {
        get { return typeArm; }
        set { typeArm = value; }
    }
    public int PositionInBattle
    {
        get { return position; }
        set { position = value; }
    }

    public int InProgress
    {
        get { return in_progress; }
        set { in_progress = value; }
    }

    public int Quantity
    {
        get { return quantity; }
        set { quantity = value; }
    }
    public string UnitName
    {
        get { return unitName; }
        set { unitName = value; }
    }
    public int Evasion
    {
        get { return evasion; }
        set { evasion = value; }
    }
    public int Defense
    {
        get { return defense; }
        set { defense = value; }
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


