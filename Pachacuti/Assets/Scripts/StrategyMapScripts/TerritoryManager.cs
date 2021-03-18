using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerritoryManager : MonoBehaviour
{
    public static TerritoryManager instance;
    public List<GameObject> territoryList = new List<GameObject>();

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
        
    }
    private void TintTerritory()
    {
        for (int i = 0; i < territoryList.Count; i++)
        {
            TerritoryHandler territoryHandler = territoryList[i].GetComponent<TerritoryHandler>();
            if (territoryHandler.territory.typePlayer == Territory.TypePlayer.NONE)
            {
                territoryHandler.TintColorTerritory(new Color32(31, 97, 237, 128));
            }
            if (territoryHandler.territory.typePlayer == Territory.TypePlayer.BOT)
            {
                territoryHandler.TintColorTerritory(new Color32(122, 75, 82, 128));
            }
            if (territoryHandler.territory.typePlayer == Territory.TypePlayer.PLAYER)
            {
                territoryHandler.TintColorTerritory(new Color32(249, 85, 138, 128));
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
                territoryHandler.territory.typePlayer = Territory.TypePlayer.PLAYER;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        TintTerritory();
    }
}
