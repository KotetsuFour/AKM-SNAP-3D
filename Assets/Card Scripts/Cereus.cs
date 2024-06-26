using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cereus : CharacterCard
{
    public new string onReveal(Board b)
    {
        return $"Player {myPlayer}'s {characterName} is awaiting targets.\n";
    }
    public new void startOfTurn(Board b)
    {
        if (b.turn == turnPlayed + 1)
        {
            Lane lane = b.getMyLane(this);
            int bonus = 0;
            for (int q = 0; q < lane.segments[myPlayer].Count; q++)
            {
                if (lane.segments[myPlayer][q].turnPlayed == b.turn)
                {
                    bonus += 2;
                }
            }
            changePermanentPower(bonus);
        }
    }
}
