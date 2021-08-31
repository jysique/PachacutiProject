using UnityEngine;
using UnityEngine.UI;
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

    private TimeSimulated init;
    private int costToUpgrade = 7;
    private float daysToBuild = 3;
    private float daysTotal = 0;
    private int position = -1;
    private int status = -1;

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
    public float DaysTotal
    {
        get { return daysTotal; }
        set { daysTotal = value; }
    }
    public TimeSimulated TimeInit
    {
        get { return init; }
        set { init = value; }
    }
    /*
    public virtual void ImproveBuilding()
    {
        this.level++;
        this.daysToBuild++;
        if (this.level % 3 == 0)
        {
            levelMaterials++;
            levelSpeed++;
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
    */
    public virtual void ImproveBuilding(Territory territory)
    {
        this.level++;
        this.daysToBuild++;
        if (this.level % 3 == 0)
        {
            levelMaterials++;
            levelSpeed++;
            territory.GetUnitCombat(this.GetType().ToString()).Attack+=3;
            territory.GetUnitCombat(this.GetType().ToString()).Precision += 2;
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
            territory.GetUnitCombat(this.GetType().ToString()).Attack -= 2;
            territory.GetUnitCombat(this.GetType().ToString()).Precision -= 2;
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
    public void ResetBuilding(Territory territory)
    {
        SubsideManyLevels(level,territory);
    }
    /*
    public void ImproveManyLevels(int _levels)
    {
        for (int i = 0; i < _levels; i++)
        {
            ImproveBuilding();
        }
    }



    public void SubsideManyLevels(int _levels)
    {
        for (int i = 0; i < _levels; i++)
        {
            SubsideBuilding();
        }
    }
        */
    public void SubsideManyLevels(int _levels,Territory territory)
    {
        for (int i = 0; i < _levels; i++)
        {
            SubsideBuilding(territory);
        }
    }
    public void ImproveManyLevels(int _levels, Territory territory)
    {
        for (int i = 0; i < _levels; i++)
        {
            ImproveBuilding(territory);
        }
    }
}
