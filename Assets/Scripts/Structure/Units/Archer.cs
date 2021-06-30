using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Archer:UnitCombat
{
    public Archer()
    {
        this.CharacterName = "Archers";
        this.UnitName = "Archers";
        this.PlusDamage = 1;
    }
}
