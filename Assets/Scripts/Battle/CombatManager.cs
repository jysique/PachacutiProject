using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CombatManager : MonoBehaviour
{

    public static CombatManager instance;
    [SerializeField] public bool turn;
    [SerializeField] private Text turnsCounter;
    //objects
    [SerializeField]private GameObject squares;
    [SerializeField] private GameObject unitGroupPrefab;
    [SerializeField] public GameObject menu;

    [Header("Resume Battle Menu")]
    [SerializeField] private GameObject Block;
    [SerializeField] private GameObject ResumeBattle;
    [SerializeField] private Button closeResumen;
    [SerializeField] private GameObject Player1;
    [SerializeField] private GameObject Player2;
    [SerializeField] private TextMeshProUGUI conquistedTxt;

    [Header("Buttons")]
    [SerializeField] private Button activatSA;
    [SerializeField] private Button surrenderBtn;
    private GameObject canvas;
    public int acumulated = 0;
    [Header("Turn Signal")]
    [SerializeField] private GameObject turnSignal;
    //
    Transform menuOriginalPos;
    //battle resume(cambiar nombre)
    [Header("Attack Resume")]
    [SerializeField] private GameObject battleResume;
    [SerializeField] private GameObject attackerPicture;
    [SerializeField] private GameObject defenderPicture;
    [SerializeField] private GameObject attackerText;
    [SerializeField] private GameObject defenderText;
    private GroupClassContainer targetAttack;

    public bool blockScreen;
    public List<UnitGroup> units = new List<UnitGroup>();
    public UnitGroup selectedUnit;
    Dictionary<int, int> playerPositions = new Dictionary<int, int>();
    Dictionary<int, int> enemyPositions = new Dictionary<int, int>();
    
    TerritoryHandler playerTerritory;
    TerritoryHandler enemyTerritory;
    [Header("Config")]
    [SerializeField] private int turns;
    public int c;
    private MilitarChief mcPlayer;
    public int Turns
    {
        get { return turns; }
    }
    private void Awake()
    {
        instance = this;
        ResumeBattle.SetActive(false);
        Block.SetActive(false);
        closeResumen.onClick.AddListener(() => FinishCombat());
        surrenderBtn.onClick.AddListener(() => OpenResumeBattle());
        activatSA.onClick.AddListener(() => ActivateSpecialAbility());
        turn = true;
        turns = 20;
        c = -1;
        canvas = GameObject.Find("Battle");
        playerPositions.Add(0, 0);
        playerPositions.Add(1, 1);
        playerPositions.Add(2, 4);
        playerPositions.Add(3, 5);
        playerPositions.Add(4, 8);
        playerPositions.Add(5, 9);

        enemyPositions.Add(0, 3);
        enemyPositions.Add(1, 2);
        enemyPositions.Add(2, 7);
        enemyPositions.Add(3, 6);
        enemyPositions.Add(4, 11);
        enemyPositions.Add(5, 10);

        menuOriginalPos = menu.transform;
    }
    public GameObject Squares
    {
        get { return squares; }
    }
 
    
    public void ReturnMenu()
    {

        menu.transform.position = new Vector3(0,500,0);
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
        pref.transform.GetChild(2).GetComponent<Image>().sprite = unitCombat.Picture;
        pref.transform.GetChild(1).GetComponent<Image>().color = Color.blue;
        if (_type != Territory.TYPEPLAYER.PLAYER)
        {
            pref.transform.GetChild(1).GetComponent<Image>().color = Color.red;
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
    //isPlayer is true si player es defensor
    //isPlayer is false si player es atacante
    bool isPlayer;
    Troop defendOriginalTroop = new Troop();
    Troop attackerOriginalTroop = new Troop();
    Troop defendActualTroop = new Troop();
    Troop attackerActualTroop = new Troop();
    int start = 0;
    public void StartWar(Troop playerTroop, Troop enemyTroop, TerritoryHandler _playerTerritory, TerritoryHandler _enemyTerritory, bool isPlayerTerritory)
    {
        turn = true;
        turns = 20;

        if (start<1)
        {
            start++;
            int ift = PlayerPrefs.GetInt("tutorialState");
            if (ift == 0)
            {
                acumulated += playerTroop.GetAllNumbersUnit();
                SortList(units);
                EventManager.instance.AddBattleEvent(playerTroop, enemyTroop, _playerTerritory, _enemyTerritory, isPlayerTerritory);
            }
            //SortList(units);
            mcPlayer = _playerTerritory.TerritoryStats.Territory.MilitarChiefTerritory;

            isPlayer = isPlayerTerritory;
            playerTerritory = _playerTerritory;
            enemyTerritory = _enemyTerritory;
            TerritoryHandler territoryOfWar;
            List<Terrain> tiles;
            if (isPlayerTerritory)
            {
                territoryOfWar = playerTerritory;
                tiles = territoryOfWar.TerritoryStats.Territory.TerrainList;
                attackerOriginalTroop.SaveTroop(enemyTroop);
                defendOriginalTroop.SaveTroop(playerTroop);
            }
            else
            {
                territoryOfWar = enemyTerritory;
                tiles = territoryOfWar.TerritoryStats.Territory.TerrainList;
                attackerOriginalTroop.SaveTroop(playerTroop);
                defendOriginalTroop.SaveTroop(enemyTroop);
                List<Terrain> newTiles = new List<Terrain>();
                for (int y = (tiles.Count / 2) - 1; y >= 0; y--)
                {
                    newTiles.Add(tiles[y]);
                }
                for (int y = tiles.Count - 1; y >= (tiles.Count / 2); y--)
                {
                    newTiles.Add(tiles[y]);
                }
                tiles = newTiles;
            }
            for (int j = 0; j < squares.transform.childCount; j++)
            {
                squares.transform.GetChild(j).GetComponent<SquareType>().terrain = tiles[j];
                squares.transform.GetChild(j).GetComponent<SquareType>().index = j;
                squares.transform.GetChild(j).GetComponent<Image>().sprite = tiles[j].Picture;
            }
        }

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
        for (int j = 0; j < squares.transform.childCount; j++)
        {
            squares.transform.GetChild(j).GetComponent<SquareType>().UpdateSquare();
        }
        selectedUnit = units[0];
        turnSignal.GetComponent<AppearAndDissapearAnimation>().Change();
       
       //MakeMovement();
    }

    private void ActivateSpecialAbility()
    {
        mcPlayer.SpecialAbility(enemyTerritory.TerritoryStats.Territory.TypePlayer);
        
    }

    public void MakeBattleResume(UnitGroup attackGroup, UnitGroup defenseGroup, GroupClassContainer tA)
    {
        targetAttack = tA;
        attackerPicture.GetComponent<Image>().sprite = attackGroup.UnitCombat.Picture;
        defenderPicture.GetComponent<Image>().sprite = defenseGroup.UnitCombat.Picture;
        attackerText.GetComponent<TextMeshProUGUI>().text = "BASE DAMAGE: " + attackGroup.UnitCombat.Attack + "\n" +
            "DEFENSE: " + attackGroup.UnitCombat.Defense + "\n" +
        "PRESICION: " + attackGroup.UnitCombat.Precision + "\n" +
        "CRITIC: " + 10 + "\n" +
        "TOTAL DAMAGE: " + CheckDamage(attackGroup, defenseGroup) + "\n";
        defenderText.GetComponent<TextMeshProUGUI>().text = "BASE DAMAGE: " + defenseGroup.UnitCombat.Attack + "\n" +
            "DEFENSE: " + defenseGroup.UnitCombat.Defense + "\n" +
        "PRESICION: " + defenseGroup.UnitCombat.Precision + "\n" +
        "CRITIC: " + 10 + "\n" +
        "TOTAL DAMAGE: " + CheckDamage(defenseGroup, attackGroup) + "\n";
        battleResume.SetActive(true);

    }

    public void CloseResume()
    {
        battleResume.SetActive(false);
    }

    public void AttackUnit()
    {
        targetAttack.ReceiveDamage();
        CloseResume();
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

    private void ClearUnits()
    {
        foreach (UnitGroup u in units) 
        {
            if(u.TypePlayer == Territory.TYPEPLAYER.PLAYER && turn)
            {
                u.Defense = false;
                u.UnitsGO.transform.GetChild(4).gameObject.SetActive(false);
                u.UnitsGO.transform.GetChild(2).GetComponent<Image>().color = Color.white;
                u.Active = true;

            }
            else if (turn == false && u.TypePlayer != Territory.TYPEPLAYER.PLAYER)
            {
                u.Defense = false;
                u.UnitsGO.transform.GetChild(4).gameObject.SetActive(false);
                u.UnitsGO.transform.GetChild(2).GetComponent<Image>().color = Color.white;
                u.Active = true;
            }

        }
      
        
    }
    public void ChangeColorTurnSignal()
    {
        turnSignal.GetComponent<AppearAndDissapearAnimation>().ChangeColor();
        turnSignal.GetComponent<AppearAndDissapearAnimation>().Change();
    }
    private void ChangeSide()
    {
        turn = !turn;
        turnSignal.GetComponent<AppearAndDissapearAnimation>().Change();
        Invoke("ChangeColorTurnSignal", 1f);
        ClearUnits();

        if (turn == false)
        {
            
            StartCoroutine(BotTurn());
        }
       

    }

    public IEnumerator BotTurn()
    {
        List<UnitGroup> bots = new List<UnitGroup>();

        foreach(UnitGroup u in units)
        {
            if(u.TypePlayer != Territory.TYPEPLAYER.PLAYER)
            {
                bots.Add(u);
                
            }
            SortList(bots);
            
        }
        yield return new WaitForSeconds(1);
        foreach (UnitGroup u in bots)
        {
            print("bot juega");
            selectedUnit = u;
            BotMovement(u);
            yield return new WaitForSeconds(5);
            
        }
    }
    public void MakeMovement(bool isAlive)
    {
        print("movimiento terminado: " + selectedUnit.UnitCombat.UnitName);
        if(selectedUnit.UnitCombat.UnitName == "Lancer")
        {
            print(isAlive);
        }
        ReturnMenu();
        //cambiar la actividad
        if(isAlive)
        {
            selectedUnit.Active = false;

            selectedUnit.UnitsGO.transform.GetChild(2).GetComponent<Image>().color = Color.gray;
        }
        

        if (CheckFinishTurn())
        {
            
            turns--;
            print("si funca we");
            ChangeSide();
        }
        if (turns <= 0)
        {
            OpenResumeBattle();
           // FinishCombat();
        }
    }
    void OpenResumeBattle()
    {
        Player2.transform.GetChild(2).gameObject.SetActive(false);
        Player1.transform.GetChild(2).gameObject.SetActive(false);
        Block.SetActive(true);
        ResumeBattle.SetActive(true);
        if (isPlayer)
        {
            ShowInfo(Player1, enemyTerritory.TerritoryStats.Territory, attackerOriginalTroop, attackerActualTroop);
            ShowInfo(Player2, playerTerritory.TerritoryStats.Territory, defendOriginalTroop, defendActualTroop);
        }
        else
        {
            ShowInfo(Player1, playerTerritory.TerritoryStats.Territory, attackerOriginalTroop, attackerActualTroop);
            ShowInfo(Player2, enemyTerritory.TerritoryStats.Territory, defendOriginalTroop, defendActualTroop);
        }

        // active winner battle
        if (attackerActualTroop.GetAllNumbersUnit() == 0)
        {
            Player2.transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            Player1.transform.GetChild(2).gameObject.SetActive(true);
        }
        if (playerTerritory.TerritoryStats.Territory.Population == 0)
        {
            conquistedTxt.text = "Territorio Conquistado";
        }
        else
        {
            conquistedTxt.text = "Territorio No Conquistado";
        }

    }

    void ShowInfo(GameObject _go,Territory _territory,Troop _original, Troop _actual)
    {
       List<Transform> t= Utils.instance.GetAllChildren(_go.transform);
        t[0].GetComponent<TextMeshProUGUI>().text = _territory.TypePlayer.ToString();
        string details = "Details \n";
        details += " Lost units: \n";
        for (int j = 0; j < _original.UnitCombats.Count; j++)
        {
            UnitGroup _ug = FoundUnit(_territory, _original.UnitCombats[j], _original.Positions[j]);
            if (_ug != null)
            {
                //print(_territory.TypePlayer.ToString()+ "-"+ _original.UnitCombats[j].UnitName + "-" + _ug.UnitCombat.Quantity);
                //_actual.AddElement(_original.UnitCombats[j].UnitName, _original.UnitCombats[j].PositionInBattle, _ug.UnitCombat.Quantity);
                _actual.AddElement(_original.UnitCombats[j].UnitName, _original.Positions[j], _ug.UnitCombat.Quantity);
                //_actual.AddElement(_ug.UnitCombat, _ug.UnitCombat.PositionInBattle);
            }
            else
            {
                //print(_territory.TypePlayer.ToString() + "-" + _original.UnitCombats[j].UnitName + "- no encontrado");
                _actual.AddElement(_original.UnitCombats[j].UnitName, _original.Positions[j], 0);
            }
        }
        

        for (int i = 0; i < _original.UnitCombats.Count; i++)
        {
            details += " - " + _original.UnitCombats[i].UnitName + ":" + (_original.UnitCombats[i].Quantity - _actual.UnitCombats[i].Quantity) + "\n";
        }        
        t[1].GetComponent<TextMeshProUGUI>().text = details;
        
    }
    private UnitGroup FoundUnit(Territory _territory, UnitCombat uc, int posInBattle)
    {
        int squares_count = Squares.transform.childCount;
        for (int i = 0; i < squares_count; i++)
        {
            SquareType _square = Squares.transform.GetChild(i).gameObject.GetComponent<SquareType>();
            UnitGroup _ug = _square.unitGroup;
            if (_ug != null && _ug.TypePlayer == _territory.TypePlayer)
            {
                //print("original: " + _ug.TypePlayer + " - " + uc.UnitName + "-" + posInBattle);
                //print("actual: " + _ug.TypePlayer + " - " + uc.UnitName + "-" + _ug.UnitCombat.PositionInBattle);
            }
            //if (_ug != null && _ug.TypePlayer == _territory.TypePlayer && _ug.UnitCombat.PositionInBattle == uc.PositionInBattle)
            if (_ug != null && _ug.TypePlayer == _territory.TypePlayer && _ug.UnitCombat.PositionInBattle == posInBattle)
            {
                //print("original: " + _ug.TypePlayer + " - " + uc.UnitName + "-" + posInBattle);
                //print("actual: " + _ug.TypePlayer + " - " + uc.UnitName + "-" + _ug.UnitCombat.PositionInBattle);
                return _ug;
            }
        }
        return null;
    }


    private bool CheckFinishTurn()
    {
        foreach(UnitGroup u in units)
        {
            if (turn)
            {
                if (u.TypePlayer == Territory.TYPEPLAYER.PLAYER)
                {
                    if (u.Active) { return false; }
                }
            
                
            }
            else 
            {
                if (u.TypePlayer != Territory.TYPEPLAYER.PLAYER)
                {
                    if (u.Active) {

                        return false; 
                    }
                }
            }
        }
        return true;
    }
    private void SetStats(UnitGroup u)
    {
        string _type = u.UnitCombat.UnitName;
        if (u.TypePlayer == Territory.TYPEPLAYER.PLAYER)
        {
            if (isPlayer)
            {
                playerTerritory.TerritoryStats.Territory.GetUnit(_type).Quantity = u.UnitCombat.Quantity;
            }
            else
            {
                playerTerritory.TerritoryStats.Territory.GetUnit(_type).Quantity += u.UnitCombat.Quantity;
            }
        }
        else
        {
            if (isPlayer)
            {
                enemyTerritory.TerritoryStats.Territory.GetUnit(_type).Quantity += u.UnitCombat.Quantity;
            }
            else
            {
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
        DateTableHandler.instance.ResumeTime();

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
                SquareByIndex(i).unitGroup = null;
                DestroyGameObjectAndChildren(squares.transform.GetChild(i).GetChild(0).gameObject);
            }
        }
        if (TutorialController.instance.IsFirstTime)
        {
            TutorialController.instance.TurnOnDialogue();
        }
        ResetAllElements();
    }

    private void ResetAllElements()
    {
        units.Clear();
        EventManager.instance.listEvents.ResetAllBattleEvents();
        ResumeBattle.SetActive(false);
        attackerActualTroop.Reset();
        defendActualTroop.Reset();
        attackerOriginalTroop.Reset();
        defendActualTroop.Reset();
        Block.SetActive(false);
        canvas.SetActive(false);
        start = 0;
    }

     
    public void Attack()
    {
        ShowAttackButtons(GetPosibleAttack(selectedUnit));
        ClearSquares();
    }
    public void Defend()
    {
        
        selectedUnit.UnitsGO.transform.GetChild(4).gameObject.SetActive(true);
        selectedUnit.Defense = true;
        MakeMovement(true);
        ClearSquares();
        TurnOffButtons();
    }
    public void Move()
    {
        MakePosibleMovement(CheckPosibleMoves(selectedUnit));
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
    public void BotMovement(UnitGroup bot)
    {

        List<int> posibleAttacks = GetPosibleAttack(bot);
        //print(units[c].UnitsGO.transform.parent.GetComponent<SquareType>().index);
        //print(posibleAttacks.Count);
        if(posibleAttacks.Count < 1)
        {
            List<int> posibleMoves = CheckPosibleMoves(bot);
            if(posibleMoves.Count < 1)
            {
                Defend();

            }
            else
            {
                //print("llego aca");
                //int max = 0;
                int probDefense = Random.Range(0,100);
                if (probDefense > 50)
                {
                    Defend();
                }
                else
                {
                    int selected = posibleMoves[0];
                    int actualIndex = bot.UnitsGO.transform.parent.GetComponent<SquareType>().index;
                    //foreach (int index in posibleMoves)
                    //{ 
                    //    ChangeUnits(actualIndex, index);
                    //    int attacks = CheckPosibleMoves(units[c]).Count;
                    //    if (attacks > max) selected = index;
                    //    ChangeUnits(actualIndex, index);
                    //}
                    //print("hasta aca tambien");
                    ChangeUnits(actualIndex, selected);
                }
                
                
            }

        }
        else
        {
            //tomar enfoque greedy
            int max = 0;
            int selected = posibleAttacks[0];
            foreach(int index in posibleAttacks)
            {
                
                int pdamage = CheckDamage(bot, UnitByIndex(index));
                if (pdamage > max)
                {
                    selected = index;
                }
                
            }
            StartCoroutine(MakeDamage(bot, UnitByIndex(selected)));
           



        }
        



    }
    private int CheckAdvantage(UnitGroup attackGroup, UnitGroup defenseGroup)
    {
        string[] strongTo = attackGroup.UnitCombat.StrongTo;
        for (int i = 0; i < strongTo.Length; i++)
        {
            if (defenseGroup.UnitCombat.GetType().ToString() == strongTo[i]) return 2;
        }
        string[] weakTo = attackGroup.UnitCombat.WeakTo;
        for (int i = 0; i < weakTo.Length; i++)
        {
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
        damage = damage - attackGroup.UnitCombat.Defense;
        if (damage < 0) damage = 0;
        return damage;
    }
    int a = 0;
    private void CheckTutorial(UnitGroup attackGroup)
    {
        bool x = attackGroup.UnitCombat.UnitName == "Archer" && attackGroup.TypePlayer == Territory.TYPEPLAYER.PLAYER;
        bool y = attackGroup.UnitCombat.UnitName == "Swordsman" && attackGroup.TypePlayer == Territory.TYPEPLAYER.PLAYER;
        if (x || y)
        {
            a++;
        }
        if (a == 2)
        {
            TutorialController.instance.IsAttacking = true;
        }
    }
    public void UpdateText(UnitGroup ug)
    {
        ug.UnitsGO.transform.GetChild(0).GetComponent<Text>().text = ug.UnitCombat.Quantity.ToString();
    }


    private IEnumerator AttackAnimation(UnitGroup defenseGroup,GameObject unit, bool crit, bool miss, int damage, bool isPlayer)
    {
        GameObject unitRot = unit.transform.GetChild(2).gameObject;
        
        for (int i = 0; i > -90; i -= 1)
        {
            var rotationVector = unitRot.transform.rotation.eulerAngles;
            rotationVector.z = i;
            unitRot.transform.rotation = Quaternion.Euler(rotationVector);
            yield return new WaitForSeconds(0.005f);
        }
        if(!miss)ShowFloatText("- " + damage + "!", defenseGroup.UnitsGO);
        if(crit)ShowFloatText("Critic!", unit);
        if(miss)ShowFloatText("Miss!", unit);
        defenseGroup.UnitCombat.Quantity = defenseGroup.UnitCombat.Quantity - damage;
        defenseGroup.UnitsGO.transform.GetChild(0).GetComponent<Text>().text = defenseGroup.UnitCombat.Quantity.ToString();
        for (int i = -90; i < 0; i += 1)
        {
            var rotationVector = unitRot.transform.rotation.eulerAngles;
            rotationVector.z = i;
            unitRot.transform.rotation = Quaternion.Euler(rotationVector);
            yield return new WaitForSeconds(0.005f);
        }
    }
    
    public IEnumerator MakeDamage(UnitGroup attackGroup, UnitGroup defenseGroup)
    {
        blockScreen = true;
        ReturnMenu();
        TurnOffButtons();
        //variables iniciales
        bool critictext=false;
        bool misstext=false;

        //tuturial
        CheckTutorial(attackGroup);

        //calculate damage with critic and presicion
        int damage = CheckDamage(attackGroup,defenseGroup);
        int presicion = attackGroup.UnitCombat.Precision;
        int presicion_hit = Random.Range(1, 100);
        if (TutorialController.instance.IsFirstTime)
        {
            presicion_hit = 0;
        }
        if (presicion > presicion_hit)
        {
            int critic = Random.Range(1, 100);
            if (TutorialController.instance.IsFirstTime)
            {
                critic = 100;
            }
            if (critic < 10)
            {
                damage *= 2;
                critictext = true;
            }
            if (defenseGroup.Defense) damage /= 2;
        }
        else
        {
            misstext = true;
            damage = 0;
        }
        //make attack animation
        StartCoroutine(AttackAnimation(defenseGroup,attackGroup.UnitsGO,critictext,misstext,damage,true));
        yield return new WaitForSeconds(2);

        critictext = false;
        misstext = false;
        //response attack
        if (defenseGroup.UnitCombat.Range>= attackGroup.UnitCombat.Range && defenseGroup.UnitCombat.Quantity > 0)
        {
            damage = CheckDamage(defenseGroup, attackGroup);
            presicion = defenseGroup.UnitCombat.Precision;
            presicion_hit = Random.Range(1, 100);
            if (TutorialController.instance.IsFirstTime)
            {
                presicion_hit = 0;
            }
            if (presicion > presicion_hit)
            {

                int critic = Random.Range(1, 100);
                if (TutorialController.instance.IsFirstTime)
                {
                    critic = 100;
                }
                if (critic < 10)
                {
                    damage *= 2;
                    critictext = true;

                }
                if (attackGroup.Defense) damage /= 2;

            }
            else
            {
                misstext = true;
                damage = 0;
            }
            StartCoroutine(AttackAnimation(attackGroup, defenseGroup.UnitsGO, critictext, misstext, damage, false));
            yield return new WaitForSeconds(2);
        }




        //VALIDACIONES FINALES
        bool alive = true;
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
            alive = false;
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
            OpenResumeBattle();
            //FinishCombat();
        }
        else
        {
          
            MakeMovement(alive);
        }
        yield return new WaitForSeconds(0);
        blockScreen = false;
    }
    public void ShowFloatText(string text, GameObject go)
    {
        InGameMenuHandler.instance.ShowFloatingText(text, "TextFloating", go.transform, Color.black);
    }
    public void AnimationInBattle(GameObject go, string name)
    {
        GameObject prefab = Resources.Load("Prefabs/BattlePrefabs/"+ name) as GameObject;
        GameObject rock = Instantiate(prefab, FindObjectOfType<Canvas>().transform);
        rock.transform.SetParent(go.transform.transform);
        Destroy(rock, 1);
    }
    private void TurnOffButtons()
    {
        foreach (UnitGroup u in units)
        {
            //if(u.TypePlayer != Territory.TYPEPLAYER.PLAYER)
            
            u.UnitsGO.transform.GetChild(3).gameObject.SetActive(false);
            
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
        //print(index);
        //  int range = unitTypes[attacker.Type].Range;
        int range = attacker.UnitCombat.Range;
        int newIndex = 0;

        for (int i = 1; i <= range; i++)
        {
            //left
            newIndex = index - (axisx * i);
            if (index % 4 != 0) {
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
            if (index % 4 != 0)
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
            if (((index + 1) % 4) != 0)
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
            
            if (((index+1)% 4) != 0)
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
            if (index % 4 != 0)
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
        UnitGroup attacker = selectedUnit;
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
                UnitByIndex(index).UnitsGO.transform.GetChild(3).gameObject.SetActive(true);
            }
        }
    }

    public UnitGroup UnitByIndex(int index)
    {
        return squares.transform.GetChild(index).GetComponent<SquareType>().unitGroup;
    }
    public UnitCombat GetUnitBySquare(int index)
    {
        return squares.transform.GetChild(index).GetComponent<GroupClassContainer>().stats.UnitCombat;
    }

    public SquareType SquareByIndex(int index)
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
                    if(SquareByIndex(newIndex).terrain.Type != Terrain.TYPE.NONE) posibleMoves.Add(newIndex);

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
                    if (SquareByIndex(newIndex).terrain.Type != Terrain.TYPE.NONE) posibleMoves.Add(newIndex);
                }
            }
        }
        
        //top
        newIndex = index - 4;
        if (newIndex >= 0 && newIndex < squares.transform.childCount)
        {
            if (SquareByIndex(newIndex).haveUnit == false || SquareByIndex(newIndex).unitGroup.TypePlayer == SquareByIndex(index).unitGroup.TypePlayer)
            {
                if (SquareByIndex(newIndex).terrain.Type != Terrain.TYPE.NONE) posibleMoves.Add(newIndex);
            }
        }
        
        //down
        newIndex = index + 4;
        if (newIndex >= 0 && newIndex < squares.transform.childCount)
        {
            if (SquareByIndex(newIndex).haveUnit == false || SquareByIndex(newIndex).unitGroup.TypePlayer == SquareByIndex(index).unitGroup.TypePlayer)
            {
                if (SquareByIndex(newIndex).terrain.Type != Terrain.TYPE.NONE) posibleMoves.Add(newIndex);
            }
        }

        return posibleMoves;
        
    }

    public int ActualUnitIndex()
    {
        return selectedUnit.UnitsGO.transform.parent.GetComponent<SquareType>().index;
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

        for (int j = 0; j < squares.transform.childCount; j++)
        {
            squares.transform.GetChild(j).GetComponent<SquareType>().UpdateSquare();
        }

        ClearSquares();
        MakeMovement(true);
    }

    public void ClearSquares()
    {
        for(int i = 0; i < squares.transform.childCount; i++)
        {
            squares.transform.GetChild(i).GetComponent<Image>().color = new Color32(168,161,161,139);
            squares.transform.GetChild(i).GetComponent<SquareType>().DeactivateChange();
        }
    }
    
    public UnitGroup ActualUnit()
    {
        return selectedUnit;
    }
}
