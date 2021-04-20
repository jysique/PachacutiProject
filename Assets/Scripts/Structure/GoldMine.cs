using UnityEngine;
[System.Serializable]
public class GoldMine:Building
{
    [SerializeField]private float velocityGold;

    public float VelocityGold
    {
        get { return velocityGold; }
        set { velocityGold = value; }
    }
}
