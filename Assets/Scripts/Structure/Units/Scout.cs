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
        this.CharacIconType = "warriors";
        this.CharacIconIndex = "horseman";
        this.Attack = 25;
        this.Defense = 60;
        this.Precision = 70;
        this.Range = 1;
        this.StrongTo = new string[] { "Archer" };
        this.WeakTo = new string[] { "Scout" };
        //this.Picture = Resources.Load<Sprite>("Textures/TemporalAssets/warriors/horseman");
    }
}
