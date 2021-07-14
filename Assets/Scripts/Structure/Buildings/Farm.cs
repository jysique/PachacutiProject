using UnityEngine;
[System.Serializable]
public class Farm : Building
{
    //  private float velocityfood = 0f;
    private int workersChannel = 0;

    public Farm()
    {
        this.Name = "Farm";
        this.AtributteToAdd = 5f;
        this.AtributteSpeed = 0;
        this.AtributteLimit = 0;
    }
    public int WorkersChannel
    {
        get { return workersChannel; }
        set { workersChannel = value; }
    }
    public override void ImproveBuilding(int _levels)
    {
        base.ImproveBuilding(_levels);
        this.workersChannel += (int)this.AtributteToAdd * _levels;
    }
    public override void SubsideBuilding(int _levels)
    {
        base.SubsideBuilding(_levels);
        this.workersChannel -= (int)this.AtributteToAdd * _levels;
    }

}
