using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
public class TroopOption : MonoBehaviour
{
    [SerializeField] private Image terrainImage;
    [SerializeField] private TMP_Dropdown dropdownUnitSelect;
    [SerializeField] private GameObject moveUnits;
    [SerializeField] private TextMeshProUGUI numberUnitsText;
    [SerializeField] private UnitCombat unitcombatTerritory;
    [SerializeField] private UnitCombat unitcombatBattle;
    [SerializeField] private Territory territory;
    [SerializeField] private Territory territoryToAttack;
    [SerializeField] private List<string> optionsDropDown = new List<string>();
    [SerializeField] private Terrain terrain;
    [SerializeField] private GameObject blockTroopOption;

    private int posInBattle;
    private bool isSelected;
    private int typeOption;
    private int numberSelected;
    private int index;
    public UnitCombat UnitCombatInBattle
    {
        get { return unitcombatBattle; }
    }
    public int PosInBattle
    {
        get { return posInBattle; }
    }
    public bool IsSelected
    {
        get { return isSelected; }
    }
    private void OnEnable()
    {
        dropdownUnitSelect.onValueChanged.AddListener(delegate { DropdownUnitSelected(); });
    }
    private void Start()
    {
        
        OnNumericButton(moveUnits);
    }
    private void UpdateTerraintSprite()
    {
        List<Terrain> terrains;
        if (typeOption == 1) {
            terrains = territoryToAttack.TerrainList;
        }
        else
        {
            terrains = territory.TerrainList;
        }
        List<Terrain> newTerrains = new List<Terrain>();
        if (typeOption == 1)
        {
            for (int y = (terrains.Count / 2) - 1; y >= 0; y--)
            {
                newTerrains.Add(terrains[y]);
            }
            for (int y = terrains.Count - 1; y >= (terrains.Count / 2); y--)
            {
                newTerrains.Add(terrains[y]);
            }
            terrains = newTerrains;
        }
        else
        {
            terrains = territory.TerrainList;
        }

        int posTerrain = 0;
        if (posInBattle >= terrains.Count / 4)
        {
            posTerrain = posInBattle + terrains.Count / 4;
        }
        else
        {
            posTerrain = posInBattle;
        }
        terrain = terrains[posTerrain];
        terrainImage.sprite = terrain.Picture;
    }
    private void UpdateBlockOption()
    {
        if (terrain.Type == Terrain.TYPE.NONE)
        {
            blockTroopOption.SetActive(true);
        }
        else
        {
            blockTroopOption.SetActive(false);
        }
    }
    /// <summary>
    /// Initialize all values in the option
    /// </summary>
    /// <param name="_type">Type(1-atacck,2-defense)</param>
    /// <param name="pos">Position in battle</param>
    /// <param name="_territory">Territori selected</param>
    public void InitTroopOption(int _type, int pos, Territory _territory, Territory _territoryToAttack = null)
    {
        typeOption = _type;
        territory = _territory;
        territoryToAttack = _territoryToAttack;
        posInBattle = pos;

        UpdateTerraintSprite();
        UpdateBlockOption();
        if (typeOption == 1)
        {
            isSelected = false;
            unitcombatTerritory = null;
            unitcombatBattle = null;
        }
        else
        {
            unitcombatTerritory = territory.GetUnit(territory.TroopDefending[posInBattle].GetType().ToString());
            if (unitcombatTerritory != null)
            {
                isSelected = true;
                unitcombatBattle = Utils.instance.GetNewUnitCombat(unitcombatTerritory.GetType().ToString());
                unitcombatBattle.Quantity = unitcombatTerritory.Quantity;
            }
            else
            {
                isSelected = false;
                unitcombatBattle = null;
            }
        }

        UpdateDropDownValues();
        OnInitDropDown();
    }
    public void OnInitDropDown()
    {

        if (typeOption == 1)
        {
            dropdownUnitSelect.value = 0;
            moveUnits.SetActive(false);
        }
        else
        {
            moveUnits.SetActive(true);
            if (unitcombatTerritory != null)
            {
                string type = GameMultiLang.GetTraduction(unitcombatBattle.GetType().ToString());
                dropdownUnitSelect.value = dropdownUnitSelect.options.FindIndex(option => option.text == type);
                UpdateText();
            }
            else
            {
                dropdownUnitSelect.value = 0;
                moveUnits.SetActive(false);
            }
        }
    }
    /// <summary>
    /// Updates the values of the drop down according to the amount in which it is had and the amount 
    /// of the current territory. If it is less than or equal to 0 it adds it to a list to disable it 
    /// in the drop down
    /// </summary>
    public void UpdateDropDownValues()
    {
        optionsDropDown.Clear();
        optionsDropDown.Add("SelectUnit");
        optionsDropDown = Utils.instance.GetListUnitCombat(territory, optionsDropDown);
        List<int> disable = new List<int>();
        for (int i = 0; i < optionsDropDown.Count; i++)
        {
            UnitCombat u = territory.GetUnit(optionsDropDown[i]);
            if (u!=null)
            {
                int b = 0;
                if (typeOption== 1)
                {
                    b = u.Quantity - GetUnitsInOtherInAttack(u);
                }
                else
                {
                    b = u.Quantity - GetUnitsInOtherInDefend(u);
                }
                if (b <= 0)
                {
                    disable.Add(i);
                }
            }
        }
        Utils.instance.InitDropdown(dropdownUnitSelect, optionsDropDown);
        dropdownUnitSelect.GetComponent<DropdownController>().indexesToDisable = disable;
    }
    /// <summary>
    /// Returns the number of units in other options of the same unit type (in attack = selectropsMenu)
    /// </summary>
    /// <param name="_unit"></param>
    /// <returns></returns>
    public int GetUnitsInOtherInAttack(UnitCombat _unit)
    {
        int attack = 0;
        for (int i = 0; i < SelectTropsMenu.instance.optionsInAttack.Length; i++)
        {
            UnitCombat u = SelectTropsMenu.instance.optionsInAttack[i].UnitCombatInBattle;
            if (i!= posInBattle && u!= null && u.GetType() == _unit.GetType())
            {
                attack += u.Quantity;
            }
        }
        return attack;
    }
    /// <summary>
    /// Returns the number of units in other options of the same unit type (in defense = in Every terrotory)
    /// </summary>
    /// <param name="_unit"></param>
    /// <returns></returns>
    public int GetUnitsInOtherInDefend(UnitCombat _unit)
    {
        int attack = 0;
        for (int i = 0; i < territory.TroopDefending.Count; i++)
        {
            UnitCombat u = territory.TroopDefending[i];
            if (i != posInBattle && u != null && u.GetType() == _unit.GetType())
            {
                attack += u.Quantity;
            }
        }
        return attack;
    }
    /// <summary>
    /// Updates the value of the drop down in any change of this, also updates 
    /// the value of the text (quantity) and its limit according to the type 
    /// </summary>
    void DropdownUnitSelected()
    {
        if (dropdownUnitSelect.value == 0)
        {
            isSelected = false;
            if (unitcombatBattle != null)
            {
                unitcombatTerritory = null;
                unitcombatBattle = null;
                UpdateMenuByUnits(moveUnits, 0, true);
            }
            moveUnits.SetActive(false);
            if (typeOption == 1)
            {
                SelectTropsMenu.instance.UpdateAllOptions();
            }
            else
            {
                territory.TroopDefending[posInBattle] = new UnitCombat();
                InGameMenuHandler.instance.UpdateAllOptions();
            }
            return;
        }
        
        index = dropdownUnitSelect.value;
        string _type = dropdownUnitSelect.options[index].text;
        string type = GameMultiLang.GetTraductionReverse(_type);
        
        unitcombatBattle = Utils.instance.GetNewUnitCombat(type);
        unitcombatTerritory = territory.GetUnit(type);
        int _limit = 0;
        if (typeOption == 1)
        {
            isSelected = true;
            moveUnits.SetActive(true);
            _limit = unitcombatTerritory.Quantity - GetUnitsInOtherInAttack(unitcombatBattle);
        }
        else
        {
            if (isSelected)
            {
                _limit = territory.TroopDefending[posInBattle].Quantity;
            }
            else
            {

                _limit = unitcombatTerritory.Quantity - GetUnitsInOtherInDefend(unitcombatBattle);
            }
            isSelected = true;
            moveUnits.SetActive(true);

        }
        UpdateMenuByUnits(moveUnits, _limit, true);
        UpdateTroopSelected();
    }

