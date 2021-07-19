
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
    [SerializeField] private List<GameObject> adjacentTerritories;
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
        set { territoryStats = value; }
    }
    public SpriteRenderer SpriteRender
    {
        get { return sr; }
    }
    public Material OutlineMaterial
    {
        get { return outlineMaterial; }
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
//            InGameMenuHandler.instance.UpdateMenu();
            WarManager.instance.selected = this;
            WarManager.instance.SetWarStatus(this.war);
            //ShowAdjacentTerritories();
        }            
        if(TerritoryStats.Territory.TypePlayer != Territory.TYPEPLAYER.NONE)
        {
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
//            print("a");
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
            if (war == true || EventManager.instance.GetIsTerritoriesIsInPandemic(this) || EventManager.instance.GetIsTerritoriesIsInPandemic()) 
            {
                ca = false;
            }
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
                InGameMenuHandler.instance.UpdateMenu();
                //InGameMenuHandler.instance.UpdateBuildings(this.territoryStats.Territory);
                break;
            case 1:
               // print("c");
                MenuManager.instance.ActivateSelectTroopsMenu(this);
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
        TerritoryManager.instance.MakeOutline(this);

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
    
    
    public void GetSizeMap()
    {
        territoryStats.Territory.width = sprite.sprite.texture.width;
        territoryStats.Territory.height = sprite.sprite.texture.height;
       // print("|w|"+ territoryStats.Territory.width + "|h|" + territoryStats.Territory.height);
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
        int gather = TerritoryStats.Territory.Gold;
        TerritoryStats.Territory.Gold -= gather;
    }
    public void GatherTerritoryFood()
    {
        int gather = TerritoryStats.Territory.FoodReward;
        TerritoryStats.Territory.FoodReward-= gather;
    }

}
