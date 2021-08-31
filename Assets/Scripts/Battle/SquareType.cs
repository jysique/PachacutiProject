
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SquareType : MonoBehaviour
{
    public bool haveUnit = false;
    public int index;
    public Terrain terrain;
    public UnitGroup unitGroup = null;

    private void Start()
    {
        DeactivateChange();
        if (unitGroup != null)
        {
            terrain.GetValue(unitGroup.UnitCombat);
        }
    }
    public void MoveButton()
    {
        CombatManager.instance.ChangeUnits(index, CombatManager.instance.ActualUnitIndex());
        
    }

    private void Update()
    {
        //print(terrain.Attribute);
        if (unitGroup != null)
        {
            //print(index + "-" + unitGroup.UnitCombat.CharacterName);
            haveUnit = true;
        }
        else {
            //print(index + "- no unitCombat");
            haveUnit = false;
        }
    }
    public void UpdateSquare()
    {
        if (unitGroup != null)
        {
            terrain.GetValue(unitGroup.UnitCombat);
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
    public void GetValue(UnitCombat uc)
    {
        int a = UnityEngine.Random.Range(this.scale*10, (this.scale+1)* 10);
        switch (this.type)
        {
            case TYPE.GRASSLAND:
                this.attribute = " +" + a + "% idk";
                break;
            case TYPE.FOREST:
                uc.Attack = uc.Attack+ (a / 100) * uc.Attack;
                this.attribute = " +" + a + "% attack";
                break;
            case TYPE.MOUNTAIN:
                uc.Precision = uc.Precision+(a / 100) * uc.Precision;
                this.attribute = " +" + a + "% presicion";
                break;
            case TYPE.SAND:
                uc.Defense = uc.Defense + (a / 100) * uc.Defense;
                this.attribute = " +" + a + "% defense";
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