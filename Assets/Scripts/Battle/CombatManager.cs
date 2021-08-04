using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour
{

    public static CombatManager instance;
    [SerializeField] private Text turnsCounter;
    [SerializeField]private GameObject squares;
    [SerializeField] private GameObject unitGroupPrefab;
    [SerializeField] private GameObject menu;

    private GameObject canvas;

    public List<UnitGroup> units = new List<UnitGroup>();
    Dictionary<int, int> playerPositions = new Dictionary<int, int>();
    Dictionary<int, int> enemyPositions = new Dictionary<int, int>();

    TerritoryHandler playerTerritory;
    TerritoryHandler enemyTerritory;

    [SerializeField] private int turns;
    public int c;
    private void Awake()
    {
        instance = this;
        canvas = GameObject.Find("Battle");
    }
    void Start()
    {
        turns = 20;
        c = -1;
        canvas = GameObject.Find("Battle");
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
    private void InstantiateUnit(GameObject square, Territory.TYPEPLAYER _type, UnitCombat unitCombat)
    {

        var pref = Instantiate(unitGroupPrefab);
        pref.transform.SetParent(square.transform, false);
        RectTransform rt = pref.GetComponent<RectTransform>();
        rt.offsetMin = new Vector2(-60, rt.offsetMin.y);
        rt.offsetMax = new Vector2(118, rt.offsetMax.y);
        rt.offsetMax = new Vector2(rt.offsetMax.x, 46);
        rt.offsetMin = new Vector2(rt.offsetMin.x, -116);
        pref.transform.GetChild(0).GetComponent<Text>().text = unitCombat.Quantity.ToString();
 //      print("N|" + unitCombat.UnitName);
//        print("P|"+ unitCombat.Picture.name);
        pref.GetComponent<Image>().sprite = unitCombat.Picture;
        if (_type != Territory.TYPEPLAYER.PLAYER)
        {
            Vector3 scale = pref.transform.localScale;
            scale.x *= -1;
            pref.transform.localScale = scale;
            Vector3 scale2 = pref.transform.GetChild(0).localScale;
            scale2.x *= -1;
            pref.transform.GetChild(0).localScale = scale2;
        }
        UnitGroup unit = new UnitGroup(_type, pref, unitCombat);
        square.GetComponent<SquareType>().unitGroup = unit;
        square.GetComponent<SquareType>().haveUnit = true;

        units.Add(unit);
        pref.GetComponent<GroupClassContainer>().stats = unit;
    }


    bool isPlayer;
    public void StartWar(Troop playerTroop, Troop enemyTroop, TerritoryHandler _playerTerritory, TerritoryHandler _enemyTerritory, bool isPlayerTerritory)
    {
        isPlayer = isPlayerTerritory;
        playerTerritory = _playerTerritory;
        enemyTerritory = _enemyTerritory;
        TerritoryHandler territoryOfWar;
        //List<SquareType.TYPESQUARE> tiles;
        List<Terrain> tiles;
        //establecer las casillas de jugador
        if (isPlayerTerritory)
        {
            territoryOfWar = playerTerritory;
            tiles = territoryOfWar.TerritoryStats.Territory.TerrainList;
        }
        else
        {
            territoryOfWar = enemyTerritory;
            tiles = territoryOfWar.TerritoryStats.Territory.TerrainList;
            List<Terrain> newTiles = new List<Terrain>();
            for (int y = (tiles.Count/2)-1; y >= 0; y--)
            {
                newTiles.Add(tiles[y]);
            }
            for (int y = tiles.Count - 1; y >= (tiles.Count / 2); y--)
            {
                newTiles.Add(tiles[y]);
            }
            /*
            for (int i = 1; i <= tiles.Count / 4; i++)
            {
                int num = i*4-1;
                for (int j = num; j>=num-3 ; j--)
                {
                    newTiles.Add(tiles[j]);
                }
            }*/
            tiles = newTiles;
        }

        

        for(int j = 0; j < squares.transform.childCount; j ++)
        {
            squares.transform.GetChild(j).GetComponent<SquareType>().terrain = tiles[j];
            squares.transform.GetChild(j).GetComponent<SquareType>().index = j;
            squares.transform.GetChild(j).GetComponent<Image>().sprite = tiles[j].Picture;
        }

        //print("count|" +playerTroop.Positions.Count);


       for (int i = 0; i < playerTroop.Positions.Count; i++)
       {
            int up = playerPositions[playerTroop.Positions[i]];
            InstantiateUnit(squares.transform.GetChild(up).gameObject, _playerTerritory.TerritoryStats.Territory.TypePlayer, playerTroop.UnitCombats[i]);
       }

       for (int i = 0; i < enemyTroop.Positions.Count; i++)
       {
            int up = enemyPositions[enemyTroop.Positions[i]];
            InstantiateUnit(squares.transform.GetChild(up).gameObject, _enemyTerritory.TerritoryStats.Territory.TypePlayer, enemyTroop.UnitCombats[i]);
        }


       SortList(units);
       Invoke("MakeMovement",0.01f);
       //MakeMovement();
    }
    private void DestroyGameObjectAndChildren(GameObject gameObject)
    {
        gameObject.GetComponentInParent<SquareType>().unitGroup = null;
        foreach (Transform item in gameObject.transform)
        {
            DestroyImmediate(item.gameObject);
        }
        DestroyImmediate(gameObject);
    }
    public void MakeMovement()
    {
        //reiniciar unidad
        c++;
        if (c >= units.Count) c = 0;
        units[c].Defense = false;
        units[c].UnitsGO.GetComponent<Image>().color = Color.white;

        //menu
        if (units[c].TypePlayer == Territory.TYPEPLAYER.PLAYER)
        {
            Vector3 newpos = units[c].UnitsGO.transform.parent.position;
            //print(units[c].UnitsGO.transform.parent.position);

            menu.transform.position = new Vector3(newpos.x + 1, newpos.y - 1, newpos.z);
            //print(menu.transform.position);

            //print(units[1].UnitsGO.transform.parent.position);
        }

        //turnos
        turns--;
        turnsCounter.text = "Turnos restantes: " + turns.ToString();

        if (turns <= 0)
        {

            FinishCombat();
        }
        if (units[c].TypePlayer != Territory.TYPEPLAYER.PLAYER)
        {
            BotMovement();
        }
        

    }
    private void SetStats(UnitGroup u)
    {
        //  print("type player|" + u.TypePlayer);
        //  print("type|" + u.Type);
        //  print("q|" + u.Quantity);
        //string _type = u.UnitCombat.GetType().ToString();
        string _type = u.UnitCombat.UnitName;
        if (u.TypePlayer == Territory.TYPEPLAYER.PLAYER)
        {
            if (isPlayer)
            {
                //print(playerTerritory.TerritoryStats.Territory.name + " es atacado entonces "+ u.UnitCombat.GetType().ToString() +" es igual a " + u.UnitCombat.Quantity);
                //playerTerritory.TerritoryStats.Territory.GetUnit(u.Type).Quantity = u.Quantity;
                playerTerritory.TerritoryStats.Territory.GetUnit(_type).Quantity = u.UnitCombat.Quantity;
            }
            else
            {
                //print(playerTerritory.TerritoryStats.Territory.name + " ataca entonces " + u.UnitCombat.GetType().ToString() + " recupera " + u.UnitCombat.Quantity);
                playerTerritory.TerritoryStats.Territory.GetUnit(_type).Quantity += u.UnitCombat.Quantity;
            }
        }
        else
        {
            if (isPlayer)
            {
                //print(enemyTerritory.TerritoryStats.Territory.name + " ataca entonces " +  u.UnitCombat.GetType().ToString() + " recupera " +u.UnitCombat.Quantity);
                enemyTerritory.TerritoryStats.Territory.GetUnit(_type).Quantity += u.UnitCombat.Quantity;
            }
            else
            {
                //print(enemyTerritory.TerritoryStats.Territory.name + " es atacado entonces " +  u.UnitCombat.GetType().ToString() + " es igual a " + u.UnitCombat.Quantity);
                enemyTerritory.TerritoryStats.Territory.GetUnit(_type).Quantity = u.UnitCombat.Quantity;
            }
        }
    }
    public void FinishCombat()
    {
        c = -1;
        turns = 20;
        foreach (UnitGroup u in units)
        {
            SetStats(u);
        }
        DateTableHandler.instance.PlayButton();

        // is player si el territorio atacado es del jugador
        if (isPlayer)
        {
            WarManager.instance.FinishWar(playerTerritory, enemyTerritory.TerritoryStats.Territory.TypePlayer);
        }
        else
        {
            WarManager.instance.FinishWar(enemyTerritory, playerTerritory.TerritoryStats.Territory.TypePlayer);
        }
        
        for(int i = 0; i < squares.transform.childCount; i++)
        {
            if (SquareByIndex(i).haveUnit)
            {
                //        Destroy(squares.transform.GetChild(i).GetChild(0));
                SquareByIndex(i).unitGroup = null;
                DestroyGameObjectAndChildren(squares.transform.GetChild(i).GetChild(0).gameObject);
            }
        }
        units.Clear();
        canvas.SetActive(false);
    }
    public void Attack()
    {
        ShowAttackButtons(GetPosibleAttack(units[c]));
        
        ClearSquares();
    }
    public void Defend()
    {
        units[c].UnitsGO.GetComponent<Image>().color = Color.blue;
        units[c].Defense = true;
        MakeMovement();
        ClearSquares();
        TurnOffButtons();
    }

    public void Move()
    {
        MakePosibleMovement(CheckPosibleMoves(units[c]));
        TurnOffButtons();
    }

    public void MakePosibleMovement(List<int> posibleAttacks)
    {
        foreach(int i in posibleAttacks)
        {
            squares.transform.GetChild(i).GetComponent<Image>().color = Color.magenta;
            squares.transform.GetChild(i).GetComponent<SquareType>().ActivateChange();
        }
    }
    public void BotMovement()
    {

        List<int> posibleAttacks = GetPosibleAttack(units[c]);
        print(units[c].UnitsGO.transform.parent.GetComponent<SquareType>().index);
        print(posibleAttacks.Count);
        if(posibleAttacks.Count < 1)
        {
            List<int> posibleMoves = CheckPosibleMoves(units[c]);
            if(posibleMoves.Count < 1)
            {
                Defend();

            }
            else
            {
                print("llego aca");
                //int max = 0;
                int selected = posibleMoves[0];
                int actualIndex = units[c].UnitsGO.transform.parent.GetComponent<SquareType>().index;
                //foreach (int index in posibleMoves)
                //{ 
                //    ChangeUnits(actualIndex, index);
                //    int attacks = CheckPosibleMoves(units[c]).Count;
                //    if (attacks > max) selected = index;
                //    ChangeUnits(actualIndex, index);
                //}
                print("hasta aca tambien");
                ChangeUnits(actualIndex, selected);
                MakeMovement();
            }

        }
        else
        {
            //tomar enfoque greedy
            int max = 0;
            int selected = posibleAttacks[0];
            foreach(int index in posibleAttacks)
            {
                
                int pdamage = CheckDamage(units[c], UnitByIndex(index));
                if (pdamage > max)
                {
                    selected = index;
                }
                
            }
            MakeDamage(units[c], UnitByIndex(selected));



        }
        



    }
    private int CheckAdvantage(UnitGroup attackGroup, UnitGroup defenseGroup)
    {
        string[] strongTo = attackGroup.UnitCombat.StrongTo;
        //string[] strongTo = unitTypes[attackGroup.Type].StrongTo;
        for (int i = 0; i < strongTo.Length; i++)
        {
            //   if (defenseGroup.Type == strongTo[i]) return 2;
            if (defenseGroup.UnitCombat.GetType().ToString() == strongTo[i]) return 2;
        }
        string[] weakTo = attackGroup.UnitCombat.WeakTo;
        //string[] weakTo = unitTypes[attackGroup.Type].WeakTo;
        for (int i = 0; i < weakTo.Length; i++)
        {
            //   if (defenseGroup.Type== weakTo[i]) return 1;
            if (defenseGroup.UnitCombat.GetType().ToString() == weakTo[i]) return 1;
        }
        return 0;
    }
    private int CheckDamage(UnitGroup attackGroup, UnitGroup defenseGroup)
    {
        int damage = attackGroup.UnitCombat.Attack;
        int advantage = CheckAdvantage(attackGroup, defenseGroup);
        if (advantage == 2)
        {
            damage *= 2;
        }
        else if (advantage == 1)
        {
            damage /= 2;
        }
        return damage;
    }
    public void MakeDamage(UnitGroup attackGroup, UnitGroup defenseGroup)
    {
        print("se hizo daño");
        //DA�O VARIA RESPECTO AL TYPE

        int damage = attackGroup.UnitCombat.Attack;
        int presicion = attackGroup.UnitCombat.Precision;


        //int damage = unitTypes[attackGroup.Type].Attack;
        //int presicion = unitTypes[attackGroup.Type].Presicion;
        int presicion_hit = Random.Range(1, 100);
        if (presicion > presicion_hit)
        {
            int advantage = CheckAdvantage(attackGroup, defenseGroup);
            if (advantage == 2)
            {
                damage *= 2;
            }
            else if (advantage == 1)
            {
                damage /= 2;
            }
            int critic = Random.Range(1, 100);
            if (critic < 10) 
            {
                damage *= 2;
                ShowFloatText("Critic!", attackGroup.UnitsGO);
            }
            if (defenseGroup.Defense) damage /= 2;
           
            
        }
        else
        {
            ShowFloatText("Miss!", attackGroup.UnitsGO);
            damage = 0;
        }

        // defenseGroup.Quantity = defenseGroup.Quantity - damage;
        
        defenseGroup.UnitCombat.Quantity = defenseGroup.UnitCombat.Quantity - damage;

        //defenseGroup.UnitsGO.transform.GetChild(0).GetComponent<Text>().text = defenseGroup.Quantity.ToString();
        defenseGroup.UnitsGO.transform.GetChild(0).GetComponent<Text>().text = defenseGroup.UnitCombat.Quantity.ToString();


        damage = defenseGroup.UnitCombat.Attack;
        presicion = defenseGroup.UnitCombat.Precision;


        presicion_hit = Random.Range(1, 100);
        if (presicion > presicion_hit)
        {
            int advantage = CheckAdvantage( defenseGroup, attackGroup);
            if (advantage == 2)
            {
                damage *= 2;
            }
            else if (advantage == 1)
            {
                damage /= 2;
            }
            int critic = Random.Range(1, 100);
            if (critic < 10)
            {
                damage *= 2;
                ShowFloatText("Critic!", defenseGroup.UnitsGO);
            }
            if (attackGroup.Defense) damage /= 2;

        }
        else
        {
            ShowFloatText("Miss!", attackGroup.UnitsGO);
            damage = 0;
        }
        print("la segunda cantidad es: " + attackGroup.UnitCombat.Quantity);
        attackGroup.UnitCombat.Quantity = attackGroup.UnitCombat.Quantity - damage;
        
        print("el segundo damage es: " + damage);
        attackGroup.UnitsGO.transform.GetChild(0).GetComponent<Text>().text = attackGroup.UnitCombat.Quantity.ToString();




        //VALIDACIONES FINALES
        TurnOffButtons();
        //if (defenseGroup.Quantity <= 0)
        if (defenseGroup.UnitCombat.Quantity <= 0)
        {
            //defenseGroup.Quantity = 0;
            defenseGroup.UnitCombat.Quantity = 0;
            defenseGroup.UnitsGO.transform.GetChild(0).GetComponent<Text>().text = "0";
            defenseGroup.UnitsGO.transform.GetChild(0).GetComponent<Text>().color = Color.red;
            SetStats(defenseGroup);
            units.Remove(defenseGroup);
            defenseGroup.UnitsGO.GetComponentInParent<SquareType>().unitGroup = null;
            defenseGroup.UnitsGO.GetComponentInParent<SquareType>().haveUnit = false;
            DestroyGameObjectAndChildren(defenseGroup.UnitsGO);
        }
        if (attackGroup.UnitCombat.Quantity <= 0)
        {
            print("suicidio");
            //defenseGroup.Quantity = 0;
            attackGroup.UnitCombat.Quantity = 0;
            attackGroup.UnitsGO.transform.GetChild(0).GetComponent<Text>().text = "0";
            attackGroup.UnitsGO.transform.GetChild(0).GetComponent<Text>().color = Color.red;
            SetStats(attackGroup);
            units.Remove(attackGroup);
            attackGroup.UnitsGO.GetComponentInParent<SquareType>().unitGroup = null;
            attackGroup.UnitsGO.GetComponentInParent<SquareType>().haveUnit = false;
            DestroyGameObjectAndChildren(attackGroup.UnitsGO);
        }
        if (EndBattle())
        {
            FinishCombat();
        }
        else
        {
            MakeMovement();
        }
    }
    private void ShowFloatText(string text, GameObject go)
    {
        InGameMenuHandler.instance.ShowFloatingText(text, "TextFloating", go.transform, Color.black);
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
    
    private List<int> GetPosibleAttack(UnitGroup attacker)
    {
        List<int> posibleAttack = new List<int>();

        int axisy = 4;
        int axisx = 1;
        int d1 = 5;
        int d2 = 3;
        int index = attacker.UnitsGO.transform.parent.GetComponent<SquareType>().index;
        print(index);
        //  int range = unitTypes[attacker.Type].Range;
        int range = attacker.UnitCombat.Range;
        int newIndex = 0;

        for (int i = 1; i <= range; i++)
        {
            //left
            newIndex = index - (axisx * i);
            if (newIndex % 4 != 0) {
                if (newIndex > 0 && newIndex < squares.transform.childCount && UnitByIndex(newIndex) != null)
                {
                    if (UnitByIndex(newIndex).TypePlayer != attacker.TypePlayer)
                    {
                        posibleAttack.Add(newIndex);
                    }
                }
            }
            
            //right
            newIndex = index + (axisx * i);
            if (newIndex % 4 != 0)
            {
                if (newIndex > 0 && newIndex < squares.transform.childCount && UnitByIndex(newIndex) != null)
                {
                    if (UnitByIndex(newIndex).TypePlayer != attacker.TypePlayer)
                    {
                        posibleAttack.Add(newIndex);
                    }
                }
            }
            //top
            newIndex = index - (axisy * i);
            if (newIndex > 0 && newIndex < squares.transform.childCount && UnitByIndex(newIndex) != null)
            {
                if (UnitByIndex(newIndex).TypePlayer != attacker.TypePlayer)
                {
                    posibleAttack.Add(newIndex);
                }
            }
            //down
            newIndex = index + (axisy * i);
            if (newIndex > 0 && newIndex < squares.transform.childCount && UnitByIndex(newIndex) != null)
            {
                if (UnitByIndex(newIndex).TypePlayer != attacker.TypePlayer)
                {
                    posibleAttack.Add(newIndex);
                }
            }

            //topleft
            newIndex = index - (d1 * i);
            if (newIndex % 4 != 0)
            {
                if (newIndex > 0 && newIndex < squares.transform.childCount && UnitByIndex(newIndex) != null)
                {
                    if (UnitByIndex(newIndex).TypePlayer != attacker.TypePlayer)
                    {
                        posibleAttack.Add(newIndex);
                    }
                }
            }
            //downright
            newIndex = index + (d1 * i);
            if (newIndex % 4 != 0)
            {
                if (newIndex > 0 && newIndex < squares.transform.childCount && UnitByIndex(newIndex) != null)
                {
                    if (UnitByIndex(newIndex).TypePlayer != attacker.TypePlayer)
                    {
                        posibleAttack.Add(newIndex);
                    }
                }
            }
            //topright
            newIndex = index - (d2 * i);
            if (newIndex % 4 != 0)
            {
                if (newIndex > 0 && newIndex < squares.transform.childCount && UnitByIndex(newIndex) != null)
                {
                    if (UnitByIndex(newIndex).TypePlayer != attacker.TypePlayer)
                    {
                        posibleAttack.Add(newIndex);
                    }
                }
            }
            //downleft
            newIndex = index + (d2 * i);
            if (newIndex % 4 != 0)
            {
                if (newIndex > 0 && newIndex < squares.transform.childCount && UnitByIndex(newIndex) != null)
                {
                    if (UnitByIndex(newIndex).TypePlayer != attacker.TypePlayer)
                    {
                        posibleAttack.Add(newIndex);
                    }
                }
            }
        }
        return posibleAttack;

    }

    private void ShowAttackButtons(List<int> posibleAttack)
    {
        UnitGroup attacker = units[c];
        foreach (int a in posibleAttack)
        {
            AttackUnitByIndex(a, attacker);
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

    private List<int> CheckPosibleMoves(UnitGroup unit)
    {

        int index = unit.UnitsGO.transform.parent.GetComponent<SquareType>().index;
        int newIndex;
        List<int> posibleMoves = new List<int>();
        //left
        newIndex = index - 1;
        if (newIndex >= 0 && newIndex < squares.transform.childCount) {
            if (index % 4 != 0)
            {
                if (SquareByIndex(newIndex).haveUnit == false || SquareByIndex(newIndex).unitGroup.TypePlayer == SquareByIndex(index).unitGroup.TypePlayer)
                {

                    posibleMoves.Add(newIndex);
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
                    posibleMoves.Add(newIndex);
                }
            }
        }
        
        //top
        newIndex = index - 4;
        if (newIndex >= 0 && newIndex < squares.transform.childCount)
        {
            if (SquareByIndex(newIndex).haveUnit == false || SquareByIndex(newIndex).unitGroup.TypePlayer == SquareByIndex(index).unitGroup.TypePlayer)
            {
                posibleMoves.Add(newIndex);
            }
        }
        
        //down
        newIndex = index + 4;
        if (newIndex >= 0 && newIndex < squares.transform.childCount)
        {
            if (SquareByIndex(newIndex).haveUnit == false || SquareByIndex(newIndex).unitGroup.TypePlayer == SquareByIndex(index).unitGroup.TypePlayer)
            {
                posibleMoves.Add(newIndex);
            }
        }

        return posibleMoves;
        
    }

    public int ActualUnitIndex()
    {
        return units[c].UnitsGO.transform.parent.GetComponent<SquareType>().index;
    }
    public void ChangeUnits(int index, int newIndex)
    {
        //print(index);
        //print(newIndex);
        SquareType square1 = SquareByIndex(index);
        SquareType square2 = SquareByIndex(newIndex);
        UnitGroup saveug = square1.unitGroup;

        //print(saveug);
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
    
    public UnitGroup ActualUnit()
    {
        return units[c];
    }
}
