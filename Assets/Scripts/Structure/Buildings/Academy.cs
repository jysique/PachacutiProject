using UnityEngine;
[System.Serializable]
public class Academy: Building
{
    public Academy()
    {
        this.initValue = 70;
        this.nameBuilding = "Academy";
        this.materialUnits = 1;
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
    }
    public override void SubsideBuilding()
    {
        base.SubsideBuilding();
    }
    */
}
