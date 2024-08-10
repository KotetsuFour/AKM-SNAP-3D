using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugBoy : CharacterCard
{
    [SerializeField] private int bonus;
    public new int getPower(Board b)
    {
        if (positionState is LaneSegment
            && positionState.cardsHere.Count >= positionState.maxCardsAllowed)
        {
            return base.getPower(b) + bonus;
        }
        return base.getPower(b);
    }
}
