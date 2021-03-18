using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PolygonCollider2D))]
public class TerritoryHandler : MonoBehaviour
{
    
    public Territory territory;
    private SpriteRenderer sprite;
    private Color32 oldColor;
    private Color32 hoverColor;
   // public Color32 startColor;

    GameObject statsGO;
    public TerritoryStats territoryStats;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
     //   sprite.color = startColor;
        InstantiateStatTerritory();

        
    }
    private void Start()
    {
        
    }

    void InstantiateStatTerritory()
    {
        GameObject canvas = GameObject.Find("Canvas");
        statsGO = Instantiate(Resources.Load("Prefabs/TerritoryStats")) as GameObject;

        statsGO.transform.SetParent(canvas.transform,false);
        statsGO.transform.position = transform.position;
        print(name + " " + statsGO.transform.position);

        territoryStats = statsGO.GetComponent<TerritoryStats>();
        territoryStats.InitalizeStats(territory.getPopulation(), territory.getVelocity());
    }

    private void PopulateTerritory()
    {
        if (territory.getTypePlayer() == Territory.TypePlayer.NONE)
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
        if (territory.getTypePlayer() != Territory.TypePlayer.PLAYER)
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
        territory.SetSelected(true);
        MakeOutline();
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
        print(TerritoryManager.instance.territoryList.Count);
        


    }
    public void Deselect()
    {
        territory.SetSelected(false);
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
}
