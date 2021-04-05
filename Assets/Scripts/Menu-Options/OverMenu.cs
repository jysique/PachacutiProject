using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverMenu : MonoBehaviour
{
    private GameObject overMenu;
    private GameObject[] overMenus;
    public void turnOffMenus()
    {
        overMenus = GameObject.FindGameObjectsWithTag("OverMenu");
        foreach(GameObject overMenu in overMenus)
        {
            overMenu.SetActive(false);
        }
        InGameMenuHandler.instance.ChangeStateTerritory(0);
        this.gameObject.SetActive(false);
    }
}
