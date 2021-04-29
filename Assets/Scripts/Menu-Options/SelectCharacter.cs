using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectCharacter : MonoBehaviour
{
    Button select;
    TerritoryHandler tempTerritoryHandler;
    void Start()
    {
        select = GetComponent<Button>();
    }
    public void ChangeSceneButton()
    {
        GlobalVariables.instance.GovernorChoose = CampainManager.instance.GovernorList.GetGovernors(gameObject.name);
        GlobalVariables.instance.SetChapterTxt("start");
        SceneManager.LoadScene(1); //LLEVA A LA MESSAGE
    }

    public void ChangeMessageSceneButton()
    {
        SceneManager.LoadScene(3); //DEMO LLEVA AL GAMEPLAY
    }
    public TerritoryHandler TerritoryHandler
    {
        get { return tempTerritoryHandler; }
        set { tempTerritoryHandler = value; }
    }
    public void HireMilitaryBossButton()
    {
        MilitarBoss militarBoss = InGameMenuHandler.instance.ml.GetMilitaryBoss(gameObject.name);
        CharacterManager.instance.HireMilitaryBoss(tempTerritoryHandler,militarBoss);
    }
}
