using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitGroup
{
    public enum TYPE
    {
        SWORD,
        SPEAR,
        AXE
    }
    TYPE type;
    int quantity;
    bool defense;
    Territory.TYPEPLAYER typePlayer;
    GameObject unitsGO;

    public UnitGroup(Territory.TYPEPLAYER tp,TYPE t, int q, GameObject ugo)
    {
        unitsGO = ugo;
        typePlayer = tp;
        type = t;
        quantity = q;
        defense = false;
    }
    public void Log()
    {
        Debug.Log("type|" + type + "|q|" + quantity + "|d|" + defense);
    }
    public GameObject UnitsGO
    {
        get { return unitsGO; }
    }

    public int Quantity
    {
        get { return quantity; }
        set { quantity = value; }
    }

    public bool Defense
    {
        get { return defense; }
        set { defense = value; }
    }

    public Territory.TYPEPLAYER TypePlayer
    {
        get { return typePlayer; }
        set { typePlayer = value; }
    }

    public TYPE Type
    {
        get { return type; }
    }
}
