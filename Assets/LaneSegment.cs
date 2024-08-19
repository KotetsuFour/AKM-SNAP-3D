using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneSegment : PositionState
{
    public Lane lane;
    public int calculatedPower;
    public int extraPower;

    // Start is called before the first frame update
    void Start()
    {

    }
    public List<CharacterCard> getRevealedCards()
    {
        List<CharacterCard> ret = new List<CharacterCard>();
        foreach (CharacterCard card in cardsHere)
        {
            if (card.revealed)
            {
                ret.Add(card);
            }
        }
        return ret;
    }
    public void calculatePowers()
    {
        for (int q = 0; q < cardsHere.Count; q++)
        {
            calculatedPower = 0;
            for (int w = 0; w < cardsHere.Count; w++)
            {
                if (!cardsHere[q].revealed)
                {
                    continue;
                }
                calculatedPower += cardsHere[q].getPower();
            }
            calculatedPower += extraPower;
        }
    }

}
