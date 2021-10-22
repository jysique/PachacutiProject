using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[System.Serializable]
public class UnitGroup
{
    [SerializeField] UnitCombat unitCombat = null;
    [SerializeField] bool defense;
    [SerializeField] private bool active;
    private bool inmunity;
    Territory.TYPEPLAYER typePlayer;
    GameObject unitsGO;


    public UnitGroup(Territory.TYPEPLAYER tp, GameObject ugo, UnitCombat uc)
    {
        unitsGO = ugo;
        typePlayer = tp;
        unitCombat = uc;
        defense = false;
        active = true;
    }
    public GameObject UnitsGO
    {
        get { return unitsGO; }
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
    public UnitCombat UnitCombat { 
        get => unitCombat; 
        set => unitCombat = value; 
    }
   
    public bool Active
    {
        get { return active; }
        set { active = value; }
    }
    public bool Inmunity
    {
        get { return inmunity; }
        set { inmunity = value; }
    }

    public string GetStats()
    {
        return "QUANTITY: " + unitCombat.Quantity + "\n" +
            "BASE DAMAGE: " + unitCombat.Attack + "\n" +
            "ARMOR: " + unitCombat.Armor + "\n" +
            "PRECISION: " + unitCombat.Precision + "\n" +
            "CRITIC: " + 10 + "\n";
    }
}
