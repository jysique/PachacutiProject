using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AdaptativeFont : MonoBehaviour
{
    TextMeshProUGUI text;
    [SerializeField] private bool continualUpdate = true;
    [SerializeField] private int fontSizeAtDefaultResolution = 40;
    [SerializeField] private static float defaultResolution = 1919f;
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        if (continualUpdate)
        {
            InvokeRepeating("Adjust", 0f, Random.Range(0.3f, 1f));
        }
        else
        {
            Adjust();
            enabled = false;
        }
    }
    void Adjust()
    {
        if (!enabled|| !gameObject.activeInHierarchy)
        {
            return;
        }
        float totalCurrentRes = Screen.height + Screen.width;
        float perc = totalCurrentRes / defaultResolution;
        int fontSize = Mathf.RoundToInt((float)fontSizeAtDefaultResolution * perc);
        text.fontSize = fontSize;
    }
}
