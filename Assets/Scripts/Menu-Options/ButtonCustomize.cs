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
        InstantiateFloatingText(TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territoryStats.territory);
    }
    private void InstantiateFloatingText(Territory t)
    {
        switch (typecustom)
        {
            case TYPECUSTOM.UP_LIMIT:
                
                float s = TerritoryManager.instance.territorySelected.GetComponent<TerritoryHandler>().territoryStats.territory.LimitPopulation;
                float s2 = s + 20;
                CheckCost(InGameMenuHandler.instance.FoodPlayer, t.IrrigationChannelTerritory.CostToUpgrade, "food",s,s2);
                break;
            case TYPECUSTOM.UP_SPEED:
                s = t.VelocityPopulation;
                s2 = s + 0.3f;
                CheckCost(InGameMenuHandler.instance.GoldPlayer, t.CostPopulation, "gold",s,s2);
                break;
            case TYPECUSTOM.UP_CHANNEL:
                s = t.IrrigationChannelTerritory.VelocityFood;
                s2 = s + t.IrrigationChannelTerritory.AtributteToAdd;
                CheckCost(InGameMenuHandler.instance.GoldPlayer, t.IrrigationChannelTerritory.CostToUpgrade, "gold",s,s2);
                break;
            case TYPECUSTOM.UP_MINE:
                s = t.GoldMineTerritory.VelocityGold;
                s2 = s + t.GoldMineTerritory.AtributteToAdd;
                CheckCost(InGameMenuHandler.instance.GoldPlayer, t.GoldMineTerritory.CostToUpgrade, "gold",s,s2);
                break;
            case TYPECUSTOM.UP_SACRED:
                s = t.SacredPlaceTerritory.Motivation;
                s2 = s + t.SacredPlaceTerritory.AtributteToAdd;
                CheckCost(InGameMenuHandler.instance.GoldPlayer, t.SacredPlaceTerritory.CostToUpgrade, "gold",s,s2);
                break;
            case TYPECUSTOM.UP_FORTRESS:
                s = t.FortressTerritory.PlusDefense;
                s2 = s + t.FortressTerritory.AtributteToAdd;
                CheckCost(InGameMenuHandler.instance.GoldPlayer, t.FortressTerritory.CostToUpgrade, "gold",s,s2);
                break;
            case TYPECUSTOM.UP_BARRACKS:
                s = t.BarracksTerritory.PlusAttack;
                s2 = s + t.BarracksTerritory.AtributteToAdd;
                CheckCost(InGameMenuHandler.instance.GoldPlayer, t.BarracksTerritory.CostToUpgrade, "gold",s,s2);
                break;
            default:
                break;
        }
    }
    private void CheckCost(int goldPlayer, int goldNeed, string element, float s, float s2)
    {
        label.GetComponent<Text>().text = "Cost -" + goldNeed.ToString() + " "+ element + "\n" + s.ToString("F2") + " -> " + s2.ToString("F2"); ;
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
        UP_LIMIT,
        UP_CHANNEL,
        UP_SPEED,
        UP_MINE,
        UP_SACRED,
        UP_FORTRESS,
        UP_BARRACKS
    }
}
