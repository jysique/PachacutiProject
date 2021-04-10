using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables instance { get; private set; }
    public string rhythmGame;
    public Governor governorChoosen;
    private string chapterTxt;
    public int velocityGame = 0;
    public int dificultyGame = 0;
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
    public Governor GovernorChoose
    {
        get { return governorChoosen; }
        set { governorChoosen = value; }
    }
    public string ChapterTxt
    {
        get { return chapterTxt; }
        set { chapterTxt = value; }
    }
    public void SetChapterTxt(string action)
    {
        chapterTxt = "Chapter" + governorChoosen.CharacterName + "_" + action;
    }
}
