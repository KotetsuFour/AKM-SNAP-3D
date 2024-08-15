using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanArsenal : CharacterCard
{
    [SerializeField] private int bonus;
    public new List<GameNotification> getResponse(GameNotification note)
    {
        if (!isMyOnReveal(note))
        {
            return null;
        }
        List<GameNotification> ret = new List<GameNotification>();
        Lane lane = ((LaneSegment)positionState).lane;
        foreach (LaneSegment seg in lane.segments)
        {
            if (seg == positionState)
            {
                continue;
            }
            foreach (CharacterCard card in seg.cardsHere)
            {
                if (card.attributes.Contains(Attribute.INNATE_MAGIC))
                {
                    GameNotification buff = new GameNotification(GameNotification.Nature.PERM_ALTER_POWER, true, this);
                    buff.setCards(new CharacterCard[] { this });
                    buff.setInts(new int[] { bonus });
                    ret.Add(buff);
                    return ret;
                }
            }
        }
        return null;
    }
}
