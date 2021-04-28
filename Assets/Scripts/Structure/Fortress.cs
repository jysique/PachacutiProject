using UnityEngine;
[System.Serializable]
public class Fortress: Building
{
    [SerializeField] private int plusDefense;

    public int PlusDefense
    {
        get { return plusDefense; }
        set { plusDefense = value; }
    }
    public override void ImproveBuilding()
    {
        base.ImproveBuilding();
        this.plusDefense += 1;
    }
}
