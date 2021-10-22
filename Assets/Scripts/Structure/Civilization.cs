using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Civilization
{
    [SerializeField] string civilizationName;
    [SerializeField] string colorString;
    public string Name
    {
        get { return civilizationName; }
    }
    public Sprite Hat1
    {
        get { return Resources.Load<Sprite>("Textures/TemporalAssets/civ/" + civilizationName.ToLower() + "-hat-1"); }
    }
    public Sprite Hat2
    {
        get { return Resources.Load<Sprite>("Textures/TemporalAssets/civ/" + civilizationName.ToLower() + "-hat-2"); }
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
    public Civilization()
    {

    }
}
