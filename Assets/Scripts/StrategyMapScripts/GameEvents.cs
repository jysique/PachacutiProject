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
    // ====RECOLECCION DE ORO ====
    public event Action onGatherGoldTriggerEnter;
    public void GatherGoldTriggerEnter()
    {
        if (onGatherGoldTriggerEnter != null)
        {
            onGatherGoldTriggerEnter();
        }
    }
    /*
    public event Action onGatherGoldTriggerExit;
    public void GatherGoldTriggerExit()
    {
        if (onGatherGoldTriggerExit != null)
        {
            onGatherGoldTriggerExit();
        }
    }
    */
    // ====RECOLECCION DE COMIDA ====
    public event Action onGatherFoodTriggerEnter;
    public void GatherFoodTriggerEnter()
    {
        if (onGatherFoodTriggerEnter != null)
        {
            onGatherFoodTriggerEnter();
        }
    }
    /*
    public event Action onGatherFoodTriggerExit;
    public void GatherFoodTriggerExit()
    {
        if (onGatherFoodTriggerExit != null)
        {
            onGatherFoodTriggerExit();
        }
    }
    */
    // ==== EVENTO PERSONALIZADO ====
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

    public event Action onCustomEventExit;
    public void CustomEventExit()
    {
        if (onCustomEventExit != null)
        {
            onCustomEventExit();
        }
    }
}
