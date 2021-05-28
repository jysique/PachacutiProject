using UnityEngine;
[System.Serializable]
public class IrrigationChannel : Building
{
    //  private float velocityfood = 0f;
    private int workersChannel = 0;
    public IrrigationChannel()
    {
        this.Name = "Irrigation Channel";
        this.AtributteToAdd = 5f;
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
}
