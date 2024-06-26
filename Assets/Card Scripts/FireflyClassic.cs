using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyClassic : CharacterCard
{
    public new string onReveal(Board b)
    {
        string ret = "";
        Lane lane = b.getMyLane(this);
        for (int q = 0; q < lane.segments.Count; q++)
        {
            if (q == myPlayer)
            {
                continue;
            }
            for (int w = 0; w < lane.segments[q].Count; w++)
            {
                if (lane.segments[q][w].turnPlayed == turnPlayed)
                {
                    CharacterCard[] cards = lane.segments[q].ToArray();
                    ret += $"Player {myPlayer}'s {characterName} detected Player {q}'s {lane.segments[q][w].characterName} and targeted ";
                    foreach (CharacterCard card in cards)
                    {
                        if (card.revealed)
                        {
                            card.destroy(b, q);
                            ret += $"{card.characterName}, ";
                        }
                    }
                    ret += "for destruction.\n";
                    break;
                }
            }
        }
        return ret;
    }
}
