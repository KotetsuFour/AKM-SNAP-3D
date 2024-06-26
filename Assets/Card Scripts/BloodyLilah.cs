using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodyLilah : CharacterCard
{
    public new List<GameNotification> getResponse(GameNotification note) {
        List<GameNotification> ret = new List<GameNotification>();
        if (!isMyOnReveal(note))
        {
            return null;
        }
        foreach (Lane lane in StaticData.board.lanes)
        {
            for (int q = 0; q < lane.segments.Count; q++)
            {
                LaneSegment seg = lane.segments[q];
                foreach (CharacterCard card in seg.cardsHere)
                {
                    if (card.characterName == "Lucid Lilah")
                    {
                        GameNotification putBack = new GameNotification(GameNotification.Nature.RELOCATE_CARD, true, this);
                        putBack.setCards(new CharacterCard[] { card });
                        putBack.setPositions(new PositionState[] { seg, StaticData.board.hands[q] });
                        ret.Add(putBack);
                    }
                }
            }
        }
        return ret;
    }
}
