using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Misu : CharacterCard
{
    public new List<GameNotification> getResponse(GameNotification note)
    {
        if (!isMyOnReveal(note))
        {
            return null;
        }
        List<GameNotification> ret = new List<GameNotification>();

        int dest = (myPlayer + 1) % StaticData.numPlayers;
        while (((LaneSegment)positionState).lane.segments[dest].isFull()
            && dest != myPlayer)
        {
            dest = (dest + 1) % StaticData.numPlayers;
        }
        if (dest != myPlayer)
        {
            GameNotification move = new GameNotification(GameNotification.Nature.RELOCATE_CARD, true, this);
            move.setCards(new CharacterCard[] { this });
            move.setPositions(new PositionState[] { positionState, ((LaneSegment)positionState).lane.segments[dest] });
            ret.Add(move);
        }
        return ret;
    }
}
