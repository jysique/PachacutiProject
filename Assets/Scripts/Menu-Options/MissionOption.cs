using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class MissionOption : MonoBehaviour
{
   // private GameObject iconMission;
    [SerializeField] private TextMeshProUGUI nameMission;
    [SerializeField] private TextMeshProUGUI descriptionMission;
    [SerializeField] private Toggle toggleMission;
    [SerializeField] private Button goToBtn;
    [SerializeField] private Mission mission;
    private MenuToolTip toolTip;

    private bool init;
    public Mission Mission
    {
        get { return mission; }
        set { mission = value; }
    }
    private void Awake()
    {
        init = false;
    }
    private void Start()
    {
        toolTip = this.GetComponent<MenuToolTip>();
    }
    public void InitializeMissionOption(int option)
    {
        init = true;
        GetMission(option);
        toggleMission.isOn = false;
        //print(mission.NameMission);
        nameMission.text = mission.NameMission;
        descriptionMission.text = mission.Message;
        TerritoryHandler t = TerritoryManager.instance.GetTerritoryHandlerByTerritory(mission.TerritoryMission[0]);
        goToBtn.onClick.AddListener(() => GlobalVariables.instance.CenterCameraToTerritory(t,false));

    }
    /// <summary>
    /// Mision[0] = Pause the game
    /// Mision[1] = Review territory menu
    /// Mision[2] = Improve the Academy building
    /// Mision[3] = Check other territories
    /// Mision[4] = Move 70 units
    /// Mision[5] = Conquist Calca
    /// </summary>
    /// <param name="option"></param>
    private void GetMission(int option)
    {
        switch (option)
        {
            case 0:
                this.mission = new MissionTutorial(0);
                break;
            case 1:
                this.mission = new MissionTutorial(1);
                break;
            case 2:
                this.mission = new MissionTutorial(2);
                break;
            case 3:
                this.mission = new MissionTutorial(3);
                break;
            case 4:
                this.mission = new MissionTutorial(4);
                break;
            case 5:
                this.mission = new MissionTutorial(5);
                break;
            case 6:
                this.mission = new MissionTutorial(6);
                break;
            case 7:
                this.mission = new MissionDefeat();
                break;
            case 8:
                this.mission = new MissionAllBuilds();
                break;
            case 9:
                this.mission = new MissionConquest();
                break;
            case 10:
                this.mission = new MissionExpansion();
                break;
            case 11:
                this.mission = new MissionProtect();
                break;
            default:
                break;
        }
    }
    private void Update()
    {
        if (init == true)
        {
            UpdateStatus();
            toolTip.SetNewInfo(mission.Tooltip);
        }
    }
    /// <summary>
    /// Update status image from mission status
    /// </summary>
    private void UpdateStatus()
    {
        if (mission.MissionStatus != Mission.STATUS.IN_PROGRESS)
        {
            toggleMission.isOn = true;
        }
        if (mission.MissionStatus == Mission.STATUS.DONE)
        {
            init = false;

        }
    }
}
