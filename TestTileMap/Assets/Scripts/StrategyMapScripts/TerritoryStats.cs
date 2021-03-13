using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerritoryStats : MonoBehaviour
{
    [SerializeField] private Image timerBar;
    private bool canPopulate = true;
    private float timeLeft;
    private float maxTime = 5f;

    public int population;
    public float velocity;
    public Text populationTxt;

    private void Start()
    {
        timeLeft = maxTime;
    }
    public void InitalizeStats(int _population, float _velocity)
    {
        velocity = _velocity;
        population = _population;
        
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

        populationTxt.text = population.ToString();
    }
    private void IncresementPopulation()
    {
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime * velocity;
            timerBar.fillAmount = timeLeft / maxTime;
        }
        else
        {
            population++;
            timeLeft = maxTime;
        }
    }
}
