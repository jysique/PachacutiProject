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
    public Dictionary<string, List<Terrain>> dictionaryAmbienceAttack = new Dictionary<string, List<Terrain>>();
    public Dictionary<string, List<Terrain>> dictionaryAmbienceDeffend = new Dictionary<string, List<Terrain>>();
    public Dictionary<string, string> dictionaryCivilization = new Dictionary<string, string>();
    public Dictionary<string, Territory.REGION> dictionaryRegion = new Dictionary<string, Territory.REGION>();
    public Dictionary<string, Territory.TYPEPLAYER> dictionaryTypePlayer = new Dictionary<string, Territory.TYPEPLAYER>();
    public Dictionary<string, List<string>> dictionaryUnitCombats = new Dictionary<string, List<string>>();

    public List<float> areas = new List<float>();
    private float area_min;
    private float area_max;

    string filename = "";
    string time;

    private void Awake()
    {
        instance = this;
        territoryList = new List<GameObject>();
        AddTerritoryData();
        ReadTerritoriesData();
        civilizations = FileHandler.ReadListFromJSON_Resource<Civilization>("civilization");
    }
    [SerializeField]
    List<Player> players = new List<Player>();
    [SerializeField]
    List<Civilization> civilizations = new List<Civilization>();
    
    void TestJson()
    {
       //print("tiempo" + System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
        if (filename.Contains(".json"))
        {
            players = FileHandler.ReadListFromJSON<Player>(filename);
        }
        else
        {
            players = FileHandler.ReadListFromJSON_Resource<Player>("players");
            time = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
            filename = time + ".json";
        }
        FileHandler.SaveToJSON<Player>(players, filename);
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
    private void ReadTerritoriesData()
    {
        string file = Resources.Load<TextAsset>("Data/Menu/territories").text;
        List<string> data = new List<string>(file.Split('\n'));
        for (int i = 0; i < data.Count; i++)
        {
            if (!data[i].StartsWith("//"))
            {
                ParseLine(data[i]);
            }
        }
        file = Resources.Load<TextAsset>("Data/Menu/players_info").text;
        data = new List<string>(file.Split('\n'));
        for (int i = 0; i < data.Count; i++)
        {
            if (!data[i].StartsWith("//"))
            {
                ParseInfo(data[i]);
            }
        }
    }
    /// <summary>
    /// a[0]:territory
    /// a[1]:region 
    /// a[2]:ambientes 
    /// a[3]:territorios adyacentes </summary>
    /// <param name="line"></param>
    void ParseLine(string line)
    {

        string[] all_line_split = line.Split(char.Parse(":"));
        string territory = all_line_split[0];

        Territory.REGION region = (Territory.REGION)Enum.Parse(typeof(Territory.REGION), all_line_split[1].ToUpper());
        dictionaryRegion.Add(territory, region);


        string[] long_ambience = all_line_split[2].Split(char.Parse("|"));
        List<string> ambience_string1 = long_ambience[0].ToUpper().Split(char.Parse(",")).ToList();
        List<Terrain> terrains = new List<Terrain>();
        AddTerrainsToList(ambience_string1, terrains);
        dictionaryAmbienceAttack.Add(territory, terrains);

        terrains = new List<Terrain>();
        List<string> ambience_string2 = long_ambience[1].ToUpper().Split(char.Parse(",")).ToList();
        AddTerrainsToList(ambience_string2, terrains);
        dictionaryAmbienceDeffend.Add(territory, terrains);

        List<string> adyacent = all_line_split[3].Split(char.Parse(",")).ToList();
        List<GameObject> goAdyacents = new List<GameObject>();
        for (int i = 0; i < adyacent.Count; i++)
        {
            goAdyacents.Add(SearchTerritoryGameObject(adyacent[i], territory));
        }
        dictionaryTerritoryAdyacent.Add(territory, goAdyacents);

    }
   
    /// <summary>
    /// a[0]:territory
    /// a[1]:type player
    /// a[2]:civ
    /// a[3]:units
    /// </summary>
    /// <param name="line"></param>
    private void ParseInfo(string line)
    {
        string[] all_line_split = line.Split(char.Parse(":"));
        string territory = all_line_split[0];

        Territory.TYPEPLAYER player = (Territory.TYPEPLAYER)Enum.Parse(typeof(Territory.TYPEPLAYER), all_line_split[1].ToUpper());
        string civ = all_line_split[2];
        List<string> units_string = all_line_split[3].Split(char.Parse("|")).ToList();

        List<string> units = new List<string>();
        for (int i = 0; i < units_string.Count; i++)
        {
            units.Add(units_string[i]);
        }
        dictionaryTypePlayer.Add(territory, player);
        dictionaryCivilization.Add(territory, civ);
        dictionaryUnitCombats.Add(territory, units);
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
            MilitarChief newMilitarBoss = Utils.instance.CreateNewMilitarChief(territoryHandler.Territory.name);
            if (territoryHandler.Territory.TypePlayer == Territory.TYPEPLAYER.NONE)
                newMilitarBoss.StrategyType = MilitarChief.TYPESTRAT.DEFENSIVE;
            territoryHandler.Territory.MilitarChiefTerritory = newMilitarBoss;
        }
    }
    public void AddListTerritoriesAdjacent()
    {
        for (int i = 0; i < territoryList.Count; i++)
        {
            TerritoryHandler territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            territoryHandler.AdjacentTerritories = dictionaryTerritoryAdyacent.Single(s => s.Key == territoryHandler.Territory.name).Value;
        }
    }
    public void AddTypePlayerData()
    {
        for (int i = 0; i < territoryList.Count; i++)
        {
            TerritoryHandler territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            Territory _territory = territoryHandler.Territory;
            _territory.TypePlayer = dictionaryTypePlayer.Single(s => s.Key == territoryHandler.Territory.name).Value;

            string _civ = dictionaryCivilization.Single(s => s.Key == territoryHandler.Territory.name).Value;
            _territory.Civilization = civilizations.Find(x => x.Name == _civ);

            for (int k = 0; k < Utils.instance.Units_string.Count; k++)
            {
                string q = dictionaryUnitCombats.Single(s => s.Key == territoryHandler.Territory.name).Value[k];
             //   print(q);
                List<string> list = q.Split(char.Parse(",")).ToList();
                for (int j = 0; j < list.Count; j++)
                {
                    
                    int _quantity = int.Parse(list[j]);
                    if (_quantity>0)
                    {
                        string _name = Utils.instance.Units_string[k] + j.ToString();
                        UnitCombat new_unit = Utils.instance.CreateNewUnitCombat(_name,Utils.instance.Units_string[k], _quantity);
                        new_unit.IsAvailable = true;
                        _territory.ListUnitCombat.AddUnitCombat(new_unit);
                    }
                }

            }
            for (int k = 0; k < _territory.ListUnitCombat.UnitCombats.Count; k++)
            {
                Building _building = _territory.GetBuilding(_territory.GetBuildingByUnit(_territory.ListUnitCombat.UnitCombats[k]));
                if (_building.Level <1)
                {
                    _building.ImproveManyLevels(1, _territory);
                    _building.ImproveCostUpgrade(1);
                    _building.Status++;
                }
            }
            if (_territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
            {
                territorySelected = territoryList[i];
                WarManager.instance.selected = territoryHandler;
                WarManager.instance.SetWarStatus(territoryHandler.war);

                GlobalVariables.instance.CenterCameraToTerritory(territoryHandler, true);

                _territory.IsClaimed = true;
                _territory.Selected = true;
            }
        }
    }
    public void AddAmbienceData()
    {
        for (int i = 0; i < territoryList.Count; i++)
        {
            TerritoryHandler territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            List<Terrain> list_terrains = dictionaryAmbienceDeffend.Single(s => s.Key == territoryHandler.Territory.name).Value;
            territoryHandler.Territory.TerrainDeffendList = dictionaryAmbienceDeffend.Single(s => s.Key == territoryHandler.Territory.name).Value;
            territoryHandler.Territory.TerrainAttackList = dictionaryAmbienceAttack.Single(s => s.Key == territoryHandler.Territory.name).Value;
        }
    }

    // 4, 8 o 12
    public void AddRegionData()
    {
        for (int i = 0; i < territoryList.Count; i++)
        {
            TerritoryHandler territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            territoryHandler.Territory.RegionTerritory = dictionaryRegion.Single(s => s.Key == territoryHandler.Territory.name).Value;
            territoryHandler.GetSizeMap();
            float w = territoryHandler.Territory.Width;
            float h = territoryHandler.Territory.Height;
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
            Territory.TYPEPLAYER tp = territoryHandler.Territory.TypePlayer;
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
                    territoryHandler.TintColorTerritory(new Color32(206, 209, 68, 255));
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
        territoryHandler.Territory.Selected = true;
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
    private GameObject SearchTerritoryGameObject(string _name, string _a)
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
                territoryHandler.Territory.TypePlayer = type;
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
        int count = 0;
        for (int i = 0; i < territoryList.Count; i++)
        {
            int index = i;
            TerritoryHandler territoryHandler = territoryList[index].GetComponent<TerritoryHandler>();
            if (territoryHandler.Territory.TypePlayer == type)
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
        string a = GlobalVariables.instance.GovernorChoose.CharacterName;
        int playerCount = CountTerrytorry(Territory.TYPEPLAYER.PLAYER);
        if (playerCount == territoryList.Count)
        {
            if (GlobalVariables.instance != null)
            {
                GlobalVariables.instance.tittle = "Win";
            }
            SceneManager.LoadScene(2);
        }
        else if (playerCount == 0)
        {
            if (GlobalVariables.instance != null)
            {
                GlobalVariables.instance.tittle = "Lose";
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
            if (_territoryHandler.Territory.TypePlayer == type)
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
            if (_territoryHandler.Territory.name == nameTerritory)
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
            Territory _territoryHandler = territoryList[i].GetComponent<TerritoryHandler>().Territory;
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
            if (_territoryHandler.Territory.RegionTerritory == region)
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
        TerritoryHandler territoryHandler = list[r];
        return territoryHandler;
    }
    public TerritoryHandler GetTerritoryHandlerByTerritory(Territory territory)
    {
        for (int i = 0; i < territoryList.Count; i++)
        {
            TerritoryHandler _territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            if (_territoryHandler.Territory.name == territory.name)
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
                //  rate += list[i].Territory.FarmTerritory.WorkersChannel / list[i].Territory.PerPeople;
                rate += list[i].Territory.FarmTerritory.LimitUnits / list[i].Territory.PerPeople;
            }
            else if (element == "goldmine")
            {
                //  rate += list[i].Territory.GoldMineTerritory.WorkersMine / list[i].Territory.PerPeople;
                rate += list[i].Territory.GoldMineTerritory.LimitUnits / list[i].Territory.PerPeople;
            }
        }
        return rate;
    }
    public bool IsLimit(TerritoryHandler territory)
    {
        List<GameObject> adjacentTerritories = territory.AdjacentTerritories;
        foreach (GameObject t in adjacentTerritories)
        {
            if (t.GetComponent<TerritoryHandler>().Territory.TypePlayer != territory.Territory.TypePlayer)
            {
                return true;
            }
        }
        return false;
    }
    private void AddTerrainsToList(List<string> _string, List<Terrain> _terrains)
    {
        for (int i = 0; i < _string.Count; i++)
        {
            string a = _string[i];
            a = a.Replace("(", "").Replace(")", "");
            string[] b = a.Split('-');
            _terrains.Add(new Terrain(b[0], int.Parse(b[1])));
        }
    }
}
