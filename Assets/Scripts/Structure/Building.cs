using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Building
{
    private string nameBuilding;
    private int level = 0;
    private float atributteToAdd;

    private int costToUpgrade = 7;
    private bool canUpgrade = true;

    private TimeSimulated init;
    private float daysToBuild = 4;
    private float daysTotal = 0;
        
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
    public virtual void ImproveBuilding(int _levels)
    {
        this.level += _levels;
        this.daysToBuild += 1 * _levels;
    }
    public virtual void SubsideBuilding(int _levels)
    {
        this.level -= _levels;
    }
    public void ImproveCostUpgrade()
    {
        this.CostToUpgrade += 3;
    }
    public void ResetBuilding()
    {
        SubsideBuilding(Level);
    }
}
