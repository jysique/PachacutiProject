using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Building
{
    private string nameBuilding;
    private int costToUpgrade = 7;
    private int level = 0;
    private float timeToBuild = 4;
    public string Name
    {
        get { return nameBuilding; }
        set { nameBuilding = value; }
    }
    public int CostToUpgrade
    {
        get { return costToUpgrade; }
        set { costToUpgrade = value; }
    }
    public int Level
    {
        get { return level; }
        set { level = value; }
    }
    public float TimeToBuild
    {
        get { return timeToBuild; }
        set { timeToBuild = value; }
    }

    public virtual void ImproveBuilding(int _levels)
    {
      //  Debug.LogWarning("improveBuilding");
        this.level += _levels;
        this.timeToBuild += 1 * _levels;
    }
}
