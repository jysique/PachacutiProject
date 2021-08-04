using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
public class TerritoryManager : MonoBehaviour
{
    public static TerritoryManager instance;
    public List<GameObject> territoryList;
    public GameObject territorySelected;

    public Dictionary<string, List<GameObject>> dictionaryTerritoryAdyacent = new Dictionary<string, List<GameObject>>();
    public Dictionary<string, List<Terrain>> dictionaryAmbience = new Dictionary<string, List<Terrain>>();
    public Dictionary<string, Territory.REGION> dictionaryRegion = new Dictionary<string, Territory.REGION>();
    public Dictionary<string, Territory.TYPEPLAYER> dictionaryTypePlayer = new Dictionary<string, Territory.TYPEPLAYER>();
    public Dictionary<string, List<int>> dictionaryUnitCombats = new Dictionary<string, List<int>>();

    [SerializeField] public List<float> areas = new List<float>();
    private float area_min;
    private float area_max;
    

    private void Awake()
    {
        instance = this;
        territoryList = new List<GameObject>();
        AddTerritoryData();
        ReadAdjacentTerritories();
        
    }

    void Start()
    {
        AddTypePlayerData();
        AddMilitaryBoss();
        AddListTerritoriesAdjacent();
        AddRegionData();
        AddAmbienceData();
        InGameMenuHandler.instance.UpdateMenu();
    }
    private void ReadAdjacentTerritories()
    {
        string file = Resources.Load<TextAsset>("Data/Menu/ListAdyacentTerritories").text;
        List<string> data = new List<string>(file.Split('\n'));
        for (int i = 0; i < data.Count; i++)
        {
            if (!data[i].StartsWith("//"))
            {
                ParseLine(data[i]);
            }            
        }
    }
    /// <summary>
    /// a[0]:territory
    /// a[1]:type player
    /// a[2]:units
    /// a[3]:region 
    /// a[4]:ambientes 
    /// a[5]:territorios adyacentes </summary>
    /// <param name="line"></param>
    void ParseLine(string line)
    {
        string[] all_line_split = line.Split(char.Parse(":"));
        string territory = all_line_split[0];

        Territory.TYPEPLAYER player = (Territory.TYPEPLAYER)Enum.Parse(typeof(Territory.TYPEPLAYER), all_line_split[1].ToUpper());
        Territory.REGION region = (Territory.REGION)Enum.Parse(typeof(Territory.REGION), all_line_split[3].ToUpper());
        List<string> units_string = all_line_split[2].Split(char.Parse(",")).ToList();

        List<string> ambience_string = all_line_split[4].ToUpper().Split(char.Parse(",")).ToList();
        List<string> adyacent = all_line_split[5].Split(char.Parse(",")).ToList();
        
        List<GameObject> goAdyacents = new List<GameObject>();
        for (int i = 0; i < adyacent.Count; i++)
        {
            goAdyacents.Add(SearchTerritoryGameObject(adyacent[i], territory));
        }
        List<Terrain> ambiences = new List<Terrain>();
        for (int i = 0; i < ambience_string.Count; i++)
        {
            ambiences.Add(new Terrain(ambience_string[i]));
        }
        List<int> units = new List<int>();
        for (int i = 0; i < units_string.Count; i++)
        {
            int a = int.Parse(units_string[i]);
            units.Add(a);
        }
        dictionaryTypePlayer.Add(territory, player);
        dictionaryUnitCombats.Add(territory, units);
        dictionaryAmbience.Add(territory, ambiences);
        dictionaryTerritoryAdyacent.Add(territory, goAdyacents);
        dictionaryRegion.Add(territory, region);

    }
    /// <summary>
    /// Add territory to list
    /// </summary>
    private void AddTerritoryData()
    {
        GameObject[] tempArray = GameObject.FindGameObjectsWithTag("Territory") as GameObject[];
        foreach (GameObject t in tempArray)
        {
            territoryList.Add(t);
        }

    }
    /// <summary>
    /// Add unit(militaryBoss) to every territory (NONE,PLAYER,BOT)
    /// </summary>
    public void AddMilitaryBoss()
    {
        for (int i = 0; i < territoryList.Count; i++)
        {
            TerritoryHandler territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            if (territoryHandler.TerritoryStats.Territory.TypePlayer == Territory.TYPEPLAYER.NONE)
            {
                MilitarChief newMilitarBoss = new MilitarChief();
                newMilitarBoss.GetMilitarBoss();
                newMilitarBoss.StrategyType = MilitarChief.TYPESTRAT.DEFENSIVE.ToString();
                territoryHandler.TerritoryStats.Territory.MilitarChiefTerritory = newMilitarBoss;
            }
            else
            {
                MilitarChief newMilitarBoss = new MilitarChief();
                newMilitarBoss.GetMilitarBoss();
                territoryHandler.TerritoryStats.Territory.MilitarChiefTerritory = newMilitarBoss;
            }
        }
    }
    public void AddListTerritoriesAdjacent()
    {
        for (int i = 0; i < territoryList.Count; i++)
        {
            TerritoryHandler territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            territoryHandler.AdjacentTerritories = dictionaryTerritoryAdyacent.Single(s => s.Key == territoryHandler.TerritoryStats.Territory.name).Value;
        }
    }
    public void AddTypePlayerData()
    {
        for (int i = 0; i < territoryList.Count; i++)
        {
            TerritoryHandler territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            Territory _territory = territoryHandler.TerritoryStats.Territory;
            _territory.TypePlayer = dictionaryTypePlayer.Single(s => s.Key == territoryHandler.TerritoryStats.Territory.name).Value;

            for (int k = 0; k < Utils.instance.units_string.Count; k++)
            {
                _territory.GetUnit(Utils.instance.units_string[k]).Quantity = dictionaryUnitCombats.Single(s => s.Key == territoryHandler.TerritoryStats.Territory.name).Value[k];
                if (_territory.GetUnit(Utils.instance.units_string[k]).Quantity > 0)
                {
                    _territory.GetBuilding(_territory.GetUnit(Utils.instance.units_string[k])).ImproveBuilding(1);
                    _territory.GetBuilding(_territory.GetUnit(Utils.instance.units_string[k])).ImproveCostUpgrade(1);
                    _territory.GetBuilding(_territory.GetUnit(Utils.instance.units_string[k])).Status++;
                }
            }

            if (_territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
            {
                territorySelected = territoryList[i];
                WarManager.instance.selected = territoryHandler;
                WarManager.instance.SetWarStatus(territoryHandler.war);

                GlobalVariables.instance.CenterCameraToTerritory(territoryHandler,true);
                _territory.IsClaimed = true;
                _territory.Selected = true;
            }

            UpdateUnitsDeffend(_territory);
        }
    }

    public void UpdateUnitsDeffend(Territory _territory)
    {
        List<string> new_strings = new List<string>();
        for (int j = 0; j < Utils.instance.units_string.Count; j++)
        {
            if (_territory.GetUnit(Utils.instance.units_string[j]).Quantity > 0)
            {
                new_strings.Add(Utils.instance.units_string[j]);
            }
        }
        int count = 0;
        int max_deffend = 4;
        _territory.TroopDefending.Clear();
        for (int j = 0; j < max_deffend; j++)
        {
            if (count < new_strings.Count)
            {
                UnitCombat _a = Utils.instance.GetNewUnitCombat(new_strings[j]);
                _a.PositionInBattle = j;
                _a.Quantity = _territory.GetUnit(new_strings[j]).Quantity;
                _territory.TroopDefending.Add(_a);
                count++;
            }
            else
            {
                UnitCombat u = new UnitCombat();
                _territory.TroopDefending.Add(u);
            }
        }
    }

    public void AddAmbienceData()
    {
        for (int i = 0; i < territoryList.Count; i++)
        {
            TerritoryHandler territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            territoryHandler.TerritoryStats.Territory.TerrainList = dictionaryAmbience.Single(s => s.Key == territoryHandler.TerritoryStats.Territory.name).Value;
        }
    }

    // 4, 8 o 12
    public void AddRegionData()
    {
        for (int i = 0; i < territoryList.Count; i++)
        {
            TerritoryHandler territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            territoryHandler.TerritoryStats.Territory.RegionTerritory = dictionaryRegion.Single(s => s.Key == territoryHandler.TerritoryStats.Territory.name).Value;
            territoryHandler.GetSizeMap();
            float w = territoryHandler.TerritoryStats.Territory.Width;
            float h = territoryHandler.TerritoryStats.Territory.Height;
            float area = w * h;
            areas.Add(area);
        }
        area_min = areas.Min();
        area_max = areas.Max();
    }
    public int ClassifyTerritory(Territory territory)
    {
        float x = (area_max - area_min) / 4;
        float territory_area = territory.Width * territory.Height;
        float b = territory_area - area_min;
        float c = b / x;
        int maxbuilding = 3 + (int)Math.Round(c);
        
        return maxbuilding;
    }

    /// <summary>
    /// Tint territory according to its type (NONE,PLAYER or BOTS)
    /// </summary>
    private void TintTerritory()
    {
        for (int i = 0; i < territoryList.Count; i++)
        {

            TerritoryHandler territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            Territory.TYPEPLAYER tp = territoryHandler.TerritoryStats.Territory.TypePlayer;
            switch (tp)
            {
                case Territory.TYPEPLAYER.NONE:
                    territoryHandler.TintColorTerritory(new Color32(96, 142, 118, 255));
                    break;
                case Territory.TYPEPLAYER.PLAYER:
                    territoryHandler.TintColorTerritory(new Color32(249, 85, 138, 255));
                    break;
                case Territory.TYPEPLAYER.BOT:
                    territoryHandler.TintColorTerritory(new Color32(122, 75, 82, 255));
                    break;
                case Territory.TYPEPLAYER.BOT2:
                    territoryHandler.TintColorTerritory(new Color32(206, 209,68, 255));
                    break;
                case Territory.TYPEPLAYER.BOT3:
                    territoryHandler.TintColorTerritory(new Color32(177, 207, 194, 255));
                    break;
                case Territory.TYPEPLAYER.BOT4:
                    territoryHandler.TintColorTerritory(new Color32(82, 110, 123, 255));
                    break;
                case Territory.TYPEPLAYER.WASTE:
                    territoryHandler.TintColorTerritory(new Color32(71, 75, 78, 255));
                    break;
            }
            
        }
    }
    public void MakeOutline(TerritoryHandler territoryHandler)
    {
        foreach (GameObject t in TerritoryManager.instance.territoryList)
        {
            t.GetComponent<TerritoryHandler>().Deselect();
            t.GetComponent<TerritoryHandler>().state = 0;
        }
        territoryHandler.TerritoryStats.Territory.Selected = true;
        territoryHandler.OutlineMaterial.SetColor("_SolidOutline", Color.green);
        territoryHandler.SpriteRender.material = territoryHandler.OutlineMaterial;
        territoryHandler.SpriteRender.sortingOrder = -8;
        territorySelected = territoryHandler.gameObject;
      //  InGameMenuHandler.instance.UpdateMenu();
    }
    /// <summary>
    /// Returns a territory gameObject by _name 
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_a"></param>
    /// <returns></returns>
    private GameObject SearchTerritoryGameObject(string _name,string _a)
    {
       // print("name" + _name + " a " +_a);
        GameObject a = GameObject.Find(_name);
        if (a == null)
        {
            a = GameObject.Find(_name.Remove(_name.Length - 1));
            
        }
        return a;
    }
    public void ChangeTerritoryToType(string _name, Territory.TYPEPLAYER type)
    {
        for (int i = 0; i < territoryList.Count; i++)
        {
            if (territoryList[i].name == _name)
            {
                TerritoryHandler territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
                territoryHandler.TerritoryStats.Territory.TypePlayer = type;
            }
        }
    }
    public void ChangeStateTerritory(int _state)
    {
        foreach (GameObject t in territoryList)
        {
            if (t.GetComponent<TerritoryHandler>().state != 1) t.GetComponent<TerritoryHandler>().state = _state;
        }
    }

    // Update is called once per frame
    void Update()
    {
        TintTerritory();
        ConditionEndChapter();
    }
    /// <summary>
    /// Count territories by type player
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public int CountTerrytorry(Territory.TYPEPLAYER type)
    {
        int count = 0 ;
        for (int i = 0; i < territoryList.Count; i++)
        {
            int index = i;
            TerritoryHandler territoryHandler = territoryList[index].GetComponent<TerritoryHandler>();
            if (territoryHandler.TerritoryStats.Territory.TypePlayer == type)
            {
               count++;
            }
        }
        return count;
    }
    /// <summary>
    /// Conditions to end the game
    /// </summary>
    public void ConditionEndChapter()
    {
        int playerCount = CountTerrytorry(Territory.TYPEPLAYER.PLAYER);
        if (playerCount == territoryList.Count)
        {
            if (GlobalVariables.instance != null)
            {
                GlobalVariables.instance.SetChapterTxt("win");
            }
            SceneManager.LoadScene(2);
        }
        else if (playerCount == 0)
        {
            if (GlobalVariables.instance != null)
            {
                GlobalVariables.instance.SetChapterTxt("lose");
            }
            SceneManager.LoadScene(2);
        }
    }
    public List<TerritoryHandler> GetAllTerritoriesHanlder()
    {
        List<TerritoryHandler> territoriesPlayer = new List<TerritoryHandler>();
        for (int i = 0; i < territoryList.Count; i++)
        {
            TerritoryHandler _territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            territoriesPlayer.Add(_territoryHandler);
        }
        return territoriesPlayer;
    }

    /// <summary>
    /// Returns a list of territories handler by type player
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<TerritoryHandler> GetTerritoriesHandlerByTypePlayer(Territory.TYPEPLAYER type)
    {
        List<TerritoryHandler> territoriesPlayer = new List<TerritoryHandler>();
        for (int i = 0; i < territoryList.Count; i++)
        {
            TerritoryHandler _territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            if (_territoryHandler.TerritoryStats.Territory.TypePlayer == type)
            {
                territoriesPlayer.Add(_territoryHandler);
            }
        }
        return territoriesPlayer;
    }
    public TerritoryHandler GetTerritoriesHandlerByName(string nameTerritory)
    {
        TerritoryHandler territoryHandler = null;
        for (int i = 0; i < territoryList.Count; i++)
        {
            TerritoryHandler _territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            if (_territoryHandler.TerritoryStats.Territory.name == nameTerritory)
            {
                territoryHandler = _territoryHandler;
            }
        }
        return territoryHandler;
    }
     public List<Territory> GetTerritoriesByTypePlayer(Territory.TYPEPLAYER type)
    {
        List<Territory> territoriesPlayer = new List<Territory>();
        for (int i = 0; i < territoryList.Count; i++)
        {
            Territory _territoryHandler = territoryList[i].GetComponent<TerritoryHandler>().TerritoryStats.Territory;
            if (_territoryHandler.TypePlayer == type)
            {
                territoriesPlayer.Add(_territoryHandler);
            }
        }
        return territoriesPlayer;
    }
    /// <summary>
    /// Returns a list of territories handler by zone of territory
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<TerritoryHandler> GetTerritoriesByZoneTerritory(Territory.REGION region)
    {
        List<TerritoryHandler> territoriesZone = new List<TerritoryHandler>();
        for (int i = 0; i < territoryList.Count; i++)
        {
            TerritoryHandler _territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            if (_territoryHandler.TerritoryStats.Territory.RegionTerritory == region)
            {
                territoriesZone.Add(_territoryHandler);
            }
        }
        return territoriesZone;
    }
    /// <summary>
    /// Get a random territory by a type player
    /// </summary>
    /// <param name="typePlayer"></param>
    /// <returns></returns>
    public TerritoryHandler GetTerritoryRandom(Territory.TYPEPLAYER typePlayer)
    {
        List<TerritoryHandler> list = GetTerritoriesHandlerByTypePlayer(typePlayer);
        int r = UnityEngine.Random.Range(0, list.Count);
        TerritoryHandler territoryHandler =  list[r];
        return territoryHandler;
    }
    public TerritoryHandler GetTerritoryHandlerByTerritory(Territory territory)
    {
        for (int i = 0; i < territoryList.Count; i++)
        {
            TerritoryHandler _territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            if (_territoryHandler.TerritoryStats.Territory.name == territory.name)
            {
                return _territoryHandler;
            }
        }
        return null;
    }

    /// <summary>
    /// Returns the empire of a territory
    /// </summary>
    /// <param name="territory"></param>
    /// <returns></returns>
    public string GetTerritoryEmpire(Territory territory)
    {
        string result = "";
        switch (territory.TypePlayer)
        {
            case Territory.TYPEPLAYER.PLAYER:
                result = "Inca";
                break;
            case Territory.TYPEPLAYER.NONE:
                result = "No empire";
                break;
            case Territory.TYPEPLAYER.BOT:
                result = "Chanca";
                break;
            case Territory.TYPEPLAYER.BOT2:
                result = "Moche";
                break;
            case Territory.TYPEPLAYER.BOT3:
                result = "Chavin";
                break;
            case Territory.TYPEPLAYER.BOT4:
                result = "Pendiente";
                break;

        }
        return result;
    }
    public int GetOveralRateResource(Territory.TYPEPLAYER typePlayer, string element)
    {
        int rate = 0;
        List<TerritoryHandler> list = GetTerritoriesHandlerByTypePlayer(typePlayer);
        for (int i = 0; i < list.Count; i++)
        {
            if (element == "channel")
            {
                rate += list[i].TerritoryStats.Territory.FarmTerritory.WorkersChannel / list[i].TerritoryStats.Territory.PerPeople;
            }
            else if (element == "goldmine")
            {
                rate += list[i].TerritoryStats.Territory.GoldMineTerritory.WorkersMine / list[i].TerritoryStats.Territory.PerPeople;
            }
        }
        return rate;
    }

    public bool IsLimit(TerritoryHandler territory)
    {
        List<GameObject> adjacentTerritories = territory.AdjacentTerritories;
        foreach (GameObject t in adjacentTerritories)
        {
            if(t.GetComponent<TerritoryHandler>().TerritoryStats.Territory.TypePlayer != territory.TerritoryStats.Territory.TypePlayer)
            {
                return true;
            }
        }
        return false;
    }
}

public class info
{
    float area;
    float width;
    float height;
}