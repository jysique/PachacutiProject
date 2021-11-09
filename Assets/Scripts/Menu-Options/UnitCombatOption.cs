using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UnitCombatOption : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI warriorsName;
    [SerializeField] TextMeshProUGUI warriorsCount;
    [SerializeField] TextMeshProUGUI warriorsExperience;
    [SerializeField] TextMeshProUGUI warriorsSpeed;
    [SerializeField] Image timeBar;
    [SerializeField] private UnitCombat unitContainer;
    [SerializeField] private Button disbandButton;
    [SerializeField] private GameObject speed;
    [SerializeField] private GameObject block;
    private bool init = false;
    private MenuToolTip toolTip;
    string stats;
    private void Start()
    {
        toolTip = GetComponent<MenuToolTip>();
    }
    public void InitializeTroopContainerOption(UnitCombat unit)
    {
        init = true;

        unitContainer = unit;
        disbandButton.onClick.AddListener(() => DisbandUnit());
    }

    void DisbandUnit()
    {
        InGameMenuHandler.instance.TerritorySelected.ListUnitCombat.DeleteUnitCombat(unitContainer);
        InGameMenuHandler.instance.UpdateMenu();
    }
    private void Update()
    {
        
        if (init)
        {
            UpdateElements();
        }

    }
    public void UpdateElements()
    {
        Territory territorySelected = InGameMenuHandler.instance.TerritorySelected;
        //warriorsName.text = GameMultiLang.GetTraduction(unitContainer.CharacterName) + "("+ unitContainer.Quantity + ")";
        warriorsName.text = unitContainer.CharacterName + "(" + unitContainer.Quantity + ")";
        if (territorySelected.TypePlayer != Territory.TYPEPLAYER.NONE)
        {

            if (territorySelected.GetBuildingByUnit(unitContainer).Level<1)
            {
                block.SetActive(true);
                block.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Build a \n" + territorySelected.GetBuildingByUnit(unitContainer).Name;
            }
            else
            {
                block.SetActive(false);
            }
            if (unitContainer.IsAvailable)
            {
                warriorsExperience.text = "EXPERIENCE:" + unitContainer.Experience;
                disbandButton.gameObject.SetActive(true);
                timeBar.gameObject.SetActive(false);
                speed.SetActive(false);
                stats = "EXP:" + unitContainer.Experience +"/100" +"\n"+ 
                    "ATTACK: " + unitContainer.Attack + "\n" +
                    "ARMOR: " + unitContainer.Armor + "\n" +
                    "EVASION: " + unitContainer.Evasion + "\n" +
                    "PRESICION: " + unitContainer.Precision + "\n";
                toolTip.SetNewInfo(stats);
            }
            else
            {
                warriorsCount.text = unitContainer.InProgress + " / " + unitContainer.Quantity;
                disbandButton.gameObject.SetActive(false);
                timeBar.gameObject.SetActive(true);
                speed.SetActive(true);
                UpdateLinearBarProgress();
                toolTip.SetNewInfo("In Progress");
            }
            warriorsSpeed.text = territorySelected.GetBuildingByUnit(unitContainer).SpeedUnits.ToString();
        }
        else
        {
            timeBar.gameObject.SetActive(false);
            disbandButton.gameObject.SetActive(false);
        }
        
    }

    void UpdateLinearBarProgress()
    {
        timeBar.fillAmount = (float)unitContainer.InProgress / (float)unitContainer.Quantity;
    }

    void BlockUnit()
    {
        
    }
}
