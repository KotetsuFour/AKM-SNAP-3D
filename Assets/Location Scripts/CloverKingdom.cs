using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloverKingdom : Location
{
    public new bool canPlayHere(CharacterCard card, Board b)
    {
        int cost = card.getCost(b, lane);
        return cost <= 3;
    }
}
