using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
[System.Serializable]
public class Mission
{
    [SerializeField]private string name;
    private string message;
    private string messagePro;  
    [SerializeField]private List<Territory> territoryMission = new List<Territory>();
    private int timeToFinish;
    private int timeBenefitsPassed;
    [SerializeField]private STATUS missionStatus;
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
        //this.timeToFinish = UnityEngine.Random.Range(2, 6);
        this.timeToFinish = 1;
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
            MissionManager.instance.SetNotificationMission(true);
            MissionManager.instance.currentMission++;
            InGameMenuHandler.instance.UpdateMenu();
        }
        else if (missionStatus == STATUS.IN_PROGRESS_BENEFITS)
        {
            FinishBenefits();
            InGameMenuHandler.instance.UpdateMenu();
        }
        else if(missionStatus == STATUS.DONE)
        {
            MissionManager.instance.SetNotificationMission(true);
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
 [System.Serializable]
public class MissionDefeat : Mission
{
    Territory.TYPEPLAYER typePlayer;
    public MissionDefeat()
    {
        this.NameMission = "Defeat Mission";
        this.typePlayer = GlobalVariables.instance.GetRandomTypePlayer();
        TerritoryMission.Add(TerritoryManager.instance.GetTerritoryRandom(this.typePlayer).territoryStats.territory);
        this.Message = "Defeat "+ GlobalVariables.instance.GetPlayerName(typePlayer) + " civilization";
        this.MessagePro = "+2 fortress nivels for " + this.TimeMissionActive + " months";
    }
    public override void CheckMission()
    {
        base.CheckMission();
        List<TerritoryHandler> t = TerritoryManager.instance.GetTerritoriesHandlerByTypePlayer(typePlayer);
        if (t.Count == 0)
        {
            base.MissionStatus = STATUS.COMPLETE;
        }
    }
    public override void InitBenefits()
    {
        base.InitBenefits();
        List<TerritoryHandler> t = TerritoryManager.instance.GetTerritoriesHandlerByTypePlayer(Territory.TYPEPLAYER.PLAYER);
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
            List<TerritoryHandler> t = TerritoryManager.instance.GetTerritoriesHandlerByTypePlayer(Territory.TYPEPLAYER.PLAYER);
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
        Territory.REGION region = GlobalVariables.instance.GetRandomRegion();
        string regionString = region.ToString();
        List<TerritoryHandler> t = TerritoryManager.instance.GetTerritoriesByZoneTerritory(regionString);
        for (int i = 0; i < t.Count; i++)
        {
            this.TerritoryMission.Add(t[i].territoryStats.territory);
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
        TerritoryMission.Add(TerritoryManager.instance.GetTerritoriesHandlerByTypePlayer(Territory.TYPEPLAYER.PLAYER)[0].territoryStats.territory);
        this.Message = "Have all buildings in one territory";
        this.MessagePro = "+20 motivation in all territories \nfor " + this.TimeMissionActive + " months";
    }
    public override void CheckMission()
    {
        base.CheckMission();
        TerritoryMission = TerritoryManager.instance.GetTerritoriesByTypePlayer(Territory.TYPEPLAYER.PLAYER);
        
        bool complete = TerritoryMission.Any(x => x.AllBuilds() == true);
        if (complete)
        {
            base.MissionStatus = STATUS.COMPLETE;
        }
    }
    public override void InitBenefits()
    {
        base.InitBenefits();
        List<TerritoryHandler> t = TerritoryManager.instance.GetTerritoriesHandlerByTypePlayer(Territory.TYPEPLAYER.PLAYER);
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
            List<TerritoryHandler> t = TerritoryManager.instance.GetTerritoriesHandlerByTypePlayer(Territory.TYPEPLAYER.PLAYER);
            for (int i = 0; i < t.Count; i++)
            {
                // this.TerritoryMission.Add(t[i].territoryStats.territory);
                t[i].territoryStats.territory.MotivationPeople -= 20;
            }
        }
    }
}
public class MissionTutorial : Mission
{
    private int count = 0;
    private int optionTutorial;
    public MissionTutorial(int option)
    {
        optionTutorial = option;
        switch (optionTutorial)
        {
            case 0:
                this.NameMission = "Check terrotiries";
                TerritoryMission.Add(TerritoryManager.instance.GetTerritoriesHandlerByName("Calca").territoryStats.territory);
                this.Message = "Click on Calca to see information in that territory";
                break;
            case 1:
                this.NameMission = "Check troops";
                TerritoryMission.Add(TerritoryManager.instance.GetTerritoriesHandlerByName("LaConvencion").territoryStats.territory);
                this.Message = "You can see your troops with right click in LaConvencion";
                break;
            case 2:
                this.NameMission = "Move 10 troops";
                TerritoryMission.Add(TerritoryManager.instance.GetTerritoriesHandlerByName("LaConvencion").territoryStats.territory);
                this.Message = "You can move your troops to another territory";
                break;
            case 3:
                this.NameMission = "Conquist Calca";
                TerritoryMission.Add(TerritoryManager.instance.GetTerritoriesHandlerByName("Calca").territoryStats.territory);
                this.Message = "If you win your first battle you can give more motivation to your people";
                break;
            default:
                break;
        }
        this.MessagePro = "+10 opinion in LaConvencion";
    }

    public override void CheckMission()
    {
        base.CheckMission();
        switch (optionTutorial)
        {
            case 0:
                if (InGameMenuHandler.instance.TerritorySelected == TerritoryMission[0])
                {
                    count++;
                }
                break;
            case 1:
                if (InGameMenuHandler.instance.TerritorySelected == TerritoryMission[0] && 
                    MenuManager.instance.contextMenu.activeSelf == true)
                {
                    count++;
                }
                break;
            case 2:
                if (TerritoryMission[0].Population == 0)
                {
                    count++;
                }
                break;
            case 3:
                if (TerritoryMission[0].TypePlayer == Territory.TYPEPLAYER.PLAYER)
                {
                    count++;
                }
                break;
            default:
                break;

        }
        if (count == 1)
        {
            base.MissionStatus = STATUS.COMPLETE;
        }
    }
    public override void InitBenefits()
    {
        base.InitBenefits();
        TerritoryManager.instance.GetTerritoriesHandlerByName("LaConvencion").territoryStats.territory.MotivationPeople += 10;
      //  Debug.LogError("in progress");
        base.MissionStatus = STATUS.IN_PROGRESS_BENEFITS;
    }
    public override void FinishBenefits()
    {
        base.FinishBenefits();
        if (base.MissionStatus == STATUS.DONE)
        {
            TerritoryManager.instance.GetTerritoriesHandlerByName("LaConvencion").territoryStats.territory.MotivationPeople -= 10;
        }
    }
}