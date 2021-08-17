using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Troop
{
    [SerializeField] List<UnitCombat> unitCombats = new List<UnitCombat>();
    [SerializeField] List<int> positions = new List<int>();

    public Troop()
    {

    }
    public Troop(int a, int b , int c)
    {
        AddElement("Swordsman", 0, a);
        AddElement("Lancer", 1, b);
        AddElement("Axeman", 2, c);
    }
    public Troop(List<UnitCombat> unitCombats)
    {
        for (int i = 0; i < unitCombats.Count; i++)
        {
            AddElement(unitCombats[i].GetType().ToString(), i, unitCombats[i].Quantity);
        }
    }
    public void LogTroop()
    {
        for (int i = 0; i < unitCombats.Count; i++)
        {
            Debug.Log("uc-" + unitCombats[i].CharacterName);
            Debug.Log("uc-" + positions[i]);
        }
    }
    public List<UnitCombat> UnitCombats
    {
          get { return unitCombats; }
    }

    public List<int> Positions
    {
        get { return positions; }
    }
    
    public void AddElement(string _type, int _position, int _number)
    {
        if (_number>0)
        {
            var unitCombat = GetNewUnitCombat(_type);
            unitCombat.Quantity = _number;
            unitCombats.Add(unitCombat);
            positions.Add(_position);
        }
    }
    public void MoveUnits(Territory territory)
    {
        for (int i = 0; i < unitCombats.Count; i++)
        {
            territory.GetUnit(unitCombats[i].GetType().ToString()).Quantity -= unitCombats[i].Quantity;
        }
    }
    public void AddMoreWarriors(Troop _troop)
    {
        for (int i = 0; i < unitCombats.Count; i++)
        {
            for (int j = 0; j < _troop.UnitCombats.Count; j++)
            {
                if (_troop.UnitCombats[j].GetType().ToString() == unitCombats[i].GetType().ToString())
                {
                    unitCombats[i].Quantity += _troop.UnitCombats[j].Quantity;
                }
            }
        }
    }

    public void AddElement(UnitCombat _unit, int _position)
    {
        if (_unit.Quantity>0)
        {
            unitCombats.Add(_unit);
            positions.Add(_position);
        }
    }
    public void DeleteElement(int _positionInArray)
    {
        unitCombats.Remove(unitCombats[_positionInArray]);
        positions.Remove(positions[_positionInArray]);
    }
    public UnitCombat GetNewUnitCombat(string _type)
    {
        switch (_type)
        {
            case "Swordsman":
                return new Swordsman();
            case "Lancer":
                return new Lancer();
            case "Axeman":
                return new Axeman();
            case "Archer":
                return new Archer();
            case "Scout":
                return new Scout();
            default:
                return null;
        }
    }
    public void Reset()
    {
        unitCombats.Clear();
        positions.Clear();
    }
    public int GetAllNumbersUnit()
    {
        int result = 0;
        for (int i = 0; i < unitCombats.Count; i++)
        {
            result += unitCombats[i].Quantity;
        }
        return result;
    }
}
