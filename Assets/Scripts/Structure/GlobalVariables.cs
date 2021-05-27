using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GlobalVariables : MonoBehaviour
{
    public static GlobalVariables instance { get; private set; }
    private Governor governorChoosen;
    private string chapterTxt;
    private int velocityGame = 2;
    // 0 = slow
    // 1 = normal
    // 2 = fast
    private int dificultyGame = 0;

    public GameObject canvas;

    //=======================================================
    //variables de tiempo
    //
    public float timeModifier = 1;

    float velocityMoving;
    float maxTimeCount;
    float timeScale;
    float warSpeed;

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
    public float MaxTimeCount
    {
        get { return DefinedMaxTimeCount(); }
    }
    public float TimeScale
    {
        get { return DefinedTimeScale(); }
    }
    public float VelocityMoving
    {
        get { return DefinedVelocityMoving(); }
    }
    
    public float WarSpeed
    {
        get { return DefinedWarSpeed(); }
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


        GovernorChoose = new Governor("Pachacuti");
        GovernorChoose.TimeInit = new TimeSimulated(14,1,1474);
        canvas = GameObject.Find("Canvas");
        velocityMoving = 0.3f;
        maxTimeCount = 12;
        timeScale = 4;
        warSpeed = 1;

        
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
        return velocityMoving*timeModifier;
        //return velocityGame == 0 ? 1.4f : velocityGame == 1 ? 1f : 0.6f;
    }
    private float DefinedMaxTimeCount()
    {
        return maxTimeCount * timeModifier;
       // return velocityGame == 0 ? 2 : velocityGame == 1 ? 6 : 10;
    }
    private float DefinedTimeScale()
    {
        return timeScale * timeModifier;
        //return velocityGame == 0 ? 18 : velocityGame == 1 ? 8 : 4;
    }
    private float DefinedWarSpeed()
    {
        return warSpeed * timeModifier;
    }

    public string GetPlayerName(Territory.TYPEPLAYER type)
    {
        switch (type)
        {
            case Territory.TYPEPLAYER.PLAYER:
                return "Incas";
            case Territory.TYPEPLAYER.BOT:
                return "Chancas";
            case Territory.TYPEPLAYER.BOT2:
                return "Mochica";
            case Territory.TYPEPLAYER.BOT3:
                return "Chavin";
            case Territory.TYPEPLAYER.BOT4:
                return "Pending";
            case Territory.TYPEPLAYER.NONE:
                return "No Empire";
            default:
                break;
        }
        return null;
    }
   

    public void ClosingApp()
    {
        print("closing app");
        Application.Quit();
    }
    public void GoToMenuGame()
    {
        if(InGameMenuHandler.instance != null)
        {
            timeModifier = MenuManager.instance.temporalTime;//aca cambiarlo para que sea una funcion
        }
        print("go to menu game");
        Transition.instance.LoadScene(0);
    }
    public void GoToMenuMessage()
    {
        Transition.instance.LoadScene(1); //DEMO LLEVA AL GAMEPLAY
    }
    public void GoToGame()
    {
        Transition.instance.LoadScene(3);
    }
    public void GoToVisualNovel()
    {
        Transition.instance.LoadScene(2);
    }
}
