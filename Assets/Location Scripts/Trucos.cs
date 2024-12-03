using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trucos : Location
{
    public override GameNotification.Permission allowNotification(GameNotification note)
    {
        if (note.getNature() == GameNotification.Nature.REVEAL_CARD
            && lane.segments.Contains((LaneSegment)note.getCharacterCards()[0].positionState)
            && StaticData.board.turn < StaticData.board.lastTurn)
        {
            return new GameNotification.Permission(this, false);
        }
        return new GameNotification.Permission(this, true);
    }
}
