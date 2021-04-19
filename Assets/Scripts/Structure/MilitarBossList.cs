using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MilitarBossList
{
    public List<MilitarBoss> MilitarBosses = new List<MilitarBoss>();
    public void PrintList()
    {
        for (int i = 0; i < MilitarBosses.Count; i++)
        {
            Debug.Log(i+ " - " +MilitarBosses[i].CharacterName);
        }
    }
    public void AddMilitar(MilitarBoss militar)
    {
        MilitarBosses.Add(militar);
    }
    public List<MilitarBoss> AddDataMilitaryList(int size)
    {
        for (int i = 0; i < size; i++)
        {
            MilitarBoss _militar = new MilitarBoss();
            _militar.GetMilitarBoss();
            MilitarBosses.Add(_militar);
        }
        return MilitarBosses;
    }
    public void DeleteList()
    {
        MilitarBosses.RemoveRange(0,MilitarBosses.Count);
    }
    public MilitarBoss GetMilitaryBoss(string name)
    {
        for (int i = 0; i < MilitarBosses.Count; i++)
        {
            if (MilitarBosses[i].CharacterName == name)
            {
                return MilitarBosses[i];
            }
        }
        return null;
    }
    public MilitarBoss GetMilitaryBoss(int index)
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
