using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectCharacter : MonoBehaviour
{
    Button select;
    TerritoryHandler tempTerritory;
    void Start()
    {
        select = GetComponent<Button>();
    }
    public void ChangeSceneButton()
    {
        GlobalVariables.instance.GovernorChoose = CampainManager.instance.GovernorList.GetGovernors(gameObject.name);
        GlobalVariables.instance.SetChapterTxt("start");
        SceneManager.LoadScene(1);
    }
    public void SetTerritoryHandler(TerritoryHandler territory)
    {
        tempTerritory = territory;
    }
    public void HireMilitaryBossButton()
    {
        MilitarBoss militarBoss = InGameMenuHandler.instance.ml.GetMilitaryBoss(gameObject.name);
        CharacterManager.instance.HireMilitaryBoss(tempTerritory,militarBoss);
    }
}
