using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Stable : Building
{

    public Stable()
    {
        this.Name = "Stable";
        this.initValue = 10;
        this.speedUnits = 0f;
        this.limitUnits = 10;
        this.atributteSpeed = 1.2f;
        this.atributteLimit = 2;
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
        this.limitUnits = 10 + this.AtributteLimit * levelLimit;
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
        this.limitUnits = 10  + this.atributteLimit * levelLimit;
        this.materialUnits = levelMaterials;
    }
    */
}
