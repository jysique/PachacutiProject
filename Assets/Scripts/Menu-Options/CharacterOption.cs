using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    void Start()
    {
        // It can be in the "start" function because it is instantiated
        InitDescription();
        InitProfile();
    }
    public void InitializeCharacterOption(string _type, Subordinate _char,TerritoryHandler _territoryHandler)
    {
        this.type = _type;
        this.character = _char;
        this.territoryHandler = _territoryHandler;
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
        TextMeshProUGUI[] allText = DescriptionCharacter.gameObject.transform.GetComponentsInChildren<TextMeshProUGUI>();
        allText[1].text = character.Influence.ToString() + "/10";
        if (type == "militar")
        {
            var militar = (MilitarChief)character;
            allText[0].text = militar.Experience.ToString() + "/10"; 
            allText[2].text = "Strategy:" + militar.StrategyType.ToLower();
            toggle.group = BattleWonMenu.instance.ToggleGroups[0];
        }else if (type == "comunal")
        {
            var militar = (MilitarChief)character;
            allText[0].text = militar.Experience.ToString() + "/10";
            allText[2].text = "Strategy:" + militar.StrategyType.ToLower();
            toggle.group = BattleWonMenu.instance.ToggleGroups[1];
        }
    }
    /// <summary>
    /// Initialize components in character profile option prefab
    /// </summary>
    void InitProfile()
    {
        Transform[] allGameObjects = ProfileCharacter.gameObject.transform.GetComponentsInChildren<Transform>();
        allGameObjects[1].GetComponent<Image>().sprite = character.Picture;
        allGameObjects[2].GetChild(0).GetComponent<TextMeshProUGUI>().text = character.CharacterName;
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
