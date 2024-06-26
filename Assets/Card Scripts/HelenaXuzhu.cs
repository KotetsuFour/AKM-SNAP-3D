using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelenaXuzhu : CharacterCard
{
    public new string onReveal(Board b)
    {
        Lane lane = b.getMyLane(this);
        if (!lane.canPlayHere())
        {
            return null;
        }
        for (int q = 0; q < b.hands[myPlayer].Count; q++)
        {
            if (b.hands[myPlayer][q].characterName == "Slinky Man")
            {
                CharacterCard ricardo = b.hands[myPlayer][q];
                b.hands[myPlayer].RemoveAt(q);
                b.addToLane(lane, ricardo, myPlayer);
                return $"Player {myPlayer}'s {characterName} summoned {ricardo.characterName} from their hand.\n";
            }
        }
        int idx = -1;
        for (int q = 0; q < StaticData.allCards.Count; q++)
        {
            if (StaticData.allCards[q].characterName == "Slinky Man")
            {
                idx = q;
                break;
            }
        }
        if (idx != -1 && b.decks[myPlayer].cards.Contains(idx))
        {
            int ricardoIdx = b.decks[myPlayer].cards.IndexOf(idx);
            CharacterCard ricardo = b.decks[myPlayer].drawIdx(ricardoIdx);
            b.addToLane(lane, ricardo, myPlayer);
            return $"Player {myPlayer}'s {characterName} summoned {ricardo.characterName} from their deck.\n";
        }
        return null;
    }
}
