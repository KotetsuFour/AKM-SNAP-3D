using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGuy : CharacterCard
{
    private bool[] gotten;
    private void Start()
    {
        gotten = new bool[StaticData.numPlayers];
    }
    public override List<GameNotification> getResponse(GameNotification note)
    {

        if (note.getNature() == GameNotification.Nature.REVEAL_CARD
            && note.getCharacterCards()[0].positionState != positionState
            && !gotten[note.getCharacterCards()[0].myPlayer])
        {
            List<GameNotification> ret = new List<GameNotification>();

            gotten[note.getCharacterCards()[0].myPlayer] = true;
            GameNotification destroy = new GameNotification(GameNotification.Nature.RELOCATE_CARD, true, this);
            destroy.setCards(new CharacterCard[] { note.getCharacterCards()[0] });
            destroy.setPositions(new PositionState[] { note.getCharacterCards()[0].positionState,
            StaticData.board.destroyedCardPiles[note.getCharacterCards()[0].myPlayer] });

            ret.Add(destroy);
            return ret;
        }
        return null;
    }
}
