using UnityEngine;
[System.Serializable]
public class Academy: Building
{
    private float speedSwordsmen;
    private int limitSwordsmen ;

    public Academy()
    {
        this.Name = "Academy";
        this.speedSwordsmen = 0f;
        this.limitSwordsmen = 70;
        this.AtributteSpeed = 0.6f;
        this.AtributteLimit = 10;
    }
    public float SpeedSwordsmen
    {
        get { return speedSwordsmen; }
        set { speedSwordsmen = value; }
    }
    public int LimitSwordsmen
    {
        get { return limitSwordsmen; }
        set { limitSwordsmen = value; }
    }
    public override void ImproveBuilding(int _levels)
    {
        base.ImproveBuilding(_levels);
        this.speedSwordsmen += this.AtributteSpeed * _levels;
        this.limitSwordsmen += this.AtributteLimit * _levels;
    }
    public override void SubsideBuilding(int _levels)
    {
        base.SubsideBuilding(_levels);
        this.speedSwordsmen -= this.AtributteSpeed * _levels;
        this.limitSwordsmen -= this.AtributteLimit * _levels;
    }
}
