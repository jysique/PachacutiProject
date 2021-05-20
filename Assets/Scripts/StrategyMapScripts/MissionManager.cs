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
        missionConqOption.Mission = new MissionConquest();
        missionExpOption.Mission = new MissionExpansion();
        missionProtOption.Mission = new MissionProtect();
        missionAllBOption.Mission = new MissionAllBuilds();
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
