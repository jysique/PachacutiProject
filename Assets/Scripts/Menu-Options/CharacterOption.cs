using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CharacterOption : MonoBehaviour
{
    [SerializeField] private GameObject DescriptionCharacter;
    [SerializeField] private GameObject ProfileCharacter;
    [SerializeField] private Toggle toggle;
    private string type;
    private Subordinate character;
    private TerritoryHandler territoryHandler;
 //   private Button btn;
    private bool isSelected;
    public bool IsSelected
    {
        get { return isSelected; }
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
        
     //   btn = transform.GetComponent<Button>();
     //   btn.onClick.AddListener(() => HireButton());
    }
    private void Update()
    {
        if (toggle.isOn)
        {
            HireCharacter();
        }
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
            var militar = (MilitarChief)character;
            allText[0].text = militar.Experience.ToString() + "/10"; 
            allText[2].text = "Strategy:" + militar.StrategyType.ToLower();
            toggle.group = BattleWonMenu.instance.toggleGroupMilitar;
        }else if (type == "comunal")
        {
            var militar = (MilitarChief)character;
            allText[0].text = militar.Experience.ToString() + "/10";
            allText[2].text = "Strategy:" + militar.StrategyType.ToLower();
            toggle.group = BattleWonMenu.instance.toggleGroupCommunal;
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
    public void HireCharacter()
    {
        if (type == "militar")
        {
            var militar = (MilitarChief)character;
            territoryHandler.territoryStats.territory.MilitarChiefTerritory = militar;
           // print("reemplazando jefe militar " + character.CharacterName);
        }
        else if (type == "comunal")
        {
          //  print("reemplazando jefe comunal " + Character.CharacterName);
        }
    }
}
