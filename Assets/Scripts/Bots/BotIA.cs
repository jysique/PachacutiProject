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
                        GetOptions(6, t, te);
                    }
                }
            }
            if (!TerritoryManager.instance.IsLimit(t))
            {
                List<GameObject> adjacents = t.AdjacentTerritories;
                foreach (GameObject a in adjacents)
                {
                    TerritoryHandler te = a.GetComponent<TerritoryHandler>();
                    //options.Add(new OptionBot(t, te, t.TerritoryStats.Territory.Swordsmen.Quantity, t.TerritoryStats.Territory.Lancers.Quantity, t.TerritoryStats.Territory.Axemen.Quantity));
                    options.Add(new OptionBot(t, te));
                    if (TerritoryManager.instance.IsLimit(te))
                    {
                        GetOptions(6, t, te);
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
                        options.Add(new OptionBot(t, te));
                        //options.Add(new OptionBot(t, te, 0,0,0));
                        if(te.TerritoryStats.Territory.Population <= t.TerritoryStats.Territory.Population && 
                            gold - warTax >= 0 && t.TerritoryStats.Territory.Population >= 5)
                        {
                            GetOptions(6, t, te);
                        }
                        
                    }
                }
            }
        }
    }

    private void GetOptions(int _options, TerritoryHandler _t,TerritoryHandler _te)
    {
        for (int i = 0; i < _options; i++)
        {
            OptionBot optionBot = new OptionBot(_t,_te);
            options.Add(optionBot);
        }
    }

    private void SelectOptions()
    {

        int i = Random.Range(0, options.Count);
        if (options[i].TroopOptionBot.GetAllNumbersUnit() != 0)
        {
            //Debug.Log("if enter");
            MoveTroops(options[i].Begin, options[i].End, options[i].TroopOptionBot);
        }
    }
    //private void MoveTroops(TerritoryHandler begin, TerritoryHandler end, int number,int number2,int number3)
    private void MoveTroops(TerritoryHandler begin, TerritoryHandler end, Troop _troop)
    {
        if (_troop.GetAllNumbersUnit() != 0)
        {
            if (begin.TerritoryStats.Territory.TypePlayer != end.TerritoryStats.Territory.TypePlayer)//conquista
            {
                if(gold - warTax >= 0)
                {
                    gold -= warTax;
                    _troop.MoveUnits(begin.TerritoryStats.Territory);
                    WarManager.instance.SendWarriors(begin, end, _troop);
                }
            }
            else//movimiento
            {
                WarManager.instance.SendWarriors(begin, end, _troop);
            }
            
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
    [SerializeField] private TerritoryHandler begin;
    [SerializeField] private TerritoryHandler end;
    [SerializeField] private Troop troop;
    public Troop TroopOptionBot
    {
        get { return troop; }
    }
    public TerritoryHandler Begin
    {
        get { return begin; }
    }
    public TerritoryHandler End
    {
        get { return end; }
    }
    /*
    public OptionBot(TerritoryHandler _begin, TerritoryHandler _end, int _number,int _number2, int _number3)
    {
        begin = _begin;
        end = _end;
        Swordsman s = new Swordsman();
        s.Quantity = _number;
        s.PositionInBattle = 0;
        this.troop.AddUnitCombat(s);
        Axeman a = new Axeman();
        a.Quantity = _number2;
        a.PositionInBattle = 1;
        this.troop.AddUnitCombat(a);
        
        Archer ar = new Archer();
        ar.Quantity = _number3;
        ar.PositionInBattle = 2;
        this.troop.AddUnitCombat(ar);
    }
    */
    public OptionBot(TerritoryHandler _begin, TerritoryHandler _end)
    {
        begin = _begin;
        end = _end;
        Troop _troop = new Troop();
        Territory _territory = _begin.TerritoryStats.Territory;
        for (int i = 0; i < _territory.ListUnitCombat.UnitCombats.Count; i++)
        {
            int random = UnityEngine.Random.Range(0, 100);
            if (random>60)
            {
                _troop.AddUnitCombat(_territory.ListUnitCombat.UnitCombats[i]);
            }
        }
        this.troop = _troop;
    }
}
