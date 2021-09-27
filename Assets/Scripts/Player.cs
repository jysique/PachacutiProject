using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Player
{
    [SerializeField] string playerLoc;
    [SerializeField] string playerNick;
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
    public Player(string _loc, string _nick)
    {
        playerLoc = _loc;
        playerNick = _nick;
    }
}
