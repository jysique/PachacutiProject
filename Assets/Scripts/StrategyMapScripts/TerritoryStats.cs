using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerritoryStats : MonoBehaviour
{
    [SerializeField] private Image timerBar;
    [SerializeField] public GameObject container;

    [SerializeField] private Text populationTxt;
    [SerializeField] private Text nameTerritoryTxt;
    [SerializeField] private Image imageTerritory;
    [SerializeField] private GameObject iconsContainer;

  //  private bool canPopulate = true;
    private float timeLeftP;
    private float timeLeftG;
    private float timeLeftF;
    private int maxTimeCount;
    public Territory territory;
    Text[] allText;
    private void Start()
    {
        
        maxTimeCount = 12;
        timeLeftP = 0;
        //timeLeftP = maxTime;
        timeLeftG = maxTimeCount;
        timeLeftF =maxTimeCount;
        //imageTerritory.color = new Color( Random.Range(0f, 1f), Random.Range(0f, 1f),Random.Range(0f, 1f));
        nameTerritoryTxt.text = territory.name;
        //allText = iconsContainer.gameObject.transform.GetComponentsInChildren<Text>();
        
    }
    public void CanPopulate()
    {
        populationTxt.text = territory.Population.ToString() + " / " + territory.LimitPopulation.ToString();
        if (territory.TypePlayer != Territory.TYPEPLAYER.NONE)
        {
            populationTxt.color = Color.white;
            if (territory.Population < territory.LimitPopulation)
            {
                IncresementPopulation();
                timerBar.enabled = true;
            }
            else if(territory.Population > territory.LimitPopulation)
            {
                populationTxt.color = Color.red;
                DecreasementPopulation();
            }
        }
        else
        {
            timerBar.enabled = false;
        }
    }
    private void FixedUpdate()
    {
        CanPopulate();
        //IncresementGold();
        //IncresementFood();
        /*
        allText[0].text = territory.IrrigationChannelTerritory.Level.ToString();
        allText[1].text = territory.GoldMineTerritory.Level.ToString();
        allText[2].text = territory.SacredPlaceTerritory.Level.ToString();
        allText[3].text = territory.FortressTerritory.Level.ToString();
        allText[4].text = territory.BarracksTerritory.Level.ToString();
        */
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

    private void DecreasementPopulation()
    {
        if (timeLeftP <= maxTimeCount)
        {
            timeLeftP += Time.deltaTime * GlobalVariables.instance.timeModifier * 12;
        }
        else
        {
            territory.Population--;
            timeLeftP = 0;
        }
    }
    public void IncresementGold()
    {
        territory.Gold+= territory.GoldMineTerritory.WorkersMine / territory.PerPeople;
    }
    public void IncresementFood()
    {
        territory.FoodReward+= territory.IrrigationChannelTerritory.WorkersChannel / territory.PerPeople;
    }
   
}
