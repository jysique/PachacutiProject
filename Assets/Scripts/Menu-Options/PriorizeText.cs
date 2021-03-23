using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorizeText : MonoBehaviour
{
    Renderer textRenderer;
    private void Awake()
    {
        textRenderer = gameObject.GetComponent<Renderer>();
    }
    private void Start()
    {
        textRenderer.sortingLayerID = 4;
    }
}
