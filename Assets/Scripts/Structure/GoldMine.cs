using UnityEngine;
[System.Serializable]
public class GoldMine:Building
{
    private float velocityGold = 1;
    public GoldMine()
    {
        this.Name = "Gold Mine";
    }
    public float VelocityGold
    {
        get { return velocityGold; }
        set { velocityGold = value; }
    }
    public override void ImproveBuilding(int _levels)
    {
        base.ImproveBuilding(_levels);
        this.velocityGold += 0.6f * _levels;
    }
}
