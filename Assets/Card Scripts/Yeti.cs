using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yeti : CharacterCard
{
    [SerializeField] private int penalty;

    public new List<GameNotification> getResponse(GameNotification note)
    {
        if (!isMyOnReveal(note))
        {
            return null;
        }

        List<GameNotification> ret = new List<GameNotification>();
        for (int q = 0; q < StaticData.board.hands.Count; q++)
        {
            if (q == myPlayer)
            {
                continue;
            }
            foreach (CharacterCard card in StaticData.board.hands[q].cardsHere)
            {
                if (card.getCost() < 6)
                {
                    GameNotification pen = new GameNotification(GameNotification.Nature.ALTER_COST, true, this);
                    pen.setCards(new CharacterCard[] { card });
                    pen.setInts(new int[] { 1 });
                    ret.Add(pen);
                    break;
                }
            }
        }
        return ret;
    }
}
