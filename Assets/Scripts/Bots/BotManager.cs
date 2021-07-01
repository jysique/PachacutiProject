using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotManager : MonoBehaviour
{
    public List<BotIA> bots;
    public static BotManager instance;
    private void Start()
    {
        CreateBots();
    }

    private void Awake()
    {
        instance = this;
    }

    private void FixedUpdate()
    {
        foreach (BotIA bot in bots)
        {
            bot.DoAction();
        }
    }

    private void CreateBots()
    {
        foreach(GameObject t in TerritoryManager.instance.territoryList )
        {
            TerritoryHandler th = t.GetComponent<TerritoryHandler>();
            if(th.TerritoryStats.Territory.TypePlayer != Territory.TYPEPLAYER.PLAYER && th.TerritoryStats.Territory.TypePlayer != Territory.TYPEPLAYER.NONE)
            {
                CreateOrAdd(th.TerritoryStats.Territory.TypePlayer, th);
            }
        }
    }

    public void CreateOrAdd(Territory.TYPEPLAYER type, TerritoryHandler t)
    {
        foreach(BotIA bot in bots)
        {
            if(bot.TypeBot == type)
            {
                bot.AddTerritory(t);
                return;
            }
        }
        BotIA newBot = new BotIA(type,500,500);
        newBot.AddTerritory(t);
        bots.Add(newBot);
       
      
    }

    public void DeleteTerritory(Territory.TYPEPLAYER type, TerritoryHandler t)
    {
        foreach (BotIA bot in bots)
        {
            if (bot.TypeBot == type)
            {
                bot.DeleteTerritory(t);
            }
        }
    }

}
