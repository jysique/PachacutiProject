
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
    public List<GameObject> AdjacentTerritories
    {
        get { return adjacentTerritories; }
        set { adjacentTerritories = value; }
    }

    GameObject statsGO;
    [SerializeField] private TerritoryStats territoryStats;
    
    public TerritoryStats TerritoryStats
    {
        get { return territoryStats; }
    }

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
        if (TerritoryStats.Territory.GetSelected())
        {
            TerritoryManager.instance.territorySelected = this.gameObject;
            sr.material = outlineMaterial;
            InGameMenuHandler.instance.UpdateMenu();
            WarManager.instance.selected = this;
            WarManager.instance.SetWarStatus(this.war);
            //ShowAdjacentTerritories();
        }            
        if(TerritoryStats.Territory.TypePlayer != Territory.TYPEPLAYER.NONE)
        {
            TerritoryStats.Territory.IrrigationChannelTerritory.ImproveBuilding(1);
            TerritoryStats.Territory.IrrigationChannelTerritory.ImproveCostUpgrade();
            TerritoryStats.Territory.GoldMineTerritory.ImproveBuilding(1);
            TerritoryStats.Territory.GoldMineTerritory.ImproveCostUpgrade();

            TerritoryStats.Territory.AcademyTerritory.ImproveBuilding(1);
            TerritoryStats.Territory.AcademyTerritory.ImproveCostUpgrade();
            TerritoryStats.Territory.BarracksTerritory.ImproveBuilding(1);
            TerritoryStats.Territory.BarracksTerritory.ImproveCostUpgrade();
            TerritoryStats.Territory.ArcheryTerritory.ImproveBuilding(1);
            TerritoryStats.Territory.ArcheryTerritory.ImproveCostUpgrade();
            TerritoryStats.Territory.IsClaimed = true;
        }

        adjacentTerritories = TerritoryManager.instance.dictionary.Single(s => s.Key == territory.name).Value;
        //TerritoryStats.Territory.RegionTerritory = TerritoryManager.instance.dictionary2.Single(s => s.Key == territory.name).Value;
    }
    void InstantiateStatTerritory()
    {
        GameObject canvas = GameObject.Find("Canvas");
        statsGO = Instantiate(Resources.Load("Prefabs/MenuPrefabs/TerritoryStats")) as GameObject;

        statsGO.transform.SetParent(GameObject.Find("StatsContainer").transform,false);
//        print(canvas.GetComponent<Canvas>().scaleFactor);
        statsGO.GetComponent<RectTransform>().anchoredPosition =  new Vector3(transform.position.x*110+paddingX, transform.position.y*110+paddingY,transform.position.z);

        territoryStats = statsGO.GetComponent<TerritoryStats>();
        TerritoryStats.Territory = territory;

    }
    private void Update()
    {
    }
    private void FixedUpdate()
    {
        if(TerritoryStats.Territory.TypePlayer != Territory.TYPEPLAYER.NONE && TerritoryStats.Territory.TypePlayer != Territory.TYPEPLAYER.PLAYER && war == false)
        {
            int prob = Random.Range(0, 401);
            if (prob < 1 && this.TerritoryStats.Territory.Population > 2)
            {
                EnemyMoveWarriors();
            }
        }
    }

    private void EnemyMoveWarriors()
    {
        /*
        int i = Random.Range(0, adjacentTerritories.Count);
        int warriorsToSend = Random.Range(3, this.TerritoryStats.Territory.Population);
        TerritoryHandler territoryToAttack = adjacentTerritories[i].GetComponent<TerritoryHandler>();
        WarManager.instance.SendWarriors(this, territoryToAttack, warriorsToSend);
        */
    }

    private void OnMouseOver()
    {
        if(EventSystem.current.IsPointerOverGameObject())
            return;
        
        if (Input.GetMouseButtonDown(1) && state == 0)
        {
            
            TerritoryStats.Territory.SetSelected(true);
            ShowStateMenu();
            MakeOutline();
            
            bool ca = false;
            TerritoryHandler selected = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
        
            if (selected == this && TerritoryStats.Territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
            {
                ca = true;
                if (TerritoryStats.Territory.IsClaimed == false)
                {
                    ca = false;
                }
            }
       //     print("1|" + TimeSystem.instance.GetIsTerritorieIsInPandemic(this));
       //     print("2|" + TimeSystem.instance.GetIsTerritorieIsInPandemic());
            if (war == true || EventManager.instance.GetIsTerritorieIsInPandemic(this) || EventManager.instance.GetIsTerritorieIsInPandemic()) 
            {
//                print("b|");
                ca = false;
            }

            
   //         print(ca);
            MenuManager.instance.ActivateContextMenu(this, ca,war, Input.mousePosition);

        }
        MenuManager.instance.CloseSelectCharacterMenu();
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
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ReadAndPlaySFX("territory_button");
        }
        
        switch (state)
        {
            case 0:

                TerritoryStats.Territory.SetSelected(true);
                ShowStateMenu();
                MakeOutline();
                break;
            case 1:
                TerritoryHandler territoryHandlerSelected = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
                Territory.TYPEPLAYER typeSelected = territoryHandlerSelected.TerritoryStats.Territory.TypePlayer;
                int _warriorsSword = MenuManager.instance.contextMenu.GetComponent<ContextMenu>().WarriorsSword();
                int _warriorsLance = MenuManager.instance.contextMenu.GetComponent<ContextMenu>().WarriorsLance();
                int _warriorsArch = MenuManager.instance.contextMenu.GetComponent<ContextMenu>().WarriorsArch();
                int totalWarriors = MenuManager.instance.contextMenu.GetComponent<ContextMenu>().TotalWarriors();
                if (InGameMenuHandler.instance.GoldPlayer >= totalWarriors || TerritoryStats.Territory.TypePlayer == typeSelected)
                {
                    if (TerritoryStats.Territory.TypePlayer != typeSelected)
                    {
                        InGameMenuHandler.instance.GoldPlayer -= totalWarriors;
                        InGameMenuHandler.instance.FoodPlayer -= totalWarriors;
                    }
                    WarManager.instance.SendWarriors(territoryHandlerSelected, this, _warriorsSword,_warriorsLance,_warriorsArch);
                }
                else
                {
                    InGameMenuHandler.instance.ShowFloatingText("you need "+totalWarriors+" golds", "TextMesh", transform, new Color32(187, 27, 128, 255));
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

    public void MakeOutline()
    {
        foreach (GameObject t in TerritoryManager.instance.territoryList)
        {
            t.GetComponent<TerritoryHandler>().Deselect();
            t.GetComponent<TerritoryHandler>().state = 0;
        }
        TerritoryStats.Territory.SetSelected(true);
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
        TerritoryStats.Territory.SetSelected(false);
        sr.sortingOrder = -8;
        sr.material = normalMaterial;
    }
    /*
    public void ImproveSpeedPopulation()
    {
        //TerritoryStats.Territory.VelocityPopulation += 0.3f;
        TerritoryStats.Territory.VelocityPopulation += TerritoryStats.Territory.ImproveSpeed;
    }
    public void ImproveLimit()
    {
        //TerritoryStats.Territory.LimitPopulation += 20;
        TerritoryStats.Territory.LimitPopulation += TerritoryStats.Territory.ImproveLimit;
    }
    */
    /// <summary>
    /// Returns the builds from any buildings in territory
    /// </summary>
    /// <param name="_option"></param>
    /// <returns></returns>
    public Building GetBuilding(Building building)
    {
        if (building is IrrigationChannel)
        {
            return TerritoryStats.Territory.IrrigationChannelTerritory;
        }
        else if (building is GoldMine)
        {
            return TerritoryStats.Territory.GoldMineTerritory;
        }
        else if (building is Fortress)
        {
            return TerritoryStats.Territory.FortressTerritory;
        }
        else if (building is Academy)
        {
            return TerritoryStats.Territory.AcademyTerritory;
        }
        else if (building is Barracks)
        {
            return TerritoryStats.Territory.BarracksTerritory;
        }
        return TerritoryStats.Territory.ArcheryTerritory;
    }
    /*
    public Building GetBuilding(int option)
    {
        if (option == 0)
        {
            return TerritoryStats.Territory.IrrigationChannelTerritory;
        }
        else if (option == 1)
        {
            return TerritoryStats.Territory.GoldMineTerritory;
        }
        else if (option == 2)
        {
            return TerritoryStats.Territory.SacredPlaceTerritory;
        }
        else if (option == 3)
        {
            return TerritoryStats.Territory.FortressTerritory;
        }
        return TerritoryStats.Territory.ArmoryTerritory;
    }
    */
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
        int gather = TerritoryStats.Territory.Gold;
        TerritoryStats.Territory.Gold -= gather;
    }
    public void GatherTerritoryFood()
    {
        int gather = TerritoryStats.Territory.FoodReward;
        TerritoryStats.Territory.FoodReward-= gather;
    }
}
