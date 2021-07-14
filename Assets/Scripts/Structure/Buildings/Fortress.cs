using UnityEngine;
[System.Serializable]
public class Fortress: Building
{
    private float plusDefense = 0f;
    public Fortress()
    {
        this.Name = "Fortress";
        this.AtributteToAdd = 1.0f;
        this.AtributteSpeed = 0;
        this.AtributteLimit = 0;
    }
    public float PlusDefense
    {
        get { return plusDefense; }
        set { plusDefense = value; }
    }
    public override void ImproveBuilding(int _levels)
    {
        base.ImproveBuilding(_levels);
        this.plusDefense += this.AtributteToAdd *_levels;
    }
    public override void SubsideBuilding(int _levels)
    {
        base.SubsideBuilding(_levels);
        this.plusDefense -= (int)this.AtributteToAdd * _levels;
    }
}
