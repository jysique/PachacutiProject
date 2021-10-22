using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MessageManager : MonoBehaviour
{
    [SerializeField] Button button;
    void Start()
    {
        button.onClick.AddListener(()=>GlobalVariables.instance.GoToGame());
    }

}
