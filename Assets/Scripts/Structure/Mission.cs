using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public class Mission
{
    [SerializeField]private string name;
    [SerializeField]private string message;
    [SerializeField]private string messagePro;  
    [SerializeField]private List<Territory> territoryMission = new List<Territory>();
    private int timeToFinish;
    private int timeBenefitsPassed;
    private STATUS missionStatus;
    private TimeSimulated timeMission;
    public STATUS MissionStatus
    {
        get { return missionStatus; }
        set { missionStatus = value; }
    }
    public string NameMission
    {
        get { return name; }
        set { name = value; }
    }
    public string Message
    {
        get { return message; }
        set { message = value; }
    }
    public string MessagePro
    {
        get { return messagePro; }
        set { messagePro = value; }
    }
    public List<Territory> TerritoryMission
    {
        get { return territoryMission; }
        set { territoryMission = value; }
    }
    public int TimeMissionActive
    {
        get { return timeToFinish; }
        set { timeToFinish = value; }
    }
    public TimeSimulated TimeMission
    {
        get { return timeMission; }
        set { timeMission = value; }
    }
    public Mission()
    {
        //this.timeToFinish = 1;
        this.timeToFinish = UnityEngine.Random.Range(2, 6);
        this.missionStatus = STATUS.IN_PROGRESS;
    }
    public void GetFinishTimeMission()
    {
        timeMission.PlusMonth(timeToFinish);
    }
    public virtual void CheckMission()
    {
    }
    public virtual void InitBenefits()
    {
        SetInitTimeMission();
    }
    public virtual void FinishBenefits()
    {
        timeBenefitsPassed = (int)TimeSystem.instance.TimeGame.DiferenceDays(timeMission) / 30;
        if (timeToFinish == timeBenefitsPassed)
        {
            this.MissionStatus = STATUS.DONE;
        }
    }
    public void CheckMissionStatus()
    {
        if (missionStatus == STATUS.IN_PROGRESS)
        {
            CheckMission();
        }
        else if (missionStatus == STATUS.COMPLETE)
        {
            InitBenefits();
            MissionManager.instance.NotificationMission.SetActive(true);
            InGameMenuHandler.instance.UpdateMenu();
        }
        else if (missionStatus == STATUS.IN_PROGRESS_BENEFITS)
        {
            FinishBenefits();
            InGameMenuHandler.instance.UpdateMenu();
        }
        else if(missionStatus == STATUS.DONE)
        {
            MissionManager.instance.NotificationMission.SetActive(true);
        }
    }
    void SetInitTimeMission()
    {
        TimeSimulated _timeGame = TimeSystem.instance.TimeGame;
        timeMission = new TimeSimulated(_timeGame.Day, _timeGame.Month, _timeGame.Year);
    }
    public enum STATUS
    {
        IN_PROGRESS, //research the mision
        COMPLETE, // if mission was complete and begin the benefits
        IN_PROGRESS_BENEFITS,
        DONE, // end of benefits
    }
}
 
public class MissionDefeat : Mission
{
    Territory.TYPEPLAYER typePlayer;
    public MissionDefeat()
    {
        this.NameMission = "Defeat Mission";
        this.typePlayer = (Territory.TYPEPLAYER)UnityEngine.Random.Range(1, Enum.GetNames(typeof(Territory.TYPEPLAYER)).Length-2);
        this.Message = "Defeat "+ GlobalVariables.instance.GetPlayerName(typePlayer) + " civilization";
        this.MessagePro = "+2 fortress nivels for " + this.TimeMissionActive + " months";
    }
    public override void CheckMission()
    {
        base.CheckMission();
        List<TerritoryHandler> t = TerritoryManager.instance.GetTerritoriesByTypePlayer(typePlayer);
        if (t.Count == 0)
        {
            base.MissionStatus = STATUS.COMPLETE;
        }
    }
    public override void InitBenefits()
    {
        base.InitBenefits();
        List<TerritoryHandler> t = TerritoryManager.instance.GetTerritoriesByTypePlayer(Territory.TYPEPLAYER.PLAYER);
        for (int i = 0; i < t.Count; i++)
        {
            t[i].territoryStats.territory.FortressTerritory.Level += 2;
        }
        base.MissionStatus = STATUS.IN_PROGRESS_BENEFITS;
    }
    public override void FinishBenefits()
    {
        base.FinishBenefits();
        if (base.MissionStatus == STATUS.DONE)
        {
            List<TerritoryHandler> t = TerritoryManager.instance.GetTerritoriesByTypePlayer(Territory.TYPEPLAYER.PLAYER);
            for (int i = 0; i < t.Count; i++)
            {
                t[i].territoryStats.territory.FortressTerritory.Level -= 2;
            }
        }
    }
}


