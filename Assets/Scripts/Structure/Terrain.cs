using System;
using UnityEngine;

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
                a = (value / 100) * ug.UnitCombat.Armor;
                this.plusDefense = (int)a;
                this.attribute = " +" + value + "% armor";
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
