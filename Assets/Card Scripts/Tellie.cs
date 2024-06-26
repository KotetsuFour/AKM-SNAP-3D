using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tellie : CharacterCard
{
    public new string onReveal(Board b)
    {
        Lane lane = b.getMyLane(this);
        lane.revealLocation();
        return $"Player {myPlayer}'s {characterName} teleported this location to {lane.location.locationName}.\n";
    }
}
