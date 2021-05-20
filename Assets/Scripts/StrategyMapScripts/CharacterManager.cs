using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    [SerializeField] private SubordinateList militarBossList = new SubordinateList();
    private Governor governor;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        if (GlobalVariables.instance == null)
        {
            governor = new Governor("Pachacuti");
            governor.TimeInit = TimeSystem.instance.TimeGame;
        }
        else
        {
            governor = GlobalVariables.instance.GovernorChoose;
            //AudioManager.activeSong.Stop();
        }
     //   governor.TimeInit.PrintTimeSimulated();
       InGameMenuHandler.instance.UpdateProfileMenu();
    }

    public SubordinateList MilitarBossList
    {
        get { return militarBossList; }
    }

    public Governor Governor
    {
        get { return governor; }
        set { governor = value; }
    }

}
