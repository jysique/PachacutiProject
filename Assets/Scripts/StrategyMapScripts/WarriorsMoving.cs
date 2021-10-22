using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarriorsMoving : MonoBehaviour
{
    [SerializeField] private Troop attackerTroop;
    [SerializeField] private TerritoryHandler attackerTerritory;
    [SerializeField] private TerritoryHandler defenderTerritory;
    public void SetAttack(TerritoryHandler _targetTerritory, TerritoryHandler _attackerTerritory, Troop _attackerTroop)
    {
        defenderTerritory = _targetTerritory;
        attackerTroop = _attackerTroop;
        attackerTerritory = _attackerTerritory;
        TextMeshPro text = transform.GetChild(0).gameObject.GetComponent<TextMeshPro>();
        text.text = attackerTroop.GetAllNumbersUnit().ToString();
    }

    void Update()
    {
        MovingWarriors();
    }
    /// <summary>
    /// Move this object and check
    /// if this object reached the target object
    /// </summary>
    private void MovingWarriors()
    {
        float step = GlobalVariables.instance.VelocityMoving * Time.deltaTime;
        GameObject target = defenderTerritory.gameObject;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        if (Vector3.Distance(transform.position, target.transform.position) < 0.001f)
        {
            if(defenderTerritory == TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>())
                MenuManager.instance.overMenuBlock.GetComponent<OverMenu>().turnOffMenus();

            WarManager.instance.MoveWarriors(attackerTerritory, defenderTerritory, attackerTroop);
            if(defenderTerritory == WarManager.instance.selected)
            {
                WarManager.instance.SetWarStatus(true);
            }

            Destroy(this.gameObject);
        }
    }
}
