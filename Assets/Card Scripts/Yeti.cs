using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yeti : CharacterCard
{
    public new string onReveal(Board b)
    {
        int playerToFreeze = Random.Range(0, StaticData.numPlayers);
        if (playerToFreeze == myPlayer)
        {
            playerToFreeze = (playerToFreeze + 1) % StaticData.numPlayers;
        }
        if (b.hands[playerToFreeze].Count > 0)
        {
            b.hands[playerToFreeze][Random.Range(0, b.hands[playerToFreeze].Count)].changeCost(1);
        }
        return $"Player {myPlayer}'s {characterName} targeted a card in Player {playerToFreeze}'s hand to give +1 Cost.\n";
    }
}
