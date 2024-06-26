using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nera : CharacterCard
{
    public new string onReveal(Board b)
    {
        Lane lane = b.getMyLane(this);
        if (lane.segments[myPlayer].Count >= lane.cardsPerPlayerInThisLane)
        {
            return null;
        }
        CharacterCard toMove = null;
        int playerIdx = Random.Range(0, StaticData.numPlayers);
        if (playerIdx == myPlayer)
        {
            playerIdx = (playerIdx + 1) % StaticData.numPlayers;
        }
        while (playerIdx != myPlayer)
        {
            if (lane.segments[playerIdx].Count > 0)
            {
                toMove = lane.segments[playerIdx][Random.Range(0, lane.segments[playerIdx].Count)];
                break;
            }
            playerIdx = (playerIdx + 1) % StaticData.numPlayers;
        }
        if (toMove == null)
        {
            return null;
        }
        lane.segments[playerIdx].Remove(toMove);
        toMove.myPlayer = myPlayer;
        b.addToLane(lane, toMove, myPlayer);
        return $"Player {myPlayer}'s {characterName} won Player {playerIdx}'s {toMove.characterName} over to their side.\n";
    }
}
