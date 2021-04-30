using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SubordinateList
{
    public List<Subordinate> MilitarBosses = new List<Subordinate>();

    public void AddSubodinateToList(Subordinate subordinate)
    {
        MilitarBosses.Add(subordinate);
    }
    public void DeleteSubodinateList()
    {
        MilitarBosses.RemoveRange(0, MilitarBosses.Count);
    }
    public List<Subordinate> AddDataSubordinateToList(int size,string type)
    {
        for (int i = 0; i < size; i++)
        {
            if(type== "militar")
            {
                MilitarBoss _militar = new MilitarBoss();
                _militar.GetMilitarBoss();
                MilitarBosses.Add(_militar);
            }
        }
        return MilitarBosses;
    }
    public MilitarBoss GetSubordinate(int index)
    {
        var m = (MilitarBoss)MilitarBosses[index];
        Debug.LogError(index + " " + m.StrategyType);
        return m;
    }
    public int CountSubordinateList()
    {
        return MilitarBosses.Count;
    }
    public void ChangueCharacIconType(string type)
    {
        for (int i = 0; i < MilitarBosses.Count; i++)
        {
            MilitarBosses[i].CharacIconType = type;
        }
    }
}
