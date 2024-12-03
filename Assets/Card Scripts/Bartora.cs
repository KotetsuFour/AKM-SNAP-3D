using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bartora : CharacterCard
{
    private int completionThreshold;
    private int followThrough;
    [SerializeField] private int bonus;
    public override List<GameNotification> getResponse(GameNotification note)
    {
        List<GameNotification> ret = new List<GameNotification>();
        if (isMyOnReveal(note))
        {
            completionThreshold++;
        }
        else if (note.getNature() == GameNotification.Nature.TURN_START
            && positionState is LaneSegment
            && StaticData.board.turn == turnPlayed + 2)
        {
            while (completionThreshold > followThrough)
            {
                followThrough++;
                GameNotification increase = new GameNotification(GameNotification.Nature.PERM_ALTER_POWER,
                    true, this);
                increase.setInts(new int[] { bonus });
                ret.Add(increase);
            }
        }
        else if (note.getNature() == GameNotification.Nature.RELOCATE_CARD
            && !(positionState is LaneSegment))
        {
            completionThreshold = 0;
            followThrough = 0;
        }
        return ret;
    }
}
