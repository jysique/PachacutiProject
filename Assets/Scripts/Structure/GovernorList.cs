using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GovernorList
{
    public List<Governor> Governors= new List<Governor>();

    public int CountList()
    {
        return Governors.Count;
    }
    public Governor GetGovernors(string name)
    {
        for (int i = 0; i < Governors.Count; i++)
        {
            if (Governors[i].CharacterName == name)
            {
                return Governors[i];
            }
        }
        return null;
    }
}
