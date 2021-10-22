using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CombatManager : MonoBehaviour
{

    public static CombatManager instance;
    [SerializeField] public bool turn;
    [SerializeField] private Text turnsCounter;
    [SerializeField] private SquareType[,] squaresGrid;
    //objects
    [SerializeField] private GameObject squares;
    [SerializeField] private GameObject unitGroupPrefab;
    [SerializeField] public GameObject menu;
    
    [Header("Maps")]
    [SerializeField] public GameObject map1;
    [SerializeField] public GameObject map2;
    [SerializeField] public GameObject map3;

    [Header("Resume Battle Menu")]
    [SerializeField] private Button closeResumen;
    [SerializeField] private GameObject Block;
    [SerializeField] private GameObject ResumeBattle;
    [SerializeField] private GameObject ResumePlayer;
    [SerializeField] private GameObject ResumeEnemy;
    [SerializeField] private TextMeshProUGUI conquistedTxt;

    [Header("Militar Chief")]
    [SerializeField] private GameObject militarchiefPlayer;
    [SerializeField] private GameObject militarchiefEnemy;
    

    [Header("Buttons")]
    [SerializeField] private Button activatSA;
    [SerializeField] private Button surrenderBtn;
    [SerializeField] private Button passTurnBtn;
    private GameObject canvas;

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

    [Header("Unit Resume")]
    [SerializeField] private GameObject unitResume;
    [SerializeField] private GameObject unitPicture;
    [SerializeField] private GameObject unitText;
    [SerializeField] private GameObject unitAttackBonus;
    [SerializeField] private GameObject unitDefenseBonus;
    [SerializeField] private GameObject unitPresicionBonus;
    public bool blockScreen;
    public List<UnitGroup> units = new List<UnitGroup>();
    public UnitGroup selectedUnit;
    List<int> playerPositions = new List<int>();
    List<int> enemyPositions = new List<int>();

    TerritoryHandler playerTerritory; //IQUIERDA 
    TerritoryHandler enemyTerritory; // DERECHA

    [SerializeField] Troop playerOriginalTroop;
    [SerializeField] Troop playerActualTroop;

    [SerializeField] Troop enemyOriginalTroop;
    [SerializeField] Troop enemyActualTroop;



    [Header("Config")]
    [SerializeField] private int turns;
    [SerializeField] private bool canBotTurn;
    [SerializeField] private bool canPlayerTurn;
    //[SerializeField] private int c;
    [SerializeField] private int limitSA;
    public bool isMenu;
    private bool attackerWin;
    private bool canSurrender;
    private bool canActiveSpecial;
    private bool isPlayerDefending;

    private int start = 0;
    private int counterWait;
    private int a = 0;
    public int acumulated = 0;
    private int activeCount;

    public GameObject Squares
    {
        get { return squares; }
    }
    public bool CanBotTurn
    {
        get { return canBotTurn; }
        set { canBotTurn = value; }
    }
    public bool CanPlayerTurn
    {
        get { return canPlayerTurn; }
        set { canPlayerTurn = value; }
    }
    public int Turns
    {
        get { return turns; }
    }
    public int LimitSA
    {
        get { return limitSA; }
        set { limitSA = value; }
    }
    private void Awake()
    {
        instance = this;
        canvas = GameObject.Find("Battle");
        turn = true;
        turns = 20;
      //  c = -1;
        ResumeBattle.SetActive(false);
        Block.SetActive(false);
        closeResumen.onClick.AddListener(() => FinishCombat());
        surrenderBtn.onClick.AddListener(() => OpenResumeBattle());
        passTurnBtn.onClick.AddListener(() => PassTurn());
        squaresGrid = new SquareType[3, 8];
        for (int i = 0; i < squares.transform.childCount; i++) {
            SquareType sq = squares.transform.GetChild(i).GetComponent<SquareType>();
            squaresGrid[sq.i, sq.j] = sq;
        }
    }
    
    private void Update()
    {
        surrenderBtn.interactable = canSurrender;
        passTurnBtn.interactable = turn;
        CheckSpecialAbility();
        CheckIsEvent();
        CheckIsEvent2();
    }

    [SerializeField] private int currentBattle;
    private void CheckBattleEvent(int _turn)
    {
        if (EventManager.instance.listEvents.BattleEvents.Count > 0)
        {
            for (int i = 0; i < EventManager.instance.listEvents.BattleEvents.Count; i++)
            {
                if (_turn == EventManager.instance.listEvents.BattleEvents[i].TurnEvent)
                {
                    CustomBattle customBattle = EventManager.instance.listEvents.BattleEvents[i];
                    BattleEventController.instance.Init(customBattle);
                    currentBattle = i;
                    customBattle.EventStatus = CustomEvent.STATUS.PROGRESS;
                }
            }
        }

    }
    public void StartCombat(TerritoryHandler defenderTerritory, TerritoryHandler attackerTerritory, Troop defenderTroop, Troop attackerTroop)
    {
        DateTableHandler.instance.PauseTime();
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ReadAndPlayMusic("fight", false);
        }
        map1.SetActive(false);
        map2.SetActive(false);
        map3.SetActive(false);
        canSurrender = true;
        canBotTurn = true;
        canPlayerTurn = true;
        turn = true;
        attackerWin = false;
        canActiveSpecial = true;
        activeCount = 0;
        limitSA = 1;
        turns = 20;
        

        if (start < 1)
        {
            start++;

            isPlayerDefending = defenderTerritory.Territory.TypePlayer == Territory.TYPEPLAYER.PLAYER;
            if (isPlayerDefending)
            {
                print("jugador defendiento");
                playerTerritory = defenderTerritory;
                enemyTerritory = attackerTerritory;
                playerOriginalTroop.GetNewTroop(defenderTroop);
                enemyOriginalTroop.GetNewTroop(attackerTroop);
            }
            else
            {
                print("jugador atacando");
                playerTerritory = attackerTerritory;
                enemyTerritory = defenderTerritory;
                playerOriginalTroop.GetNewTroop(attackerTroop);
                enemyOriginalTroop.GetNewTroop(defenderTroop);
            }
            int ift = PlayerPrefs.GetInt("tutorialState");
            if (ift == 0)
            {
                acumulated += playerOriginalTroop.GetAllNumbersUnit();
                SortList(units);
                EventManager.instance.AddBattleEvents(playerOriginalTroop, enemyOriginalTroop, playerTerritory, enemyTerritory, isPlayerDefending);
                CheckBattleEvent(turns);
            }
            for (int i = 0; i < defenderTerritory.Territory.TerrainAttackList.Count; i++)
            {
                playerPositions.Add(defenderTerritory.Territory.TerrainAttackList[i].Position);
            }
            for (int i = 0; i < defenderTerritory.Territory.TerrainDeffendList.Count; i++)
            {
                enemyPositions.Add(defenderTerritory.Territory.TerrainDeffendList[i].Position);
            }
            List<Terrain> tiles = GetTerrains(defenderTerritory.Territory);
            
            for (int j = 0; j < squares.transform.childCount; j++)
            {
                squares.transform.GetChild(j).GetComponent<SquareType>().Terrain = tiles[j];
                squares.transform.GetChild(j).GetComponent<SquareType>().Index = j;
                squares.transform.GetChild(j).GetComponent<Image>().sprite = tiles[j].Picture;
            }
            SetMilitarChief(militarchiefPlayer, playerTerritory.Territory.MilitarChiefTerritory, playerTerritory.Territory.TypePlayer);
            SetMilitarChief(militarchiefEnemy, enemyTerritory.Territory.MilitarChiefTerritory, enemyTerritory.Territory.TypePlayer);
        }
        for (int i = 0; i < playerOriginalTroop.UnitCombats.Count; i++)
        {
            int up = playerPositions[i];
            InstantiateUnit(squares.transform.GetChild(up).gameObject, playerTerritory.Territory.TypePlayer, playerOriginalTroop.UnitCombats[i]);
        }
        for (int i = 0; i < enemyOriginalTroop.UnitCombats.Count; i++)
        {
            int up = enemyPositions[i];
            InstantiateUnit(squares.transform.GetChild(up).gameObject, enemyTerritory.Territory.TypePlayer, enemyOriginalTroop.UnitCombats[i]);
        }
        UpdateBuffTerrain();
        selectedUnit = units[0];
        turnSignal.GetComponent<AppearAndDissapearAnimation>().Change();
    }
   
    private List<Terrain> GetTerrains(Territory _territory)
    {
        int deffendCounter = 0;
        int attackCounter = 0;
        List<Terrain> tiles = new List<Terrain>();
        for (int i = 0; i < squares.transform.childCount; i++)
        {
            if ( _territory.TerrainDeffendList[deffendCounter].Position == i)
            {
                tiles.Add(_territory.TerrainDeffendList[deffendCounter]);
                if (deffendCounter < _territory.TerrainDeffendList.Count - 1)
                {
                    deffendCounter++;
                }
            }else if (_territory.TerrainAttackList[attackCounter].Position == i)
            {
                
                tiles.Add(_territory.TerrainAttackList[attackCounter]);
                if (attackCounter < _territory.TerrainAttackList.Count - 1)
                {
                    attackCounter++;
                }
            }
            else
            {
                tiles.Add(new Terrain("NONE", i));
            }
        }
        return tiles;
    }
    private void CheckSpecialAbility()
    {

        if (canActiveSpecial && activeCount < limitSA && playerTerritory.Territory.MilitarChiefTerritory.CheckCanSpecialAbility())
        {
            activatSA.interactable = true;
        }
        else
        {
            activatSA.interactable = false;
        }
    }
    public void ReturnMenu()
    {
        menu.transform.position = new Vector3(0, 500, 0);
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
    public void InstantiateUnitPlayer(int pos, UnitCombat unitCombat)
    {
        GameObject square = squares.transform.GetChild(pos).gameObject;
        InstantiateUnit(square, Territory.TYPEPLAYER.PLAYER, unitCombat);
    }
    public void InstantiateUnitPlayer(GameObject square, UnitCombat unitCombat)
    {
        InstantiateUnit(square, Territory.TYPEPLAYER.PLAYER, unitCombat);
    }
    private void InstantiateUnit(GameObject square, Territory.TYPEPLAYER _type, UnitCombat _unitCombat)
    {
        if (square.GetComponent<SquareType>().HaveUnit && square.transform.childCount > 0)
        {
            //print("destroy");
            units.Remove(square.GetComponent<SquareType>().UnitGroup);
            Destroy(square.transform.GetChild(0).gameObject);
        }
        GameObject go = Resources.Load("Prefabs/BattlePrefabs/Test/"+_unitCombat.CharacterName+ "unitGroup") as GameObject;
        var pref = Instantiate(go);
        pref.transform.SetParent(square.transform, false);

        UnitCombat unitCombat = Utils.instance.CreateNewUnitCombat(_unitCombat.CharacterName,_unitCombat.Quantity);
        unitCombat.InProgress = _unitCombat.InProgress;
//        unitCombat.Quantity = _unitCombat.Quantity;
        unitCombat.IsAvailable = _unitCombat.IsAvailable;
        unitCombat.PositionInBattle = _unitCombat.PositionInBattle;

        List<Transform> transforms = Utils.instance.GetAllChildren(pref.transform);
        pref.transform.GetChild(0).GetComponent<Text>().text = unitCombat.Quantity.ToString();
        pref.transform.GetChild(1).GetComponent<Image>().color = Color.blue;

        if (_type != Territory.TYPEPLAYER.PLAYER)
        {
            pref.transform.GetChild(1).GetComponent<Image>().color = Color.red;
        }


        UnitGroup unit = new UnitGroup(_type, pref, unitCombat);
        square.GetComponent<SquareType>().UnitGroup = unit;
        square.GetComponent<SquareType>().HaveUnit = true;

        units.Add(unit);
        pref.GetComponent<GroupClassContainer>().stats = unit;
        GroupClassContainer groupClass = pref.GetComponent<GroupClassContainer>();
        int j = square.GetComponent<SquareType>().j;
        bool _a = j >= 4 && j <= 7;
        bool _b = j >= 12  && j <= 15;
        bool _c = j >= 20 && j <= 24;
        if (_a || _b || _c)
            groupClass.IsLeft();
        else
            groupClass.IsRight();
    }
    public void UpdateBuffTerrain()
    {
        for (int j = 0; j < squares.transform.childCount; j++)
        {
            squares.transform.GetChild(j).GetComponent<SquareType>().UpdateSquare();
        }
    }
    private void ActivateSpecialAbility(MilitarChief _militar)
    {
        //_militar.SpecialAbility(Territory.TYPEPLAYER.PLAYER, enemyTerritory.Territory.TypePlayer);
        _militar.SpecialAbility(playerTerritory.Territory, enemyTerritory.Territory);
        canActiveSpecial = false;
        activeCount++;
    }
    public void MakeBattleResume(UnitGroup attackGroup, UnitGroup defenseGroup, GroupClassContainer tA)
    {
        targetAttack = tA;
        attackerPicture.GetComponent<Image>().sprite = attackGroup.UnitCombat.PictureSpriteSheet;
        defenderPicture.GetComponent<Image>().sprite = defenseGroup.UnitCombat.PictureSpriteSheet;
        attackerText.GetComponent<TextMeshProUGUI>().text = attackGroup.GetStats() + "TOTAL DAMAGE: " + CheckDamage(attackGroup, defenseGroup) + "\n";
        defenderText.GetComponent<TextMeshProUGUI>().text = defenseGroup.GetStats() + "TOTAL DAMAGE: " + CheckDamage(defenseGroup, attackGroup) + "\n";

        battleResume.SetActive(true);
    }
    public void MakeUnitResume()
    {
        AudioManager.instance.ReadAndPlaySFX("menu_click");
        UnitGroup unit = selectedUnit;
        unitPicture.GetComponent<Image>().sprite = unit.UnitCombat.PictureSpriteSheet;
        unitText.GetComponent<TextMeshProUGUI>().text = unit.GetStats();
        Terrain unitTerrain = selectedUnit.UnitsGO.GetComponentInParent<SquareType>().Terrain;
        if (unitTerrain.Plus_attack >  0)
        {
            unitAttackBonus.SetActive(true);
            unitAttackBonus.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+"+ unitTerrain.Plus_attack+"%";

        }
        else
        {
            unitAttackBonus.SetActive(false);
        }
        if (unitTerrain.Plus_defense > 0)
        {
            unitDefenseBonus.SetActive(true);
            unitDefenseBonus.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + unitTerrain.Plus_defense + "%";

        }
        else
        {
            unitDefenseBonus.SetActive(false);
        }
        if (unitTerrain.Plus_presicion > 0)
        {
            unitPresicionBonus.SetActive(true);
            unitPresicionBonus.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+" + unitTerrain.Plus_presicion + "%";

        }
        else
        {
            unitPresicionBonus.SetActive(false);
        }
        unitResume.SetActive(true);
    }

    public void CloseAttackResume()
    {
        battleResume.SetActive(false);
        AudioManager.instance.ReadAndPlaySFX("send_units");
    }


    public void CloseUnitResume()
    {
        unitResume.SetActive(false);
        AudioManager.instance.ReadAndPlaySFX("send_units");
    }

    public void CloseMenu()
    {
        CloseAttackResume();
        AudioManager.instance.ReadAndPlaySFX("send_units");
    }

    public void AttackUnit()
    {
        AudioManager.instance.ReadAndPlaySFX("menu_click");
        targetAttack.ReceiveDamage();
        CloseAttackResume();
    }
    private void DestroyGameObjectAndChildren(GameObject gameObject)
    {
        gameObject.GetComponentInParent<SquareType>().UnitGroup = null;
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
            if (turn && u.TypePlayer == Territory.TYPEPLAYER.PLAYER)
            {
                ClearUnitGroup(u);
            }
            else if (turn == false && u.TypePlayer != Territory.TYPEPLAYER.PLAYER)
            {
                ClearUnitGroup(u);
            }
        }
    }
    private void ClearUnitGroup(UnitGroup ug)
    {
        ug.Defense = false;
        ug.UnitsGO.transform.GetChild(4).gameObject.SetActive(false);
        ug.UnitsGO.transform.GetChild(2).GetComponent<Image>().color = Color.white;
        ug.Active = true;
    }

    public void ChangeColorTurnSignal()
    {
        turnSignal.GetComponent<AppearAndDissapearAnimation>().ChangeColor();
        turnSignal.GetComponent<AppearAndDissapearAnimation>().Change();
        ClearUnits();
    }
    private void ChangeSide()
    {
        turn = !turn;
        turnSignal.GetComponent<AppearAndDissapearAnimation>().Change();
        //print("side " + turn);
        Invoke("ChangeColorTurnSignal", 1f);
        counterWait = 1;
        if (turn == false)
        {
            canActiveSpecial = false;
            StartCoroutine(WaitingEventInBotTurn());
        }
        else
        {
            canActiveSpecial = true;
            StartCoroutine(WaitingEventInPlayerTurn());
        }
        blockScreen = false;
    }
    IEnumerator WaitingEventInBotTurn()
    {
        yield return new WaitUntil(() => counterWait < 0);
        print("bot turn");
        StartCoroutine(BotTurn());
    }
    IEnumerator WaitingEventInPlayerTurn()
    {
        yield return new WaitUntil(() => counterWait < 0);
        print("player turn");
        StartCoroutine(PlayerTurn());
    }
    private void CheckIsEvent()
    {
        if ( EventManager.instance.listEvents.BattleEvents[currentBattle].EventStatus == CustomEvent.STATUS.FINISH && counterWait >= 0)
        {
            //print("counter| " + counter);
            counterWait--;
        }
    }
    private void CheckIsEvent2()
    {
        if (EventManager.instance.listEvents.BattleEvents[currentBattle].IsAccepted == true && counterWait >= 0)
        {
            counterWait--;
        }
    }
    private IEnumerator PlayerTurn()
    {
        if (canPlayerTurn)
        {
            yield return null;
        }
        else
        {
            canActiveSpecial = false;
            List<UnitGroup> players = new List<UnitGroup>();
            foreach (UnitGroup u in units)
            {
                if (u.TypePlayer == Territory.TYPEPLAYER.PLAYER)
                {
                    players.Add(u);

                }
                //SortList(bots);
            }
            yield return new WaitForSeconds(0.5f);
            foreach (UnitGroup u in players)
            {
                selectedUnit = u;
                CantMovements();
                yield return new WaitForSeconds(1);
            }
            canPlayerTurn = true;
        }
    }
    public IEnumerator BotTurn()
    {
        List<UnitGroup> bots = new List<UnitGroup>();
        foreach (UnitGroup u in units)
        {
            if (u.TypePlayer != Territory.TYPEPLAYER.PLAYER)
            {
                bots.Add(u);

            }
            SortList(bots);
        }
        yield return new WaitForSeconds(0.5f);
        foreach (UnitGroup u in bots)
        {
            selectedUnit = u;
            if (canBotTurn)
            {
                BotMovement(u);
            }
            else
            {
                CantMovements();
            }
            yield return new WaitForSeconds(3);
        }
        canBotTurn = true;
    }
    private void PassTurn()
    {
        canPlayerTurn = false;
        StartCoroutine(PlayerTurn());
    }
    private void MakeMovement(bool isAlive)
    {
        //   print("movimiento terminado: " + selectedUnit.UnitCombat.UnitName);
        ReturnMenu();
        //cambiar la actividad
        if (isAlive)
        {
            selectedUnit.Active = false;
            selectedUnit.UnitsGO.transform.GetChild(2).GetComponent<Image>().color = Color.gray;
        }
        if (CheckFinishTurn())
        {
            StartCoroutine(WaitingToChangeSide());
        }
    }

    IEnumerator WaitingToChangeSide()
    {
//        print("esperar");
        canSurrender = false;
        yield return new WaitForSeconds(1.5f);
        canSurrender = true;
//        print("turns--");
        turns--;

        CheckBattleEvent(turns);

        ChangeSide();
        if (turns <= 0)
        {
            OpenResumeBattle();
        }
    }

    void SetMilitarChief(GameObject _go, MilitarChief _militar, Territory.TYPEPLAYER _type)
    {
        List<Transform> t = Utils.instance.GetAllChildren(_go.transform);
        t[0].GetComponent<Image>().sprite = _militar.Picture;
        t[3].GetComponent<TextMeshProUGUI>().text = _militar.Experience.ToString();
        t[7].GetComponent<TextMeshProUGUI>().text = _militar.CharacterName;
        t[8].GetComponent<TextMeshProUGUI>().text = _militar.StrategyType.ToString().ToLower();
        t[9].GetComponent<TextMeshProUGUI>().text = _type.ToString().ToLower();
        if (_type == Territory.TYPEPLAYER.PLAYER)
        {
            activatSA.onClick.RemoveAllListeners();
            activatSA.onClick.AddListener(() => ActivateSpecialAbility(_militar));
        }
        t[11].GetComponent<TextMeshProUGUI>().text = _militar.AbilityName;
    }
    public void OpenResumeBattle()
    {
        map1.SetActive(true);
        map2.SetActive(true);
        map3.SetActive(true);
       
        //ocultando winner
        ResumeEnemy.transform.GetChild(2).gameObject.SetActive(false);
        ResumePlayer.transform.GetChild(2).gameObject.SetActive(false);
        Block.SetActive(true);
        ResumeBattle.SetActive(true);
        ShowInfo(ResumePlayer, playerTerritory.Territory, playerOriginalTroop, playerActualTroop);
        ShowInfo(ResumeEnemy, enemyTerritory.Territory, enemyOriginalTroop, enemyActualTroop);
        print("Enemy" + enemyTerritory.Territory.TypePlayer.ToString().ToLower());
        print("Player" + playerTerritory.Territory.TypePlayer.ToString().ToLower());
        if (isPlayerDefending)
        {
            if (playerActualTroop.GetAllNumbersUnit() == 0)
            {
                ResumeEnemy.transform.GetChild(2).gameObject.SetActive(true);
                if (playerTerritory.Territory.Population == 0)
                    conquistedTxt.text = "Batalla perdida. Territorio Perdido";
                else
                    conquistedTxt.text = "Batalla perdida.";
            }else if (enemyActualTroop.GetAllNumbersUnit() == 0)
            {
                ResumePlayer.transform.GetChild(2).gameObject.SetActive(true);
                if (enemyTerritory.Territory.Population == 0)
                    conquistedTxt.text = "Batalla ganada. Territorio Defendido";
                else
                    conquistedTxt.text = "Batalla ganada.";
            }
            else
                conquistedTxt.text = "Termino la batalla ";
        }
        else
        {
            if (playerActualTroop.GetAllNumbersUnit() == 0)
            {
                ResumeEnemy.transform.GetChild(2).gameObject.SetActive(true);
                conquistedTxt.text = "Batalla perdida.";
            }
            else if (enemyActualTroop.GetAllNumbersUnit() == 0)
            {
                ResumePlayer.transform.GetChild(2).gameObject.SetActive(true);
                if (enemyTerritory.Territory.Population == 0)
                    conquistedTxt.text = "Batalla ganada. Territorio Conquistado";
                else
                    conquistedTxt.text = "Batalla ganada. ";
            }
            else
                conquistedTxt.text = "Termino la batalla ";
        }
        
        AudioManager.instance.ReadAndPlayMusic("soundtrack",false);
    }
    void ShowInfo(GameObject _go, Territory _territory, Troop _original, Troop _actual)
    {
        List<Transform> t = Utils.instance.GetAllChildren(_go.transform);
        t[0].GetComponent<TextMeshProUGUI>().text = _territory.TypePlayer.ToString();
        string details = "Details \n";
        details += " Lost units: \n";
        for (int j = 0; j < _original.UnitCombats.Count; j++)
        {
            
            UnitGroup _ug = SearchUnit(_territory, _original.UnitCombats[j]);
            UnitCombat new_uc = new UnitCombat();
            if (_ug != null)
            {
                new_uc = _ug.UnitCombat;
            }
            else
            {
                new_uc.CharacterName = _original.UnitCombats[j].CharacterName;
                new_uc.Quantity = 0;
            }
            _actual.AddUnitCombat(new_uc);
        }
        for (int i = 0; i < _original.UnitCombats.Count; i++)
        {
            details += " - " + _original.UnitCombats[i].CharacterName + ":" + (_original.UnitCombats[i].Quantity - _actual.UnitCombats[i].Quantity) + "\n";
        }
        t[1].GetComponent<TextMeshProUGUI>().text = details;
    }
    private UnitGroup SearchUnit(Territory _territory, UnitCombat uc)
    {
        int squares_count = Squares.transform.childCount;
        for (int i = 0; i < squares_count; i++)
        {
            SquareType _square = Squares.transform.GetChild(i).gameObject.GetComponent<SquareType>();
            UnitGroup _ug = _square.UnitGroup;
            if (_ug != null && _ug.TypePlayer == _territory.TypePlayer && _ug.UnitCombat.PositionInBattle == uc.PositionInBattle)
            {
                return _ug;
            }
        }
        return null;
    }

    private bool CheckFinishTurn()
    {
        foreach (UnitGroup u in units)
        {
            if (turn)
            {
                if (u.TypePlayer == Territory.TYPEPLAYER.PLAYER)
                {
                    if (u.Active) 
                    { 
                        return false;
                    }
                }
            }
            else
            {
                if (u.TypePlayer != Territory.TYPEPLAYER.PLAYER)
                {
                    if (u.Active)
                    {

                        return false;
                    }
                }
            }
        }
        return true;
    }
    
    private void SetStats(UnitGroup u)
    {
        string _type = u.UnitCombat.CharacterName;
        if (u.TypePlayer == Territory.TYPEPLAYER.PLAYER)
        {
            if (attackerWin)
                enemyTerritory.Territory.ListUnitCombat.AddUnitCombat(u.UnitCombat);
            else
                playerTerritory.Territory.ListUnitCombat.AddUnitCombat(u.UnitCombat);
        }
        else
        {
            if (attackerWin)
                playerTerritory.Territory.ListUnitCombat.AddUnitCombat(u.UnitCombat);
            else
                enemyTerritory.Territory.ListUnitCombat.AddUnitCombat(u.UnitCombat);
            
        }
    }
    public void FinishCombat()
    {
    //    c = -1;
        turns = 20;
        
        if (isPlayerDefending)
        {
            WarManager.instance.FinishWar(playerTerritory, enemyTerritory,attackerWin);
        }
        else
        {
            WarManager.instance.FinishWar(enemyTerritory, playerTerritory,attackerWin);
        }
        // is player si el territorio atacado es del jugador
        foreach (UnitGroup u in units)
        {
            SetStats(u);
        }

        for (int i = 0; i < squares.transform.childCount; i++)
        {
            if (SquareByIndex(i).HaveUnit)
            {
                SquareByIndex(i).UnitGroup = null;
                DestroyGameObjectAndChildren(squares.transform.GetChild(i).GetChild(0).gameObject);
            }
        }
        if (TutorialController.instance.IsFirstTime)
        {
            TutorialController.instance.TurnOnDialogue();
        }
        DateTableHandler.instance.ResumeTime();
        ResetAllElements();
    }
    private void ResetAllElements()
    {
        units.Clear();
        EventManager.instance.listEvents.ResetAllBattleEvents();
        ResumeBattle.SetActive(false);

        enemyOriginalTroop.Clear();
        playerOriginalTroop.Clear();
        playerActualTroop.Clear();
        enemyActualTroop.Clear();

        Block.SetActive(false);
        canvas.SetActive(false);
        start = 0;
    }
    public void Attack()
    {
        AudioManager.instance.ReadAndPlaySFX("menu_click");
        ShowAttackButtons(GetPosibleAttack(selectedUnit));
        ClearSquares();
    }
    public void Defend()
    {
        AudioManager.instance.ReadAndPlaySFX("menu_click");
        selectedUnit.UnitsGO.transform.GetChild(4).gameObject.SetActive(true);
        selectedUnit.Defense = true;
        MakeMovement(true);
        ClearSquares();
        TurnOffButtons();
    }
    public void CantMovements()
    {
        AnimationInBattle(selectedUnit.UnitsGO, "cantTurn");
        MakeMovement(true);
        ClearSquares();
    }
    public void Move()
    {
        AudioManager.instance.ReadAndPlaySFX("menu_click");
        MakePosibleMovement(CheckPosibleMoves(selectedUnit));
        TurnOffButtons();
    }
    public void MakePosibleMovement(List<int> posibleAttacks)
    {
        foreach (int i in posibleAttacks)
        {
            squares.transform.GetChild(i).GetComponent<Image>().color = Color.magenta;
            squares.transform.GetChild(i).GetComponent<SquareType>().ActivateChange();
        }
    }
    public void BotMovement(UnitGroup bot)
    {
        print("moviento " + bot.UnitCombat.CharacterName);
        List<int> posibleAttacks = GetPosibleAttack(bot);
        if (posibleAttacks.Count < 1)
        {
            List<int> posibleMoves = CheckPosibleMoves(bot);
            if (posibleMoves.Count < 1)
            {
                Defend();
            }
            else
            {
                //print("llego aca");
                //int max = 0;
                int probDefense = Random.Range(0, 100);
                if (probDefense > 50)
                {
                    Defend();
                }
                else
                {
                    int selected = posibleMoves[0];
                    int actualIndex = bot.UnitsGO.transform.parent.GetComponent<SquareType>().Index;
                    ChangeUnits(actualIndex, selected);
                }
            }
        }
        else
        {
            int max = 0;
            int selected = posibleAttacks[0];
            foreach (int index in posibleAttacks)
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
        damage = damage - defenseGroup.UnitCombat.Armor;
        if (damage < 0) damage = 0;
        return damage;
    }
    
    private void CheckTutorial(UnitGroup attackGroup)
    {
        bool x = attackGroup.UnitCombat.CharacterName == "Archer" && attackGroup.TypePlayer == Territory.TYPEPLAYER.PLAYER;
        bool y = attackGroup.UnitCombat.CharacterName == "Swordsman" && attackGroup.TypePlayer == Territory.TYPEPLAYER.PLAYER;
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
    //private IEnumerator AttackAnimation(UnitGroup defenseGroup, GameObject unit, bool crit, bool miss, int damage, bool isPlayer)
    private IEnumerator AttackAnimation(UnitGroup defenseGroup, UnitGroup attackGroup, bool crit, bool miss, int damage, bool isPlayer)
    {
        GameObject attackGO = attackGroup.UnitsGO;
        GameObject deffendGO = defenseGroup.UnitsGO;
        //GameObject unitRot = unitGO.transform.GetChild(2).gameObject;

        GroupClassContainer groupClass = attackGO.transform.GetComponent<GroupClassContainer>();

        SquareType attackerSquare = attackGO.transform.parent.GetComponent<SquareType>();
        SquareType defenderSquare = deffendGO.transform.parent.GetComponent<SquareType>();
        int attackIndex = attackerSquare.j;
        int defenderIndex = defenderSquare.j;
        if (attackIndex >= defenderIndex)
        {
            groupClass.IsLeft();
        }
        else
        {
            groupClass.IsRight();
        }


        groupClass.AttackAnimation();
        if (!defenseGroup.Inmunity)
        {
            defenseGroup.UnitCombat.Quantity = defenseGroup.UnitCombat.Quantity - damage;
            defenseGroup.UnitsGO.transform.GetChild(0).GetComponent<Text>().text = defenseGroup.UnitCombat.Quantity.ToString();
            if (!miss) ShowFloatText("- " + damage + "!", defenseGroup.UnitsGO);
        }
        else
        {
            ShowFloatText("inmunity!", defenseGroup.UnitsGO);
            defenseGroup.Inmunity = false;
        }
        if (crit) ShowFloatText("Critic!", attackGO);
        if (miss) ShowFloatText("Miss!", attackGO);

        yield return new WaitForSeconds(1f);
        groupClass.IdleAnimation();
    }
    public IEnumerator MakeDamage(UnitGroup attackGroup, UnitGroup defenseGroup)
    {
        blockScreen = true;
        ReturnMenu();
        TurnOffButtons();
        //variables iniciales
        bool critictext = false;
        bool misstext = false;

        //tuturial
        CheckTutorial(attackGroup);

        //calculate damage with critic and presicion
        int damage = CheckDamage(attackGroup, defenseGroup);
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
        AudioManager.instance.ReadAndPlaySFX(attackGroup.UnitCombat.PathAudio);
        StartCoroutine(AttackAnimation(defenseGroup, attackGroup, critictext, misstext, damage, true));
        yield return new WaitForSeconds(1);

        critictext = false;
        misstext = false;

        //response attack
        if (defenseGroup.UnitCombat.Range >= attackGroup.UnitCombat.Range && defenseGroup.UnitCombat.Quantity > 0)
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

            AudioManager.instance.ReadAndPlaySFX(defenseGroup.UnitCombat.PathAudio);
            StartCoroutine(AttackAnimation(attackGroup, defenseGroup, critictext, misstext, damage, false));
            yield return new WaitForSeconds(1);
        }

        bool alive = true;
        if (defenseGroup.UnitCombat.Quantity <= 0)
        {
            //defenseGroup.Quantity = 0;
            defenseGroup.UnitCombat.Quantity = 0;
            defenseGroup.UnitsGO.transform.GetChild(0).GetComponent<Text>().text = "0";
            defenseGroup.UnitsGO.transform.GetChild(0).GetComponent<Text>().color = Color.red;
           // SetStats(defenseGroup);
            units.Remove(defenseGroup);
            defenseGroup.UnitsGO.GetComponentInParent<SquareType>().UnitGroup = null;
            defenseGroup.UnitsGO.GetComponentInParent<SquareType>().HaveUnit = false;
            DestroyGameObjectAndChildren(defenseGroup.UnitsGO);
            print("mori |" + blockScreen);
        }
        if (attackGroup.UnitCombat.Quantity <= 0)
        {
            alive = false;
            attackGroup.UnitCombat.Quantity = 0;
            attackGroup.UnitsGO.transform.GetChild(0).GetComponent<Text>().text = "0";
            attackGroup.UnitsGO.transform.GetChild(0).GetComponent<Text>().color = Color.red;
            units.Remove(attackGroup);
            attackGroup.UnitsGO.GetComponentInParent<SquareType>().UnitGroup = null;
            attackGroup.UnitsGO.GetComponentInParent<SquareType>().HaveUnit = false;
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
        blockScreen = false;
        yield return new WaitForSeconds(0.5f);
    }
    public void UpdateUnitGroup(UnitGroup unitGroup)
    {
        if (unitGroup.UnitCombat.Quantity <= 0)
        {
            //defenseGroup.Quantity = 0;
            unitGroup.UnitCombat.Quantity = 0;
            unitGroup.UnitsGO.transform.GetChild(0).GetComponent<Text>().text = "0";
            unitGroup.UnitsGO.transform.GetChild(0).GetComponent<Text>().color = Color.red;
          //  SetStats(unitGroup);
            units.Remove(unitGroup);
            unitGroup.UnitsGO.GetComponentInParent<SquareType>().UnitGroup = null;
            unitGroup.UnitsGO.GetComponentInParent<SquareType>().HaveUnit = false;
            DestroyGameObjectAndChildren(unitGroup.UnitsGO);
        }
    }
    public void ShowFloatTextButton(string text)
    {
        InGameMenuHandler.instance.ShowFloatingText(text, "TextFloating", activatSA.transform, Color.black);
    }
    public void ShowFloatTextMC(string text)
    {
        InGameMenuHandler.instance.ShowFloatingText(text, "TextFloating", militarchiefPlayer.transform, Color.black);
    }
    public void ShowFloatText(string text, GameObject go)
    {
        InGameMenuHandler.instance.ShowFloatingText(text, "TextFloating", go.transform, Color.black);
    }
    public void AnimationInBattle(GameObject go, string name)
    {
        GameObject prefab = Resources.Load("Prefabs/BattlePrefabs/" + name) as GameObject;
        GameObject _go = Instantiate(prefab, FindObjectOfType<Canvas>().transform);
        _go.transform.SetParent(go.transform.transform);
        _go.transform.position = Vector3.zero;
        Destroy(_go, 1);
    }

    private void TurnOffButtons()
    {
        foreach (UnitGroup u in units)
        {
            //if(u.TypePlayer != Territory.TYPEPLAYER.PLAYER)

            u.UnitsGO.transform.GetChild(3).gameObject.SetActive(false);

        }
    }
    public bool EndBattle()
    {
        Territory.TYPEPLAYER check = units[0].TypePlayer;
        foreach (UnitGroup u in units)
        {
            if (u.TypePlayer != check)
            {
                return false;
            }
        }
        return true;
    }
    private List<int> GetPosibleAttack(UnitGroup attacker)
    {
        List<int> posibleAttack = new List<int>();

        //INDICE DEL ATACANTE
        SquareType attackerSquare = attacker.UnitsGO.transform.parent.GetComponent<SquareType>();
        int attackI = attackerSquare.i;
        int attackJ = attackerSquare.j;
        int range = attacker.UnitCombat.Range;
        int newIndexI = 0;
        int newIndexJ = 0;

        for (int i = 1; i <= range; i++)
        {
            //TOP
            newIndexI = attackI - i;
            if (newIndexI >= 0 && newIndexI < 3)
            {
                if(squaresGrid[newIndexI, attackJ].UnitGroup != null)
                {
                    if (squaresGrid[newIndexI, attackJ].UnitGroup.TypePlayer != attacker.TypePlayer)
                    {
                        posibleAttack.Add(squaresGrid[newIndexI, attackJ].Index);

                    }
                }
                
            }
            //DOWN
            newIndexI = attackI + i;
            if (newIndexI >= 0 && newIndexI < 3)
            {
                if (squaresGrid[newIndexI, attackJ].UnitGroup != null)
                {
                    if (squaresGrid[newIndexI, attackJ].UnitGroup.TypePlayer != attacker.TypePlayer)
                    {
                        posibleAttack.Add(squaresGrid[newIndexI, attackJ].Index);

                    }
                }

            }
            //LEFT
            newIndexJ = attackJ - i;
            if (newIndexJ >= 0 && newIndexJ < 8)
            {
                if (squaresGrid[attackI, newIndexJ].UnitGroup != null)
                {
                    if (squaresGrid[attackI, newIndexJ].UnitGroup.TypePlayer != attacker.TypePlayer)
                    {
                        posibleAttack.Add(squaresGrid[attackI, newIndexJ].Index);

                    }
                }

            }
            //RIGHT
            newIndexJ = attackJ + i;
            if (newIndexJ >= 0 && newIndexJ < 8)
            {
                if (squaresGrid[attackI, newIndexJ].UnitGroup != null)
                {
                    if (squaresGrid[attackI, newIndexJ].UnitGroup.TypePlayer != attacker.TypePlayer)
                    {
                        posibleAttack.Add(squaresGrid[attackI, newIndexJ].Index);

                    }
                }

            }

            //topleft
            newIndexJ = attackJ - i;
            newIndexI = attackI - i;
            if (newIndexJ >= 0 && newIndexJ < 8 && newIndexI >= 0 && newIndexI < 3)
            {
                if (squaresGrid[newIndexI, newIndexJ].UnitGroup != null)
                {
                    if (squaresGrid[newIndexI, newIndexJ].UnitGroup.TypePlayer != attacker.TypePlayer)
                    {
                        posibleAttack.Add(squaresGrid[newIndexI, newIndexJ].Index);

                    }
                }

            }
            //downright
            newIndexJ = attackJ + i;
            newIndexI = attackI + i;
            if (newIndexJ >= 0 && newIndexJ < 8 && newIndexI >= 0 && newIndexI < 3)
            {
                if (squaresGrid[newIndexI, newIndexJ].UnitGroup != null)
                {
                    if (squaresGrid[newIndexI, newIndexJ].UnitGroup.TypePlayer != attacker.TypePlayer)
                    {
                        posibleAttack.Add(squaresGrid[newIndexI, newIndexJ].Index);

                    }
                }

            }
            //topright
            newIndexJ = attackJ + i;
            newIndexI = attackI - i;
            if (newIndexJ >= 0 && newIndexJ < 8 && newIndexI >= 0 && newIndexI < 3)
            {
                if (squaresGrid[newIndexI, newIndexJ].UnitGroup != null)
                {
                    if (squaresGrid[newIndexI, newIndexJ].UnitGroup.TypePlayer != attacker.TypePlayer)
                    {
                        posibleAttack.Add(squaresGrid[newIndexI, newIndexJ].Index);

                    }
                }

            }
            //downleft
            newIndexJ = attackJ - i;
            newIndexI = attackI + i;
            if (newIndexJ >= 0 && newIndexJ < 8 && newIndexI >= 0 && newIndexI < 3)
            {
                if (squaresGrid[newIndexI, newIndexJ].UnitGroup != null)
                {
                    if (squaresGrid[newIndexI, newIndexJ].UnitGroup.TypePlayer != attacker.TypePlayer)
                    {
                        posibleAttack.Add(squaresGrid[newIndexI, newIndexJ].Index);

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
        return squares.transform.GetChild(index).GetComponent<SquareType>().UnitGroup;
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

        SquareType unitSquare = unit.UnitsGO.transform.parent.GetComponent<SquareType>();
     
        int attackI = unitSquare.i;
        int attackJ = unitSquare.j;
        int newIndexI = 0;
        int newIndexJ = 0;
        List<int> posibleMoves = new List<int>();

        //TOP
        newIndexI = attackI - 1;
        if (newIndexI >= 0 && newIndexI <= 2)
        {
            if (squaresGrid[newIndexI, attackJ].HaveUnit == false || squaresGrid[newIndexI, attackJ].UnitGroup.TypePlayer == unit.TypePlayer)
            {
                if (squaresGrid[newIndexI, attackJ].Terrain.Type != Terrain.TYPE.NONE)
                {
                    posibleMoves.Add(squaresGrid[newIndexI, attackJ].Index);

                }
            }

        }

        //DOWN
        newIndexI = attackI + 1;
        if (newIndexI >= 0 && newIndexI <= 2)
        {
            print(newIndexI);
            print(newIndexI);
            if (squaresGrid[newIndexI, attackJ].HaveUnit == false || squaresGrid[newIndexI, attackJ].UnitGroup.TypePlayer == unit.TypePlayer)
            {
                if (squaresGrid[newIndexI, attackJ].Terrain.Type != Terrain.TYPE.NONE)
                {
                    posibleMoves.Add(squaresGrid[newIndexI, attackJ].Index);

                }
            }

        }

        //LEFT
        newIndexJ = attackJ - 1;
        if (newIndexJ >= 0 && newIndexJ <= 7)
        {
            if (squaresGrid[attackI, newIndexJ].HaveUnit == false || squaresGrid[attackI, newIndexJ].UnitGroup.TypePlayer == unit.TypePlayer)
            {
                if (squaresGrid[attackI, newIndexJ].Terrain.Type != Terrain.TYPE.NONE)
                {
                    posibleMoves.Add(squaresGrid[attackI, newIndexJ].Index);

                }
            }

        }

        //RIGHT
        newIndexJ = attackJ + 1;
        if (newIndexJ >= 0 && newIndexJ <= 7)
        {
            if (squaresGrid[attackI, newIndexJ].HaveUnit == false || squaresGrid[attackI, newIndexJ].UnitGroup.TypePlayer == unit.TypePlayer)
            {
                if (squaresGrid[attackI, newIndexJ].Terrain.Type != Terrain.TYPE.NONE)
                {
                    posibleMoves.Add(squaresGrid[attackI, newIndexJ].Index);

                }
            }

        }

        return posibleMoves;

    }

    public int ActualUnitIndex()
    {
        return selectedUnit.UnitsGO.transform.parent.GetComponent<SquareType>().Index;
    }
    public void ChangeUnits(int index, int newIndex)
    {

        SquareType square1 = SquareByIndex(index);
        SquareType square2 = SquareByIndex(newIndex);
        UnitGroup saveug = square1.UnitGroup;

        //print(saveug);
        if (square1.HaveUnit)
        {
            square1.gameObject.transform.GetChild(0).transform.position = square2.gameObject.transform.position;
            square1.gameObject.transform.GetChild(0).transform.SetParent(square2.transform);

        }
        square1.UnitGroup = square2.UnitGroup;
        if (square2.HaveUnit)
        {
            square2.gameObject.transform.GetChild(0).transform.position = square1.gameObject.transform.position;
            square2.gameObject.transform.GetChild(0).transform.SetParent(square1.transform);

        }
        square2.UnitGroup = saveug;
        /*
        for (int j = 0; j < squares.transform.childCount; j++)
        {
            squares.transform.GetChild(j).GetComponent<SquareType>().UpdateSquare();
        }
        */
        UpdateBuffTerrain();
        ClearSquares();
        MakeMovement(true);
    }

    public void ClearSquares()
    {
        for (int i = 0; i < squares.transform.childCount; i++)
        {
            squares.transform.GetChild(i).GetComponent<Image>().color = new Color32(168, 161, 161, 139);
            squares.transform.GetChild(i).GetComponent<SquareType>().DeactivateChange();
        }
    }

    public UnitGroup ActualUnit()
    {
        return selectedUnit;
    }


}
