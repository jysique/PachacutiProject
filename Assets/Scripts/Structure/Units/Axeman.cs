using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Axeman : UnitCombat
{
    public Axeman()
    {
        this.CharacterName = "Axeman";
        this.UnitName = "Axeman";
        this.CharacIconType = "warriors";
        this.CharacIconIndex = "axe";
        this.Attack = 20;
        this.Defense = 60;
        this.Precision = 75;
        this.Range = 1;
        this.StrongTo = new string[] { "Lancer" };
        this.WeakTo = new string[] { "Swordsman" };
      //  this.Picture = Resources.Load<Sprite>("Textures/TemporalAssets/warriors/axe");
    }
}
