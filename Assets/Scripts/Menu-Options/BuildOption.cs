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

    public Building TerritoryBuilding
    {
        get { return building; }
        set { building = value; }
    }
    void Start()
    {
        levelTxt = transform.Find("LevelTxt").transform.GetComponent<TextMeshProUGUI>();
        costTxt = transform.Find("CostTxt").transform.GetComponent<TextMeshProUGUI>();
        block = transform.Find("Block").gameObject;
        nameTxt = transform.Find("NameTxt").transform.GetComponent<TextMeshProUGUI>();
        nameTxt2 = block.gameObject.transform.Find("Text").gameObject.GetComponent<TextMeshProUGUI>();
        costTxt = transform.Find("CostTxt").gameObject.GetComponent<TextMeshProUGUI>();
        
        upgradeBtn = this.gameObject.transform.GetChild(7).gameObject.GetComponent<Button>();
        linearBarProgress = this.gameObject.transform.GetChild(5).gameObject.GetComponent<Image>();

    }
    // Update is called once per frame
    void Update()
    {
        nameTxt.text = building.Name;
        nameTxt2.text = "Press '+' To Build " + building.Name;
        levelTxt.text = "Level:" + building.Level.ToString();

        bool a = building.Level > 0 ? false : true;
        block.SetActive(a);
        upgradeBtn.interactable = building.CanUpdrade;
        linearBarProgress.fillAmount = (float)building.DaysTotal / (float)building.DaysToBuild;
    }

}
