using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
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

    public void CenterCameraToTerritory(TerritoryHandler territoryHandler)
    {
        Camera main = Camera.main;
        Vector3 origin = main.transform.position;
        //  main.transform.position = new Vector3(territoryHandler.transform.position.x - 5, territoryHandler.transform.position.y, -10);
        main.transform.position = new Vector3(territoryHandler.transform.position.x, territoryHandler.transform.position.y, -10);
        Vector3 difference = origin - main.transform.position;
        foreach (GameObject i in TerritoryManager.instance.territoryList)
        {
            i.GetComponent<TerritoryHandler>().TerritoryStats.container.transform.position += difference;
        }
        foreach (GameObject i in InGameMenuHandler.instance.listFloatingText)
        {
            i.GetComponent<Transform>().transform.position += difference;
        }
    }

    public Territory.TYPEPLAYER GetRandomTypePlayer()
    {
        return (Territory.TYPEPLAYER)UnityEngine.Random.Range(1, Enum.GetNames(typeof(Territory.TYPEPLAYER)).Length - 3);
    }
    public Territory.REGION GetRandomRegion()
    {
        return (Territory.REGION)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Territory.REGION)).Length - 1);
    }

    public List<Transform> GetAllChildren(Transform aTransform, List<Transform> aList = null)
    {
        if (aList == null)
            aList = new List<Transform>();
        int start = aList.Count;
        for (int n = 0; n < aTransform.childCount; n++)
            aList.Add(aTransform.GetChild(n));
        for (int i = start; i < aList.Count; i++)
        {
            var t = aList[i];
            for (int n = 0; n < t.childCount; n++)
                aList.Add(t.GetChild(n));
        }
        return aList;
    }
    public void InitDropdown(TMP_Dropdown _dropdown, List<string> _items)
    {
        _dropdown.options.Clear();

        foreach (var item in _items)
        {
            _dropdown.options.Add(new TMP_Dropdown.OptionData()
            {
                //text = item
                text = GameMultiLang.GetTraduction(item)
            }) ;
        }
    }
}
