using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golyat : CharacterCard
{
    public new List<GameNotification> getResponse(GameNotification note)
    {
        List<GameNotification> ret = new List<GameNotification>();
        if (note.getNature() == GameNotification.Nature.RELOCATE_CARD
            &&
            (note.getPositions()[1] == StaticData.board.destroyedCardPiles[myPlayer]
            || note.getPositions()[1] == StaticData.board.discardPiles[myPlayer]))
        {
            int laneIdx = StaticData.getRandomInt(StaticData.board.lanes.Length);
            int done = laneIdx;
            do
            {
                laneIdx = (laneIdx + 1) % StaticData.board.lanes.Length;
            } while (laneIdx != done
                    &&
                    (!StaticData.board.lanes[laneIdx].players.Contains(myPlayer)
                    || StaticData.board.lanes[laneIdx].segments[myPlayer].isFull()));

            Lane testLane = StaticData.board.lanes[laneIdx];
            if (testLane.players.Contains(myPlayer) && !testLane.segments[myPlayer].isFull())
            {
                GameNotification regen = new GameNotification(GameNotification.Nature.RELOCATE_CARD, true, this);
                regen.setCards(new CharacterCard[] { this });
                regen.setPositions(new PositionState[] { positionState, testLane.segments[myPlayer] });

                ret.Add(regen);
            }

            StaticData.board.extraEnergies[myPlayer]++;
        }

        return ret;
    }
}
