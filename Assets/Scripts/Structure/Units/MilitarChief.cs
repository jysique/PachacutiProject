using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MilitarChief : Subordinate
{

    [SerializeField] private TYPESTRAT strategyType;
    private string abilityName;


    public TYPESTRAT StrategyType
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
    }
    public MilitarChief(string characterName, int age , string origin, int index, int influence, int opinion, int experience, int typeStrat)
    {
        this.characterName = characterName;
        this.age = age;
        this.origin = origin;
        this.pathPicture = "Military/0" + index.ToString();
        this.influence = influence;
        this.opinion = opinion;
        this.experience = experience;
        this.strategyType = (TYPESTRAT)typeStrat;
        GetSpecialAbility();
    }
    public void GetSpecialAbility()
    {
        switch (strategyType)
        {
            case TYPESTRAT.AGGRESSIVE:
                this.abilityName = "Power up";
                break;
            case TYPESTRAT.TERRAIN_MASTER:
                this.abilityName = "Tramp on";
                break;
            case TYPESTRAT.DEFENSIVE:
                this.abilityName = "Intimidation"; //Intimidation
                break;
            case TYPESTRAT.SACRED_WARRIOR:
                this.abilityName = "Guardian Angel"; // inmunidad
                break;
            case TYPESTRAT.SIEGE_EXPERT:
                this.abilityName = "Unit Changue";  //cambio de unidad
                break;
            case TYPESTRAT.BRAVE:
                this.abilityName = "Disable"; // cancelacion
                break;
            case TYPESTRAT.ORGANIZER:
                this.abilityName = "Juxtapose"; //Yuxtaponer
                break;
            default:
                break;
        }
    }
    public bool CheckCanSpecialAbility()
    {
        switch (strategyType)
        {
            case TYPESTRAT.ORGANIZER:
                return (GetFirstPositionFree() != -1);
            case TYPESTRAT.AGGRESSIVE:
            case TYPESTRAT.TERRAIN_MASTER:
            case TYPESTRAT.DEFENSIVE:
            case TYPESTRAT.SACRED_WARRIOR:
            case TYPESTRAT.SIEGE_EXPERT:
            case TYPESTRAT.BRAVE:
                return true;
            default:
                return false;
        }
    }
    public void SpecialAbility(Territory attacker, Territory deffender)
    {
        List<UnitGroup> ug = new List<UnitGroup>();
        UnitGroup dug = null;
        UnitCombat duc = null;
        int pos = 0;
        Territory.TYPEPLAYER attacker_type = attacker.TypePlayer;
        Territory.TYPEPLAYER deffender_type = deffender.TypePlayer;
        //        Debug.LogError("special ability");
        switch (strategyType)
        {
            case TYPESTRAT.AGGRESSIVE:
                for (int i = 0; i < 8; i++)
                {
                    SquareType _square = CombatManager.instance.Squares.transform.GetChild(i).gameObject.GetComponent<SquareType>();
                    UnitGroup _ug = _square.UnitGroup;
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
            case TYPESTRAT.TERRAIN_MASTER:
                for (int i = 0; i < 8; i++)
                {
                    SquareType _square = CombatManager.instance.Squares.transform.GetChild(i).gameObject.GetComponent<SquareType>();
                    UnitGroup _ug = _square.UnitGroup;
                    if (_ug != null && _ug.TypePlayer == deffender_type)
                    {
                        ug.Add(_ug);
                        break;
                    }
                }
                //cambiar por uno seleccionable
                ug[0].UnitCombat.Quantity -= 10;
                if (ug[0].UnitCombat.Quantity < 0)
                {
                    ug[0].UnitCombat.Quantity = 0;
                }
                CombatManager.instance.AnimationInBattle(ug[0].UnitsGO, "rock");
                break;
            case TYPESTRAT.DEFENSIVE:
                for (int i = 0; i < 8; i++)
                {
                    SquareType _square = CombatManager.instance.Squares.transform.GetChild(i).gameObject.GetComponent<SquareType>();
                    UnitGroup _ug = _square.UnitGroup;
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
            case TYPESTRAT.SACRED_WARRIOR:
                for (int i = 0; i < 8; i++)
                {
                    SquareType _square = CombatManager.instance.Squares.transform.GetChild(i).gameObject.GetComponent<SquareType>();
                    UnitGroup _ug = _square.UnitGroup;
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
            case TYPESTRAT.SIEGE_EXPERT:
                for (int i = 0; i < 8; i++)
                {
                    SquareType _square = CombatManager.instance.Squares.transform.GetChild(i).gameObject.GetComponent<SquareType>();
                    UnitGroup _ug = _square.UnitGroup;
                    if (_ug != null && _ug.TypePlayer == attacker_type)
                    {
                        pos = i;
                        ug.Add(_ug);
                        break;
                    }
                }
                dug = ug[0];
                // string s = GetUnitNameRandom(dug.UnitCombat.CharacterName, attacker);
                // duc = Utils.instance.CreateNewUnitCombat(s, dug.UnitCombat.Quantity);
                duc = GetUnitCombatRandom(dug.UnitCombat,attacker);
                CombatManager.instance.InstantiateUnitPlayer(CombatManager.instance.Squares.transform.GetChild(pos).gameObject, duc);
                break;
            case TYPESTRAT.BRAVE:
                for (int i = 0; i < 8; i++)
                {
                    SquareType _square = CombatManager.instance.Squares.transform.GetChild(i).gameObject.GetComponent<SquareType>();
                    UnitGroup _ug = _square.UnitGroup;
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
            case TYPESTRAT.ORGANIZER:
                for (int i = 0; i < 8; i++)
                {
                    SquareType _square = CombatManager.instance.Squares.transform.GetChild(i).gameObject.GetComponent<SquareType>();
                    UnitGroup _ug = _square.UnitGroup;
                    if (_ug != null && _ug.TypePlayer == attacker_type)
                    {
                        ug.Add(_ug);
                        break;
                    }
                }
                dug = ug[0];
                int _a = dug.UnitCombat.Quantity;
                string _name= dug.UnitCombat.UnitType + "new";
                UnitCombat new_uc = Utils.instance.CreateNewUnitCombat(_name, dug.UnitCombat.UnitType,_a/4);
                pos = GetFirstPositionFree();
                CombatManager.instance.InstantiateUnitPlayer(pos, new_uc);
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
            UnitGroup _ug = _square.UnitGroup;
            if (_ug == null)
            {
                return pos;
            }
        }
        return -1;
    }

    private UnitCombat GetUnitCombatRandom(UnitCombat original, Territory territory)
    {
        List<string> units = Utils.instance.Units_string;
        int random = UnityEngine.Random.Range(0, units.Count);
        UnitCombat unit;
        string _name = units[random] + "new";
        UnitCombat temporalUnit = Utils.instance.CreateNewUnitCombat(_name,units[random], original.Quantity);
        
        if (units[random] == original.UnitType || territory.GetBuildingByUnit(temporalUnit).Level < 1)
        {
            unit = GetUnitCombatRandom(original, territory);
        }
        else
        {
            unit = temporalUnit;
        }
        return unit;
    }
}