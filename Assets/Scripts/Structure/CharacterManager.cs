using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    [SerializeField] private MilitarBossList militarBossList = new MilitarBossList();
    [SerializeField]private Governor governor= new Governor();

    private void Awake()
    {
        instance = this;
        ReadJson("Data/Menu/MilitarBosses");
    }
    private void Start()
    {
        governor = GlobalVariables.instance.governorChoosen;
        InGameMenuHandler.instance.UpdateProfileMenu();
        AudioManager.activeSong.Stop();
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


}
