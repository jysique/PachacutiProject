using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;


public class GroupClassContainer : MonoBehaviour
{
    public UnitGroup stats;
    public bool active;
    public void OpenResume()
    {
        CombatManager.instance.MakeBattleResume(CombatManager.instance.ActualUnit(), stats, this);
    }

    public void ReceiveDamage() {
        StartCoroutine(CombatManager.instance.MakeDamage(CombatManager.instance.ActualUnit(),stats));
    }

    private void OnMouseOver()
    {

        if (Input.GetMouseButtonDown(1) && stats.Active == true && !CombatManager.instance.blockScreen)
        {

            if (stats.TypePlayer == Territory.TYPEPLAYER.PLAYER)
            {
                CombatManager.instance.selectedUnit = stats;
                Vector3 newpos = stats.UnitsGO.transform.position;
                CombatManager.instance.menu.transform.position = new Vector3(newpos.x + 1, newpos.y - 1, newpos.z);
            }
        }
        
    }
    private void Update()
    {

        //active = stats.Active;
    }

}
