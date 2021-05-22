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
    /// <summary>
    /// Initialize components in character description option prefab
    /// </summary>
    void InitDescription()
    {
        Text[] allText = DescriptionCharacter.gameObject.transform.GetComponentsInChildren<Text>();
        allText[1].text = character.Influence.ToString() + "/10";
        if (type == "militar")
        {
            var militar = (MilitarBoss)character;
            allText[0].text = militar.Experience.ToString() + "/10"; 
            allText[2].text = "Strategy:" + militar.StrategyType.ToLower();
        }
    }
    /// <summary>
    /// Initialize components in character profile option prefab
    /// </summary>
    void InitProfile()
    {
        Transform[] allGameObjects = ProfileCharacter.gameObject.transform.GetComponentsInChildren<Transform>();
        allGameObjects[1].GetComponent<Image>().sprite = character.Picture;
        allGameObjects[2].GetChild(0).GetComponent<Text>().text = character.CharacterName;
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
        InGameMenuHandler.instance.UpdateMenu();
        MenuManager.instance.CloseCharacterSelection();
        MenuManager.instance.ResumeGame();
    }
}
