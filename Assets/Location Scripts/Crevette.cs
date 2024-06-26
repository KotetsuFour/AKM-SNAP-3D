using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crevette : Location
{
    public new string ongoingEffect(Board b)
    {
        List<CharacterCard> affected = new List<CharacterCard>();
        for (int q = 0; q < lane.segments.Count; q++)
        {
            for (int w = 0; w < lane.segments[q].Count; w++)
            {
                CharacterCard card = lane.segments[q][w];
                if (card.revealed && card.attributes.Contains(CharacterCard.Attribute.INNATE_MAGIC))
                {
                    affected.Add(card);
                    card.changeTemporaryPower(-3);
                }
            }
        }
        if (affected.Count == 0)
        {
            return null;
        }
        string ret = $"{locationName} reduced the Power of ";
        foreach (CharacterCard card in affected)
        {
            ret += $"{card.characterName}, ";
        }
        ret += "by 3.\n";
        return ret;
    }
}
