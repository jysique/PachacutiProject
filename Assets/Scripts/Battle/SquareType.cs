using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquareType : MonoBehaviour
{
    /*
    public enum TYPESQUARE
    {
        GRASSLAND,
        FOREST,
        MOUNTAIN,
        SAND,
        NONE 
    }
    */
    public bool haveUnit = false;
    public int index;
    //public TYPESQUARE typeSquare;
    public Ambience ambience;
    public UnitGroup unitGroup = null;

    private void Start()
    {
        DeactivateChange();
        
    }
    public void MoveButton()
    {
        CombatManager.instance.ChangeUnits(index, CombatManager.instance.ActualUnitIndex());
        CombatManager.instance.MakeMovement();
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
public class Ambience
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