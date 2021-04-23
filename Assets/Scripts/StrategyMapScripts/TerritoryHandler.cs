
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(PolygonCollider2D))]
public class TerritoryHandler : MonoBehaviour
{
    private SpriteRenderer sr;
    public int state;
    public bool war;
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Material normalMaterial;
    public Territory territory;
    public SpriteRenderer sprite;
    private Color32 oldColor;
    private Color32 hoverColor;
    // public Color32 startColor;
    [SerializeField]private List<GameObject> adjacentTerritories;

    GameObject statsGO;
    public TerritoryStats territoryStats;

    private void Awake()
    {
        war = false;
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
            WarManager.instance.selected = this;
            WarManager.instance.SetWarStatus(this.war);
            //ShowAdjacentTerritories();
        }
    }

    void InstantiateStatTerritory()
    {
        GameObject canvas = GameObject.Find("Canvas");
        statsGO = Instantiate(Resources.Load("Prefabs/TerritoryStats")) as GameObject;

        statsGO.transform.SetParent(GameObject.Find("StatsContainer").transform,false);
        statsGO.transform.position = transform.position;


        territoryStats = statsGO.GetComponent<TerritoryStats>();
        territoryStats.InitalizeStats(territory);
    }

    private void PopulateTerritory()
    {
        if (territory.TypePlayer != Territory.TYPEPLAYER.NONE && territory.Population < territory.LimitPopulation)
        {
            territoryStats.SetCanPopulate(true);
        }
        else
        {
            territoryStats.SetCanPopulate(false);
        }
    }
    private void Update()
    {
        territory.SetStats(territoryStats);
        PopulateTerritory();
    }
    private void FixedUpdate()
    {
        if(territory.TypePlayer == Territory.TYPEPLAYER.BOT)
        {
           
            int prob = Random.Range(0, 401);
            if (prob < 1 && this.territory.Population > 2)
            {
                EnemyMoveWarriors();
            }
        }
    }

    private void EnemyMoveWarriors()
    {
        
        int i = Random.Range(0, adjacentTerritories.Count);
        int warriorsToSend = Random.Range(3, this.territory.Population);
        TerritoryHandler territoryToAttack = adjacentTerritories[i].GetComponent<TerritoryHandler>();
        InGameMenuHandler.instance.SendWarriors(this, territoryToAttack, warriorsToSend);
    }

    private void OnMouseOver()
    {
        
        if (Input.GetMouseButtonDown(1) && state == 0)
        {
            territory.SetSelected(true);
            MakeOutline();
            bool ca = false;
            TerritoryHandler selected = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
            for (int i = 0; i < selected.adjacentTerritories.Count; i++)
            {
                //print(selected.adjacentTerritories[i]);
                //print(this);
                if (selected.adjacentTerritories[i].GetComponent<TerritoryHandler>() == this) ca = true;

            }
            if (selected.territory.TypePlayer != Territory.TYPEPLAYER.PLAYER) ca = false;
            if (selected == this && territory.TypePlayer == Territory.TYPEPLAYER.PLAYER) ca = true;
            InGameMenuHandler.instance.ActivateContextMenu(this, ca,war, Input.mousePosition);

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
                InGameMenuHandler.instance.SendWarriors(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>(), this, InGameMenuHandler.instance.warriorsNumber);
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
    
    public void ImproveSpeedPopulation()
    {
        territoryStats.velocityPopulation += 0.3f;
        territory.SetStats(territoryStats);
    }
    public void ImproveLimit()
    {
        territoryStats.limitPopulation += 20;
        territory.SetStats(territoryStats);
    }
    public void ImproveSpeedGold()
    {
        territoryStats.velocityGold += 0.6f;            //
        territory.GoldMineTerritory.Level += 1;         //
        territory.SetStats(territoryStats);
    }
    public void GatherTerritoryGold()
    {
        int gather = territoryStats.gold;
        territoryStats.gold -= gather;
    }
    public void GatherTerritoryFood()
    {
        int gather = territoryStats.food;
        territoryStats.food-= gather;
    }
}
