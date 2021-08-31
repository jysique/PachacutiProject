using UnityEngine;
[System.Serializable]
public class GoldMine:Building
{
    public GoldMine()
    {
        this.nameBuilding = "GoldMine";
        this.limitUnits = 0;
        this.atributteLimit = 5;
    }
    public override void ImproveBuilding(Territory territory)
    {
        base.ImproveBuilding(territory);
        this.limitUnits = this.atributteLimit* level;
    }
    public override void SubsideBuilding(Territory territory)
    {
        base.SubsideBuilding(territory);
        this.limitUnits = this.atributteLimit * level;
    }
}
