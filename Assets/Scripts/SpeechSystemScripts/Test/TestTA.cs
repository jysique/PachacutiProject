using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TestTA : MonoBehaviour
{
    public Text text;
    TextArchitect architect;

    [TextArea(5,10)]
    public string say;
    public int characterPerFrame = 1;
    public float speed = 1f;
    public bool useEncap = true;
    private void Start()
    {
        architect = new TextArchitect(say);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            architect = new TextArchitect(say,"",characterPerFrame,speed,useEncap);
        }
        text.text = architect.currentText;
    }
}
