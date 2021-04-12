using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables instance { get; private set; }
    private Governor governorChoosen;
    private string chapterTxt;
    private int velocityGame = 0;
    private int dificultyGame = 0;

    public int VelocityGame
    {
        get { return velocityGame; }
        set { velocityGame = value; }
    }
    public int DificultyGame
    {
        get { return dificultyGame; }
        set { dificultyGame = value; }
    }
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
