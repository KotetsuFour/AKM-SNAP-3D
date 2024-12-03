using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugBoy : CharacterCard
{
    [SerializeField] private int baseBonus;
    private int currentBonus;

    private void Start()
    {
        currentBonus = baseBonus;
    }
    public override List<GameNotification> getResponse(GameNotification note)
    {
        List<GameNotification> ret = new List<GameNotification>();
        if (ongoingMultiplier > 0
            &&
            (note.getNature() == GameNotification.Nature.RELOCATE_CARD
            || note.getNature() == GameNotification.Nature.CREATE_CARD
            || note.getNature() == GameNotification.Nature.PLAY_CARD
            ))
        {
            GameNotification ong = new GameNotification(GameNotification.Nature.ONGOING, true, this);
            ong.setCards(new CharacterCard[] { note.getCharacterCards()[note.getCharacterCards().Length - 1] });
            ret.Add(ong);
        }
        else if (note.getNature() == GameNotification.Nature.ALTER_ONGOING
            && note.getCharacterCards()[0] == this)
        {
            GameNotification reverse = new GameNotification(GameNotification.Nature.TEMP_ALTER_POWER, false, this);
            reverse.setCards(new CharacterCard[] { this });
            reverse.setInts(new int[] { -currentBonus });
            ret.Add(reverse);

            currentBonus = Mathf.RoundToInt(ongoingMultiplier * baseBonus);

            GameNotification buff = new GameNotification(GameNotification.Nature.ONGOING, true, this);
            ret.Add(buff);
        }
        else if (isMyOngoing(note)
            && positionState is LaneSegment && positionState.isFull())
        {
            GameNotification buff = new GameNotification(GameNotification.Nature.TEMP_ALTER_POWER, false, this);
            buff.setCards(new CharacterCard[] { this });
            buff.setInts(new int[] { currentBonus });
            ret.Add(buff);
        }

        return ret;
    }
}
