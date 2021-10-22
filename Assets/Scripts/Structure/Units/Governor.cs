using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Governor : RoyalFamily
{
    private string culture;
    private List<string> misions;
    [SerializeField] private int dayPeriod;
    [SerializeField] private int monthPeriod;
    [SerializeField] private int yearPeriod;
    private TimeSimulated timeInit;
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
    public TimeSimulated TimeInit
    {
        get { return new TimeSimulated(dayPeriod, monthPeriod, yearPeriod); }
        set { timeInit = value; }
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
        this.Age = Random.Range(20, 30);
        this.Origin = "Qosqo";
        this.CharacIconType = "Governor";
        this.CharacIconIndex = "0" + Random.Range(1, 4).ToString();
        this.Diplomacy = Random.Range(3,10);
        this.Militancy = Random.Range(3, 10);
        this.Managment = Random.Range(3, 10);
        this.Prestige = Random.Range(3, 10);
        this.Piety = Random.Range(3, 10);
        this.Culture = "Default";
        this.misions = null;
    }
}
