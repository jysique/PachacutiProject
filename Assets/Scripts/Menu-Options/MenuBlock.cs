using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBlock : MonoBehaviour
{
    private void OnEnable()
    {
     //   print("2");
        StartCoroutine(WaitAndActivate(0.2f));
    }
    private IEnumerator WaitAndActivate(float waitTime)
    {
        
        yield return new WaitForSeconds(waitTime);
        InGameMenuHandler.instance.TurnOnBlock();
        InGameMenuHandler.instance.ChangeStateTerritory(2);
     //   print("3");
       
    }
}
