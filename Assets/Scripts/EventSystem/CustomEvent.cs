using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CustomEvent
{
    
    [SerializeField] protected int costEvent;

    [SerializeField] protected string nameEvent;
    [SerializeField] protected string btn1;
    [SerializeField] protected string btn2;
    [SerializeField] protected string btn3;

    [SerializeField] protected string message;
    [SerializeField] protected string acceptMessageEvent;
    [SerializeField] protected string declineMessageEvent;
    [SerializeField] protected string thirdMessageEvent;

    [SerializeField] protected string requirementMessageEvent;
    [SerializeField] protected string resultMessageEvent;

    [SerializeField] protected bool isBattle;
    [SerializeField] protected bool w = false;
    [SerializeField] protected TerritoryHandler territoryEvent;
    [SerializeField] protected TimeSimulated timeInit;
    [SerializeField] protected TimeSimulated timeFinal;
    [SerializeField] protected STATUS eventStatus;
    [SerializeField] protected EVENTTYPE eventType;
    [SerializeField] protected bool isAcceptedEvent;

    public bool IsBattle
    {
        get { return isBattle; }
    }
    public bool W
    {
        get { return w; }
        set { w = value; }
    }
    public STATUS EventStatus {
        get { return eventStatus; }
        set { eventStatus = value; }
    }
    public bool IsAccepted
    {
        get { return isAcceptedEvent; }
        set { isAcceptedEvent = value; }
    }
    public int DifferenceToFinal
    {
        get {return timeFinal.DiferenceDays(timeInit); }
    }
    public EVENTTYPE EventType
    {
        get { return eventType; }
        set { eventType = value; }
    }
    public string NameEvent
    {
        get { return nameEvent; }
        set { nameEvent = value; }
    }
    public string MessageEvent
    {
        get { return message; }
        set { message = value; }
    }
    public TerritoryHandler TerritoryEvent
    {
        get { return territoryEvent; }
        set { territoryEvent = value; }
    }
    public string AcceptMessageEvent
    {
        get { return acceptMessageEvent; }
        set { acceptMessageEvent = value; }
    }
    public string DeclineMessageEvent
    {
        get { return declineMessageEvent; }
        set { declineMessageEvent = value; }
    }
    public string ResultMessageEvent
    {
        get { return resultMessageEvent; }
        set { resultMessageEvent = value; }
    }
    public string Button1
    {
        get { return btn1; }
        set { btn1 = value; }
    }
    public string Button2
    {
        get { return btn2; }
        set { btn2 = value; }
    }
    public string Button3
    {
        get { return btn3; }
        set { btn3 = value; }
    }
    public string RequirementMessageEvent
    {
        get { return requirementMessageEvent; }
        set { requirementMessageEvent = value; }
    }
    public TimeSimulated TimeInitEvent
    {
        get { return timeInit; }
        set { timeInit = value; }
    }
    public TimeSimulated TimeFinalEvent
    {
        get { return timeFinal; }
        set { timeFinal = value; }
    }
    public void PrintEvent(int i)
    {
        Debug.Log(i + " eventType - " + eventType.ToString());
        Debug.Log(i + " territorio- " + TerritoryEvent.name);
        Debug.Log(i + " init time- " + TimeInitEvent.PrintTimeSimulated());
        Debug.Log(i + " final time- " + TimeFinalEvent.PrintTimeSimulated());
    }
    public CustomEvent()
    {

    }

    public CustomEvent(TimeSimulated _initTime, TerritoryHandler territory)
    {
        this.isBattle = false;
        this.isAcceptedEvent = false;
        if (territory != null)
        {
            this.eventType = EVENTTYPE.CONQUIST;
            this.territoryEvent = territory;
        }
        else
        {
            this.eventType = (EVENTTYPE)UnityEngine.Random.Range(0,11);
            //this.eventType = EVENTTYPE.EARTHQUAKER;
            //this.eventType = (EVENTTYPE)UnityEngine.Random.Range(0, Enum.GetNames(typeof(EVENTTYPE)).Length - 6);
            this.territoryEvent = TerritoryManager.instance.GetTerritoryRandom(Territory.TYPEPLAYER.PLAYER);
        }

        this.eventStatus = STATUS.ANNOUNCE;
        this.costEvent = UnityEngine.Random.Range(1, InGameMenuHandler.instance.GoldPlayer / 2);
        GetTimesEvents(_initTime);
        GetMessage();
        GetRequirement();
    }
    protected void GetTimesEvents(TimeSimulated _initTime)
    {
        this.timeInit = new TimeSimulated(_initTime);
        int rDays1 = UnityEngine.Random.Range(EventManager.instance.MinDays, EventManager.instance.MaxDays);
        timeInit.PlusDays(rDays1);

        if (this.eventType == EVENTTYPE.EARTHQUAKER)
        {
            this.timeFinal = new TimeSimulated(timeInit);
            timeFinal.PlusDays(1);
        }else if (this.eventType == EVENTTYPE.CONQUIST)
        {
            this.timeFinal = new TimeSimulated(timeInit);
            timeFinal.PlusDays(1000);
            this.eventStatus = STATUS.PROGRESS;
        }
        else
        {
            this.timeFinal = new TimeSimulated(timeInit);
            int rDays2 = UnityEngine.Random.Range(10, 15);
            timeFinal.PlusDays(rDays2);
        }
    }
    /// <summary>
    /// Change messages of the custom event
    /// </summary>
    protected void GetMessage()
    {
        this.nameEvent = GameMultiLang.GetTraductionEvents(eventType.ToString() + "_NAME");
        this.btn1 = GameMultiLang.GetTraductionEvents(eventType.ToString() + "1");
        this.btn2 = GameMultiLang.GetTraductionEvents(eventType.ToString() + "2");
        this.btn3 = GameMultiLang.GetTraductionEvents(eventType.ToString() + "3");
        this.message = GameMultiLang.GetTraductionEvents(eventType.ToString() + "M").Replace("TERRITORYEVENT", TerritoryEvent.name);
        this.acceptMessageEvent = GameMultiLang.GetTraductionEvents(eventType.ToString() + "A").Replace("TERRITORYEVENT", TerritoryEvent.name);
        this.declineMessageEvent = GameMultiLang.GetTraductionEvents(eventType.ToString() + "D").Replace("TERRITORYEVENT", TerritoryEvent.name);
        this.thirdMessageEvent = GameMultiLang.GetTraductionEvents(eventType.ToString() + "T").Replace("TERRITORYEVENT", TerritoryEvent.name);
    }
    protected void GetRequirement()
    {
        switch (eventType)
        {
            case EVENTTYPE.EARTHQUAKER:
                this.requirementMessageEvent = "";
                break;
            case EVENTTYPE.DROUGHT:
            case EVENTTYPE.ALL_T_PANDEMIC:
            case EVENTTYPE.PANDEMIC:
            case EVENTTYPE.ALL_T_PLAGUE:
            case EVENTTYPE.PLAGUE:
                this.requirementMessageEvent = GameMultiLang.GetTraduction("Requirements") + "\n" + GameMultiLang.GetTraduction("REQ1").Replace("COSTEVENT", costEvent.ToString());
                break;
            case EVENTTYPE.PETITION_MIN:
            case EVENTTYPE.PETITION_FOR:
            case EVENTTYPE.REBELION:
                this.requirementMessageEvent = GameMultiLang.GetTraduction("Requirements") + "\n" + GameMultiLang.GetTraduction("REQ2").Replace("COSTEVENT", costEvent.ToString());
                break;
           // case EVENTTYPE.GRACE_DIV:
            case EVENTTYPE.GRACE_MIN:
            case EVENTTYPE.GRACE_FOOD:
                this.requirementMessageEvent = GameMultiLang.GetTraduction("Requirements") + "\n" + GameMultiLang.GetTraduction("REQ3").Replace("COSTEVENT", costEvent.ToString());
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Retuns if is posible to accept the custom event
    /// </summary>
    /// <returns></returns>
    public bool GetAcceptButton()
    {
        bool accept = false;
        switch (eventType)
        {
            case EVENTTYPE.DROUGHT:
            case EVENTTYPE.ALL_T_PANDEMIC:
            case EVENTTYPE.PANDEMIC:
            case EVENTTYPE.ALL_T_PLAGUE:
            case EVENTTYPE.PLAGUE:
                if (InGameMenuHandler.instance.GoldPlayer >= costEvent && InGameMenuHandler.instance.FoodPlayer >= costEvent)
                {
                    accept = true;
                }
                break;
            case EVENTTYPE.REBELION:
            case EVENTTYPE.PETITION_MIN:
            case EVENTTYPE.PETITION_FOR:
         //   case EVENTTYPE.GRACE_DIV:
            case EVENTTYPE.GRACE_MIN:
            case EVENTTYPE.GRACE_FOOD:
                if (InGameMenuHandler.instance.GoldPlayer >= costEvent)
                {
                    accept = true;
                }
                break;
            case EVENTTYPE.EXCURSION:
            case EVENTTYPE.INTOXICATE:
            case EVENTTYPE.WOOKIE:
            case EVENTTYPE.REINFORCEMENT:
            case EVENTTYPE.CURE:
            case EVENTTYPE.ABILITYX2:
            case EVENTTYPE.PILLAGE:
            case EVENTTYPE.BURN:
                accept = true;
                break;
            default:
                break;
        }
        //Debug.LogError("canAccept|" + accept);
        return accept;
    }
    /// <summary>
    /// Accept Action event // first action of event
    /// </summary>
    public virtual void AcceptEventAction()
    {
        this.eventStatus = STATUS.FINISH;
        this.isAcceptedEvent = true;
        switch (eventType)
        {
            case EVENTTYPE.REBELION:
                InGameMenuHandler.instance.GoldPlayer -= costEvent;
                break;
            case EVENTTYPE.DROUGHT:
            case EVENTTYPE.ALL_T_PANDEMIC:
            case EVENTTYPE.PANDEMIC:
            case EVENTTYPE.ALL_T_PLAGUE:
            case EVENTTYPE.PLAGUE:
                InGameMenuHandler.instance.GoldPlayer -= costEvent;
                InGameMenuHandler.instance.FoodPlayer -= costEvent;
                break;
            case EVENTTYPE.PETITION_MIN:
                InGameMenuHandler.instance.GoldPlayer -= costEvent;
                TerritoryEvent.TerritoryStats.Territory.GoldMineTerritory.ImproveManyLevels(2, territoryEvent.TerritoryStats.Territory);
                break;
            case EVENTTYPE.PETITION_FOR:
                InGameMenuHandler.instance.GoldPlayer -= costEvent;
                TerritoryEvent.TerritoryStats.Territory.FortressTerritory.ImproveManyLevels(1, territoryEvent.TerritoryStats.Territory);
                break;
            case EVENTTYPE.GRACE_MIN:
                InGameMenuHandler.instance.GoldPlayer -= costEvent;
                TerritoryEvent.TerritoryStats.Territory.GoldMineTerritory.ImproveManyLevels(3, territoryEvent.TerritoryStats.Territory);
                break;
            case EVENTTYPE.GRACE_FOOD:
                InGameMenuHandler.instance.GoldPlayer -= costEvent;
                TerritoryEvent.TerritoryStats.Territory.FarmTerritory.ImproveManyLevels(3, territoryEvent.TerritoryStats.Territory);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Decline Action event// second action of event
    /// </summary>
    public virtual void DeclineEventAction()
    {
        this.eventStatus= STATUS.FINISH;
        this.isAcceptedEvent = false;
        switch (eventType)
        {
            case EVENTTYPE.EARTHQUAKER:
                TerritoryEvent.TerritoryStats.Territory.ResetAllBuilds();
                break;
            case EVENTTYPE.REBELION:
                TerritoryEvent.TerritoryStats.Territory.TypePlayer = Territory.TYPEPLAYER.NONE;
                //    Debug.LogError("perdimos el territorio");
                break;
            case EVENTTYPE.DROUGHT:
                TerritoryEvent.TerritoryStats.Territory.FarmTerritory.ResetBuilding(territoryEvent.TerritoryStats.Territory);
                TerritoryEvent.TerritoryStats.Territory.ListUnitCombat.ReduceAllQuantity();

                break;
            case EVENTTYPE.ALL_T_PANDEMIC:
                List<TerritoryHandler> list = TerritoryManager.instance.GetTerritoriesHandlerByTypePlayer(Territory.TYPEPLAYER.PLAYER);
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].TerritoryStats.Territory.ListUnitCombat.ReduceAllQuantity();
                }
                break;
            case EVENTTYPE.PANDEMIC:
                TerritoryEvent.TerritoryStats.Territory.ListUnitCombat.ReduceAllQuantity();
                break;
            case EVENTTYPE.ALL_T_PLAGUE:
                List<TerritoryHandler> list2 = TerritoryManager.instance.GetTerritoriesHandlerByTypePlayer(Territory.TYPEPLAYER.PLAYER);
                for (int i = 0; i < list2.Count; i++)
                {
                    list2[i].TerritoryStats.Territory.FarmTerritory.ResetBuilding(territoryEvent.TerritoryStats.Territory);
                }
                break;
            case EVENTTYPE.PLAGUE:
                TerritoryEvent.TerritoryStats.Territory.FarmTerritory.ResetBuilding(territoryEvent.TerritoryStats.Territory);
                break;
            case EVENTTYPE.PETITION_MIN:
                break;
            case EVENTTYPE.PETITION_FOR:
                break;
            case EVENTTYPE.GRACE_MIN:
                InGameMenuHandler.instance.GoldPlayer += 15;
                break;
            case EVENTTYPE.GRACE_FOOD:
                InGameMenuHandler.instance.FoodPlayer += 15;
                break;
            default:
                break;
        }
        if(InGameMenuHandler.instance.GoldPlayer< 0)
        {
            InGameMenuHandler.instance.GoldPlayer = 0;
        }
        InGameMenuHandler.instance.UpdateMenu();
    }
    /// <summary>
    /// Decline Action event// third action of event
    /// </summary>
    public virtual void AdicitionalEventAction()
    {
        //this.eventStatus = STATUS.FINISH;
        //this.isAcceptedEvent = false;
        switch (eventType)
        {
            case EVENTTYPE.EARTHQUAKER:
            case EVENTTYPE.REBELION:
            case EVENTTYPE.DROUGHT:
            case EVENTTYPE.ALL_T_PANDEMIC:
            case EVENTTYPE.PANDEMIC:
            case EVENTTYPE.ALL_T_PLAGUE:
            case EVENTTYPE.PLAGUE:
            case EVENTTYPE.PETITION_MIN:
            case EVENTTYPE.PETITION_FOR:
            case EVENTTYPE.GRACE_MIN:
            case EVENTTYPE.GRACE_FOOD:
                break;
            default:
                break;
        }
        if (InGameMenuHandler.instance.GoldPlayer < 0)
        {
            InGameMenuHandler.instance.GoldPlayer = 0;
        }

    }
    public virtual void CloseEventAction()
    {

    }
    /// <summary>
    /// Returns the message if is accepted of decline
    /// </summary>
    /// <returns></returns>
    public string ResultsEvent()
    {
        if (isAcceptedEvent)
        {
            this.resultMessageEvent = GameMultiLang.GetTraduction("CompleteResults").Replace("TERRITORYEVENT", TerritoryEvent.name);
            return this.acceptMessageEvent;
        }
        else {
            this.resultMessageEvent = GameMultiLang.GetTraduction("IncompleteResults").Replace("TERRITORYEVENT", TerritoryEvent.name);
            if (eventType == EVENTTYPE.EARTHQUAKER)
            {
                this.resultMessageEvent = GameMultiLang.GetTraduction("EarthquarkeResults").Replace("TERRITORYEVENT", TerritoryEvent.name);
            }
            if (eventType == EVENTTYPE.WASTE)
            {
                this.resultMessageEvent = GameMultiLang.GetTraduction("ExplorationResults").Replace("TERRITORYEVENT", TerritoryEvent.name);
            }
            return this.declineMessageEvent;
        }
    }
    public enum EVENTTYPE
    {
        EARTHQUAKER,
        REBELION,
        DROUGHT,
        ALL_T_PANDEMIC,
        PANDEMIC,
        ALL_T_PLAGUE,
        PLAGUE,
        PETITION_MIN,
        PETITION_FOR,
        GRACE_MIN,
        GRACE_FOOD,
        //CONQUIST TERRITORY
        CONQUIST,
        //EXPLORATION WASTE
        WASTE,
        //CUSTOM BATTLE
        //INIT
        EXCURSION,
        INTOXICATE,
        WOOKIE,
        //MIDDLE
        REINFORCEMENT,
        CURE,
        ABILITYX2,
        //FINAL
        PILLAGE,
        BURN,
    }
    public enum STATUS
    {
        ANNOUNCE, // creado
        PROGRESS, // entre init time y finish time
        FINISH //finishTime
    }
}
[Serializable]
public class CustomBuilding : CustomEvent
{
    [SerializeField] private Building building;
    public Building BuildingEvent
    {
        get { return building; }
        set { building = value; }
    }
    public CustomBuilding(TimeSimulated _initTime, TerritoryHandler territory, Building _building)
    {
        this.territoryEvent = territory;
        building = territoryEvent.TerritoryStats.Territory.GetBuilding(_building);
        building.TimeInit = new TimeSimulated(_initTime);
        timeInit = new TimeSimulated(_initTime);
    }
    public void FinishUpgradeBuilding(int levels)
    {
        building.CanUpdrade = true;
        building.DaysTotal = 0;
        building.ImproveManyLevels(levels,this.territoryEvent.TerritoryStats.Territory);
        building.ImproveCostUpgrade(levels);
    }
}


[Serializable]
public class CustomUnitCombat : CustomEvent
{
    [SerializeField] private UnitCombat unitCombat;
    public UnitCombat UnitCombatEvent
    {
        get { return unitCombat; }
        set { unitCombat = value; }
    }
    public CustomUnitCombat(TimeSimulated _initTime, TerritoryHandler territory, UnitCombat _unitCombat)
    {
        this.territoryEvent = territory;
        this.unitCombat = _unitCombat;
        _unitCombat.TimeInit = new TimeSimulated(_initTime);
        _unitCombat.DaysToCreate = _unitCombat.Quantity/20;
        timeInit = new TimeSimulated(_initTime);
    }
    public void FinishAddingUnit()
    {
        unitCombat.IsAvailable = true;
        //unitCombat.InProgress = unitCombat.Quantity;
        //building.CanUpdrade = true;
        //building.DaysTotal = 0;
        //building.ImproveManyLevels(levels, this.territoryEvent.TerritoryStats.Territory);
        //building.ImproveCostUpgrade(levels);
    }
}

[Serializable]
public class CustomExpedition : CustomEvent
{
    [SerializeField] private Troop troop;
    [SerializeField] private TerritoryHandler attacker;
    public Troop TroopEvent
    {
        get { return troop; }
        set { troop = value; }
    }
    public CustomExpedition(TimeSimulated _initTime, Troop troopToWaste, TerritoryHandler attackerterritory, TerritoryHandler wasterterritory)
    {
        this.troop = troopToWaste;
        this.isAcceptedEvent = false;
        this.attacker = attackerterritory;
        this.territoryEvent = wasterterritory;
        this.eventType = EVENTTYPE.WASTE;
        this.eventStatus = STATUS.PROGRESS;
        this.requirementMessageEvent = " ";
      //  this.costEvent = UnityEngine.Random.Range(1, InGameMenuHandler.instance.GoldPlayer / 2);
        GetTimesExpeditionEvents(_initTime);
        GetMessage();
        this.MessageEvent = this.MessageEvent.Replace("NUMBER", troop.GetAllNumbersUnit().ToString());
    }
    void GetTimesExpeditionEvents(TimeSimulated _initTime)
    {
        this.timeInit = new TimeSimulated(_initTime);
        this.timeFinal = new TimeSimulated(timeInit);
        int rDays2 = UnityEngine.Random.Range(10, 15);
        timeFinal.PlusDays(rDays2);
    }
    public void ReturnUnits()
    {
        WarManager.instance.SendWarriors(territoryEvent, attacker, troop);
    }
}
[Serializable]
public class CustomBattle : CustomEvent
{
    [SerializeField] int turn;
    List<UnitGroup> ug = new List<UnitGroup>();
    [SerializeField] private Troop troop = new Troop();
    Territory.TYPEPLAYER typeEvent;
    bool miss = false;
    int index_to_mod;
    int units_to_mod;
    int mod;
    int costgold;
    int costfood;
    string building_to_mod;
    public int TurnEvent
    {
        get { return turn; }
    }
    public CustomBattle(string _type, Troop _troopPlayer, Troop _troopNoPlayer, TerritoryHandler _territoryPlayer, TerritoryHandler _territoryNoPlayer)
    {
        this.eventStatus = STATUS.ANNOUNCE;
        this.isBattle = true;
        GetTypeBattle(_type);
        if ( eventType != EVENTTYPE.INTOXICATE)
        {
            this.troop.SaveTroop(_troopPlayer);
            this.typeEvent = _territoryPlayer.TerritoryStats.Territory.TypePlayer;
            this.territoryEvent = _territoryPlayer;
        }
        else
        {
            this.troop.SaveTroop(_troopNoPlayer);
            this.typeEvent = _territoryNoPlayer.TerritoryStats.Territory.TypePlayer;
            this.territoryEvent = _territoryNoPlayer;
        }
        GetMessage();
        GetRequirement();
    }

    public CustomBattle(int _a,int _turn, Troop _troopPlayer, Troop _troopNoPlayer, TerritoryHandler _territoryPlayer, TerritoryHandler _territoryNoPlayer)
    {
        isAcceptedEvent = false;
        this.eventStatus = STATUS.ANNOUNCE;
        this.isBattle = true;
        this.eventType = (EVENTTYPE)_a;
        this.turn = _turn;
        if (eventType != EVENTTYPE.INTOXICATE)
        {
            this.troop.SaveTroop(_troopPlayer);
            this.typeEvent = _territoryPlayer.TerritoryStats.Territory.TypePlayer;
            this.territoryEvent = _territoryPlayer;
        }
        else
        {
            this.troop.SaveTroop(_troopNoPlayer);
            this.typeEvent = _territoryNoPlayer.TerritoryStats.Territory.TypePlayer;
            this.territoryEvent = _territoryNoPlayer;
        }
        GetMessage();
        GetRequirement();
    }

    private void GetTypeBattle(string battleType)
    {
        int turns = CombatManager.instance.Turns;
        switch (battleType)
        {
            case "INIT":
                this.turn = 20;
                this.eventType = (EVENTTYPE)UnityEngine.Random.Range(13, 16);
                break;
            case "MIDDLE":
                this.turn = turns/2;
                this.eventType = (EVENTTYPE)UnityEngine.Random.Range(16, 19);
                break;
            case "FINAL":
                this.turn = 2;
                this.eventType = (EVENTTYPE)UnityEngine.Random.Range(19, 21);
                break;
            default:
                break;
        }
    }

    private void Cure()
    {
        int squares_count = CombatManager.instance.Squares.transform.childCount;
        int max_units_count = troop.UnitCombats.Count-1;
        int random = UnityEngine.Random.Range(0, max_units_count);
        UnitCombat uc = troop.UnitCombats[random];
        // int _pos = troop.Positions[random];
        int _pos = troop.UnitCombats[random].PositionInBattle;
        for (int i = 0; i < squares_count; i++)
        {
            SquareType _square = CombatManager.instance.Squares.transform.GetChild(i).gameObject.GetComponent<SquareType>();
            UnitGroup _ug = _square.UnitGroup;
            if (_ug != null && _ug.TypePlayer == this.typeEvent && _ug.UnitCombat.PositionInBattle == _pos)
            {
                ug.Add(_ug);
                break;
            }
        }
        int _original_quantity = uc.Quantity;
        int _actual_quantity = ug[0].UnitCombat.Quantity;
        units_to_mod = (_original_quantity - _actual_quantity);
    }
    private void GetUnitGroup()
    {
        for (int i = 0; i < 8; i++)
        {
            SquareType _square = CombatManager.instance.Squares.transform.GetChild(i).gameObject.GetComponent<SquareType>();
            UnitGroup _ug = _square.UnitGroup;
            if (_ug != null && _ug.TypePlayer == territoryEvent.TerritoryStats.Territory.TypePlayer)
            {
                ug.Add(_ug);
            }
        }
    }
    private void GetFirstUnitGroup()
    {
        for (int i = 0; i < 8; i++)
        {
            SquareType _square = CombatManager.instance.Squares.transform.GetChild(i).gameObject.GetComponent<SquareType>();
            UnitGroup _ug = _square.UnitGroup;
            if (_ug != null && _ug.TypePlayer == territoryEvent.TerritoryStats.Territory.TypePlayer)
            {
                ug.Add(_ug);
                break;
            }
        }
    }
    private UnitGroup GetRandomUnitGroup()
    {
        GetUnitGroup();
        int random = UnityEngine.Random.Range(0, ug.Count);
        index_to_mod = random;
        return ug[random];
    }
    private void GetUnitDeleted()
    {
        building_to_mod = " ";
        if (UnityEngine.Random.Range(0, 100) <= 20 && ug.Count > 1)
        {
            building_to_mod = ug[1].UnitCombat.UnitName;
            ug[1].UnitCombat.Quantity = 0;
        }
    }
    private void CheckStatusInEvent()
    {
        if (ug.Count > 0)
        {
            for (int i = 0; i < ug.Count; i++)
            {
                CombatManager.instance.UpdateText(ug[i]);
                CombatManager.instance.UpdateUnitGroup(ug[i]);
            }
            if (CombatManager.instance.EndBattle())
            {
                CombatManager.instance.OpenResumeBattle();
            }
        }
    }
    private int GetFirstPositionFree()
    {
        for (int i = 0; i < 8; i++)
        {
            int pos = i;
            SquareType _square = CombatManager.instance.Squares.transform.GetChild(i).gameObject.GetComponent<SquareType>();
            UnitGroup _ug = _square.UnitGroup;
            if (_ug == null)
            {
                return pos;
            }
        }
        return -1;
    }
    private void PillageTerritory()
    {
        territoryEvent.TerritoryStats.Territory.Gold -= costgold;
        territoryEvent.TerritoryStats.Territory.FoodReward -= costfood;
    }
    public override void AcceptEventAction()
    {
        base.AcceptEventAction();
        isAcceptedEvent = true;
        this.eventStatus = STATUS.PROGRESS;
        switch (eventType)
        {
            case EVENTTYPE.EXCURSION:
                GetFirstUnitGroup();
                mod = 6;
                for (int i = 0; i < ug.Count; i++)
                {
                    ug[i].UnitCombat.Attack += mod;
                }
                GetUnitDeleted();
                break;
            case EVENTTYPE.INTOXICATE:
                mod = 20;
                GetUnitGroup();
                for (int i = 0; i < ug.Count; i++)
                {
                    ug[i].UnitCombat.Precision -= mod;
                }
                miss = false;
                break;
            case EVENTTYPE.WOOKIE:
                UnitGroup u = GetRandomUnitGroup();
                
                u.UnitCombat.Quantity -= 10;
                if (u.UnitCombat.Quantity<= 0)
                {
                    u.UnitCombat.Quantity = 0;
                }
                //HACER QUE SE COMPRUEBE SI SE TERMINO EL COMBATE PARA EVITAR BUGS PINSHI SAPO
                break;
            case EVENTTYPE.REINFORCEMENT:
                GetFirstUnitGroup();
                units_to_mod = 12;
                index_to_mod = GetFirstPositionFree();
                break;
            case EVENTTYPE.CURE:
                Cure();
                break;
            case EVENTTYPE.ABILITYX2:
                CombatManager.instance.LimitSA++;
                break;
            case EVENTTYPE.PILLAGE:
                costgold = territoryEvent.TerritoryStats.Territory.Gold;
                PillageTerritory();
                break;
            case EVENTTYPE.BURN:
                building_to_mod = "Farm";
                break;
            default:
                break;
        }
        CheckStatusInEvent();

    }
    public override void DeclineEventAction()
    {
        base.DeclineEventAction();
        isAcceptedEvent = true;
        this.eventStatus = STATUS.PROGRESS;
        switch (eventType)
        {
            case EVENTTYPE.EXCURSION:
                GetUnitGroup();
                mod = 12;
                for (int i = 0; i < ug.Count; i++)
                {
                    ug[i].UnitCombat.Attack += 12;
                }
                GetUnitDeleted();
                break;
            case EVENTTYPE.INTOXICATE:
                GetUnitGroup();
                mod = 10;
                for (int i = 0; i < ug.Count; i++)
                {
                    ug[i].UnitCombat.Precision -= 10;
                }
                miss = false;
                break;
            case EVENTTYPE.WOOKIE:
                UnitGroup u = GetRandomUnitGroup();
                
                u.UnitCombat.Quantity -= 20;
                if (u.UnitCombat.Quantity <= 0)
                {
                    u.UnitCombat.Quantity = 0;
                }
                //ACA TAMBIEN COMPROBAR PS NO TE PASES SAPO
                break;
            case EVENTTYPE.REINFORCEMENT:
                GetFirstUnitGroup();
                units_to_mod = 8;
                index_to_mod = GetFirstPositionFree();
                break;
            case EVENTTYPE.CURE:
                Cure();
                break;
            case EVENTTYPE.ABILITYX2:
                CombatManager.instance.LimitSA++;
                break;
            case EVENTTYPE.PILLAGE:
                costfood = territoryEvent.TerritoryStats.Territory.FoodReward;
                PillageTerritory();
                break;
            case EVENTTYPE.BURN:
                int random = UnityEngine.Random.Range(0, Utils.instance.Buildings_string.Count);
                building_to_mod = Utils.instance.Buildings_string[random];
                break;
            default:
                break;
        }
        CheckStatusInEvent();
    }
    public override void AdicitionalEventAction()
    {
        base.AdicitionalEventAction();
        isAcceptedEvent = true;
        this.eventStatus = STATUS.PROGRESS;
        switch (eventType)
        {
            case EVENTTYPE.EXCURSION:
                GetUnitGroup();
                miss = true;
                break;
            case EVENTTYPE.INTOXICATE:
                GetUnitGroup();
                miss = true;
                break;
            case EVENTTYPE.WOOKIE:
                UnitGroup u = GetRandomUnitGroup();
                u.UnitCombat.Quantity = 0;
                break;
            case EVENTTYPE.REINFORCEMENT:
                GetFirstUnitGroup();
                miss = true;
                break;
            case EVENTTYPE.CURE:
                Cure();
                miss = true;
                break;
            case EVENTTYPE.ABILITYX2:
                miss = true;
                break;
            case EVENTTYPE.PILLAGE:
                miss = true;
                break;
            case EVENTTYPE.BURN:
                miss = true;
                break;
            default:
                break;
        }
        CheckStatusInEvent();
    }
    // animacion en batalla
    public override void CloseEventAction()
    {
        base.CloseEventAction();
        isAcceptedEvent = false;
        this.eventStatus = STATUS.FINISH;
        switch (eventType)
        {
            case EVENTTYPE.EXCURSION:
                if (!miss)
                {
                    for (int i = 0; i < ug.Count; i++)
                    {
                        CombatManager.instance.ShowFloatText("+"+ mod +" attack", ug[i].UnitsGO);
                    }
                    if (building_to_mod != " ")
                    {
                        CombatManager.instance.ShowFloatTextMC("- " + building_to_mod);
                    }
                }
                else
                {
                    CombatManager.instance.ShowFloatTextMC("- miss");
                }
                break;
            case EVENTTYPE.INTOXICATE:
                //Debug.Log("miss? " + miss);
                if (!miss)
                {
                    for (int i = 0; i < ug.Count; i++)
                    {
                        CombatManager.instance.ShowFloatText("-" + mod + " precision", ug[i].UnitsGO);
                    }
                }
                else
                {
                    for (int i = 0; i < ug.Count; i++)
                    {
                        CombatManager.instance.ShowFloatText("miss intoxicate", ug[i].UnitsGO);
                    }
                    
                }
                break;
            case EVENTTYPE.WOOKIE:
                for (int i = 0; i < ug.Count; i++)
                {
                    CombatManager.instance.ShowFloatText("attacked by wookie", ug[i].UnitsGO);
                }
                break;
            case EVENTTYPE.REINFORCEMENT:
                if (!miss)
                {
                    UnitCombat new_uc = Utils.instance.GetNewUnitCombat(ug[0].UnitCombat.UnitName);
                    new_uc.Quantity = units_to_mod;
                    CombatManager.instance.InstantiateUnitPlayer(index_to_mod, new_uc);
                }
                else
                {
                    CombatManager.instance.ShowFloatText("miss", ug[0].UnitsGO);
                }
                break;
            case EVENTTYPE.CURE:
                if (!miss)
                {
                    ug[0].UnitCombat.Quantity += units_to_mod;
                    CombatManager.instance.ShowFloatText("+" + units_to_mod + " units", ug[0].UnitsGO);
                }
                else
                {
                    CombatManager.instance.ShowFloatText("miss", ug[0].UnitsGO);
                }
                break;
            case EVENTTYPE.ABILITYX2:
                if (!miss)
                {
                    CombatManager.instance.ShowFloatTextButton("+1 time");
                }
                else
                {
                    CombatManager.instance.ShowFloatTextButton("miss");
                }
                
                break;
            case EVENTTYPE.PILLAGE:
                if (!miss)
                {
                    InGameMenuHandler.instance.GoldPlayer += costgold;
                    InGameMenuHandler.instance.GoldPlayer += costfood;
                    CombatManager.instance.ShowFloatTextMC("- " + costgold + " gold\n - " + costfood + " food");
                    //CombatManager.instance.ShowFloatText("+ "+costgold +" gold\n + " + costfood + " food", militarGO);
                }
                else
                {
                    CombatManager.instance.ShowFloatTextMC("miss");
                }
                break;
            case EVENTTYPE.BURN:
                if (!miss)
                {
                    territoryEvent.TerritoryStats.Territory.GetBuilding(building_to_mod).ResetBuilding(territoryEvent.TerritoryStats.Territory);
                    CombatManager.instance.ShowFloatTextMC("Reseting "+ building_to_mod);
                }
                else
                {
                    CombatManager.instance.ShowFloatTextMC("miss");
                }
                break;
            default:
                break;
        }
        CheckStatusInEvent();
    }
}