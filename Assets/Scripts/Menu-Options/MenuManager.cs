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
    public GameObject canvas;

    [Header("Menu de Pause")]
    [SerializeField] private GameObject PauseMenu;

    [Header("Select MilitaryBoss variables")]
    [SerializeField] GameObject CurrentCaseMenu;
    [SerializeField] private GameObject CharacterSelection;

    public static bool isGamePaused = false;
    public float temporalTime = 1;
    private List<GameObject> characterOptions;
    public SubordinateList subordinateList;
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
    }
    public void PauseGame()
    {
        turnOffMenus();
        if(GlobalVariables.instance.timeModifier != 0) temporalTime = GlobalVariables.instance.timeModifier;
        print("pause" + temporalTime);
        GlobalVariables.instance.timeModifier = 0;

    }
    public void ResumeGame()
    {
        print("temporal time" + temporalTime);
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

    public void OpenCurrentCaseMenu(TerritoryHandler territoryHandler)
    {
        CurrentCaseMenu.SetActive(true);
        CurrentCaseMenu.transform.Find("Territory").GetComponent<Image>().sprite = territoryHandler.sprite.sprite;
        CurrentCaseMenu.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = "You just won the battle of " + territoryHandler.territoryStats.territory.name;
        InstantiateCharacterOption(territoryHandler, "militar");
    }
    public void CloseCurrentCaseMenu()
    {
        CurrentCaseMenu.SetActive(false);
    }

    public void InstantiateCharacterOption(TerritoryHandler territoryHandler, string type)
    {
        subordinateList.AddDataSubordinateToList(3, type);
        CharacterSelection.SetActive(true);
        characterOptions = new List<GameObject>();
        TextMeshProUGUI instructionText = CharacterSelection.transform.Find("InstructionText").transform.GetComponent<TextMeshProUGUI>();
        instructionText.text = "Select a " + type + " Chief to " + territoryHandler.territoryStats.territory.name;
        Transform gridLayout = CharacterSelection.transform.Find("ScrollArea/ScrollContainer/GridLayout").transform;
        foreach (Subordinate charac in subordinateList.MilitarBosses)
        {
            GameObject characterOption = Instantiate(Resources.Load("Prefabs/MenuPrefabs/CharacterGameOption")) as GameObject;
            characterOption.transform.SetParent(gridLayout.transform, false);
            characterOption.name = charac.CharacterName;
            characterOption.GetComponent<CharacterOption>().Type = type;
            characterOption.GetComponent<CharacterOption>().Character = charac;
            characterOption.GetComponent<CharacterOption>().territoryHandlerInCharacter = territoryHandler;
            characterOptions.Add(characterOption);
        }
        PauseGame();
    }
    public void CloseCharacterSelection()
    {
        CharacterSelection.SetActive(false);
        for (int i = 0; i < characterOptions.Count; i++)
        {
            Destroy(characterOptions[i]);
        }
        subordinateList.DeleteSubodinateList();
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
}
