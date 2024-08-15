using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nera : CharacterCard
{
    [SerializeField] private List<int> protectedCosts;
    public new GameNotification.Permission allowNotification(GameNotification note)
    {
        if (note.getNature() == GameNotification.Nature.RELOCATE_CARD
            && note.getPositions()[1] == StaticData.board.destroyedCardPiles[myPlayer]
            && note.getCharacterCards()[0].myPlayer == myPlayer
            && protectedCosts.Contains(note.getCharacterCards()[0].baseCost))
        {
            return new GameNotification.Permission(this, false);
        }
        return new GameNotification.Permission(this, true);
    }
}
