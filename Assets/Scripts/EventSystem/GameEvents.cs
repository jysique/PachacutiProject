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
    /// <summary>
    /// ===Gather gold event ====
    /// </summary>
    public event Action onGatherGoldTriggerEnter;
    public void GatherGoldTriggerEnter()
    {
        if (onGatherGoldTriggerEnter != null)
        {
            onGatherGoldTriggerEnter();
        }
    }
    /// <summary>
    /// ===Gather food event ====
    /// </summary>
    public event Action onGatherFoodTriggerEnter;
    public void GatherFoodTriggerEnter()
    {
        if (onGatherFoodTriggerEnter != null)
        {
            onGatherFoodTriggerEnter();
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
