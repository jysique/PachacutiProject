using UnityEngine;
[System.Serializable]
public class GoldMine:Building
{
    private float velocityGold = 0;
    public GoldMine()
    {
        this.Name = "Gold Mine";
        this.AtributteToAdd = 0.6f;
    }
    public float VelocityGold
    {
        get { return velocityGold; }
        set { velocityGold = value; }
    }
    public override void ImproveBuilding(int _levels)
    {
        base.ImproveBuilding(_levels);
        this.velocityGold += this.AtributteToAdd * _levels;
    }
}
