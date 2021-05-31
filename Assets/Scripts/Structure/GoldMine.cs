using UnityEngine;
[System.Serializable]
public class GoldMine:Building
{
    //  private float velocityGold = 0;
    private int workersMine = 0;
    public GoldMine()
    {
        this.Name = "Gold Mine";
        this.AtributteToAdd = 5f;
    }
    public int WorkersMine
    {
      //  get { return velocityGold; }
      //  set { velocityGold = value; }
        get { return workersMine; }
        set { workersMine = value; }
    }
    public override void ImproveBuilding(int _levels)
    {
        base.ImproveBuilding(_levels);
        this.workersMine += (int)this.AtributteToAdd * _levels;
    }
    public override void SubsideBuilding(int _levels)
    {
        base.SubsideBuilding(_levels);
        this.workersMine -= (int)this.AtributteToAdd * _levels;
    }
}
