using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeBattle : MonoBehaviour
{
    public TerritoryHandler mainTerritory;
    public TerritoryHandler attackedTerritory;
    


    public void Battle()
    {
        PlayerPrefs.SetInt("tutorialState", 0);
        Troop attackTroop = new Troop(50,50,50);
        //Troop attackTroop = new Troop(mainTerritory.TerritoryStats.Territory.TroopDefending);
        WarManager.instance.MoveWarriors(attackedTerritory,mainTerritory,attackTroop);
    }
        
}

    

