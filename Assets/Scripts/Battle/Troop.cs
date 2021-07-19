using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Troop
{
  //  [SerializeField] List<string> types= new List<string>();
    [SerializeField] List<UnitCombat> unitCombats = new List<UnitCombat>();
    [SerializeField] List<int> positions = new List<int>();
  //  [SerializeField] List<int> numbers = new List<int>();

    public Troop()
    {

    }
    public Troop(int a, int b , int c)
    {
        AddElement("Swordsman", 0, a);
        AddElement("Lancer", 1, b);
        AddElement("Axeman", 2, c);
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
            //types.Add(_type);
            positions.Add(_position);
           // numbers.Add(_number);
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
    public void Reset()
    {
        unitCombats.Clear();
      //  types.Clear();
        positions.Clear();
      //  numbers.Clear();
    }
    public int GetAllNumbersUnit()
    {
        int result = 0;
        /*
        for (int i = 0; i < numbers.Count; i++)
        {
            result += numbers[i];
        }
        */
        for (int i = 0; i < unitCombats.Count; i++)
        {
            result += unitCombats[i].Quantity;
        }
        return result;
    }
    /*
    public int GetNumberUnity(string _type)
    {
        int result = 0;
        for (int i = 0; i < numbers.Count; i++)
        {
            if (types[i] == _type)
            {
                result += numbers[i];
            }
        }
        return result;
    }
    
    public int GetNumberUnity(UnitCombat _type)
    {
        int result = 0;
        for (int i = 0; i < numbers.Count; i++)
        {
            if (unitCombats[i] == _type)
            {
                result += numbers[i];
            }
        }
        return result;
    }
    */
}
