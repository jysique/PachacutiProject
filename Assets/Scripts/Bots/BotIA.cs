using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BotIA 
{
    private bool actionRealized;
    [SerializeField] private int c;
    [SerializeField] private int actionNumber;
    [SerializeField] public int desicionTime = 400;
    private int warTax = 10;

    //Bot variables
    [SerializeField]private List<OptionBot> options;
    [SerializeField]private List<TerritoryHandler> territories;
    [SerializeField] Territory.TYPEPLAYER typeBot;
    [SerializeField] int gold;
    [SerializeField] int food;
    public int GoldBot
    {
        get { return gold; }
        set { gold = value; }
    }
    public int FoodBot
    {
        get { return food; }
        set { food = value; }
    }
    public BotIA(Territory.TYPEPLAYER _typeBot, int _gold, int _food)
    {
        actionRealized = false;
        typeBot = _typeBot;
        gold = _gold;
        food = _food;
        c = 0;
        ObtainActionNumber();
        options = new List<OptionBot>();
        territories = new List<TerritoryHandler>();
    }

    public void AddTerritory(TerritoryHandler territory)
    {
        territories.Add(territory);
    }

    public List<TerritoryHandler> Territories
    {
        get { return territories; }
        set { territories = value; }
    }
    public Territory.TYPEPLAYER TypeBot
    {
        get { return typeBot; }
        set { typeBot = value; }
    }

  

    public void DoAction()
    {
        c= (int)(c + (1* GlobalVariables.instance.timeModifier));
        if (c >= actionNumber && !actionRealized)
        {
            GetPosibleOptions();
            SelectOptions();
            actionRealized = true;
        }
        if(c >= desicionTime)
        {
            c = 0;
            ObtainActionNumber();
            GetResources();
            actionRealized = false;
        }
    }
    private void ObtainActionNumber()
    {
        actionNumber = Random.Range(0, desicionTime);

    }

    private void GetPosibleOptions()
    {

        options.Clear();
        foreach (TerritoryHandler t in territories)
        {
            if (t.war)
            {
                List<GameObject> adjacents = t.AdjacentTerritories;
                foreach (GameObject a in adjacents)
                {
                    TerritoryHandler te = a.GetComponent<TerritoryHandler>();
                    if (te.TerritoryStats.Territory.TypePlayer == typeBot)
                    {
                        GetMoveOptions(3, t, te);
                    }
                }
            }
            if (!TerritoryManager.instance.IsLimit(t))
            {
                List<GameObject> adjacents = t.AdjacentTerritories;
                foreach (GameObject a in adjacents)
                {
                    TerritoryHandler te = a.GetComponent<TerritoryHandler>();
                    GetMoveOptions(1, t, te);
                    if (TerritoryManager.instance.IsLimit(te))
                    {
                        GetMoveOptions(6, t, te);
                    }
                }
            }
            else
            {
                List<GameObject> adjacents = t.AdjacentTerritories;
                foreach (GameObject a in adjacents)
                {
                    TerritoryHandler te = a.GetComponent<TerritoryHandler>();
                    if(te.TerritoryStats.Territory.TypePlayer != t.TerritoryStats.Territory.TypePlayer && te.TerritoryStats.Territory.TypePlayer != Territory.TYPEPLAYER.WASTE)
                    {
                        GetMoveOptions(1, t, te);
                        if(te.TerritoryStats.Territory.Population <= t.TerritoryStats.Territory.Population && 
                            gold - warTax >= 0 && t.TerritoryStats.Territory.Population >= 5)
                        {
                            GetImproveBuilding(2, t, te);
                            GetAddUnitOptions(2,t, te);
                            GetMoveOptions(6, t, te);
                        }
                    }
                }
            }
        }
    }
    private void GetImproveBuilding(int _options, TerritoryHandler _t, TerritoryHandler _te)
    {
        for (int i = 0; i < _options; i++)
        {
            OptionUpgradeBuilding optionBot = new OptionUpgradeBuilding(_t, _te);
            options.Add(optionBot);
        }
    }

    private void GetMoveOptions(int _options, TerritoryHandler _t,TerritoryHandler _te)
    {
        for (int i = 0; i < _options; i++)
        {
            OptionMoveTroop optionBot = new OptionMoveTroop(_t,_te);
            options.Add(optionBot);
        }
    }
    private void GetAddUnitOptions(int _options, TerritoryHandler _t, TerritoryHandler _te)
    {
        for (int i = 0; i < _options; i++)
        {
            OptionAddUnit optionBot = new OptionAddUnit(_t, _te);
            options.Add(optionBot);
        }
    }

    private void SelectOptions()
    {

        int i = Random.Range(0, options.Count);
      //  options[i].Logger();
        switch (options[i].NameOption)
        {
            case "move":
                OptionMoveTroop a = (OptionMoveTroop)options[i];
                MoveTroops(a.Begin, a.End, a.TroopOptionBot);
                break;
            case "add":
                OptionAddUnit b = (OptionAddUnit)options[i];
                AddNewUnitCombat(b.Begin,b.UnitCombat);
                break;
            case "upgrade":
                OptionUpgradeBuilding c = (OptionUpgradeBuilding)options[i];
                UpgradeBuilding(c.Begin, c.Building);
                break;
            default:
                break;
        }
        
    }
    private void UpgradeBuilding(TerritoryHandler begin, Building building)
    {
        if (gold - building.CostToUpgrade>=0)
        {
            //Debug.Log("upgrade" + begin.name + " - " + building.Name);
            gold -= building.CostToUpgrade;
            //
            EventManager.instance.AddEvent(begin, building);
        }
        else
        {
            Debug.Log("no resources to upgrade building");
        }
        
    }
    private void AddNewUnitCombat(TerritoryHandler begin, UnitCombat unitCombat)
    {
        if (gold - unitCombat.Quantity >= 0)
        {
            gold -= unitCombat.Quantity;
            //
            EventManager.instance.AddEvent(begin, unitCombat);
        }
        else
        {
            Debug.Log("no resources to add unit");
        }

    }
    private void MoveTroops(TerritoryHandler begin, TerritoryHandler end, Troop _troop)
    {
        if (_troop.GetAllNumbersUnit() != 0)
        {
            if (begin.TerritoryStats.Territory.TypePlayer != end.TerritoryStats.Territory.TypePlayer)//conquista
            {
                if(gold - warTax >= 0)
                {
                    gold -= warTax;
                 //   Debug.Log("mover");
                    _troop.MoveUnits(begin.TerritoryStats.Territory);
                    WarManager.instance.SendWarriors(begin, end, _troop);
                }
            }
            else//movimiento
            {
                WarManager.instance.SendWarriors(begin, end, _troop);
            }

        }
        else
        {
          //  Debug.Log("troop == 0");
        }
    }

    public void GetResources()
    {
        foreach (TerritoryHandler t in territories)
        {
            gold += t.TerritoryStats.Territory.Gold;
            food += t.TerritoryStats.Territory.FoodReward;
            t.TerritoryStats.Territory.Gold = 0;
            t.TerritoryStats.Territory.FoodReward = 0;
        }
    }

    public void DeleteTerritory(TerritoryHandler t)
    {
        territories.Remove(t);
    }
}
[System.Serializable]
public class OptionBot
{
    [SerializeField] protected TerritoryHandler begin;
    [SerializeField] protected TerritoryHandler end;
    [SerializeField] protected string nameoption;
    public string NameOption
    {
        get { return nameoption; }
    }
    public TerritoryHandler Begin
    {
        get { return begin; }
    }
    public TerritoryHandler End
    {
        get { return end; }
    }

