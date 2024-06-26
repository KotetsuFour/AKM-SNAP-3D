using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bartora : CharacterCard
{
    public new List<GameNotification> getResponse(GameNotification note)
    {
        List<GameNotification> ret = new List<GameNotification>();
        if (note.getNature() == GameNotification.Nature.TURN_START
            && positionState is LaneSegment
            && StaticData.board.turn == turnPlayed + 2)
        {
            GameNotification increase = new GameNotification(GameNotification.Nature.PERM_ALTER_POWER,
                true, this);
            increase.setInts(new int[] { 9 });
            ret.Add(increase);
        }
        return ret;
    }
}
