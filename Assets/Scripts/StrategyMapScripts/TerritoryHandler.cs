
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
[RequireComponent(typeof(PolygonCollider2D))]
public class TerritoryHandler : MonoBehaviour
{
    private SpriteRenderer sr;
    public int state;
    public bool war;
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Territory territory;
    [SerializeField] private float paddingX;
    [SerializeField] private float paddingY;
    public SpriteRenderer sprite;
    private Color32 oldColor;
    private Color32 hoverColor;
    [SerializeField]private List<GameObject> adjacentTerritories;
    //public List<GameObject> adjacent;

    GameObject statsGO;
    [SerializeField] public TerritoryStats territoryStats;

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
        if (territoryStats.territory.GetSelected())
        {
            TerritoryManager.instance.territorySelected = this.gameObject;
            sr.material = outlineMaterial;
            InGameMenuHandler.instance.UpdateMenu();
            WarManager.instance.selected = this;
            WarManager.instance.SetWarStatus(this.war);
            //ShowAdjacentTerritories();
        }            
        if(territoryStats.territory.TypePlayer != Territory.TYPEPLAYER.NONE)
        {
            territoryStats.territory.IrrigationChannelTerritory.ImproveBuilding(1);
            territoryStats.territory.GoldMineTerritory.ImproveBuilding(1);
        }

