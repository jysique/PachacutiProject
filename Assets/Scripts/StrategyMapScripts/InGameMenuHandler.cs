using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuHandler : MonoBehaviour
{
    //instance
    public static InGameMenuHandler instance;

 
    //

    [SerializeField] private Territory selectedTerritory;
    [NonSerialized] public List<GameObject> listFloatingText = new List<GameObject>();

    [Header("Menu military")]
    [SerializeField] private GameObject menuBlock;
    [SerializeField] private Image militaryBossPicture;
    [SerializeField] private TextMeshProUGUI militarWarriorsCount;
    [SerializeField] private TextMeshProUGUI GenerationSpeed;
    [SerializeField] private TextMeshProUGUI warriorsLimit;
    [SerializeField] private TextMeshProUGUI militaryBossName;
    [SerializeField] private TextMeshProUGUI militaryBossExperience;
    [SerializeField] private TextMeshProUGUI militaryBossEstrategy;
    [SerializeField] private TextMeshProUGUI militaryBossInfluence;

    [Header("Menu territory")]
    [SerializeField] private GameObject menuBlockTerritory;
    [SerializeField] private TextMeshProUGUI goldCount;
    [SerializeField] private TextMeshProUGUI foodCount;
    [SerializeField] private TextMeshProUGUI GoldGeneration;
    [SerializeField] private TextMeshProUGUI FoodGeneration;
    [SerializeField] private TextMeshProUGUI MotivationBonus;
    [SerializeField] private TextMeshProUGUI AttackBonus;
    [SerializeField] private TextMeshProUGUI DefenseBonus;
    [Header("Menu buildings")]
    [SerializeField] private GameObject menuBlockBuildings;
    [SerializeField] private Image[] countdownImages;
    [SerializeField] private Button[] buttons;
    [SerializeField] private BuildOption IrrigationChannelOption;
    [SerializeField] private BuildOption GoldMineOption;
    [SerializeField] private BuildOption SacredPlaceOption;
    [SerializeField] private BuildOption FortressOption;
    [SerializeField] private BuildOption BarracksOption;

    
    //resources
    private int goldPlayer= 100;
    private int foodPlayer = 100;
    private int sucesionSizePlayer;
    private int scorePlayer;
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
    }
    void Start()
    {
        InitializeButtons();
        UpdateMenu();
    }
    /// <summary>
    /// Update all elements of the militar menu of the territory-selected
    /// if the territory-selected doesn't belong to the player 
    /// it blocks
    /// </summary>
    private void UpdateMilitarMenu()
    {
        menuBlock.SetActive(false);
        MilitarChief mChief = selectedTerritory.MilitarChiefTerritory;
        militaryBossName.text = mChief.CharacterName;
        militaryBossPicture.sprite = mChief.Picture;
        militaryBossExperience.text = mChief.Experience.ToString()+ "/10";
        militaryBossEstrategy.text = "Strategy: " + mChief.StrategyType;
        militaryBossInfluence.text = mChief.Influence.ToString() +"/10";
        GenerationSpeed.text = selectedTerritory.VelocityPopulation.ToString();
        warriorsLimit.text = selectedTerritory.LimitPopulation.ToString();
        if (selectedTerritory.TypePlayer != Territory.TYPEPLAYER.PLAYER)
        {
            menuBlock.SetActive(true);                
        }
    }
    /// <summary>
    /// Update all elements of the territory menu of the territory-selected
    /// if the territory-selected doesn't belong to the player 
    /// it blocks
    /// </summary>
    private void UpdateTerritoryMenu()
    {
        menuBlockTerritory.SetActive(false);
        //territoryImage.sprite = selectedTerritory.
        if (selectedTerritory.TypePlayer != Territory.TYPEPLAYER.PLAYER)
        {
            menuBlockTerritory.SetActive(true);
            
        }
    }
    /// <summary>
    /// Update all elements of the buildings menu of the territory-selected
    /// if the territory-selected doesn't belong to the player 
    /// it blocks
    /// </summary>
    private void UpdateBuildingsMenu()
    {
        menuBlockBuildings.SetActive(false);
        if (selectedTerritory.TypePlayer != Territory.TYPEPLAYER.PLAYER)
        {
            menuBlockBuildings.SetActive(true);
        }
    }
    /// <summary>
    /// Update all buildings of the territory-selected
    /// </summary>
    private void UpdateCountDownImage()
    {
        IrrigationChannelOption.TerritoryBuilding = selectedTerritory.IrrigationChannelTerritory;
        GoldMineOption.TerritoryBuilding = selectedTerritory.GoldMineTerritory;
        SacredPlaceOption.TerritoryBuilding = selectedTerritory.SacredPlaceTerritory;
        FortressOption.TerritoryBuilding = selectedTerritory.FortressTerritory;
        BarracksOption.TerritoryBuilding = selectedTerritory.ArmoryTerritory;
    }
    /// <summary>
    /// Update all menus of the territory-selected
    /// </summary>
    public void UpdateMenu()
    {
        //selectedTerritory = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territoryStats.territory;
        UpdateMilitarMenu();
        UpdateTerritoryMenu();
        UpdateBuildingsMenu();

    }
    /// <summary>
    /// Update all text depending of the territory-selected
    /// </summary>
    private void UpdateAllText()
    {
        militarWarriorsCount.text = selectedTerritory.Population.ToString() + " / " + selectedTerritory.LimitPopulation.ToString() + " units";
        goldCount.text = selectedTerritory.Gold.ToString();
        foodCount.text = selectedTerritory.FoodReward.ToString();
        GoldGeneration.text = (selectedTerritory.GoldMineTerritory.WorkersMine / 5) + " every day";
        FoodGeneration.text = (selectedTerritory.IrrigationChannelTerritory.WorkersChannel / 5) + " every day";
        MotivationBonus.text = selectedTerritory.SacredPlaceTerritory.Motivation.ToString() + "/10";
        AttackBonus.text = selectedTerritory.FortressTerritory.PlusDefense.ToString() + "/10";
        DefenseBonus.text = selectedTerritory.ArmoryTerritory.PlusAttack.ToString() + "/10";
    }
    void Update()
    {
        selectedTerritory = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territoryStats.territory;
        UpdateAllText();
        UpdateCountDownImage();
    }
    private void InitializeButtons()
    {
        buttons[0].onClick.AddListener(() => ImproveIrrigateChannelButton());
        buttons[1].onClick.AddListener(() => ImproveMineGoldButton());
        buttons[2].onClick.AddListener(() => ImproveSacredPlaceButton());
        buttons[3].onClick.AddListener(() => ImproveFortressButton());
        buttons[4].onClick.AddListener(() => ImproveBarracksButton());
    }
    public void ImproveSpeedPopulationButton()
    {
        ImproveSpeedPopulation(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>());
        UpdateMenu();
    }
    public void ImproveLimitButton()
    {
        ImproveLimitPopulation(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>());
        UpdateMenu();
    }
    public void ImproveIrrigateChannelButton()
    {
        TerritoryHandler territoryHandler = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
        ImproveBuildingInHandler(territoryHandler, territoryHandler.territoryStats.territory.IrrigationChannelTerritory);
        UpdateMenu();
    }
    public void ImproveMineGoldButton()
    {
        TerritoryHandler territoryHandler = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
        ImproveBuildingInHandler(territoryHandler, territoryHandler.territoryStats.territory.GoldMineTerritory);
        UpdateMenu();
    }
    public void ImproveSacredPlaceButton()
    {
        TerritoryHandler territoryHandler = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
        ImproveBuildingInHandler(territoryHandler, territoryHandler.territoryStats.territory.SacredPlaceTerritory);
        UpdateMenu();
    }
    public void ImproveFortressButton()
    {
        TerritoryHandler territoryHandler = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
        ImproveBuildingInHandler(territoryHandler, territoryHandler.territoryStats.territory.FortressTerritory);
        TimeSystem.instance.AddEvent(territoryHandler, territoryHandler.territoryStats.territory.FortressTerritory);
        UpdateMenu();
    }
    public void ImproveBarracksButton()
    {
        TerritoryHandler territoryHandler = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
        ImproveBuildingInHandler(territoryHandler, territoryHandler.territoryStats.territory.ArmoryTerritory);
        TimeSystem.instance.AddEvent(territoryHandler, territoryHandler.territoryStats.territory.ArmoryTerritory);
        UpdateMenu();
    }
    private void ImproveSpeedPopulation(TerritoryHandler territoryHandler)
    {
        if (goldPlayer >= territoryHandler.territoryStats.territory.CostSpeedPopulation)
        {
            territoryHandler.ImproveSpeedPopulation();
            goldPlayer -= territoryHandler.territoryStats.territory.CostSpeedPopulation;
            ShowFloatingText("+0.3 speed population", "TextMesh", territoryHandler.transform, new Color32(0, 19, 152, 255));
            ShowFloatingText("-" + territoryHandler.territoryStats.territory.CostSpeedPopulation.ToString(), "TextFloating", ResourceTableHandler.instance.GoldAnimation, Color.white);
            territoryHandler.territoryStats.territory.CostSpeedPopulation += territoryHandler.territoryStats.territory.AddCost;
        }

    }
    private void ImproveLimitPopulation(TerritoryHandler territoryHandler)
    {
        if (foodPlayer >= territoryHandler.territoryStats.territory.CostLimitPopulation)
        {
            territoryHandler.ImproveLimit();
            foodPlayer -= territoryHandler.territoryStats.territory.CostLimitPopulation;
            ShowFloatingText("+20 limit", "TextMesh", territoryHandler.transform, new Color32(0, 19, 152, 255));
            ShowFloatingText("-" + territoryHandler.territoryStats.territory.CostLimitPopulation.ToString(), "TextFloating", ResourceTableHandler.instance.FoodAnimation, Color.white);
            territoryHandler.territoryStats.territory.CostLimitPopulation += territoryHandler.territoryStats.territory.AddCost;
        }
    }
    private void ImproveBuildingInHandler(TerritoryHandler territoryHandler, Building building)
    {
        if (goldPlayer >= building.CostToUpgrade)
        {
            //ImproveTerritory(territoryHandler, territoryHandler.GetBuilding(building));
            TimeSystem.instance.AddEvent(territoryHandler, territoryHandler.GetBuilding(building));
            goldPlayer -= territoryHandler.GetBuilding(building).CostToUpgrade;
            ShowFloatingText("+1 "+building.Name+" level", "TextMesh", territoryHandler.transform, new Color32(0, 19, 152, 255));
            ShowFloatingText("-" + territoryHandler.GetBuilding(building).CostToUpgrade.ToString(), "TextFloating", ResourceTableHandler.instance.GoldAnimation, Color.white);
            territoryHandler.GetBuilding(building).ImproveCostUpgrade();
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
    public void ShowFloatingText(string text,string namePrefab,Transform _t, Color32 color,float posX = 0,float posY = 0)
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
        //go.transform.position = _t.position;
        go.transform.position = new Vector3(_t.position.x+ posX,_t.position.y + posY,_t.position.z);
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
        ShowFloatingText("+"+ temp.ToString(), "TextFloating", ResourceTableHandler.instance.GoldAnimation, Color.white);
        goldPlayer += temp;
    }
    public void GatherFoodResourceButton()
    {
        int temp = 0;
        for (int i = 0; i < TerritoryManager.instance.GetTerritoriesByTypePlayer(Territory.TYPEPLAYER.PLAYER).Count; i++)
        {
            temp += GatherFood(TerritoryManager.instance.GetTerritoriesByTypePlayer(Territory.TYPEPLAYER.PLAYER)[i]);
        }
        ShowFloatingText("+" + temp.ToString(), "TextFloating", ResourceTableHandler.instance.FoodAnimation,Color.white);
        foodPlayer += temp;
        
    }

}
