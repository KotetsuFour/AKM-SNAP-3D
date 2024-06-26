using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golyat : CharacterCard
{
    public new string onReveal(Board b)
    {
        string ret = "";
        Lane lane = b.getMyLane(this);
        for (int q = 0; q < lane.segments.Count; q++)
        {
            if (q != myPlayer)
            {
                CharacterCard strongest = null;
                for (int w = 0; w < lane.segments[q].Count; w++)
                {
                    if (lane.segments[q][w].revealed && (strongest == null || strongest.getPower(b) > lane.segments[q][w].getPower(b)))
                    {
                        strongest = lane.segments[q][w];
                    }
                }
                if (strongest != null)
                {
                    ret += $"Player {myPlayer}'s {characterName} targeted Player {q}'s {strongest.characterName} for destruction.\n";
                    strongest.destroy(b, q);
                }
            }
        }
        return ret;
    }
}
