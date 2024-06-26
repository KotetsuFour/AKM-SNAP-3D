using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xen : CharacterCard
{
    public new string onReveal(Board b)
    {
        CharacterCard muse = null;
        foreach (Lane lane in b.lanes)
        {
            foreach (CharacterCard card in lane.segments[myPlayer])
            {
                if (card.revealed && (muse == null || muse.getPower(b) < card.getPower(b)))
                {
                    muse = card;
                }
            }
        }
        if (muse == null)
        {
            return null;
        }
        setBasePower(muse.basePower);
        changePermanentPower(muse.permanentAlterPower);
        changeTemporaryPower(muse.temporaryAlterPower);
        attributes = new List<Attribute>();
        attributes.AddRange(muse.attributes);
        return $"Player {myPlayer}'s {characterName} mimics their {muse.characterName}'s Power and Attributes.\n";
    }
}
