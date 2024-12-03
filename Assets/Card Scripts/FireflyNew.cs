using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyNew : CharacterCard
{
    public override List<GameNotification> getResponse(GameNotification note)
    {
        if (!isMyOnReveal(note))
        {
            return null;
        }
        List<GameNotification> ret = new List<GameNotification>();

        foreach (CharacterCard card in StaticData.board.hands[myPlayer].cardsHere)
        {
            if (card.getCost() > 1)
            {
                GameNotification decrease = new GameNotification(GameNotification.Nature.ALTER_COST, true, this);
                decrease.setInts(new int[] { -1 });
                ret.Add(decrease);
            }
        }
        return ret;
    }
}
