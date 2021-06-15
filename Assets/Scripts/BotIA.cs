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
            GetGold();
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
            Debug.Log("check");
            if (t.war)
            {
                List<GameObject> adjacents = t.AdjacentTerritories;
                foreach (GameObject a in adjacents)
                {
                    TerritoryHandler te = a.GetComponent<TerritoryHandler>();
                    if (te.territoryStats.territory.TypePlayer == typeBot)
                    {
                        options.Add(new OptionBot(te, t, t.territoryStats.territory.Population/2));
                        options.Add(new OptionBot(te, t, t.territoryStats.territory.Population/2));
                        options.Add(new OptionBot(te, t, t.territoryStats.territory.Population / 2));
                        options.Add(new OptionBot(te, t, t.territoryStats.territory.Population / 2));
                        options.Add(new OptionBot(te, t, t.territoryStats.territory.Population / 2));
                        options.Add(new OptionBot(te, t, t.territoryStats.territory.Population / 2));
                    }
                }
            }
            if (!TerritoryManager.instance.IsLimit(t))
            {
                Debug.Log("no limite");
                List<GameObject> adjacents = t.AdjacentTerritories;
                foreach (GameObject a in adjacents)
                {
                    TerritoryHandler te = a.GetComponent<TerritoryHandler>();
                    options.Add(new OptionBot(t, te, t.territoryStats.territory.Population));
                    if (TerritoryManager.instance.IsLimit(te))
                    {
                        options.Add(new OptionBot(t, te, t.territoryStats.territory.Population));
                        options.Add(new OptionBot(t, te, t.territoryStats.territory.Population));
                    }
                }

            }
            else
            {
                Debug.Log("limite");
                List<GameObject> adjacents = t.AdjacentTerritories;
                foreach (GameObject a in adjacents)
                {
                    TerritoryHandler te = a.GetComponent<TerritoryHandler>();
                    if(te.territoryStats.territory.TypePlayer != t.territoryStats.territory.TypePlayer)
                    {
                        options.Add(new OptionBot(t, te, 0));
                        if(te.territoryStats.territory.Population <= t.territoryStats.territory.Population && gold - warTax >= 0 && t.territoryStats.territory.Population >= 5)
                        {
                            options.Add(new OptionBot(t, te, t.territoryStats.territory.Population));
                            options.Add(new OptionBot(t, te, t.territoryStats.territory.Population));
                            options.Add(new OptionBot(t, te, t.territoryStats.territory.Population));
                            options.Add(new OptionBot(t, te, t.territoryStats.territory.Population));
                        }
                        
                    }
                }
            }
        }
    }

    private void SelectOptions()
    {
        Debug.Log(options.Count);
        int i = Random.Range(0, options.Count);
        if(options[i].Number != 0)
        {
            MoveTroops(options[i].Begin, options[i].End, options[i].Number);
        }
        
    }

    private void MoveTroops(TerritoryHandler begin, TerritoryHandler end, int number)
    {

        if (number != 0)
        {
            if (begin.territoryStats.territory.TypePlayer != end.territoryStats.territory.TypePlayer)//conquista
            {
                if(gold - warTax >= 0)
                {
                    gold -= warTax;
                    WarManager.instance.SendWarriors(begin, end, number);
                }
            }
            else//movimiento
            {
                WarManager.instance.SendWarriors(begin, end, number);
            }
            
        }
    }

    private void GetGold()
    {
        foreach (TerritoryHandler t in territories)
        {
            gold += t.territoryStats.territory.Gold;
            t.territoryStats.territory.Gold = 0;
        }
    }






}

public class OptionBot
{
    [SerializeField] private TerritoryHandler begin;
    [SerializeField] private TerritoryHandler end;
    [SerializeField] private int number;

    public TerritoryHandler Begin
    {
        get { return begin; }
    }
    public TerritoryHandler End
    {
        get { return end; }
    }
    public int Number
    {
        get { return number; }
    }
    public OptionBot(TerritoryHandler _begin, TerritoryHandler _end, int _number)
    {
        begin = _begin;
        end = _end;
        number = _number;
    }
}
