using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelenaXuzhu : CharacterCard
{
    public override List<GameNotification> getResponse(GameNotification note)
    {
        if (!isMyOnReveal(note))
        {
            return null;
        }
        List<GameNotification> ret = new List<GameNotification>();
        if (StaticData.board.decks[myPlayer].cardsHere.Count > 0
            && !positionState.isFull())
        {
            GameNotification draw = new GameNotification(GameNotification.Nature.RELOCATE_CARD, true, null);
            draw.setCards(new CharacterCard[] { StaticData.board.decks[myPlayer].topCard() });
            draw.setPositions(new PositionState[] { StaticData.board.decks[myPlayer], StaticData.board.hands[myPlayer] });
        }
        return ret;
    }

}
