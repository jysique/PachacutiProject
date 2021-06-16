using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public static MenuManager instance;

    [SerializeField] public GameObject contextMenu;
    [SerializeField] public GameObject overMenuBlock;
    [SerializeField] public GameObject toolTip;
    [SerializeField] public Button ContinueBattleWon;
    [SerializeField] public TextMeshProUGUI titleBattleWonMenu;
    [SerializeField] public TextMeshProUGUI titleSelectCharacterMenu;
    [SerializeField] public TextMeshProUGUI descriptionSelectCharacterMenu;
    [SerializeField] public Image imageBattleWonMenu;
    public GameObject canvas;

    [Header("Menu de Pause")]
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject ChiefSelection;

    [Header("Select MilitaryBoss variables")]
    [SerializeField] GameObject BattlewonMenu;
    [SerializeField] GameObject SelecCharacterMenu;
    private int maxInstantiateCharacters = 3;
    List<GameObject> options = new List<GameObject>();

    public static bool isGamePaused = false;
    public float temporalTime;
    
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InitButtonMenuPause();
    }
    public void PauseMenuGame()
    {
        PauseMenu.SetActive(true);
    }
    public void ResumeMenuGame()
    {
        PauseMenu.SetActive(false);
    }
    void InitButtonMenuPause()
    {
        Button[] allPauseButton = PauseMenu.gameObject.transform.GetComponentsInChildren<Button>();

        allPauseButton[0].onClick.AddListener(() => ResumeMenuGame());
        allPauseButton[1].onClick.AddListener(() => GlobalVariables.instance.GoToMenuGame());
        allPauseButton[2].onClick.AddListener(() => GlobalVariables.instance.ClosingApp());

        ContinueBattleWon.onClick.AddListener(() => CloseBattleWonMenu());
    }
    public void TurnOffBlock()
    {
        overMenuBlock.SetActive(false);
    }
    public void TurnOnBlock()
    {
        overMenuBlock.SetActive(true);
    }

    public void ActivateContextMenu(TerritoryHandler territoryToAttack, bool canAttack, bool isWar, Vector3 mousePosition)
    {
        TurnOnBlock();
        contextMenu.SetActive(true);
        TerritoryManager.instance.ChangeStateTerritory(2);
        Vector3 mousePosCamera = Camera.main.ScreenToWorldPoint(mousePosition);
        contextMenu.transform.position = new Vector3(mousePosCamera.x, mousePosCamera.y, contextMenu.transform.position.z);
        contextMenu.GetComponent<ContextMenu>().SetMenu(canAttack, isWar, territoryToAttack);
    }

    public void OpenBattleWonMenu(TerritoryHandler territoryHandler)
    {
        BattlewonMenu.SetActive(true);
        BattlewonMenu.transform.Find("BattleWonDescription").GetComponent<Button>().onClick.AddListener(() => GlobalVariables.instance.CenterCameraToTerritory(territoryHandler));
        //   titleBattleWonMenu.text = "You just won the battle of " + territoryHandler.territoryStats.territory.name;
        titleBattleWonMenu.text = GameMultiLang.GetTraduction("BattleWon") + territoryHandler.territoryStats.territory.name;
        imageBattleWonMenu.sprite = territoryHandler.sprite.sprite;
        DateTableHandler.instance.PauseTime();
    }
    public void CloseBattleWonMenu()
    {
        BattlewonMenu.SetActive(false);
        DateTableHandler.instance.ResumeTime();
    }

    public void turnOffMenus()
    {
        GameObject[] overMenus;
        overMenus = GameObject.FindGameObjectsWithTag("OverMenu");
        foreach (GameObject overMenu in overMenus)
        {
            overMenu.SetActive(false);
        }
        TerritoryManager.instance.ChangeStateTerritory(0);
    }
    
    public void OpenSelectCharacterMenu(Territory territory, Character character)
    {
        SelecCharacterMenu.SetActive(true);
        InstantiateCharacterOption(territory, ChiefSelection, character , options);
        descriptionSelectCharacterMenu.text = character.Description;
        //titleSelectCharacterMenu.text = "Select the characters for " + territory.name + " territory";
        titleSelectCharacterMenu.text = GameMultiLang.GetTraduction("SelectTerritory") + territory.name + " " +GameMultiLang.GetTraduction("TerritoryLabel");
        //  territory.TypePlayer = Territory.TYPEPLAYER.PLAYER;
    }
    public void CloseSelectCharacterMenu()
    {
        SelecCharacterMenu.SetActive(false);
        for (int i = 0; i < options.Count; i++)
        {
            Destroy(options[i]);
        }
    }
    
    void InstantiateCharacterOption(Territory territory, GameObject selection,Character character, List<GameObject> list)
    {
        SubordinateList subordinateList = new SubordinateList();
        subordinateList.AddDataSubordinateToList(maxInstantiateCharacters, character);
        Transform gridLayout = selection.transform.Find("ScrollArea/ScrollContainer/GridLayout").transform;
        foreach (Transform child in gridLayout)
        {
            Destroy(child.gameObject);
        }
        foreach (Subordinate charac in subordinateList.Chiefs)
        {
            GameObject characterOption = Instantiate(Resources.Load("Prefabs/MenuPrefabs/CharacterGameOption")) as GameObject;
            characterOption.transform.SetParent(gridLayout.transform, false);
            characterOption.name = charac.CharacterName;
            characterOption.GetComponent<CharacterOption>().InitializeCharacterOption(charac, territory);
            list.Add(characterOption);
        }
        subordinateList.DeleteSubodinateList();
    }
}