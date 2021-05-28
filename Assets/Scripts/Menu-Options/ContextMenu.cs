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
    [SerializeField] GameObject buttonBlock;
    [SerializeField] TabGroup tabManager;
    [SerializeField] TabButton tabMilitar;
    [SerializeField] TabButton tabTerritory;

    public int WarriorsCount()
    {
        int res = int.Parse(warriorsCount.text);
        return res;
    }

    private void Update()
    {
        moveButton.GetComponent<MenuToolTip>().SetNewInfo("This button let you move the especified\n" +
                                                          "quantity of units to another territory\n" +
                                                          "the cost is " + WarriorsCount()+ " of gold\n" +
                                                          "if is an enemy territory");
        
    }
    public void SetMenu(bool canAttack, bool isWar, TerritoryHandler ta)
    {
        int limit = ta.territoryStats.territory.Population;
        clickedTerritory = ta;
        warriorsCount.text = limit.ToString();
        moveButton.interactable = canAttack;
        if (!canAttack)
        {
            buttonBlock.SetActive(true);
        }
        else
        {
            buttonBlock.SetActive(false);
        }
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
        
        if (selected.territoryStats.territory.Population > 0)
        {

            WarManager.instance.SelectTerritory(int.Parse(warriorsCount.text));
            

            MenuManager.instance.TurnOffBlock();
            TerritoryManager.instance.ChangeStateTerritory(0);
            this.gameObject.SetActive(false);
        }
        

    }

    public void ShowMilitarMenu()
    {
        tabManager.OnTabSelected(tabMilitar);
        MenuManager.instance.TurnOffBlock();
        TerritoryManager.instance.ChangeStateTerritory(0);
        this.gameObject.SetActive(false);
    }
    public void ShowTerritoryMenu()
    {
        tabManager.OnTabSelected(tabTerritory);
        MenuManager.instance.TurnOffBlock();
        TerritoryManager.instance.ChangeStateTerritory(0);
        this.gameObject.SetActive(false);
    }
    
}
