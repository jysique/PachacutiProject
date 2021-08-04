
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
    public enum TYPE
    {
        GRASSLAND,
        FOREST,
        MOUNTAIN,
        SAND,
        NONE
    }

    [SerializeField]private TYPE type;
    private Sprite picture;
    public Terrain(string _type)
    {
        this.type = (TYPE)Enum.Parse(typeof(TYPE), _type);
    }
    public Terrain()
    {

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

}