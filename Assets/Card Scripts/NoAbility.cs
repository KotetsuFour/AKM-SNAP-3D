using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoAbility : CharacterCard
{
    private bool activated;

    public void activate()
    {
        activated = true;
    }
    public override GameNotification.Permission allowNotification(GameNotification note)
    {
        if (activated)
        {
            return secretAllowNotification(note);
        }
        return new GameNotification.Permission(this, true);
    }
    public override GameNotification.Permission allowPermission(GameNotification.Permission note)
    {
        if (activated)
        {
            return secretAllowPermission(note);
        }
        return new GameNotification.Permission(this, true);
    }

    public override List<GameNotification> getResponse(GameNotification note)
    {
        if (activated)
        {
            return secretAbility(note);
        }
        return null;
    }
    public List<GameNotification> secretAbility(GameNotification note)
    {
        return base.getResponse(note);
    }
    public GameNotification.Permission secretAllowNotification(GameNotification note)
    {
        return base.allowNotification(note);
    }
    public GameNotification.Permission secretAllowPermission(GameNotification.Permission permit)
    {
        return base.allowPermission(permit);
    }

}
