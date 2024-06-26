using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBoy : CharacterCard
{
    [SerializeField] private CharacterCard rock;
    public new string onReveal(Board b)
    {
        string ret = "";
        Lane lane = b.getMyLane(this);
        for (int q = 0; q < lane.segments.Count; q++)
        {
            if (q != myPlayer)
            {
                if (lane.segments[q].Count < lane.cardsPerPlayerInThisLane)
                {
                    b.addToLane(lane, Instantiate(rock), q);
                    ret += $"Player {myPlayer}'s {characterName} added a Rock to Player {q}'s side.\n";
                }
            }
        }
        return ret;
    }
}
