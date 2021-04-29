using UnityEngine;
[System.Serializable]
public class Fortress: Building
{
    private float plusDefense = 1f;
    public Fortress()
    {
        this.Name = "Fortress";
    }
    public float PlusDefense
    {
        get { return plusDefense; }
        set { plusDefense = value; }
    }
    public override void ImproveBuilding(int _levels)
    {
        base.ImproveBuilding(_levels);
        this.plusDefense += 1*_levels;
    }
}
