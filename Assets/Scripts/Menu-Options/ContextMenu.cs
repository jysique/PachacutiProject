using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenu : MonoBehaviour
{
    [SerializeField]TerritoryHandler territoryToAttack;
    public void SetMenu(bool canAttack, TerritoryHandler ta)
    {
        territoryToAttack = ta;
        transform.GetChild(0).GetComponent<Button>().interactable = canAttack;
    }
    public void AttackTerritory()
    {
        InGameMenuHandler.instance.SendWarriors(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>(), territoryToAttack, 1);
        InGameMenuHandler.instance.TurnOffBlock();
        InGameMenuHandler.instance.ChangeStateTerritory(0);
        this.gameObject.SetActive(false);
    }
}
