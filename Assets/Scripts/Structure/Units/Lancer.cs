using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Lancer:UnitCombat
{
    public Lancer()
    {
        this.CharacterName = "Lancer";
        this.UnitName = "Lancer";
        this.CharacIconType = "warriors";
        this.CharacIconIndex = "spear";
        this.Attack = 15;
        this.Precision = 85;
        this.Range = 1;
        this.StrongTo = new string[] { "Swordsman" };
        this.WeakTo = new string[] { "Axeman" };
      //  this.Picture = Resources.Load<Sprite>("Textures/TemporalAssets/warriors/spear");
    }
}
