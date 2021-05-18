using UnityEngine;
[System.Serializable]
public class Barrack : Building
{
    private int plusAttack = 1;
    public Barrack()
    {
        this.Name = "Barrack";
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
