using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Governor : RoyalFamily
{
     private string culture;
     private List<string> misions;

    public string Culture
    {
        get { return culture; }
        set { culture = value; }
    }
    public List<string> Misions
    {
        get { return misions; }
        set { misions = value; }
    }
    public void AddMision(string value)
    {
        misions.Add(value);
    }
    public void DeleteMision(string value)
    {
        misions.Remove(value);
    }
    public Governor(string _name)
    {
        this.CharacterName = _name;
        this.Age = 30;
        this.Origin = "Qosqo";
        this.Campaign = "Default";
        this.Personality = "Default";
        this.CharacIconType = "Emperor";
        this.CharacIconIndex = "01";
        this.Diplomacy = 10;
        this.Militancy = 10;
        this.Managment = 10;
        this.Prestige = 10;
        this.Piety = 10;
        this.Culture = "Default";
        this.misions = null;
    }
}
