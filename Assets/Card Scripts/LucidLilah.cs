using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LucidLilah : CharacterCard
{
    [SerializeField] private CharacterCard dreamRaven;
    [SerializeField] private CharacterCard bloodyLilah;
    private int primed;
    private int followThrough;
    [SerializeField] private int turnFourBonus;

    public override List<GameNotification> getResponse(GameNotification note)
    {
        List<GameNotification> ret = new List<GameNotification>();
        if (!isMyOnReveal(note))
        {
            if (note.getNature() == GameNotification.Nature.PLAY_CARD
                && note.getCharacterCards()[0].myPlayer == myPlayer
                && primed > followThrough)
            {
                primed++;
                CharacterCard target = note.getCharacterCards()[0];
                if (StaticData.board.turn == 1)
                {
                    GameNotification effect = new GameNotification(GameNotification.Nature.TRANSFORM_CARD, true, this);
                    effect.setCards(new CharacterCard[] { target, dreamRaven });
                    ret.Add(effect);
                }
                else if (StaticData.board.turn == 2)
                {
                    GameNotification effect = new GameNotification(GameNotification.Nature.RELOCATE_CARD, true, this);
                    effect.setCards(new CharacterCard[] { target });
                    effect.setPositions(new PositionState[] { target.positionState, StaticData.board.destroyedCardPiles[myPlayer] });
                    ret.Add(effect);
                }
                else if (StaticData.board.turn == 3)
                {
                    Lane moveTo = StaticData.board.oneLaneToTheRight(((LaneSegment)target.positionState).lane);
                    if (moveTo != null)
                    {
                        GameNotification effect = new GameNotification(GameNotification.Nature.RELOCATE_CARD, true, this);
                        effect.setCards(new CharacterCard[] { target });
                        effect.setPositions(new PositionState[] { target.positionState, moveTo.segments[target.myPlayer] });
                        ret.Add(effect);
                    }
                }
                else if (StaticData.board.turn == 4)
                {
                    GameNotification effect = new GameNotification(GameNotification.Nature.PERM_ALTER_POWER, true, this);
                    effect.setCards(new CharacterCard[] { target });
                    effect.setInts(new int[] { turnFourBonus });
                    ret.Add(effect);
                }
                else if (StaticData.board.turn == 5)
                {
                    GameNotification effect = new GameNotification(GameNotification.Nature.CHANGE_LOCATION, true, this);
                    effect.setLocations(new Location[] { ((LaneSegment)target.positionState).lane.location, StaticData.getRandomLocation() });
                    ret.Add(effect);
                }
                else if (StaticData.board.turn == 6)
                {
                    GameNotification effect = new GameNotification(GameNotification.Nature.CREATE_CARD, true, this);
                    effect.setCards(new CharacterCard[] { bloodyLilah });
                    effect.setPositions(new PositionState[] { StaticData.board.hands[myPlayer] });
                    effect.setInts(new int[] { myPlayer });
                    ret.Add(effect);
                }
                else if (StaticData.board.turn == 7)
                {
                    GameNotification effect = new GameNotification(GameNotification.Nature.PERM_ALTER_POWER, true, this);
                    effect.setCards(new CharacterCard[] { target });
                    effect.setInts(new int[] { target.getPermanentPower() });
                    ret.Add(effect);
                }
            }
            else if (note.getNature() == GameNotification.Nature.RELOCATE_CARD
                && note.getPositions()[1] == StaticData.board.destroyedCardPiles[myPlayer]
                && note.getCause() == this)
            {
                for (int q = 0; q < 2; q++)
                {
                    int numInDeck = StaticData.board.decks[myPlayer].cardsHere.Count;
                    GameNotification draw = new GameNotification(GameNotification.Nature.RELOCATE_CARD, true, this);
                    draw.setCards(new CharacterCard[] { StaticData.board.decks[myPlayer].cardsHere[numInDeck - (q + 1)] });
                    draw.setPositions(new PositionState[] { StaticData.board.decks[myPlayer], StaticData.board.hands[myPlayer] });
                    ret.Add(draw);
                }
            }
            return null;
        }
        else
        {
            primed++;
        }

        return ret;
    }

}
