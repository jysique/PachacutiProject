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
    }

    void Start()
    {
        AddTerritoryData();
    }

    private void AddTerritoryData()
    {
        GameObject[] tempArray = GameObject.FindGameObjectsWithTag("Territory") as GameObject[];
        foreach (GameObject t in tempArray)
        {
            territoryList.Add(t);
        }
        int a = 0;
        // print(territoryList.Count);
        for (int i = 0; i < territoryList.Count; i++)
        {
            TerritoryHandler territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            if (territoryHandler.territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
            {
                territoryHandler.territory.MilitarBoss = CharacterManager.instance.militarBosses[a];
                a++;
            }

        }
    }

    private void TintTerritory()
    {
        for (int i = 0; i < territoryList.Count; i++)
        {
            TerritoryHandler territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            if (territoryHandler.territory.TypePlayer == Territory.TYPEPLAYER.NONE)
            {
                territoryHandler.TintColorTerritory(new Color32(31, 97, 237, 255));
            }
            if (territoryHandler.territory.TypePlayer == Territory.TYPEPLAYER.BOT)
            {
                territoryHandler.TintColorTerritory(new Color32(122, 75, 82, 255));
            }
            if (territoryHandler.territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
            {
                territoryHandler.TintColorTerritory(new Color32(249, 85, 138, 255));
            }
        }
    }
    public int GetGolds(Territory.TYPEPLAYER type)
    {
        int goldPlayer = 0;
        for (int i = 0; i < territoryList.Count; i++)
        {
            TerritoryHandler territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            if (territoryHandler.territory.TypePlayer == type)
            {
                goldPlayer += territoryHandler.territory.GoldReward;
            }
        }
        return goldPlayer;
    }

    public int GetFood(Territory.TYPEPLAYER type)
    {
        int foodPlayer = 0;
        for (int i = 0; i < territoryList.Count; i++)
        {
            TerritoryHandler territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            if (territoryHandler.territory.TypePlayer == type)
            {
                foodPlayer += territoryHandler.territory.FoodReward;
            }
        }
        return foodPlayer;
    }
    public void ChangeTerritory(string _name)
    {
        for (int i = 0; i < territoryList.Count; i++)
        {
            if (territoryList[i].name == _name)
            {
                TerritoryHandler territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
                territoryHandler.territory.TypePlayer = Territory.TYPEPLAYER.PLAYER;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        TintTerritory();
        CountTerrytorry();
    }

    void CountTerrytorry()
    {
        int tBot = 0;
        int tPlayer = 0;
        for (int i = 0; i < territoryList.Count; i++)
        {
            int index = i;
            TerritoryHandler territoryHandler = territoryList[index].GetComponent<TerritoryHandler>();
            if (territoryHandler.territory.TypePlayer == Territory.TYPEPLAYER.BOT)
            {
                tBot++;
            }else if (territoryHandler.territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
            {
                tPlayer++;
            }
        }
        ConditionEndChapter(tBot, tPlayer);
    }
    void ConditionEndChapter(int a, int b)
    {
        if (a == territoryList.Count)
        {
            GlobalVariables.instance.chapterTxt = "ChapterPachacuti_lose";
            SceneManager.LoadScene("VisualNovelScene");
        }
        else if (b == territoryList.Count)
        {
            print("ganaste");
            GlobalVariables.instance.chapterTxt = "ChapterPachacuti_win";
            SceneManager.LoadScene("VisualNovelScene");
        }
    }
}
