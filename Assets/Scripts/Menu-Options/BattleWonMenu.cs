using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleWonMenu : MonoBehaviour
{
    public static BattleWonMenu instance;
    private Image territoryImage;
    private Text titleBattle;
    private Text instructionTitle;
    private Button continueButton;
    [SerializeField] private GameObject MilitarChiefSelection;
    [SerializeField] private GameObject CommunalChiefSelection;
    [SerializeField] public ToggleGroup toggleGroupMilitar;
    [SerializeField] public ToggleGroup toggleGroupCommunal;
    private List<GameObject> characterOptions;
    private SubordinateList subordinateList;
    
    bool a;
    bool b;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        continueButton = transform.Find("ButtonContinue").GetComponent<Button>();
        continueButton.onClick.AddListener(() => HireButton());
        continueButton.interactable = false;
    }
    private void Update()
    {
        if (toggleGroupMilitar.AnyTogglesOn())
        {
            a = true;
        }
        /*
        if (toggleGroupCommunal.AnyTogglesOn())
        {
            b = true;
        }
        */
        if (a)
        {
            continueButton.interactable = true;
        }
    }
    public void InitBattleWonMenu(TerritoryHandler territoryHandler)
    {
        territoryImage = transform.Find("BattleWonDescription/Territory").GetComponent<Image>();
        territoryImage.sprite = territoryHandler.sprite.sprite;
        titleBattle = transform.Find("BattleWonDescription/Title").GetComponent<Text>();
        titleBattle.text = "You just won the battle of " + territoryHandler.territoryStats.territory.name;
        instructionTitle = transform.Find("InstructionText").GetComponent<Text>();
        instructionTitle.text += territoryHandler.territoryStats.territory.name + " territory";
        InstantiateCharacterOption(territoryHandler, MilitarChiefSelection, "militar");
        InstantiateCharacterOption(territoryHandler, CommunalChiefSelection, "comunal");
    }

    public void InstantiateCharacterOption(TerritoryHandler territoryHandler, GameObject selection, string type)
    {
        subordinateList = new SubordinateList();
        subordinateList.AddDataSubordinateToList(3, type);
        characterOptions = new List<GameObject>();
        Transform gridLayout = selection.transform.Find("ScrollArea/ScrollContainer/GridLayout").transform;
        foreach (Subordinate charac in subordinateList.Chiefs)
        {
            GameObject characterOption = Instantiate(Resources.Load("Prefabs/MenuPrefabs/CharacterGameOption2")) as GameObject;
            characterOption.transform.SetParent(gridLayout.transform, false);
            characterOption.name = charac.CharacterName;
            characterOption.GetComponent<CharacterOption>().Type = type;
            characterOption.GetComponent<CharacterOption>().Character = charac;
            characterOption.GetComponent<CharacterOption>().territoryHandlerInCharacter = territoryHandler;
            characterOptions.Add(characterOption);
        }
        subordinateList.DeleteSubodinateList();
        //PauseGame();
    }
    private void HireButton()
    {

        MenuManager.instance.CloseBattleWonMenu();
    }
}
