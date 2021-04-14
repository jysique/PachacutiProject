﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TerritoryManager : MonoBehaviour
{
    public static TerritoryManager instance;
    public List<GameObject> territoryList = new List<GameObject>();
    public GameObject territorySelected;
    private int a = 0;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        AddTerritoryData();
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
    public List<TerritoryHandler> GetTerritoryHandlers()
    {
        List<TerritoryHandler> list = new List<TerritoryHandler>();
        for (int i = 0; i < territoryList.Count; i++)
        {
            TerritoryHandler territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            if (territoryHandler.territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
            {
                list.Add(territoryHandler);
            }
        }
        return list;
    }
    public void AddMilitaryBoss()
    {
        
        for (int i = 0; i < territoryList.Count; i++)
        {
            TerritoryHandler territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            if (territoryHandler.territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
            {
                territoryHandler.territory.MilitarBoss = CharacterManager.instance.MilitarBossList.GetMilitaryBoss(a);
                a++;
            }
        }
    }
    public void AddSpecificCharacter(TerritoryHandler territoryHandler, MilitarBoss militarBoss)
    {
        territoryHandler.territory.MilitarBoss = militarBoss;
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
        ConditionEndChapter();
    }

    public int CountTerrytorry(Territory.TYPEPLAYER type)
    {
        int count = 0 ;
        for (int i = 0; i < territoryList.Count; i++)
        {
            int index = i;
            TerritoryHandler territoryHandler = territoryList[index].GetComponent<TerritoryHandler>();
            if (territoryHandler.territory.TypePlayer == type)
            {
               count++;
            }
        }
        return count;
    }
    void ConditionEndChapter()
    {
        int playerCount = CountTerrytorry(Territory.TYPEPLAYER.PLAYER);
        int botCount = CountTerrytorry(Territory.TYPEPLAYER.BOT);
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
        }
    }
}
