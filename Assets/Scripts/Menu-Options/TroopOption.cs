using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TroopOption : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropdownUnitSelect;
    [SerializeField] GameObject moveUnits;
    [SerializeField] private TextMeshProUGUI numberUnitsText;
    [SerializeField] private Button button;
    [SerializeField] private GameObject block;
    private TerritoryHandler territoryAttacker;
    private int limit;
    public bool isSelected;

    private UnitCombat unitcombat;
    private int numberSelected;
    private void Start()
    {
        button.onClick.AddListener(() => UpdateUnit());
    }
    public void InitTroopOption()
    {
        
        isSelected = false;
        territoryAttacker = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
        moveUnits.SetActive(false);
        UpdateDropDown();
        dropdownUnitSelect.value = 0;
        block.SetActive(false);
    }
    public void UpdateDropDown()
    {
        GlobalVariables.instance.InitDropdown(dropdownUnitSelect, territoryAttacker.TerritoryStats.Territory.GetListUnitCombat());

        dropdownUnitSelect.onValueChanged.AddListener(delegate { DropdownUnitSelected(); });
    }
    public void ResetValue()
    {
        dropdownUnitSelect.value = 0;
        moveUnits.SetActive(false);
    }
    void DropdownUnitSelected()
    {
        int index = dropdownUnitSelect.value;
        string _type = dropdownUnitSelect.options[index].text;
        string type = GameMultiLang.GetTraductionReverse(_type);
        unitcombat = territoryAttacker.TerritoryStats.Territory.GetUnit(type);
        if (unitcombat != null)
        {
            //  print(unit.NumbersUnit);
            moveUnits.SetActive(true);
            limit = unitcombat.NumbersUnit;
            UpdateMenuByUnits(moveUnits, unitcombat.NumbersUnit, true);
        }
        UpdateDropDown();
    }
    

    public void UpdateNumericButton(Button btn, int _limit, bool _canAttack)
    {
        btn.interactable = _canAttack;
        btn.GetComponent<NumericButton>().limit = _limit;
        btn.GetComponent<NumericButton>().lockButton = _canAttack;
        btn.GetComponent<NumericButton>().pointerDown = false;
    }
    public void UpdateMenuByUnits(GameObject go, int _limit, bool canAttack)
    {
        List<Transform> tr = GlobalVariables.instance.GetAllChildren(go.transform);
        //decrease
        UpdateNumericButton(tr[1].GetComponent<Button>(), _limit, canAttack);
        //increase
        UpdateNumericButton(tr[2].GetComponent<Button>(), _limit, canAttack);
        numberUnitsText.text = _limit.ToString();
        
    }
    private void Update()
    {

    }
    public void UpdateUnit()
    {
        isSelected = true;
        numberSelected = int.Parse(numberUnitsText.text);
        territoryAttacker.TerritoryStats.Territory.GetUnit(unitcombat.GetType().ToString()).NumbersUnit -= numberSelected;
        block.SetActive(true);
        SelectTropsMenu.instance.UpdateAllDropdown();
        SelectTropsMenu.instance.TroopSelected.AddElement(unitcombat.GetType().ToString(), SelectTropsMenu.instance.GetIndexTroopOption(this), numberSelected);
    }
}
