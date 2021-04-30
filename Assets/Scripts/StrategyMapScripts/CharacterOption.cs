using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterOption : MonoBehaviour
{
    [SerializeField] private GameObject DescriptionCharacter;
    [SerializeField] private GameObject ProfileCharacter;
    private string type;
    private Subordinate character;
    private TerritoryHandler territoryHandler;
    private Button btn;
    private int indexList;
    public int Index
    {
        get { return indexList; }
        set { indexList = value; }
    }
    public string Type
    {
        get { return type; }
        set { type = value; }
    }
    public Subordinate Character
    {
        get { return character; }
        set { character = value; }
    }
    public TerritoryHandler territoryHandlerInCharacter
    {
        get { return territoryHandler; }
        set { territoryHandler = value; }
    }
    void Start()
    {
        InitDescription();
        InitProfile();
        btn = transform.GetComponent<Button>();
        btn.onClick.AddListener(() => HireButton());
    }
    void InitDescription()
    {
        Text[] allText = DescriptionCharacter.gameObject.transform.GetComponentsInChildren<Text>();
        //print("all text" +allText.Length);
        allText[0].text = "Origin: " + character.Origin;
        allText[1].text = "Age: " + character.Age.ToString();
        allText[2].text = "Campain: " + character.Campaign;
        allText[4].text = "Opinion: " + character.Opinion.ToString();
        allText[5].text = "Influence: " + character.Influence.ToString();
        if (type == "militar")
        {
            var militar = (MilitarBoss)character;
            allText[3].text = "Exp:" + militar.Experience.ToString();
            allText[6].text = "StraType:" + militar.StrategyType.ToLower();
        }
    }
    void InitProfile()
    {
        Transform[] allGameObjects = ProfileCharacter.gameObject.transform.GetComponentsInChildren<Transform>();
        allGameObjects[1].GetComponent<Image>().sprite = character.Picture;
        allGameObjects[3].GetComponent<Text>().text = character.CharacterName;
    }
    /// <summary>
    /// Function to use to replace a single Character of every territory
    /// </summary>
    public void HireButton()
    {
        if (type == "militar")
        {
            var militar = (MilitarBoss)character;
            territoryHandler.territoryStats.territory.MilitarBossTerritory = militar;
        }

        InGameMenuHandler.instance.CloseCharacterSelection();
        Time.timeScale = 1;
    }
}
