using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Player
{
    //  [SerializeField] string playerLoc;
    //  [SerializeField] string playerNick;
    /*
      public string Player_Loc
      {
          get { return playerLoc; }
          set { playerLoc = value; }
      }
      public string Player_Nick
      {
          get { return playerNick; }
          set { playerNick = value; }
      }
    */
    [SerializeField] string name;
    [SerializeField] Territory.REGION region;
    [SerializeField] Territory.TYPEPLAYER type;
    [SerializeField] int gold;
    [SerializeField] int food;
    [SerializeField] int motivation;
    [SerializeField] int opinion;
    [SerializeField] MilitarChief militar;
    [SerializeField] List<Units> troop = new List<Units>();

    public List<Units> Troop
    {
        get { return troop; }
        set { troop = value; }
    }
}
[System.Serializable]
public class Units
{
    [SerializeField] List<int> s = new List<int>();
    public List<int> S
    {
        get { return s; }
        set { s = value; }
    }
}