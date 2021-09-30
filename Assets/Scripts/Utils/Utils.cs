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

    public UnitCombat GetUnitCombatRandom(Territory territory)
    {
        UnitCombat u = new UnitCombat();
        int random = UnityEngine.Random.Range(0, units_string.Count);
        Building building = territory.GetBuildingByUnit(units_string[random]);

        if (building.Level>0)
        {
            u = GetNewUnitCombat(units_string[random]);
        }
        else
        {
            u = GetUnitCombatRandom(territory);
        }
        return u;
    }

    public Building GetBuildingRandom(Territory territory)
    {

        int random = UnityEngine.Random.Range(0, buildings_string.Count);
        return territory.GetBuilding(buildings_string[random]);
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
        //print(uc.UnitName);
        UnitCombat a = Utils.instance.GetNewUnitCombat(uc.UnitName);
        /*
        if (uc == null)
            print("uc es null");
        if (a == null)
            print("a es null");
        if (uc.UnitName == null)
            print("uc name es null");
        */
        uc.UnitName= a.UnitName;
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

    public List<string> GetListUnitCombat2(Territory territory, List<string> unit)
    {
        for (int i = 0; i < units_string.Count; i++)
        {
            if (territory.GetBuildingByUnit(units_string[i]).Level>0)
            {
                unit.Add(units_string[i]);
            }
        }
        return unit;
    }

    public List<string> GetListUnitCombat(Territory territory, List<string> unitCombatDropOption)
    {
        for (int i = 0; i < units_string.Count; i++)
        {
            if (territory.ListUnitCombat.SearchUnitCombat(units_string[i]))
            {
                unitCombatDropOption.Add(units_string[i]);
            }
        }
        return unitCombatDropOption;
    }
}

