using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Castle : Building
{
    private float speedAxemen;
    private int limitAxemen;

    public Castle()
    {
        this.Name = "Castle";
        this.speedAxemen = 0f;
        this.limitAxemen = 70;
        this.AtributteSpeed = 0.6f;
        this.AtributteLimit = 10;
    }
    public float SpeedAxemen
    {
        get { return speedAxemen; }
        set { speedAxemen = value; }
    }
    public int LimitAxemen
    {
        get { return limitAxemen; }
        set { limitAxemen = value; }
    }
    public override void ImproveBuilding(int _levels)
    {
        base.ImproveBuilding(_levels);
        this.speedAxemen += this.AtributteSpeed * _levels;
        this.limitAxemen += this.AtributteLimit * _levels;
    }
    public override void SubsideBuilding(int _levels)
    {
        base.SubsideBuilding(_levels);
        this.speedAxemen -= this.AtributteSpeed * _levels;
        this.limitAxemen -= this.AtributteLimit * _levels;
    }
}
