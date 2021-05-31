using UnityEngine;
[System.Serializable]
public class SacredPlace: Building
{
    private float motivation = 0f;

    public SacredPlace()
    {
        this.Name = "SacredPlace";
        this.AtributteToAdd = 0.6f;
    }
    public float Motivation
    {
        get { return motivation; }
        set { motivation = value; }
    }
    public override void ImproveBuilding(int _levels)
    {
        base.ImproveBuilding(_levels);
        this.motivation += this.AtributteToAdd * _levels;
    }
    public override void SubsideBuilding(int _levels)
    {
        base.SubsideBuilding(_levels);
        this.motivation-= (int)this.AtributteToAdd * _levels;
    }
}
