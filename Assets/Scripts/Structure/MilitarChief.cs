using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MilitarChief : Subordinate
{

    [SerializeField] private int experience;
    [SerializeField] private string strategyType;

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
        AGGRESSIVE,
        TERRAIN_MASTER,
        DEFENSIVE,
        SACRED_WARRIOR,
        SIEGE_EXPERT
    }
    public MilitarChief()
    {
        this.Description = "The military chief strongly influences the strength of your warriors when attacking other territories.";
    }
    public void GetMilitarBoss()
    {
        System.Random rnd = new System.Random(DateTime.Now.Second);
        this.CharacterName = GetRandomName();
        this.Age = UnityEngine.Random.Range(20, 30);
        this.Origin = "Qosqo";
        this.Campaign = "DEFAULT_CAMPAIGN";
        this.Personality = "DEFAULT_PERSONALITY";
        this.CharacIconType = "Military";
        this.CharacIconIndex = "0" + UnityEngine.Random.Range(1, 3).ToString();
        this.Influence = UnityEngine.Random.Range(3, 10);
        this.Opinion = UnityEngine.Random.Range(3, 10);
        this.Experience = UnityEngine.Random.Range(3, 10);
        TYPESTRAT _t = (TYPESTRAT)UnityEngine.Random.Range(0, 5);
        this.StrategyType = _t.ToString();
    }

    private string[] namesList = new string[] { "Unay", "Asiri", "Samin ","Sayri",
                                                "Haylli","Tupac", "Raymi","Wara", 
                                                "Qori","Yaku" };
    private string[] lastNamesList= new string[] { "","Illa", "Inka", "Wari", "Amaru",
                                                 "Amaru","Cancha","Ccasani"};

    public string GetRandomName()
    {
        
        string final_name = "";
        int a = UnityEngine.Random.Range(0, namesList.Length);
        final_name += namesList[a];
        final_name += " ";
        int b = UnityEngine.Random.Range(0, lastNamesList.Length);
        final_name += lastNamesList[b];
        return final_name;
    }
}
