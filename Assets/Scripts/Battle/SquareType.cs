using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SquareType : MonoBehaviour
{
    public enum TYPESQUARE
    {
        GRASSLAND,
        FOREST,
        MOUNTAIN,
        SAND,
        NONE 
    }
    public bool haveUnit = false;
    public int index;
    public TYPESQUARE typeSquare;
    public UnitGroup unitGroup;

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
