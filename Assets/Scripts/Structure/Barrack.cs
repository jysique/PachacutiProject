using UnityEngine;
[System.Serializable]
public class Barrack : Building
{
    private int plusAttack = 0;
    public Barrack()
    {
        this.Name = "Barrack";
        this.AtributteToAdd = 1.0f;
    }
    public int PlusAttack
    {
        get { return plusAttack; }
        set { plusAttack = value; }
    }
    public override void ImproveBuilding(int _levels)
    {
        base.ImproveBuilding(_levels);
        this.plusAttack += (int)this.AtributteToAdd * _levels;
    }
}
