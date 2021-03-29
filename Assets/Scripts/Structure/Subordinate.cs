using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Subordinate : Character
{
    [SerializeField] private string influence;
    [SerializeField] private int opinion;
    
    public string Influence
    {
        get { return influence; }
        set { influence = value; }
    }
    public int Opinion
    {
        get { return opinion; }
        set { opinion = value; }
    }
}
