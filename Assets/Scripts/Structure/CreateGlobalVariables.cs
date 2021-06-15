using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateGlobalVariables : MonoBehaviour
{
    public GameObject gvPrefab;
    public GameObject langPrefab;
    private void Awake()
    {
      
        if (GlobalVariables.instance == null)
        {

            var gvprefab = Instantiate(gvPrefab);
            var langprefab = Instantiate(langPrefab);
            gvprefab.transform.SetParent(this.transform);
            langprefab.transform.SetParent(this.transform);
        }
    }
}
