using UnityEngine;
[System.Serializable]
public class Academy: Building
{
    //private float motivation = 0f;
    [SerializeField] private float speedSwordsmen = 0f;
    [SerializeField] private int limitSwordsmen ;

    public Academy()
    {
        this.Name = "Academy";
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
