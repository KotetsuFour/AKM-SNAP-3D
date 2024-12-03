using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeakBoy : CharacterCard
{
    public override List<GameNotification> getResponse(GameNotification note)
    {
        if (!isMyOnReveal(note))
        {
            return null;
        }

        List<GameNotification> ret = new List<GameNotification>();

        int bonus = 0;
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
                        bonus++;
                    }
                }
            }
        }
        GameNotification buff = new GameNotification(GameNotification.Nature.PERM_ALTER_POWER, true, this);
        buff.setCards(new CharacterCard[] { this });
        buff.setInts(new int[] { bonus });
        ret.Add(buff);

        return ret;
    }
}
