using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MilitarBoss : Subordinate
{
    public enum TYPESTRAT
    {
        AGGRESSIVE,
        TERRAIN_MASTER,
        DEFENSIVE,
        SACRED_WARRIOR,
        SIEGE_EXPERT

    }
    private int experience;
    private TYPESTRAT strategyType;
    public int Experience
    {
        get { return experience; }
        set { experience = value; }
    }

    public TYPESTRAT StrategyType
    {
        get { return strategyType; }
        set { strategyType = value; }
    }

    public MilitarBoss(string _characName, string _characIconIndex, int _age, string _origin, int _exp, int _strat,string _influence, string _personality,int _opinion)
    {
        this.CharacIcon = "Military";
        this.Picture = Resources.Load<Sprite>("Textures/TemporalAssets/"+CharacIcon+"/"+_characIconIndex);
        this.CharacterName = _characName;
        this.Age = _age;
        this.Origin = _origin;
        this.Personality = _personality;
        this.Experience = _exp;
        
        this.Influence = _influence;
        this.Opinion = _opinion;
        this.CanElegible = true;
        this.CanMove = false;

        switch (_strat)
        {
            case 0:
                strategyType = TYPESTRAT.AGGRESSIVE;
                break;
            case 1:
                strategyType = TYPESTRAT.DEFENSIVE;
                break;
            case 2:
                strategyType = TYPESTRAT.SACRED_WARRIOR;
                break;
            case 3:
                strategyType = TYPESTRAT.SIEGE_EXPERT;
                break;
            case 4:
                strategyType = TYPESTRAT.TERRAIN_MASTER;
                break;
        }

    }
    /*
    [SerializeField] private string name;
    [SerializeField] private Sprite picture;
    //stats
    [SerializeField] private int experience;
    [SerializeField] private int strategy;
    [SerializeField] private int military;


    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int Experience
    {
        get { return experience; }
        set { experience = value; }
    }

    public int Strategy
    {
        get { return strategy; }
        set { strategy = value; }
    }

    public int Military
    {
        get { return military; }
        set { military = value; }
    }

    public Sprite Picture
    {
        get { return picture; }
        set { picture = value; }
    }
    */


}