public class MissionConquest : Mission
{
    public MissionConquest()
    {
        this.NameMission = "Conquest Mission";
        TerritoryMission.Add(TerritoryManager.instance.GetTerritoryRandom(Territory.TYPEPLAYER.NONE).territoryStats.territory);
        this.Message = "Conquer " + TerritoryMission[0].name + " territory";
        this.MessagePro = "+5 exp military chief \nfor " + this.TimeMissionActive + " months";
    }
    public override void CheckMission()
    {
        base.CheckMission();
        if (base.TerritoryMission[0].TypePlayer == Territory.TYPEPLAYER.PLAYER)
        {
            base.MissionStatus = STATUS.COMPLETE;
        }
    }
    public override void InitBenefits()
    {
        base.InitBenefits();
        TerritoryMission[0].MilitarChiefTerritory.Experience += 15;
        base.MissionStatus = STATUS.IN_PROGRESS_BENEFITS;
    }
    public override void FinishBenefits()
    {
        base.FinishBenefits();
        if (base.MissionStatus == STATUS.DONE)
        {
            TerritoryMission[0].MilitarChiefTerritory.Experience -= 10;
        }
    }
}
public class MissionExpansion : Mission
{
    public MissionExpansion()
    {
        this.NameMission = "Expansion Mission";
        Territory.REGION region = (Territory.REGION)UnityEngine.Random.Range(0, Enum.GetNames(typeof(Territory.REGION)).Length -1);
        string regionString = region.ToString();
        List<TerritoryHandler> t = TerritoryManager.instance.GetTerritoriesByZoneTerritory(regionString);
        //Debug.Log("r- "+ region.ToString() +"-t-" +t.Count);
        for (int i = 0; i < t.Count; i++)
        {
            this.TerritoryMission.Add(t[i].territoryStats.territory);
          //Debug.Log(this.TerritoryMission[i].name);
        }
        
        this.Message = "Conquer " + regionString.ToLower().Replace("_", " ");
        this.MessagePro = "+2 irrigation channels nivels in " + regionString.ToLower().Replace("_", " ") + "\n for " + this.TimeMissionActive + " months";
    }
    
