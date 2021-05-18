using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InformationMenuHandler : MonoBehaviour
{
    [SerializeField] Button missionButton;
    [SerializeField] GameObject missionMenu;
    private bool isMissionMenu = false;
    private void Awake()
    {
        missionButton.onClick.AddListener(() => OpenMissionMenu());
        missionMenu.SetActive(false);
    }
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OpenMissionMenu()
    {
        if (isMissionMenu)
        {
            missionMenu.SetActive(false);
            isMissionMenu = false;
        }
        else
        {
            missionMenu.SetActive(true);
            isMissionMenu = true;
        }
        
    }
}
