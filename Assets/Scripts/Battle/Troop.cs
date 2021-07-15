using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Troop
{
    [SerializeField] List<string> types= new List<string>();
    [SerializeField] List<int> positions = new List<int>();
    [SerializeField] List<int> numbers = new List<int>();

    public List<string> Types
    {
        get { return types; }
    }
    public List<int> Positions
    {
        get { return positions; }
    }
    public List<int> Numbers
    {
        get { return numbers; }
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
}
