using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class MissionOption : MonoBehaviour
{
   // private GameObject iconMission;
    private Text nameMission;
    private Text descriptionMission;
    private Image statusMission;
    [SerializeField]private Mission mission;
    public Mission Mission
    {
        get { return mission; }
        set { mission = value; }
    }
    public void InitializeMission()
    {
        nameMission = transform.Find("NameTxt").transform.GetComponent<Text>();
        descriptionMission = transform.Find("DescriptionTxt").transform.GetComponent<Text>();
        descriptionMission.GetComponent<MenuToolTip>().SetNewInfo(mission.MessagePro);
        statusMission = transform.Find("StateMission").transform.GetComponent<Image>();
        nameMission.text = mission.NameMission;
        descriptionMission.text = mission.Message;
    }
    private void Update()
    {
        UpdateStatus();
    }
    /// <summary>
    /// Update status image from mission status
    /// </summary>
    private void UpdateStatus()
    {
        statusMission.GetComponent<MenuToolTip>().SetNewInfo("Mission Status: " + mission.MissionStatus.ToString().ToLower().Replace("_", " "));
        if (mission.MissionStatus == Mission.STATUS.COMPLETE)
        {
            statusMission.color = new Color(0f, 0f, 0f, 1f);
        }
        else if (mission.MissionStatus == Mission.STATUS.DONE)
        {
            statusMission.color = new Color(0.5f, 0.5f, 0.5f, 1f);
        }
    }
}
