using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Scout : UnitCombat
{
    public Scout()
    {
        this.CharacterName = "Scout";
        this.UnitName = "Scout";
        this.PlusDamage = 4;
    }
}
