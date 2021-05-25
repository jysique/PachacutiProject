using UnityEngine;
[System.Serializable]
public class IrrigationChannel : Building
{
    private float velocityfood = 0f;
    public IrrigationChannel()
    {
        this.Name = "Irrigation Channel";
        this.AtributteToAdd = 0.6f;
    }
    public float VelocityFood
    {
        get { return velocityfood; }
        set { velocityfood = value; }
    }
    public override void ImproveBuilding(int _levels)
    {
        base.ImproveBuilding(_levels);
        this.velocityfood += this.AtributteToAdd * _levels;
    }
}
