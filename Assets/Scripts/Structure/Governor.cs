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
}
