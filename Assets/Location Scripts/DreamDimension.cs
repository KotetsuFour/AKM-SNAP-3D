using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreamDimension : Location
{
    public new List<GameNotification> getResponse(GameNotification note)
    {
        if (!isMyLocationReveal(note))
        {
            return null;
        }
        List<GameNotification> ret = new List<GameNotification>();
        foreach (Deck deck in StaticData.board.decks)
        {
            foreach (CharacterCard card in deck.cardsHere)
            {
                GameNotification replace = new GameNotification(GameNotification.Nature.TRANSFORM_CARD, false, this);
                replace.setCards(new CharacterCard[] { card, StaticData.getRandomCard() });
                ret.Add(replace);
            }
        }
        return ret;
    }
}
