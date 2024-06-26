using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamDimension : Location
{
    private int turnRevealed;
    public new string onLocationReveal(Board b)
    {
        turnRevealed = b.turn;
        return null;
    }
    public new string ongoingEffect(Board b)
    {
        if (b.turn != turnRevealed)
        {
            return null;
        }
        for (int q = 0; q < lane.segments.Count; q++)
        {
            CharacterCard[] cards = lane.segments[q].ToArray();
            for (int w = 0; w < cards.Length; w++)
            {
                Destroy(cards[w]);
                lane.segments[q].Remove(cards[w]);
                b.addToLane(lane, Instantiate(StaticData.allCards[Random.Range(0, StaticData.allCards.Count)]), q);
            }
        }
        for (int q = 0; q < lane.segments.Count; q++)
        {
            for (int w = 0; w < lane.segments[q].Count; w++)
            {
                lane.segments[q][w].onReveal(b);
            }
        }
        return $"All cards at {locationName} become random cards.\n";
    }
}
