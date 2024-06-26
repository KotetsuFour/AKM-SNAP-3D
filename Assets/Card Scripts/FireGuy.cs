using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGuy : CharacterCard
{
    public new string onReveal(Board b)
    {
        string ret = "";
        Lane lane = b.getMyLane(this);
        for (int q = 0; q < lane.segments.Count; q++)
        {
            if (q != myPlayer)
            {
                CharacterCard weakest = null;
                for (int w = 0; w < lane.segments[q].Count; w++)
                {
                    if (lane.segments[q][w].revealed && (weakest == null || weakest.getPower(b) < lane.segments[q][w].getPower(b)))
                    {
                        weakest = lane.segments[q][w];
                    }
                }
                if (weakest != null)
                {
                    ret += $"Player {myPlayer}'s {characterName} targeted Player {q}'s {weakest.characterName} for destruction.\n";
                    weakest.destroy(b, q);
                }
            }
        }
        return ret;
    }
}
