using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TutorialController : MonoBehaviour
{
    public static TutorialController instance { get; private set; }

    [SerializeField] private List<GameObject> blocks;
    [SerializeField] private GameObject move;
    [SerializeField] private GameObject attack;
    [SerializeField] private GameObject deffend;
    [SerializeField] Button finishTutorial;
    [SerializeField]private int progress = 0;
    [SerializeField] private List<string> data = new List<string>();
    [SerializeField] private List<Tutorial> tutorialList = new List<Tutorial>();
    bool init = false;
    private Tutorial currentTutorial;
    private float timerCountDown;
    private TimeSimulated timeTutorial;
    private bool canSpeech = true;
    private bool isFirstTime = false;
    private bool isCompleted = false;
    private bool canMoveUnits = false;
    private bool canSelectTroops = false;
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
    public void TutorialMilitar()
    {
        blocks[1].SetActive(true);
    }
    public void TurnOffDialogue()
    {
        canSpeech = false;
      //  ChapterController.instance.speechSystemRoot.SetActive(false);
      //  ChapterController.instance.backgroundRoot.SetActive(false);

    }
    public void TurnOnDialogue()
    {
        canSpeech = true;
      //  ChapterController.instance.speechSystemRoot.SetActive(true);
      //  ChapterController.instance.backgroundRoot.SetActive(true);
    }
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
        // 0 no tutorial 
        // 1 begin tutorial
        // PlayerPrefs.DeleteAll();
        ift = PlayerPrefs.GetInt("tutorialState",0);

        if (ift == 1)
        {
            Invoke("InitTutorial", 2);
        }
    }

    private void Start()
    {
        print(ift);
        timeTutorial = new TimeSimulated(TimeSystem.instance.TimeGame);
        timeTutorial.PlusDays(1);
        timerCountDown = 0.8f;
        if (ift == 0) // no tutorial
        {
            TurnMoveButton(true);
            TurnDefButton(true);
            TurnAttackButton(true);
            TurnBlocks(true);

            canMoveUnits = true;
            canSelectTroops = true;
            isFirstTime = false;
            EventManager.instance.InitEvents();
        }
        else// so tutorial
        {
            TurnMoveButton(false);
            TurnDefButton(false);
            TurnAttackButton(false);
            TurnBlocks(false);

            isFirstTime = false;
            canMoveUnits = false;
            canSelectTroops = false;
            isFirstTime = true;
        }
    }
    public void InitTutorial()
    {
        ChapterController.instance.speechSystemButtons.SetActive(false);
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
    private void TurnBlocks(bool _a)
    {
        foreach (var item in blocks)
        {
            item.SetActive(_a);
        }
    }
    private void CompleteAllTutorials()
    {
        isCompleted = true;
        EventManager.instance.InitEvents();
        TurnBlocks(true);
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

    void HandleLineInTutorial()
    {
        TurnOnDialogue();
        ChapterController.instance.HandleLine(data[progress]);
    }

    public void MoveTroopInTutorial(TerritoryHandler territoryToAttack)
    {
        //TODO
        //WarManager.instance.SendWarriors(selectedTutorial, territoryToAttack, troopSelected);
    }

}
