using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UnitCombatOption : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI warriorsName;
    [SerializeField] TextMeshProUGUI warriorsCount;
    [SerializeField] TextMeshProUGUI warriorsSpeed;
    [SerializeField] Image timeBar;
    [SerializeField] private UnitCombat unitContainer;
    [SerializeField] private Button disbandButton;
    [SerializeField] private GameObject speed;
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
        warriorsName.text = GameMultiLang.GetTraduction(unitContainer.UnitName);
        if (territorySelected.TypePlayer != Territory.TYPEPLAYER.NONE)
        {
            if (unitContainer.IsAvailable)
            {
                //warriorsCount.text = unitContainer.Quantity + " / " + territorySelected.GetLimit(unitContainer) + GameMultiLang.GetTraduction("units");
                warriorsCount.text = "QUANTITY: " + unitContainer.Quantity;
                disbandButton.gameObject.SetActive(true);
                timeBar.gameObject.SetActive(false);
                speed.SetActive(false);
                stats = "ATTACK: " + unitContainer.Attack + "\n" +
                    "DEFENSE: " + unitContainer.Defense + "\n" +
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
                //CorroboratePopulation(unitContainer);
                UpdateLinearBarProgress();
                toolTip.SetNewInfo("In Progress");
            }
            warriorsSpeed.text = territorySelected.GetSpeed(unitContainer).ToString();
        }
        else
        {
            timeBar.gameObject.SetActive(false);
            disbandButton.gameObject.SetActive(false);
        }
        
    }

    void UpdateLinearBarProgress()
    {
        int diference = TimeSystem.instance.TimeGame.DiferenceDays(unitContainer.TimeInit);
        int hours = (int)TimeSystem.instance.TimeGame.Hour + (24 * diference);
        timeBar.fillAmount = hours / ((float)unitContainer.DaysToCreate * 24);
        int a = (int)(timeBar.fillAmount *100)* unitContainer.Quantity /100;
        unitContainer.InProgress = a;
    }

}
