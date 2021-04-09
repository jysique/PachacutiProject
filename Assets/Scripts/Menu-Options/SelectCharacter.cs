using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectCharacter : MonoBehaviour
{
    Button select;
    TerritoryHandler t;
    void Start()
    {
        select = GetComponent<Button>();
    }
    public void ChangeSceneButton()
    {
        GlobalVariables.instance.GovernorChoose = CampainManager.instance.GovernorList.GetGovernors(gameObject.name);
        SceneManager.LoadScene(1);
    }
    public void SetTerritoryHandler(TerritoryHandler _t)
    {
        t = _t;
    }
    public void HireMilitaryBossButton()
    {
        MilitarBoss temp = InGameMenuHandler.instance.ml.GetMilitaryBoss(gameObject.name);
        CharacterManager.instance.HireMilitaryBoss(t,temp);
    }
}
