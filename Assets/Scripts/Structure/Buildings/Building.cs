using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class Building
{
    [Header("Stats build")]
    [SerializeField] protected string nameBuilding;
    [SerializeField] protected int level = 0;

    protected float atributteSpeed;
    protected int atributteLimit;
    protected float atributteToAdd;
    
    protected float speedUnits;
    protected int limitUnits;
    protected int materialUnits;

    protected int levelSpeed = 0;
    protected int levelLimit = 0;
    protected int levelMaterials = 1;

    protected int initValue;
    protected bool isBuildMilitar = false;
    private bool canUpgrade = true;


    private int costToUpgrade = 7;

    private float daysToBuild = 3;

    private int position = -1;
    private int status = -1;

    [SerializeField] private float percentaje;
    public float Percentaje
    {
        get { return percentaje; }
        set { percentaje = value; }
    }
    public bool IsMilitary
    {
        get { return isBuildMilitar; }
    }
    public float SpeedUnits
    {
        get { return speedUnits; }
        set { speedUnits = value; }
    }
    public int LimitUnits
    {
        get { return limitUnits; }
        set { limitUnits = value; }
    }
    public int MaterialUnits
    {
        get { return materialUnits; }
        set { materialUnits = value; }
    }

    public int Status
    {
        get { return status; }
        set { status = value; }
    }
    public int PositionInGridLayout
    {
        get { return position; }
        set { position = value; }
    }
    
    public string Name
    {
        get { return nameBuilding; }
        set { nameBuilding = value; }
    }
    public int Level
    {
        get { return level; }
        set { level = value; }
    }
    public float AtributteToAdd
    {
        get { return atributteToAdd; }
        set { atributteToAdd = value; }
    }
    public float AtributteSpeed
    {
        get { return atributteSpeed; }
        set { atributteSpeed = value; }
    }
    public int AtributteLimit
    {
        get { return atributteLimit; }
        set { atributteLimit = value; }
    }
    public int CostToUpgrade
    {
        get { return costToUpgrade; }
        set { costToUpgrade = value; }
    }
    public bool CanUpdrade
    {
        get { return canUpgrade; }
        set { canUpgrade = value; }
    }
    public float DaysToBuild
    {
        get { return daysToBuild; }
        set { daysToBuild = value; }
    }

   
    public virtual void ImproveBuilding(Territory territory)
    {
        this.level++;
        this.daysToBuild++;
        if (this.level % 3 == 0)
        {
            levelMaterials++;
            levelSpeed++;
            List<UnitCombat> ucs = territory.ListUnitCombat.GetUnitListByBuild(this.GetType().ToString());
            //Debug.LogWarning("uc encntrados por " + this.GetType().ToString() + ":" +ucs.Count);
            foreach (var item in ucs)
            {
                item.Attack += UnityEngine.Random.Range(1, 3);
                item.Precision += UnityEngine.Random.Range(1, 2);
            }
        }
        else if (this.level % 3 == 1)
        {
            levelSpeed++;
        }
        else
        {
            levelLimit++;
            levelSpeed++;
        }
        this.speedUnits = this.AtributteSpeed * levelSpeed;
        this.limitUnits = this.initValue + this.AtributteLimit * levelLimit;
        this.materialUnits = levelMaterials;
    }
    public virtual void SubsideBuilding(Territory territory)
    {
        this.level--;
        this.daysToBuild--;
        if (this.Level % 3 == 0)
        {
            levelMaterials--;
            levelSpeed--;
            List<UnitCombat> ucs = territory.ListUnitCombat.GetUnitListByBuild(this.GetType().ToString());
            //Debug.LogWarning("uc encntrados por " + this.GetType().ToString() + ":" +ucs.Count);
            foreach (var item in ucs)
            {
                item.Attack -= UnityEngine.Random.Range(1, 3);
                item.Precision -= UnityEngine.Random.Range(1, 2);
            }
            //territory.ListUnitCombat.GetFirstUnitCombat(this.GetType().ToString()).Attack -= 2;
            //territory.ListUnitCombat.GetFirstUnitCombat(this.GetType().ToString()).Precision -= 2;
        }
        else if (this.Level % 3 == 1)
        {
            levelSpeed--;
        }
        else
        {
            levelLimit--;
            levelSpeed--;
        }
        this.speedUnits = this.atributteSpeed * levelSpeed;
        this.limitUnits = this.initValue + this.atributteLimit * levelLimit;
        this.materialUnits = levelMaterials;
    }


    public void ImproveCostUpgrade(int _levels)
    {
        this.CostToUpgrade += 3* _levels;
    }
    public void ImproveManyLevels(int _levels, Territory territory)
    {
        for (int i = 0; i < _levels; i++)
        {
            ImproveBuilding(territory);
        }
    }
    public void SubsideManyLevels(int _levels,Territory territory)
    {
        for (int i = 0; i < _levels; i++)
        {
            SubsideBuilding(territory);
        }
    }
    public void ResetBuilding(Territory territory)
    {
        SubsideManyLevels(level, territory);
    }
}
