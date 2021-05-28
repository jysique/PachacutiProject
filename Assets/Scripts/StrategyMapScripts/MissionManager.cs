using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;

    [SerializeField] private MissionOption missionDefOption;
    [SerializeField] private MissionOption missionConqOption;
    [SerializeField] private MissionOption missionExpOption;
    [SerializeField] private MissionOption missionProtOption;
    [SerializeField] private MissionOption missionAllBOption;

    [SerializeField] private Button btnMission;
    [SerializeField] private GameObject notificationMission;
    public GameObject NotificationMission
    {
        get { return notificationMission; }
    }
    private void Awake()
    {
        instance = this;
        notificationMission.SetActive(true);
    }
    private void Start()
    {
        missionDefOption.Mission = new MissionDefeat();
        missionDefOption.InitializeMission();
        missionConqOption.Mission = new MissionConquest();
        missionConqOption.InitializeMission();
        missionExpOption.Mission = new MissionExpansion();
        missionExpOption.InitializeMission();
        missionProtOption.Mission = new MissionProtect();
        missionProtOption.InitializeMission();
        missionAllBOption.Mission = new MissionAllBuilds();
        missionAllBOption.InitializeMission();
        btnMission.onClick.AddListener(() => CheckByPlayer());
    }
    private void Update()
    {
        missionDefOption.Mission.CheckMissionStatus();
        missionConqOption.Mission.CheckMissionStatus();
        missionExpOption.Mission.CheckMissionStatus();
        missionProtOption.Mission.CheckMissionStatus();
        missionAllBOption.Mission.CheckMissionStatus();
    }
    private void CheckByPlayer()
    {
        print("hola");
        notificationMission.SetActive(false);
    }
    
}
