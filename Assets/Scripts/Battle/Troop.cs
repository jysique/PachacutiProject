using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class Troop
{
    [SerializeField] List<UnitCombat> unitCombats = new List<UnitCombat>();
   
    public List<UnitCombat> UnitCombats
    {
        get { return unitCombats; }
    }
    public Troop()
    {

    }
    public void LogTroop()
    {
        for (int i = 0; i < unitCombats.Count; i++)
        {
            Debug.Log("uc-" + unitCombats[i].CharacterName);
        }
    }


    public void SaveTroop(Troop _troop)
    {
        for (int i = 0; i < _troop.UnitCombats.Count; i++)
        {
            AddUnitCombat(_troop.UnitCombats[i]);
        }
    }
    public void MoveUnits(Territory territory)
    {
        for (int i = 0; i < unitCombats.Count; i++)
        {
            //Debug.Log("borrando "+ unitCombats[i].UnitName + " en " + territory.name);
            territory.ListUnitCombat.DeleteUnitCombat(unitCombats[i]);
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
    public int GetAllNumbersUnit()
    {
        int result = 0;
        for (int i = 0; i < unitCombats.Count; i++)
        {
            result += unitCombats[i].Quantity;
        }
        return result;
    }

    public void AddUnitCombat(UnitCombat a)
    {
        unitCombats.Add(a);
    }
    public void DeleteUnitCombat(UnitCombat a)
    {
        unitCombats.Remove(a);
    }
    public void AddUnitCombat(string _type,int _in_progress, int _quantity)
    {
        UnitCombat a = Utils.instance.GetNewUnitCombat(_type);
        a.InProgress = _in_progress;
        a.Quantity = _quantity;
        if (a.InProgress == a.Quantity)
            a.IsAvailable = true;
        else
            a.IsAvailable = false;
        unitCombats.Add(a);
    }
    public void Clear()
    {
        unitCombats.Clear();
    }
    public int GetPopulation()
    {
        int sum = 0;
        for (int i = 0; i < unitCombats.Count; i++)
        {
            if (unitCombats[i].IsAvailable)
            {
                sum += unitCombats[i].Quantity;
            }
        }
        return sum;
    }
    
    public bool SearchUnitCombat(string _type)
    {
        return unitCombats.Any(x => x.UnitName == _type);
    }

    public int GetUnitQuantity(string _type)
    {
        if (!SearchUnitCombat(_type))
        {
           // Debug.Log("No se encontro tipo " + _type);
            return 0;
        }
        else
        {
            return unitCombats.FindAll(x => (x.UnitName == _type)).Select(x => x.Quantity).Sum();
        }
    }
    public UnitCombat GetFirstUnitCombat(string _type)
    {
        if (GetUnitsListByType(_type) == null)
        {
            Debug.Log("no se encontro unidades " + _type);
            return null;
        }
        else
        {
            return GetUnitsListByType(_type)[0];
        }
    }


    public UnitCombat GetUnitCombat(string _type, int _index)
    {
        if (_index >= unitCombats.Count)
        {
            Debug.Log("no se encontro el index " + _index);
            return null;
        }
        else
        {
            return GetUnitsListByType(_type)[_index];
        }

    }

    public List<UnitCombat> GetUnitsListByType(string _type)
    {
        if (!SearchUnitCombat(_type))
        {
            Debug.Log("No se encontro tipo " + _type);
            return null;
        }
        else
        {
            return unitCombats.FindAll(x => (x.UnitName == _type) && (x.IsAvailable==true)).ToList();
        }
    }

    public List<UnitCombat> GetUnitListByBuild(string _building)
    {
        switch (_building)
        {
            case "Academy":
                return GetUnitsListByType("Swordsman");
            case "Archery":
                return GetUnitsListByType("Archer");
            case "Barracks":
                return GetUnitsListByType("Lancer");
            case "Castle":
                return GetUnitsListByType("Axeman");
            case "Stable":
                return GetUnitsListByType("Scout");
            default:
                Debug.LogError("No se encontro edificio");
                return null;
        }
    }

    //EVENTS
    public void ReduceQuantity(string _type)
    {
        if (!SearchUnitCombat(_type))
        {
            Debug.Log("no se encontro type " + _type);
        }
        else
        {
            List<UnitCombat> _units = GetUnitsListByType(_type);
            for (int i = 0; i < _units.Count; i++)
            {
                _units[i].Quantity /= 2;
            }
        }
    }
    public void ReduceAllQuantity()
    {
        ReduceQuantity("Swordsman");
        ReduceQuantity("Lancer");
        ReduceQuantity("Archer");
        ReduceQuantity("Axemen");
        ReduceQuantity("Scouts");
    }



}
