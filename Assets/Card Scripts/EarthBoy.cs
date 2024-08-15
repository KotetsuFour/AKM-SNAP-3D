using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthBoy : CharacterCard
{
    [SerializeField] private CharacterCard rock;

    public new List<GameNotification> getResponse(GameNotification note)
    {
        if (!isMyOnReveal(note))
        {
            return null;
        }

        List<GameNotification> ret = new List<GameNotification>();
        Lane lane = ((LaneSegment)positionState).lane;
        for (int q = 0; q < lane.segments.Count; q++)
        {
            LaneSegment seg = lane.segments[q];
            if (seg == positionState || seg.isFull())
            {
                continue;
            }
            GameNotification create = new GameNotification(GameNotification.Nature.CREATE_CARD, true, this);
            create.setCards(new CharacterCard[] { rock });
            create.setPositions(new PositionState[] { seg });
            create.setInts(new int[] { q });

            ret.Add(create);
        }

        return ret;
    }
}
