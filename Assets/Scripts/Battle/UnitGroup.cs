using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UnitGroup
{
    
    string type;
    int quantity;
    bool defense;
    Territory.TYPEPLAYER typePlayer;
    GameObject unitsGO;

    public UnitGroup(Territory.TYPEPLAYER tp,string t, int q, GameObject ugo)
    {
        unitsGO = ugo;
        typePlayer = tp;
        type = t;
        quantity = q;
        defense = false;
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

    public string Type
    {
        get { return type; }
    }
}
