using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenu : MonoBehaviour
{
    [SerializeField] TerritoryHandler clickedTerritory;
    [SerializeField] Text warriorsCount;
    [SerializeField] Button increase;
    [SerializeField] Button decrease;
    [SerializeField] Button moveButton;
    [SerializeField] TabGroup tabManager;
    [SerializeField] TabButton tabMilitar;
    [SerializeField] TabButton tabTerritory;
    public void SetMenu(bool canAttack, bool isWar, TerritoryHandler ta)
    {
        int limit = ta.territoryStats.territory.Population;
        clickedTerritory = ta;
        warriorsCount.text = limit.ToString();
        moveButton.interactable = canAttack;
        increase.interactable = canAttack;
        increase.GetComponent<NumericButton>().limit = limit;
        increase.GetComponent<NumericButton>().lockButton = canAttack;
        increase.GetComponent<NumericButton>().pointerDown = false;
        decrease.interactable = canAttack;
        decrease.GetComponent<NumericButton>().limit = limit;
        decrease.GetComponent<NumericButton>().lockButton = canAttack;
        decrease.GetComponent<NumericButton>().pointerDown = false;
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

    public void ShowMilitarMenu()
    {
        tabManager.OnTabSelected(tabMilitar);
        InGameMenuHandler.instance.TurnOffBlock();
        InGameMenuHandler.instance.ChangeStateTerritory(0);
        this.gameObject.SetActive(false);
    }
    public void ShowTerritoryMenu()
    {
        tabManager.OnTabSelected(tabTerritory);
        InGameMenuHandler.instance.TurnOffBlock();
        InGameMenuHandler.instance.ChangeStateTerritory(0);
        this.gameObject.SetActive(false);
    }
    
}
