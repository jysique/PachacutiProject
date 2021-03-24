using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCM : MonoBehaviour
{
    [HideInInspector]public CharacterVN Luissa;
    [HideInInspector] public CharacterVN Carmen;
    private void Start()
    {
        Luissa = CharacterManager.instance.GetCharacter("Luissa", enableCreatedCharacterOnStart:false);
        
    }
    public string[] s = new string[]
    {
        "Hi, my name is Luissa",
        "How are you? ",
        "Oh... thats nice",
        "What do you want to do?"

    };
    int index = 0;
    public Vector2 moveTarget;
    public float moveSpeed;
    public bool smooth;

    public int bodyIndex, expressionIndex = 0;
    public float speed;
    public bool smoothTransitions = false;
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
        if (Input.GetKey(KeyCode.M))
        {
            Luissa.MoveTo(moveTarget, moveSpeed,smooth);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Luissa.StopMoving(true);
        }
        if (Input.GetKey(KeyCode.B))
        {
            Luissa.TransitionBody(Luissa.GetSprite(bodyIndex),speed,smoothTransitions);
        }
        if (Input.GetKey(KeyCode.J))
        {
            Luissa.SetBody(bodyIndex);
        }
        if (Input.GetKey(KeyCode.E))
        {
            Luissa.SetExpression(expressionIndex);
        }
    }
}
