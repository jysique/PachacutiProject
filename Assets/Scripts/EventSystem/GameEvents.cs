using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;
    
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {

    }
    public event Action onProductionResourceTriggerEnter;
    public void ProductionResourceTriggerEnter()
    {
        if (onProductionResourceTriggerEnter != null)
        {
            onProductionResourceTriggerEnter();
        }
    }
    public event Action onConsumptionResourceTriggerEnter;
    public void ConsumptionResourceTriggerEnter()
    {
        if (onConsumptionResourceTriggerEnter != null)
        {
            onConsumptionResourceTriggerEnter();
        }
    }
    /// <summary>
    /// ===Gather gold event ====
    /// </summary>
    public event Action onGatherResourceTriggerEnter;
    public void GatherResourceTriggerEnter()
    {
        if (onGatherResourceTriggerEnter != null)
        {
            onGatherResourceTriggerEnter();
        }
    }
    
    /// <summary>
    /// ===Custom event ====
    /// </summary>
    public event Action onCustomEventEnter;
    public void CustomEventEnter()
    {
        if (onCustomEventEnter != null)
        {
            onCustomEventEnter();
        }
    }
    public event Action onCustomWarningEventEnter;
    public void CustomWarningEventEnter()
    {
        if (onCustomWarningEventEnter != null)
        {
            onCustomWarningEventEnter();
        }
    }
    /// <summary>
    /// Exit action event
    /// </summary>
    public event Action onCustomEventExit;
    public void CustomEventExit()
    {
        if (onCustomEventExit != null)
        {
            onCustomEventExit();
        }
    }
}
