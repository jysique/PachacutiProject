using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWindow : MonoBehaviour
{
    [SerializeField] private GameObject nextWindow;
    [SerializeField] private GameObject thisWindow;
    [SerializeField] private GameObject options;
    public void ChangeMenuWindow()
    {
        nextWindow.gameObject.SetActive(true);
        thisWindow.gameObject.SetActive(false);
        if (gameObject.name == "NewGameButton")
        {
            options.gameObject.SetActive(true);
        }
        AudioManager.instance.ReadAndPlaySFX("ButtonPush");
    }

    public void CloseWindow()
    {
        thisWindow.gameObject.SetActive(false);
    }

    public void CloseGame()
    {
        print("closing app");
        Application.Quit();
    }
}
