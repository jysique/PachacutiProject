using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TurnManager : MonoBehaviour
{
    int totalTurns;
   // [SerializeField] int actualTurn;

    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI text;
    void Start()
    {
        totalTurns = CombatManager.instance.Turns;
    }

    void UpdateFillImage()
    {
        image.fillAmount = (CombatManager.instance.Turns *0.5f / totalTurns);
    }
    // Update is called once per frame
    void Update()
    {
        UpdateFillImage();
        text.text = CombatManager.instance.Turns.ToString();
    }
}
