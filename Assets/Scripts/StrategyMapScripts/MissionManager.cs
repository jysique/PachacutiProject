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
        missionAllBOption.InitializeMissionOption(4);
        missionDefOption.InitializeMissionOption(0);
        missionConqOption.InitializeMissionOption(1);
        missionExpOption.InitializeMissionOption(2);
        missionProtOption.InitializeMissionOption(3);
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
