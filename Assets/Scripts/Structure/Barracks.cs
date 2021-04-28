using UnityEngine;
[System.Serializable]
public class Barracks : Building
{
    [SerializeField] private int plusAttack;

    public int PlusAttack
    {
        get { return plusAttack; }
        set { plusAttack = value; }
    }
    public override void ImproveBuilding()
    {
        base.ImproveBuilding();
        this.plusAttack += 1;
    }
}
