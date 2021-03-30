using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    public List<MilitarBoss> militarBosses = new List<MilitarBoss>();

    private void Awake()
    {
        instance = this;
        militarBosses.Add(new MilitarBoss("Illari Sami", "01", 35, "Inca", 0,0,"Agresivo"," ",10 ));
        militarBosses.Add(new MilitarBoss("Killari Sami", "02", 30, "Inca",0, 1, "Agresivo", " ",20));
        militarBosses.Add(new MilitarBoss("Asiri Sulay", "02", 30, "Inca", 0,2, " ", " ",14));
    }
    private void Start()
    {
        AudioManager.activeSong.Stop();
    }
    public MilitarBoss GetMilitarBoss(int i)
    {
        return militarBosses[i];
    }
}
