using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenOfClovers : CharacterCard
{
    [SerializeField] private int summonCost;
    public new List<GameNotification> getResponse(GameNotification note)
    {
        if (!isMyOnReveal(note))
        {
            return null;
        }
        List<GameNotification> ret = new List<GameNotification>();
        foreach (Lane lane in StaticData.board.lanes)
        {
            if (lane == ((LaneSegment)positionState).lane
                || !lane.players.Contains(myPlayer)
                || lane.segments[myPlayer].isFull())
            {
                continue;
            }
            GameNotification summon = new GameNotification(GameNotification.Nature.CREATE_CARD, true, this);
            summon.setCards(new CharacterCard[] { StaticData.getRandomCostCard(summonCost) });
            summon.setPositions(new PositionState[] { lane.segments[myPlayer] });
            summon.setInts(new int[] { myPlayer });
            ret.Add(summon);
        }
        return ret;
    }
}
