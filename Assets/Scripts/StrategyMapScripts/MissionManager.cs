using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;

    [SerializeField] private MissionOption missionDefOption;
    [SerializeField] private MissionOption missionConqOption;
    [SerializeField] private MissionOption missionExpOption;
    [SerializeField] private MissionOption missionProtOption;
    [SerializeField] private MissionOption missionAllBOption;

    private void Awake()
    {
        instance = this;

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
    }
    private void Update()
    {
        missionDefOption.Mission.CheckMissionStatus();
        missionConqOption.Mission.CheckMissionStatus();
        missionExpOption.Mission.CheckMissionStatus();
        missionProtOption.Mission.CheckMissionStatus();
        missionAllBOption.Mission.CheckMissionStatus();
    }
}
