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
    [SerializeField] private GameObject CustomEventList;
    [Header("Menu de movilizacion de tropas")]
    [SerializeField] public InputField warriorsCount;
    [Header("Menu territorio")]
    [SerializeField] private GameObject menuBlockTerritory;
    [SerializeField] private Text territoryName;
    [SerializeField] private Text goldCount;
    [SerializeField] private Image territoryImage;
    [SerializeField] private Image[] countdownImages;
    [SerializeField] private Button[] buttons;

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

    [Header("Evento")]
    [SerializeField] GameObject CurrentCaseMenu;
    [SerializeField] private GameObject CustomEventSelection;
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
    private int goldPlayer= 50;
    private int foodPlayer = 20;
    private int sucesionSizePlayer;
    private int scorePlayer;
    //private int goldNeedSpeed = 10;
    //private int foodNeedLimit = 10;
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
    private void Awake()
    {
        instance = this;
        warriorsNumber = 0;
    }

    public void InstantiateEventOption()
    {
        characterOptions = new List<GameObject>();
        Transform gridLayout = CustomEventList.transform.Find("ScrollArea/ScrollContainer/GridLayout").transform;
        foreach (Transform child in gridLayout.transform)
        {
            Destroy(child.gameObject);
        }
        GameObject customEventOption = Instantiate(Resources.Load("Prefabs/MenuPrefabs/CustomEventOption")) as GameObject;
        customEventOption.transform.SetParent(gridLayout.transform, false);
//        customEventOption.name = "Evento_"+i;
        customEventOption.GetComponent<CustomEventOption>().Custom = TimeSystem.instance.listEvents.CustomEvents[TimeSystem.instance.IndexListEvent];
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
        GoldMineOption.TerritoryBuilding = selectedTerritory.GoldMineTerritory;
        SacredPlaceOption.TerritoryBuilding = selectedTerritory.SacredPlaceTerritory;
        FortressOption.TerritoryBuilding = selectedTerritory.FortressTerritory;
        BarracksOption.TerritoryBuilding = selectedTerritory.BarracksTerritory;
        countdownImages[0].fillAmount = selectedTerritory.TotalTime[0] / selectedTerritory.GoldMineTerritory.TimeToBuild;
        countdownImages[1].fillAmount = selectedTerritory.TotalTime[1] / selectedTerritory.SacredPlaceTerritory.TimeToBuild;
        countdownImages[2].fillAmount = selectedTerritory.TotalTime[2] / selectedTerritory.FortressTerritory.TimeToBuild;
        countdownImages[3].fillAmount = selectedTerritory.TotalTime[3] / selectedTerritory.BarracksTerritory.TimeToBuild;
        for (int i = 0; i < 4; i++)
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
        goldCount.text = "Gold: " + selectedTerritory.Gold.ToString() + "   Food: " + selectedTerritory.FoodReward.ToString();
        goldCount.text += "\nGeneration speed: " + selectedTerritory.GoldMineTerritory.VelocityGold.ToString();
        goldCount.text += "\nMotivation bonus: " + selectedTerritory.SacredPlaceTerritory.Motivation.ToString();
        goldCount.text += "\nAttack bonus: " + selectedTerritory.FortressTerritory.PlusDefense.ToString();
        goldCount.text += "\nDefense bonus: " + selectedTerritory.BarracksTerritory.PlusAttack.ToString();
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
        if (goldPlayer >= territoryHandler.territoryStats.territory.CostPopulation)
        {
            territoryHandler.ImproveSpeedPopulation();
            //int t = (int)((territoryHandler.territoryStats.territory.VelocityPopulation-1)/0.3);
            //t++;
            goldPlayer -= territoryHandler.territoryStats.territory.CostPopulation;
            ShowFloatingText("+0.3 velocity population", "TextMesh", territoryHandler.transform);
            ShowFloatingText("-"+ territoryHandler.territoryStats.territory.CostPopulation.ToString(), "TextFloating", goldAnimation);
            territoryHandler.territoryStats.territory.CostPopulation += 10;
        }
        
    }
    private void ImproveLimit(TerritoryHandler territoryHandler)
    {
        if (foodPlayer >= territoryHandler.territoryStats.territory.CostFood)
        {
            territoryHandler.ImproveLimit();
            foodPlayer -= territoryHandler.territoryStats.territory.CostFood;
            ShowFloatingText("+20 limit", "TextMesh", territoryHandler.transform);
            ShowFloatingText("-" + territoryHandler.territoryStats.territory.CostFood.ToString(), "TextFloating", foodAnimation);
            territoryHandler.territoryStats.territory.CostFood += 10;
        }
    }
    private void ImproveMineGold(TerritoryHandler territoryHandler)
    {
        if (goldPlayer >= territoryHandler.territoryStats.territory.GoldMineTerritory.CostToUpgrade)
        {
            // territoryHandler.ImproveTerritory(countdownImages,buttons,0);
            ImproveTerritory(territoryHandler, 0);
            goldPlayer -= territoryHandler.territoryStats.territory.GoldMineTerritory.CostToUpgrade;
            ShowFloatingText("+1 gold mine level", "TextMesh", territoryHandler.transform);
            ShowFloatingText("-" + territoryHandler.territoryStats.territory.GoldMineTerritory.CostToUpgrade.ToString(), "TextFloating", goldAnimation);
            territoryHandler.territoryStats.territory.GoldMineTerritory.CostToUpgrade += 3;
        }
    }

    private void ImproveSacredPlace(TerritoryHandler territoryHandler)
    {
        if (goldPlayer >= territoryHandler.territoryStats.territory.SacredPlaceTerritory.CostToUpgrade)
        {
            //   territoryHandler.ImproveTerritory(countdownImages, buttons, 1);
            ImproveTerritory(territoryHandler, 1);
            goldPlayer -= territoryHandler.territoryStats.territory.SacredPlaceTerritory.CostToUpgrade;
            ShowFloatingText("+1 sacredPlace level", "TextMesh", territoryHandler.transform);
            ShowFloatingText("-" + territoryHandler.territoryStats.territory.SacredPlaceTerritory.CostToUpgrade.ToString(), "TextFloating", goldAnimation);
            territoryHandler.territoryStats.territory.SacredPlaceTerritory.CostToUpgrade += 3;
        }
    }

    private void ImproveFortress(TerritoryHandler territoryHandler)
    {
        if (goldPlayer >= territoryHandler.territoryStats.territory.FortressTerritory.CostToUpgrade)
        {
            // territoryHandler.ImproveTerritory(countdownImages, buttons, 2);
            ImproveTerritory(territoryHandler, 2);
            goldPlayer -= territoryHandler.territoryStats.territory.FortressTerritory.CostToUpgrade;
            ShowFloatingText("+1 fortress level", "TextMesh", territoryHandler.transform);
            ShowFloatingText("-" + territoryHandler.territoryStats.territory.FortressTerritory.CostToUpgrade.ToString(), "TextFloating", goldAnimation);
            territoryHandler.territoryStats.territory.FortressTerritory.CostToUpgrade += 3;
        }
    }

    private void ImproveBarracks(TerritoryHandler territoryHandler)
    {
        if (goldPlayer >= territoryHandler.territoryStats.territory.BarracksTerritory.CostToUpgrade)
        {
            //territoryHandler.ImproveTerritory(countdownImages, buttons, 3);
            ImproveTerritory(territoryHandler, 3);
            goldPlayer -= territoryHandler.territoryStats.territory.BarracksTerritory.CostToUpgrade;
            ShowFloatingText("+1 barracks level", "TextMesh", territoryHandler.transform);
            ShowFloatingText("-" + territoryHandler.territoryStats.territory.BarracksTerritory.CostToUpgrade.ToString(), "TextFloating", goldAnimation);
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
            territoryH.territoryStats.territory.TotalTime[option] += Time.deltaTime / duration;
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

    public void OpenCurrentCaseMenu(TerritoryHandler territoryHandler)
    {
        CurrentCaseMenu.SetActive(true);
        CurrentCaseMenu.transform.Find("Title").GetComponent<Text>().text= "You just won the battle of " + territoryHandler.territoryStats.territory.name;
        InstantiateCharacterOption(territoryHandler);
    }
    public void CloseCurrentCaseMenu()
    {
        CurrentCaseMenu.SetActive(false);
    }
    public void InstantiateCharacterOption(TerritoryHandler territoryHandler)
    {
        ml.AddDataMilitaryList(3);
        CharacterSelection.SetActive(true);
        characterOptions = new List<GameObject>();
        Text instructionText = CharacterSelection.transform.Find("InstructionText").transform.GetComponent<Text>();
        instructionText.text = "Select a military Chief \n to " + territoryHandler.territoryStats.territory.name;
        Transform gridLayout = CharacterSelection.transform.Find("ScrollArea/ScrollContainer/GridLayout").transform;
        foreach (MilitarBoss charac in ml.MilitarBosses)
        {
            GameObject characterOption = Instantiate(Resources.Load("Prefabs/MenuPrefabs/CharacterGameOption")) as GameObject;
            characterOption.transform.SetParent(gridLayout.transform, false);
            characterOption.name = charac.CharacterName;
            characterOption.transform.Find("Character/ImageCharacter").gameObject.GetComponent<Image>().sprite = charac.Picture;
            characterOption.transform.Find("Character/TextBackground/NameCharacter").gameObject.GetComponent<Text>().text = charac.CharacterName;
            characterOption.transform.Find("Description/OrigenCharacter").gameObject.GetComponent<Text>().text = "From: " + charac.Origin;
            characterOption.transform.Find("Description/AgeCharacter").gameObject.GetComponent<Text>().text = "Age: " + charac.Age.ToString();
            characterOption.transform.Find("Description/CampainCharacter").gameObject.GetComponent<Text>().text = "Personality: " +charac.Personality.ToLower();
            characterOption.transform.Find("Description/ExperienceCharacter").gameObject.GetComponent<Text>().text = "Experience: " + charac.Experience.ToString();
            characterOption.transform.Find("Description/OpinionCharacter").gameObject.GetComponent<Text>().text = "Opinion: " + charac.Opinion.ToString();
            characterOption.transform.Find("Description/InfluenceCharacter").gameObject.GetComponent<Text>().text = "Influence" + charac.Influence.ToString();
            characterOption.transform.Find("Description/StrategyTypeCharacter").gameObject.GetComponent<Text>().text = "Strategy:" + charac.StrategyType.ToLower();
            characterOption.GetComponent<SelectCharacter>().TerritoryHandler =  territoryHandler;
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
        turnOffMenus();
        Time.timeScale = 0;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
    void Start()
    {
      //  InstantiateEventOption();
        AcceptEventButton.onClick.AddListener(() => AcceptCustomEventButton());
        DeclineEventButton.onClick.AddListener(() => DeclineCustomEventButton());
        CloseEventButton.onClick.AddListener(() => CloseCustomEventButton());
        UpdateMenu();
    }
    private CustomEvent currentCustomEvent;
    public void CustomEventAppearance(CustomEvent custom)
    {
        InitCustomEvent(custom);
        CustomEventSelection.gameObject.SetActive(true);
        AcceptEventButton.gameObject.SetActive(true);
        DeclineEventButton.gameObject.SetActive(true);
        DetailsTextCustomEvent.text += custom.MessageEventA + custom.ElementEvent;
        //print(custom.MessageEvent);
    }
    
    public void WarningEventAppearance(CustomEvent custom)
    {
        InitCustomEvent(custom);
        CustomEventSelection.gameObject.SetActive(true);
        CloseEventButton.gameObject.SetActive(true);
        DetailsTextCustomEvent.text += "In " + custom.DaysToFinal + " days, "+ custom.MessageEventB + custom.ElementEvent;
        
    }
    private void InitCustomEvent(CustomEvent custom)
    {
        PauseGame();
        ResetTextCustomEvent();
        currentCustomEvent = custom;
        DetailsTextCustomEvent.text = "The people of " + custom.TerritoryEvent.name + " territory give you a message:\n";
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
    public void turnOffMenus()
    {
        GameObject[] overMenus;
        overMenus = GameObject.FindGameObjectsWithTag("OverMenu");
        foreach (GameObject overMenu in overMenus)
        {
            overMenu.SetActive(false);
        }
        InGameMenuHandler.instance.ChangeStateTerritory(0);
    }
}
