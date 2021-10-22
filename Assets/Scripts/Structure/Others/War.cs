using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class War
{


    [SerializeField] private TerritoryHandler territoryDefender;
    [SerializeField] private TerritoryHandler territoryAttacker;
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

    public War(TerritoryHandler _territoryDeffender, TerritoryHandler _territoryAttacker, Troop troopAttacker, float speedAttacker, float speedDefender, float _critic1, float _critic2)
    {
        critic1 = _critic1;
        critic2 = _critic2;
        status = 0;
        time1 = 0;
        time2 = 0;
        limit = 1000;
        attackerTroop = troopAttacker;

        warriorsAttackerSpeed = speedAttacker;
        warriorsDefenderSpeed = speedDefender;
        territoryDefender = _territoryDeffender;
        territoryAttacker = _territoryAttacker;
    }

    public float SpeedAttackers
    {
        get { return warriorsAttackerSpeed; }
    }
    public float SpeedDefender
    {
        get { return warriorsDefenderSpeed; }
    }
    public TerritoryHandler TerritoryWar
    {
        get { return territoryDefender; }
        set { territoryDefender = value; }
    }
    public TerritoryHandler TerritoryAttacker
    {
        get { return territoryAttacker; }
        set { territoryAttacker = value; }
    }
    bool isAttackerWin;
    public void UpdateStatus()
    {
        if(status == 1)
        {
            if (territoryDefender.Territory.TypePlayer != Territory.TYPEPLAYER.PLAYER && territoryDefender.Territory.TypePlayer != Territory.TYPEPLAYER.NONE)
            {
                BotManager.instance.DeleteTerritory(territoryDefender.Territory.TypePlayer, territoryDefender);
            }

            if (territoryDefender.Territory.Population <= 0)
            {
                isAttackerWin = true;
            }
            else
            {
                isAttackerWin = false;
            }

            WarManager.instance.FinishWar(territoryDefender, territoryAttacker, isAttackerWin);

            if (WarManager.instance.selectedWar == this)
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
            Battle();
        }
    }
    void Battle()
    {
        if (Random.Range(0, 5000) < critic1)
        {
            criticMod1 = 2;
            WarManager.instance.ShowCritic(this, true);
        }
        else criticMod1 = 1;
        if (Random.Range(0, 5000) < critic2)
        {
            criticMod2 = 2;
            WarManager.instance.ShowCritic(this, false);
        }
        else criticMod2 = 1;
        time1 += (warriorsDefenderSpeed * GlobalVariables.instance.WarSpeed) * criticMod1;
        time2 += (warriorsAttackerSpeed * GlobalVariables.instance.WarSpeed) * criticMod2;
        //WASTAGE ATTACKER
        if (time1 >= limit)
        {
            time1 = 0;
            Attack(attackerTroop);

        }
        //WASTAGE DEFENDER
        if (time2 >= limit)
        {
            time2 = 0;
            Attack(territoryDefender.Territory.ListUnitCombat);
        }
        //terminar
        if (attackerTroop.GetAllNumbersUnit() <= 0 || territoryDefender.Territory.Population <= 0)
        {
            status = 1;
        }
    }
    void Attack(Troop _troop)
    {
        int index = Random.Range(0, _troop.UnitCombats.Count);
        int size = Random.Range(0, 10);
        _troop.UnitCombats[index].Quantity -= size;
        if (_troop.UnitCombats[index].Quantity <= 0)
        {
            _troop.UnitCombats.RemoveAt(index);
        }
    }
}























