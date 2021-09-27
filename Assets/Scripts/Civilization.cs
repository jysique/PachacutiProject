using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Civilization
{
    [SerializeField] string civilizationName;
    [SerializeField] string colorString;
    //private Sprite hat1;
    //private Sprite hat2;
    //private Color32 color;
    public string Name
    {
        get { return civilizationName; }
    }
    public Sprite Hat1
    {
        get { return Resources.Load<Sprite>("Textures/TemporalAssets/civ/" + civilizationName.ToLower() + "-hat-1"); }
    //    set { hat1 = value; }
    }
    public Sprite Hat2
    {
        get { return Resources.Load<Sprite>("Textures/TemporalAssets/civ/" + civilizationName.ToLower() + "-hat-2"); }
    //    set { hat1 = value; }
    }
    public Color32 ColorBackground
    {
        get { return GetColor(); }
    }
    private Color32 GetColor()
    {
        string trim = colorString.Trim();
        byte[] _color = Array.ConvertAll(trim.Split(','), byte.Parse);
        return new Color32(_color[0], _color[1], _color[2], 255);
    }
    /*
    public Civilization(string name, string color)
    {
        this.civilizationName = name;

        string trim = color.Trim();
        byte[] _color = Array.ConvertAll(trim.Split(','), byte.Parse);
        this.color = new Color32(_color[0], _color[1], _color[2], 255);
    }
    */
    public Civilization()
    {

    }
    /*
    public static Civilization Chanchas = new Civilization("Chanca", new Color32(114, 165, 195, 255));
    public static Civilization Incas = new Civilization("Inca", new Color32(114, 165, 195, 255));
    public static Civilization None = new Civilization("None", Color.white);
    public static Civilization Mochica = new Civilization("Mochica", new Color32(114, 165, 195, 255));
    */
}
