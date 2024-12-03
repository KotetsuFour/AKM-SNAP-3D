using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tavish : CharacterCard
{
    public override GameNotification.Permission allowNotification(GameNotification note)
    {
        if (
            (note.getNature() == GameNotification.Nature.RELOCATE_CARD
            && note.getCharacterCards()[0] == this
            && note.getPositions()[0] is LaneSegment
            )
            ||
            ((note.getNature() == GameNotification.Nature.PERM_ALTER_POWER || note.getNature() == GameNotification.Nature.TEMP_ALTER_POWER)
            && note.getCharacterCards()[0] == this
            && note.getInts()[0] < 0
            ))
        {
            return new GameNotification.Permission(this, false);
        }
        return new GameNotification.Permission(this, true);
    }
}
