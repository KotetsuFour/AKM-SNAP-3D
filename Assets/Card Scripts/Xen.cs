using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xen : CharacterCard
{
    public override List<GameNotification> getResponse(GameNotification note)
    {
        if (!isMyOnReveal(note))
        {
            return null;
        }
        CharacterCard last = StaticData.board.archive.playedCards[myPlayer][StaticData.board.archive.playedCards[myPlayer].Count - 1];
        if (last != null && last.positionState is LaneSegment)
        {
            List<GameNotification> ret = new List<GameNotification>();
            GameNotification alter = new GameNotification(GameNotification.Nature.PERM_ALTER_POWER, true, this);
            alter.setCards(new CharacterCard[] { this });
            alter.setInts(new int[] { last.getPermanentPower() - getPermanentPower() });
            ret.Add(alter);

            return ret;
        }

        return null;
    }
}
