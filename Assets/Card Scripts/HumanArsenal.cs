using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanArsenal : CharacterCard
{
    public new string onReveal(Board b)
    {
        Lane lane = b.getMyLane(this);
        for (int q = 0; q < lane.segments.Count; q++)
        {
            if (q == myPlayer)
            {
                continue;
            }
            for (int w = 0; w < lane.segments[q].Count; w++)
            {
                if (lane.segments[q][w].attributes.Contains(Attribute.INNATE_MAGIC))
                {
                    changePermanentPower(3);
                    return $"Player {myPlayer}'s {characterName} detected Player {q}'s {lane.segments[q][w].characterName} and gained 3 Power.\n";
                }
            }
        }
        return null;
    }
}
