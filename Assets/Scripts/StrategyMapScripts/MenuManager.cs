using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public static MenuManager instance;

    [SerializeField] public GameObject contextMenu;
    [SerializeField] public GameObject overMenuBlock;
    [SerializeField] public GameObject toolTip;
    
    [Header("Menu de Pause")]
    [SerializeField] private GameObject PauseMenu;

    [Header("Select MilitaryBoss variables")]
    [SerializeField] GameObject BattlewonMenu;
    

    public static bool isGamePaused = false;
    public float temporalTime;
    
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        InitButtonMenuPause();
    }

    private void PauseMenuGame()
    {
        PauseMenu.SetActive(true);
        PauseGame();
    }
    private void ResumeMenuGame()
    {
        PauseMenu.SetActive(false);
        ResumeGame();
    }
    void InitButtonMenuPause()
    {
        Button[] allPauseButton = PauseMenu.gameObject.transform.GetComponentsInChildren<Button>();

        allPauseButton[0].onClick.AddListener(() => ResumeMenuGame());
        allPauseButton[1].onClick.AddListener(() => GlobalVariables.instance.GoToMenuGame());
        allPauseButton[2].onClick.AddListener(() => GlobalVariables.instance.ClosingApp());
    }
    public void PauseGame()
    {
        turnOffMenus();

        temporalTime = GlobalVariables.instance.timeModifier;
        //print("pause" + temporalTime);
        GlobalVariables.instance.timeModifier = 0;

    }
    public void ResumeGame()
    {
       // print("temporal time" + temporalTime);
        GlobalVariables.instance.timeModifier = temporalTime;
        //        print(temporalTime);
    }
    public void EscapeGame()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                ResumeMenuGame();
            }
            else
            {
                PauseMenuGame();
            }
            //  SceneManager.LoadScene(0);
        }
    }
    public void TurnOffBlock()
    {
        overMenuBlock.SetActive(false);
    }
    public void TurnOnBlock()
    {
        overMenuBlock.SetActive(true);
    }

    public void ActivateContextMenu(TerritoryHandler territoryToAttack, bool canAttack, bool isWar, Vector3 mousePosition)
    {
        TurnOnBlock();
        contextMenu.SetActive(true);
        TerritoryManager.instance.ChangeStateTerritory(2);
        Vector3 mousePosCamera = Camera.main.ScreenToWorldPoint(mousePosition);
        contextMenu.transform.position = new Vector3(mousePosCamera.x, mousePosCamera.y, contextMenu.transform.position.z);
        contextMenu.GetComponent<ContextMenu>().SetMenu(canAttack, isWar, territoryToAttack);
    }

    public void OpenBattleWonMenu(TerritoryHandler territoryHandler)
    {
        BattlewonMenu.SetActive(true);
        BattleWonMenu.instance.InitBattleWonMenu(territoryHandler);
        PauseGame();
    }
    public void CloseBattleWonMenu()
    {
        BattlewonMenu.SetActive(false);
        ResumeGame();
    }

    public void turnOffMenus()
    {
        GameObject[] overMenus;
        overMenus = GameObject.FindGameObjectsWithTag("OverMenu");
        foreach (GameObject overMenu in overMenus)
        {
            overMenu.SetActive(false);
        }
        TerritoryManager.instance.ChangeStateTerritory(0);
    }

    private void Update()
    {
        EscapeGame();
    }
}
