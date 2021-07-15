using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class War
{

    [SerializeField] private Territory.TYPEPLAYER attackers;
    [SerializeField] private TerritoryHandler territory;
    [SerializeField]public int warriors1Count;
    [SerializeField]public int warriors2Count;
    [SerializeField]private float warriors1Speed;
    [SerializeField]private float warriors2Speed;
    [SerializeField]private int status;
    [SerializeField]private float time1;
    [SerializeField] private float time2;
    [SerializeField] private float limit;
    [SerializeField] private float critic1;
    [SerializeField] private float critic2;

    private int criticMod1;
    private int criticMod2;

    public War(int c1,float s1,float s2, TerritoryHandler _territoryHandler, Territory.TYPEPLAYER _attackers, float _critic1, float _critic2)
    {
        critic1 = _critic1;
        critic2 = _critic2;
        status = 0;
        time1 = 0;
        time2 = 0;
        limit = 400;
        warriors1Count = c1;
        warriors2Count = _territoryHandler.TerritoryStats.Territory.Population;
        warriors1Speed = s1;
        warriors2Speed = s2;
        territory = _territoryHandler;
        attackers = _attackers;
    }
    public float Speed1
    {
        get { return warriors1Speed; }
    }
    public float Speed2
    {
        get { return warriors2Speed; }
    }
    public Territory.TYPEPLAYER Attackers
    {
        get { return attackers; }
        set { attackers = value; }
    }
    public TerritoryHandler TerritoryWar
    {
        get { return territory; }
        set { territory = value; }
    }

    public void UpdateStatus()
    {
        if(status == 1)
        {
            if (territory.TerritoryStats.Territory.TypePlayer != Territory.TYPEPLAYER.PLAYER && territory.TerritoryStats.Territory.TypePlayer != Territory.TYPEPLAYER.NONE)
            {
                
                BotManager.instance.DeleteTerritory(territory.TerritoryStats.Territory.TypePlayer, territory);
            }

            WarManager.instance.FinishWar(territory,attackers);
            
            if(WarManager.instance.selectedWar == this)
            {
                MenuManager.instance.overMenuBlock.GetComponent<OverMenu>().turnOffMenus();
            }
            try
            {
                WarManager.instance.warList.Remove(this);
            }
            catch (System.Exception)
            {

                throw;
            }
            
        }
        else
        {
            if (Random.Range(0, 5000) < critic1)
            {
                criticMod1 = 2;
                WarManager.instance.ShowCritic(this, true);
            }
            else criticMod1 = 1;
            if (Random.Range(0, 5000) < critic2) { 
                criticMod2 = 2;
                WarManager.instance.ShowCritic(this, false);
            }
            else criticMod2 = 1;
            time1 += (warriors2Speed* GlobalVariables.instance.WarSpeed)*criticMod1;
            time2 += (warriors1Speed* GlobalVariables.instance.WarSpeed)*criticMod2;
            if (time1 >= limit)
            {
                time1 = 0;
                warriors1Count--;
            }
            if (time2 >= limit)
            {
                time2 = 0;
                // territory.TerritoryStats.Territory.Population--;
                if (index == 0)
                {
                    territory.TerritoryStats.Territory.Swordsmen.NumbersUnit--;
                }
                else if (index ==1)
                {
                    territory.TerritoryStats.Territory.Lancers.NumbersUnit--;
                }else if (index == 2)
                {
                    territory.TerritoryStats.Territory.Axemen.NumbersUnit--;
                }
                if (territory.TerritoryStats.Territory.Swordsmen.NumbersUnit == 0)
                {
                    index = 1;
                    if (territory.TerritoryStats.Territory.Lancers.NumbersUnit == 0)
                    {
                        index = 2;
                    }
                }
                warriors2Count = territory.TerritoryStats.Territory.Population;
            }
            if (warriors1Count <= 0 || warriors2Count <= 0) status = 1;
        }
    }
    public int index = 0;
    public TerritoryHandler GetTerritory()
    {
        return territory;
    }
}























