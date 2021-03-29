using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables instance { get; private set; }

    public string characterChoosen;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public string GetChapterTxt(string action)
    {
        return "Chapter" + characterChoosen + "_" + action;
    }
}
