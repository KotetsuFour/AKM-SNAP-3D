using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlinkyMan : CharacterCard
{
    public new string continuousEffect(Board b)
    {
        string ret = "";
        foreach (Lane lane in b.lanes)
        {
            if (lane.players.Contains(myPlayer))
            {
                foreach (CharacterCard c in lane.segments[myPlayer])
                {
                    if (c.series == Series.KOTETSU_CLASSIC || c.series == Series.BROTHER || c.series == Series.SISTER)
                    {
                        ret += $"Player {myPlayer}'s {characterName} gives their {c.characterName} 1 Power this turn.\n";
                        c.changeTemporaryPower(1);
                    }
                }
            }
        }
        return ret;
    }
}
