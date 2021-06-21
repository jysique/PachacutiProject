using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        print("sapo cochino");
        switch (typecustom)
        {
            case TYPECUSTOM.UP_LIMIT:
                float s = t.LimitPopulation;
                float s2 = s + t.ImproveLimit;
                CheckCost(InGameMenuHandler.instance.FoodPlayer, t.CostLimitPopulation, "food",s,s2);
                break;
            case TYPECUSTOM.UP_SPEED:
                s = t.VelocityPopulation;
                s2 = s + t.ImproveSpeed;
                CheckCost(InGameMenuHandler.instance.GoldPlayer, t.CostSpeedPopulation, "gold",s,s2);
                break;
            case TYPECUSTOM.UP_CHANNEL:
                s = t.IrrigationChannelTerritory.WorkersChannel;
                s2 = s + t.IrrigationChannelTerritory.AtributteToAdd;
                CheckCost(InGameMenuHandler.instance.GoldPlayer, t.IrrigationChannelTerritory.CostToUpgrade, "gold",s,s2);
                break;
            case TYPECUSTOM.UP_MINE:
                s = t.GoldMineTerritory.WorkersMine;
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
            case TYPECUSTOM.UP_ARMORY:
                s = t.ArmoryTerritory.PlusAttack;
                s2 = s + t.ArmoryTerritory.AtributteToAdd;
                CheckCost(InGameMenuHandler.instance.GoldPlayer, t.ArmoryTerritory.CostToUpgrade, "gold",s,s2);
                break;
            default:
                break;
        }
    }
    private void CheckCost(int goldPlayer, int goldNeed, string element, float s, float s2)
    {
        label.GetComponent<TextMeshProUGUI>().text = "Cost -" + goldNeed.ToString() + " "+ element + " " + s.ToString("F2") + " -> " + s2.ToString("F2"); ;
        if (goldPlayer >= goldNeed)
        {
            label.GetComponent<TextMeshProUGUI>().color = Color.white;
        }
        else
        {
            label.GetComponent<TextMeshProUGUI>().color = Color.red;
        }
    }
    public enum TYPECUSTOM
    {
        UP_LIMIT,
        UP_SPEED,
        UP_CHANNEL,
        UP_MINE,
        UP_SACRED,
        UP_FORTRESS,
        UP_ARMORY
    }
}
