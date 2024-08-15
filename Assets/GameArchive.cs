using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameArchive
{
    public List<CharacterCard>[] playedCards;
    public int[] destroyedCards;
    public int[] discardedCards;
    public int[] movedCards;
    public GameArchive()
    {
        playedCards = new List<CharacterCard>[StaticData.numPlayers];
        for (int q = 0; q < playedCards.Length; q++)
        {
            playedCards[q] = new List<CharacterCard>();
        }
        destroyedCards = new int[StaticData.numPlayers];
        discardedCards = new int[StaticData.numPlayers];
        movedCards = new int[StaticData.numPlayers];
    }
}
