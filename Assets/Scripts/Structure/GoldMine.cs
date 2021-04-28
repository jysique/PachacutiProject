using UnityEngine;
[System.Serializable]
public class GoldMine:Building
{
    [SerializeField]private float velocityGold;

    public float VelocityGold
    {
        get { return velocityGold; }
        set { velocityGold = value; }
    }
    public override void ImproveBuilding()
    {
        base.ImproveBuilding();
        this.velocityGold += 0.6f;
    }
}
