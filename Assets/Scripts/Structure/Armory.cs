using UnityEngine;
[System.Serializable]
public class Armory : Building
{
    private int plusAttack = 0;
    public Armory()
    {
        this.Name = "Armory";
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
    public override void SubsideBuilding(int _levels)
    {
        base.SubsideBuilding(_levels);
        this.plusAttack -= (int)this.AtributteToAdd * _levels;
    }
}
