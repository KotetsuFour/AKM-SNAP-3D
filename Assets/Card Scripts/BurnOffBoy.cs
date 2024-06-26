using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnOffBoy : CharacterCard
{
    public new List<GameNotification> getResponse(GameNotification note)
    {
        if (!isMyOnReveal(note))
        {
            return null;
        }
        List<GameNotification> ret = new List<GameNotification>();
        foreach (Lane lane in StaticData.board.lanes)
        {
            for (int q = 0; q < lane.segments.Count; q++)
            {
                if (q == myPlayer)
                {
                    continue;
                }
                foreach (CharacterCard card in lane.segments[q].cardsHere)
                {
                    if (card.attributes.Contains(Attribute.WATER))
                    {
                        GameNotification dry = new GameNotification(GameNotification.Nature.PERM_ALTER_POWER, true, this);
                        dry.setCards(new CharacterCard[] { card });
                        dry.setInts(new int[] { -1 });
                        ret.Add(dry);
                    }
                }
            }
        }
        return ret;
    }
}
