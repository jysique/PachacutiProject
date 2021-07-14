using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Archery : Building
{
    private float speedArcher ;
    private int limitArcher;
    public Archery()
    {
        this.Name = "Archery";
        this.speedArcher = 0f;
        this.limitArcher = 70;
        this.AtributteSpeed = 0.6f;
        this.AtributteLimit = 10;
    }
    public float SpeedArcher
    {
        get { return speedArcher; }
        set { speedArcher = value; }
    }
    public int LimitArcher
    {
        get { return limitArcher; }
        set { limitArcher = value; }
    }
    public override void ImproveBuilding(int _levels)
    {
        base.ImproveBuilding(_levels);
        this.speedArcher += this.AtributteSpeed * _levels;
        this.limitArcher += this.AtributteLimit * _levels;
    }
    public override void SubsideBuilding(int _levels)
    {
        base.SubsideBuilding(_levels);
        this.speedArcher -= this.AtributteSpeed * _levels;
        this.limitArcher -= this.AtributteLimit * _levels;
    }
}
