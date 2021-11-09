using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;

    [Header("Menu Manager")]
    [SerializeField] public GameObject contextMenu;
    [SerializeField] public GameObject selectTroopsMenu;
    [SerializeField] public GameObject overMenuBlock;
    [SerializeField] public GameObject toolTip;

    public GameObject canvas;

    [Header("Menu de Pause")]
    [SerializeField] private GameObject PauseMenu;
    [SerializeField] private GameObject ChiefSelection;
    //public static bool isGamePaused = false;
    public float temporalTime;

    [Header("Menu territory")]
    [SerializeField] private Button openBtn;
    [SerializeField] private TextMeshProUGUI openTxt;
    [SerializeField] private GameObject menuTerritory;
    

    [Header("Select MilitaryBoss variables")]
    [SerializeField] GameObject SelecCharacterMenu;
    [SerializeField] public TextMeshProUGUI titleSelectCharacterMenu;
    [SerializeField] public TextMeshProUGUI descriptionSelectCharacterMenu;
    private int maxInstantiateCharacters = 3;
    List<GameObject> options = new List<GameObject>();

    [Header("Tabs Button")]
    [SerializeField] private TabButton tabProfile;
    [SerializeField] private TabButton tabEvent;
    [SerializeField] private TabButton tabMission;
    [SerializeField] private TabButton tabTerritory;
    [SerializeField] private TabButton tabMilitar;
    [SerializeField] private TabButton tabBuilding;
    public bool IsOpen
    {
        get { return isOpen; }
    }
    public TabButton TabMilitar
    {
        get { return tabMilitar; }
    }
    public TabButton TabBuiling
    {
        get { return tabBuilding; }
    }
    public void AccessTabProfile()
    {
        tabProfile.AccessToMenu();
    }
    public void AccessTabEvent()
    {
        tabEvent.AccessToMenu();
    }
    public void AccessTabMission()
    {
        tabMission.AccessToMenu();
    }
    public void AccessTabTerritory()
    {
        tabTerritory.AccessToMenu();
    }
    public void AccessTabMilitar()
    {
        tabMilitar.AccessToMenu();
    }
    public void AccessTabBuildings()
    {
        tabBuilding.AccessToMenu();
    }

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InitButtonMenuPause();
        openBtn.onClick.AddListener(() => OpenMenuLearp());
    }

    public void PauseMenuGame()
    {
        PauseMenu.SetActive(true);
    }
    public void ResumeMenuGame()
    {
        PauseMenu.SetActive(false);
    }

    public void OpenMenuLearp()
    {
        RectTransform rectTransform = menuTerritory.GetComponent<RectTransform>(); //getting reference to this componen
        currentTime = 0;
        StartCoroutine(Moving(rectTransform));

    }
    float timeOfTravel = 0.5f; //time after object reach a target place 
    float currentTime; // actual floting time 
    float normalizedValue;
    private bool isOpen = false;
    IEnumerator Moving(RectTransform rect)
    {   
        Vector2 startPosition = new Vector2(0, 0);
        Vector2 endPosition = new Vector2(570,0);
        while (currentTime <= timeOfTravel)
        {
            currentTime += Time.deltaTime;
            normalizedValue = currentTime / timeOfTravel; // we normalize our time 
            openBtn.interactable = false;
            if (!isOpen)
            {
                
                rect.anchoredPosition = Vector3.Lerp(rect.anchoredPosition, endPosition, normalizedValue);
                yield return new WaitForEndOfFrame();
            }
            else
            {
                rect.anchoredPosition = Vector3.Lerp(rect.anchoredPosition, startPosition, normalizedValue);
                yield return new WaitForEndOfFrame();
            }
        }
        openBtn.interactable = true;
        if (!isOpen)
        {
            isOpen = true;
            openTxt.text = "C\nL\nO\nS\nE";
        }
        else
        {
            isOpen = false;
            openTxt.text = "O\nP\nE\nN";
        }
        yield return null;
    }

    void InitButtonMenuPause()
    {
        Button[] allPauseButton = PauseMenu.gameObject.transform.GetComponentsInChildren<Button>();

        allPauseButton[0].onClick.AddListener(() => ResumeMenuGame());
        allPauseButton[1].onClick.AddListener(() => GlobalVariables.instance.GoToMenuGame());
        allPauseButton[2].onClick.AddListener(() => GlobalVariables.instance.ClosingApp());
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

    public void ActivateSelectTroopsMenu(TerritoryHandler territoryToAttack)
    {
        selectTroopsMenu.SetActive(true);
        selectTroopsMenu.GetComponent<SelectTropsMenu>().InitMenu(territoryToAttack);
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
    
    public void OpenSelectCharacterMenu(MilitarChief character)
    {
        SelecCharacterMenu.SetActive(true);
        InstantiateCharacterOption( ChiefSelection, character , options);
        descriptionSelectCharacterMenu.text = GameMultiLang.GetTraduction("MilitaryChiefDescription");
        titleSelectCharacterMenu.text = GameMultiLang.GetTraduction("SelectTerritory") + InGameMenuHandler.instance.TerritorySelected.name + " " +GameMultiLang.GetTraduction("TerritoryLabel");
    }
    public void CloseSelectCharacterMenu()
    {
        SelecCharacterMenu.SetActive(false);
        for (int i = 0; i < options.Count; i++)
        {
            Destroy(options[i]);
        }
    }
    
    void InstantiateCharacterOption(GameObject selection,Character character, List<GameObject> list)
    {
        SubordinateList subordinateList = new SubordinateList();
        string territoryName = InGameMenuHandler.instance.TerritorySelected.name;
        subordinateList.AddDataSubordinateToList(maxInstantiateCharacters, character, territoryName);
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
            characterOption.GetComponent<CharacterOption>().InitializeCharacterOption(charac);
            list.Add(characterOption);
        }
        subordinateList.DeleteSubodinateList();
    }
}