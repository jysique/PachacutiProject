using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class Building
{
    [SerializeField]private int costToUpgrade;
    [SerializeField]private int level;
    [SerializeField]private float timeToBuild;
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

    public virtual void ImproveBuilding()
    {
      //  Debug.LogWarning("improveBuilding");
        this.level += 1;
        this.timeToBuild += 1;
    }
}
