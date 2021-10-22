using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    
    private void OnMouseOver()
    {
        bool input = Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(0);
        if (input && CombatManager.instance.isMenu && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            //print("cerrar");
            CombatManager.instance.menu.SetActive(false);
        }
    }
    
}
