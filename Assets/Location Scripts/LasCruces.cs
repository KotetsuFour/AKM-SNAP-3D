using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LasCruces : Location
{
    public new string ongoingEffect(Board b)
    {
        List<CharacterCard> affected = new List<CharacterCard>();
        for (int q = 0; q < lane.segments.Count; q++)
        {
            for (int w = 0; w < lane.segments[q].Count; w++)
            {
                CharacterCard card = lane.segments[q][w];
                if (card != null && (card.attributes.Contains(CharacterCard.Attribute.FIRE)
                    || card.attributes.Contains(CharacterCard.Attribute.WATER)
                    || card.attributes.Contains(CharacterCard.Attribute.EARTH)
                    || card.attributes.Contains(CharacterCard.Attribute.AIR)
                    || card.attributes.Contains(CharacterCard.Attribute.LIGHTNING)
                    || card.attributes.Contains(CharacterCard.Attribute.ICE)
                    || card.attributes.Contains(CharacterCard.Attribute.LIFE)))
                {
                    affected.Add(card);
                    card.changeTemporaryPower(2);
                }
            }
        }
        if (affected.Count == 0)
        {
            return null;
        }
        string ret = $"{locationName} increases the Power of ";
        foreach (CharacterCard card in affected)
        {
            ret += $"{card.characterName}, ";
        }
        ret += "by 2.\n";
        return ret;
    }
}
