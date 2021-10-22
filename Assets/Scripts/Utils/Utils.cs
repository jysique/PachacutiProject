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

    public UnitCombat GetUnitCombatRandom(Territory territory,int quantity)
    {
        
        int random = UnityEngine.Random.Range(0, units_string.Count);
        UnitCombat unit = CreateNewUnitCombat(units_string[random], 0);
        Building building = territory.GetBuildingByUnit(unit);

        if (building.Level>0)
        {
            unit = CreateNewUnitCombat(units_string[random],quantity);
        }
        else
        {
            unit = GetUnitCombatRandom(territory,quantity);
        }
        return unit;
    }

    public Building GetBuildingRandom(Territory territory)
    {

        int random = UnityEngine.Random.Range(0, buildings_string.Count);
        return territory.GetBuilding(buildings_string[random]);
    }

    public UnitCombat CreateNewUnitCombat(string _type, int quantity)
    {
        switch (_type)
        {
            case "Swordsman":
                return new UnitCombat("Swordsman", quantity, "warriors", "sword", 10, 6, 90, 1, new string[] { "Axeman" }, new string[] { "Lancer" });
            case "Lancer":
                return new UnitCombat("Lancer", quantity, "warriors", "spear", 15, 5, 85, 1, new string[] { "Swordsman" }, new string[] { "Axeman" });
            case "Axeman":
                return new UnitCombat("Axeman", quantity, "warriors", "axe", 20, 3, 75, 1, new string[] { "Lancer" }, new string[] { "Swordsman" });
            case "Archer":
                return new UnitCombat("Archer", quantity, "warriors", "archer", 10, 3, 70, 2, new string[] { "Scout" }, new string[] { "Archer" });
            case "Scout":
                return new UnitCombat("Scout", quantity, "warriors", "horseman", 25, 10, 70, 1, new string[] { "Archer" }, new string[] { "Scout" });
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

    public List<string> GetListUnitCombat(Territory territory, List<string> unit)
    {
        for (int i = 0; i < units_string.Count; i++)
        {
            UnitCombat temp = CreateNewUnitCombat(units_string[i], 0);
            if (territory.GetBuildingByUnit(temp).Level>0)
            {
                unit.Add(units_string[i]);
            }
        }
        return unit;
    }
}

