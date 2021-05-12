using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGlobalVariables : MonoBehaviour
{
    public GameObject gvPrefab;
    private void Awake()
    {
      
        if (GlobalVariables.instance == null)
        {

            var prefab = Instantiate(gvPrefab);
            prefab.transform.SetParent(this.transform);
        }
    }
}