        adjacentTerritories = TerritoryManager.instance.dictionary.Single(s => s.Key == territory.name).Value;
    }
    void InstantiateStatTerritory()
    {
        GameObject canvas = GameObject.Find("Canvas");
        statsGO = Instantiate(Resources.Load("Prefabs/MenuPrefabs/TerritoryStats")) as GameObject;

        statsGO.transform.SetParent(GameObject.Find("StatsContainer").transform,false);
//        print(canvas.GetComponent<Canvas>().scaleFactor);
        statsGO.GetComponent<RectTransform>().anchoredPosition =  new Vector3(transform.position.x*110+paddingX, transform.position.y*110+paddingY,transform.position.z);

        territoryStats = statsGO.GetComponent<TerritoryStats>();
        territoryStats.territory = territory;

    }
    /// <summary>
    /// If is none type player and this territory is limit 
    /// can populate the territory
    /// </summary>
    private void PopulateTerritory()
    {
        if (territoryStats.territory.TypePlayer != Territory.TYPEPLAYER.NONE && territoryStats.territory.Population < territoryStats.territory.LimitPopulation)
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
        PopulateTerritory();
    }
    private void FixedUpdate()
    {
        if(territoryStats.territory.TypePlayer != Territory.TYPEPLAYER.NONE && territoryStats.territory.TypePlayer != Territory.TYPEPLAYER.PLAYER && war == false)
        {
           
            int prob = Random.Range(0, 401);
            if (prob < 1 && this.territoryStats.territory.Population > 2)
            {
                EnemyMoveWarriors();
            }
        }
    }

    private void EnemyMoveWarriors()
    {
        
        int i = Random.Range(0, adjacentTerritories.Count);
        int warriorsToSend = Random.Range(3, this.territoryStats.territory.Population);
        TerritoryHandler territoryToAttack = adjacentTerritories[i].GetComponent<TerritoryHandler>();
        WarManager.instance.SendWarriors(this, territoryToAttack, warriorsToSend);
    }

    private void OnMouseOver()
    {
        if(EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(1) && state == 0)
        {
            territoryStats.territory.SetSelected(true);
            ShowStateMenu();
            MakeOutline();
            bool ca = false;
            TerritoryHandler selected = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
            /*
            for (int i = 0; i < selected.adjacentTerritories.Count; i++)
            {
                //print(selected.adjacentTerritories[i]);
                //print(this);
                if (selected.adjacentTerritories[i].GetComponent<TerritoryHandler>() == this) ca = true;
            }
            */
            //if (selected.territoryStats.territory.TypePlayer != Territory.TYPEPLAYER.PLAYER) ca = false;
            if (selected == this && territoryStats.territory.TypePlayer == Territory.TYPEPLAYER.PLAYER) ca = true;
            if (war == true) ca = false;
            MenuManager.instance.ActivateContextMenu(this, ca,war, Input.mousePosition);

        }
    }
    private void OnMouseExit()
    {
        sprite.color = oldColor;
    }
    /// <summary>
    /// State of the territory 
    /// </summary>
    private void OnMouseUpAsButton()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        switch (state)
        {
            case 0:
                territoryStats.territory.SetSelected(true);
                ShowStateMenu();
                MakeOutline();
                break;
            case 1:
                //print("moving");
                TerritoryHandler territoryHandlerSelected = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
                Territory.TYPEPLAYER typeSelected = territoryHandlerSelected.territoryStats.territory.TypePlayer;
                int warriorsCount = MenuManager.instance.contextMenu.GetComponent<ContextMenu>().WarriorsCount();

                if (InGameMenuHandler.instance.GoldPlayer >= warriorsCount || territoryStats.territory.TypePlayer == typeSelected)
                {
                    if (territoryStats.territory.TypePlayer != typeSelected)
                    {
                        InGameMenuHandler.instance.GoldPlayer -= warriorsCount;
                    }
                    WarManager.instance.SendWarriors(territoryHandlerSelected, this, warriorsCount);
                }
                else
                {
                    //print("no tiene suficiente oro");
                    InGameMenuHandler.instance.ShowFloatingText("you need "+warriorsCount+" golds", "TextMesh", transform, new Color32(187, 27, 128, 255));
                }
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
            t.GetComponent<TerritoryHandler>().state = 0;
        }
        territoryStats.territory.SetSelected(true);
        outlineMaterial.SetColor("_SolidOutline", Color.green);
        sr.material = outlineMaterial;
        sr.sortingOrder = -8;
        TerritoryManager.instance.territorySelected = this.gameObject;
        InGameMenuHandler.instance.UpdateMenu();
    }
    /// <summary>
    /// See which territories adjacent you can select
    /// </summary>
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
    /// <summary>
    /// Hide the territories adjacent you can select
    /// </summary>
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
        territoryStats.territory.SetSelected(false);
        sr.sortingOrder = -8;
        sr.material = normalMaterial;
    }
    
    public void ImproveSpeedPopulation()
    {
        territoryStats.territory.VelocityPopulation += 0.3f;
    }
    public void ImproveLimit()
    {
        territoryStats.territory.LimitPopulation += 20;
    }
    /// <summary>
    /// Returns time to builds from any buildings in territory
    /// </summary>
    /// <param name="_option"></param>
    /// <returns></returns>
    public float CalculateDuration(int _option)
    {
        float duration = 0;
        switch (_option)
        {
            case 0:
                duration = territoryStats.territory.IrrigationChannelTerritory.TimeToBuild;
                break;
            case 1:
                duration = territoryStats.territory.GoldMineTerritory.TimeToBuild;
                break;
            case 2:
                duration = territoryStats.territory.SacredPlaceTerritory.TimeToBuild;
                break;
            case 3:
                duration = territoryStats.territory.FortressTerritory.TimeToBuild;
                break;
            case 4:
                duration = territoryStats.territory.BarracksTerritory.TimeToBuild;
                break;
            default:
                break;
        }
        return duration;
    }
    /// <summary>
    /// Action : improve buildings to 1 level
    /// </summary>
    /// <param name="_option"></param>
    public void ImproveBuildings(int _option)
    {
        switch (_option)
        {
            case 0:
                territoryStats.territory.IrrigationChannelTerritory.ImproveBuilding(1);
                break;
            case 1:
                territoryStats.territory.GoldMineTerritory.ImproveBuilding(1);
                break;
            case 2:
                territoryStats.territory.SacredPlaceTerritory.ImproveBuilding(1);
                break;
            case 3:
                territoryStats.territory.FortressTerritory.ImproveBuilding(1);
                break;
            case 4:
                territoryStats.territory.BarracksTerritory.ImproveBuilding(1);
                break;
            default:
                break;
        }
    }

    public void ShowStateMenu()
    {

        WarManager.instance.selected = this;
        WarManager.instance.SetWarStatus(this.war);
        TerritoryManager.instance.ChangeStateTerritory(0);
        MenuManager.instance.TurnOffBlock();

    }
    /// <summary>
    /// Action gather gold and food in this territory
    /// </summary>
    public void GatherTerritoryGold()
    {
        int gather = territoryStats.territory.Gold;
        territoryStats.territory.Gold -= gather;
    }
    public void GatherTerritoryFood()
    {
        int gather = territoryStats.territory.FoodReward;
        territoryStats.territory.FoodReward-= gather;
    }
}
