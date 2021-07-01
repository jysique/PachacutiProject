using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Swordsman:UnitCombat
{
    public Swordsman()
    {
        this.CharacterName = "Sword";
        this.UnitName = "Sword";
        this.PlusDamage = 3;
    }

}
