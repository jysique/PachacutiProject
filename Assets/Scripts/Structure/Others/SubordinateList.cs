using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SubordinateList
{
    public List<Subordinate> Chiefs = new List<Subordinate>();

    public void AddSubodinateToList(Subordinate subordinate)
    {
        Chiefs.Add(subordinate);
    }
    public void DeleteSubodinateList()
    {
        Chiefs.RemoveRange(0, Chiefs.Count);
    }
    public void AddDataSubordinateToList(int size,Character character)
    {
        for (int i = 0; i < size; i++)
        {
            if (character is MilitarChief)
            {
                MilitarChief _militar = new MilitarChief();
                _militar.GetMilitarBoss();
                Chiefs.Add(_militar);
            }
            /*else if(character is )
            {
                MilitarChief _militar = new MilitarChief();
                _militar.GetMilitarBoss();
                Chiefs.Add(_militar);
            }
            */
        }
       // return MilitarBosses;
    }
    public int CountSubordinateList()
    {
        return Chiefs.Count;
    }
}
