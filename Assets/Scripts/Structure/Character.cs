using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Character
{


    [SerializeField] private string characterName;
    [SerializeField] private int age;
    [SerializeField] private string origin;
    [SerializeField] private string campaign;
    private string personality;
    private Sprite picture;
    private bool canElegible;
    private bool canMove;
    
    public Sprite Picture 
    { 
        get { return picture; }
        set { picture = value; }
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
    public string Campaign
    {
        get { return campaign; }
        set { campaign = value; }
    }
    public string Personality
    {
        get { return personality; }
        set { personality = value; }
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
}
