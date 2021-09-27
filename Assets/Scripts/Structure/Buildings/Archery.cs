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
    /*
    public override void ImproveBuilding()
    {
        base.ImproveBuilding();
        if (this.level % 3 == 0)
        {
            levelSpeed++;
        }
        else if (this.level % 3 == 1)
        {
            levelLimit++;
            levelSpeed++;
        }
        else
        {
            levelMaterials++;
            levelSpeed++;
        }
        this.speedUnits = this.AtributteSpeed * levelSpeed;
        this.limitUnits = 70 + this.AtributteLimit * levelLimit;
        this.materialUnits = levelMaterials;
    }
    public override void SubsideBuilding()
    {
        base.SubsideBuilding();
        if (this.Level % 3 == 0)
        {
            levelSpeed--;
        }
        else if (this.Level % 3 == 1)
        {
            levelLimit--;
        }
        else
        {
            levelMaterials--;
        }
        this.speedUnits = this.atributteSpeed * levelSpeed;
        this.limitUnits = 70 +this.atributteLimit * levelLimit;
        this.materialUnits = levelMaterials;
    }
    */
}
