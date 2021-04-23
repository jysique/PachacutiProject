using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class InGameMenuHandler : MonoBehaviour
{
    public int warriorsNumber;
    public static InGameMenuHandler instance;
    [SerializeField] private GameObject contextMenu;
    [SerializeField] private GameObject warriorsPrefab;
    [SerializeField] private Territory selectedTerritory;
    [Header("Variables globales")]
    [SerializeField] public Canvas canvas;
    [Header("ToolTip")]
    [SerializeField] public GameObject toolTip;
    [Header("OverMenu")]
    [SerializeField] private GameObject menuConfirm;
    [SerializeField] public GameObject overMenuBlock;
    [Header("Menu personaje")]
    [SerializeField] private Text governorName;
    [SerializeField] private Text governorAge;
    [SerializeField] private Text governorOrigin;
    [SerializeField] private Image governorPicture;
    [SerializeField] private Text governorDiplomacy;
    [SerializeField] private Text governorMilitancy;
    [SerializeField] private Text governorManagement;
    [SerializeField] private Text governorPrestige;
    [SerializeField] private Text governorPiety;

    [Header("Menu militar")]
    [SerializeField] private GameObject menuBlock;
    [SerializeField] private Image militaryBossPicture;
    [SerializeField] private Text militarWarriorsCount;
    [SerializeField] private Text GenerationSpeed;
    [SerializeField] private Text militaryBossName;
    [SerializeField] private Text militaryBossExperience;
    [SerializeField] private Text militaryBossEstrategy;
    [SerializeField] private Text militaryBossMilitary;
    [Header("Menu de movilizacion de tropas")]
    [SerializeField] public InputField warriorsCount;
    [Header("Menu territorio")]
    [SerializeField] private Text territoryName;
    [SerializeField] private Text goldCount;
    [SerializeField] private Text[] levelTexts;
    [SerializeField] private Image[] countdownImages;
    [SerializeField] private Button[] buttons;
    [Header("Recursos")]
    [SerializeField] private Text goldGenerated;
    [SerializeField] private Text foodGenerated;
   // [SerializeField] private Text sucesionSizeTxt;
    [SerializeField] private Text scoreTxt;
    [SerializeField] private Transform goldAnimation;
    [SerializeField] private Transform foodAnimation;
    [Header("Evento")]
    [SerializeField] private GameObject CustomEventSelection;
    [SerializeField] private Text TerritoryEventTxt;
    [SerializeField] private Text DetailsTextCustomEvent;
    [SerializeField] private Text AcceptTextCustomEvent;
    [SerializeField] private Text DeclineTextCustomEvent;
    [SerializeField] private Button AcceptEventButton;
    [SerializeField] private Button CloseEventButton;
    [SerializeField] private Button DeclineEventButton;

    [Header("Select MilitaryBoss variables")]
    [SerializeField] private GameObject CharacterSelection;
    

    private List<GameObject> characterOptions;
    public MilitarBossList ml;
    private int goldPlayer;
    int foodPlayer;
    int sucesionSizePlayer;
    int scorePlayer;
    int goldNeedSpeed = 10;
    int foodNeedLimit = 10;
    //perfil menu
    public int GoldPlayer
    {
        get { return goldPlayer; }
        set { goldPlayer = value; }
    }
    public int FoodPlayer
    {
        get { return foodPlayer; }
        set { foodPlayer= value; }
    }
    public int GoldNeedSpeed
    {
        get { return goldNeedSpeed; }
    }
    public int FoodNeedLimit
    {
        get { return foodNeedLimit; }
    }
    private void Awake()
    {
        instance = this;
        warriorsNumber = 0;
    }
    public void UpdateResourceTable()
    {
        goldGenerated.text = goldPlayer.ToString();
        foodGenerated.text = foodPlayer.ToString();
     //   sucesionSizeTxt.text = "0";
        scoreTxt.text = TerritoryManager.instance.CountTerrytorry(Territory.TYPEPLAYER.PLAYER).ToString();
    }
    public void UpdateMilitarMenu()
    {

        if (selectedTerritory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
        {
            menuBlock.SetActive(false);
            MilitarBoss boss = selectedTerritory.MilitarBossTerritory;
            militaryBossName.text = "Nombre: " + boss.CharacterName;
            militaryBossPicture.sprite = boss.Picture;
            militaryBossExperience.text = "Experiencia: " + boss.Experience;
            militaryBossEstrategy.text = "Estrategia: " + boss.StrategyType;
            militaryBossMilitary.text = "Influencia: " + boss.Influence;
            GenerationSpeed.text = " " + selectedTerritory.VelocityPopulation;
            
        }
        else
        {
            menuBlock.SetActive(true);
        }
    }
    public void UpdateTerritoryMenu()
    {
        if (selectedTerritory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
        {
            territoryName.text = selectedTerritory.name;
            goldCount.text = "Oro: " + selectedTerritory.Gold.ToString();
            goldCount.text += "\nVelocidad oro: " + selectedTerritory.GoldMineTerritory.VelocityGold.ToString();
            levelTexts[0].text = "Level: " + selectedTerritory.GoldMineTerritory.Level.ToString();
            levelTexts[1].text = "Level: " + selectedTerritory.SacredPlaceTerritory.Level.ToString();
            levelTexts[2].text = "Level: " + selectedTerritory.FortressTerritory.Level.ToString();
            levelTexts[3].text = "Level: " + selectedTerritory.BarracksTerritory.Level.ToString();
        }
        else
        {
            menuBlock.SetActive(true);
        }
        
    }
    public void UpdateProfileMenu()
    {
        Governor temp = CharacterManager.instance.Governor;
        governorName.text = "Nombre: " + temp.CharacterName;
        governorAge.text = "Edad: " + temp.Age.ToString();
        governorPicture.sprite = temp.Picture;
        governorOrigin.text = "Origen: " + temp.Origin;
        governorDiplomacy.text = "Diplomacia: " + temp.Diplomacy;
        governorMilitancy.text = "Militar: " + temp.Militancy;
        governorManagement.text = "Administracion: " + temp.Managment;
        governorPrestige.text = "Prestigio: " + temp.Prestige;
        governorPiety.text = "Piedad: " + temp.Piety;
    }
    public void UpdateMenu()
    {
        selectedTerritory = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territoryStats.territory;
        UpdateMilitarMenu();
        UpdateTerritoryMenu();

    }
    void Update()
    {
        Territory selectedTerritory = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territoryStats.territory;
        militarWarriorsCount.text = selectedTerritory.Population.ToString() + " / " + selectedTerritory.LimitPopulation.ToString()+ " unidades" ;
        
        UpdateResourceTable();
        EscapeGame();
    }

    //move warriors
    public void MoveWarriorsButton()
    {
        TurnOnBlock();
        menuConfirm.SetActive(true);
        warriorsCount.text = "1";
        ChangeStateTerritory(2);

    }
    public void ChangeStateTerritory(int _state)
    {
        foreach (GameObject t in TerritoryManager.instance.territoryList)
        {
            if(t.GetComponent<TerritoryHandler>().state !=1)t.GetComponent<TerritoryHandler>().state = _state;
        }
    }

    public void CloseWarriorsButton()
    {
        menuConfirm.SetActive(false);
        TurnOffBlock();
        ChangeStateTerritory(0);
    }
    public void SelectTerritory()
    {
        warriorsNumber = int.Parse(warriorsCount.text);
        if (TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territoryStats.territory.Population - warriorsNumber >= 0)
        {
            menuConfirm.SetActive(false);
            TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().ShowAdjacentTerritories();
        }
    }

    public void SendWarriors(TerritoryHandler selected, TerritoryHandler otherTerritory, int _warriorsNumber)
    {

        selected.territoryStats.territory.Population = selected.territoryStats.territory.Population - _warriorsNumber;
        warriorsPrefab.GetComponent<WarriorsMoving>().SetAttack(otherTerritory.gameObject, _warriorsNumber, selected.territoryStats.territory.TypePlayer, selected.territoryStats.territory.MilitarBossTerritory);
        Instantiate(warriorsPrefab, selected.transform.position , Quaternion.identity);

    }
    public void MoveWarriors(TerritoryHandler otherTerritory, int attackPower, TerritoryHandler attacker)
    {
        Territory.TYPEPLAYER temp = otherTerritory.territoryStats.territory.TypePlayer;
        if (otherTerritory.territoryStats.territory.TypePlayer == type)
        {
            otherTerritory.territoryStats.territory.Population = otherTerritory.territoryStats.territory.Population + attackPower;

        }
        else
        {
            if (otherTerritory.war)
            {
                WarManager.instance.AddMoreWarriors(otherTerritory, attackPower);

            }
            else
            {
                float vAttack = WarManager.instance.SetAttackFormula(attacker, attackPower);
                float vDef = WarManager.instance.SetDefenseFormula(otherTerritory);
                WarManager.instance.AddWar(attackPower, otherTerritory.territoryStats.population, vAttack, vDef, otherTerritory, attacker.territory.TypePlayer);
             
                otherTerritory.war = true;
                otherTerritory.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }
            
        }


    }

    public void ActivateContextMenu(TerritoryHandler territoryToAttack, bool canAttack, bool isWar, Vector3 mousePosition)
    {
        TurnOnBlock();
        contextMenu.SetActive(true);
        ChangeStateTerritory(2);
        Vector3 mousePosCamera = Camera.main.ScreenToWorldPoint(mousePosition);
        contextMenu.transform.position = new Vector3(mousePosCamera.x, mousePosCamera.y, contextMenu.transform.position.z);
        contextMenu.GetComponent<ContextMenu>().SetMenu(canAttack, isWar, territoryToAttack);
    }
    public void ImproveSpeedPopulationButton()
    {
        ImproveSpeedPopulation(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>());
        //goldNeedSpeed += 2;
        UpdateMenu();
    }
    public void ImproveLimitButton()
    {

        ImproveLimit(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>());
        //foodNeedLimit += 3;
        UpdateMenu();
    }
    public void ImproveMineGoldButton()
    {

        ImproveMineGold(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>());
        UpdateMenu();
    }
    public void ImproveSacredPlaceButton()
    {

        ImproveSacredPlace(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>());
        UpdateMenu();
    }
    public void ImproveFortressButton()
    {

        ImproveFortress(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>());
        UpdateMenu();
    }
    public void ImproveBarracksButton()
    {

        ImproveBarracks(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>());
        UpdateMenu();
    }

    private void ImproveSpeedPopulation(TerritoryHandler territoryHandler)
    {
        if (goldPlayer >= goldNeedSpeed)
        {
            territoryHandler.ImproveSpeedPopulation();
            goldPlayer -= goldNeedSpeed;
            ShowFloatingText("+0.3 veloc", "TextMesh", territoryHandler.transform);
            ShowFloatingText("-"+goldNeedSpeed.ToString(), "TextFloating", goldAnimation);
        }
        
    }
    private void ImproveLimit(TerritoryHandler territoryHandler)
    {
        if (foodPlayer >= foodNeedLimit)
        {
            territoryHandler.ImproveLimit();
            foodPlayer -= foodNeedLimit;
            ShowFloatingText("+20 limit", "TextMesh", territoryHandler.transform);
            ShowFloatingText("-" + foodNeedLimit.ToString(), "TextFloating", foodAnimation);
        }
    }
    private void ImproveMineGold(TerritoryHandler territoryHandler)
    {
        if (goldPlayer >= territoryHandler.territoryStats.territory.GoldMineTerritory.CostToUpgrade)
        {
            territoryHandler.ImproveTerritory(countdownImages,buttons,0);
            goldPlayer -= territoryHandler.territoryStats.territory.GoldMineTerritory.CostToUpgrade;
            ShowFloatingText("+0.6 vel gold", "TextMesh", territoryHandler.transform);
            ShowFloatingText("-" + territoryHandler.territoryStats.territory.GoldMineTerritory.CostToUpgrade.ToString(), "TextFloating", goldAnimation);
            territoryHandler.territoryStats.territory.GoldMineTerritory.CostToUpgrade += 3;
        }
    }

    private void ImproveSacredPlace(TerritoryHandler territoryHandler)
    {
        if (goldPlayer >= territoryHandler.territoryStats.territory.SacredPlaceTerritory.CostToUpgrade)
        {
            territoryHandler.ImproveTerritory(countdownImages, buttons, 1);
            goldPlayer -= territoryHandler.territoryStats.territory.SacredPlaceTerritory.CostToUpgrade;
            ShowFloatingText("+0.6 motivation", "TextMesh", territoryHandler.transform);
            ShowFloatingText("-" + territoryHandler.territoryStats.territory.SacredPlaceTerritory.CostToUpgrade.ToString(), "TextFloating", goldAnimation);
            territoryHandler.territoryStats.territory.SacredPlaceTerritory.CostToUpgrade += 3;
        }
    }

    private void ImproveFortress(TerritoryHandler territoryHandler)
    {
        if (goldPlayer >= territoryHandler.territoryStats.territory.FortressTerritory.CostToUpgrade)
        {
            territoryHandler.ImproveTerritory(countdownImages, buttons, 2);
            goldPlayer -= territoryHandler.territoryStats.territory.FortressTerritory.CostToUpgrade;
            ShowFloatingText("+1 defense", "TextMesh", territoryHandler.transform);
            ShowFloatingText("-" + territoryHandler.territoryStats.territory.FortressTerritory.CostToUpgrade.ToString(), "TextFloating", goldAnimation);
            territoryHandler.territoryStats.territory.FortressTerritory.CostToUpgrade += 3;
        }
    }

    private void ImproveBarracks(TerritoryHandler territoryHandler)
    {
        if (goldPlayer >= territoryHandler.territoryStats.territory.BarracksTerritory.CostToUpgrade)
        {
            territoryHandler.ImproveTerritory(countdownImages, buttons, 3);
            goldPlayer -= territoryHandler.territoryStats.territory.BarracksTerritory.CostToUpgrade;
            ShowFloatingText("+1 attack", "TextMesh", territoryHandler.transform);
            ShowFloatingText("-" + territoryHandler.territoryStats.territory.BarracksTerritory.CostToUpgrade.ToString(), "TextFloating", goldAnimation);
            territoryHandler.territoryStats.territory.BarracksTerritory.CostToUpgrade += 3;
        }
    }

    private int GatherGold(TerritoryHandler territoryHandler)
    {
        int gatherGold = territoryHandler.territoryStats.territory.Gold;
        territoryHandler.GatherTerritoryGold();
        return gatherGold;
    }
    private int GatherFood(TerritoryHandler territoryHandler)
    {
        int gatherFood = territoryHandler.territoryStats.territory.FoodReward;
        territoryHandler.GatherTerritoryFood();
        return gatherFood;
    }

    void ShowFloatingText(string text,string namePrefab,Transform _t)
    {
        GameObject prefab = Resources.Load("Prefabs/MenuPrefabs/"+namePrefab) as GameObject;
        GameObject go= null;
        if (namePrefab == "TextFloating")
        {
            go = Instantiate(prefab, FindObjectOfType<Canvas>().transform);
            go.transform.SetParent(_t.transform);
            go.GetComponent<Text>().text = text;
        }else if (namePrefab == "TextMesh")
        {
            go = Instantiate(prefab);
            go.transform.SetParent(GameObject.Find("StatsContainer").transform, false);
            go.transform.GetComponentInChildren<Text>().text = text;
        }
        go.transform.position = _t.position;
        Destroy(go, 1.2f);
        Resources.UnloadUnusedAssets();
    }
    public void GatherGoldResourceButton()
    {
        int temp = 0;
        for (int i = 0; i < TerritoryManager.instance.GetTerritoriesByTypePlayer(Territory.TYPEPLAYER.PLAYER).Count; i++)
        {
            temp+= GatherGold(TerritoryManager.instance.GetTerritoriesByTypePlayer(Territory.TYPEPLAYER.PLAYER)[i]);
        }
        ShowFloatingText("+"+ temp.ToString(), "TextFloating",goldAnimation);
        goldPlayer += temp;
    }
    public void GatherFoodResourceButton()
    {
        int temp = 0;
        for (int i = 0; i < TerritoryManager.instance.GetTerritoriesByTypePlayer(Territory.TYPEPLAYER.PLAYER).Count; i++)
        {
            temp += GatherFood(TerritoryManager.instance.GetTerritoriesByTypePlayer(Territory.TYPEPLAYER.PLAYER)[i]);
        }
        ShowFloatingText("+" + temp.ToString(), "TextFloating",foodAnimation);
        foodPlayer += temp;
        
    }
    public void TurnOffBlock()
    {
        overMenuBlock.SetActive(false);
    }
    public void TurnOnBlock()
    {
        
        overMenuBlock.SetActive(true);
    }

    public void InstantiateCharacterOption(TerritoryHandler territory)
    {
        ml.AddDataMilitaryList(5);
        CharacterSelection.SetActive(true);
        characterOptions = new List<GameObject>();
        Transform gridLayout = CharacterSelection.transform.Find("ScrollArea/ScrollContainer/GridLayout").transform;
        foreach (MilitarBoss charac in ml.MilitarBosses)
        {
            GameObject characterOption = Instantiate(Resources.Load("Prefabs/MenuPrefabs/CharacterGameOption")) as GameObject;
            characterOption.transform.SetParent(gridLayout.transform, false);
            characterOption.name = charac.CharacterName;
            characterOption.transform.Find("Character/ImageCharacter").gameObject.GetComponent<Image>().sprite = charac.Picture;
            characterOption.transform.Find("Character/TextBackground/NameCharacter").gameObject.GetComponent<Text>().text = charac.CharacterName;
            characterOption.transform.Find("Description/OrigenCharacter").gameObject.GetComponent<Text>().text = charac.Origin;
            characterOption.transform.Find("Description/AgeCharacter").gameObject.GetComponent<Text>().text = charac.Age.ToString();
            characterOption.transform.Find("Description/CampainCharacter").gameObject.GetComponent<Text>().text = charac.Personality;
            characterOption.transform.Find("Description/StatsCharacter").gameObject.GetComponent<Text>().text = charac.Influence + " | " + charac.Opinion + " | " + charac.Experience + " | " + charac.StrategyType;
            characterOption.GetComponent<SelectCharacter>().SetTerritoryHandler(territory);
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
        ml.DeleteList();
    }
    public void EscapeGame()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
    }
    public void PauseGame()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    void Start()
    {
        AcceptEventButton.onClick.AddListener(() => AcceptCustomEventButton());
        DeclineEventButton.onClick.AddListener(() => DeclineCustomEventButton());
        CloseEventButton.onClick.AddListener(() => CloseCustomEventButton());
    }
    private CustomEvent currentCustomEvent;
    public void CustomEventAppearance(CustomEvent custom)
    {
        InitCustomEvent(custom);
        CustomEventSelection.gameObject.SetActive(true);
        AcceptEventButton.gameObject.SetActive(true);
        DeclineEventButton.gameObject.SetActive(true);
        DetailsTextCustomEvent.text = custom.MessageEvent;
        print(custom.MessageEvent);
    }
    
    public void WarningEventAppearance(CustomEvent custom)
    {
        InitCustomEvent(custom);
        CustomEventSelection.gameObject.SetActive(true);
        CloseEventButton.gameObject.SetActive(true);
        DetailsTextCustomEvent.text = "En "+ custom.TerritoryEvent + " " + custom.ElementEvent;
        DetailsTextCustomEvent.text += "\n";
        DetailsTextCustomEvent.text += "En " + custom.DaysToFinal + " dias te pediran esto:";
    }
    private void InitCustomEvent(CustomEvent custom)
    {
        PauseGame();
        ResetTextCustomEvent();
        currentCustomEvent = custom;
        TerritoryEventTxt.text = custom.TerritoryEvent;
        AcceptTextCustomEvent.text += "Beneficios \n " + custom.AcceptMessageEvent;
        DeclineTextCustomEvent.text += "Prejuicios \n " + custom.DeclineMessageEvent;
    }
    public void AcceptCustomEventButton()
    {
        currentCustomEvent.AcceptEventAction();
        UpdateMenu();
        CloseCustomEventButton();
    }
    public void DeclineCustomEventButton()
    {
        currentCustomEvent.DeclineEventAction();
        UpdateMenu();
        CloseCustomEventButton();
    }
    public void CloseCustomEventButton()
    {
        CustomEventSelection.gameObject.SetActive(false);
        ResetTextCustomEvent();
        ResumeGame();
    }
    private void ResetTextCustomEvent()
    {
        CloseEventButton.gameObject.SetActive(false);
        AcceptEventButton.gameObject.SetActive(false);
        DeclineEventButton.gameObject.SetActive(false);
        DetailsTextCustomEvent.text = " ";
        AcceptTextCustomEvent.text = " ";
        DeclineTextCustomEvent.text = " ";
    }
}
