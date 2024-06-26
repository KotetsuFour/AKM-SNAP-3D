using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeakBoy : CharacterCard
{
    public new string onReveal(Board b)
    {
        string ret = "";
        int bonus = 0;
        for (int laneIdx = 0; laneIdx < b.lanes.Length; laneIdx++)
        {
            Lane lane = b.lanes[laneIdx];
            for (int q = 0; q < lane.segments.Count; q++)
            {
                if (q == myPlayer)
                {
                    continue;
                }
                for (int w = 0; w < lane.segments[q].Count; w++)
                {
                    if (lane.segments[q][w].revealed && lane.segments[q][w].attributes.Contains(Attribute.WATER))
                    {
                        bonus++;
                        changePermanentPower(1);
                    }
                }
            }
        }
        if (bonus == 0)
        {
            return null;
        }
        changePermanentPower(bonus);
        ret += $"Player {myPlayer}'s {characterName} gained {bonus} Power from opponents' Water cards.\n";
        return ret;
    }
}
