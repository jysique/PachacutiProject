using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonCustomize : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject label;
    public TYPECUSTOM typecustom;
    private void Start()
    {
        //label.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        //label.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //label.SetActive(false);
    }
    private void Update()
    {
        InstantiateFloatingText(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>());
    }
    private void InstantiateFloatingText(TerritoryHandler t)
    {
        if (typecustom == TYPECUSTOM.UP_LIMIT)
        {
            CheckCost(InGameMenuHandler.instance.FoodPlayer, InGameMenuHandler.instance.FoodNeedLimit, "food");
            float s = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territoryStats.territory.LimitPopulation;
            float s2 = s + 20;
            label.GetComponent<Text>().text += "\n" + s.ToString("F2") + " -> " + s2.ToString("F2");

        }
        else if (typecustom == TYPECUSTOM.UP_SPEED)
        {
            CheckCost(InGameMenuHandler.instance.GoldPlayer, InGameMenuHandler.instance.GoldNeedSpeed, "gold");
            float s = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territoryStats.territory.VelocityPopulation;
            float s2 = s + 0.3f;
            label.GetComponent<Text>().text += "\n" + s.ToString("F2") + " -> " + s2.ToString("F2");
        }
        else if (typecustom == TYPECUSTOM.UP_MINE)
        {
            CheckCost(InGameMenuHandler.instance.GoldPlayer, t.territoryStats.territory.GoldMineTerritory.CostToUpgrade, "gold");
            float s = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territoryStats.territory.GoldMineTerritory.VelocityGold;
            float s2 = s + 0.6f;
            label.GetComponent<Text>().text += "\n" + s.ToString("F2") + " -> " + s2.ToString("F2");
        }
        else if (typecustom == TYPECUSTOM.UP_SACRED)
        {
            CheckCost(InGameMenuHandler.instance.GoldPlayer, t.territoryStats.territory.SacredPlaceTerritory.CostToUpgrade, "gold");
            label.GetComponent<Text>().text += "\n+0.6 motivation";
        }
        else if (typecustom == TYPECUSTOM.UP_FORTRESS)
        {
            CheckCost(InGameMenuHandler.instance.GoldPlayer, t.territoryStats.territory.FortressTerritory.CostToUpgrade, "gold");
            label.GetComponent<Text>().text += "\n+1 fortress level";
        }
        else if (typecustom == TYPECUSTOM.UP_BARRACKS)
        {
            CheckCost(InGameMenuHandler.instance.GoldPlayer, t.territoryStats.territory.BarracksTerritory.CostToUpgrade, "gold");
            label.GetComponent<Text>().text += "\n+1 barracks level";
        }
    }
    private void CheckCost(int goldPlayer, int goldNeed, string element)
    {
        label.GetComponent<Text>().text = "Cost -" + goldNeed.ToString() + " "+ element;
        if (goldPlayer >= goldNeed)
        {
            label.GetComponent<Text>().color = Color.white;
        }
        else
        {
            label.GetComponent<Text>().color = Color.red;
        }
    }
    public enum TYPECUSTOM
    {
        BUTTON,
        ICON,
        UP_LIMIT,
        UP_SPEED,
        UP_MINE,
        UP_SACRED,
        UP_FORTRESS,
        UP_BARRACKS
    }
}
