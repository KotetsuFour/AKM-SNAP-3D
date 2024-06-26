using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticStatePrepare : MonoBehaviour
{
    public List<CharacterCard> allCards;
    public List<List<CharacterCard>> cardsByCost;
    public List<Location> allLocations;

    public List<Deck> decks;

    // Start is called before the first frame update
    void Start()
    {
        cardsByCost = new List<List<CharacterCard>>();
        for (int q = 0; q < allCards.Count; q++)
        {
            CharacterCard card = allCards[q];
            card.id = q;
            while (cardsByCost.Count - 1 < card.baseCost)
            {
                cardsByCost.Add(new List<CharacterCard>());
            }
            cardsByCost[card.baseCost].Add(card);
        }
        StaticData.allCards.AddRange(allCards);
        StaticData.cardsByCost.AddRange(cardsByCost);
        StaticData.allLocations.AddRange(allLocations);
        StaticData.decks = new List<List<int>>();
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
