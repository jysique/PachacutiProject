using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Archery : Building
{
    [SerializeField] private float speedArchers = 0f;
    [SerializeField] private int limitArchers = 70;

    public Archery()
    {
        this.Name = "Archery";
    }
    public float SpeedArchers
    {
        get { return speedArchers; }
        set { speedArchers = value; }
    }
    public int LimitArchers
    {
        get { return limitArchers; }
        set { limitArchers = value; }
    }
    public override void ImproveBuilding(int _levels)
    {
        base.ImproveBuilding(_levels);
        this.speedArchers += this.AtributteSpeed * _levels;
        this.limitArchers += this.AtributteLimit * _levels;
    }
    public override void SubsideBuilding(int _levels)
    {
        base.SubsideBuilding(_levels);
        this.speedArchers -= this.AtributteSpeed * _levels;
        this.limitArchers -= this.AtributteLimit * _levels;
    }
}
