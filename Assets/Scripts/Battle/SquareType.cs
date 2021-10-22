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
        if (unitGroup != null && unitGroup.UnitCombat != null)
        {
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