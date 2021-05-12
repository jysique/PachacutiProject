using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerritoryStats : MonoBehaviour
{
    [SerializeField] private Image timerBar;
    [SerializeField] public GameObject container;
    private bool canPopulate = true;
    private float timeLeftP;
    private float timeLeftG;
    private float timeLeftF;
    private int maxTimeCount;
    public Territory territory;

    public Text populationTxt;
    public Text nameTerritoryTxt;
    public Image imageTerritory;
    private void Start()
    {
        maxTimeCount = 12;
        timeLeftP = 0;
        //timeLeftP = maxTime;
        timeLeftG = maxTimeCount;
        timeLeftF =maxTimeCount;
        imageTerritory.color = new Color(
                  Random.Range(0f, 1f),
      Random.Range(0f, 1f),
      Random.Range(0f, 1f)
            );
    }
    public void SetCanPopulate(bool temp)
    {
        timerBar.enabled = temp;
        canPopulate = temp;
    }
    private void FixedUpdate()
    {
        if (canPopulate)
        {
            IncresementPopulation();

        }
        IncresementGold();
        IncresementFood();
        
        populationTxt.text = territory.Population.ToString() + " / " + territory.LimitPopulation.ToString();
    }
    private void IncresementPopulation()
    {
        if (timeLeftP <= maxTimeCount)
        {
            timeLeftP += Time.deltaTime * territory.VelocityPopulation * GlobalVariables.instance.timeModifier;
            timerBar.fillAmount = timeLeftP / maxTimeCount;
        }
        else
        {
            territory.Population++;
            timeLeftP = 0;
        }
    }
    private void IncresementGold()
    {

        if (timeLeftG > 0)
        {
            // timeLeftG -= Time.deltaTime * velocityGold;
            timeLeftG -= Time.deltaTime * territory.GoldMineTerritory.VelocityGold * GlobalVariables.instance.timeModifier;
        }
        else
        {
            territory.Gold++;
            //gold++;
            timeLeftG = maxTimeCount;
        }
    }
    private void IncresementFood()
    {

        if (timeLeftF > 0)
        {
            // timeLeftF -= Time.deltaTime * velocityFood;
            timeLeftF -= Time.deltaTime * territory.IrrigationChannelTerritory.VelocityFood * GlobalVariables.instance.timeModifier;
        }
        else
        {
            territory.FoodReward++;
            //food++;
            timeLeftF = maxTimeCount;
        }
    }

}
