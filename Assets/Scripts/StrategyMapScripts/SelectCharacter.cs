using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCharacter : MonoBehaviour
{
    Button select;
    void Start()
    {
        select = GetComponent<Button>();
        
    }
    public void ChangeSceneButton()
    {
        GlobalVariables.instance.GovernorChoose = CampainManager.instance.GovernorList.GetGovernors(gameObject.name);
        //GlobalVariables.instance.SetChapterTxt("start");
        GlobalVariables.instance.GoToMenuMessage();
    }

    public void ChangeMessageSceneButton()
    {
        GlobalVariables.instance.GoToGame();
    }
}
