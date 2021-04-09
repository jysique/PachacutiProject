using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MilitarBoss : Subordinate
{

    [SerializeField] private int experience;
    [SerializeField] private string strategyType;

    private TYPESTRAT type;
    public TYPESTRAT Type
    {
        //get { return (MilitarBoss.TYPESTRAT)System.Enum.Parse(typeof(MilitarBoss.TYPESTRAT), StrategyType); }
        get { return TYPESTRAT.DEFAULT; }
    }

    public int Experience
    {
        get { return experience; }
        set { experience = value; }
    }

    public string StrategyType
    {
        get { return strategyType; }
        set { strategyType = value; }
    }

    public enum TYPESTRAT
    {
        DEFAULT,
        AGGRESSIVE,
        TERRAIN_MASTER,
        DEFENSIVE,
        SACRED_WARRIOR,
        SIEGE_EXPERT
    }
    public void GetMilitarBoss()
    {
        System.Random rnd = new System.Random(DateTime.Now.Second);
        this.CharacterName = GetRandomName();
        this.Age = UnityEngine.Random.Range(20, 30);
        this.Origin = "Qosqo";
        this.Campaign = "Default";
        this.Personality = "Default";
        this.CharacIconType = "Military";
        this.CharacIconIndex = "0" + UnityEngine.Random.Range(1, 3).ToString();
        this.Influence = UnityEngine.Random.Range(3, 10);
        this.Opinion = UnityEngine.Random.Range(3, 10);
        this.Experience = UnityEngine.Random.Range(3, 10);
        TYPESTRAT _t = (TYPESTRAT)UnityEngine.Random.Range(0, 5);
        this.strategyType = _t.ToString();
    }


    private string[] names = new string[] { "Unay", "Asiri", "Samin ","Sayri" };
    private string[] lastNames= new string[] { "","Illa", "Inka", "Wari" };
    public string GetRandomName()
    {
        
        string final_name = "";
        int a = UnityEngine.Random.Range(0, names.Length);
        final_name += names[a];
        final_name += " ";
        int b = UnityEngine.Random.Range(0, lastNames.Length);
        final_name += lastNames[b];
        return final_name;
    }
}
