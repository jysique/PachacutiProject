using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
[System.Serializable]
public class Mission
{
    [SerializeField]private string name;
    [SerializeField]private List<Territory> territoryMission = new List<Territory>();
    [SerializeField] private STATUS missionStatus;
    private string message;
    private string messagePro;
    private string tooltip;
    private int timeToFinish;
    private int timeBenefitsPassed;
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
    public string Tooltip
    {
        get { return tooltip; }
        set { tooltip = value; }
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
      //  this.timeToFinish = UnityEngine.Random.Range(2, 6);
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
            InGameMenuHandler.instance.UpdateMenu();
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
            MissionManager.instance.indexMission++;
            //InGameMenuHandler.instance.UpdateMenu();
        }
        else if (missionStatus == STATUS.IN_PROGRESS_BENEFITS)
        {
            FinishBenefits();
        }
        else if(missionStatus == STATUS.DONE)
        {
            MissionManager.instance.SetNotificationMission(false);
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
    public void GetInfo(Mission mission,int option = -1)
    {
        if (option>=0)
        {
            this.NameMission = GameMultiLang.GetTraduction(mission.GetType().ToString()+option + "_name");
            this.Message = GameMultiLang.GetTraduction(mission.GetType().ToString()+ option + "_message");
            this.tooltip = GameMultiLang.GetTraduction(mission.GetType().ToString() + option + "_tooltip");
        }
        else
        {
            this.NameMission = GameMultiLang.GetTraduction(mission.GetType().ToString() + "_name");
            this.Message = GameMultiLang.GetTraduction(mission.GetType().ToString() + "_message");
        }
        this.MessagePro = GameMultiLang.GetTraduction(mission.GetType().ToString() + "_message_pro");
        this.Message = this.Message.Replace("TERRITORY", TerritoryMission[0].name);
        this.MessagePro = this.MessagePro.Replace("TIME", this.TimeMissionActive.ToString()).Replace("TERRITORY", this.TerritoryMission[0].name);
    }
}
[System.Serializable]
public class MissionTutorial : Mission
{
    private int count = 0;
    private int optionTutorial;

