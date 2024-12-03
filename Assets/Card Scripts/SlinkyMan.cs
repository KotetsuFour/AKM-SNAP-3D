using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlinkyMan : CharacterCard
{
    [SerializeField] private int baseBonus;
    private int currentBonus;

    private List<CharacterCard> affected;

    private void Start()
    {
        affected = new List<CharacterCard>();
        currentBonus = baseBonus;
    }
    public override List<GameNotification> getResponse(GameNotification note)
    {
        List<GameNotification> ret = new List<GameNotification>();
        if (ongoingMultiplier > 0
            &&
            (note.getNature() == GameNotification.Nature.RELOCATE_CARD
            || note.getNature() == GameNotification.Nature.CREATE_CARD
            || note.getNature() == GameNotification.Nature.TRANSFORM_CARD
            || note.getNature() == GameNotification.Nature.PLAY_CARD
            || note.getNature() == GameNotification.Nature.REVEAL_CARD
            ))
        {
            GameNotification ong = new GameNotification(GameNotification.Nature.ONGOING, true, this);
            ong.setCards(new CharacterCard[] { note.getCharacterCards()[note.getCharacterCards().Length - 1] });
            ret.Add(ong);
        }
        else if (note.getNature() == GameNotification.Nature.ALTER_ONGOING
            && note.getCharacterCards()[0] == this)
        {
            foreach (CharacterCard card in affected)
            {
                GameNotification reverse = relieve(card);
                ret.Add(reverse);
            }
            currentBonus = Mathf.RoundToInt(ongoingMultiplier * baseBonus);
            foreach (CharacterCard card in affected)
            {
                GameNotification buff = affect(card);
                ret.Add(buff);
            }
        }
        else if (isMyOngoing(note)
            && note.getCharacterCards()[0] != this
            &&
            (note.getCharacterCards()[0].series == Series.KOTETSU_CLASSIC
            || note.getCharacterCards()[0].series == Series.BROTHER
            || note.getCharacterCards()[0].series == Series.SISTER)
            )
        {
            CharacterCard card = note.getCharacterCards()[0];
            if (!affected.Contains(card) && isOnMySideOfTheField(card))
            {
                affected.Add(card);
                ret.Add(affect(card));
            }
            else if (affected.Contains(card) && !isOnMySideOfTheField(card))
            {
                affected.Remove(card);
                ret.Add(relieve(card));
            }
        }

        return ret;
    }

    private GameNotification relieve(CharacterCard card)
    {
        GameNotification reverse = new GameNotification(GameNotification.Nature.TEMP_ALTER_POWER, false, this);
        reverse.setCards(new CharacterCard[] { card });
        reverse.setInts(new int[] { -currentBonus });
        return reverse;
    }

    private GameNotification affect(CharacterCard card)
    {
        GameNotification buff = new GameNotification(GameNotification.Nature.TEMP_ALTER_POWER, false, this);
        buff.setCards(new CharacterCard[] { card });
        buff.setInts(new int[] { currentBonus });
        return buff;
    }
}
