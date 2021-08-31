using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class MissionManager : MonoBehaviour
{
    public static MissionManager instance;


    [SerializeField] private ScrollRect scroll;
    [SerializeField] private GameObject notificationMission;
    [SerializeField] private GameObject missionList;
    [HideInInspector] public int indexMission = 0;

    private TimeSimulated timeMission;
    [SerializeField] private List<Mission> listMission = new List<Mission>();
    [SerializeField] private Mission currentMission;
    public void SetNotificationMission(bool active)
    {
        notificationMission.SetActive(active);
    }

    public void ScrollToBottom()
    {
        scroll.normalizedPosition = new Vector2(0, 0);
    }

    private void Awake()
    {
        instance = this;
        SetNotificationMission(false);
    }
    private void Start()
    {
        GameEvents.instance.onMissionTriggerEnter += onMissionEnter;
        //Activa desde el primer segundo del juego las misiones
        Invoke("InitMissions", 2f);
    }
    private void InitMissions()
    {
        GameEvents.instance.MissionTriggerEnter();
        DateTableHandler.instance.PausePlayeButton(true);
        //break;
    }


    private void Update()
    {
        // desde la segunda mision , se activa cada 1 dia despues de completar la mision anterior
        if (timeMission != null && TimeSystem.instance.TimeGame.EqualsDate(timeMission))
        {
            GameEvents.instance.MissionTriggerEnter();
        }
        else
        {
            GameEvents.instance.CustomEventExit();
        }

        if (listMission.Count ==indexMission+1 && listMission[indexMission].MissionStatus == Mission.STATUS.COMPLETE)
        {
            timeMission = new TimeSimulated(TimeSystem.instance.TimeGame);
            timeMission.PlusDays(1);
        }
        if (currentMission != null)
        {
            currentMission.CheckMissionStatus();
        }
    }
    private void onMissionEnter()
    {
        if (indexMission <12)
        {
            Transform gridLayout = missionList.transform.Find("ScrollArea/ScrollContainer/GridLayout").transform;
            GameObject missionOption = Instantiate(Resources.Load("Prefabs/MenuPrefabs/MissionOption")) as GameObject;
            missionOption.transform.SetParent(gridLayout.transform, false);
            missionOption.GetComponent<MissionOption>().InitializeMissionOption(indexMission);
            currentMission = missionOption.GetComponent<MissionOption>().Mission;
            listMission.Add(missionOption.GetComponent<MissionOption>().Mission);
            AlertManager.AlertMission();
            SetNotificationMission(true);
        }
    }
}
