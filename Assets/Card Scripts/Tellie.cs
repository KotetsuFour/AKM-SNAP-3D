using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tellie : CharacterCard
{
    public override List<GameNotification> getResponse(GameNotification note)
    {
        if (!isMyOnReveal(note))
        {
            return null;
        }
        List<GameNotification> ret = new List<GameNotification>();

        GameNotification change = new GameNotification(GameNotification.Nature.CHANGE_LOCATION, true, this);
        change.setLocations(new Location[] { ((LaneSegment)positionState).lane.location, StaticData.getRandomLocation() });
        ret.Add(change);

        return ret;
    }
}
