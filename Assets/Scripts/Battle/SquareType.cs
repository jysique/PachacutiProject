 
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SquareType : MonoBehaviour
{
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
        else {
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
            Utils.instance.Reset(unitGroup.UnitCombat);
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
    [SerializeField] private float value;
    private Sprite picture;
    public string Attribute
    {
        get { return attribute; }
        set { attribute = value; }
    }
    public Terrain(string _type)
    {
        this.type = (TYPE)Enum.Parse(typeof(TYPE), _type);
        this.scale = UnityEngine.Random.Range(1, 5);
        this.attribute = "";
    }
    public void GetValue(UnitGroup ug)
    {
        /*
        Debug.Log("=====================");
        Debug.Log("type|" + ug.TypePlayer);
        Debug.Log(ug.TypePlayer+"|name|" + ug.UnitCombat.UnitName);
        Debug.Log(ug.TypePlayer+"|attack|" + ug.UnitCombat.Attack);
        Debug.Log(ug.TypePlayer+"|precision|" + ug.UnitCombat.Precision);
        Debug.Log(ug.TypePlayer+"|defense|" + ug.UnitCombat.Defense);
        */
        value = UnityEngine.Random.Range(this.scale*10, (this.scale+1)* 10);
        //Debug.Log("value|" + value);
        float a;
        switch (this.type)
        {
            case TYPE.GRASSLAND:
                a = (value / 100);
                ug.UnitCombat.Evasion += (int)a;
                this.attribute = " +" + value + "% evasion";
                break;
            case TYPE.FOREST:
                a = (value / 100) * ug.UnitCombat.Attack;
                ug.UnitCombat.Attack += (int)a;
                this.attribute = " +" + value + "% attack";
                break;
            case TYPE.MOUNTAIN:
                a = (value / 100) * ug.UnitCombat.Precision;
                ug.UnitCombat.Precision += (int)a;
                if (ug.UnitCombat.Precision > 100)
                {
                    ug.UnitCombat.Precision = 100;
                }
                this.attribute = " +" + value + "% presicion";
                break;
            case TYPE.SAND:
                a = (value / 100) * ug.UnitCombat.Defense;
                ug.UnitCombat.Defense += (int)a;
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
