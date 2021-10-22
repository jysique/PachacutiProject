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
    [SerializeField] private GameObject menuBlockMilitaryOther;
    [SerializeField] private GameObject menuBlockMilitaryCharacter;
    [SerializeField] private GameObject menuBlockMilitaryWaste;

    [SerializeField] private GameObject containerTroops;
    [SerializeField] private Button selectMilitarChief;
    [SerializeField] private Image militaryBossPicture;
    [SerializeField] private TextMeshProUGUI militaryBossName;
    [SerializeField] private TextMeshProUGUI militaryBossExperience;
    [SerializeField] private TextMeshProUGUI militaryBossEstrategy;
    [SerializeField] private TextMeshProUGUI militaryBossInfluence;

    [SerializeField] private Button addUnit;
    [SerializeField] private TMP_Dropdown dropdownUnitsCombat;
    [SerializeField] private GameObject moveUnits;
    [SerializeField] private TextMeshProUGUI numberUnitsText;
    [SerializeField] GameObject CreateUnitGO;

    [Header("Menu territory")]
    [SerializeField] private GameObject menuBlockTerritoryWaste;
    [SerializeField] private GameObject menuBlockTerritoryOther;

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
    [SerializeField] private GameObject containerBuildings;
    [SerializeField] private GameObject menuBlockBuildingsOther;
    [SerializeField] private GameObject menuBlockBuildingsWaste;
    [SerializeField] private TMP_Dropdown dropdownBuildings;
    [SerializeField] GameObject CreateBuidingGO;

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
        dropdownUnitsCombat.onValueChanged.AddListener(delegate { DropdownItemSelected2(selectedTerritory); });
        dropdownBuildings.onValueChanged.AddListener(delegate { DropdownItemSelected(selectedTerritory); });
    }
    /// <summary>
    /// Update all elements of the militar menu of the territory-selected
    /// if the territory-selected doesn't belong to the player 
    /// it blocks
    /// </summary>
    private void UpdateMilitarMenu()
    {
        menuBlockMilitaryOther.SetActive(false);
        menuBlockMilitaryWaste.SetActive(false);
        menuBlockMilitaryCharacter.SetActive(false);
        MilitarChief mChief = selectedTerritory.MilitarChiefTerritory;
        militaryBossName.text = mChief.CharacterName;
        militaryBossPicture.sprite = mChief.Picture;
        militaryBossExperience.text = mChief.Experience.ToString()+ "/10";
        militaryBossEstrategy.text = GameMultiLang.GetTraduction("StrategyTitle") + mChief.StrategyType;
        militaryBossInfluence.text = mChief.Influence.ToString() +"/10";

        if (selectedTerritory.TypePlayer != Territory.TYPEPLAYER.PLAYER)
        {
            if (selectedTerritory.TypePlayer == Territory.TYPEPLAYER.WASTE)
            {
                menuBlockMilitaryWaste.SetActive(true);
            }
            else
            {
                menuBlockMilitaryOther.SetActive(true);
            }
        }
        else
        {
            if (selectedTerritory.IsClaimed == false)
            {
                menuBlockMilitaryCharacter.SetActive(true);
            }
            else
            {
                menuBlockMilitaryCharacter.SetActive(false);
            }
        }
        dropdownUnitsCombat.value = 0;
        UpdateDropDownValues(selectedTerritory);
        UpdateTroopsContainer(selectedTerritory);
    }
    /// <summary>
    /// Update all elements of the territory menu of the territory-selected
    /// if the territory-selected doesn't belong to the player 
    /// it blocks
    /// </summary>
    private void UpdateTerritoryMenu()
    {
        menuBlockTerritoryOther.SetActive(false);
        menuBlockTerritoryWaste.SetActive(false);
        territoryEmpire.text = TerritoryManager.instance.GetTerritoryEmpire(selectedTerritory);
        territoryRegion.text = selectedTerritory.RegionTerritory.ToString().Split(char.Parse("_"))[0];
        MotivationBonus.text = selectedTerritory.MotivationTerritory.ToString() + "/10";
        //   AttackBonus.text = selectedTerritory.FortressTerritory.PlusDefense.ToString() + "/10";
        //   DefenseBonus.text = selectedTerritory.ArmoryTerritory.PlusAttack.ToString() + "/10";
        GoldGeneration.text = (selectedTerritory.GoldMineTerritory.LimitUnits / 5) + GameMultiLang.GetTraduction("EveryDay");
        FoodGeneration.text = (selectedTerritory.FarmTerritory.LimitUnits / 5) + GameMultiLang.GetTraduction("EveryDay");
        //GoldGeneration.text = (selectedTerritory.GoldMineTerritory.WorkersMine / 5) + GameMultiLang.GetTraduction("EveryDay");
        //FoodGeneration.text = (selectedTerritory.FarmTerritory.WorkersChannel / 5) + GameMultiLang.GetTraduction("EveryDay");

        if (selectedTerritory.TypePlayer != Territory.TYPEPLAYER.PLAYER)
        {
            menuBlockTerritoryOther.SetActive(true);           
        }
        if (selectedTerritory.TypePlayer == Territory.TYPEPLAYER.WASTE)
        {
            menuBlockTerritoryWaste.SetActive(true);
        }
    }

    /// <summary>
    /// Update all elements of the buildings menu of the territory-selected
    /// if the territory-selected doesn't belong to the player 
    /// it blocks
    /// </summary>
    private void UpdateBuildingsMenu()
    {
        Transform gridLayout = containerBuildings.transform.Find("ScrollArea/ScrollContainer/GridLayout").transform;
        foreach (Transform child in gridLayout.transform)
        {
            if (child.name != "CreateBuiding")
            {
                Destroy(child.gameObject);

            }
        }

        menuBlockBuildingsOther.SetActive(false);
        menuBlockBuildingsWaste.SetActive(false);
        if (selectedTerritory.TypePlayer != Territory.TYPEPLAYER.PLAYER)
        {
            menuBlockBuildingsOther.SetActive(true);
        }
        if (selectedTerritory.TypePlayer == Territory.TYPEPLAYER.WASTE)
        {
            menuBlockBuildingsWaste.SetActive(true);
        }
       //   UpdateDropdown(dropdownBuildings, selectedTerritory.GetListBuildings());
        UpdateDropdown(dropdownBuildings, Utils.instance.GetListBuildings(selectedTerritory));
        UpdateBuildings(selectedTerritory);
    }
    /// <summary>
    /// Update all menus of the territory-selected
    /// </summary>
    public void UpdateMenu()
    {
        selectedTerritory = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().Territory;
        UpdateMilitarMenu();
        UpdateTerritoryMenu();
        UpdateBuildingsMenu();
    }
    /// <summary>
    /// Update all text depending of the territory-selected
    /// </summary>
    private void UpdateAllText()
    {

        goldCount.text = selectedTerritory.Gold.ToString();
        foodCount.text = selectedTerritory.FoodReward.ToString();
        GoldGeneration.text = (selectedTerritory.GoldMineTerritory.LimitUnits / 5) + GameMultiLang.GetTraduction("PerDay");
        FoodGeneration.text = (selectedTerritory.FarmTerritory.LimitUnits / 5) + GameMultiLang.GetTraduction("PerDay");
        //GoldGeneration.text = (selectedTerritory.GoldMineTerritory.WorkersMine / 5) + GameMultiLang.GetTraduction("PerDay");
        //FoodGeneration.text = (selectedTerritory.FarmTerritory.WorkersChannel / 5) + GameMultiLang.GetTraduction("PerDay");
        MotivationBonus.text = selectedTerritory.MotivationTerritory.ToString() + "/10";
        DefenseBonus.text = selectedTerritory.FortressTerritory.PlusDefense.ToString() + "/10";
    }

    void Update()
    {
        //selectedTerritory = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().Territory;
        CreateUnitGO.transform.SetAsFirstSibling();
        CreateBuidingGO.transform.SetAsLastSibling();
        //addUnit.interactable = (dropdownUnitsCombat.value != 0);
        UpdateAllText();
    }

    public void ImproveBuildingButton(Building _building)
    {
        TerritoryHandler territoryHandler = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
        Building building = territoryHandler.Territory.GetBuilding(_building);
        ImproveBuildingInHandler(territoryHandler, building);
        EventManager.instance.AddEvent(territoryHandler, building);
      //  UpdateMenu();
    }

    public void ImproveNewUnitButton(UnitCombat unitCombat)
    {
        TerritoryHandler territoryHandler = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
        AddNewUnitInHandler(territoryHandler, unitCombat);
        EventManager.instance.AddEvent(territoryHandler, unitCombat);
    }

    private void AddNewUnitInHandler(TerritoryHandler territoryHandler, UnitCombat unitCombat)
    {
        if (goldPlayer >= unitCombat.Quantity) // QUANTITY WORKS LIKE COST
        {
            goldPlayer -= unitCombat.Quantity;
            //ShowFloatingText("+1 " + GameMultiLang.GetTraduction(building.Name) + " level", "TextMesh", territoryHandler.transform, new Color32(0, 19, 152, 255));
            //ShowFloatingText("-" + territoryHandler.Territory.GetBuilding(building).CostToUpgrade.ToString(), "TextFloating", ResourceTableHandler.instance.GoldAnimation, Color.white);
        }
        else
        {
            ShowFloatingText("no gold", "TextMesh", territoryHandler.transform, new Color32(0, 19, 152, 255));
        }
    }

    private void ImproveBuildingInHandler(TerritoryHandler territoryHandler, Building building)
    {
        if (goldPlayer >= building.CostToUpgrade)
        {
            goldPlayer -= territoryHandler.Territory.GetBuilding(building).CostToUpgrade;
            ShowFloatingText("+1 " + GameMultiLang.GetTraduction(building.Name) + " level", "TextMesh", territoryHandler.transform, new Color32(0, 19, 152, 255));
            ShowFloatingText("-" + territoryHandler.Territory.GetBuilding(building).CostToUpgrade.ToString(), "TextFloating", ResourceTableHandler.instance.GoldAnimation, Color.white);
        }
        else
        {
            ShowFloatingText("no gold", "TextMesh", territoryHandler.transform, new Color32(0, 19, 152, 255));
        }
    }
    private int GetGoldGather(TerritoryHandler territoryHandler)
    {   
        
        int gatherGold = territoryHandler.Territory.Gold;
        territoryHandler.Territory.GatherTerritoryGold();
        return gatherGold;
    }
    private int GetFoodGather(TerritoryHandler territoryHandler)
    {
        int gatherFood = territoryHandler.Territory.FoodReward;
        territoryHandler.Territory.GatherTerritoryFood();
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
        MilitarChief _militarChief = new MilitarChief();
        addUnit.onClick.AddListener(() => AddUnitButton());
        selectMilitarChief.onClick.AddListener(() => MenuManager.instance.OpenSelectCharacterMenu(_militarChief));
    }

    private void UpdateDropdown(TMP_Dropdown _dropdown, List<string> _items)
    {
        Utils.instance.InitDropdown(_dropdown,_items);
    }

    void DropdownItemSelected(Territory territory)
    {
        
        int index = dropdownBuildings.value;
        string _type = dropdownBuildings.options[index].text;

        string type = GameMultiLang.GetTraductionReverse(_type);

        TerritoryHandler handler = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
        Building building = handler.Territory.GetBuilding(type);
        if (building != null)
        {
            if (territory.numberOfBuildings < TerritoryManager.instance.ClassifyTerritory(territory))
            {
                territory.numberOfBuildings++;
                building.Status++;
                ImproveBuildingButton(building);
                InstantiateBuilding(building);
            }
            else
            {
                AlertManager.AlertLimitBuilding(territory.name);
            }

        }

        UpdateDropdown(dropdownBuildings, Utils.instance.GetListBuildings(territory));
        dropdownBuildings.value = 0;
    }

    [SerializeField] UnitCombat dummy= new UnitCombat();
    
    public void UpdateTroopSelected()
    {
        /*
        int numberSelected = int.Parse(numberUnitsText.text);
        if (dummy!=null)
        {
            dummy.Quantity = numberSelected;
        }
        */
    }
    private void UpdateNumericButton(Button btn, int _limit, bool _canAttack)
    {
        btn.interactable = _canAttack;
        btn.GetComponent<NumericButton>().limit = _limit;
        btn.GetComponent<NumericButton>().lockButton = _canAttack;
        btn.GetComponent<NumericButton>().pointerDown = false;
    }
    private void AddUnitButton()
    {
        //print("añadiendo " + dummy.UnitName + " cantidad " + numberUnitsText.text);
        dummy.Quantity = int.Parse(numberUnitsText.text);
        selectedTerritory.ListUnitCombat.AddUnitCombat(dummy);
        ImproveNewUnitButton(dummy);
        
        UpdateMilitarMenu();
    }
    private void UpdateMenuByUnits(GameObject go, int _limit, bool isInitialized, string value = null)
    {
        List<Transform> tr = Utils.instance.GetAllChildren(go.transform);
        UpdateNumericButton(tr[1].GetComponent<Button>(), _limit, true);        //decrease
        UpdateNumericButton(tr[2].GetComponent<Button>(), _limit, true);        //increase
        if (isInitialized)
        {
            numberUnitsText.text = _limit.ToString();
            if (value != null)
            {
                numberUnitsText.text = value;
            }
        }
    }
   
    private List<string> optionsDropDown = new List<string>();
    private void UpdateDropDownValues(Territory _territory)
    {
        optionsDropDown.Clear();
        optionsDropDown.Add("SelectUnit");
        optionsDropDown = Utils.instance.GetListUnitCombat(_territory, optionsDropDown);
        
        Utils.instance.InitDropdown(dropdownUnitsCombat, optionsDropDown);
    }
    private void DropdownItemSelected2(Territory territory)
    {

        if (dropdownUnitsCombat.value == 0)
        {
            addUnit.interactable = false;
            dummy = null;
            return;
        }
        addUnit.interactable = true;
        int index = dropdownUnitsCombat.value;
        string _type = dropdownUnitsCombat.options[index].text;
        string type = GameMultiLang.GetTraductionReverse(_type);
        dummy = Utils.instance.CreateNewUnitCombat(type,0);
        Building building = selectedTerritory.GetBuildingByUnit(dummy);
        int limit = building.LimitUnits - selectedTerritory.ListUnitCombat.GetUnitQuantity(type);
        if (limit <= 0)
        {
            addUnit.interactable = false;
        }
        UpdateMenuByUnits(moveUnits, limit, true);

        //UpdateDropDownValues(territory);
       // dropdownUnitsCombat.value = 0;
    }


    private void InstantiateBuilding(Building building)
    {
        if (building.Level>0|| building.Status>-1)
        {
            Transform gridLayout = containerBuildings.transform.Find("ScrollArea/ScrollContainer/GridLayout").transform;
            GameObject buildingOption = Instantiate(Resources.Load("Prefabs/MenuPrefabs/BuildingOption")) as GameObject;
            buildingOption.name = building.GetType().ToString();
            buildingOption.transform.SetParent(gridLayout.transform, false);
            
            if (building.PositionInGridLayout< 0)
            {
                buildingOption.transform.SetSiblingIndex(0);
            }
            else
            {
                buildingOption.transform.SetSiblingIndex(building.PositionInGridLayout);
            }
            
            buildingOption.GetComponent<BuildOption>().InitializeBuildingOption(building);
        }
    }
    private void UpdateBuildings(Territory territory)
    {
        InstantiateBuilding(territory.FarmTerritory);
        InstantiateBuilding(territory.GoldMineTerritory);
        InstantiateBuilding(territory.FortressTerritory);
        InstantiateBuilding(territory.AcademyTerritory);
        InstantiateBuilding(territory.BarracksTerritory);
        InstantiateBuilding(territory.CastleTerritory);
        InstantiateBuilding(territory.StableTerritory);
        InstantiateBuilding(territory.ArcheryTerritory);
    }


    public void InstantiateUnitCombat(UnitCombat unit)
    {
        if (unit.Quantity > 0 || selectedTerritory.GetBuildingByUnit(unit).Level > 0)
        {
            Transform gridLayout = containerTroops.transform.Find("ScrollArea/ScrollContainer/GridLayout").transform;
            GameObject unitCombatOption = Instantiate(Resources.Load("Prefabs/MenuPrefabs/UnitCombatOption")) as GameObject;
            unitCombatOption.transform.SetParent(gridLayout.transform, false);
            unitCombatOption.GetComponent<UnitCombatOption>().InitializeTroopContainerOption(unit);
        }
    }
    public void UpdateTroopsContainer(Territory territory)
    {
        Transform gridLayout = containerTroops.transform.Find("ScrollArea/ScrollContainer/GridLayout").transform;
        foreach (Transform child in gridLayout.transform)
        {
            if (child.name != "CreateUnit")
                Destroy(child.gameObject);
        }
        for (int i = 0; i < territory.ListUnitCombat.UnitCombats.Count; i++)
        {
            InstantiateUnitCombat(territory.ListUnitCombat.UnitCombats[i]);
        }
    }

}
