using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MilitarChief : Subordinate
{

    [SerializeField] private int experience;
    [SerializeField] private string strategyType;
    private string abilityName;
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
    public string AbilityName
    {
        get { return abilityName; }
        set { abilityName = value; }
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
        this.CharacIconType = "Military";
        //this.Description = GameMultiLang.GetTraduction("MILITARYDESCRIPTION");
    }
    public void GetMilitarBoss()
    {
        System.Random rnd = new System.Random(DateTime.Now.Second);
        this.CharacterName = GetRandomName();
        this.Age = UnityEngine.Random.Range(20, 30);
        this.Origin = "Qosqo";
        this.Campaign = "DEFAULT_CAMPAIGN";
        this.Personality = "DEFAULT_PERSONALITY";
//        Debug.LogError("GET|"+this.CharacIconType);
        this.CharacIconIndex = "0" + UnityEngine.Random.Range(1, 3).ToString();
        this.Influence = UnityEngine.Random.Range(3, 10);
        this.Opinion = UnityEngine.Random.Range(3, 10);
        this.Experience = UnityEngine.Random.Range(3, 10);
        TYPESTRAT _t = (TYPESTRAT)UnityEngine.Random.Range(0, 5);
        this.StrategyType = _t.ToString();
        GetSpecialAbility();
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
    public void GetSpecialAbility()
    {
        switch (strategyType)
        {
            case "AGGRESSIVE":
                this.abilityName = "Power up";
                break;
            case "TERRAIN_MASTER":
                this.abilityName = "Tramp!";
                break;
            case "DEFENSIVE":
                this.abilityName = "Intimidation";
                break;
            case "SACRED_WARRIOR":
                this.abilityName = "No define";
                break;
            case "SIEGE_EXPERT":
                this.abilityName = "No define";
                break;
            default:
                break;
        }
    }
    public void SpecialAbility(Territory.TYPEPLAYER _type)
    {
        List<UnitGroup> ug = new List<UnitGroup>();
//        Debug.LogError("special ability");
        switch (strategyType)
        {
            case "AGGRESSIVE":
                for (int i = 0; i < 8; i++)
                {
                    SquareType _square = CombatManager.instance.Squares.transform.GetChild(i).gameObject.GetComponent<SquareType>();
                    UnitGroup _ug = _square.unitGroup;
                    if (_ug != null && _ug.TypePlayer == Territory.TYPEPLAYER.PLAYER)
                    {
                        ug.Add(_ug);
                    }
                }
                for (int i = 0; i < ug.Count; i++)
                {
                    CombatManager.instance.AnimationInBattle(ug[i].UnitsGO, "powerUp");
                }
                break;
            case "TERRAIN_MASTER":
                for (int i = 0; i < 8; i++)
                {
                    SquareType _square = CombatManager.instance.Squares.transform.GetChild(i).gameObject.GetComponent<SquareType>();
                    UnitGroup _ug = _square.unitGroup;
                    if (_ug != null && _ug.TypePlayer == _type)
                    {
                        ug.Add(_ug);
                        break;
                    }
                }
                //cambiar por uno seleccionable
                ug[0].UnitCombat.Quantity -= 10;
                if (ug[0].UnitCombat.Quantity< 0)
                {
                    ug[0].UnitCombat.Quantity = 0;
                }
                CombatManager.instance.AnimationInBattle(ug[0].UnitsGO, "rock");
                break;
            case "DEFENSIVE":
                for (int i = 0; i < 8; i++)
                {
                    SquareType _square = CombatManager.instance.Squares.transform.GetChild(i).gameObject.GetComponent<SquareType>();
                    UnitGroup _ug = _square.unitGroup;
                    if (_ug != null && _ug.TypePlayer == _type)
                    {
                        ug.Add(_ug);
                    }
                }
                for (int i = 0; i < ug.Count; i++)
                {
                    CombatManager.instance.AnimationInBattle(ug[i].UnitsGO, "powerDown");
                }
                break;
            case "SACRED_WARRIOR":

                break;
            case "SIEGE_EXPERT":

                break;
            default:
                break;
        }
        for (int i = 0; i < ug.Count; i++)
        {
            CombatManager.instance.UpdateText(ug[i]);
        }
        
    }
    /*
    AGGRESSIVE - grito de guerra + daño player
    TERRAIN_MASTER - trampa
    DEFENSIVE - intimidacion - reduce el daño del enemigo
    SACRED_WARRIOR - 1 turno mas
    SIEGE_EXPERT - 
     */
}