using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arkham : Location
{
    [SerializeField] private int costToSummon;
    public override List<GameNotification> getResponse(GameNotification note)
    {
        if (!isMyLocationReveal(note))
        {
            return null;
        }
        List<GameNotification> ret = new List<GameNotification>();
        foreach (PositionState hand in StaticData.board.hands)
        {
            GameNotification create = new GameNotification(GameNotification.Nature.CREATE_CARD, true, this);
            create.setCards(new CharacterCard[] { StaticData.getRandomCostCard(costToSummon) });
            create.setPositions(new PositionState[] { hand });
            ret.Add(create);
        }

        return ret;
    }
}
