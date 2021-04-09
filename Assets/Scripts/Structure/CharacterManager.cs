using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    [SerializeField]private MilitarBossList militarBossList = new MilitarBossList();
    private Governor governor;

    private void Awake()
    {
        instance = this;
        ReadJson("Data/Menu/MilitarBosses");
    }
    private void Start()
    {
        if (GlobalVariables.instance == null)
        {
            governor = new Governor("Pachacuti");
        }
        else
        {
            governor = GlobalVariables.instance.governorChoosen;
            AudioManager.activeSong.Stop();
        }
        InGameMenuHandler.instance.UpdateProfileMenu();
    }
    public MilitarBossList MilitarBossList
    {
        get{ return militarBossList; }
    }

    public Governor Governor
    {
        get { return governor; }
        set { governor = value; }
    }
    void ReadJson(string route)
    {
        TextAsset asset = Resources.Load(route) as TextAsset;
        if (asset != null)
        {
            militarBossList = JsonUtility.FromJson<MilitarBossList>(asset.text);
            militarBossList.ChangueCharacIconType();
        }
        else
        {
            print("asset is null");
        }
    }
    public void HireMilitaryBoss(TerritoryHandler territory, MilitarBoss militar)
    {
        MilitarBossList.AddMilitar(militar);
        TerritoryManager.instance.AddSpecificCharacter(territory, militar);
        InGameMenuHandler.instance.CloseCharacterSelection();
    }

}
