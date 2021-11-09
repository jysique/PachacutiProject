using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{
    [SerializeField] protected string characterName;
    [SerializeField] protected int age;
    [SerializeField] protected string origin;
    [SerializeField] protected string pathPicture;
    [SerializeField] protected string pathAudio;

    [SerializeField] protected int experience;
    protected bool canElegible;
    protected bool canMove;


    public Character()
    {

    }
    public Sprite Picture 
    { 
        get { return Resources.Load<Sprite>("Textures/TemporalAssets/" + pathPicture); }
    }
    public Sprite PictureSpriteSheet
    {
        get { return Resources.LoadAll<Sprite>("Textures/TemporalAssets/" + pathPicture)[0]; }
    }
    public string PathAudio
    {
        get { return pathAudio; }
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
    public int Experience
    {
        get { return experience; }
        set { experience = value; }
    }

}
