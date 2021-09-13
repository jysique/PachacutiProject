using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Archer: UnitCombat
{
    public Archer()
    {
        this.CharacterName = "Archer";
        this.UnitName = "Archer";
        this.CharacIconType = "warriors";
        this.CharacIconIndex = "archer";
        this.Attack = 10;
        this.Defense = 3;
        this.Evasion = 0;
        this.Precision = 70;
        this.Range = 2;
        this.StrongTo = new string[] { "Scout" };
        this.WeakTo = new string[] { "Archer" };
        //this.Picture = Resources.Load<Sprite>("Textures/TemporalAssets/warriors/archer");
    }
}
