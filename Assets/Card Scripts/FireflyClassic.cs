using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyClassic : CharacterCard
{
    [SerializeField] private int threshold;
    public override List<GameNotification> getResponse(GameNotification note)
    {
        if (!isMyOnReveal(note))
        {
            return null;
        }
        List<GameNotification> ret = new List<GameNotification>();
        Lane lane = ((LaneSegment)positionState).lane;
        foreach (LaneSegment seg in lane.segments)
        {
            foreach (CharacterCard card in seg.cardsHere)
            {
                if (card.getPower() <= threshold)
                {
                    GameNotification destroy = new GameNotification(GameNotification.Nature.RELOCATE_CARD, true, this);
                    destroy.setCards(new CharacterCard[] { card });
                    destroy.setPositions(new PositionState[] { seg, StaticData.board.destroyedCardPiles[card.myPlayer] });
                    ret.Add(destroy);
                }
            }
        }
        return ret;
    }
}
