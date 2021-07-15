using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitType
{
    int attack;
    int presicion;
    int range;
    string unitName;
    string[] strongTo;
    string[] weakTo;
    Sprite picture;

    public UnitType(string _unitName, int _attack, int _presicion, int _range, string[] _strongTo, string[] _weakTo, Sprite _picture)
    {
        unitName = _unitName;
        attack = _attack;
        presicion = _presicion;
        range = _range;
        picture = _picture;
        strongTo = _strongTo;
        weakTo = _weakTo;
    }

    public int Attack{
        get { return attack; }
        }
    public int Presicion
    {
        get { return presicion; }
    }
    public int Range
    {
        get { return range; }
    }
    public Sprite Picture
    {
        get { return picture; }
    }

    public string[] StrongTo
    {
        get { return strongTo; }
    }
    public string[] WeakTo
    {
        get { return weakTo; }
    }





}
