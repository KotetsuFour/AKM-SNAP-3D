using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cereus : CharacterCard
{
    [SerializeField] private int bonus;
    private int turnToCheck;
    public override List<GameNotification> getResponse(GameNotification note)
    {
        List<GameNotification> ret = new List<GameNotification>();
        if (!isMyOnReveal(note))
        {
            if (note.getNature() == GameNotification.Nature.PLAY_CARD
                && turnToCheck != 0 && StaticData.board.turn == turnPlayed + 1
                && note.getCharacterCards()[0].positionState == positionState)
            {
                GameNotification gain = new GameNotification(GameNotification.Nature.PERM_ALTER_POWER, true, this);
                gain.setCards(new CharacterCard[] { this } );
                gain.setInts(new int[] { bonus });
                ret.Add(gain);
            }
            else
            {
                return null;
            }
        }
        else
        {
            turnToCheck = StaticData.board.turn + 1;
        }

        return ret;
    }
}
