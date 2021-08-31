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
        this.nameEvent = GameMultiLang.GetTraduction(eventType.ToString() + "_NAME");
        this.btn1 = GameMultiLang.GetTraduction(eventType.ToString() + "1");
        this.btn2 = GameMultiLang.GetTraduction(eventType.ToString() + "2");
        this.btn3 = GameMultiLang.GetTraduction(eventType.ToString() + "3");
        this.message = GameMultiLang.GetTraduction(eventType.ToString() + "M").Replace("TERRITORYEVENT", TerritoryEvent.name);
        this.acceptMessageEvent = GameMultiLang.GetTraduction(eventType.ToString() + "A").Replace("TERRITORYEVENT", TerritoryEvent.name);
        this.declineMessageEvent = GameMultiLang.GetTraduction(eventType.ToString() + "D").Replace("TERRITORYEVENT", TerritoryEvent.name);
        this.thirdMessageEvent = GameMultiLang.GetTraduction(eventType.ToString() + "T").Replace("TERRITORYEVENT", TerritoryEvent.name);
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
            case EVENTTYPE.REINFORCEMENT:
            case EVENTTYPE.CURE:
            //case EVENTTYPE.REBEL:
            case EVENTTYPE.TRAMP:
            case EVENTTYPE.PILLAGE:
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
        TerritoryManager.instance.UpdateUnitsDeffend(territoryEvent.TerritoryStats.Territory);
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
        TerritoryManager.instance.UpdateUnitsDeffend(territoryEvent.TerritoryStats.Territory);
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
                TerritoryEvent.TerritoryStats.Territory.Lancers.Quantity /= 2;
                TerritoryEvent.TerritoryStats.Territory.Swordsmen.Quantity /= 2;
                TerritoryEvent.TerritoryStats.Territory.Axemen.Quantity /= 2;
                TerritoryEvent.TerritoryStats.Territory.Scouts.Quantity /= 2;
                break;
            case EVENTTYPE.ALL_T_PANDEMIC:
                List<TerritoryHandler> list = TerritoryManager.instance.GetTerritoriesHandlerByTypePlayer(Territory.TYPEPLAYER.PLAYER);
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].TerritoryStats.Territory.Lancers.Quantity /= 2;
                    list[i].TerritoryStats.Territory.Swordsmen.Quantity /= 2;
                    list[i].TerritoryStats.Territory.Axemen.Quantity /= 2;
                    list[i].TerritoryStats.Territory.Scouts.Quantity /= 2;
                }
                break;
            case EVENTTYPE.PANDEMIC:
                TerritoryEvent.TerritoryStats.Territory.Lancers.Quantity /= 2;
                TerritoryEvent.TerritoryStats.Territory.Swordsmen.Quantity /= 2;
                TerritoryEvent.TerritoryStats.Territory.Axemen.Quantity /= 2;
                TerritoryEvent.TerritoryStats.Territory.Scouts.Quantity /= 2;
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
           // case EVENTTYPE.GRACE_DIV:
           //     TerritoryEvent.TerritoryStats.Territory.OpinionTerritory -= 10;
           //     break;
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
        TerritoryManager.instance.UpdateUnitsDeffend(territoryEvent.TerritoryStats.Territory);
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
          //  case EVENTTYPE.GRACE_DIV:
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
            if (eventType == EVENTTYPE.EXPLORATION)
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
     //   GRACE_DIV,
        GRACE_MIN,
        GRACE_FOOD,
        CONQUIST,
        EXPLORATION,
        REINFORCEMENT,
        CURE,
        // REBEL,
        TRAMP,
        PILLAGE
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
        this.eventType = EVENTTYPE.EXPLORATION;
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
    UnitGroup ug;
    [SerializeField] private Troop troop = new Troop();
    Territory.TYPEPLAYER typeEvent;
    int units_to_mod;
    int costgold;
    int costfood;
    bool miss = false;
    public int TurnEvent
    {
        get { return turn; }
    }
    public CustomBattle(Troop _troopPlayer, Troop _troopNoPlayer, TerritoryHandler _territoryPlayer, TerritoryHandler _territoryNoPlayer)
    {
        this.eventStatus = STATUS.ANNOUNCE;
    //    this.territoryEvent = _territoryHandler;
        this.turn = UnityEngine.Random.Range(0, 21);
        // this.eventType = (EVENTTYPE)UnityEngine.Random.Range(14, 17);
        this.eventType = EVENTTYPE.PILLAGE;
        this.isBattle = true;
        if (eventType != EVENTTYPE.TRAMP && eventType != EVENTTYPE.PILLAGE)
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
    private void GetUnitGroup(int _refDividedBy, bool add )
    {
        int squares_count = CombatManager.instance.Squares.transform.childCount;
        int max_units_count = troop.UnitCombats.Count-1;
        int random = UnityEngine.Random.Range(0, max_units_count);
        Debug.Log("max|" + max_units_count + "rand" + random);
        Debug.Log(troop.UnitCombats[random].UnitName);
        UnitCombat uc = troop.UnitCombats[random];
        int _pos = troop.Positions[random];
        for (int i = 0; i < squares_count; i++)
        {
            SquareType _square = CombatManager.instance.Squares.transform.GetChild(i).gameObject.GetComponent<SquareType>();
            UnitGroup _ug = _square.unitGroup;
            if (_ug != null && _ug.TypePlayer == this.typeEvent && _ug.UnitCombat.PositionInBattle == _pos)
            {
                ug = _ug;
                int _original_quantity = uc.Quantity;
                int _actual_quantity = _ug.UnitCombat.Quantity;
                if (add)
                {
                    units_to_mod = (_original_quantity - _actual_quantity) / _refDividedBy;
                }
                else
                {
                    units_to_mod = _actual_quantity / _refDividedBy;
                }
                Debug.Log("origin" + _original_quantity);
                Debug.Log("actual" + _actual_quantity);
                Debug.Log("refuerzos" + units_to_mod);
               // reinforcement = _reinforcement;
                break;
            }
        }
    }
    private void PillageTerritory()
    {
        territoryEvent.TerritoryStats.Territory.Gold -= costgold;
        territoryEvent.TerritoryStats.Territory.FoodReward -= costfood;
    }
    public override void AcceptEventAction()
    {
        base.AcceptEventAction();
        switch (eventType)
        {
            case EVENTTYPE.REINFORCEMENT:
                GetUnitGroup(2, true);
                units_to_mod = 20;
                break;
            case EVENTTYPE.CURE:
                GetUnitGroup(1, true);
                break;
                //case EVENTTYPE.REBEL:
                //break;
            case EVENTTYPE.TRAMP:
                GetUnitGroup(1,false);
                break;
            case EVENTTYPE.PILLAGE:
                GetUnitGroup(1, false); // reemplazar el unitgrupo por el transform del militar
                costgold = territoryEvent.TerritoryStats.Territory.Gold;
                PillageTerritory();
                break;
            default:
                break;
        }
    }
    public override void DeclineEventAction()
    {
        base.DeclineEventAction();
        switch (eventType)
        {
            case EVENTTYPE.REINFORCEMENT:
                GetUnitGroup(2, true);
                units_to_mod = 10;
                break;
            case EVENTTYPE.CURE:
                GetUnitGroup(1, true);
                break;
            //case EVENTTYPE.REBEL:
            //  break;
            case EVENTTYPE.TRAMP:
                GetUnitGroup(2,false);
                break;
            case EVENTTYPE.PILLAGE:
                GetUnitGroup(1, false); // reemplazar el unitgrupo por el transform del militar
                costfood = territoryEvent.TerritoryStats.Territory.FoodReward;
                PillageTerritory();
                break;
            default:
                break;
        }
    }
    public override void AdicitionalEventAction()
    {
        base.AdicitionalEventAction();
        switch (eventType)
        {
            case EVENTTYPE.REINFORCEMENT:
                GetUnitGroup(1,true);
                units_to_mod = 0;
                break;
            case EVENTTYPE.CURE:
                GetUnitGroup(1,true);
                break;
            /*
        case EVENTTYPE.REBEL:
            break;
            */
            case EVENTTYPE.TRAMP:
                miss = true;
                GetUnitGroup(1, false);
                break;
            case EVENTTYPE.PILLAGE:
                GetUnitGroup(1, false); // reemplazar el unitgrupo por el transform del militar
                costfood = territoryEvent.TerritoryStats.Territory.FoodReward/3;
                costgold = territoryEvent.TerritoryStats.Territory.Gold/3;
                PillageTerritory();
                break;
            default:
                break;
        }

    }
    public override void CloseEventAction()
    {
        base.CloseEventAction();
        switch (eventType)
        {
            case EVENTTYPE.REINFORCEMENT:
            case EVENTTYPE.CURE:
                ug.UnitCombat.Quantity += units_to_mod;
                CombatManager.instance.ShowFloatText("+" + units_to_mod + " units", ug.UnitsGO);
                break;
            //  case EVENTTYPE.REBEL:
            //      break;
            case EVENTTYPE.TRAMP:
                if (!miss)
                {
                    ug.UnitCombat.Quantity -= units_to_mod;
                    CombatManager.instance.AnimationInBattle(ug.UnitsGO,"rock");
                    //CombatManager.instance.ShowFloatText("-" + units_to_mod + " units", ug.UnitsGO);
                    //Verificar ug
                }
                else
                {
                    CombatManager.instance.ShowFloatText("miss" ,ug.UnitsGO); 
                }
                
                break;
            case EVENTTYPE.PILLAGE:
                InGameMenuHandler.instance.GoldPlayer += costgold;
                InGameMenuHandler.instance.GoldPlayer += costfood;
                CombatManager.instance.ShowFloatText("- "+costgold +" gold\n - " + costfood + " food", ug.UnitsGO);
                //CombatManager.instance.ShowFloatText("+ "+costgold +" gold\n + " + costfood + " food", militarGO);
                break;
            default:
                break;
        }
        CombatManager.instance.UpdateText(ug);
    }
}