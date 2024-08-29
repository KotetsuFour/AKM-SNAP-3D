using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardLibrary : MonoBehaviour
{
    [SerializeField] private List<CharacterCard> allNonSpecialCards;
    [SerializeField] private List<CharacterCard> allSpecialCards;
    // Start is called before the first frame update
    void Start()
    {
        for (int q = 0; q < allNonSpecialCards.Count; q++)
        {
            allNonSpecialCards[q].id = q;
        }
        StaticData.allCards = allNonSpecialCards;

        StaticData.cardsByCost = new List<List<CharacterCard>>();
        foreach (CharacterCard card in allNonSpecialCards)
        {
            int cost = card.baseCost;

            while (StaticData.cardsByCost.Count < cost + 1)
            {
                StaticData.cardsByCost.Add(new List<CharacterCard>());
            }

            StaticData.cardsByCost[cost].Add(card);
        }

        for (int q = allNonSpecialCards.Count; q < allNonSpecialCards.Count + allSpecialCards.Count; q++)
        {
            allSpecialCards[q - allNonSpecialCards.Count].id = q;
        }
    }
}
