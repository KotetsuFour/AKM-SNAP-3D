using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tavish : CharacterCard
{
    public new void changePermanentPower(int amount)
    {
        if (amount < 0)
        {
            return;
        }
        permanentAlterPower += amount;
    }
    public new void changeTemporaryPower(int amount)
    {
        if (amount < 0)
        {
            return;
        }
        temporaryAlterPower += amount;
    }
    public new void setBasePower(int p)
    {
        if (p < basePower)
        {
            return;
        }
        basePower = p;
    }
    public new void setBaseCost(int c)
    {
        baseCost = c;
    }
    public new void destroy(Board b, int player)
    {
        return;
    }



}
