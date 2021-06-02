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
    [SerializeField] private string personality;
    [SerializeField] private string characIconType;
    [SerializeField] private string characIconIndex;
    private string description;
    private Sprite picture;
    private bool canElegible;
    private bool canMove;

    public Character()
    {

    }
    public Sprite Picture 
    { 
        get { return Resources.Load<Sprite>("Textures/TemporalAssets/" + characIconType + "/" + characIconIndex); }
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
    public string Description
    {
        get { return description; }
        set { description = value; }
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
