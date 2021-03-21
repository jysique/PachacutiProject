using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLayer : MonoBehaviour
{
    BCFC controller;
    public Texture tex;
    // public MovieTexture mov;
    public float speed = 2;
    public bool smooth;
    private void Start()
    {
        controller = BCFC.instance;
        
    }
    private void Update()
    {
        BCFC.LAYER layer = null;
        if (Input.GetKey(KeyCode.Q))
        {
            layer = controller.background;
            
        }
        if (Input.GetKey(KeyCode.W))
        {
            layer = controller.cinematic;
            print("hola2");
        }
        if (Input.GetKey(KeyCode.E))
        {
            layer = controller.foreground;
            print("hola3");
        }
        if (Input.GetKey(KeyCode.T))
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                layer.TransitionToTexture(tex,speed,smooth);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                //layer.TransitionToTexture(movie);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                layer.SetTexture(tex);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                //layer.SetTexture(movie);
            }
        }
    }
}
