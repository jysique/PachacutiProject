using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TroopContainerOption : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI warriorsName;
    [SerializeField] TextMeshProUGUI warriorsCount;
    [SerializeField] TextMeshProUGUI warriorsSpeed;
    private UnitCombat unitContainer;
    private bool init = false;
    public void InitializeTroopContainerOption(UnitCombat unit)
    {
        init = true;
        unitContainer = unit;
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
        Territory territorySelected = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().TerritoryStats.Territory;
        warriorsName.text = GameMultiLang.GetTraduction(unitContainer.UnitName);
        warriorsCount.text = unitContainer.NumbersUnit + " / " + territorySelected.GetLimit(unitContainer) + GameMultiLang.GetTraduction("units");
        warriorsSpeed.text = territorySelected.GetSpeed(unitContainer).ToString();
        if (unitContainer.NumbersUnit > territorySelected.GetLimit(unitContainer))
        {
            warriorsCount.color = Color.red;
            warriorsCount.GetComponent<MenuToolTip>().SetNewInfo(GameMultiLang.GetTraduction("tooltip5"));
        }
        else
        {
            warriorsCount.color = new Color32(50, 50, 50, 255);
            warriorsCount.GetComponent<MenuToolTip>().SetNewInfo(GameMultiLang.GetTraduction("tooltip6"));
        }
    }
}