    public OptionBot(TerritoryHandler _begin, TerritoryHandler _end)
    {
        begin = _begin;
        end = _end;

    }
    public virtual void Logger()
    {
        Debug.Log("se eligio " + nameoption + " en " + begin.name);
        Debug.Log("info:");
    }
}
public class OptionMoveTroop : OptionBot
{
    [SerializeField] private Troop troop;
    public Troop TroopOptionBot
    {
        get { return troop; }
    }
    public OptionMoveTroop(TerritoryHandler _begin, TerritoryHandler _end) : base(_begin, _end)
    {
        nameoption = "move";
        Troop _troop = new Troop();
        Territory _territory = _begin.TerritoryStats.Territory;
        for (int i = 0; i < _territory.ListUnitCombat.UnitCombats.Count; i++)
        {
            
            if (Random.Range(0, 100) > 60 && _territory.ListUnitCombat.UnitCombats[i].IsAvailable)
            {
                _troop.AddUnitCombat(_territory.ListUnitCombat.UnitCombats[i]);
            }
        }
        this.troop = _troop;
    }
    public override void Logger()
    {
        base.Logger();
        Debug.Log(" nro unidades " + troop.UnitCombats.Count);
    }
}
public class OptionAddUnit: OptionBot
{
    [SerializeField] private UnitCombat unitCombat;
    public UnitCombat UnitCombat
    {
        get { return unitCombat; }
    }

    public OptionAddUnit(TerritoryHandler _begin, TerritoryHandler _end) : base(_begin, _end)
    {
        nameoption = "add";
        unitCombat = Utils.instance.GetUnitCombatRandom(_begin.TerritoryStats.Territory);
        unitCombat.Quantity = Random.Range(10, 30);
    }
    public override void Logger()
    {
        base.Logger();
        Debug.Log(" name " + unitCombat.UnitName + " nro unidades " + unitCombat.Quantity);
    }
}
public class OptionUpgradeBuilding : OptionBot
{
    [SerializeField] private Building building;
    public Building Building
    {
        get { return building; }
    }

    public OptionUpgradeBuilding(TerritoryHandler _begin, TerritoryHandler _end) : base(_begin, _end)
    {
        nameoption = "upgrade";
        building = Utils.instance.GetBuildingRandom(_begin.TerritoryStats.Territory);
    }
    public override void Logger()
    {
        base.Logger();
        Debug.Log(" name " + building.Name);
    }
}
