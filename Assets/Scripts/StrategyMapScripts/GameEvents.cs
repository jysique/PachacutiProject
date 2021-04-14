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
    public event Action onGatherGoldTriggerEnter;
    public void GatherGoldTriggerEnter()
    {
        if (onGatherGoldTriggerEnter != null)
        {
            onGatherGoldTriggerEnter();
        }
    }
    public event Action onGatherGoldTriggerExit;
    public void GatherGoldTriggerExit()
    {
        if (onGatherGoldTriggerExit != null)
        {
            onGatherGoldTriggerExit();
        }
    }
    public event Action onGatherFoodTriggerEnter;
    public void GatherFoodTriggerEnter()
    {
        if (onGatherFoodTriggerEnter != null)
        {
            onGatherFoodTriggerEnter();
        }
    }
    public event Action onGatherFoodTriggerExit;
    public void GatherFoodTriggerExit()
    {
        if (onGatherFoodTriggerExit != null)
        {
            onGatherFoodTriggerExit();
        }
    }
}
