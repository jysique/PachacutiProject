using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Stable : Building
{
     private float speedScouts;
     private int limitScouts;

    public Stable()
    {
        this.Name = "Stable";
        this.speedScouts = 0f;
        this.limitScouts = 10;
        this.AtributteSpeed = 0.2f;
        this.AtributteLimit = 2;
    }
    public float SpeedScouts
    {
        get { return speedScouts; }
        set { speedScouts = value; }
    }
    public int LimitScouts
    {
        get { return limitScouts; }
        set { limitScouts = value; }
    }
    public override void ImproveBuilding(int _levels)
    {
        base.ImproveBuilding(_levels);
        this.speedScouts += this.AtributteSpeed * _levels;
        this.limitScouts += this.AtributteLimit * _levels;
    }
    public override void SubsideBuilding(int _levels)
    {
        base.SubsideBuilding(_levels);
        this.speedScouts -= this.AtributteSpeed * _levels;
        this.limitScouts -= this.AtributteLimit * _levels;
    }
}
