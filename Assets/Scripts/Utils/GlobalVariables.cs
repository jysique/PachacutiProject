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
    //=======================================================
    //variables de tiempo
    //
    public float timeModifier = 1;

    float velocityMoving;
    float maxTimeCount;
    float timeScale;
    float warSpeed;
    public string charac = "Hakan";
    public string tittle = "Start";

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

        GovernorChoose = new Governor("Pachacuti",14,1,1475);
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
    private float DefinedVelocityMoving()
    {
        return velocityMoving*timeModifier;
    }
    private float DefinedMaxTimeCount()
    {
        return maxTimeCount * timeModifier;
    }
    private float DefinedTimeScale()
    {
        return timeScale * timeModifier;
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
        //print("go to menu game");
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
    /// <summary>
    /// Place the camera on the territory 
    /// </summary>
    /// <param name="territoryHandler">territory</param>
    /// <param name="isSelected">if is outline/selected</param>
    public void CenterCameraToTerritory(TerritoryHandler territoryHandler,bool isSelected)
    {
        Camera main = Camera.main;
        Vector3 origin = main.transform.position;
        main.transform.position = new Vector3(territoryHandler.transform.position.x, territoryHandler.transform.position.y, -10);
        Vector3 difference = origin - main.transform.position;
        
        foreach (GameObject i in TerritoryManager.instance.territoryList)
        {
            //i.GetComponent<TerritoryHandler>().TerritoryStats.container.transform.position += difference;
        }
        
        foreach (GameObject i in InGameMenuHandler.instance.listFloatingText)
        {
            i.GetComponent<Transform>().transform.position += difference;
        }
        
        if (isSelected)
        {
            territoryHandler.MakeOutline();
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
}
