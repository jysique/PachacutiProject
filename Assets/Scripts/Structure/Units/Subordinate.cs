using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Subordinate : Character
{
    protected int influence;
    protected int opinion;
    
    public int Influence
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
