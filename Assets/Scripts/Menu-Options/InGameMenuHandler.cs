using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameMenuHandler : MonoBehaviour
{
    public int warriorsNumber;
    public static InGameMenuHandler instance;
    [SerializeField] private GameObject contextMenu;
    [SerializeField] private GameObject warriorsPrefab;
    [SerializeField] private Territory selectedTerritory;

    [Header("OverMenu")]
    [SerializeField] private GameObject menuConfirm;
    [SerializeField] public GameObject overMenuBlock;
    [Header("Menu personaje")]
    [SerializeField] private Text governorName;
    [SerializeField] private Text governorAge;
    [SerializeField] private Text governorOrigin;
    [SerializeField] private Image governorPicture;
    [SerializeField] private Text governorDiplomacy;
    [SerializeField] private Text governorMilitancy;
    [SerializeField] private Text governorManagement;
    [SerializeField] private Text governorPrestige;
    [SerializeField] private Text governorPiety;



    [Header("Menu militar")]

    [SerializeField] private GameObject menuBlock;

    [SerializeField] private Image militaryBossPicture;
    [SerializeField] private Text militarWarriorsCount;
    [SerializeField] private Text GenerationSpeed;
    [SerializeField] private Text militaryBossName;
    [SerializeField] private Text militaryBossExperience;
    [SerializeField] private Text militaryBossEstrategy;
    [SerializeField] private Text militaryBossMilitary;
    [Header("Menu de movilizacion de tropas")]
    [SerializeField] private InputField warriorsCount;
    [Header("Menu territorio")]
    [SerializeField] private Text territoryName;
    [SerializeField] private Text goldCount;
    [Header("Recursos")]
    [SerializeField] private Text goldGenerated;
 //   [SerializeField] private Text foodGenerated;
 //   [SerializeField] private Text sucesionSizeTxt;
    [SerializeField] private Text scoreTxt;
    [SerializeField] private Transform goldAnimation;
 //   [SerializeField] private Transform foodAnimation;
    [Header("Select MilitaryBoss variables")]
    [SerializeField] private GameObject CharacterSelection;

    private List<GameObject> characterOptions;
    public MilitarBossList ml;
    int goldPlayer;
  //  int foodPlayer;
    int sucesionSizePlayer;
    int scorePlayer;
    //perfil menu
    
    private void Start()
    {
    }
    private void Awake()
    {
        instance = this;
        warriorsNumber = 0;
    }
    public void UpdateResourceTable()
    {
        goldGenerated.text = goldPlayer.ToString();
     //   foodGenerated.text = foodPlayer.ToString();
     //   sucesionSizeTxt.text = "0";
        scoreTxt.text = TerritoryManager.instance.CountTerrytorry(Territory.TYPEPLAYER.PLAYER).ToString();
    }
    public void UpdateMilitarMenu()
    {

        if (selectedTerritory.TypePlayer == Territory.TYPEPLAYER.PLAYER)
        {
            menuBlock.SetActive(false);
            MilitarBoss boss = selectedTerritory.MilitarBoss;
            militaryBossName.text = "Nombre: " + boss.CharacterName;
            militaryBossPicture.sprite = boss.Picture;
            militaryBossExperience.text = "Experiencia: " + boss.Experience;
            militaryBossEstrategy.text = "Estrategia: " + boss.StrategyType;
            militaryBossMilitary.text = "Influencia: " + boss.Influence;
            GenerationSpeed.text = "Velocidad de crecimiento: " + selectedTerritory.VelocityPopulation;
            
        }
        else
        {
            menuBlock.SetActive(true);
        }
    }
    public void UpdateTerritoryMenu()
    {
        territoryName.text = selectedTerritory.name;
    }
    public void UpdateProfileMenu()
    {
        Governor temp = CharacterManager.instance.Governor;
        governorName.text = "Nombre: " + temp.CharacterName;
        governorAge.text = "Edad: " + temp.Age.ToString();
        governorPicture.sprite = temp.Picture;
        governorOrigin.text = "Origen: " + temp.Origin;
        governorDiplomacy.text = "Diplomacia: " + temp.Diplomacy;
        governorMilitancy.text = "Militar: " + temp.Militancy;
        governorManagement.text = "Administracion: " + temp.Managment;
        governorPrestige.text = "Prestigio: " + temp.Prestige;
        governorPiety.text = "Piedad: " + temp.Piety;
    }
    public void UpdateMenu()
    {
        selectedTerritory = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territory;
        UpdateMilitarMenu();
        UpdateTerritoryMenu();

    }
    void Update()
    {
        Territory selectedTerritory = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territory;
        militarWarriorsCount.text = "Tropas: " + selectedTerritory.Population.ToString() + " / " + selectedTerritory.LimitPopulation.ToString();
        goldCount.text = "Oro: " + selectedTerritory.Gold.ToString();
        UpdateResourceTable();
    }

    //move warriors
    public void MoveWarriorsButton()
    {
        TurnOnBlock();
        menuConfirm.SetActive(true);
        warriorsCount.text = "1";
        ChangeStateTerritory(2);

    }
    public void ChangeStateTerritory(int _state)
    {
        foreach (GameObject t in TerritoryManager.instance.territoryList)
        {
            t.GetComponent<TerritoryHandler>().state = _state;
        }
    }

    public void CloseWarriorsButton()
    {
        menuConfirm.SetActive(false);
        TurnOffBlock();
        ChangeStateTerritory(0);
    }
    public void SelectTerritory()
    {
        warriorsNumber = int.Parse(warriorsCount.text);
        if (TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territoryStats.population - warriorsNumber >= 0)
        {
            menuConfirm.SetActive(false);
            TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().ShowAdjacentTerritories();
        }
    }

    public void SendWarriors(TerritoryHandler selected, TerritoryHandler otherTerritory, int _warriorsNumber)
    {

        selected.territoryStats.population = selected.territoryStats.population - _warriorsNumber;
        warriorsPrefab.GetComponent<WarriorsMoving>().SetAttack(otherTerritory.gameObject, 1, _warriorsNumber, selected.territory.TypePlayer, selected.territory.MilitarBoss);
        Instantiate(warriorsPrefab, selected.transform.position , Quaternion.identity);

    }
    public void MoveWarriors(TerritoryHandler otherTerritory, int attackPower, Territory.TYPEPLAYER type)
    {
        Territory.TYPEPLAYER temp = otherTerritory.territory.TypePlayer;
        if (otherTerritory.territory.TypePlayer == type)
        {
            otherTerritory.territoryStats.population = otherTerritory.territoryStats.population + attackPower;

        }
        else
        {
            if (otherTerritory.war)
            {
                WarManager.instance.AddMoreWarriors(otherTerritory, attackPower);
                print("added");
            }
            else
            {
                WarManager.instance.AddWar(attackPower, otherTerritory.territoryStats.population, 2, 2, otherTerritory, type);
                print("war");
                otherTerritory.war = true;
                otherTerritory.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }
            
        }


    }

    public void ActivateContextMenu(TerritoryHandler territoryToAttack, bool canAttack, bool isWar, Vector3 mousePosition)
    {
        TurnOnBlock();
        contextMenu.SetActive(true);
        ChangeStateTerritory(2);
        Vector3 mousePosCamera = Camera.main.ScreenToWorldPoint(mousePosition);
        contextMenu.transform.position = new Vector3(mousePosCamera.x, mousePosCamera.y, contextMenu.transform.position.z);
        contextMenu.GetComponent<ContextMenu>().SetMenu(canAttack, isWar, territoryToAttack);
    }
    public void ImproveSpeedButton()
    {
        ImproveSpeed(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>());
        goldNeedSpeed += 3;
        UpdateMenu();
    }
    public void ImproveLimitButton()
    {

        ImproveLimit(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>());
        goldNeedLimit += 3;
        UpdateMenu();
    }
    int goldNeedSpeed = 10;
    int goldNeedLimit = 10;
    public void ImproveSpeed(TerritoryHandler territoryHandler)
    {
        if (goldPlayer >= goldNeedSpeed)
        {
            territoryHandler.ImproveSpeed();
            goldPlayer -= goldNeedSpeed;
            ShowFloatingText("+0.3 veloc", "TextMesh", territoryHandler.transform);
            ShowFloatingText("-"+goldNeedSpeed.ToString(), "TextFloating", goldAnimation);
        }
        
    }
    public void ImproveLimit(TerritoryHandler territoryHandler)
    {
        if (goldPlayer >= goldNeedLimit)
        {
            territoryHandler.ImproveLimit();
            goldPlayer -= goldNeedLimit;
            ShowFloatingText("+20 limit", "TextMesh", territoryHandler.transform);
            ShowFloatingText("-"+goldNeedLimit.ToString(), "TextFloating", goldAnimation);
        }
    }
    public int GatherGold(TerritoryHandler territoryHandler)
    {
        int gatherGold = territoryHandler.territoryStats.gold;
        territoryHandler.GatherTerritoryGold();
        return gatherGold;
    }
    /*
    public int GatherFood(TerritoryHandler territoryHandler)
    {
        int gatherFood = territoryHandler.territoryStats.food;
        territoryHandler.GatherTerritoryFood();
        return gatherFood;
    }
    */
    void ShowFloatingText(string text,string namePrefab,Transform _t)
    {
        GameObject prefab = Resources.Load("Prefabs/MenuPrefabs/"+namePrefab) as GameObject;
        GameObject go = new GameObject();
        if (namePrefab == "TextFloating")
        {
            go = Instantiate(prefab, FindObjectOfType<Canvas>().transform);
            go.transform.parent = _t.transform;
            go.GetComponent<Text>().text = text;
        }else if (namePrefab == "TextMesh")
        {
            go = Instantiate(prefab);
            go.transform.SetParent(GameObject.Find("StatsContainer").transform, false);
            go.transform.GetComponentInChildren<Text>().text = text;
        }
        go.transform.position = _t.position;
        Destroy(go, 1.2f);
    }
    public void GatherGoldResource()
    {
        int temp = 0;
        for (int i = 0; i < TerritoryManager.instance.GetTerritoryHandlers().Count; i++)
        {
            temp+= GatherGold(TerritoryManager.instance.GetTerritoryHandlers()[i]);
        }
        ShowFloatingText("+"+ temp.ToString(), "TextFloating",goldAnimation);
        goldPlayer += temp;
    }
    /*
    public void GatherFoodResource()
    {
        int temp = 0;
        for (int i = 0; i < TerritoryManager.instance.GetTerritoryHandlers().Count; i++)
        {
            temp += GatherFood(TerritoryManager.instance.GetTerritoryHandlers()[i]);
        }
        ShowFloatingText("+" + temp.ToString(), "TextFloating",foodAnimation);
        foodPlayer += temp;
    }
    */
    public void TurnOffBlock()
    {
        overMenuBlock.SetActive(false);
    }
    public void TurnOnBlock()
    {
        overMenuBlock.SetActive(true);
    }

    public void InstantiateCharacterOption(TerritoryHandler territory)
    {
        ml.AddDataMilitaryList(5);
        CharacterSelection.SetActive(true);
        characterOptions = new List<GameObject>();
        Transform gridLayout = CharacterSelection.transform.Find("ScrollArea/ScrollContainer/GridLayout").transform;
        foreach (MilitarBoss charac in ml.MilitarBosses)
        {
            GameObject characterOption = Instantiate(Resources.Load("Prefabs/MenuPrefabs/CharacterGameOption")) as GameObject;
            characterOption.transform.SetParent(gridLayout.transform, false);
            characterOption.name = charac.CharacterName;
            characterOption.transform.Find("Character/ImageCharacter").gameObject.GetComponent<Image>().sprite = charac.Picture;
            characterOption.transform.Find("Character/TextBackground/NameCharacter").gameObject.GetComponent<Text>().text = charac.CharacterName;
            characterOption.transform.Find("Description/OrigenCharacter").gameObject.GetComponent<Text>().text = charac.Origin;
            characterOption.transform.Find("Description/AgeCharacter").gameObject.GetComponent<Text>().text = charac.Age.ToString();
            characterOption.transform.Find("Description/CampainCharacter").gameObject.GetComponent<Text>().text = charac.Personality;
            characterOption.transform.Find("Description/StatsCharacter").gameObject.GetComponent<Text>().text = charac.Influence + " | " + charac.Opinion + " | " + charac.Experience + " | " + charac.StrategyType;
            characterOption.GetComponent<SelectCharacter>().SetTerritoryHandler(territory);
            characterOptions.Add(characterOption);
        }
    }
    public void CloseCharacterSelection()
    {
        CharacterSelection.SetActive(false);
        for (int i = 0; i < characterOptions.Count; i++)
        {
            Destroy(characterOptions[i]);
        }
    }
}
