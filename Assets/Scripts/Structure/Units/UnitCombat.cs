using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[System.Serializable]
public class UnitCombat : Subordinate
{
    [SerializeField] private int position;
    [SerializeField] private int in_progress;
    [SerializeField] private int quantity;
    [SerializeField] private bool isAvailable = false;

    
    [SerializeField] protected int attack;
    [SerializeField] protected int armor;
    [SerializeField] protected int evasion;
    [SerializeField] protected int precision;
    [SerializeField] protected int range;
    [SerializeField] protected string[] strongTo;
    [SerializeField] protected string[] weakTo;
    [SerializeField] protected int typeArm;
    [SerializeField] protected string audio;

    public UnitCombat()
    {

    }
    public UnitCombat(string name, int quantity, string icontype, string index, int attack, int armor, 
        int precision, int range, string[] strongTo, string[] weakTo)
    {
        this.quantity = quantity;
        this.characterName = name;
        this.characIconType = icontype;
        this.characIconIndex = index;
        this.attack = attack;
        this.armor = armor;
        this.audio = this.characIconType + "/" + this.characIconIndex;
        this.evasion = 0;
        this.precision = precision;
        this.range = range;
        this.strongTo = strongTo;
        this.weakTo = weakTo;
    }

    public string GetDescription()
    {
        return this.characterName + " (" + quantity + ")";
    }
    public string GetStats()
    {
        return "BASE DAMAGE: " + attack + "\n" +
            "ARMOR: " + armor + "\n" +
            "PRECISION: " + precision + "\n" +
            "CRITIC: " + 10 + "\n";
    }

    public string PathAudio
    {
        get { return audio; }
    }
    public bool IsAvailable
    {
        get { return isAvailable; }
        set { isAvailable = value; }
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
    public int Evasion
    {
        get { return evasion; }
        set { evasion = value; }
    }
    public int Armor
    {
        get { return armor; }
        set { armor = value; }
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


