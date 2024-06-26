using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueenOfClovers : CharacterCard
{
    public new int getCost(Board board, Lane lane)
    {
        foreach (CharacterCard c in lane.segments[myPlayer])
        {
            if (c.characterName == "Narkio")
            {
                return baseCost + permanentAlterCost + temporaryAlterCost - 1;
            }
        }
        return baseCost + permanentAlterCost + temporaryAlterCost;
    }
}
