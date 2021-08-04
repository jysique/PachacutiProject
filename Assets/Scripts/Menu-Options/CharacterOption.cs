using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CharacterOption : MonoBehaviour
{
    [SerializeField] private GameObject DescriptionCharacter;
    [SerializeField] private GameObject ProfileCharacter;
    private Button CharacterButton;
    //private  type;
    private Subordinate character;
 //   private Territory territory;
    void Start()
    {
        // It can be in the "start" function because it is instantiated
        InitDescription();
        InitProfile();
        CharacterButton = transform.GetComponent<Button>();
        CharacterButton.onClick.AddListener(()=>HireCharacter());
    }
    public void InitializeCharacterOption(Subordinate _char,Territory _territory=null)
    {
        this.character = _char;
    }
    private void Update()
    {
    }
    /// <summary>
    /// Initialize components in character description option prefab
    /// </summary>
    void InitDescription()
    {
        TextMeshProUGUI[] allText = DescriptionCharacter.gameObject.transform.GetComponentsInChildren<TextMeshProUGUI>();
        allText[1].text = character.Influence.ToString() + "/10";
        allText[2].text = character.CharacterName;
        if (this.character is MilitarChief)
        {
            var militar = (MilitarChief)character;
            allText[0].text = militar.Experience.ToString() + "/10"; 
            allText[3].text = "Strategy:" + militar.StrategyType.ToLower();
        }/*else if (this.character is )
        {
            var militar = (MilitarChief)character;
            allText[0].text = militar.Experience.ToString() + "/10";
            allText[3].text = "Strategy:" + militar.StrategyType.ToLower();
        }*/
    }
    /// <summary>
    /// Initialize components in character profile option prefab
    /// </summary>
    void InitProfile()
    {
        Transform[] allGameObjects = ProfileCharacter.gameObject.transform.GetComponentsInChildren<Transform>();
        allGameObjects[1].GetComponent<Image>().sprite = character.Picture;
    }
    /// <summary>
    /// Function to use to replace a single Character of every territory
    /// </summary>
    public void HireCharacter()
    {
        Territory territory = InGameMenuHandler.instance.TerritorySelected;
        if (this.character is MilitarChief)
        {
            var militar = (MilitarChief)character;
            territory.MilitarChiefTerritory = militar;
        }
        territory.IsClaimed = true;
        //territory.IsClaimed = true;
        MenuManager.instance.CloseSelectCharacterMenu();
        CustomEvent c = EventManager.instance.listEvents.GetCustomEventByTerritory(territory,CustomEvent.EVENTTYPE.CONQUIST);
        if (c!=null)
        {
            c.DeclineEventAction();
        }
        else
        {
            TerritoryManager.instance.UpdateUnitsDeffend(territory);
        }
        EventManager.instance.UpdateEventListOption();
        InGameMenuHandler.instance.UpdateMenu();
    }
}
