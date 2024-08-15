using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnrevealedLocation : Location
{
    [SerializeField] private int turnToReveal;

    public new List<GameNotification> getResponse(GameNotification note)
    {
        if (note.getNature() == GameNotification.Nature.TURN_START
            && StaticData.board.turn >= turnToReveal)
        {
            List<GameNotification> ret = new List<GameNotification>();

            GameNotification reveal = new GameNotification(GameNotification.Nature.CHANGE_LOCATION, false, this);
            Location setTo = StaticData.getRandomLocation();
            reveal.setLocations(new Location[] { this, setTo });
            ret.Add(reveal);

            return ret;
        }
        return null;
    }
}
