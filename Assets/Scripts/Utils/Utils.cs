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
        string _name = units_string[random] + "new";
        UnitCombat unit = CreateNewUnitCombat(_name,units_string[random], quantity);
        Building building = territory.GetBuildingByUnit(unit);

        if (building.Level>0)
        {
            unit = CreateNewUnitCombat(_name,units_string[random],quantity);
        }
        else
        {
            unit = GetUnitCombatRandom(territory,quantity);
        }
        return unit;
    }



    public UnitCombat CreateNewUnitCombat(string charactername, string type, int quantity)
    {
        switch (type)
        {
            case "Sword":
                return new UnitCombat(charactername, "Sword", quantity,  10, 6, 90,0, 1, new string[] { "Axe" }, new string[] { "Lancer" });
            case "Lancer":
                return new UnitCombat(charactername, "Lancer", quantity,  15, 5, 85,0, 1, new string[] { "Sword" }, new string[] { "Axe" });
            case "Axe":
                return new UnitCombat(charactername, "Axe", quantity, 0, 3, 75, 0, 1, new string[] { "Lancer" }, new string[] { "Sword" });
            case "Archer":
                return new UnitCombat(charactername, "Archer", quantity, 10, 3, 70, 0, 2, new string[] { "Scout" }, new string[] { "Archer" });
            case "Scout":
                return new UnitCombat(charactername, "Scout", quantity, 25, 10, 70, 0, 1, new string[] { "Archer" }, new string[] { "Scout" });
            default:
                return null;
        }
    }
    public MilitarChief CreateNewMilitarChief(string origin)
    {
        int age = UnityEngine.Random.Range(20, 30);
        int index = Random.Range(1, 3);
        int influence = UnityEngine.Random.Range(3, 10);
        int opinion = UnityEngine.Random.Range(3, 10);
        int experience = UnityEngine.Random.Range(3, 10);
        int strategyType = Random.Range(0, 6);
        return new MilitarChief(GetRandomName(),age,origin,index,influence,opinion,experience,strategyType);
    }

    private string[] namesList = new string[] { "Unay", "Asiri", "Samin ","Sayri",
                                                "Haylli","Tupac", "Raymi","Wara",
                                                "Qori","Yaku" };
    private string[] lastNamesList = new string[] { "","Illa", "Inka", "Wari", "Amaru",
                                                 "Amaru","Cancha","Ccasani"};

    public string GetRandomName()
    {
        string final_name = "";
        int a = UnityEngine.Random.Range(0, namesList.Length);
        final_name += namesList[a];
        final_name += " ";
        int b = UnityEngine.Random.Range(0, lastNamesList.Length);
        final_name += lastNamesList[b];
        return final_name;
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
        "Sword",
        "Archer",
        "Lancer",
        "Axe",
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
            UnitCombat temp = CreateNewUnitCombat("dummy",units_string[i], 0);
            if (territory.GetBuildingByUnit(temp).Level>0)
            {
                unit.Add(units_string[i]);
            }
        }
        return unit;
    }
}

