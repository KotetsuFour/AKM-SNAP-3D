using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloverKingdom : Location
{
    [SerializeField] private int threshold;
    public override bool allowPlaceCard(CharacterCard card, LaneSegment seg)
    {
        if (seg.lane == lane && card.getCost() > threshold)
        {
            return false;
        }
        return true;
    }
}
