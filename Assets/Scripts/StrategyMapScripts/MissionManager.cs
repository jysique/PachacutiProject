using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;
    [SerializeField]private TimeSimulated timeMission;
    [SerializeField] private Button btnMission;
    [SerializeField] private GameObject notificationMission;
    [SerializeField] private GameObject missionList;
    public int currentMission = 0;
    public List<Mission> listMission = new List<Mission>();
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
        GameEvents.instance.onMissionTriggerEnter += onMissionEnter;
        btnMission.onClick.AddListener(() => CheckByPlayer());
        timeMission = new TimeSimulated(TimeSystem.instance.TimeGame);
        timeMission.PlusDays(2);
    }
    private void Update()
    {
        if (TimeSystem.instance.TimeGame.EqualsDate(timeMission))
        {
            GameEvents.instance.MissionTriggerEnter();
        }
        else
        {
            GameEvents.instance.CustomEventExit();
        }
        if (listMission.Count ==currentMission+1 && listMission[currentMission].MissionStatus == Mission.STATUS.COMPLETE)
        {
            timeMission = new TimeSimulated(TimeSystem.instance.TimeGame);
            timeMission.PlusDays(1);
        }
       // print("l|" + listMission.Count);
    }
    private void CheckByPlayer()
    {
        notificationMission.SetActive(false);
    }
    private void onMissionEnter()
    {
        if (currentMission <9)
        {
            Transform gridLayout = missionList.transform.Find("ScrollArea/ScrollContainer/GridLayout").transform;
            GameObject missionOption = Instantiate(Resources.Load("Prefabs/MenuPrefabs/MissionOption")) as GameObject;
            missionOption.transform.SetParent(gridLayout.transform, false);
            missionOption.GetComponent<MissionOption>().InitializeMissionOption(currentMission);
            listMission.Add(missionOption.GetComponent<MissionOption>().Mission);
            AlertManager.AlertMission();
//            print("mission|" + currentMission);
        }
        
    }


}
