using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arkham : Location
{
    public new string ongoingEffect(Board b)
    {
        if (b.turn != 3)
        {
            return null;
        }
        List<CharacterCard[]> cards = new List<CharacterCard[]>();
        for (int q = 0; q < lane.segments.Count; q++)
        {
            cards.Add(lane.segments[q].ToArray());
            lane.segments[q].Clear();
        }
        for (int q = 0; q < lane.players.Count; q++)
        {
            int switchToPlayerIdx = (q + 1) % lane.players.Count;
            for (int w = 0; w < cards[lane.players[q]].Length; w++)
            {
                CharacterCard toSwitch = cards[lane.players[q]][w];
                toSwitch.myPlayer = lane.players[switchToPlayerIdx];
                b.addToLane(lane, toSwitch, lane.players[switchToPlayerIdx]);
            }
        }
        return $"{locationName} switched the sides of cards located there.\n";
    }
}
