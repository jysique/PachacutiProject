using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    [SerializeField] private SubordinateList militarBossList = new SubordinateList();
    private Governor governor;

    [Header("Menu personaje")]

    [SerializeField] private Text governorName;
    [SerializeField] private Text governorAge;
    [SerializeField] private Text governorOrigin;
    [SerializeField] private Image governorPicture;
    [SerializeField] private Text governorDiplomacy;
    [SerializeField] private Text governorMilitancy;
    [SerializeField] private Text governorManagement;
    [SerializeField] private Text governorPrestige;
    [SerializeField] private Text governorPiety;

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
       UpdateProfileMenu();
    }
    public void UpdateProfileMenu()
    {
        Governor temp = CharacterManager.instance.Governor;
        governorName.text =  temp.CharacterName;
        governorAge.text = "Age: " + temp.Age.ToString();
        governorPicture.sprite = temp.Picture;
        governorOrigin.text = "Birth place: " + temp.Origin;
        governorDiplomacy.text = temp.Diplomacy + "/10";
        governorMilitancy.text =  temp.Militancy + "/10";
        governorManagement.text = temp.Managment + "/10";
        governorPrestige.text = temp.Prestige + "/10";
        governorPiety.text = temp.Piety + "/10";
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
