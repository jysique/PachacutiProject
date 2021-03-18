using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCM : MonoBehaviour
{
    [HideInInspector]public Character Luissa;
    private void Start()
    {
        Luissa = CharacterManager.instance.GetCharacter("Luissa");
    }
    public string[] s = new string[]
    {
        "Hi, my name is Luissa",
        "How are you? ",
        "Oh... thats nice",
        "What do you want to do?"

    };
    int index = 0;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(index < s.Length)
            {
                Luissa.Say(s[index]);
            }
            else
            {
                DialogueSystem.instance.Close();
            }
            index++;
        }
    }
}
