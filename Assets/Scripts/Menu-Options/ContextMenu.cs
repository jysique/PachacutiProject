using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenu : MonoBehaviour
{
    [SerializeField]TerritoryHandler clickedTerritory;
    [SerializeField] GameObject warMenu;
    [SerializeField] InputField warriorsCount;
    public void SetMenu(bool canAttack, bool isWar, TerritoryHandler ta)
    {
        
        clickedTerritory = ta;
        warriorsCount.text = ta.territoryStats.territory.Population.ToString();
        transform.GetChild(0).GetComponent<Button>().interactable = canAttack;
        transform.GetChild(0).GetChild(1).gameObject.SetActive(canAttack);
        //transform.GetChild(1).GetComponent<Button>().interactable = isWar;
    }
    public void AttackTerritory()
    {
        TerritoryHandler selected = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
        if (selected == clickedTerritory)
        {
            //InGameMenuHandler.instance.MoveWarriorsButton();
            //InGameMenuHandler.instance.ChangeStateTerritory(2);
            InGameMenuHandler.instance.warriorsCount.text = warriorsCount.text;
            InGameMenuHandler.instance.SelectTerritory();
        }
        if (selected.territoryStats.territory.Population > 0)
        {


            InGameMenuHandler.instance.SendWarriors(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>(), clickedTerritory, 1);
            InGameMenuHandler.instance.TurnOffBlock();
            InGameMenuHandler.instance.ChangeStateTerritory(0);
            this.gameObject.SetActive(false);
        }

    }

    public void ShowStateMenu()
    {
        
        WarManager.instance.selected = clickedTerritory;
        WarManager.instance.SetWarStatus(clickedTerritory.war);
        InGameMenuHandler.instance.ChangeStateTerritory(0);
        InGameMenuHandler.instance.TurnOffBlock();
        this.gameObject.SetActive(false);

    }
}
