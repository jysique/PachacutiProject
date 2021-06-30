using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class NumericButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private TextMeshProUGUI number;
    [SerializeField] private bool inc;
    public int limit;
    public bool lockButton;
    public bool pointerDown = false;
    private int t = 0;
    private int total = 5;
    public void IncreaseNumber()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ReadAndPlaySFX("increase");
        }
        int temporal = int.Parse(number.text)+1;
        if (temporal <= limit) number.text = temporal.ToString();
        //else number.text = 1.ToString();
    }
    public void DecreaseNumber()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.ReadAndPlaySFX("decrease");
        }
        int temporal = int.Parse(number.text) - 1;
        if(temporal > 0) {
            number.text = temporal.ToString();
        }
        else {
            number.text = limit.ToString();
        }
    }
 
    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
        t = total+1;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerDown = false;
    }
    private void FixedUpdate()
    {
        if (pointerDown && lockButton)
        {
            if (t > total)
            {
                if (inc) IncreaseNumber();
                else DecreaseNumber();
                t = 0;


            }
            else
            {
                t++;
            }
           
        }
    }
}
