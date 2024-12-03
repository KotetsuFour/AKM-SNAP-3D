using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowGirl : CharacterCard
{
    public override List<GameNotification> getResponse(GameNotification note)
    {
        if (note.getNature() != GameNotification.Nature.TURN_END)
        {
            return null;
        }
        List<GameNotification> ret = new List<GameNotification>();

        int bonus = StaticData.board.energies[myPlayer];
        GameNotification buff = new GameNotification(GameNotification.Nature.PERM_ALTER_POWER, true, this);
        buff.setCards(new CharacterCard[] { this });
        buff.setInts(new int[] { bonus });
        ret.Add(buff);

        return ret;
    }
}
