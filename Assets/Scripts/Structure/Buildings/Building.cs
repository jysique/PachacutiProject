using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Building
{
    [SerializeField] private string nameBuilding;
    [SerializeField] private int level = 0;
    private float atributteSpeed;
    private int atributteLimit;
    private float atributteToAdd;

    private int costToUpgrade = 7;
    private bool canUpgrade = true;

    private TimeSimulated init;
    private float daysToBuild = 4;
    private float daysTotal = 0;
    private int status = -1;
    private int position=-1;
    public int PositionInGridLayout
    {
        get { return position; }
        set { position = value; }
    }
    public int Status
    {
        get { return status; }
        set { status = value; }
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
    public virtual void ImproveBuilding(int _levels)
    {
        this.level += _levels;
        this.daysToBuild += 1 * _levels;
    }
    public virtual void SubsideBuilding(int _levels)
    {
        this.level -= _levels;
    }
    public void ImproveCostUpgrade(int _levels)
    {
        this.CostToUpgrade += 3* _levels;
    }
    public void ResetBuilding()
    {
        SubsideBuilding(Level);
    }
}
