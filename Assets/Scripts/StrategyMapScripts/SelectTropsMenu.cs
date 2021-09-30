using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class SelectTropsMenu : MonoBehaviour
{

    public static SelectTropsMenu instance;
    TerritoryHandler territoryAttacker;
    TerritoryHandler territoryToAttack;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Button moveUnits;
    [SerializeField] private Button cancel;
    [SerializeField] private GameObject containerUC;
    [SerializeField] Troop troopSelected = new Troop();
    List<UCTempOption> tempOptions = new List<UCTempOption>();
    int total_cost = 0;
    int limit_troop;
    public Troop TroopSelected
    {
        get { return troopSelected; }
        set { troopSelected = value; }
    }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        cancel.onClick.AddListener(() => CancelMoveUnits());
        moveUnits.onClick.AddListener(() => MoveUnits());
    }

    private void Update()
    {
        //print("selected" + CountSelected());
        total_cost = GetTotalCost();
        moveUnits.interactable = HasEnoughResources() && IsLimitSelected();
    }
    public void InitMenu(TerritoryHandler _territoryToAttack)
    {
        tempOptions.Clear();
        
        territoryToAttack = _territoryToAttack;
        territoryAttacker = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
        limit_troop = Mathf.RoundToInt(territoryToAttack.TerritoryStats.Territory.TerrainList.Count / 2);
        DateTableHandler.instance.PauseButton();
        string _title = GameMultiLang.GetTraduction("SelectTroopsTitle").Replace("QUANTITY", limit_troop.ToString());
        title.text = _title;
        Transform gridLayout = containerUC.transform.Find("ScrollArea/ScrollContainer/GridLayout").transform;
        foreach (Transform child in gridLayout.transform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < territoryAttacker.TerritoryStats.Territory.ListUnitCombat.UnitCombats.Count; i++)
        {
            UnitCombat uc = territoryAttacker.TerritoryStats.Territory.ListUnitCombat.UnitCombats[i];
            if (uc.IsAvailable)
            {
                InstantiateUnitCombat(uc);
            }
        }
    }
    public bool IsLimitSelected()
    {
        return CountSelected() > 0 && CountSelected() <= limit_troop;
    }

    public int CountSelected()
    {
        int a = 0;
        for (int i = 0; i < tempOptions.Count; i++)
        {
            if (tempOptions[i].IsSelected())
            {
                a++;
            }
        }
        return a;
    }
    private bool HasEnoughResources()
    {
        return (total_cost <= InGameMenuHandler.instance.GoldPlayer)
            && (total_cost <= InGameMenuHandler.instance.FoodPlayer);
    }
    private int GetTotalCost()
    {
        int cost = 0;
        foreach (UCTempOption item in tempOptions)
        {
            if (item.IsSelected())
            {
                cost += item.UnitCombat.Quantity;
            }
        }
        return cost;
    }
    private void InstantiateUnitCombat(UnitCombat _unitCombat)
    {
        Transform gridLayout = containerUC.transform.Find("ScrollArea/ScrollContainer/GridLayout").transform;
        GameObject ucOption = Instantiate(Resources.Load("Prefabs/MenuPrefabs/UCTemp")) as GameObject;
        ucOption.name = _unitCombat.UnitName;
        ucOption.transform.SetParent(gridLayout.transform, false);
        UCTempOption tempOption = ucOption.GetComponent<UCTempOption>();
        tempOption.InitOption(_unitCombat);
        tempOptions.Add(tempOption);
    }
    public void MoveUnits()
    {
        int i = 0;
        foreach (UCTempOption item in tempOptions)
        {
            if (item.IsSelected())
            {
               // item.UnitCombat.PositionInBattle = i;
                troopSelected.AddUnitCombat(item.UnitCombat);
               // i++;
            }
        }
        Territory.TYPEPLAYER typeSelected = territoryAttacker.TerritoryStats.Territory.TypePlayer;

        int totalWarriors = troopSelected.GetAllNumbersUnit();
        if (InGameMenuHandler.instance.GoldPlayer >= totalWarriors || territoryToAttack.TerritoryStats.Territory.TypePlayer == typeSelected)
        {
            if (territoryToAttack.TerritoryStats.Territory.TypePlayer != typeSelected)
            {
                InGameMenuHandler.instance.GoldPlayer -= totalWarriors;
                InGameMenuHandler.instance.FoodPlayer -= totalWarriors;
            }
            troopSelected.MoveUnits(territoryAttacker.TerritoryStats.Territory);

            WarManager.instance.SendWarriors(territoryAttacker, territoryToAttack, troopSelected);
        }
        else
        {
            InGameMenuHandler.instance.ShowFloatingText("you need " + totalWarriors + " golds", "TextMesh", transform, new Color32(187, 27, 128, 255));
        }
        CancelMoveUnits();
    }

    public void CancelMoveUnits()
    {
        troopSelected.Clear();
        this.gameObject.SetActive(false);
        DateTableHandler.instance.PlayButton();
    }
    /*
    [SerializeField] public Button moveUnits;
    [SerializeField] public Button cancel;
    TerritoryHandler territoryAttacker;
    TerritoryHandler territoryToAttack;
    [SerializeField] public TroopOption[] optionsInAttack { get; private set; }
    [SerializeField] Troop troopSelected = new Troop();
    
    public int total_cost = 0;
    public Troop TroopSelected
    {
        get { return troopSelected; }
        set { troopSelected = value; }
    }

    private void Update()
    {
        total_cost = GetTotalCost();
        moveUnits.interactable = GetIsAllSelected() && HasEnoughResources();
        SetTooltips();
    }
    private bool HasEnoughResources()
    {
        return (total_cost <= InGameMenuHandler.instance.GoldPlayer) 
            && (total_cost <= InGameMenuHandler.instance.FoodPlayer);
    }
    private void SetTooltips()
    {
        MenuToolTip toolTip = moveUnits.GetComponent<MenuToolTip>();
        if (!GetIsAllSelected())
        {
            toolTip.SetNewInfo(GameMultiLang.GetTraduction("tooltip9"));
        }
        else if (HasEnoughResources())
        {
            toolTip.SetNewInfo(GameMultiLang.GetTraduction("tooltip10").Replace("COST", total_cost.ToString()));
        }
        else
        {
            toolTip.SetNewInfo(GameMultiLang.GetTraduction("tooltip11"));
        }

    }
    private int GetTotalCost()
    {
        int cost = 0;
        foreach (TroopOption item in optionsInAttack)
        {
            if (item.UnitCombatInBattle != null && item.UnitCombatInBattle.Quantity > 0)
            {
                cost +=item.UnitCombatInBattle.Quantity;
            }
        }
        return cost;
    }
    private void Start()
    {
        cancel.onClick.AddListener(() => CancelMoveUnits());
        moveUnits.onClick.AddListener(() => MoveUnits());
    }
    public void InitMenu(TerritoryHandler _territoryToAttack)
    {
        // DateTableHandler.instance.PauseTime();
        DateTableHandler.instance.PauseButton();
        territoryToAttack = _territoryToAttack;
        territoryAttacker = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
        optionsInAttack = GetComponentsInChildren<TroopOption>();
        for (int i = 0; i < optionsInAttack.Length; i++)
        {
            optionsInAttack[i].InitTroopOption(1, i, territoryAttacker.TerritoryStats.Territory, territoryToAttack.TerritoryStats.Territory);
        }
    }
    public void MoveUnits()
    {
        foreach (TroopOption item in optionsInAttack)
        {
            if (item.UnitCombatInBattle!= null && item.UnitCombatInBattle.Quantity > 0)
            {
                troopSelected.AddElement(item.UnitCombatInBattle, item.PosInBattle);
            }
        }
        Territory.TYPEPLAYER typeSelected = territoryAttacker.TerritoryStats.Territory.TypePlayer;

        int totalWarriors = troopSelected.GetAllNumbersUnit();
        if (InGameMenuHandler.instance.GoldPlayer >= totalWarriors || territoryToAttack.TerritoryStats.Territory.TypePlayer == typeSelected)
        {
            if (territoryToAttack.TerritoryStats.Territory.TypePlayer != typeSelected)
            {
                InGameMenuHandler.instance.GoldPlayer -= totalWarriors;
                InGameMenuHandler.instance.FoodPlayer -= totalWarriors;
            }
            troopSelected.MoveUnits(territoryAttacker.TerritoryStats.Territory);

            WarManager.instance.SendWarriors(territoryAttacker, territoryToAttack, troopSelected);
        }
        else
        {
            InGameMenuHandler.instance.ShowFloatingText("you need "+totalWarriors+" golds", "TextMesh", transform, new Color32(187, 27, 128, 255));
        }
        CancelMoveUnits();
    }
    public void CancelMoveUnits()
    {
        troopSelected.Reset();
        this.gameObject.SetActive(false);
        //DateTableHandler.instance.ResumeTime();
        DateTableHandler.instance.PlayButton();
    }
    public void UpdateAllOptions()
    {
        foreach (var o in optionsInAttack)
        {
            o.UpdateDropDownValues();
            o.UpdateLimit();
        }
    }
    public bool GetIsAllSelected() 
    {
        bool a = false;
        for (int i = 0; i < optionsInAttack.Length; i++)
        {
            if (optionsInAttack[i].IsSelected == true)
            {
                a = true;
            }
        }
        return a;
    }
    */
}
