using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Utils : MonoBehaviour
{
    public static Utils instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    public List<Transform> GetAllChildren(Transform aTransform, List<Transform> aList = null)
    {
        if (aList == null)
            aList = new List<Transform>();
        int start = aList.Count;
        for (int n = 0; n < aTransform.childCount; n++)
            aList.Add(aTransform.GetChild(n));
        for (int i = start; i < aList.Count; i++)
        {
            var t = aList[i];
            for (int n = 0; n < t.childCount; n++)
                aList.Add(t.GetChild(n));
        }
        return aList;
    }
    public void InitDropdown(TMP_Dropdown _dropdown, List<string> _items)
    {
        _dropdown.options.Clear();

        foreach (var item in _items)
        {
            _dropdown.options.Add(new TMP_Dropdown.OptionData()
            {
                //text = item
                text = GameMultiLang.GetTraduction(item)
            });
        }
    }
    public int GetIndexTroopOption(TroopOption[] _troopOptions, TroopOption _troopOption)
    {
        return System.Array.IndexOf(_troopOptions, _troopOption);
    }
    
    public UnitCombat GetNewUnitCombat(string _type)
    {
        switch (_type)
        {
            case "Swordsman":
                return new Swordsman();
            case "Lancer":
                return new Lancer();
            case "Axeman":
                return new Axeman();
            case "Archer":
                return new Archer();
            case "Scout":
                return new Scout();
            default:
                return null;
        }
    }
    public Building GetNewBuilding(string _type)
    {
        switch (_type)
        {
            case "Academy":
                return new Academy();
            case "Archery":
                return new Archery();
            case "Barracks":
                return new Barracks();
            case "Castle":
                return new Castle();
            case "Farm":
                return new Farm();
            case "Fortress":
                return new Fortress();
            case "GoldMine":
                return new GoldMine();
            case "Stable":
                return new Stable();
            default:
                return null;
        }
    }


    public UnitCombat Reset(UnitCombat uc)
    {
        UnitCombat a = Utils.instance.GetNewUnitCombat(uc.UnitName);
        uc.CharacterName = a.CharacterName;
        uc.Evasion = a.Evasion;
        uc.UnitName = a.UnitName;
     //   uc.Attack = a.Attack;
        uc.Defense = a.Defense;
        uc.Evasion = a.Evasion;
        uc.Precision = a.Precision;
        uc.Range = a.Range;
        uc.StrongTo = a.StrongTo;
        uc.WeakTo = a.WeakTo;
        return a;
    }

    private List<string> buildings_string = new List<string>()
    {
        "Archery",
        "Farm",
        "GoldMine",
        "Fortress",
        "Academy",
        "Barracks",
        "Castle",
        "Stable",
        
    };
    private List<string> units_string = new List<string>()
    {
        "Swordsman",
        "Archer",
        "Lancer",
        "Axeman",
        "Scout"
        
    };

    public List<string> Buildings_string
    {
        get { return buildings_string; }
    }
    public List<string> Units_string
    {
        get { return units_string; }
    }

    public List<string> GetListBuildings(Territory territory)
    {
        List<string> buildings = new List<string>();
        buildings.Add("CreateBuilding");
        for (int i = 0; i < buildings_string.Count; i++)
        {
            if (territory.GetBuilding(buildings_string[i]).Status<0)
            {
                buildings.Add(buildings_string[i]);
            }
        }
        return buildings;
    }
    public List<string> GetListUnitCombat(Territory territory, List<string> unitCombatDropOption)
    {
        for (int i = 0; i < units_string.Count; i++)
        {
            if (territory.GetUnit(units_string[i]).Quantity > 0)
            {
                unitCombatDropOption.Add(units_string[i]);
            }
        }
        return unitCombatDropOption;
    }
}
