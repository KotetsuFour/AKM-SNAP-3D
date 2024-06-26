using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Location : NotificationHandler
{
    public string locationName;
    public string abilityDescription;
    public Lane lane;
    public string onLocationReveal(Board b) { return null; }
    public string onCardReveal(CharacterCard card, Board b) { return null; }
    public string ongoingEffect(Board b) { return null; }
    public bool canReveal(Board b) { return true; }
}
