using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectCharacter : MonoBehaviour
{
    Button select;

    void Start()
    {
        select = GetComponent<Button>();
        select.onClick.AddListener(() => ChangeScene());
    }
    void ChangeScene()
    {
        // SceneManager.LoadScene(2);
        // GlobalVariables.instance.characterChoosen = gameObject.name;
        GlobalVariables.instance.GovernorChoose = CampainManager.instance.GovernorList.GetGovernors(gameObject.name);
        SceneManager.LoadScene("VisualNovelScene");
    }
}
