using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class UnitGroup
{
    /*
    string type;
    int quantity;
    */
    [SerializeField] bool defense;
    [SerializeField] private bool active;
    [SerializeField] UnitCombat unitCombat;
    Territory.TYPEPLAYER typePlayer;
    GameObject unitsGO;
    /*
    public UnitGroup(Territory.TYPEPLAYER tp,string t, int q, GameObject ugo)
    {
        unitsGO = ugo;
        typePlayer = tp;
       // type = t;
       // quantity = q;
       // defense = false;
    }
    */
    public UnitGroup(Territory.TYPEPLAYER tp, GameObject ugo, UnitCombat uc)
    {
        unitsGO = ugo;
        typePlayer = tp;
        unitCombat = uc;
        defense = false;
        active = true;
    }

    public void Log()
    {
    //    Debug.Log("type|" + type + "|q|" + quantity + "|d|" + defense);
    }
    public GameObject UnitsGO
    {
        get { return unitsGO; }
    }
    /*
    public int Quantity
    {
        get { return quantity; }
        set { quantity = value; }
    }
    */
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
    /*
    public string Type
    {
        get { return type; }
    }
    */
    public UnitCombat UnitCombat { 
        get => unitCombat; 
        set => unitCombat = value; 
    }
   
    public bool Active
    {
        get { return active; }
        set { active = value; }
    }
}
