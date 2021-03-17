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
    }

    public void CloseGame()
    {
        print("closing app");
        Application.Quit();
    }
}
