using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public static Transition instance { get; private set; }
    public Animator anim;
    public int scene = 0;

    private void Awake()
    {
        instance = this;
        anim = GetComponent<Animator>();
    }
    public void MakeStayTransition()
    {
        
        anim.SetBool("stay", true);
    }
    public void LoadScene(int _scene)
    {
       
        print("cargar escena");
        scene = _scene;
        anim.SetBool("end",true);
    }
    public void ChangeScene()
    {
        SceneManager.LoadScene(scene);
    }
}
