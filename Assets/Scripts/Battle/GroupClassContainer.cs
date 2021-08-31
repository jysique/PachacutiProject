using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupClassContainer : MonoBehaviour
{
    public UnitGroup stats;
    public void ReceiveDamage() {
        CombatManager.instance.MakeDamage(CombatManager.instance.ActualUnit(),stats);
        // CombatManager.instance.MakeDamage(stats, CombatManager.instance.ActualUnit());
        //CombatManager.instance.MakeMovement();
    }


}
