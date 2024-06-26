using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugBoy : CharacterCard
{
    public new int getPower(Board b)
    {
        if (positionState is LaneSegment
            && positionState.cardsHere.Count >= positionState.maxCardsAllowed)
        {
            return basePower + permanentAlterPower + temporaryAlterCost + 3;
        }
        return basePower + permanentAlterPower + temporaryAlterPower;
    }
}
