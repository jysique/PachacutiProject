using UnityEngine;
[System.Serializable]
public class GoldMine:Buildings
{
    [SerializeField]private float velocityGold;

    public float VelocityGold
    {
        get { return velocityGold; }
        set { velocityGold = value; }
    }
}
