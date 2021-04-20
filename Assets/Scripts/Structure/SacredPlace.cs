using UnityEngine;
[System.Serializable]
public class SacredPlace: Building
{
    [SerializeField] private float motivation;

    public float Motivation
    {
        get { return motivation; }
        set { motivation = value; }
    }
}
