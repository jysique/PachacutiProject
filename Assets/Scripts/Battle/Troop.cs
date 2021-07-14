using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Troop
{
    [SerializeField] List<string> types= new List<string>();
    [SerializeField] List<int> positions = new List<int>();
    [SerializeField] List<int> numbers = new List<int>();

    public Troop()
    {

    }
    public Troop(int a, int b , int c)
    {
        AddElement("Swordsman", 0, a);
        AddElement("Lancer", 1, b);
        AddElement("Axeman", 2, c);

    }
    public void AddElement(string _type, int _position, int _number)
    {
        types.Add(_type);
        positions.Add(_position);
        numbers.Add(_number);
    }
    public void Reset()
    {
        types.Clear();
        positions.Clear();
        numbers.Clear();
    }
    public int GetAllNumbersUnit()
    {
        int result = 0;
        for (int i = 0; i < numbers.Count; i++)
        {
            result += numbers[i];
        }
        return result;
    }
    public int GetNumberUnity(string _type)
    {
        int result = 0;
        for (int i = 0; i < numbers.Count; i++)
        {
            if (types[i] == _type) {
                result += numbers[i];
            }
        }
        return result;
    }
}
