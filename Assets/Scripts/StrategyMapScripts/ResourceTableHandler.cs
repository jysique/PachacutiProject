using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceTableHandler : MonoBehaviour
{
    public static ResourceTableHandler instance;
    [SerializeField] private Text goldGenerated;
    [SerializeField] private Text foodGenerated;
    [SerializeField] private Text scoreTxt;
    [SerializeField] private Transform goldAnimation;
    [SerializeField] private Transform foodAnimation;
    public Transform GoldAnimation
    {
        get { return goldAnimation; }
    }
    public Transform FoodAnimation
    {
        get { return foodAnimation; }
    }
    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        UpdateResourceTable();
    }
    public void UpdateResourceTable()
    {
        goldGenerated.text = InGameMenuHandler.instance.GoldPlayer.ToString();
        foodGenerated.text = InGameMenuHandler.instance.FoodPlayer.ToString();
        scoreTxt.text = TerritoryManager.instance.CountTerrytorry(Territory.TYPEPLAYER.PLAYER).ToString();
    }
}
