using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables instance { get; private set; }

    //public string characterChoosen;
    public Governor governorChoosen;
    private void Awake()
    {
        governorChoosen = new Governor();
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
    public Governor GovernorChoose
    {
        get { return governorChoosen; }
        set { governorChoosen = value; }
    }
    public string GetChapterTxt(string action)
    {
        string characterChoosen = governorChoosen.CharacterName;
        return "Chapter" + characterChoosen + "_" + action;
    }
}
