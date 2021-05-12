using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private GameObject CustomEventList;

    [Header("Menu de movilizacion de tropas")]
    [SerializeField] public InputField warriorsCount;

    [Header("Menu territorio")]
    [SerializeField] private GameObject menuBlockTerritory;
    [SerializeField] private Text territoryName;
    [SerializeField] private Text goldCount;
    [SerializeField] private Text foodCount;
    [SerializeField] private Text GoldGeneration;
    [SerializeField] private Text FoodGeneration;
    [SerializeField] private Text MotivationBonus;
    [SerializeField] private Text AttackBonus;
    [SerializeField] private Text DefenseBonus;
    [SerializeField] private Image territoryImage;
    [SerializeField] private Image[] countdownImages;
    [SerializeField] private Button[] buttons;

    [SerializeField] private BuildOption IrrigationChannelOption;
    [SerializeField] private BuildOption GoldMineOption;
    [SerializeField] private BuildOption SacredPlaceOption;
    [SerializeField] private BuildOption FortressOption;
    [SerializeField] private BuildOption BarracksOption;

    [Header("Recursos")]
    [SerializeField] private Text goldGenerated;
    [SerializeField] private Text foodGenerated;
   // [SerializeField] private Text sucesionSizeTxt;
    [SerializeField] private Text scoreTxt;
    [SerializeField] private Transform goldAnimation;
    [SerializeField] private Transform foodAnimation;

    [Header("Menu de Pause")]
    [SerializeField] private GameObject PauseMenu;

    [Header("Evento")]
    [SerializeField] GameObject CurrentCaseMenu;
    [SerializeField] private GameObject CustomEventSelection;
    [SerializeField] private Text TitleTextCustomEvent;
    [SerializeField] private Text DetailsTextCustomEvent;
    [SerializeField] private Text AcceptTextCustomEvent;
    [SerializeField] private Text DeclineTextCustomEvent;
    [SerializeField] private Button AcceptEventButton;
    [SerializeField] private Button CloseEventButton;
    [SerializeField] private Button DeclineEventButton;
    private CustomEvent currentCustomEvent;
    [NonSerialized] public List<GameObject> listFloatingText = new List<GameObject>();

    [Header("Select MilitaryBoss variables")]
    [SerializeField] private GameObject CharacterSelection;
    

    private List<GameObject> characterOptions;
    //public MilitarBossList ml;
    public SubordinateList subordinateList;

    private int goldPlayer= 20;
    private int foodPlayer = 10;
    private int sucesionSizePlayer;
    private int scorePlayer;

    private float temporalTime;
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
    private void Awake()
    {
        instance = this;
        warriorsNumber = 0;
    }
    public void InstantiateEventListOption(CustomEventList customlist)
    {
        Transform gridLayout = CustomEventList.transform.Find("ScrollArea/ScrollContainer/GridLayout").transform;
        foreach (Transform child in gridLayout.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (CustomEvent customEvent in customlist.CustomEvents)
        {
            GameObject customEventOption = Instantiate(Resources.Load("Prefabs/MenuPrefabs/CustomEventOption")) as GameObject;
            customEventOption.transform.SetParent(gridLayout.transform, false);
            customEventOption.GetComponent<CustomEventOption>().Custom = customEvent;
            if (customEvent.StatusEvent == CustomEvent.STATUS.ANNOUNCE)
            {
                DestroyImmediate(customEventOption);
            }else if (customEvent.StatusEvent == CustomEvent.STATUS.FINISH)
            {
                Destroy(customEventOption,1);
            }
        }

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
        menuBlock.SetActive(false);
        MilitarBoss boss = selectedTerritory.MilitarBossTerritory;
        militaryBossName.text = "Name: " + boss.CharacterName;
        militaryBossPicture.sprite = boss.Picture;
        militaryBossExperience.text = "Experience: " + boss.Experience;
        militaryBossEstrategy.text = "Strategy: " + boss.StrategyType;
        militaryBossMilitary.text = "Influence: " + boss.Influence;
        GenerationSpeed.text = " " + selectedTerritory.VelocityPopulation;
        if (selectedTerritory.TypePlayer != Territory.TYPEPLAYER.PLAYER)
        {
            menuBlock.SetActive(true);                
        }
       
    }
    public void UpdateTerritoryMenu()
    {
        menuBlockTerritory.SetActive(false);
        territoryName.text = selectedTerritory.name;
        territoryImage.sprite = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().sprite.sprite;
        //territoryImage.sprite = selectedTerritory.
        if (selectedTerritory.TypePlayer != Territory.TYPEPLAYER.PLAYER)
        {
            menuBlockTerritory.SetActive(true);
            
        }
        
        
    }
    void UpdateCountDownImage()
    {
        IrrigationChannelOption.TerritoryBuilding = selectedTerritory.GoldMineTerritory;
        GoldMineOption.TerritoryBuilding = selectedTerritory.GoldMineTerritory;
        SacredPlaceOption.TerritoryBuilding = selectedTerritory.SacredPlaceTerritory;
        FortressOption.TerritoryBuilding = selectedTerritory.FortressTerritory;
        BarracksOption.TerritoryBuilding = selectedTerritory.BarracksTerritory;
        countdownImages[0].fillAmount = selectedTerritory.TotalTime[0] / selectedTerritory.IrrigationChannelTerritory.TimeToBuild;
        countdownImages[1].fillAmount = selectedTerritory.TotalTime[1] / selectedTerritory.GoldMineTerritory.TimeToBuild;
        countdownImages[2].fillAmount = selectedTerritory.TotalTime[2] / selectedTerritory.SacredPlaceTerritory.TimeToBuild;
        countdownImages[3].fillAmount = selectedTerritory.TotalTime[3] / selectedTerritory.FortressTerritory.TimeToBuild;
        countdownImages[4].fillAmount = selectedTerritory.TotalTime[4] / selectedTerritory.BarracksTerritory.TimeToBuild;
        for (int i = 0; i < 5; i++)
        {
            buttons[i].interactable = selectedTerritory.CanUpgrade[i];
        }
    }
    public void UpdateProfileMenu()
    {
        Governor temp = CharacterManager.instance.Governor;
        governorName.text = "Name: " + temp.CharacterName;
        governorAge.text = "Age: " + temp.Age.ToString();
        governorPicture.sprite = temp.Picture;
        governorOrigin.text = "Birth place: " + temp.Origin;
        governorDiplomacy.text = "Diplomacy: " + temp.Diplomacy;
        governorMilitancy.text = "Military: " + temp.Militancy;
        governorManagement.text = "Administration: " + temp.Managment;
        governorPrestige.text = "Prestige: " + temp.Prestige;
        governorPiety.text = "Piety: " + temp.Piety;
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
        militarWarriorsCount.text = selectedTerritory.Population.ToString() + " / " + selectedTerritory.LimitPopulation.ToString()+ " units" ;
        goldCount.text = "Gold: " + selectedTerritory.Gold.ToString();
        foodCount.text = "Food: " + selectedTerritory.FoodReward.ToString();
        GoldGeneration.text = "Generation speed: " + selectedTerritory.GoldMineTerritory.VelocityGold.ToString();
        FoodGeneration.text = "Generation speed: " + selectedTerritory.IrrigationChannelTerritory.VelocityFood.ToString();
        MotivationBonus.text = "Motivation bonus: " + selectedTerritory.SacredPlaceTerritory.Motivation.ToString();
        AttackBonus.text = "Attack bonus: " + selectedTerritory.FortressTerritory.PlusDefense.ToString();
        DefenseBonus.text = "Defense bonus: " + selectedTerritory.BarracksTerritory.PlusAttack.ToString();
        
        UpdateResourceTable();
        EscapeGame();
        UpdateCountDownImage();
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
        warriorsPrefab.GetComponent<WarriorsMoving>().SetAttack(otherTerritory.gameObject, _warriorsNumber,  selected);
        Instantiate(warriorsPrefab, selected.transform.position , Quaternion.identity);

    }
    public void MoveWarriors(TerritoryHandler otherTerritory, int attackPower, TerritoryHandler attacker)
    {
        Territory.TYPEPLAYER temp = otherTerritory.territoryStats.territory.TypePlayer;
        if (otherTerritory.territoryStats.territory.TypePlayer == attacker.territoryStats.territory.TypePlayer)
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
                WarManager.instance.AddWar(attackPower, otherTerritory.territoryStats.territory.Population, vAttack, vDef, otherTerritory, attacker.territoryStats.territory.TypePlayer);
             
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
    /*
    public void ImproveSpeedPopulationButton()
    {
        ImproveSpeedPopulation(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>());
        //goldNeedSpeed += 2;
        UpdateMenu();
    }
    */
    public void ImproveLimitButton()
    {

        ImproveLimit(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>());
        //foodNeedLimit += 3;
        UpdateMenu();
    }
    public void ImproveIrrigateChannelButton()
    {

        ImproveIrrigateChannel(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>());
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
    private void ImproveLimit(TerritoryHandler territoryHandler)
    {
        if (foodPlayer >= territoryHandler.territoryStats.territory.IrrigationChannelTerritory.CostToUpgrade)
        {
            territoryHandler.ImproveLimit();
            foodPlayer -= territoryHandler.territoryStats.territory.IrrigationChannelTerritory.CostToUpgrade;
            ShowFloatingText("+20 limit", "TextMesh", territoryHandler.transform, new Color32(0, 19, 152, 255));
            ShowFloatingText("-" + territoryHandler.territoryStats.territory.IrrigationChannelTerritory.CostToUpgrade.ToString(), "TextFloating", foodAnimation, Color.white);
            territoryHandler.territoryStats.territory.IrrigationChannelTerritory.CostToUpgrade += 10;
        }
    }
    private void ImproveIrrigateChannel(TerritoryHandler territoryHandler)
    {
        if (goldPlayer >= territoryHandler.territoryStats.territory.IrrigationChannelTerritory.CostToUpgrade)
        {
            ImproveTerritory(territoryHandler, 0);
            goldPlayer -= territoryHandler.territoryStats.territory.IrrigationChannelTerritory.CostToUpgrade;
            ShowFloatingText("+0.3 velocity population", "TextMesh", territoryHandler.transform, new Color32(0, 19, 152, 255));
            ShowFloatingText("-" + territoryHandler.territoryStats.territory.CostPopulation.ToString(), "TextFloating", goldAnimation, Color.white);
            territoryHandler.territoryStats.territory.IrrigationChannelTerritory.CostToUpgrade += 3;
        }
    }

    private void ImproveMineGold(TerritoryHandler territoryHandler)
    {
        if (goldPlayer >= territoryHandler.territoryStats.territory.GoldMineTerritory.CostToUpgrade)
        {
            ImproveTerritory(territoryHandler, 1);
            goldPlayer -= territoryHandler.territoryStats.territory.GoldMineTerritory.CostToUpgrade;
            ShowFloatingText("+1 gold mine level", "TextMesh", territoryHandler.transform, new Color32(0, 19, 152, 255));
            ShowFloatingText("-" + territoryHandler.territoryStats.territory.GoldMineTerritory.CostToUpgrade.ToString(), "TextFloating", goldAnimation, Color.white);
            territoryHandler.territoryStats.territory.GoldMineTerritory.CostToUpgrade += 3;
        }
    }
    private void ImproveSacredPlace(TerritoryHandler territoryHandler)
    {
        if (goldPlayer >= territoryHandler.territoryStats.territory.SacredPlaceTerritory.CostToUpgrade)
        {
            ImproveTerritory(territoryHandler, 2);
            goldPlayer -= territoryHandler.territoryStats.territory.SacredPlaceTerritory.CostToUpgrade;
            ShowFloatingText("+1 sacredPlace level", "TextMesh", territoryHandler.transform, new Color32(0, 19, 152, 255));
            ShowFloatingText("-" + territoryHandler.territoryStats.territory.SacredPlaceTerritory.CostToUpgrade.ToString(), "TextFloating", goldAnimation, Color.white);
            territoryHandler.territoryStats.territory.SacredPlaceTerritory.CostToUpgrade += 3;
        }
    }
    private void ImproveFortress(TerritoryHandler territoryHandler)
    {
        if (goldPlayer >= territoryHandler.territoryStats.territory.FortressTerritory.CostToUpgrade)
        {
            ImproveTerritory(territoryHandler, 3);
            goldPlayer -= territoryHandler.territoryStats.territory.FortressTerritory.CostToUpgrade;
            ShowFloatingText("+1 fortress level", "TextMesh", territoryHandler.transform, new Color32(0, 19, 152, 255));
            ShowFloatingText("-" + territoryHandler.territoryStats.territory.FortressTerritory.CostToUpgrade.ToString(), "TextFloating", goldAnimation, Color.white);
            territoryHandler.territoryStats.territory.FortressTerritory.CostToUpgrade += 3;
        }
    }
    private void ImproveBarracks(TerritoryHandler territoryHandler)
    {
        if (goldPlayer >= territoryHandler.territoryStats.territory.BarracksTerritory.CostToUpgrade)
        {
            ImproveTerritory(territoryHandler, 4);
            goldPlayer -= territoryHandler.territoryStats.territory.BarracksTerritory.CostToUpgrade;
            ShowFloatingText("+1 barracks level", "TextMesh", territoryHandler.transform, new Color32(0,19,152,255));
            ShowFloatingText("-" + territoryHandler.territoryStats.territory.BarracksTerritory.CostToUpgrade.ToString(), "TextFloating", goldAnimation,Color.white);
            territoryHandler.territoryStats.territory.BarracksTerritory.CostToUpgrade += 3;
        }
    }
    void ImproveTerritory(TerritoryHandler territoryHandler, int _option)
    {
        StartCoroutine(CountDownTimerCouroutine(territoryHandler,_option));
    }
    IEnumerator CountDownTimerCouroutine(TerritoryHandler territoryH, int option)
    {
        float duration = territoryH.CalculateDuration(option);
        //while (territoryH.totalTime <= duration)
        while (territoryH.territoryStats.territory.TotalTime[option] <= duration)
        {
            territoryH.territoryStats.territory.CanUpgrade[option] = false;
            territoryH.territoryStats.territory.TotalTime[option] += Time.deltaTime * GlobalVariables.instance.timeModifier / duration;
            yield return null;
        }
        territoryH.territoryStats.territory.TotalTime[option] = 0;
        territoryH.territoryStats.territory.CanUpgrade[option] = true;
        territoryH.ImproveBuildings(option);
        UpdateTerritoryMenu();
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


    public void ShowFloatingText(string text,string namePrefab,Transform _t, Color32 color)
    {
        GameObject prefab = Resources.Load("Prefabs/MenuPrefabs/"+namePrefab) as GameObject;
        
        GameObject go= null;
        if (namePrefab == "TextFloating")
        {
            go = Instantiate(prefab, FindObjectOfType<Canvas>().transform);
            go.transform.SetParent(_t.transform);
            go.GetComponent<Text>().text = text;
            go.GetComponent<Text>().color = color;
        }
        else if (namePrefab == "TextMesh")
        {
            go = Instantiate(prefab);
            go.transform.SetParent(GameObject.Find("StatsContainer").transform, false);
            go.transform.GetComponentInChildren<Text>().text = text;
            go.transform.GetComponentInChildren<Text>().color = color;
            listFloatingText.Add(go);
        }
        go.transform.position = _t.position;
        StartCoroutine(ResetGameObjects(go));
        Resources.UnloadUnusedAssets();
    }
    IEnumerator ResetGameObjects(GameObject go)
    {
        yield return new WaitForSeconds(1.2f);
        Destroy(go);
        listFloatingText.Clear();
    }
    public void GatherGoldResourceButton()
    {
        int temp = 0;
        for (int i = 0; i < TerritoryManager.instance.GetTerritoriesByTypePlayer(Territory.TYPEPLAYER.PLAYER).Count; i++)
        {
            temp+= GatherGold(TerritoryManager.instance.GetTerritoriesByTypePlayer(Territory.TYPEPLAYER.PLAYER)[i]);
        }
        ShowFloatingText("+"+ temp.ToString(), "TextFloating",goldAnimation, Color.white);
        goldPlayer += temp;
    }
    public void GatherFoodResourceButton()
    {
        int temp = 0;
        for (int i = 0; i < TerritoryManager.instance.GetTerritoriesByTypePlayer(Territory.TYPEPLAYER.PLAYER).Count; i++)
        {
            temp += GatherFood(TerritoryManager.instance.GetTerritoriesByTypePlayer(Territory.TYPEPLAYER.PLAYER)[i]);
        }
        ShowFloatingText("+" + temp.ToString(), "TextFloating",foodAnimation,Color.white);
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
    public void OpenCurrentCaseMenu(TerritoryHandler territoryHandler)
    {
        CurrentCaseMenu.SetActive(true);
        CurrentCaseMenu.transform.Find("Territory").GetComponent<Image>().sprite = territoryHandler.sprite.sprite;
        CurrentCaseMenu.transform.Find("Title").GetComponent<Text>().text= "You just won the battle of " + territoryHandler.territoryStats.territory.name;
        InstantiateCharacterOption(territoryHandler,"militar");
    }
    public void CloseCurrentCaseMenu()
    {
        CurrentCaseMenu.SetActive(false);
    }
    public void InstantiateCharacterOption(TerritoryHandler territoryHandler,string type)
    {
        subordinateList.AddDataSubordinateToList(3, type);
        CharacterSelection.SetActive(true);
        characterOptions = new List<GameObject>();
        Text instructionText = CharacterSelection.transform.Find("InstructionText").transform.GetComponent<Text>();
        instructionText.text = "Select a "+type+" Chief to " + territoryHandler.territoryStats.territory.name;
        Transform gridLayout = CharacterSelection.transform.Find("ScrollArea/ScrollContainer/GridLayout").transform;
        foreach (Subordinate charac in subordinateList.MilitarBosses)
        {
            GameObject characterOption = Instantiate(Resources.Load("Prefabs/MenuPrefabs/CharacterGameOption")) as GameObject;
            characterOption.transform.SetParent(gridLayout.transform, false);
            characterOption.name = charac.CharacterName;
            characterOption.GetComponent<CharacterOption>().Type = type;
            characterOption.GetComponent<CharacterOption>().Character = charac;
            characterOption.GetComponent<CharacterOption>().territoryHandlerInCharacter =  territoryHandler;
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
    public static bool isGamePaused = false;
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
    private void PauseGame()
    {
        turnOffMenus();
        temporalTime = GlobalVariables.instance.timeModifier;
        GlobalVariables.instance.timeModifier = 0;
        
    }
    private void ResumeGame()
    {
        GlobalVariables.instance.timeModifier = temporalTime;
        print(temporalTime);
    }
    void Start()
    {
        AcceptEventButton.onClick.AddListener(() => AcceptCustomEventButton());
        DeclineEventButton.onClick.AddListener(() => DeclineCustomEventButton());
        CloseEventButton.onClick.AddListener(() => CloseCustomEventButton());
        buttons[0].onClick.AddListener(() => ImproveIrrigateChannelButton());
        buttons[1].onClick.AddListener(() => ImproveMineGoldButton());
        buttons[2].onClick.AddListener(() => ImproveSacredPlaceButton());
        buttons[3].onClick.AddListener(() => ImproveFortressButton());
        buttons[4].onClick.AddListener(() => ImproveBarracksButton());
        InitButtonMenuPause();
        UpdateMenu();
    }

    public void FinishCustomEventAppearance(CustomEvent custom)
    {
        InitCustomEvent(custom);
        AcceptEventButton.gameObject.SetActive(false);
        DeclineEventButton.gameObject.SetActive(false);
        AcceptTextCustomEvent.gameObject.SetActive(false);
        TitleTextCustomEvent.text = "End Event";
        //DetailsTextCustomEvent.text += custom.MessageEventA + custom.ElementEvent;
        DetailsTextCustomEvent.text = "You were unable to complete the requirements of the " + custom.TerritoryEvent.name+" territory petition.";
        custom.DeclineEventAction();
    }
    
    public void WarningEventAppearance(CustomEvent custom, int daysToFinal)
    {
        InitCustomEvent(custom);
        CloseEventButton.gameObject.SetActive(true);
        DetailsTextCustomEvent.text += custom.MessageEvent + daysToFinal + " days left";

    }
    public void InitCustomEvent(CustomEvent custom)
    {
        CustomEventSelection.gameObject.SetActive(true);
        PauseGame();
        ResetTextCustomEvent();
        currentCustomEvent = custom;
        //DetailsTextCustomEvent.text = "The people of " + custom.TerritoryEvent.name + " territory give you a message:\n";
        AcceptTextCustomEvent.text += "If you accept: \n " + custom.AcceptMessageEvent;
        DeclineTextCustomEvent.text += "If you decline: \n " + custom.DeclineMessageEvent;
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
        InstantiateEventListOption(TimeSystem.instance.listEvents);
        ResumeGame();
    }
    private void ResetTextCustomEvent()
    {
        AcceptEventButton.gameObject.SetActive(true);
        DeclineEventButton.gameObject.SetActive(true);
        CloseEventButton.gameObject.SetActive(true);
        AcceptTextCustomEvent.gameObject.SetActive(true);
        DetailsTextCustomEvent.text = " ";
        AcceptTextCustomEvent.text = " ";
        DeclineTextCustomEvent.text = " ";
        TitleTextCustomEvent.text = "Event";
    }
    public void turnOffMenus()
    {
        GameObject[] overMenus;
        overMenus = GameObject.FindGameObjectsWithTag("OverMenu");
        foreach (GameObject overMenu in overMenus)
        {
            overMenu.SetActive(false);
        }
        ChangeStateTerritory(0);
    }
    void InitButtonMenuPause()
    {
        Button[] allPauseButton = PauseMenu.gameObject.transform.GetComponentsInChildren<Button>();

        allPauseButton[0].onClick.AddListener(() => ResumeMenuGame());
        allPauseButton[1].onClick.AddListener(() => GlobalVariables.instance.GoToMenuGame());
        allPauseButton[2].onClick.AddListener(() => GlobalVariables.instance.ClosingApp());
    }

}
