using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{

    public static CombatManager instance;
    [SerializeField] private Text turnsCounter;
    public List<UnitGroup> units = new List<UnitGroup>();
    [SerializeField]private GameObject squares;
    private GameObject canvas;
    Dictionary<string, UnitType> unitTypes = new Dictionary<string , UnitType>();
    Dictionary<SquareType.TYPESQUARE, Sprite> territoryPictures = new Dictionary<SquareType.TYPESQUARE, Sprite>();
    Dictionary<int, int> playerPositions = new Dictionary<int, int>();
    Dictionary<int, int> enemyPositions = new Dictionary<int, int>();

    [SerializeField] private GameObject unitGroupPrefab;
    [SerializeField] private GameObject menu;

    TerritoryHandler playerTerritory;
    TerritoryHandler enemyTerritory;

    [SerializeField] private int turns;
    public int c;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        turns = 20;
        c = -1;//contador de turnos
        canvas = GameObject.Find("Battle");


        //posiciones de unidades
        playerPositions.Add(0,0);
        playerPositions.Add(1,1);
        playerPositions.Add(2,4);
        playerPositions.Add(3,5);
        playerPositions.Add(4,8);
        playerPositions.Add(5,9);

        enemyPositions.Add(0, 3);
        enemyPositions.Add(1, 2);
        enemyPositions.Add(2, 7);
        enemyPositions.Add(3, 6);
        enemyPositions.Add(4, 11);
        enemyPositions.Add(5, 10);


        //unidades
        UnitType swordman = new UnitType("Swordsman", 10, 100, 1, new string[] { "Axeman"}, new string[] { "Lancer" }, Resources.Load<Sprite>("Textures/TemporalAssets/warriors/w1"));
        unitTypes.Add("Swordsman", swordman);
        UnitType lancer = new UnitType("Lancer", 15, 85, 1, new string[] { "Swordsman" }, new string[] { "Axeman" }, Resources.Load<Sprite>("Textures/TemporalAssets/warriors/spear"));
        unitTypes.Add("Lancer", lancer);
        UnitType axeman = new UnitType("Axeman", 20, 75, 1, new string[] { "Lancer" }, new string[] { "Swordsman" }, Resources.Load<Sprite>("Textures/TemporalAssets/warriors/axe"));
        unitTypes.Add("Axeman", axeman);
        UnitType archer = new UnitType("Archer", 10, 70, 2, new string[] { "Scout" }, new string[] { "Archer" }, Resources.Load<Sprite>("Textures/TemporalAssets/warriors/archer"));
        unitTypes.Add("Archer", archer);
        UnitType scout = new UnitType("Scout", 25, 70, 1, new string[] { "Archer" }, new string[] { "Scout" }, Resources.Load<Sprite>("Textures/TemporalAssets/warriors/horseman"));
        unitTypes.Add("Scout", scout);


        //Diccionario de territorios
        territoryPictures.Add(SquareType.TYPESQUARE.FOREST, Resources.Load<Sprite>("Textures/TemporalAssets/terrain/forest"));
        territoryPictures.Add(SquareType.TYPESQUARE.SAND, Resources.Load<Sprite>("Textures/TemporalAssets/terrain/sand"));
        territoryPictures.Add(SquareType.TYPESQUARE.GRASSLAND, Resources.Load<Sprite>("Textures/TemporalAssets/terrain/grassland"));
        territoryPictures.Add(SquareType.TYPESQUARE.MOUNTAIN, Resources.Load<Sprite>("Textures/TemporalAssets/terrain/mountain"));

        //pruebas
        TerritoryHandler ph = new TerritoryHandler();
        TerritoryStats prueba = new TerritoryStats();
        Territory pr = new Territory();
        pr.TypePlayer = Territory.TYPEPLAYER.PLAYER;
        prueba.Territory = pr;
        ph.TerritoryStats = prueba;

        

        TerritoryHandler eh = new TerritoryHandler();
        TerritoryStats prueba2 = new TerritoryStats();
        Territory pr2 = new Territory();
        pr2.TypePlayer = Territory.TYPEPLAYER.BOT;
        prueba2.Territory = pr2;
        eh.TerritoryStats = prueba2;

        Troop pTroop = new Troop();
        Troop eTroop = new Troop();
        pTroop.AddElement("Swordsman",1,150);
        pTroop.AddElement("Archer", 2, 150);
        pTroop.AddElement("Lancer", 3, 150);
        eTroop.AddElement("Axeman", 1, 150);
        eTroop.AddElement("Axeman", 2, 150);
        eTroop.AddElement("Scout", 3, 50);
        StartWar(pTroop,eTroop,ph,eh,true);
        

    }

    void SortList(List<UnitGroup> alpha)
    {
        for (int i = 0; i < alpha.Count; i++)
        {
            UnitGroup temp = alpha[i];
            int randomIndex = Random.Range(i, alpha.Count);
            alpha[i] = alpha[randomIndex];
            alpha[randomIndex] = temp;
        }
    }

    private void InstantiateUnit(int number, Sprite _sprite, GameObject square, Territory.TYPEPLAYER _type, string weapontype)
    {
        
        var pref = Instantiate(unitGroupPrefab);
        pref.transform.SetParent(square.transform, false);
        RectTransform rt = pref.GetComponent<RectTransform>();
        rt.offsetMin = new Vector2(-60,rt.offsetMin.y);
        rt.offsetMax = new Vector2(118, rt.offsetMax.y);
        rt.offsetMax = new Vector2(rt.offsetMax.x, 46);
        rt.offsetMin = new Vector2(rt.offsetMin.x, -116);
        //pref.GetComponent<RectTransform>().anchoredPosition = new Vector3(positions[pos].x * 100  , positions[pos].y * 100  , positions[pos].z); ;
        pref.transform.GetChild(0).GetComponent<Text>().text = number.ToString();
        pref.GetComponent<Image>().sprite = _sprite;
        if(_type != Territory.TYPEPLAYER.PLAYER)
        {
            Vector3 scale = pref.transform.localScale;
            scale.x *= -1;
            pref.transform.localScale = scale;
            Vector3 scale2 = pref.transform.GetChild(0).localScale;
            scale2.x *= -1;
            pref.transform.GetChild(0).localScale = scale2;
        }
        UnitGroup unit = new UnitGroup(_type, weapontype, number, pref);
        square.GetComponent<SquareType>().unitGroup = unit;
        square.GetComponent<SquareType>().haveUnit = true;

        units.Add(unit);
        pref.GetComponent<GroupClassContainer>().stats = unit;
    }
    public void StartWar(Troop playerTroop, Troop enemyTroop, TerritoryHandler _playerTerritory, TerritoryHandler _enemyTerritory, bool isPlayerTerritory)
    {
        playerTerritory = _playerTerritory;
        enemyTerritory = _playerTerritory;
        TerritoryHandler territoryOfWar;
        List<SquareType.TYPESQUARE> tiles;
        //establecer las casillas de jugador
        if (isPlayerTerritory)
        {
            territoryOfWar = playerTerritory;
            tiles = territoryOfWar.TerritoryStats.Territory.Tiles;
        }
        else
        {
            territoryOfWar = enemyTerritory;
            tiles = territoryOfWar.TerritoryStats.Territory.Tiles;
            List<SquareType.TYPESQUARE> newTiles = new List<SquareType.TYPESQUARE>();
            for (int y = (tiles.Count/2)-1; y > 0; y--)
            {
                newTiles.Add(tiles[y]);
            }
            for (int y = tiles.Count - 1; y > (tiles.Count / 2); y--)
            {
                newTiles.Add(tiles[y]);
            }
            tiles = newTiles;
        }

        

        for(int j = 0; j < squares.transform.childCount; j ++)
        {
            squares.transform.GetChild(j).GetComponent<SquareType>().typeSquare = SquareType.TYPESQUARE.GRASSLAND;
            squares.transform.GetChild(j).GetComponent<SquareType>().index = j;
            squares.transform.GetChild(j).GetComponent<Image>().sprite = territoryPictures[squares.transform.GetChild(j).GetComponent<SquareType>().typeSquare];
            //squares.transform.GetChild(j).GetComponent<SquareType>().typeSquare = tiles[j];              
        }
        


       for (int i = 0; i < playerTroop.Positions.Count; i++)
       {
           int up = playerPositions[playerTroop.Positions[i]];
           InstantiateUnit(playerTroop.Numbers[i], unitTypes[playerTroop.Types[i]].Picture, squares.transform.GetChild(up).gameObject, _playerTerritory.TerritoryStats.Territory.TypePlayer, playerTroop.Types[i]);
       }

       for (int i = 0; i < enemyTroop.Positions.Count; i++)
       {
           int up = enemyPositions[enemyTroop.Positions[i]];
           InstantiateUnit(enemyTroop.Numbers[i], unitTypes[enemyTroop.Types[i]].Picture , squares.transform.GetChild(up).gameObject, _enemyTerritory.TerritoryStats.Territory.TypePlayer, enemyTroop.Types[i]);
       }


       SortList(units);
       Invoke("MakeMovement",0.01f);
       //MakeMovement();
        
    }

    public void MakeMovement()
    {
        


        //reiniciar unidad
        c++;
        if (c >= units.Count) c = 0;
        units[c].Defense = false;
        units[c].UnitsGO.GetComponent<Image>().color = Color.white;

        //menu

        Vector3 newpos = units[c].UnitsGO.transform.parent.position;
        print(units[c].UnitsGO.transform.parent.position);
            
        menu.transform.position = new Vector3(newpos.x+1,newpos.y-1,newpos.z);
        print(menu.transform.position);

        print(units[1].UnitsGO.transform.parent.position);
        
        //turnos
        turns--;
        turnsCounter.text = "Turnos restantes: "+ turns.ToString();
        if (turns <= 0)
        {

            FinishCombat();
        }
        
    }
    private void SetStats(UnitGroup u)
    {
        //restar las unidades perdidas al final
    }
    public void FinishCombat()
    {
        c = -1;
        turns = 20;
        foreach (UnitGroup u in units)
        {
            //SetStats(u);
        }
        DateTableHandler.instance.PlayButton();
        //WarManager.instance.FinishWar(territory, attackerTerritory.TerritoryStats.Territory.TypePlayer, 0);
        for(int i = 0; i < squares.transform.childCount; i++)
        {
            Destroy(squares.transform.GetChild(0).GetChild(0));
        }
        units.Clear();
        //canvas.SetActive(false);
    }
    public void Attack()
    {
        SetPosibleAttack(units[c]);
    }
    public void Defend()
    {
        units[c].UnitsGO.GetComponent<Image>().color = Color.blue;
        units[c].Defense = true;
        MakeMovement();
    }

    public void Move()
    {
        CheckPosibleMoves(units[c]);
    }

    private int CheckAdvantage(UnitGroup attackGroup, UnitGroup defenseGroup)
    {
        string[] strongTo = unitTypes[attackGroup.Type].StrongTo;
        for(int i = 0; i < strongTo.Length; i++)
        {
            if (defenseGroup.Type == strongTo[i]) return 2;
        }
        string[] weakTo = unitTypes[attackGroup.Type].WeakTo;
        for (int i = 0; i < weakTo.Length; i++)
        {
            if (defenseGroup.Type == weakTo[i]) return 1;
        }
        return 0;
    }

    public void MakeDamage(UnitGroup defenseGroup)
    {
        UnitGroup attackGroup = units[c];

        int damage = 10;

        int advantage = CheckAdvantage(attackGroup, defenseGroup);
        if (advantage==2)
        {
            damage *= 2;
        }
        else if (advantage == 1)
        {
            damage /= 2;
        }
        int critic = Random.Range(1,100);
        if (critic < 10) damage *= 2;
        if (defenseGroup.Defense) damage /= 2;
        print(defenseGroup.Quantity);
        print(damage);

        defenseGroup.Quantity = defenseGroup.Quantity - damage;

        defenseGroup.UnitsGO.transform.GetChild(0).GetComponent<Text>().text = defenseGroup.Quantity.ToString();

        TurnOffButtons();
        if (defenseGroup.Quantity <= 0)
        {

            defenseGroup.Quantity = 0;
            defenseGroup.UnitsGO.transform.GetChild(0).GetComponent<Text>().text = "0";
            defenseGroup.UnitsGO.transform.GetChild(0).GetComponent<Text>().color = Color.red;
            //SetStats(defenseGroup);
            units.Remove(defenseGroup);
        }
        if (EndBattle())
        {
            FinishCombat();
        }


    }

    private void TurnOffButtons()
    {
        foreach (UnitGroup u in units)
        {
            //if(u.TypePlayer != Territory.TYPEPLAYER.PLAYER)
            
            u.UnitsGO.transform.GetChild(1).gameObject.SetActive(false);
            
        }
    }

    private bool EndBattle()
    {
        Territory.TYPEPLAYER check = units[0].TypePlayer;
        foreach (UnitGroup u in units)
        {
            if(u.TypePlayer != check)
            {
                return false;
            }
        }
        return true;
    }
    
    private void SetPosibleAttack(UnitGroup attacker)
    {
        int axisy = 4;
        int axisx = 1;
        int d1 = 5;
        int d2 = 3;
        int index = attacker.UnitsGO.transform.parent.GetComponent<SquareType>().index;
        int range = unitTypes[attacker.Type].Range;
        int newIndex = 0;

        for (int i = 1; i <= range; i++)
        {
            //left
            newIndex = index - (axisx * i);
            if (index % 4 != 0)AttackUnitByIndex(newIndex,attacker);
            //right
            newIndex = index + (axisx * i);
            if (newIndex % 4 != 0) AttackUnitByIndex(newIndex, attacker);
            //top
            newIndex = index - (axisy * i);
            AttackUnitByIndex(newIndex, attacker);
            //down
            newIndex = index + (axisy * i);
            AttackUnitByIndex(newIndex, attacker);

            //topleft
            newIndex = index - (d1 * i);
            if (index % 4 != 0) AttackUnitByIndex(newIndex, attacker);
            //downright
            newIndex = index + (d1 * i);
            if (newIndex % 4 != 0) AttackUnitByIndex(newIndex, attacker);
            //topright
            newIndex = index - (d2 * i);
            if (newIndex % 4 != 0) AttackUnitByIndex(newIndex, attacker);
            //downleft
            newIndex = index + (d2 * i);
            if (index % 4 != 0) AttackUnitByIndex(newIndex, attacker);
        }
        

    }

    private void AttackUnitByIndex(int index, UnitGroup attacker)
    {
        if (index > 0 && index < squares.transform.childCount && UnitByIndex(index) != null)
        {
            if (UnitByIndex(index).TypePlayer != attacker.TypePlayer)
            {
                UnitByIndex(index).UnitsGO.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
    }

    private UnitGroup UnitByIndex(int index)
    {
        return squares.transform.GetChild(index).GetComponent<SquareType>().unitGroup;
    }

    private SquareType SquareByIndex(int index)
    {
        return squares.transform.GetChild(index).GetComponent<SquareType>();
    }

    private void CheckPosibleMoves(UnitGroup unit)
    {

        int index = unit.UnitsGO.transform.parent.GetComponent<SquareType>().index;
        int newIndex;
        print(index);
        //left
        newIndex = index - 1;
        if (newIndex >= 0 && newIndex < squares.transform.childCount) {
            if (index % 4 != 0)
            {
                if (SquareByIndex(newIndex).haveUnit == false || SquareByIndex(newIndex).unitGroup.TypePlayer == SquareByIndex(index).unitGroup.TypePlayer)
                {

                    squares.transform.GetChild(newIndex).GetComponent<Image>().color = Color.magenta;
                    squares.transform.GetChild(newIndex).GetComponent<SquareType>().ActivateChange();
                }
            }
        }
               
        //right
        newIndex = index + 1;
        if (newIndex >= 0 && newIndex < squares.transform.childCount)
        {
            if (newIndex % 4 != 0)
            {
                if (SquareByIndex(newIndex).haveUnit == false || SquareByIndex(newIndex).unitGroup.TypePlayer == SquareByIndex(index).unitGroup.TypePlayer)
                {
                    squares.transform.GetChild(newIndex).GetComponent<Image>().color = Color.magenta;
                    squares.transform.GetChild(newIndex).GetComponent<SquareType>().ActivateChange();
                }
            }
        }
        
        //top
        newIndex = index - 4;
        if (newIndex >= 0 && newIndex < squares.transform.childCount)
        {
            if (SquareByIndex(newIndex).haveUnit == false || SquareByIndex(newIndex).unitGroup.TypePlayer == SquareByIndex(index).unitGroup.TypePlayer)
            {
                squares.transform.GetChild(newIndex).GetComponent<Image>().color = Color.magenta;
                squares.transform.GetChild(newIndex).GetComponent<SquareType>().ActivateChange();
            }
        }
        
        //down
        newIndex = index + 4;
        if (newIndex >= 0 && newIndex < squares.transform.childCount)
        {
            if (SquareByIndex(newIndex).haveUnit == false || SquareByIndex(newIndex).unitGroup.TypePlayer == SquareByIndex(index).unitGroup.TypePlayer)
            {
                squares.transform.GetChild(newIndex).GetComponent<Image>().color = Color.magenta;
                squares.transform.GetChild(newIndex).GetComponent<SquareType>().ActivateChange();
            }
        }
        
    }

    public int ActualUnitIndex()
    {
        return units[c].UnitsGO.transform.parent.GetComponent<SquareType>().index;
    }
    public void ChangeUnits(int index, int newIndex)
    {
        print(index);
        print(newIndex);
        SquareType square1 = SquareByIndex(index);
        SquareType square2 = SquareByIndex(newIndex);
        UnitGroup saveug = square1.unitGroup;

        print(saveug);
        if (square1.haveUnit)
        {
            square1.gameObject.transform.GetChild(0).transform.position = square2.gameObject.transform.position;
            square1.gameObject.transform.GetChild(0).transform.SetParent(square2.transform);
            
            
        }
        square1.unitGroup = square2.unitGroup;
        if (square2.haveUnit) 
        {
            square2.gameObject.transform.GetChild(0).transform.position = square1.gameObject.transform.position;
            square2.gameObject.transform.GetChild(0).transform.SetParent(square1.transform);
            
            
        }
        square2.unitGroup = saveug;
        MakeMovement();
        ClearSquares();
    }

    public void ClearSquares()
    {
        for(int i = 0; i < squares.transform.childCount; i++)
        {
            squares.transform.GetChild(i).GetComponent<Image>().color = Color.white;
            squares.transform.GetChild(i).GetComponent<SquareType>().DeactivateChange();
        }
    }
    
}
