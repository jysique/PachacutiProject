using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SelectTropsMenu : MonoBehaviour
{
    public static SelectTropsMenu instance;
    [SerializeField] public Button moveUnits;
    [SerializeField] public Button cancel;
    TerritoryHandler territoryAttacker;
    TerritoryHandler territoryToAttack;
    [SerializeField] public TroopOption[] optionsInAttack { get; private set; }
    [SerializeField] Troop troopSelected = new Troop();
    public int acumulated = 0;

    public Troop TroopSelected
    {
        get { return troopSelected; }
        set { troopSelected = value; }
    }
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        moveUnits.interactable = GetIsAllSelected();
    }
    private void Start()
    {
        cancel.onClick.AddListener(() => CancelMoveUnits());
        moveUnits.onClick.AddListener(() => MoveUnits());
    }
    public void CancelMoveUnits()
    {
        troopSelected.Reset();
        this.gameObject.SetActive(false);
        DateTableHandler.instance.ResumeTime();
    }
    public void InitMenu(TerritoryHandler _territoryToAttack)
    {
        DateTableHandler.instance.PauseTime();
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
            acumulated += totalWarriors;
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
}
