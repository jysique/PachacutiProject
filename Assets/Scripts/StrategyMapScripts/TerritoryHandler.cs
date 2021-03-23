using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(PolygonCollider2D))]
public class TerritoryHandler : MonoBehaviour
{
    private SpriteRenderer sr;
    public int state;
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Material normalMaterial;
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
        sr = this.GetComponent<SpriteRenderer>();
        if (territory.GetSelected())
        {
            TerritoryManager.instance.territorySelected = this.gameObject;
            sr.material = outlineMaterial;
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
                InGameMenuHandler.instance.SendWarriors(this);
                HideAdjacentTerritories();
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
        outlineMaterial.SetColor("_SolidOutline", Color.green);
        sr.material = outlineMaterial;
        sr.sortingOrder = -8;
        TerritoryManager.instance.territorySelected = this.gameObject;
        InGameMenuHandler.instance.UpdateMenu();
        


    }

    public void ShowAdjacentTerritories()
    {
        sr.material = normalMaterial;
        sr.sortingOrder =-9;
        foreach(GameObject t in adjacentTerritories)
        {
            t.GetComponent<TerritoryHandler>().state = 1;
            outlineMaterial.SetColor("_SolidOutline", Color.yellow);
            t.GetComponent<TerritoryHandler>().sr.material = outlineMaterial;
            t.GetComponent<TerritoryHandler>().sr.sortingOrder = -8;
        }
    }
    public void HideAdjacentTerritories()
    {
        foreach (GameObject t in TerritoryManager.instance.territoryList)
        {
            t.GetComponent<TerritoryHandler>().state = 0;
            t.GetComponent<TerritoryHandler>().Deselect();

        }
        TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().MakeOutline();
    }
    public void Deselect()
    {
        territory.SetSelected(false);
        sr.sortingOrder = -8;
        sr.material = normalMaterial;
    }
}
