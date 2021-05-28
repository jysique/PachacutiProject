using UnityEngine;
[System.Serializable]
public class Fortress: Building
{
    private float plusDefense = 0f;
    public Fortress()
    {
        this.Name = "Fortress";
        this.AtributteToAdd = 1.0f;
    }
    public float PlusDefense
    {
        get { return plusDefense; }
        set { plusDefense = value; }
    }
    public override void ImproveBuilding(int _levels)
    {
        base.ImproveBuilding(_levels);
        this.plusDefense += this.AtributteToAdd *_levels;
    }
}
