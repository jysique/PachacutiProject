using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables instance { get; private set; }
    private Governor governorChoosen;
    private string chapterTxt;
    private int velocityGame = 1;
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
    public int MaxTimeCount
    {
        get { return DefinedMaxTimeCount(); }
    }
    public int TimeScale
    {
        get { return DefinedTimeScale(); }
    }
    public float VelocityMoving
    {
        get { return DefinedVelocityMoving(); }
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
    private float DefinedVelocityMoving()
    {
        return velocityGame == 0 ? 1.4f : velocityGame == 1 ? 1f : 0.6f;
    }
    private int DefinedMaxTimeCount()
    {
        return velocityGame == 0 ? 2 : velocityGame == 1 ? 6 : 10;
    }
    private int DefinedTimeScale()
    {
        return velocityGame == 0 ? 18 : velocityGame == 1 ? 8 : 4;
    }
}
