using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenu : MonoBehaviour
{
    [SerializeField]TerritoryHandler territoryToAttack;
    [SerializeField] GameObject warMenu;
    public void SetMenu(bool canAttack, bool isWar, TerritoryHandler ta)
    {
        territoryToAttack = ta;
        transform.GetChild(0).GetComponent<Button>().interactable = canAttack;
        transform.GetChild(1).GetComponent<Button>().interactable = isWar;
    }
    public void AttackTerritory()
    {
        InGameMenuHandler.instance.SendWarriors(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>(), territoryToAttack, 1);
        InGameMenuHandler.instance.TurnOffBlock();
        InGameMenuHandler.instance.ChangeStateTerritory(0);
        this.gameObject.SetActive(false);
    }

    public void ShowWarMenu()
    {
        
        WarManager.instance.selected = territoryToAttack;
        WarManager.instance.SetWarStatus(true);
        warMenu.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
