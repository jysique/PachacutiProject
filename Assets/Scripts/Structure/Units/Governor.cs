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
    public TimeSimulated TimeInit
    {
        get { return timeInit; }
        set { timeInit = value; }
    }
    public Governor(string _name, int day,int month,int year)
    {
        this.CharacterName = _name;
        this.Age = Random.Range(20, 30);
        this.Origin = "Qosqo";
        this.pathPicture = "Governor/0" + Random.Range(1, 4).ToString();
        this.Diplomacy = Random.Range(3,10);
        this.Militancy = Random.Range(3, 10);
        this.Managment = Random.Range(3, 10);
        this.Prestige = Random.Range(3, 10);
        this.Piety = Random.Range(3, 10);
        this.Culture = "Default";
        this.timeInit = new TimeSimulated(day, month, year);
    }
}
