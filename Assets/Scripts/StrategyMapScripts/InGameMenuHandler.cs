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
    [SerializeField] private Image[] countdownImages;
    [SerializeField] private Button[] buttons;
    [SerializeField] private BuildOption IrrigationChannelOption;
    [SerializeField] private BuildOption GoldMineOption;
    [SerializeField] private BuildOption SacredPlaceOption;
    [SerializeField] private BuildOption FortressOption;
    [SerializeField] private BuildOption BarracksOption;

    
    //resources
    private int goldPlayer= 20;
    private int foodPlayer = 10;
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
    

    public void UpdateMilitarMenu()
    {
        menuBlock.SetActive(false);
        MilitarBoss boss = selectedTerritory.MilitarBossTerritory;
        militaryBossName.text = boss.CharacterName;
        militaryBossPicture.sprite = boss.Picture;
        militaryBossExperience.text = boss.Experience.ToString()+ "/10";
        militaryBossEstrategy.text = "Strategy: " + boss.StrategyType;
        militaryBossInfluence.text = boss.Influence.ToString() +"/10";
        GenerationSpeed.text = selectedTerritory.VelocityPopulation.ToString();
        warriorsLimit.text = selectedTerritory.LimitPopulation.ToString();
        if (selectedTerritory.TypePlayer != Territory.TYPEPLAYER.PLAYER)
        {
            menuBlock.SetActive(true);                
        }
       
    }
    public void UpdateTerritoryMenu()
    {
        menuBlockTerritory.SetActive(false);
        //territoryImage.sprite = selectedTerritory.
        if (selectedTerritory.TypePlayer != Territory.TYPEPLAYER.PLAYER)
        {
            menuBlockTerritory.SetActive(true);
            
        }
        
        
    }
    void UpdateCountDownImage()
    {
        IrrigationChannelOption.TerritoryBuilding = selectedTerritory.IrrigationChannelTerritory;
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
        goldCount.text = selectedTerritory.Gold.ToString();
        foodCount.text = selectedTerritory.FoodReward.ToString();
        GoldGeneration.text = selectedTerritory.GoldMineTerritory.VelocityGold.ToString();
        FoodGeneration.text = selectedTerritory.IrrigationChannelTerritory.VelocityFood.ToString();
        MotivationBonus.text = selectedTerritory.SacredPlaceTerritory.Motivation.ToString() + "/10"; 
        AttackBonus.text = selectedTerritory.FortressTerritory.PlusDefense.ToString() + "/10";
        DefenseBonus.text = selectedTerritory.BarracksTerritory.PlusAttack.ToString() + "/10";

        //UpdateResourceTable();

        UpdateCountDownImage();
    }
   
    
  
    
    
    private void ImproveSpeedPopulation(TerritoryHandler territoryHandler)
    {
        if (goldPlayer >= territoryHandler.territoryStats.territory.CostPopulation)
        {
            territoryHandler.ImproveSpeedPopulation();
            goldPlayer -= territoryHandler.territoryStats.territory.CostPopulation;
            ShowFloatingText("+0.3 velocity population", "TextMesh", territoryHandler.transform, new Color32(0, 19, 152, 255));
            ShowFloatingText("-" + territoryHandler.territoryStats.territory.CostPopulation.ToString(), "TextFloating", ResourceTableHandler.instance.GoldAnimation, Color.white);
            territoryHandler.territoryStats.territory.CostPopulation += 10;
        }

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
            ShowFloatingText("-" + territoryHandler.territoryStats.territory.IrrigationChannelTerritory.CostToUpgrade.ToString(), "TextFloating", ResourceTableHandler.instance.FoodAnimation, Color.white);
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
            ShowFloatingText("-" + territoryHandler.territoryStats.territory.CostPopulation.ToString(), "TextFloating", ResourceTableHandler.instance.GoldAnimation, Color.white);
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
            ShowFloatingText("-" + territoryHandler.territoryStats.territory.GoldMineTerritory.CostToUpgrade.ToString(), "TextFloating", ResourceTableHandler.instance.GoldAnimation, Color.white);
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
            ShowFloatingText("-" + territoryHandler.territoryStats.territory.SacredPlaceTerritory.CostToUpgrade.ToString(), "TextFloating", ResourceTableHandler.instance.GoldAnimation, Color.white);
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
            ShowFloatingText("-" + territoryHandler.territoryStats.territory.FortressTerritory.CostToUpgrade.ToString(), "TextFloating", ResourceTableHandler.instance.GoldAnimation, Color.white);
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
            ShowFloatingText("-" + territoryHandler.territoryStats.territory.BarracksTerritory.CostToUpgrade.ToString(), "TextFloating", ResourceTableHandler.instance.GoldAnimation,Color.white);
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
   
   
    void Start()
    {
        buttons[0].onClick.AddListener(() => ImproveIrrigateChannelButton());
        buttons[1].onClick.AddListener(() => ImproveMineGoldButton());
        buttons[2].onClick.AddListener(() => ImproveSacredPlaceButton());
        buttons[3].onClick.AddListener(() => ImproveFortressButton());
        buttons[4].onClick.AddListener(() => ImproveBarracksButton());
        
        
        UpdateMenu();
    }


    
    
    
 
}
