using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Swordsman:UnitCombat
{
    public Swordsman()
    {
        this.CharacterName = "Swordsman";
        this.UnitName = "Swordsman";
        this.CharacIconType = "warriors";
        this.CharacIconIndex = "w1";
        this.Attack = 10;
        this.Precision = 95;
        this.Range = 1;
        this.StrongTo = new string[] { "Axeman" };
        this.WeakTo = new string[] { "Lancer" };
       // this.Picture = Resources.Load<Sprite>("Textures/TemporalAssets/warriors/w1");
    }
}
