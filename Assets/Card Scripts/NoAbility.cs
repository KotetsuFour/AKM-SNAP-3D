using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NoAbility : CharacterCard
{
    private bool activated;

    public void activate()
    {
        activated = true;
    }

    public new List<GameNotification> getResponse(GameNotification note)
    {
        if (activated)
        {
            return secretAbility(note);
        }
        return null;
    }
    public abstract List<GameNotification> secretAbility(GameNotification note);
}
