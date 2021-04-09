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
    public War(int c1,float s1,float s2, TerritoryHandler _territory, Territory.TYPEPLAYER _attackers )
    {
        status = 0;
        time1 = 0;
        time2 = 0;
        warriors1Count = c1;
        warriors2Count = _territory.territoryStats.population;
        warriors1Speed = s1;
        warriors2Speed = s2;
        territory = _territory;
        attackers = _attackers;
    }

    public void UpdateStatus(float t)
    {
        if(status == 1)
        {
            WarManager.instance.FinishWar(territory,attackers);
        }
        else
        {
            time1 += t;
            time2 += t;
            if (time1 >= warriors1Speed)
            {
                time1 = 0;
                warriors1Count--;
                
            }
            if (time2 >= warriors2Speed)
            {
                time2 = 0;
                territory.territoryStats.population--;
                warriors2Count = territory.territoryStats.population;
            }
            if (warriors1Count == 0 || warriors2Count == 0) status = 1;
        }
        

    }
    public TerritoryHandler GetTerritory()
    {
        return territory;
    }
}























