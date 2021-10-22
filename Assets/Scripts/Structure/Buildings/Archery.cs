using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Archery : Building
{
    public Archery()
    {
        this.nameBuilding = "Archery";
        this.initValue = 70;
        this.speedUnits = 0f;
        this.limitUnits = 70;
        this.atributteSpeed = 2.0f;
        this.atributteLimit = 10;
        isBuildMilitar = true;
    }
}
