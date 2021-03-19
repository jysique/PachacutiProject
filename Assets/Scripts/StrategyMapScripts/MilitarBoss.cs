using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MilitarBoss 
{
    [SerializeField] private string name;
    [SerializeField] private Sprite picture;
    //stats
    [SerializeField] private int experience;
    [SerializeField] private int strategy;
    [SerializeField] private int military;


    public string Name
    {
        get { return name; }
        set { name = value; }
    }

    public int Experience
    {
        get { return experience; }
        set { experience = value; }
    }

    public int Strategy
    {
        get { return strategy; }
        set { strategy = value; }
    }

    public int Military
    {
        get { return military; }
        set { military = value; }
    }

    public Sprite Picture
    {
        get { return picture; }
        set { picture = value; }
    }



}
