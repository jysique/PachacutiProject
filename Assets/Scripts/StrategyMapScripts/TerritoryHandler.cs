using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PolygonCollider2D))]
public class TerritoryHandler : MonoBehaviour
{
    public int state;
    public Territory territory;
    private SpriteRenderer sprite;
    private Color32 oldColor;
    private Color32 hoverColor;
    // public Color32 startColor;
    [SerializeField]private List<GameObject> adjacentTerritories;

    GameObject statsGO;
    public TerritoryStats territoryStats;

    private void Awake()
    {
        state = 0;
        sprite = GetComponent<SpriteRenderer>();
     //   sprite.color = startColor;
        InstantiateStatTerritory();

        
    }
    private void Start()
    {
        if (territory.GetSelected())
        {
            TerritoryManager.instance.territorySelected = this.gameObject;
            InGameMenuHandler.instance.UpdateMenu();
            //ShowAdjacentTerritories();
        }
    }

    void InstantiateStatTerritory()
    {
        GameObject canvas = GameObject.Find("Canvas");
        statsGO = Instantiate(Resources.Load("Prefabs/TerritoryStats")) as GameObject;

        statsGO.transform.SetParent(canvas.transform,false);
        statsGO.transform.position = transform.position;


        territoryStats = statsGO.GetComponent<TerritoryStats>();
        territoryStats.InitalizeStats(territory.GetPopulation(), territory.GetVelocity());
    }

    private void PopulateTerritory()
    {
        if (territory.GetTypePlayer() == Territory.TypePlayer.NONE)
        {
            territoryStats.SetCanPopulate(false);
        }
        else
        {
            territoryStats.SetCanPopulate(true);
        }
        
    }

    private void Update()
    {
        territory.SetStats(0, 0, territoryStats.population, territoryStats.velocity);
        PopulateTerritory();
    }

    private void OnMouseEnter()
    {
        oldColor = sprite.color;
        if (territory.GetTypePlayer() != Territory.TypePlayer.PLAYER)
        {
            sprite.color = hoverColor;
        } 
        
    }
    private void OnMouseExit()
    {
        sprite.color = oldColor;
    }

    private void OnMouseUpAsButton()
    {
        switch (state)
        {
            case 0:
                territory.SetSelected(true);
                MakeOutline();
                break;
            case 1:
                print("moving");
                InGameMenuHandler.instance.MoveWarriors(this);
                foreach (GameObject t in TerritoryManager.instance.territoryList)
                {
                    t.GetComponent<TerritoryHandler>().state = 0;
                    if (t == TerritoryManager.instance.territorySelected) continue;
                    t.GetComponent<TerritoryHandler>().Deselect();
                    
                }
                break;
            case 2:
                break;

        }
        
    }

    private void OnDrawGizmos()
    {
        territory.name = name;
        this.tag = "Territory";
    }

    public void TintColorTerritory(Color32 _color)
    {
        sprite.color = _color;
        hoverColor = sprite.color;
        hoverColor.a = 180;
    }

    private void MakeOutline()
    {
        foreach (GameObject t in TerritoryManager.instance.territoryList)
        {
            t.GetComponent<TerritoryHandler>().Deselect();
        }
        territory.SetSelected(true);
        this.transform.GetChild(0).gameObject.SetActive(true);
        this.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        TerritoryManager.instance.territorySelected = this.gameObject;
        InGameMenuHandler.instance.UpdateMenu();
        


    }

    public void ShowAdjacentTerritories()
    {
        foreach(GameObject t in adjacentTerritories)
        {
            t.GetComponent<TerritoryHandler>().state = 1;
            t.transform.GetChild(0).gameObject.SetActive(true);
            t.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }
    public void Deselect()
    {
        territory.SetSelected(false);
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
}
