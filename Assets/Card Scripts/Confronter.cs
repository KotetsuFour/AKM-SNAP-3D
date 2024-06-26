using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confronter : CharacterCard
{
    [SerializeField] private int bonus;
    public new string onReveal(Board b)
    {
        Lane lane = b.getMyLane(this);
        for (int q = 0; q < lane.segments.Count; q++)
        {
            if (q != myPlayer)
            {
                for (int w = 0; w < lane.segments[q].Count; w++)
                {
                    if (lane.segments[q][w].turnPlayed == turnPlayed)
                    {
                        string ret = $"Player {myPlayer}'s {characterName} detected Player {q}'s {lane.segments[q][w].characterName} and gained {bonus} Power.\n";
                        changePermanentPower(bonus);
                        return ret;
                    }
                }
            }
        }
        return null;
    }
}
