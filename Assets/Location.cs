using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Location : NotificationHandler
{
    public string locationName;
    public string abilityDescription;
    public Lane lane;

    public bool isMyLocationReveal(GameNotification note)
    {
        return note.getNature() == GameNotification.Nature.CHANGE_LOCATION
            && note.getLocations()[1] == this;
    }
}
