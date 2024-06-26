using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trucos : Location
{
    public new bool canReveal(Board b)
    {
        return b.turn == Board.lastTurn;
    }
}
