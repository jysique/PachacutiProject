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
    [SerializeField] private Image statusMission;
    [SerializeField] private Button goToBtn;
    [SerializeField] private Mission mission;
    
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
    }
    public void InitializeMissionOption(int option)
    {
        init = true;
        GetMission(option);
        toggleMission.isOn = false;
        nameMission.text = mission.NameMission;
        descriptionMission.text = mission.Message;
        TerritoryHandler t = TerritoryManager.instance.GetTerritoryHandlerByTerritory(mission.TerritoryMission[0]);
        goToBtn.onClick.AddListener(() => GlobalVariables.instance.CenterCameraToTerritory(t,false));
        descriptionMission.GetComponent<MenuToolTip>().AddNewInfo(mission.MessagePro);
        nameMission.GetComponent<MenuToolTip>().AddNewInfo(mission.MessagePro);
        statusMission.GetComponent<MenuToolTip>().AddNewInfo(mission.MessagePro);

    }
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
                this.mission = new MissionDefeat();
                break;
            case 6:
                this.mission = new MissionAllBuilds();
                break;
            case 7:
                this.mission = new MissionConquest();
                break;
            case 8:
                this.mission = new MissionExpansion();
                break;
            case 9:
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
            mission.CheckMissionStatus();
        }
    }
    /// <summary>
    /// Update status image from mission status
    /// </summary>
    private void UpdateStatus()
    {
        statusMission.GetComponent<MenuToolTip>().SetNewInfo(GameMultiLang.GetTraduction("MissionStatus") + mission.MissionStatus.ToString().ToLower().Replace("_", " "));
        if (mission.MissionStatus != Mission.STATUS.IN_PROGRESS)
        {
            toggleMission.isOn = true;
        }
    }
}
