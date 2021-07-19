using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarriorsMoving : MonoBehaviour
{
    //prefab variables
    [SerializeField]private GameObject target;
    //stats
    [SerializeField] private Troop playerTroop;

    [SerializeField]private Territory.TYPEPLAYER territorytype;
    private MilitarChief mb;
    private MilitarChief.TYPESTRAT militaryBossStrategy;
    private int militaryBossExperience;
    [SerializeField]private TerritoryHandler attacker;
    //result
    [SerializeField] private float attackPower;

    public void SetAttack(GameObject _target, Troop _playerTroop, TerritoryHandler attackerTerritory)
    {
        target = _target;

        playerTroop = _playerTroop;
        attacker = attackerTerritory;
        territorytype = attackerTerritory.TerritoryStats.Territory.TypePlayer;
        mb = attackerTerritory.TerritoryStats.Territory.MilitarChiefTerritory;
        //TOTAL DE GUERREROS
        this.transform.GetChild(0).gameObject.GetComponent<TextMeshPro>().text = playerTroop.GetAllNumbersUnit().ToString();

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
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);
        if (Vector3.Distance(transform.position, target.transform.position) < 0.001f)
        {
            if(target.GetComponent<TerritoryHandler>() == TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>())
                MenuManager.instance.overMenuBlock.GetComponent<OverMenu>().turnOffMenus();
            WarManager.instance.MoveWarriors(target.GetComponent<TerritoryHandler>(), attacker,playerTroop);
            if(target.GetComponent<TerritoryHandler>() == WarManager.instance.selected)
            {
                WarManager.instance.SetWarStatus(true);
            }
            Destroy(this.gameObject);
        }
    }
}
