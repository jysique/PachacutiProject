using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BuildOption : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI costTxt;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI nameBlockTxt;
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private Button upgradeBtn;
    [SerializeField] private Image linearBarProgress;
    [SerializeField] private GameObject block;
    
    private Building territoryBuilding;
    private bool init;
    

    //private 

    public Building TerritoryBuilding
    {
        get { return territoryBuilding; }
        set { territoryBuilding = value; }
    }
    private void Awake()
    {
        init = false;
       // linearBarProgress = this.gameObject.transform.GetChild(5).gameObject.GetComponent<Image>();
    }
    void Start()
    {
        upgradeBtn.onClick.AddListener(() => UpgradeButton());  
    }
    public void InitializeBuildingOption(Building building)
    {
        territoryBuilding = building;
        init = true;
        CheckCost();
        UpdateElements();
    }
    private void CheckCost()
    {
        Territory t = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().TerritoryStats.Territory;
        float s = 0;
        float s2 = 0;
        switch (territoryBuilding.GetType().ToString())
        {
            case "Farm":
                s = t.FarmTerritory.WorkersChannel;
                s2 = s + t.FarmTerritory.AtributteToAdd;
                UploadCost(InGameMenuHandler.instance.GoldPlayer, t.FarmTerritory.CostToUpgrade, "gold", s, s2);
                break;
            case "GoldMine":
                s = t.GoldMineTerritory.WorkersMine;
                s2 = s + t.GoldMineTerritory.AtributteToAdd;
                UploadCost(InGameMenuHandler.instance.GoldPlayer, t.GoldMineTerritory.CostToUpgrade, "gold", s, s2);
                break;
            case "Fortress":
                s = t.FortressTerritory.PlusDefense;
                s2 = s + t.FortressTerritory.AtributteToAdd;
                UploadCost(InGameMenuHandler.instance.GoldPlayer, t.FortressTerritory.CostToUpgrade, "gold", s, s2);
                break;
            case "Academy":
                s = t.AcademyTerritory.SpeedSwordsmen;
                s2 = s + t.AcademyTerritory.AtributteSpeed;
                UploadCost(InGameMenuHandler.instance.GoldPlayer, t.AcademyTerritory.CostToUpgrade, "gold", s, s2);
                break;
            case "Barracks":
                s = t.BarracksTerritory.SpeedLancer;
                s2 = s + t.BarracksTerritory.AtributteSpeed;
                UploadCost(InGameMenuHandler.instance.GoldPlayer, t.BarracksTerritory.CostToUpgrade, "gold", s, s2);
                break;
            case "Castle":
                s = t.CastleTerritory.SpeedAxemen;
                s2 = s + t.CastleTerritory.AtributteSpeed;
                UploadCost(InGameMenuHandler.instance.GoldPlayer, t.CastleTerritory.CostToUpgrade, "gold", s, s2);
                break;
            case "Stable":
                s = t.StableTerritory.SpeedScouts;
                s2 = s + t.StableTerritory.AtributteSpeed;
                UploadCost(InGameMenuHandler.instance.GoldPlayer, t.StableTerritory.CostToUpgrade, "gold", s, s2);
                break;
            default:
                break;
        }
    }
    private void UploadCost(int goldPlayer, int goldNeed, string element, float s, float s2)
    {
        costTxt.text = "Cost -" + goldNeed.ToString() + " " + element + " " + s.ToString("F2") + " -> " + s2.ToString("F2"); ;
        if (goldPlayer >= goldNeed)
        {
            costTxt.color = Color.white;
        }
        else
        {
            costTxt.color = Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (init)
        {
            UpdateLinearBarProgress();
            CheckBlock();
            SetStatus();
            CheckStatusImprove();
            int a = this.transform.GetSiblingIndex();
            territoryBuilding.PositionInGridLayout = a;
        }
    }
    void UpdateElements()
    {
        nameTxt.text = GameMultiLang.GetTraduction(territoryBuilding.Name);
        nameBlockTxt.text = GameMultiLang.GetTraduction("Press") + GameMultiLang.GetTraduction(territoryBuilding.Name);
        levelTxt.text = "(" + territoryBuilding.Level.ToString() + ")";
        
    }
    void UpdateLinearBarProgress()
    {
        if (!territoryBuilding.CanUpdrade)
        {
            int diference = TimeSystem.instance.TimeGame.DiferenceDays(territoryBuilding.TimeInit);
            int hours = (int)TimeSystem.instance.TimeGame.Hour + (24 * diference);
            linearBarProgress.fillAmount = hours / ((float)territoryBuilding.DaysToBuild * 24);
        }
        else
        {
            linearBarProgress.fillAmount = 0;
        }
    }
    void UpgradeButton()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ReadAndPlaySFX("construct");
        }
        InGameMenuHandler.instance.ImproveBuildingButton(territoryBuilding);
        CheckCost();
        UpdateElements();
    }
    void CheckBlock()
    {
        bool a = territoryBuilding.Level > 0 ? false : true;
        block.SetActive(a);
    }
    void CheckStatusImprove()
    {
        MenuToolTip upgradeToolTip = upgradeBtn.GetComponent<MenuToolTip>();
        switch (status)
        {
            case STATE.UPGRADE:
                upgradeBtn.interactable = true;
                upgradeToolTip.SetNewInfo(GameMultiLang.GetTraduction("tooltip1"));
                break;
            case STATE.NOT_RESOURCES:
                upgradeBtn.interactable = false;
                upgradeToolTip.SetNewInfo(GameMultiLang.GetTraduction("tooltip2"));
                break;
            case STATE.PROCESS:
                upgradeBtn.interactable = false;
                upgradeToolTip.SetNewInfo(GameMultiLang.GetTraduction("tooltip3"));
                break;
            default:
                break;
        }
    }
    private void SetStatus()
    {
        if (InGameMenuHandler.instance.GoldPlayer < territoryBuilding.CostToUpgrade)
        {
            status = STATE.NOT_RESOURCES;
        }
        else
        {
            if (territoryBuilding.CanUpdrade)
            {
                status = STATE.UPGRADE;
            }
            else
            {
                status = STATE.PROCESS;
            }
        }
    }
    private STATE status;
    private enum STATE
    {
        UPGRADE,
        NOT_RESOURCES,
        PROCESS
    }
}
