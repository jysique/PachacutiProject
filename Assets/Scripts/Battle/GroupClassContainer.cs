using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupClassContainer : MonoBehaviour
{
    public UnitGroup stats;

    public void ReceiveDamage() {
        CombatManager.instance.MakeDamage(stats);
        CombatManager.instance.MakeMovement();
    }


}
