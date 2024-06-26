using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipFlopGirl : CharacterCard
{
    public new string onReveal(Board b)
    {
        string ret = "";
        for (int laneIdx = 0; laneIdx < b.lanes.Length; laneIdx++)
        {
            Lane lane = b.lanes[laneIdx];
            for (int q = 0; q < lane.segments[myPlayer].Count; q++)
            {
                if (lane.segments[myPlayer][q] != this
                    &&lane.segments[myPlayer][q].revealed && lane.segments[myPlayer][q].attributes.Contains(Attribute.WATER))
                {
                    ret += $"Player {myPlayer}'s {characterName} increased their {lane.segments[myPlayer][q].characterName}'s Power by 1.\n";
                    lane.segments[myPlayer][q].changePermanentPower(1);
                }
            }
        }
        return ret;
    }
}
