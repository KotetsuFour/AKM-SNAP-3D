using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireflyNew : CharacterCard
{
    public new string onReveal(Board b)
    {
        List<CharacterCard> myHand = b.hands[myPlayer];
        for (int q = 0; q < myHand.Count; q++)
        {
            myHand[q].changeCost(-1);
        }
        return $"Player {myPlayer}'s {characterName} reduced the cost of cards in their hand by 1.\n";
    }
}
