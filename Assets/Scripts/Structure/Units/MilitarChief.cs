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
        SIEGE_EXPERT,
        BRAVE,
        ORGANIZER
    }
    public MilitarChief()
    {
        this.Description = "The military chief strongly influences the strength of your warriors when attacking other territories.";
        this.CharacIconType = "Military";
    }
    public void GetMilitarBoss()
    {
        System.Random rnd = new System.Random(DateTime.Now.Second);
        this.CharacterName = GetRandomName();
        this.Age = UnityEngine.Random.Range(20, 30);
        this.Origin = "Qosqo";
        this.Campaign = "DEFAULT_CAMPAIGN";
        this.Personality = "DEFAULT_PERSONALITY";
        this.CharacIconIndex = "0" + UnityEngine.Random.Range(1, 3).ToString();
        this.Influence = UnityEngine.Random.Range(3, 10);
        this.Opinion = UnityEngine.Random.Range(3, 10);
        this.Experience = UnityEngine.Random.Range(3, 10);
        TYPESTRAT _t = (TYPESTRAT)UnityEngine.Random.Range(0, 6);
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
                this.abilityName = "Tramp on";
                break;
            case "DEFENSIVE":
                this.abilityName = "Intimidation"; //Intimidation
                break;
            case "ORGANIZER":
                this.abilityName = "Juxtapose"; //Yuxtaponer
                break;
            case "SIEGE_EXPERT":
                this.abilityName = "Unit Changue";  //cambio de unidad
                break;
            case "BRAVE":
                this.abilityName = "Disable"; // cancelacion
                break;
            case "SACRED_WARRIOR":
                this.abilityName = "Guardian Angel"; // inmunidad
                break;
            default:
                break;
        }
    }
    public bool CheckCanSpecialAbility()
    {
        switch (strategyType)
        {
            case "ORGANIZER":
                return (GetFirstPositionFree() != -1);
            case "SACRED_WARRIOR":
            case "AGGRESSIVE":
            case "TERRAIN_MASTER":
            case "DEFENSIVE":
            case "SIEGE_EXPERT":
            case "BRAVE":
                return true;
            default:
                return false;
        }
    }
    public void SpecialAbility(Territory.TYPEPLAYER attacker_type, Territory.TYPEPLAYER deffender_type)
    {
        List<UnitGroup> ug = new List<UnitGroup>();
        UnitGroup dug = null;
        UnitCombat duc = null;
        int pos = 0;

        //        Debug.LogError("special ability");
        switch (strategyType)
        {
            case "AGGRESSIVE":
                for (int i = 0; i < 8; i++)
                {
                    SquareType _square = CombatManager.instance.Squares.transform.GetChild(i).gameObject.GetComponent<SquareType>();
                    UnitGroup _ug = _square.unitGroup;
                    if (_ug != null && _ug.TypePlayer == attacker_type)
                    {
                        ug.Add(_ug);
                    }
                }
                for (int i = 0; i < ug.Count; i++)
                {
                    CombatManager.instance.AnimationInBattle(ug[i].UnitsGO, "powerUp");
                    ug[i].UnitCombat.Attack += 10;
                }
                break;
            case "TERRAIN_MASTER":
                for (int i = 0; i < 8; i++)
                {
                    SquareType _square = CombatManager.instance.Squares.transform.GetChild(i).gameObject.GetComponent<SquareType>();
                    UnitGroup _ug = _square.unitGroup;
                    if (_ug != null && _ug.TypePlayer == deffender_type)
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
                    if (_ug != null && _ug.TypePlayer == deffender_type)
                    {
                        ug.Add(_ug);
                    }
                }
                for (int i = 0; i < ug.Count; i++)
                {
                    CombatManager.instance.AnimationInBattle(ug[i].UnitsGO, "powerDown");
                    ug[i].UnitCombat.Attack -= 10;
                }
                break;
            case "ORGANIZER":
                for (int i = 0; i < 8; i++)
                {
                    SquareType _square = CombatManager.instance.Squares.transform.GetChild(i).gameObject.GetComponent<SquareType>();
                    UnitGroup _ug = _square.unitGroup;
                    if (_ug != null && _ug.TypePlayer == attacker_type)
                    {
                        ug.Add(_ug);
                        break;
                    }
                }
                dug = ug[0];
                int a = dug.UnitCombat.Quantity;
                UnitCombat new_uc = Utils.instance.GetNewUnitCombat(dug.UnitCombat.UnitName);
                new_uc.Quantity = a / 4;
                pos = GetFirstPositionFree();
                // CombatManager.instance.AnimationInBattle(CombatManager.instance.Squares.transform.GetChild(pos).gameObject, "platform");
                CombatManager.instance.InstantiateUnitPlayer(pos, new_uc);
                break;
            case "SIEGE_EXPERT":
                for (int i = 0; i < 8; i++)
                {
                    SquareType _square = CombatManager.instance.Squares.transform.GetChild(i).gameObject.GetComponent<SquareType>();
                    UnitGroup _ug = _square.unitGroup;
                    if (_ug != null && _ug.TypePlayer == attacker_type)
                    {
                        pos = i;
                        ug.Add(_ug);
                        break;
                    }
                }
                dug = ug[0];
                string s = GetUnitNameRandom(dug.UnitCombat.UnitName);
                duc = Utils.instance.GetNewUnitCombat(s);
                duc.Quantity = dug.UnitCombat.Quantity;
                CombatManager.instance.InstantiateUnitPlayer(CombatManager.instance.Squares.transform.GetChild(pos).gameObject, duc);
                break;
            case "BRAVE":

                for (int i = 0; i < 8; i++)
                {
                    SquareType _square = CombatManager.instance.Squares.transform.GetChild(i).gameObject.GetComponent<SquareType>();
                    UnitGroup _ug = _square.unitGroup;
                    if (_ug != null && _ug.TypePlayer == deffender_type)
                    {
                        ug.Add(_ug);
                    }
                }
                for (int i = 0; i < ug.Count; i++)
                {
                    CombatManager.instance.AnimationInBattle(ug[i].UnitsGO, "cantTurn");
                }
                if (attacker_type == Territory.TYPEPLAYER.PLAYER)
                {
                    CombatManager.instance.CanBotTurn = false;
                }
                else
                {
                    CombatManager.instance.CanPlayerTurn = false;
                }

                break;
            case "SACRED_WARRIOR":
                for (int i = 0; i < 8; i++)
                {
                    SquareType _square = CombatManager.instance.Squares.transform.GetChild(i).gameObject.GetComponent<SquareType>();
                    UnitGroup _ug = _square.unitGroup;
                    if (_ug != null && _ug.TypePlayer == attacker_type)
                    {
                        ug.Add(_ug);
                    }
                }
                for (int i = 0; i < ug.Count; i++)
                {
                    ug[i].Inmunity = true;
                    CombatManager.instance.AnimationInBattle(ug[i].UnitsGO, "inmunity");
                }
                break;
            default:
                break;
        }
        
        CombatManager.instance.UpdateBuffTerrain();
        for (int i = 0; i < ug.Count; i++)
        {
            CombatManager.instance.UpdateText(ug[i]);
            CombatManager.instance.UpdateUnitGroup(ug[i]);
        }
        if (CombatManager.instance.EndBattle())
        {
            CombatManager.instance.OpenResumeBattle();
        }

    }
    private int GetFirstPositionFree()
    {
        for (int i = 0; i < 8; i++)
        {
            int pos = i;
            SquareType _square = CombatManager.instance.Squares.transform.GetChild(i).gameObject.GetComponent<SquareType>();
            UnitGroup _ug = _square.unitGroup;
            if (_ug == null)
            {
                return pos;
            }
        }
        return -1;
    }
    private string GetUnitNameRandom(string original)
    {
        string f = "";
        List<string> units = Utils.instance.Units_string;
        int random = UnityEngine.Random.Range(0, units.Count);
        if (units[random] == original)
        {
            f = GetUnitNameRandom(original);
        }
        else
        {
            f = units[random];
        }
        return f;
    }
    /*
    AGGRESSIVE - grito de guerra + daño player
    TERRAIN_MASTER - trampa
    DEFENSIVE - intimidacion - reduce el daño del enemigo
    SACRED_WARRIOR - 1 turno mas
    SIEGE_EXPERT - 
     */
}