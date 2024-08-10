using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodyLilah : CharacterCard
{
    public new GameNotification.Permission allowNotification(GameNotification note)
    {
        if (note.getNature() == GameNotification.Nature.ON_REVEAL)
        {
            foreach (LaneSegment seg in ((LaneSegment)positionState).lane.segments)
            {
                if (seg.cardsHere.Contains(note.getCharacterCards()[0]))
                {
                    return new GameNotification.Permission(this, false, 2);
                }
            }
        }
        return new GameNotification.Permission(this, true);
    }
}
