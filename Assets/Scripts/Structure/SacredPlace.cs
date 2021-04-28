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
    public override void ImproveBuilding()
    {
        base.ImproveBuilding();
        this.motivation += 0.6f;
    }
}
