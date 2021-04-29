using UnityEngine;
[System.Serializable]
public class Barracks : Building
{
    private int plusAttack = 1;
    public Barracks()
    {
        this.Name = "Barracks";
    }
    public int PlusAttack
    {
        get { return plusAttack; }
        set { plusAttack = value; }
    }
    public override void ImproveBuilding(int _levels)
    {
        base.ImproveBuilding(_levels);
        this.plusAttack += 1 * _levels;
    }
}
