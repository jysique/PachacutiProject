using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SelectTropsMenu : MonoBehaviour
{
    public static SelectTropsMenu instance;
    [SerializeField] public Button moveUnits;
    TerritoryHandler territoryAttacker;
    TerritoryHandler territoryToAttack;
    [SerializeField] TroopOption[] options;
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
        moveUnits.onClick.AddListener(() => MoveUnits());
    }
    public void InitMenu(TerritoryHandler _territoryToAttack)
    {
        DateTableHandler.instance.PauseTime();
        territoryToAttack = _territoryToAttack;
        territoryAttacker = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
        options = GetComponentsInChildren<TroopOption>();
        foreach (var o in options)
        {
            o.InitTroopOption();
        }
    }
    public void MoveUnits()
    {
        Territory.TYPEPLAYER typeSelected = territoryAttacker.TerritoryStats.Territory.TypePlayer;
        int totalWarriors = troopSelected.GetAllNumbersUnit();
        acumulated += totalWarriors;
        if (InGameMenuHandler.instance.GoldPlayer >= totalWarriors || territoryToAttack.TerritoryStats.Territory.TypePlayer == typeSelected)
        {
            if (territoryToAttack.TerritoryStats.Territory.TypePlayer != typeSelected)
            {
                InGameMenuHandler.instance.GoldPlayer -= totalWarriors;
                InGameMenuHandler.instance.FoodPlayer -= totalWarriors;
            }
            WarManager.instance.SendWarriors(territoryAttacker, territoryToAttack, troopSelected);
        }
        else
        {
            InGameMenuHandler.instance.ShowFloatingText("you need "+totalWarriors+" golds", "TextMesh", transform, new Color32(187, 27, 128, 255));
        }
        troopSelected.Reset();
        this.gameObject.SetActive(false);
        DateTableHandler.instance.ResumeTime();
    }
    public int GetIndexTroopOption(TroopOption troopOption)
    {
        return System.Array.IndexOf(options, troopOption);
    }
    public void UpdateAllDropdown()
    {
        foreach (var o in options)
        {
            o.UpdateDropDown();
            if (o.isSelected == false) {
                o.ResetValue();
            }
        }
    }
    public bool GetIsAllSelected() 
    {
        bool a = false;
        for (int i = 0; i < options.Length; i++)
        {
            if (options[i].isSelected == true)
            {
                a = true;
            }
        }
        return a;
    }
}
