using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDS : MonoBehaviour
{
    DialogueSystem dialogue;
    private void Start()
    {
        dialogue = DialogueSystem.instance;
    }
    public string[] s = new string[]
    {
        "Hi, how are you?:Diego",
        "It's lovely weather today.:Jose",
        "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Cras luctus lectus nibh, id po",
        "To be honest , im glad its not snowing any more!:Luis",
        "I ...: Diego",
        "I have to tell you something..."
    };
    public string[] t = new string[]
{
        "I ...: Diego",
        "I have to tell you something..."
};

    int index = 0;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!dialogue.isSpeaking|| dialogue.isWaitingForUserInput)
            {
                if (index >= s.Length)
                {
                    return;
                }
                Say(s[index]);
                //SayAdd(t[index]);
                index++;
            }
        }
    }

    private void Say(string s)
    {
        string[] parts = s.Split(':');
        string speech = parts[0];
        string speaker = (parts.Length >= 2) ? parts[1] : "";
        dialogue.Say(speech, speaker);
    }
    /*
    private void SayAdd(string s)
    {
        string[] parts = s.Split(':');
        string speech = parts[0];
        string speaker = (parts.Length >= 2) ? parts[1] : "";
        dialogue.SayAdd(speech, speaker);
    }
    */
}
