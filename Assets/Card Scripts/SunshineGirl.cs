using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunshineGirl : CharacterCard
{
    public new string onReveal(Board b)
    {
        string ret = "";
        foreach (Lane lane in b.lanes)
        {
            for (int q = 0; q < lane.segments[myPlayer].Count; q++)
            {
                if (lane.segments[myPlayer][q] != this
                    && lane.segments[myPlayer][q].abilityDescription.Contains("Ongoing: "))
                {
                    ret += $"Player {myPlayer}'s {characterName} increases their {lane.segments[myPlayer][q].characterName}'s Power by 2.\n";
                    lane.segments[myPlayer][q].changePermanentPower(2);
                }
            }
        }
        return ret;
    }
}
