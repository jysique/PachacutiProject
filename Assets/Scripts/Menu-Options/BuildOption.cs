using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BuildOption : MonoBehaviour
{
    private TextMeshProUGUI nameTxt;
    private TextMeshProUGUI levelTxt;
    private TextMeshProUGUI costTxt;
    private Button upgradeBtn;
    private Image linearBarProgress;
    private GameObject block;
    private TextMeshProUGUI nameTxt2;
    private Building building;
    private bool init;

    private MenuToolTip upgradeToolTip;
    public Building TerritoryBuilding
    {
        get { return building; }
        set { building = value; }
    }
    private void Awake()
    {
        init = false;
        levelTxt = transform.Find("LevelTxt").transform.GetComponent<TextMeshProUGUI>();
        costTxt = transform.Find("CostTxt").transform.GetComponent<TextMeshProUGUI>();
        block = transform.Find("Block").gameObject;
        nameTxt = transform.Find("NameTxt").transform.GetComponent<TextMeshProUGUI>();
        nameTxt2 = block.gameObject.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>();
        costTxt = transform.Find("CostTxt").gameObject.GetComponent<TextMeshProUGUI>();

        upgradeBtn = this.gameObject.transform.GetChild(7).gameObject.GetComponent<Button>();
        upgradeToolTip = upgradeBtn.GetComponent<MenuToolTip>();
        linearBarProgress = this.gameObject.transform.GetChild(5).gameObject.GetComponent<Image>();
    }
    void Start()
    {
        upgradeBtn.onClick.AddListener(() => UpgradeButton());  
    }
    public void InitializeBuildingOption(Building building)
    {
        TerritoryBuilding = building;
        init = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (init == true)
        {
            UpdateElements();
            //linearBarProgress.fillAmount = (float)building.DaysTotal / (float)building.DaysToBuild;
            UpdateLinearBarProgress();
            CheckBlock();
            SetStatus();
            CheckStatusImprove();
        }
    }
    void UpdateElements()
    {
        nameTxt.text = GameMultiLang.GetTraduction(building.Name);
        nameTxt2.text = GameMultiLang.GetTraduction("Press") + GameMultiLang.GetTraduction(building.Name);
        levelTxt.text = "Level:" + building.Level.ToString();


      //  upgradeBtn.interactable = building.CanUpdrade;
    }
    void UpdateLinearBarProgress()
    {
        if (!building.CanUpdrade)
        {
            int diference = TimeSystem.instance.TimeGame.DiferenceDays(building.TimeInit);
            int hours = (int)TimeSystem.instance.TimeGame.Hour + (24 * diference);
            linearBarProgress.fillAmount = hours / ((float)building.DaysToBuild * 24);
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
        InGameMenuHandler.instance.ImproveBuildingButton(building);
    }
    void CheckBlock()
    {
        bool a = building.Level > 0 ? false : true;
        block.SetActive(a);
    }
    void CheckStatusImprove()
    {
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
        if (InGameMenuHandler.instance.GoldPlayer < building.CostToUpgrade)
        {
            status = STATE.NOT_RESOURCES;
        }
        else
        {
            if (building.CanUpdrade)
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
