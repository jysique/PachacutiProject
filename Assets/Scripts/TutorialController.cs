using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    public static TutorialController instance { get; private set; }

    [SerializeField] private List<GameObject> blocks;
    [SerializeField] private GameObject battle1;
    [SerializeField] private GameObject battle2;


    [SerializeField]private List<string> data = new List<string>();
    [SerializeField]private int progress = 0;
    bool init = false;
    
    [SerializeField] private List<Tutorial> tutorialList = new List<Tutorial>();
    private Tutorial currentTutorial;
    private float timerCountDown;
    private TimeSimulated timeTutorial;

    private bool isFirstTime = false;
    private bool canSpeech = true;
    private bool isCompleted = false;
    private bool canMoveUnits = false; //
    private bool canSelectTroops = false; //
    private bool isAttacking = false;
    int ift;
    public bool IsFirstTime
    {
        get { return isFirstTime; }
    }
    public bool IsAttacking
    {
        get { return isAttacking; }
        set { isAttacking = value; }
    }
    public bool CanMoveUnits
    {
        get { return canMoveUnits; }
        set { canMoveUnits = value; }
    }
    public bool CanSelectTroops
    {
        get { return canSelectTroops; }
        set { canSelectTroops = value; }
    }
    public bool IsTutorialCompleted
    {
        get { return isCompleted; }
        set { isCompleted = value; }
    }
    public List<Tutorial> TutorialList
    {
        get { return tutorialList; }
        set { tutorialList = value; }
    }
    public bool CanSpeech
    {
        get { return canSpeech; }
        set { canSpeech = value; }
    }
    public void TurnBattle1Indication(bool _a)
    {
        
        battle1.SetActive(_a);
    }
    public void TurnBattle2Indication(bool _a)
    {
        battle2.SetActive(_a);
    }
    public void TutorialMilitar()
    {
        blocks[1].SetActive(true);
    }
    public void TurnOffDialogue()
    {
        canSpeech = false;
        ChapterController.instance.speechSystemRoot.SetActive(false);
    }
    [SerializeField] private GameObject move;
    [SerializeField] private GameObject attack;
    [SerializeField] private GameObject deffend;
    public void TurnMoveButton(bool _a)
    {
        move.SetActive(_a);
    }
    public void TurnDefButton(bool _a)
    {
        deffend.SetActive(_a);
    }
    public void TurnAttackButton(bool _a)
    {
        attack.SetActive(_a);
    }
    public void TurnOnDialogue()
    {
        canSpeech = true;
        ChapterController.instance.speechSystemRoot.SetActive(true);
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        //ift = PlayerPrefs.GetInt("tutorialState", 1);
        ift = PlayerPrefs.GetInt("tutorialState", 0);
    }
    private void Start()
    {
        print(ift);
        TurnBattle1Indication(false);
        TurnBattle2Indication(false);
        timeTutorial = new TimeSimulated(TimeSystem.instance.TimeGame);
        timeTutorial.PlusDays(1);
        PlayerPrefs.DeleteAll();
        timerCountDown = 0.8f;
        
        if (ift == 0)
        {
            TurnMoveButton(true);
            TurnDefButton(true);
            TurnAttackButton(true);
        }
        else
        {
            TurnMoveButton(false);
            TurnDefButton(false);
            TurnAttackButton(false);
        }
    }
    
    public void InitTutorial()
    {
        LoadChapterFile("Tutorial");
        TurnOnDialogue();
        tutorialList.Add(new TutorialMissions(0));
        tutorialList.Add(new TutorialMoveTroops(1));
        tutorialList.Add(new TutorialMoveSword(2));
        tutorialList.Add(new TutorialDefendSword(3));
        tutorialList.Add(new TutorialAttackArcher(4));
        HandleLineInTutorial();
        SetNextTutorial(0);
        init = true;
        isFirstTime = true;

    }
    public void CompletedTutorial()
    {
        SetNextTutorial(currentTutorial.Order + 1);
    }

    private void SetNextTutorial(int currentOrder)
    {
        currentTutorial = GetTutorialByOrder(currentOrder);
        if (currentTutorial == null)
        {
            CompleteAllTutorials();
            return;
        }
    }
    private void CompleteAllTutorials()
    {
        isCompleted = true;
        EventManager.instance.InitEvents();
        foreach (var item in blocks)
        {
            item.SetActive(true);
        }
       // isFirstTime = false;
        print("all tutorials completed");
    }

    private Tutorial GetTutorialByOrder(int order)
    {
        for (int i = 0; i < tutorialList.Count; i++)
        {
            if (tutorialList[i].Order == order)
            {
                return tutorialList[i];
            }
        }
        return null;
    }
    private void LoadChapterFile(string fileName)
    {
        string file = Resources.Load<TextAsset>("Data/Dialogue/Tutorial/" + fileName).text;
        data = new List<string>(file.Split('\n'));
        progress = 0;
        ChapterController.instance.cachedLastSpeaker = "";
    }

    private void Update()
    {
        //if (TimeSystem.instance.TimeGame.EqualsDate(timeTutorial))
        if (false)
        {
            if (ift == 1)
            {
                InitTutorial();
            }
        }
        else
        {
            GameEvents.instance.CustomEventExit();
        }
        if (init)
        {
            if (timerCountDown > 0)
            {
                timerCountDown -= Time.deltaTime * 0.5f;
            }
            else
            {
                if (canSpeech)
                {
                    if (progress < data.Count-1)
                    {
                        progress++;
                        timerCountDown = 0.8f;
                        HandleLineInTutorial();
                    }
                    else
                    {
                        isFirstTime = false;
                        TurnOffDialogue();
                        PlayerPrefs.SetInt("tutorialState",0);
                    }
                }
            }
            //canSpeech = time when characterVN is saying something 
            if (currentTutorial != null && !canSpeech)
            {
                currentTutorial.CheckTutorial();
            }
        }
    }
    void HandleLineInTutorial()
    {
        TurnOnDialogue();
        ChapterController.instance.HandleLine(data[progress]);
    }

    public void MoveTroopInTutorial(TerritoryHandler territoryToAttack)
    {
        TerritoryHandler selectedTutorial = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>();
        Swordsman s = new Swordsman();
        s.Quantity = selectedTutorial.TerritoryStats.Territory.Swordsmen.Quantity;
        Archer a = new Archer();
        a.Quantity = selectedTutorial.TerritoryStats.Territory.Archers.Quantity;
        Troop troopSelected = new Troop();
        troopSelected.AddElement(s, 0);
        troopSelected.AddElement(a, 3);
        troopSelected.MoveUnits(selectedTutorial.TerritoryStats.Territory);
        WarManager.instance.SendWarriors(selectedTutorial, territoryToAttack, troopSelected);
    }

}
