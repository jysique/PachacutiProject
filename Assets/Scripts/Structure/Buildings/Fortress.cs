using UnityEngine;
[System.Serializable]
public class Fortress: Building
{
    private float plusDefense = 0f;
    public Fortress()
    {
        this.Name = "Fortress";
        this.AtributteToAdd = 1.0f;
        this.AtributteSpeed = 0;
        this.AtributteLimit = 0;
    }
    public float PlusDefense
    {
        get { return plusDefense; }
        set { plusDefense = value; }
    }
    public override void ImproveBuilding(Territory territory)
    {
        base.ImproveBuilding(territory);
        this.plusDefense = this.atributteToAdd * level;
    }
    public override void SubsideBuilding(Territory territory)
    {
        base.SubsideBuilding(territory);
        this.plusDefense = this.atributteToAdd * level;
    }
}
