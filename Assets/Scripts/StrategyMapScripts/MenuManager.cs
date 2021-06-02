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

    private void PauseMenuGame()
    {
        PauseMenu.SetActive(true);
        PauseGame();
    }
    private void ResumeMenuGame()
    {
        PauseMenu.SetActive(false);
        ResumeGame();
    }
    void InitButtonMenuPause()
    {
        Button[] allPauseButton = PauseMenu.gameObject.transform.GetComponentsInChildren<Button>();

        allPauseButton[0].onClick.AddListener(() => ResumeMenuGame());
        allPauseButton[1].onClick.AddListener(() => GlobalVariables.instance.GoToMenuGame());
        allPauseButton[2].onClick.AddListener(() => GlobalVariables.instance.ClosingApp());

        ContinueBattleWon.onClick.AddListener(() => CloseBattleWonMenu());
    }
    public void PauseGame()
    {
        turnOffMenus();
        if(GlobalVariables.instance.timeModifier != 0) temporalTime = GlobalVariables.instance.timeModifier;
        GlobalVariables.instance.timeModifier = 0;

    }
    public void ResumeGame()
    {
       // print("temporal time" + temporalTime);
        GlobalVariables.instance.timeModifier = temporalTime;
        //        print(temporalTime);
    }
    public void EscapeGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeMenuGame();
            }
            else
            {
                PauseMenuGame();
            }
            //  SceneManager.LoadScene(0);
        }
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
        titleBattleWonMenu.text = "You just won the battle of " + territoryHandler.territoryStats.territory.name;
        imageBattleWonMenu.sprite = territoryHandler.sprite.sprite;
        
        PauseGame();
    }
    public void CloseBattleWonMenu()
    {
        BattlewonMenu.SetActive(false);
        ResumeGame();
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

    private void Update()
    {
        EscapeGame();
    }
    List<GameObject> options = new List<GameObject>();
    public void OpenSelectCharacterMenu(Territory territory, Character character)
    {
        SelecCharacterMenu.SetActive(true);
        InstantiateCharacterOption(territory, ChiefSelection, character , options);
        descriptionSelectCharacterMenu.text = character.Description;
        titleSelectCharacterMenu.text = "Select the characters for " + territory.name + " territory";
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
    private int maxInstantiateCharacters = 3;
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

/*
 * public static BattleWonMenu instance;
    private Image territoryImage;
    private Text titleBattle;
    private Text instructionTitle;
    private Button continueButton;
    [SerializeField] private GameObject MilitarChiefSelection;
  //  [SerializeField] private GameObject CommunalChiefSelection;

    private List<GameObject> militarOptions = new List<GameObject>();
  //  private List<GameObject> communalOptions = new List<GameObject>();
    private int maxInstantiateCharacters = 3;
    private int numberOfTab = 1;
    private ToggleGroup[] toggleGroups;
    private bool[] canContinue;

    public ToggleGroup[] ToggleGroups
    {
        get { return toggleGroups; }
    }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {

    }
    private void OnEnable()
    {
        continueButton = transform.Find("ButtonContinue").GetComponent<Button>();
        territoryImage = transform.Find("BattleWonDescription/Territory").GetComponent<Image>();
        titleBattle = transform.Find("BattleWonDescription/Title").GetComponent<Text>();
        instructionTitle = transform.Find("InstructionText").GetComponent<Text>();

        continueButton.onClick.AddListener(() => ContinueButton2());
        continueButton.interactable = false;

        toggleGroups = new ToggleGroup[numberOfTab];
        canContinue = new bool[numberOfTab];
        toggleGroups[0] = MilitarChiefSelection.GetComponent<ToggleGroup>();
     //   toggleGroups[1] = CommunalChiefSelection.GetComponent<ToggleGroup>();

        for (int i = 0; i < toggleGroups.Length; i++)
        {
            toggleGroups[i].SetAllTogglesOff();
        }
        ResetCanContinue();
    }
    private void Update()
    {
        for (int i = 0; i < toggleGroups.Length; i++)
        {
            CheckToggleGruop(i);
        }
        if (canContinue.All(x=>x == true))
        {
            continueButton.interactable = true;
        }
    }
    void InstantiateCharacterOption(TerritoryHandler territoryHandler, GameObject selection, List<GameObject> list,string type)
    {
        SubordinateList subordinateList = new SubordinateList();
        subordinateList.AddDataSubordinateToList(maxInstantiateCharacters, type);
        Transform gridLayout = selection.transform.Find("ScrollArea/ScrollContainer/GridLayout").transform;
        foreach (Subordinate charac in subordinateList.Chiefs)
        {
            GameObject characterOption = Instantiate(Resources.Load("Prefabs/MenuPrefabs/CharacterGameOption")) as GameObject;
            characterOption.transform.SetParent(gridLayout.transform, false);
            characterOption.name = charac.CharacterName;
            characterOption.GetComponent<CharacterOption>().InitializeCharacterOption(type,charac,territoryHandler);
            list.Add(characterOption);
        }
        subordinateList.DeleteSubodinateList();
    }
    void ContinueButton()
    {
        ResetCanContinue();
        for (int i = 0; i < militarOptions.Count; i++)
        {
            Destroy(militarOptions[i]);
          //  Destroy(communalOptions[i]);
        }
        MenuManager.instance.CloseBattleWonMenu();
    }
    void ContinueButton2()
    {
        MenuManager.instance.CloseBattleWonMenu();
    }

    void ResetCanContinue()
    {
        for (int i = 0; i < canContinue.Length; i++)
        {
            canContinue[i] = false;
        }
    }
    void CheckToggleGruop(int i)
    {
        if (toggleGroups[i].AnyTogglesOn())
        {
            canContinue[i] = true;
        }
    }
    public void InitBattleWonMenu(TerritoryHandler territoryHandler)
    {
        territoryImage.sprite = territoryHandler.sprite.sprite;
        titleBattle.text = "You just won the battle of " + territoryHandler.territoryStats.territory.name;
        instructionTitle.text += territoryHandler.territoryStats.territory.name + " territory";
        InstantiateCharacterOption(territoryHandler, MilitarChiefSelection, militarOptions, "militar");
      //  InstantiateCharacterOption(territoryHandler, CommunalChiefSelection, communalOptions, "comunal");
    }

 * */