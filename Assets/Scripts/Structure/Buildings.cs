using UnityEngine;

[System.Serializable]
public class Buildings
{
    [SerializeField]private int costToUpgrade;
    [SerializeField]private int level;
    [SerializeField] private float timeToBuild;

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
}
