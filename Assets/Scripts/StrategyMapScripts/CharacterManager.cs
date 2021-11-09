using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    private Governor governor;

    [Header("Menu personaje")]

    [SerializeField] private TextMeshProUGUI governorName;
    [SerializeField] private TextMeshProUGUI governorAge;
    [SerializeField] private TextMeshProUGUI governorOrigin;
    [SerializeField] private Image governorPicture;
    [SerializeField] private TextMeshProUGUI governorDiplomacy;
    [SerializeField] private TextMeshProUGUI governorMilitancy;
    [SerializeField] private TextMeshProUGUI governorManagement;
    [SerializeField] private TextMeshProUGUI governorPrestige;
    [SerializeField] private TextMeshProUGUI governorPiety;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {

        governor = GlobalVariables.instance.GovernorChoose;
       UpdateProfileMenu();
    }
    public void UpdateProfileMenu()
    {
        Governor temp = CharacterManager.instance.Governor;
        governorName.text =  temp.CharacterName;
        governorAge.text = GameMultiLang.GetTraduction("AgeLabel") + temp.Age.ToString();
        governorPicture.sprite = temp.Picture;
        governorOrigin.text = GameMultiLang.GetTraduction("BirthLabel") + temp.Origin;
        governorDiplomacy.text = temp.Diplomacy + "/20";
        governorMilitancy.text =  temp.Militancy + "/20";
        governorManagement.text = temp.Managment + "/20";
        governorPrestige.text = temp.Prestige + "/20";
        governorPiety.text = temp.Piety + "/20";
    }

    public Governor Governor
    {
        get { return governor; }
        set { governor = value; }
    }

}
