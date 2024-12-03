using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feng : CharacterCard
{
    [SerializeField] private int bonus;

    public override List<GameNotification> getResponse(GameNotification note)
    {
        if (!isMyOnReveal(note))
        {
            return null;
        }
        List<GameNotification> ret = new List<GameNotification>();
        List<LaneSegment> segments = ((LaneSegment)positionState).lane.segments;
        foreach (LaneSegment seg in segments)
        {
            if (seg == positionState)
            {
                continue;
            }
            foreach (CharacterCard card in seg.cardsHere)
            {
                if (card.turnPlayed == StaticData.board.turn)
                {
                    GameNotification buff = new GameNotification(GameNotification.Nature.PERM_ALTER_POWER, true, this);
                    buff.setCards(new CharacterCard[] { this });
                    buff.setInts(new int[] { bonus });
                    ret.Add(buff);
                    return ret;
                }
            }
        }

        return ret;
    }
}
