using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Lancer:UnitCombat
{
    public Lancer()
    {
        this.CharacterName = "Lance";
        this.UnitName = "Lancers";
        this.PlusDamage = 4;
    }
}
