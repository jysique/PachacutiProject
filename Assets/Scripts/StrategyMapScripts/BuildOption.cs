using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuildOption : MonoBehaviour
{
    private Text nameTxt;
    private Text levelTxt;
    private Text costTxt;
    private Button upgradeBtn;
    private Image linearBarProgress;
    private GameObject block;
    private Text nameTxt2;
    private Building building;
    public Building TerritoryBuilding
    {
        get { return building; }
        set { building = value; }
    }
    void Start()
    {
        levelTxt = transform.Find("LevelTxt").transform.GetComponent<Text>();
        costTxt = transform.Find("CostTxt").transform.GetComponent<Text>();
        block = transform.Find("Block").gameObject;
        nameTxt = transform.Find("NameTxt").transform.GetComponent<Text>();
        nameTxt2 = block.gameObject.transform.Find("Text").gameObject.GetComponent<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        nameTxt.text = building.Name;
        nameTxt2.text = "Press '+' To Build " + building.Name;
        levelTxt.text = "Level:" + building.Level.ToString();
       // costTxt.text = building.CostToUpgrade.ToString();
        bool a = building.Level > 0 ? false : true;
        block.SetActive(a);
    }
}
