using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public int playerIndex = 0;
    public int PlayerIndex
    {
        get { return playerIndex; }
        set { playerIndex = value; }
    }
}
