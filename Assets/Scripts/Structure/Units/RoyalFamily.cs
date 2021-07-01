using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoyalFamily : Character
{
    [SerializeField]private int diplomacy;
    [SerializeField]private int militancy;
    [SerializeField]private int managment;
    [SerializeField]private int prestige;
    [SerializeField]private int piety;

    public int Diplomacy
    {
        get {return diplomacy; }
        set { diplomacy = value; }
    }
    public int Militancy
    {
        get {return militancy; }
        set {militancy = value; }
    }
    public int Managment
    {
        get {return managment; }
        set {managment = value; }
    }
    public int Prestige
    {
        get {return prestige; }
        set {prestige = value; }
    }
    public int Piety
    {
        get {return piety; }
        set {piety = value; }
    }
}
