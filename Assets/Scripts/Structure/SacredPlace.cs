using UnityEngine;
[System.Serializable]
public class SacredPlace: Building
{
    private float motivation = 1.0f;

    public SacredPlace()
    {
        this.Name = "SacredPlace";
    }
    public float Motivation
    {
        get { return motivation; }
        set { motivation = value; }
    }
    public override void ImproveBuilding(int _levels)
    {
        base.ImproveBuilding(_levels);
        this.motivation += 0.6f * _levels;
    }
}
