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
    private void Awake()
    {
        instance = this;
        territoryList = new List<GameObject>();
        AddTerritoryData();
        ReadAdjacentTerritories();
    }

    void Start()
    {
        AddMilitaryBoss();
        
    }
    public Dictionary<string,List<GameObject>> dictionary = new Dictionary<string,List<GameObject>>();
    private void ReadAdjacentTerritories()
    {
        string file = Resources.Load<TextAsset>("Data/Menu/ListAdyacentTerritories").text;
        List<string> data = new List<string>(file.Split('\n'));
        for (int i = 0; i < data.Count; i++)
        {
            ParseLine(data[i]);
        }
    }
    void ParseLine(string line)
    {
        string[] a = line.Split(char.Parse(":"));
        List<string> b = a[1].Split(char.Parse(",")).ToList();
        List<GameObject> c = new List<GameObject>();
        for (int i = 0; i < b.Count; i++)
        {
           c.Add(SearchTerritoryGameObject(b[i],a[0]));
        }
        dictionary.Add(a[0], c);
           
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
            if (territoryHandler.territoryStats.territory.TypePlayer == Territory.TYPEPLAYER.BOT)
            {
                MilitarChief newMilitarBoss = new MilitarChief();
                newMilitarBoss.GetMilitarBoss();
                newMilitarBoss.StrategyType = MilitarChief.TYPESTRAT.DEFENSIVE.ToString();
                territoryHandler.territoryStats.territory.MilitarChiefTerritory = newMilitarBoss;
                //a++;
            }
            else
            {
                MilitarChief newMilitarBoss = new MilitarChief();
                newMilitarBoss.GetMilitarBoss();
                territoryHandler.territoryStats.territory.MilitarChiefTerritory = newMilitarBoss;
            }
        }
    }
    /// <summary>
    /// Tint territory according to its type (NONE,PLAYER or BOTS)
    /// </summary>
    private void TintTerritory()
    {
        for (int i = 0; i < territoryList.Count; i++)
        {

            TerritoryHandler territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            Territory.TYPEPLAYER tp = territoryHandler.territoryStats.territory.TypePlayer;
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
            }
            
        }
    }
    /// <summary>
    /// Returns a territory gameObject by _name 
    /// </summary>
    /// <param name="_name"></param>
    /// <param name="_a"></param>
    /// <returns></returns>
    private GameObject SearchTerritoryGameObject(string _name,string _a)
    {
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
                territoryHandler.territoryStats.territory.TypePlayer = type;
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
            if (territoryHandler.territoryStats.territory.TypePlayer == type)
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
    /// <summary>
    /// Returns a list of territories handler by type player
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public List<TerritoryHandler> GetTerritoriesByTypePlayer(Territory.TYPEPLAYER type)
    {
        List<TerritoryHandler> territoriesPlayer = new List<TerritoryHandler>();
        for (int i = 0; i < territoryList.Count; i++)
        {
            TerritoryHandler _territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            if (_territoryHandler.territoryStats.territory.TypePlayer == type)
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
            if (_territoryHandler.territoryStats.territory.RegionTerritory == region)
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
        List<TerritoryHandler> list = GetTerritoriesByTypePlayer(typePlayer);
        int r = UnityEngine.Random.Range(0, list.Count);
        TerritoryHandler territoryHandler =  list[r];
        return territoryHandler;
    }

}
