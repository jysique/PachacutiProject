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
    private float maxTime;

    public Territory territory;

    public Text populationTxt;
    public Text nameTerritoryTxt;
    public Image imageTerritory;
    private void Start()
    {
        maxTime = 6f;
        if (GlobalVariables.instance != null)
        {
            maxTime = GlobalVariables.instance.MaxTimeCount;
        }
        timeLeftP = maxTime;
        timeLeftG = maxTime;
        timeLeftF = maxTime;
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
        nameTerritoryTxt.text = territory.name;
        populationTxt.text = territory.Population.ToString();
    }
    private void IncresementPopulation()
    {
        
        if (timeLeftP > 0)
        {
            timeLeftP -= Time.deltaTime * territory.VelocityPopulation;
            timerBar.fillAmount = timeLeftP / maxTime;
        }
        else
        {
            territory.Population++;
            //population++;
            timeLeftP = maxTime;
        }
    }
    private void IncresementGold()
    {

        if (timeLeftG > 0)
        {
            // timeLeftG -= Time.deltaTime * velocityGold;
            timeLeftG -= Time.deltaTime * territory.GoldMineTerritory.VelocityGold;
        }
        else
        {
            territory.Gold++;
            //gold++;
            timeLeftG = maxTime;
        }
    }
    private void IncresementFood()
    {

        if (timeLeftF > 0)
        {
            // timeLeftF -= Time.deltaTime * velocityFood;
            timeLeftF -= Time.deltaTime * territory.VelocityFood;
        }
        else
        {
            territory.FoodReward++;
            //food++;
            timeLeftF = maxTime;
        }
    }

}
