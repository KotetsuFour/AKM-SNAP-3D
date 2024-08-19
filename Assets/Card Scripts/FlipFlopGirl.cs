using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipFlopGirl : CharacterCard
{
    [SerializeField] private int bonus;
    public new List<GameNotification> getResponse(GameNotification note)
    {
        if (!isMyOnReveal(note))
        {
            return null;
        }
        List<GameNotification> ret = new List<GameNotification>();
        foreach (Lane lane in StaticData.board.lanes)
        {
            foreach (CharacterCard card in lane.segments[myPlayer].cardsHere)
            {
                if (card != this && card.attributes.Contains(Attribute.WATER))
                {
                    GameNotification buff = new GameNotification(GameNotification.Nature.PERM_ALTER_POWER, true, this);
                    buff.setCards(new CharacterCard[] { card });
                    buff.setInts(new int[] { bonus });
                    ret.Add(buff);
                }
            }
        }
        return ret;
    }
}
