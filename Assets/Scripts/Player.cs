using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Player
{
    [SerializeField] string name;
    [SerializeField] Territory.TYPEPLAYER type;
    [SerializeField] int gold;
    [SerializeField] int food;
    [SerializeField] int motivation;
    [SerializeField] int opinion;
    [SerializeField] MilitarChief militar;
    [SerializeField] List<string> troop;

    public List<string> Troop
    {
        get { return troop; }
        set { troop = value; }
    }
}