    /// <summary>
    /// Mision[0] = Pause the game
    /// Mision[1] = Review territory menu
    /// Mision[2] = Improve the Academy building
    /// Mision[3] = Check other territories
    /// Mision[4] = Move 70 units
    /// Mision[5] = Conquist Calca
    /// </summary>
    /// <param name="option"></param>
    public MissionTutorial(int option)
    {
        optionTutorial = option;
        switch (optionTutorial)
        {
            case 0:
            case 1:
            case 2:
            case 5:
                TerritoryMission.Add(TerritoryManager.instance.GetTerritoriesHandlerByName("LaConvencion").TerritoryStats.Territory);
                break;
            case 3:
                TerritoryMission.Add(TerritoryManager.instance.GetTerritoriesHandlerByName("LaConvencion").TerritoryStats.Territory);
                TutorialController.instance.TutorialMilitar();
                break;
            case 4:
            case 6:
                TerritoryMission.Add(TerritoryManager.instance.GetTerritoriesHandlerByName("Calca").TerritoryStats.Territory);
                break;
            default:
                break;
        }
        GetInfo(this, option);
    }
    public override void CheckMission()
    {
        base.CheckMission();
        switch (optionTutorial)
        {
            case 0:
                if (DateTableHandler.instance.IsButtonPause == true)
                {
                    count++;
                }
                break;
            case 1:
                if (MenuManager.instance.IsOpen == true)
                {
                    count++;
                }
                break;
            case 2:
                if (InGameMenuHandler.instance.TerritorySelected == TerritoryMission[0] &&
                    TerritoryMission[0].ArcheryTerritory.Level == 1)
                {
                    count++;
                }
                break;
            case 3:
                if (TerritoryMission[0].Archers.Quantity == 10)
                {
                    count++;
                }
                break;
            case 4:
                if (InGameMenuHandler.instance.TerritorySelected == TerritoryMission[0])
                {
                    count++;
                }
                break;
            case 5:
                if (SelectTropsMenu.instance != null && SelectTropsMenu.instance.acumulated >= 70)
                {
                    count++;
                }
                break;
            case 6:
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
            if (optionTutorial < 4)
            {
                TutorialController.instance.TurnOnDialogue();
            }
            if (optionTutorial == 4) //completa la [MISSION 4(Revisar calca)]
            {
                TutorialController.instance.CanMoveUnits = true;
            }
            base.MissionStatus = STATUS.COMPLETE;
        }

    }
    public override void InitBenefits()
    {
        base.InitBenefits();
        TerritoryManager.instance.GetTerritoriesHandlerByName("LaConvencion").TerritoryStats.Territory.MotivationTerritory += 10;
        //  Debug.LogError("in progress");
        base.MissionStatus = STATUS.IN_PROGRESS_BENEFITS;
    }
    public override void FinishBenefits()
    {
        base.FinishBenefits();
        if (base.MissionStatus == STATUS.DONE)
        {
            TerritoryManager.instance.GetTerritoriesHandlerByName("LaConvencion").TerritoryStats.Territory.MotivationTerritory -= 10;
        }
    }
}
public class MissionDefeat : Mission
{
    Territory.TYPEPLAYER typePlayer;
    public MissionDefeat()
    {
        this.typePlayer = GlobalVariables.instance.GetRandomTypePlayer();
        TerritoryMission.Add(TerritoryManager.instance.GetTerritoryRandom(this.typePlayer).TerritoryStats.Territory);
        GetInfo(this);
        this.Message = this.Message.Replace("CIV", GlobalVariables.instance.GetPlayerName(typePlayer));
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
            t[i].TerritoryStats.Territory.FortressTerritory.Level += 2;
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
                t[i].TerritoryStats.Territory.FortressTerritory.Level -= 2;
            }
        }
    }
}
public class MissionConquest : Mission
{
    public MissionConquest()
    {
        TerritoryMission.Add(TerritoryManager.instance.GetTerritoryRandom(Territory.TYPEPLAYER.NONE).TerritoryStats.Territory);
        GetInfo(this);

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
        Territory.REGION region = GlobalVariables.instance.GetRandomRegion();
        string regionString = region.ToString();
        List<TerritoryHandler> t = TerritoryManager.instance.GetTerritoriesByZoneTerritory(region);
        for (int i = 0; i < t.Count; i++)
        {
            this.TerritoryMission.Add(t[i].TerritoryStats.Territory);
        }
        GetInfo(this);
        this.Message =this.Message.Replace("REGION", regionString.ToLower().Replace("_", " "));
        this.Message= this.MessagePro.Replace("REGION", regionString.ToLower().Replace("_", " "));
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
            TerritoryMission[i].FarmTerritory.ImproveBuilding(2);
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
                TerritoryMission[i].FarmTerritory.SubsideBuilding(2);
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
        monthTimePassed = 0;
        TimeSimulated _timeGame = TimeSystem.instance.TimeGame;
        initProtectionTime = new TimeSimulated(_timeGame.Day, _timeGame.Month, _timeGame.Year);
    }
    public MissionProtect()
    {
        SetInitTimeProtection();
        TerritoryMission.Add(TerritoryManager.instance.GetTerritoryRandom(Territory.TYPEPLAYER.PLAYER).TerritoryStats.Territory);
        GetInfo(this);
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
        }
    }
    public override void InitBenefits()
    {
        base.InitBenefits();
        TerritoryMission[0].FarmTerritory.ImproveBuilding(2);
        base.MissionStatus = STATUS.IN_PROGRESS_BENEFITS;
    }
    public override void FinishBenefits()
    {
        base.FinishBenefits();
        if (base.MissionStatus == STATUS.DONE)
        {
            TerritoryMission[0].FarmTerritory.SubsideBuilding(2);
        }
    }
}
public class MissionAllBuilds : Mission
{
    public MissionAllBuilds()
    {
        TerritoryMission.Add(TerritoryManager.instance.GetTerritoriesHandlerByTypePlayer(Territory.TYPEPLAYER.PLAYER)[0].TerritoryStats.Territory);
        GetInfo(this);
    }
    public override void CheckMission()
    {
        base.CheckMission();
        TerritoryMission = TerritoryManager.instance.GetTerritoriesByTypePlayer(Territory.TYPEPLAYER.PLAYER);
        
        bool complete = TerritoryMission.Any(x => x.AllBuildsLevels() == true);
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
            t[i].TerritoryStats.Territory.MotivationTerritory += 20;
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
                // this.TerritoryMission.Add(t[i].TerritoryStats.Territory);
                t[i].TerritoryStats.Territory.MotivationTerritory -= 20;
            }
        }
    }
}
