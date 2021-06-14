using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ChangeWindow : MonoBehaviour
{
    [SerializeField] private GameObject nextWindow;
    [SerializeField] private GameObject thisWindow;
    public void ChangeMenuWindow()
    {
        nextWindow.gameObject.SetActive(true);
        thisWindow.gameObject.SetActive(false);

        AudioManager.instance.ReadAndPlaySFX("ButtonPush");
    }
    /*
    GovernorList governorList = new GovernorList();
    public void ChangeSceneButton()
    {
        ReadJ();                                                            //BORRAR
        GlobalVariables.instance.GovernorChoose = governorList.Governors[1];//BORRAR
        //SceneManager.LoadScene(1); //LLEVA A LA VISUAL NOVEL
        GlobalVariables.instance.GoToMenuMessage();
    }
    /// <summary>
    /// Funcion para la prueba de prototipo
    /// Delete later
    /// </summary>
    public void ReadJ()
    {
        TextAsset asset = Resources.Load("Data/Menu/Governors") as TextAsset;
        if (asset != null)
        {
            governorList = JsonUtility.FromJson<GovernorList>(asset.text);
            governorList.ChangueCharacIconType();
        }
        else
        {
            print("asset is null");
        }
    }
    /// <summary>
    /// Fin de DeleteLater
    /// </summary>
    public void CloseWindow()
    {
        thisWindow.gameObject.SetActive(false);
    }

    public void CloseGame()
    {
        GlobalVariables.instance.ClosingApp();
    }
    */
}
