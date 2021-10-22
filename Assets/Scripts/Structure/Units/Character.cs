using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
    [SerializeField] protected string characterName;
    [SerializeField] protected int age;
    [SerializeField] protected string origin;

    [SerializeField] protected string characIconType;
    [SerializeField] protected string characIconIndex;

    private bool canElegible;
    private bool canMove;


    public Character()
    {

    }
    public Sprite Picture 
    { 
        get { return Resources.Load<Sprite>("Textures/TemporalAssets/" + characIconType + "/" + characIconIndex); }
    }
    public Sprite PictureSpriteSheet
    {
        get { return Resources.LoadAll<Sprite>("Textures/TemporalAssets/warriors/Test/" + characIconIndex)[0]; }
    }

    public string CharacterName
    { 
        get { return characterName; }   
        set {characterName = value; }
    }
    public int Age
    {
        get { return age; }
        set { age = value; }
    }
    public string Origin
    {
        get { return origin; }
        set { origin = value; }
    }

    public bool CanElegible
    {
        get { return canElegible; }
        set { canElegible = value; }
    }
    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }
    public string CharacIconType
    {
        get { return characIconType; }
        set { characIconType = value; }
    }
    public string CharacIconIndex
    {
        get { return characIconIndex; }
        set { characIconIndex = value; }
    }
}
