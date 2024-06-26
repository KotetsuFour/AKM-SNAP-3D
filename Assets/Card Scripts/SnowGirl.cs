using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowGirl : CharacterCard
{
    public new string continuousEffect(Board b)
    {
        int extra = b.energies[myPlayer];
        changePermanentPower(extra);
        return $"Player {myPlayer}'s {characterName} gains {extra} Power from unspent energy.\n";
    }
}
