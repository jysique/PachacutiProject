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
                        /*
                        options.Add(new OptionBot(te, t, t.TerritoryStats.Territory.Swordsmen.Quantity / 2, t.TerritoryStats.Territory.Lancers.Quantity / 2, t.TerritoryStats.Territory.Axemen.Quantity / 2));
                        options.Add(new OptionBot(te, t, t.TerritoryStats.Territory.Swordsmen.Quantity / 2, t.TerritoryStats.Territory.Axemen.Quantity / 2, t.TerritoryStats.Territory.Scouts.Quantity / 2));
                        options.Add(new OptionBot(te, t, t.TerritoryStats.Territory.Swordsmen.Quantity / 2, t.TerritoryStats.Territory.Scouts.Quantity / 2, t.TerritoryStats.Territory.Archers.Quantity / 2));
                        options.Add(new OptionBot(te, t, t.TerritoryStats.Territory.Lancers.Quantity / 2, t.TerritoryStats.Territory.Scouts.Quantity / 2, t.TerritoryStats.Territory.Archers.Quantity / 2));
                        options.Add(new OptionBot(te, t, t.TerritoryStats.Territory.Swordsmen.Quantity / 2, t.TerritoryStats.Territory.Axemen.Quantity / 2, t.TerritoryStats.Territory.Archers.Quantity / 2));
                        options.Add(new OptionBot(te, t, t.TerritoryStats.Territory.Axemen.Quantity / 2, t.TerritoryStats.Territory.Scouts.Quantity / 2, t.TerritoryStats.Territory.Archers.Quantity / 2));
                        */
                    }
                }
            }
            if (!TerritoryManager.instance.IsLimit(t))
            {
                List<GameObject> adjacents = t.AdjacentTerritories;
                foreach (GameObject a in adjacents)
                {
                    TerritoryHandler te = a.GetComponent<TerritoryHandler>();
                    options.Add(new OptionBot(t, te, t.TerritoryStats.Territory.Swordsmen.Quantity, t.TerritoryStats.Territory.Lancers.Quantity, t.TerritoryStats.Territory.Axemen.Quantity));
                    if (TerritoryManager.instance.IsLimit(te))
                    {
                        /*
                        options.Add(new OptionBot(t, te,t.TerritoryStats.Territory.Swordsmen.Quantity, t.TerritoryStats.Territory.Lancers.Quantity, t.TerritoryStats.Territory.Axemen.Quantity));
                        options.Add(new OptionBot(t, te,t.TerritoryStats.Territory.Swordsmen.Quantity, t.TerritoryStats.Territory.Lancers.Quantity, t.TerritoryStats.Territory.Axemen.Quantity));

                        options.Add(new OptionBot(t, te, t.TerritoryStats.Territory.Swordsmen.Quantity, t.TerritoryStats.Territory.Lancers.Quantity, t.TerritoryStats.Territory.Axemen.Quantity));
                        options.Add(new OptionBot(t, te, t.TerritoryStats.Territory.Swordsmen.Quantity, t.TerritoryStats.Territory.Axemen.Quantity, t.TerritoryStats.Territory.Scouts.Quantity));
                        options.Add(new OptionBot(t, te, t.TerritoryStats.Territory.Swordsmen.Quantity, t.TerritoryStats.Territory.Scouts.Quantity, t.TerritoryStats.Territory.Archers.Quantity));
                        options.Add(new OptionBot(t, te, t.TerritoryStats.Territory.Lancers.Quantity, t.TerritoryStats.Territory.Scouts.Quantity, t.TerritoryStats.Territory.Archers.Quantity));
                        options.Add(new OptionBot(t, te, t.TerritoryStats.Territory.Swordsmen.Quantity, t.TerritoryStats.Territory.Axemen.Quantity, t.TerritoryStats.Territory.Archers.Quantity));
                        options.Add(new OptionBot(t, te, t.TerritoryStats.Territory.Axemen.Quantity, t.TerritoryStats.Territory.Scouts.Quantity, t.TerritoryStats.Territory.Archers.Quantity));
                                                */
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
                        options.Add(new OptionBot(t, te, 0,0,0));
                        if(te.TerritoryStats.Territory.Population <= t.TerritoryStats.Territory.Population && 
                            gold - warTax >= 0 && t.TerritoryStats.Territory.Population >= 5)
                        {
                            GetOptions(6, t, te);
                           /* options.Add(new OptionBot(t, te, t.TerritoryStats.Territory.Swordsmen.Quantity, t.TerritoryStats.Territory.Lancers.Quantity, t.TerritoryStats.Territory.Axemen.Quantity));
                            options.Add(new OptionBot(t, te, t.TerritoryStats.Territory.Swordsmen.Quantity, t.TerritoryStats.Territory.Axemen.Quantity, t.TerritoryStats.Territory.Scouts.Quantity));
                            options.Add(new OptionBot(t, te, t.TerritoryStats.Territory.Swordsmen.Quantity, t.TerritoryStats.Territory.Scouts.Quantity, t.TerritoryStats.Territory.Archers.Quantity));
                            options.Add(new OptionBot(t, te, t.TerritoryStats.Territory.Lancers.Quantity, t.TerritoryStats.Territory.Scouts.Quantity, t.TerritoryStats.Territory.Archers.Quantity));
                            options.Add(new OptionBot(t, te, t.TerritoryStats.Territory.Swordsmen.Quantity, t.TerritoryStats.Territory.Axemen.Quantity, t.TerritoryStats.Territory.Archers.Quantity));
                            options.Add(new OptionBot(t, te, t.TerritoryStats.Territory.Axemen.Quantity, t.TerritoryStats.Territory.Scouts.Quantity, t.TerritoryStats.Territory.Archers.Quantity));
                           */
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
        //Debug.Log(typeBot.ToString() + "-"+i+"-"+options[i].AllNumber);
        if (options[i].AllNumber != 0)
        {
            //Debug.Log("if enter");
            //  MoveTroops(options[i].Begin, options[i].End, options[i].Number, options[i].Number2, options[i].Number3);
            MoveTroops(options[i].Begin, options[i].End, options[i].TroopOptionBot);
        }
    }
    //private void MoveTroops(TerritoryHandler begin, TerritoryHandler end, int number,int number2,int number3)
    private void MoveTroops(TerritoryHandler begin, TerritoryHandler end, Troop _troop)
    {
        //Troop troop = new Troop(number, number2, number3);
        if (_troop.GetAllNumbersUnit() != 0)
        {
            if (begin.TerritoryStats.Territory.TypePlayer != end.TerritoryStats.Territory.TypePlayer)//conquista
            {
                if(gold - warTax >= 0)
                {
                    gold -= warTax;
                    // begin.TerritoryStats.Territory.Swordsmen.Quantity -= number;
                    // begin.TerritoryStats.Territory.Lancers.Quantity -= number2;
                    // begin.TerritoryStats.Territory.Axemen.Quantity -= number3;
                    _troop.MoveUnits(begin.TerritoryStats.Territory);
                    WarManager.instance.SendWarriors(begin, end, _troop);
                }
            }
            else//movimiento
            {
                //    WarManager.instance.SendWarriors(begin, end, number,number2,number3);
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
    [SerializeField] private int number;
    [SerializeField] private int number2;
    [SerializeField] private int number3;
    private int all_number;
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
    public int AllNumber
    {
        get { return all_number; }
    }
    public int Number
    {
        get { return number; }
    }
    public int Number2
    {
        get { return number2; }
    }
    public int Number3
    {
        get { return number3; }
    }
    public OptionBot(TerritoryHandler _begin, TerritoryHandler _end, int _number,int _number2, int _number3)
    {
        begin = _begin;
        end = _end;
        number = _number;
        number2 = _number2;
        number3 = _number3;
        all_number = number + number2 + number3;
    }
    public OptionBot(TerritoryHandler _begin, TerritoryHandler _end)
    {
        begin = _begin;
        end = _end;
        Troop _troop = new Troop();
        if (_begin.TerritoryStats.Territory.Swordsmen.Quantity>0)
        {
            //int r = UnityEngine.Random.Range(1, _begin.TerritoryStats.Territory.Swordsmen.Quantity);
            Swordsman s = new Swordsman();
            s.Quantity = _begin.TerritoryStats.Territory.Swordsmen.Quantity;
            s.PositionInBattle = 0;
            _troop.AddElement(s, 0);
        }
        if (_begin.TerritoryStats.Territory.Archers.Quantity > 0)
        {
            //int r = UnityEngine.Random.Range(1, _begin.TerritoryStats.Territory.Archers.Quantity);
            Archer a = new Archer();
            a.Quantity = _begin.TerritoryStats.Territory.Archers.Quantity;
            a.PositionInBattle = 1;
            _troop.AddElement(a, 1);
            //  _troop.AddElement(_begin.TerritoryStats.Territory.Archers, r);
        }
        if (_begin.TerritoryStats.Territory.Lancers.Quantity > 0)
        {
            //int r = UnityEngine.Random.Range(1, _begin.TerritoryStats.Territory.Scouts.Quantity);
            Lancer l = new Lancer();
            l.Quantity = _begin.TerritoryStats.Territory.Lancers.Quantity;
            l.PositionInBattle = 2;
            _troop.AddElement(l, 2);
            //  _troop.AddElement(_begin.TerritoryStats.Territory.Scouts, r);
        }
        if (_begin.TerritoryStats.Territory.Scouts.Quantity > 0)
        {
            //int r = UnityEngine.Random.Range(1, _begin.TerritoryStats.Territory.Scouts.Quantity);
            Scout sc = new Scout();
            sc.Quantity = _begin.TerritoryStats.Territory.Scouts.Quantity;
            sc.PositionInBattle = 3;
            _troop.AddElement(sc, 3);
            //  _troop.AddElement(_begin.TerritoryStats.Territory.Scouts, r);
        }
        //_troop.LogTroop();
        this.troop = _troop;
        all_number = this.troop.GetAllNumbersUnit();
    }
}