    /// <summary>
    /// Update the limit value in the numeric button
    /// </summary>
    public void UpdateLimit()
    {
        if (isSelected)
        {
            int _limit = 0;
            if (typeOption == 1)
            {
                _limit = unitcombatTerritory.Quantity - GetUnitsInOtherInAttack(unitcombatBattle);
                
            }
            else
            {
                _limit = unitcombatTerritory.Quantity - GetUnitsInOtherInDefend(unitcombatBattle);
            }
            
            UpdateMenuByUnits(moveUnits, _limit, false);
        }
    }
    public void UpdateText()
    {
        if (unitcombatBattle!=null)
        {
            unitcombatBattle.Quantity = territory.TroopDefending[posInBattle].Quantity;
            UpdateMenuByUnits(moveUnits, unitcombatBattle.Quantity, true);
        }
    }
    public void UpdateTroopSelected()
    {
        numberSelected = int.Parse(numberUnitsText.text);
        unitcombatBattle.PositionInBattle = posInBattle;
        unitcombatBattle.Quantity = numberSelected;

        if (typeOption == 1)
        {
            SelectTropsMenu.instance.UpdateAllOptions();
        }
        else
        {
            territory.TroopDefending[posInBattle] = unitcombatBattle;
            InGameMenuHandler.instance.UpdateAllOptions();
        }
        
    }
    private void UpdateNumericButton(Button btn, int _limit, bool _canAttack)
    {
        btn.interactable = _canAttack;
        btn.GetComponent<NumericButton>().limit = _limit;
        btn.GetComponent<NumericButton>().lockButton = _canAttack;
        btn.GetComponent<NumericButton>().pointerDown = false;
    }

    public void UpdateMenuByUnits(GameObject go, int _limit, bool isInitialized, string value = null)
    {
        List<Transform> tr = Utils.instance.GetAllChildren(go.transform);
        UpdateNumericButton(tr[1].GetComponent<Button>(), _limit,true);        //decrease
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
    
    public void OnNumericButton(GameObject go)
    {
        Button increase = go.transform.Find("DecreaseButton").GetComponent<Button>();
        Button decrease = go.transform.Find("IncreaseButton").GetComponent<Button>();
        increase.onClick.AddListener(() => UpdateTroopSelected());
        decrease.onClick.AddListener(() => UpdateTroopSelected());
    }

}
