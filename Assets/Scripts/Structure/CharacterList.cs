using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterList
{
    public List<Character> Characters = new List<Character>();
    public void AddCharacter(Character character)
    {
        Characters.Add(character);
    }
    public Character GetCharacter(int index)
    {
        return Characters[index];
    }
    public int CountList()
    {
        return Characters.Count;
    }
}
