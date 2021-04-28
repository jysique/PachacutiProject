using UnityEngine;
[System.Serializable]
public class Fortress: Building
{
    [SerializeField] private float plusDefense;

    public float PlusDefense
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