    public override void CheckMission()
    {
        base.CheckMission();
        //bool complete = TerritoryMission.All(x => x.TypePlayer == Territory.TYPEPLAYER.PLAYER);
        int a = 0;
        for (int i = 0; i < TerritoryMission.Count; i++)
        {
            if(TerritoryMission[i].TypePlayer == Territory.TYPEPLAYER.PLAYER)
            {
                a++;
            }
        }

        //Debug.Log("bool_:" + a +"-"+ TerritoryMission.Count);
        if (a == TerritoryMission.Count)
        {
            base.MissionStatus = STATUS.COMPLETE;
        }
    }
    public override void InitBenefits()
    {
        base.InitBenefits();
        for (int i = 0; i < TerritoryMission.Count; i++)
        {
            TerritoryMission[i].IrrigationChannelTerritory.ImproveBuilding(2);
        }
        base.MissionStatus = STATUS.IN_PROGRESS_BENEFITS;
    }
    public override void FinishBenefits()
    {
        base.FinishBenefits();
        if (base.MissionStatus == STATUS.DONE)
        {
            for (int i = 0; i < TerritoryMission.Count; i++)
            {
                TerritoryMission[i].IrrigationChannelTerritory.SubsideBuilding(2);
            }
        }
    }
}
public class MissionProtect : Mission
{
    int monthTimePassed;
    TimeSimulated initProtectionTime;
    void SetInitTimeProtection()
    {
        TimeSimulated _timeGame = TimeSystem.instance.TimeGame;
        initProtectionTime = new TimeSimulated(_timeGame.Day, _timeGame.Month, _timeGame.Year);
    }
    public MissionProtect()
    {
        monthTimePassed = 0;
        SetInitTimeProtection();
        TerritoryMission.Add(TerritoryManager.instance.GetTerritoryRandom(Territory.TYPEPLAYER.PLAYER).territoryStats.territory);
        this.NameMission = "Protect Mission";        
        this.Message = "Protect "+ this.TerritoryMission[0].name + " territory for 3 months";
        this.MessagePro = "+2 irrigation channels nivels in "+ this.TerritoryMission[0].name + " territory\nfor " + this.TimeMissionActive + " months";
    }
    public override void CheckMission()
    {
        base.CheckMission();
        if (this.TerritoryMission[0].TypePlayer == Territory.TYPEPLAYER.PLAYER)
        {
            monthTimePassed = TimeSystem.instance.TimeGame.DiferenceDays(initProtectionTime) / 30;
            if (monthTimePassed == 3)
            {
                base.MissionStatus = STATUS.COMPLETE;
            }
        }
        else
        {
            SetInitTimeProtection();
            monthTimePassed = 0;
        }
    }
    public override void InitBenefits()
    {
        base.InitBenefits();
        TerritoryMission[0].IrrigationChannelTerritory.ImproveBuilding(2);
        base.MissionStatus = STATUS.IN_PROGRESS_BENEFITS;
    }
    public override void FinishBenefits()
    {
        base.FinishBenefits();
        if (base.MissionStatus == STATUS.DONE)
        {
            TerritoryMission[0].IrrigationChannelTerritory.SubsideBuilding(2);
        }
    }
}
public class MissionAllBuilds : Mission
{
    public MissionAllBuilds()
    {
        this.NameMission = "All in One";
        this.Message = "Have all buildings in one territory";
        this.MessagePro = "+20 opinion in all territories \nfor " + this.TimeMissionActive + " months";
    }
    public override void CheckMission()
    {
        base.CheckMission();
        List<TerritoryHandler> t = TerritoryManager.instance.GetTerritoriesByTypePlayer(Territory.TYPEPLAYER.PLAYER);
        for (int i = 0; i < t.Count; i++)
        {
            this.TerritoryMission.Add(t[i].territoryStats.territory);
        }
        bool complete = TerritoryMission.Any(x => x.AllBuilds() == true);
        if (complete)
        {
            base.MissionStatus = STATUS.COMPLETE;
        }
    }
    public override void InitBenefits()
    {
        base.InitBenefits();
        List<TerritoryHandler> t = TerritoryManager.instance.GetTerritoriesByTypePlayer(Territory.TYPEPLAYER.PLAYER);
        for (int i = 0; i < t.Count; i++)
        {
            t[i].territoryStats.territory.MotivationPeople += 20;
        }
        base.MissionStatus = STATUS.IN_PROGRESS_BENEFITS;
    }
    public override void FinishBenefits()
    {
        base.FinishBenefits();
        if (base.MissionStatus == STATUS.DONE)
        {
            List<TerritoryHandler> t = TerritoryManager.instance.GetTerritoriesByTypePlayer(Territory.TYPEPLAYER.PLAYER);
            for (int i = 0; i < t.Count; i++)
            {
                // this.TerritoryMission.Add(t[i].territoryStats.territory);
                t[i].territoryStats.territory.MotivationPeople -= 20;
            }
        }
    }
}
