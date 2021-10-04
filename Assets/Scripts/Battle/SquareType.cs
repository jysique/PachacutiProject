using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SquareType : MonoBehaviour
{
    public int i;
    public int j;
    [SerializeField] private bool haveUnit;
    [SerializeField] private int index;
    [SerializeField] private Terrain terrain;
    [SerializeField] private UnitGroup unitGroup;

    public bool HaveUnit
    {
        get { return haveUnit; }
        set { haveUnit = value; }
    }
    public int Index
    {
        get { return index; }
        set { index = value; }
    }
    public Terrain Terrain
    {
        get { return terrain; }
        set { terrain = value; }
    }
    public UnitGroup UnitGroup
    {
        get { return unitGroup; }
        set { unitGroup = value; }
    }

    private void Start()
    {
        DeactivateChange();
    }
    public void MoveButton()
    {
        CombatManager.instance.ChangeUnits(index, CombatManager.instance.ActualUnitIndex());
    }
    private void Update()
    {
        if (unitGroup != null)
        {
            haveUnit = true;
        }
        else
        {
            haveUnit = false;
        }
    }
    public void UpdateSquare()
    {
        //print("type" + unitGroup.TypePlayer.ToString());
        //print("unit name " + unitGroup.UnitCombat.UnitName);
        //print("char name " + unitGroup.UnitCombat.CharacterName);
        if (unitGroup != null && unitGroup.UnitCombat != null)
        {
            //print("reset");
            //            Utils.instance.Reset(unitGroup.UnitCombat);
//            Utils.instance.Reset(unitGroup.UnitCombat);
            terrain.GetValue(unitGroup);
        }
        else
        {
            terrain.Attribute = "no attribute";
        }
    }
    public void ActivateChange()
    {
        GetComponent<Button>().enabled = true;
    }
    public void DeactivateChange()
    {
        GetComponent<Button>().enabled = false;
    }
}

[System.Serializable]
public class Terrain
{
    [SerializeField] private TYPE type;
    [SerializeField] private int scale;
    [SerializeField] private string attribute;
    [SerializeField] private int plusEvasion;
    [SerializeField] private int plusAttack;
    [SerializeField] private int plusPrecision;
    [SerializeField] private int plusDefense;
    [SerializeField] private float value;
    [SerializeField] private int position;
    private Sprite picture;
    public int Position
    {
        get { return position; }
    }
    public string Attribute
    {
        get { return attribute; }
        set { attribute = value; }
    }
    public int Plus_evasion
    {
        get { return plusEvasion; }
    }
    public int Plus_attack
    {
        get { return plusAttack; }
    }
    public int Plus_defense
    {
        get { return plusDefense; }
    }
    public int Plus_presicion
    {
        get { return plusPrecision; }
    }
    public Terrain(string _type, int _position)
    {
        this.position = _position;
        this.type = (TYPE)Enum.Parse(typeof(TYPE), _type);
        this.scale = UnityEngine.Random.Range(1, 5);
        this.plusAttack = 0;
        this.plusDefense = 0;
        this.plusPrecision = 0;
        this.plusEvasion = 0;

        this.attribute = "";
    }
    public void GetValue(UnitGroup ug)
    {
        value = UnityEngine.Random.Range(this.scale * 10, (this.scale + 1) * 10);
        float a;
        //int b;
        switch (this.type)
        {
            case TYPE.GRASSLAND:
                a = (value / 100) * ug.UnitCombat.Evasion;
                this.plusEvasion = (int)a;
                this.attribute = " +" + value + "% evasion";
                break;
            case TYPE.FOREST:
                a = (value / 100) * ug.UnitCombat.Attack;
                this.plusAttack = (int)a;
                this.attribute = " +" + value + "% attack";
                break;
            case TYPE.MOUNTAIN:
                a = (value / 100) * ug.UnitCombat.Precision;
                this.plusPrecision = (int)a;
                this.attribute = " +" + value + "% presicion";
                break;
            case TYPE.SAND:
                a = (value / 100) * ug.UnitCombat.Defense;
                this.plusDefense = (int)a;
                this.attribute = " +" + value + "% defense";
                break;
            case TYPE.NONE:
                break;
            default:
                break;
        }
    }
    public TYPE Type
    {
        get { return type; }
        set { type = value; }
    }
    public Sprite Picture
    {
        get { return Resources.Load<Sprite>("Textures/TemporalAssets/terrain/" + type.ToString().ToLower()); }
        set { picture = value; }
    }

    // precision o defensa
    public enum TYPE
    {
        GRASSLAND,
        FOREST,
        MOUNTAIN,
        SAND,
        NONE
    }
}
