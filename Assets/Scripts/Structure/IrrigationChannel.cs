using UnityEngine;
[System.Serializable]
public class IrrigationChannel : Building
{
    private float velocityfood = 1f;
    public IrrigationChannel()
    {
        this.Name = "Irrigation Channel";
    }
    public float VelocityFood
    {
        get { return velocityfood; }
        set { velocityfood = value; }
    }
    public override void ImproveBuilding(int _levels)
    {
        base.ImproveBuilding(_levels);
        this.velocityfood += 0.6f * _levels;
    }
}
