using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerritoryStats : MonoBehaviour
{
    [SerializeField] private Image timerBar;
    private bool canPopulate = true;
    private float timeLeftP;
    private float timeLeftG;
    private float timeLeftF;
    private float maxTime = 5f;
    private float difTime;

    public int population;
    public int limitPopulation;
    public int gold;
    public int food;

    public float velocityPopulation;
    public float velocityGold;
    public float velocityFood;

    public Text populationTxt;

    private void Start()
    {
        if (GlobalVariables.instance != null)
        {
            difTime = GlobalVariables.instance.velocityGame;
        }
        else
        {
            difTime = 0f;
        }
        maxTime = 5f - difTime;
        timeLeftP = maxTime;
        timeLeftG = maxTime;
        timeLeftF = maxTime;
    }
    public void InitalizeStats(Territory territory)
    {
        population = territory.Population;
        gold = territory.GoldReward;
        food = territory.FoodReward;
        limitPopulation = territory.LimitPopulation;

        velocityPopulation = territory.VelocityPopulation;
        velocityGold = territory.VelocityGold;
        velocityFood = territory.VelocityFood;
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
        populationTxt.text = population.ToString();
    }
    private void IncresementPopulation()
    {
        
        if (timeLeftP > 0)
        {
            timeLeftP -= Time.deltaTime * velocityPopulation;
            print(maxTime);
            timerBar.fillAmount = timeLeftP / maxTime;
        }
        else
        {
            population++;
            timeLeftP = maxTime;
        }
    }
    private void IncresementGold()
    {

        if (timeLeftG > 0)
        {
            timeLeftG -= Time.deltaTime * velocityGold;
        }
        else
        {
            gold++;
            timeLeftG = maxTime;
        }
    }
    private void IncresementFood()
    {

        if (timeLeftF > 0)
        {
            timeLeftF -= Time.deltaTime * velocityFood;
        }
        else
        {
            food++;
            timeLeftF = maxTime;
        }
    }

}
