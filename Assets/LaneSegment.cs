using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LaneSegment : PositionState
{
    public Lane lane;
    public int calculatedPower;
    public int extraPower;

    private TextMeshProUGUI powerDisplay;

    // Start is called before the first frame update
    void Start()
    {
        powerDisplay = StaticData.findDeepChild(lane.transform, "Power" + myPlayer)
            .GetComponent<TextMeshProUGUI>();
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
        calculatedPower = 0;
        for (int q = 0; q < cardsHere.Count; q++)
        {
            if (!cardsHere[q].revealed)
            {
                continue;
            }
            calculatedPower += cardsHere[q].getPower();
        }
        calculatedPower += extraPower;

        powerDisplay.text = "" + calculatedPower;
    }

}
