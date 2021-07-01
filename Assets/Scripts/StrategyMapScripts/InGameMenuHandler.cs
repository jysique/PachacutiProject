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

    [Header("State Menu")]
    [SerializeField] private TextMeshProUGUI irrigateLevel;
    [SerializeField] private TextMeshProUGUI goldMineLevel;
    [SerializeField] private TextMeshProUGUI fortressLevel;
    [SerializeField] private TextMeshProUGUI academyLevel;
    [SerializeField] private TextMeshProUGUI barracksLevel;
    [SerializeField] private TextMeshProUGUI archeryLevel;

    [Header("Menu military")]
    [SerializeField] private GameObject menuBlockWarriors;
    [SerializeField] private GameObject menuBlockCharacter;
    [SerializeField] private Button selectMilitarChief;
    [SerializeField] private Image militaryBossPicture;
    [SerializeField] private GameObject swordMenWarriors;
    [SerializeField] private GameObject lancersWarriors;
    [SerializeField] private GameObject archersWarriors;
    // [SerializeField] private TextMeshProUGUI militarWarriorsCount;
    // [SerializeField] private TextMeshProUGUI GenerationSpeed;
    // [SerializeField] private TextMeshProUGUI warriorsLimit;
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
    [SerializeField] private BuildOption FortressOption;
    [SerializeField] private BuildOption AcademyOption;
    [SerializeField] private BuildOption BarracksOption;
    [SerializeField] private BuildOption ArcheryOption;

    //resources
    private int goldPlayer = 500;
    private int foodPlayer = 500;
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
    private void UpdateStateMenu()
    {
        irrigateLevel.text = selectedTerritory.IrrigationChannelTerritory.Level.ToString();
        goldMineLevel.text = selectedTerritory.GoldMineTerritory.Level.ToString();
        fortressLevel.text = selectedTerritory.FortressTerritory.Level.ToString();
        academyLevel.text = selectedTerritory.AcademyTerritory.Level.ToString();
        barracksLevel.text = selectedTerritory.BarracksTerritory.Level.ToString();
        archeryLevel.text = selectedTerritory.ArcheryTerritory.Level.ToString();
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
        militaryBossEstrategy.text = GameMultiLang.GetTraduction("StrategyTitle") + mChief.StrategyType;
        militaryBossInfluence.text = mChief.Influence.ToString() +"/10";
    //    GenerationSpeed.text = selectedTerritory.VelocityPopulation.ToString();
    //    warriorsLimit.text = selectedTerritory.LimitPopulation.ToString();

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
        MotivationBonus.text = selectedTerritory.MotivationTerritory.ToString() + "/10";
     //   AttackBonus.text = selectedTerritory.FortressTerritory.PlusDefense.ToString() + "/10";
     //   DefenseBonus.text = selectedTerritory.ArmoryTerritory.PlusAttack.ToString() + "/10";
        GoldGeneration.text = (selectedTerritory.GoldMineTerritory.WorkersMine / 5) + GameMultiLang.GetTraduction("EveryDay");
        FoodGeneration.text = (selectedTerritory.IrrigationChannelTerritory.WorkersChannel / 5) + GameMultiLang.GetTraduction("EveryDay");
        
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
        FortressOption.InitializeBuildingOption(selectedTerritory.FortressTerritory);
        AcademyOption.InitializeBuildingOption(selectedTerritory.AcademyTerritory);
        BarracksOption.InitializeBuildingOption(selectedTerritory.BarracksTerritory);
        ArcheryOption.InitializeBuildingOption(selectedTerritory.ArcheryTerritory);
    }
    /// <summary>
    /// Update all menus of the territory-selected
    /// </summary>
    public void UpdateMenu()
    {
        selectedTerritory = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().TerritoryStats.Territory;
        UpdateStateMenu();
        UpdateMilitarMenu();
        UpdateTerritoryMenu();
        UpdateBuildingsMenu();

    }
    /// <summary>
    /// Update all text depending of the territory-selected
    /// </summary>
    private void UpdateAllText()
    {
        // militarWarriorsCount.text = selectedTerritory.Population.ToString() + " / " + selectedTerritory.LimitPopulation.ToString() + " units";
        /*
         if (selectedTerritory.Population > selectedTerritory.LimitPopulation)
         {
             militarWarriorsCount.color = Color.red;
             militarWarriorsCount.GetComponent<MenuToolTip>().SetNewInfo(GameMultiLang.GetTraduction("tooltip5A") +"\n " +
                                                                          GameMultiLang.GetTraduction("tooltip5B") + "\n" +
                                                                          GameMultiLang.GetTraduction("tooltip5C"));
         }
         else
         {
             militarWarriorsCount.color = new Color32(50,50,50,255);
             militarWarriorsCount.GetComponent<MenuToolTip>().SetNewInfo(GameMultiLang.GetTraduction("tooltip6"));
         }
        */
        CheckPopulation(swordMenWarriors, selectedTerritory.Swordsmen);
        CheckPopulation(lancersWarriors, selectedTerritory.Lancers);
        CheckPopulation(archersWarriors, selectedTerritory.Archer);
        goldCount.text = selectedTerritory.Gold.ToString();
        foodCount.text = selectedTerritory.FoodReward.ToString();
        GoldGeneration.text = (selectedTerritory.GoldMineTerritory.WorkersMine / 5) + GameMultiLang.GetTraduction("PerDay");
        FoodGeneration.text = (selectedTerritory.IrrigationChannelTerritory.WorkersChannel / 5) + GameMultiLang.GetTraduction("PerDay");
        MotivationBonus.text = selectedTerritory.MotivationTerritory.ToString() + "/10";
        DefenseBonus.text = selectedTerritory.FortressTerritory.PlusDefense.ToString() + "/10";
    }

    void CheckPopulation(GameObject groupstexts, UnitCombat unit)
    {
        TextMeshProUGUI[] texts = groupstexts.GetComponentsInChildren<TextMeshProUGUI>();
        texts[0].text = GameMultiLang.GetTraduction(unit.UnitName);
        texts[1].text = unit.NumbersUnit + " / " + selectedTerritory.GetLimit(unit) + GameMultiLang.GetTraduction("units");
        texts[2].text = selectedTerritory.GetSpeed(unit).ToString();
        if (unit.NumbersUnit > selectedTerritory.GetLimit(unit))
        {
            texts[1].color = Color.red;
            texts[1].GetComponent<MenuToolTip>().SetNewInfo(GameMultiLang.GetTraduction("tooltip5"));
        }
        else
        {
            texts[1].color = new Color32(50, 50, 50, 255);
            texts[1].GetComponent<MenuToolTip>().SetNewInfo(GameMultiLang.GetTraduction("tooltip6"));
        }
        
    }

    void Update()
    {
        selectedTerritory = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().TerritoryStats.Territory;
        UpdateAllText();
        UpdateCountDownImage();
    }

    public void ImproveBuildingButton(Building _building)
    {
        TerritoryHandler territoryHandler = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
        Building building = territoryHandler.GetBuilding(_building);
        ImproveBuildingInHandler(territoryHandler, building);
        EventManager.instance.AddEvent(territoryHandler, building);
        UpdateMenu();
    }
    private void ImproveBuildingInHandler(TerritoryHandler territoryHandler, Building building)
    {
        if (goldPlayer >= building.CostToUpgrade)
        {
            goldPlayer -= territoryHandler.GetBuilding(building).CostToUpgrade;
            territoryHandler.GetBuilding(building).ImproveCostUpgrade();
            territoryHandler.TerritoryStats.Territory.OpinionTerritory += 10;
            ShowFloatingText("+1 " + GameMultiLang.GetTraduction(building.Name) + " level", "TextMesh", territoryHandler.transform, new Color32(0, 19, 152, 255));
            ShowFloatingText("-" + territoryHandler.GetBuilding(building).CostToUpgrade.ToString(), "TextFloating", ResourceTableHandler.instance.GoldAnimation, Color.white);
        }
    }
    private int GetGoldGather(TerritoryHandler territoryHandler)
    {
        int gatherGold = territoryHandler.TerritoryStats.Territory.Gold;
        territoryHandler.GatherTerritoryGold();
        return gatherGold;
    }
    private int GetFoodGather(TerritoryHandler territoryHandler)
    {
        int gatherFood = territoryHandler.TerritoryStats.Territory.FoodReward;
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
            temp+= GetGoldGather(TerritoryManager.instance.GetTerritoriesHandlerByTypePlayer(Territory.TYPEPLAYER.PLAYER)[i]);
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
            temp += GetFoodGather(TerritoryManager.instance.GetTerritoriesHandlerByTypePlayer(Territory.TYPEPLAYER.PLAYER)[i]);
        }
        ShowFloatingText("+" + temp.ToString(), "TextFloating", ResourceTableHandler.instance.FoodAnimation,Color.white);
        foodPlayer += temp;
    }
    private void InitButtons()
    {
        MilitarChief militarChief = new MilitarChief();
        selectMilitarChief.onClick.AddListener(() => MenuManager.instance.OpenSelectCharacterMenu(militarChief));
    }
}
