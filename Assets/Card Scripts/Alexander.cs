using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alexander : CharacterCard
{
    public new List<GameNotification> getResponse(GameNotification note)
    {
        List<GameNotification> ret = new List<GameNotification>();
        if (isMyOnReveal(note))
        {
            for (int q = 0; q < StaticData.numPlayers; q++)
            {
                if (q == myPlayer)
                {
                    continue;
                }
                List<CharacterCard> options = ((LaneSegment)positionState).lane.segments[q].getRevealedCards();
                if (options.Count <= 0)
                {
                    continue;
                }
                int idx = Random.Range(0, options.Count);
                GameNotification destroy = new GameNotification(GameNotification.Nature.RELOCATE_CARD, true, this);
                destroy.setCards(new CharacterCard[] { options[idx] });
                destroy.setPositions(new PositionState[] {
                    ((LaneSegment)positionState).lane.segments[q],
                    StaticData.board.destroyedCardPiles[q]
                });
                
                ret.Add(destroy);
            }
        }
        return ret;
    }
}
