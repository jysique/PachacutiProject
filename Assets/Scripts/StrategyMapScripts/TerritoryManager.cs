using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TerritoryManager : MonoBehaviour
{
    public static TerritoryManager instance;
    public List<GameObject> territoryList = new List<GameObject>();
    public GameObject territorySelected;
    private void Awake()
    {
        instance = this;
        AddTerritoryData();

    }

    void Start()
    {
        AddMilitaryBoss();
    }
    
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
                MilitarBoss newMilitarBoss = new MilitarBoss();
                newMilitarBoss.GetMilitarBoss();
                newMilitarBoss.StrategyType = MilitarBoss.TYPESTRAT.DEFENSIVE.ToString();
                territoryHandler.territoryStats.territory.MilitarBossTerritory = newMilitarBoss;
                //a++;
            }
            else
            {
                MilitarBoss newMilitarBoss = new MilitarBoss();
                newMilitarBoss.GetMilitarBoss();
                territoryHandler.territoryStats.territory.MilitarBossTerritory = newMilitarBoss;
            }
        }
    }
    private void TintTerritory()
    {
        for (int i = 0; i < territoryList.Count; i++)
        {
            TerritoryHandler territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            if (territoryHandler.territoryStats.territory.TypePlayer == Territory.TYPEPLAYER.NONE)
            {
                territoryHandler.TintColorTerritory(new Color32(31, 97, 237, 255));
            }
            if (territoryHandler.territoryStats.territory.TypePlayer == Territory.TYPEPLAYER.BOT)
            {
                territoryHandler.TintColorTerritory(new Color32(122, 75, 82, 255));
            }
            if (territoryHandler.territoryStats.territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
            {
                territoryHandler.TintColorTerritory(new Color32(249, 85, 138, 255));
            }
        }
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

    // Update is called once per frame
    void Update()
    {
        TintTerritory();
        ConditionEndChapter();
    }

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
    public void ConditionEndChapter()
    {
        int playerCount = CountTerrytorry(Territory.TYPEPLAYER.PLAYER);
        int botCount = CountTerrytorry(Territory.TYPEPLAYER.BOT);
        /*
        if (playerCount == territoryList.Count)
        {
            if (GlobalVariables.instance != null)
            {
                GlobalVariables.instance.SetChapterTxt("win");
            }
            SceneManager.LoadScene("VisualNovelScene");
        }
        else if (botCount == territoryList.Count)
        {
            if (GlobalVariables.instance != null)
            {
                GlobalVariables.instance.SetChapterTxt("lose");
            }
            SceneManager.LoadScene("VisualNovelScene");
        }*/
        if (playerCount == territoryList.Count)
        {
            if (GlobalVariables.instance != null)
            {
                GlobalVariables.instance.SetChapterTxt("win");
            }
            SceneManager.LoadScene("VisualNovelScene");
        }else if (playerCount == 0)
        {
            if (GlobalVariables.instance != null)
            {
                GlobalVariables.instance.SetChapterTxt("lose");
            }
            SceneManager.LoadScene("VisualNovelScene");
        }
    }

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
    public TerritoryHandler GetTerritoryRandom()
    {
        List<TerritoryHandler> list = GetTerritoriesByTypePlayer(Territory.TYPEPLAYER.PLAYER);
        int r = UnityEngine.Random.Range(0, list.Count);
        TerritoryHandler territoryHandler =  list[r];
        return territoryHandler;
    }
}
