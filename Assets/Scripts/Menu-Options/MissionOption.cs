using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class MissionOption : MonoBehaviour
{
   // private GameObject iconMission;
    private TextMeshProUGUI nameMission;
    private TextMeshProUGUI descriptionMission;
    private Image statusMission;
    [SerializeField]private Mission mission;
    private bool init;
    public Mission Mission
    {
        get { return mission; }
        set { mission = value; }
    }
    private void Awake()
    {
        init = false;
        nameMission = transform.Find("NameTxt").transform.GetComponent<TextMeshProUGUI>();
        descriptionMission = transform.Find("DescriptionTxt").transform.GetComponent<TextMeshProUGUI>();
        statusMission = transform.Find("StateMission").transform.GetComponent<Image>();

    }
    private void Start()
    {
    }
    public void InitializeMissionOption(int option)
    {
        init = true;
        GetMission(option);
        nameMission.text = mission.NameMission;
        descriptionMission.text = mission.Message;
        descriptionMission.GetComponent<MenuToolTip>().AddNewInfo(mission.MessagePro);
        nameMission.GetComponent<MenuToolTip>().AddNewInfo(mission.MessagePro);
        statusMission.GetComponent<MenuToolTip>().AddNewInfo(mission.MessagePro);

    }
    private void GetMission(int option)
    {
        switch (option)
        {
            case 0:
                this.mission = new MissionDefeat();
                break;
            case 1:
                this.mission = new MissionConquest();
                break;
            case 2:
                this.mission = new MissionExpansion();
                break;
            case 3:
                this.mission = new MissionProtect();
                break;
            case 4:
                this.mission = new MissionAllBuilds();
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
        statusMission.GetComponent<MenuToolTip>().SetNewInfo("Mission Status: " + mission.MissionStatus.ToString().ToLower().Replace("_", " "));
        if (mission.MissionStatus == Mission.STATUS.IN_PROGRESS)
        {
            statusMission.color = new Color(0.25f, 0.5f, 1.0f, 1f);
        }
        if (mission.MissionStatus == Mission.STATUS.IN_PROGRESS_BENEFITS)
        {
            statusMission.color = new Color(1.0f, 0f, 0f, 1f);
        }
        else if (mission.MissionStatus == Mission.STATUS.DONE)
        {
            statusMission.color = new Color(0.5f, 1.0f, 0.5f, 1f);
        }
    }
}
