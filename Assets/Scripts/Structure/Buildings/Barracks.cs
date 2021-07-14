using UnityEngine;
[System.Serializable]
public class Barracks : Building
{
    private float speedLancer = 0f;
    private int limitLancer = 70;

    public Barracks()
    {
        this.Name ="Barracks";
        this.speedLancer = 0f;
        this.limitLancer = 70;
        this.AtributteSpeed = 0.6f;
        this.AtributteLimit = 10;
    }
    public float SpeedLancer
    {
        get { return speedLancer; }
        set { speedLancer = value; }
    }
    public int LimitLancer
    {
        get { return limitLancer; }
        set { limitLancer = value; }
    }
    public override void ImproveBuilding(int _levels)
    {
        base.ImproveBuilding(_levels);
        this.speedLancer += this.AtributteSpeed * _levels;
        this.limitLancer += this.AtributteLimit * _levels;

    }
    public override void SubsideBuilding(int _levels)
    {
        base.SubsideBuilding(_levels);
        this.speedLancer -= (int)this.AtributteSpeed * _levels;
        this.limitLancer -= (int)this.AtributteLimit * _levels;
    }
}
