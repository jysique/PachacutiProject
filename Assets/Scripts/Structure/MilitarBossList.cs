using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MilitarBossList
{
    public List<MilitarBoss> MilitarBosses = new List<MilitarBoss>();

    public void AddCharacter(MilitarBoss militar)
    {
        MilitarBosses.Add(militar);
    }
    public MilitarBoss GetCharacter(int index)
    {
        return MilitarBosses[index];
    }
    public int CountList()
    {
        return MilitarBosses.Count;
    }
    public void ChangueCharacIconType()
    {
        for (int i = 0; i < MilitarBosses.Count; i++)
        {
            MilitarBosses[i].CharacIconType = "Military";
        }
    }
}
