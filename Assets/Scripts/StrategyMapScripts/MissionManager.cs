using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;

    [SerializeField] private MissionOption missionSelectOtherTerritory;
    [SerializeField] private MissionOption missionOpenContextMenu;
    [SerializeField] private MissionOption missionMoveTroops;
    [SerializeField] private MissionOption missionFirstBattle;
    [SerializeField] private MissionOption missionDefOption;
    [SerializeField] private MissionOption missionConqOption;
    [SerializeField] private MissionOption missionExpOption;
    [SerializeField] private MissionOption missionProtOption;
    [SerializeField] private MissionOption missionAllBOption;

    [SerializeField] private Button btnMission;
    [SerializeField] private GameObject notificationMission;
    int a = 0;
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
        btnMission.onClick.AddListener(() => CheckByPlayer());
    }
    public void InitializeMissions()
    {
        missionSelectOtherTerritory.InitializeMissionOption(0);
        missionOpenContextMenu.InitializeMissionOption(1);
        missionMoveTroops.InitializeMissionOption(2);
        missionFirstBattle.InitializeMissionOption(3);
        missionDefOption.InitializeMissionOption(4);
        missionConqOption.InitializeMissionOption(5);
        missionExpOption.InitializeMissionOption(6);
        missionProtOption.InitializeMissionOption(7);
        missionAllBOption.InitializeMissionOption(8);
    }
    private void Update()
    {
    }
    private void CheckByPlayer()
    {
        notificationMission.SetActive(false);
        a++;
        if(a == 1)
        {
            InitializeMissions();
        }
    }
    
}
