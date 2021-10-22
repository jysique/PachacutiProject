
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
[RequireComponent(typeof(PolygonCollider2D))]
public class TerritoryHandler : MonoBehaviour
{

    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Territory territory;
    [SerializeField] private float paddingX;
    [SerializeField] private float paddingY;
    [SerializeField] private List<GameObject> adjacentTerritories;

    public int state;
    public bool war;
    private SpriteRenderer sprite;
    private Color32 oldColor;
    private Color32 hoverColor;
    private TerritoryStats territoryStats;
    
    public List<GameObject> AdjacentTerritories
    {
        get { return adjacentTerritories; }
        set { adjacentTerritories = value; }
    }
    public Territory Territory
    {
        get { return territory; }
        set { territory = value; }
    }
    public SpriteRenderer SpriteRender
    {
        get { return sprite; }
        set { sprite = value; }
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

        InstantiateStatTerritory();
    }
    private void Start()
    {
        if (territory.Selected)
        {
            TerritoryManager.instance.territorySelected = this.gameObject;
            sprite.material = outlineMaterial;
            WarManager.instance.selected = this;
            WarManager.instance.SetWarStatus(this.war);
        }            
        if(territory.TypePlayer != Territory.TYPEPLAYER.NONE)
        {
            territory.IsClaimed = true;
        }
    }
    void InstantiateStatTerritory()
    {
        GameObject go = Resources.Load("Prefabs/MenuPrefabs/TerritoryStats") as GameObject;
        GameObject stats = Instantiate(go, this.transform);
        stats.transform.position = new Vector3(stats.transform.position.x + paddingX, stats.transform.position.y + paddingY,0f);
        territoryStats = stats.GetComponent<TerritoryStats>();
        territoryStats.InitStats(territory);

    }
    private void Update()
    {
        territoryStats.UpdatePopulation(territory);
    }
    private void OnMouseOver()
    {
        if(EventSystem.current.IsPointerOverGameObject())
            return;
        
        if (Input.GetMouseButtonDown(1) && state == 0)
        {
            territory.Selected = true;
            ShowStateMenu();
            MakeOutline();

            bool ca = false;
            TerritoryHandler selected = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
        
            if (selected == this && territory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
            {
                ca = true;
            }
            if (war == true || EventManager.instance.GetIsTerritoriesIsInPandemic(this) || EventManager.instance.GetIsTerritoriesIsInPandemic()) 
            {
                ca = false;
            }
            if (TutorialController.instance.CanMoveUnits)
            {
                MenuManager.instance.ActivateContextMenu(this, ca, war, Input.mousePosition);
            }
            

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
                territory.Selected =true;
                ShowStateMenu();
                MakeOutline();
                InGameMenuHandler.instance.UpdateMenu();
                //InGameMenuHandler.instance.UpdateBuildings(this.territoryStats.Territory);
                break;
            case 1:
                if (TutorialController.instance.CanSelectTroops)
                {
                    MenuManager.instance.ActivateSelectTroopsMenu(this);
                }
                else{
                    TutorialController.instance.MoveTroopInTutorial(this);
                    //TutorialController.instance.CanSelectTroops = true;
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
        TerritoryManager.instance.MakeOutline(this);

    }
    /// <summary>
    /// See which territories adjacent you can select
    /// </summary>
    public void ShowAdjacentTerritories()
    {
        sprite.material = normalMaterial;
        sprite.sortingOrder =-9;
        foreach(GameObject t in adjacentTerritories)
        {
            t.GetComponent<TerritoryHandler>().state = 1;
            outlineMaterial.SetColor("_SolidOutline", Color.yellow);
            t.GetComponent<TerritoryHandler>().sprite.material = outlineMaterial;
            t.GetComponent<TerritoryHandler>().sprite.sortingOrder = -8;
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
        territory.Selected = false;
        sprite.sortingOrder = -8;
        sprite.material = normalMaterial;
    }
    
    
    public void GetSizeMap()
    {
        territory.Width = sprite.sprite.texture.width;
        territory.Height = sprite.sprite.texture.height;
    }

    public void ShowStateMenu()
    {

        WarManager.instance.selected = this;
        WarManager.instance.SetWarStatus(this.war);
        TerritoryManager.instance.ChangeStateTerritory(0);
        MenuManager.instance.TurnOffBlock();

    }

}
