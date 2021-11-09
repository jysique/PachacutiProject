using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{

    private void OnMouseOver()
    {
        
        bool input = Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0);
        /*
        if (input && CombatManager.instance != null)
        {
            print("1" + CombatManager.instance.isMenu);
            print("2" +!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject());
        }
        */


        if (input && CombatManager.instance != null && CombatManager.instance.isMenu && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            CombatManager.instance.menu.SetActive(false);
        }
    }
    
}
