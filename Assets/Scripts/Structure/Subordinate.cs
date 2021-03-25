using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Subordinate : Character
{
    private string influence;
    private int opinion;
    private string characIcon;
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
    public string CharacIcon
    {
        get { return characIcon; }
        set { characIcon = value; }
    }
}
