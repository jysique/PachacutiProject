using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AppearAndDissapearAnimation : MonoBehaviour
{
    Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void ChangeColor()
    {
        if(CombatManager.instance.turn == true)
        {
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "PLAYER TURN";
            transform.GetChild(1).GetComponent<Image>().color = Color.blue;
        }
        else
        {
            transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "ENEMY TURN";
            transform.GetChild(1).GetComponent<Image>().color = Color.red;
        }
        
    }
    public void Change()
    {
        anim.SetTrigger("change");
    }

    public void Stay()
    {
        anim.SetTrigger("stay");
    }

}
