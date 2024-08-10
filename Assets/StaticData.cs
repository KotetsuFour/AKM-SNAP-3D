using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StaticData
{
    public static int numPlayers;
    public static int player;
    public static Board board;

    public static int numCardsPerLane = 4;

    public static List<CharacterCard> allCards = new List<CharacterCard>();
    public static List<List<CharacterCard>> cardsByCost = new List<List<CharacterCard>>();

    public static List<int> myDeck;
    public static List<List<int>> decks = new List<List<int>>();

    public static List<Location> allLocations = new List<Location>();

    public static int[] finalScores;

    public static Transform findDeepChild(Transform parent, string childName)
    {
        LinkedList<Transform> kids = new LinkedList<Transform>();
        for (int q = 0; q < parent.childCount; q++)
        {
            kids.AddLast(parent.GetChild(q));
        }
        while (kids.Count > 0)
        {
            Transform current = kids.First.Value;
            kids.RemoveFirst();
            if (current.name == childName || current.name + "(Clone)" == childName)
            {
                return current;
            }
            for (int q = 0; q < current.childCount; q++)
            {
                kids.AddLast(current.GetChild(q));
            }
        }
        return null;
    }

    public static Location getRandomLocation()
    {
        return allLocations[Random.Range(0, allLocations.Count)];
    }
    public static CharacterCard getRandomCostCard(int cost)
    {
        return cardsByCost[cost][Random.Range(0, cardsByCost[cost].Count)];
    }
    public static CharacterCard getRandomCard()
    {
        return allCards[Random.Range(0, allCards.Count)];
    }
}
