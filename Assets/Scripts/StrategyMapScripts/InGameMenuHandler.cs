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

    [SerializeField] private Territory selectedTerritory;
    [NonSerialized] public List<GameObject> listFloatingText = new List<GameObject>();

    [Header("Menu military")]
    [SerializeField] private GameObject menuBlockWarriors;
    [SerializeField] private GameObject menuBlockCharacter;
    [SerializeField] private Button selectMilitarChief;
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
    [SerializeField] private TextMeshProUGUI territoryEmpire;
    [SerializeField] private TextMeshProUGUI territoryRegion;
    [SerializeField] private TextMeshProUGUI territoryReligion;
    [SerializeField] private TextMeshProUGUI goldCount;
    [SerializeField] private TextMeshProUGUI foodCount;
    [SerializeField] private TextMeshProUGUI GoldGeneration;
    [SerializeField] private TextMeshProUGUI FoodGeneration;
    [SerializeField] private TextMeshProUGUI MotivationBonus;
    [SerializeField] private TextMeshProUGUI AttackBonus;
    [SerializeField] private TextMeshProUGUI DefenseBonus;

    [Header("Menu buildings")]
    [SerializeField] private GameObject menuBlockBuildings;
    [SerializeField] private BuildOption IrrigationChannelOption;
    [SerializeField] private BuildOption GoldMineOption;
    [SerializeField] private BuildOption SacredPlaceOption;
    [SerializeField] private BuildOption FortressOption;
    [SerializeField] private BuildOption BarracksOption;

    //resources
    private int goldPlayer = 30;
    private int foodPlayer = 30;
    private int sucesionSizePlayer;
    private int scorePlayer;
    public Territory TerritorySelected
    {
        get { return selectedTerritory; }
    }
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
        InitButtons();
        UpdateMenu();
    }
    /// <summary>
    /// Update all elements of the militar menu of the territory-selected
    /// if the territory-selected doesn't belong to the player 
    /// it blocks
    /// </summary>
    private void UpdateMilitarMenu()
    {
        menuBlockWarriors.SetActive(false);
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
            menuBlockWarriors.SetActive(true);
        }
        else
        {
            if (selectedTerritory.IsClaimed == false)
            {
                menuBlockCharacter.SetActive(true);
                menuBlockWarriors.SetActive(true);
            }
            else
            {
                menuBlockCharacter.SetActive(false);
                menuBlockWarriors.SetActive(false);
            }
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
        territoryEmpire.text = TerritoryManager.instance.GetTerritoryEmpire(selectedTerritory);
        territoryRegion.text = selectedTerritory.RegionTerritory.ToString().Split(char.Parse("_"))[0];
        MotivationBonus.text = selectedTerritory.SacredPlaceTerritory.Motivation.ToString() + "/10";
        AttackBonus.text = selectedTerritory.FortressTerritory.PlusDefense.ToString() + "/10";
        DefenseBonus.text = selectedTerritory.ArmoryTerritory.PlusAttack.ToString() + "/10";
        GoldGeneration.text = (selectedTerritory.GoldMineTerritory.WorkersMine / 5) + " every day";
        FoodGeneration.text = (selectedTerritory.IrrigationChannelTerritory.WorkersChannel / 5) + " every day";
        
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
        IrrigationChannelOption.InitializeBuildingOption(selectedTerritory.IrrigationChannelTerritory);
        GoldMineOption.InitializeBuildingOption(selectedTerritory.GoldMineTerritory);
        SacredPlaceOption.InitializeBuildingOption(selectedTerritory.SacredPlaceTerritory);
        FortressOption.InitializeBuildingOption(selectedTerritory.FortressTerritory);
        BarracksOption.InitializeBuildingOption(selectedTerritory.ArmoryTerritory);
    }
    /// <summary>
    /// Update all menus of the territory-selected
    /// </summary>
    public void UpdateMenu()
    {
        selectedTerritory = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territoryStats.territory;
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
        if (selectedTerritory.Population > selectedTerritory.LimitPopulation)
        {
            militarWarriorsCount.color = Color.red;
            militarWarriorsCount.GetComponent<MenuToolTip>().SetNewInfo("Population overflow \n " +
                                                                         "You cannot generate troops until you \n" +
                                                                         "have less than the population limit");
        }
        else
        {
            militarWarriorsCount.color = new Color32(50,50,50,255);
            militarWarriorsCount.GetComponent<MenuToolTip>().SetNewInfo("Actual quantity of warriors.");
        }
        goldCount.text = selectedTerritory.Gold.ToString();
        foodCount.text = selectedTerritory.FoodReward.ToString();
        GoldGeneration.text = (selectedTerritory.GoldMineTerritory.WorkersMine / 5) + " per day";
        FoodGeneration.text = (selectedTerritory.IrrigationChannelTerritory.WorkersChannel / 5) + " per day";
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
    public void ImproveSpeedPopulationButton()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ReadAndPlaySFX("upgrade");
        }
        ImproveSpeedPopulation(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>());
        UpdateMenu();
    }
    public void ImproveLimitButton()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ReadAndPlaySFX("upgrade");
        }
        ImproveLimitPopulation(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>());
        UpdateMenu();
    }
    public void ImproveBuildingButton(Building _building)
    {
        TerritoryHandler territoryHandler = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
        Building building = territoryHandler.GetBuilding(_building);
        ImproveBuildingInHandler(territoryHandler, building);
        TimeSystem.instance.AddEvent(territoryHandler, building);
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
            goldPlayer -= territoryHandler.GetBuilding(building).CostToUpgrade;
            territoryHandler.GetBuilding(building).ImproveCostUpgrade();
            ShowFloatingText("+1 " + building.Name + " level", "TextMesh", territoryHandler.transform, new Color32(0, 19, 152, 255));
            ShowFloatingText("-" + territoryHandler.GetBuilding(building).CostToUpgrade.ToString(), "TextFloating", ResourceTableHandler.instance.GoldAnimation, Color.white);
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
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ReadAndPlaySFX("coins");
        }
        int temp = 0;
        for (int i = 0; i < TerritoryManager.instance.GetTerritoriesHandlerByTypePlayer(Territory.TYPEPLAYER.PLAYER).Count; i++)
        {
            temp+= GatherGold(TerritoryManager.instance.GetTerritoriesHandlerByTypePlayer(Territory.TYPEPLAYER.PLAYER)[i]);
        }
        ShowFloatingText("+"+ temp.ToString(), "TextFloating", ResourceTableHandler.instance.GoldAnimation, Color.white);
        goldPlayer += temp;
    }
    public void GatherFoodResourceButton()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ReadAndPlaySFX("food");
        }
        int temp = 0;
        for (int i = 0; i < TerritoryManager.instance.GetTerritoriesHandlerByTypePlayer(Territory.TYPEPLAYER.PLAYER).Count; i++)
        {
            temp += GatherFood(TerritoryManager.instance.GetTerritoriesHandlerByTypePlayer(Territory.TYPEPLAYER.PLAYER)[i]);
        }
        ShowFloatingText("+" + temp.ToString(), "TextFloating", ResourceTableHandler.instance.FoodAnimation,Color.white);
        foodPlayer += temp;
    }
    private void InitButtons()
    {
        MilitarChief militarChief = new MilitarChief();
        selectMilitarChief.onClick.AddListener(() => MenuManager.instance.OpenSelectCharacterMenu(selectedTerritory, militarChief));
    }
}
