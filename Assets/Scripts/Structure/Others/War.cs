using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class War
{

    [SerializeField] private Territory.TYPEPLAYER attackersType;
    [SerializeField] private TerritoryHandler territory;
    [SerializeField] public Troop attackerTroop;

    [SerializeField] private float warriorsAttackerSpeed;
    [SerializeField] private float warriorsDefenderSpeed;
    [SerializeField] private int status;
    [SerializeField] private float time1;
    [SerializeField] private float time2;
    [SerializeField] private float limit;
    [SerializeField] private float critic1;
    [SerializeField] private float critic2;

    private int criticMod1;
    private int criticMod2;
    private int indexAttackers = 0;
    private int indexDefenders = 0;

    public War(Troop troopAttacker, float speedAttacker, float speedDefender, float _critic1, float _critic2, TerritoryHandler _territoryHandler, Territory.TYPEPLAYER _attackers)
    {
        critic1 = _critic1;
        critic2 = _critic2;
        status = 0;
        time1 = 0;
        time2 = 0;
        limit = 400;
        attackerTroop = troopAttacker;

        warriorsAttackerSpeed = speedAttacker;
        warriorsDefenderSpeed = speedDefender;
        territory = _territoryHandler;
        attackersType = _attackers;
    }

    public float SpeedAttackers
    {
        get { return warriorsAttackerSpeed; }
    }
    public float SpeedDefender
    {
        get { return warriorsDefenderSpeed; }
    }
    public Territory.TYPEPLAYER AttackerType
    {
        get { return attackersType; }
        set { attackersType = value; }
    }
    public TerritoryHandler TerritoryWar
    {
        get { return territory; }
        set { territory = value; }
    }

    public TerritoryHandler GetTerritory()
    {
        return territory;
    }
    public void UpdateStatus()
    {
        if(status == 1)
        {
            if (territory.TerritoryStats.Territory.TypePlayer != Territory.TYPEPLAYER.PLAYER && territory.TerritoryStats.Territory.TypePlayer != Territory.TYPEPLAYER.NONE)
            {
                
                BotManager.instance.DeleteTerritory(territory.TerritoryStats.Territory.TypePlayer, territory);
            }

            WarManager.instance.FinishWar(territory,attackersType);
            
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
            time1 += (warriorsDefenderSpeed* GlobalVariables.instance.WarSpeed)*criticMod1;
            time2 += (warriorsAttackerSpeed* GlobalVariables.instance.WarSpeed)*criticMod2;
            //WASTAGE ATTACKER
            if (time1 >= limit)
            {
                time1 = 0;
                for (int i = 0; i < attackerTroop.UnitCombats.Count; i++)
                {
                    if (attackerTroop.UnitCombats[i].Quantity>0 && indexAttackers == i)
                    {
                        attackerTroop.UnitCombats[i].Quantity--;
                    }
                    if (attackerTroop.UnitCombats[i].Quantity == 0)
                    {
                        indexAttackers++;
                    }
                }
            }
            //WASTAGE DEFENDER
            if (time2 >= limit)
            {
                time2 = 0;
                // territory.TerritoryStats.Territory.Population--;
                if (territory.TerritoryStats.Territory.Swordsmen.Quantity == 0)
                {
                    indexDefenders = 1;
                    if (territory.TerritoryStats.Territory.Lancers.Quantity == 0)
                    {
                        indexDefenders = 2;
                        if (territory.TerritoryStats.Territory.Axemen.Quantity == 0)
                        {
                            indexDefenders = 3;
                            if (territory.TerritoryStats.Territory.Scouts.Quantity == 0)
                            {
                                indexDefenders = 4;
                            }
                        }
                    }
                }

                if (indexDefenders == 0 )
                {
                    territory.TerritoryStats.Territory.Swordsmen.Quantity--;
                }else if (indexDefenders ==1)
                {
                    territory.TerritoryStats.Territory.Lancers.Quantity--;
                }else if (indexDefenders == 2)
                {
                    territory.TerritoryStats.Territory.Axemen.Quantity--;
                }else if (indexDefenders == 3)
                {
                    territory.TerritoryStats.Territory.Scouts.Quantity--;
                }else if (indexDefenders == 4)
                {
                    territory.TerritoryStats.Territory.Archers.Quantity--;
                }
               // warriors2Count = territory.TerritoryStats.Territory.Population;
            }
            if (attackerTroop.GetAllNumbersUnit() <= 0 || territory.TerritoryStats.Territory.Population <= 0) status = 1;
        }
    }
 
}























