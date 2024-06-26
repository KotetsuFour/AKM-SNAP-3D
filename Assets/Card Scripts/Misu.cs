using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Misu : CharacterCard
{
    public new string onReveal(Board b)
    {
        Lane lane = b.getMyLane(this);
        int moveToPlayer = (myPlayer + 1) % lane.segments.Count;
        while (moveToPlayer != myPlayer && lane.segments[moveToPlayer].Count >= lane.cardsPerPlayerInThisLane)
        {
            moveToPlayer = (moveToPlayer + 1) % lane.segments.Count;
        }
        if (moveToPlayer == myPlayer)
        {
            return null;
        }
        int startingPlayer = myPlayer;
        lane.segments[myPlayer].Remove(this);
        myPlayer = moveToPlayer;
        b.addToLane(lane, this, moveToPlayer);
        return $"Player {startingPlayer}'s {characterName} switched to Player {myPlayer}'s side.\n";
    }
}
