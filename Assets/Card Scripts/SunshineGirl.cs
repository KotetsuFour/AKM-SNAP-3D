using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunshineGirl : CharacterCard
{
    [SerializeField] private int bonus;
    private List<LaneSegment> activationLanes;
    private void Start()
    {
        activationLanes = new List<LaneSegment>();
    }
    public override List<GameNotification> getResponse(GameNotification note)
    {
        List<GameNotification> ret = new List<GameNotification>();
        if (isMyOnReveal(note))
        {
            activationLanes.Add((LaneSegment)positionState);
        }
        else if (note.getNature() == GameNotification.Nature.PLAY_CARD
            && activationLanes.Contains((LaneSegment)note.getPositions()[0]))
        {
            LaneSegment seg = (LaneSegment)note.getPositions()[0];
            while (activationLanes.Contains(seg))
            {
                activationLanes.Remove(seg);
            }
        }
        else if (note.getNature() == GameNotification.Nature.FINALIZE_PLAY_PHASE
            && positionState is LaneSegment)
        {
            while (activationLanes.Count > 0)
            {
                GameNotification buff = new GameNotification(GameNotification.Nature.PERM_ALTER_POWER, true, this);
                buff.setCards(new CharacterCard[] { this });
                buff.setInts(new int[] { bonus });

                ret.Add(buff);

                activationLanes.RemoveAt(0);
            }
        }
        else if (note.getNature() == GameNotification.Nature.RELOCATE_CARD
            && !(positionState is LaneSegment))
        {
            activationLanes = new List<LaneSegment>();
        }
        else
        {
            return null;
        }

        return ret;
    }
}
