using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : PositionState
{
    // Start is called before the first frame update
    void Start()
    {
        shuffle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void shuffle()
    {
        for (int q = 0; q < cardsHere.Count; q++)
        {
            int randIdx = Random.Range(0, cardsHere.Count);
            CharacterCard temp = cardsHere[q];
            cardsHere[q] = cardsHere[randIdx];
            cardsHere[randIdx] = temp;
        }
    }
    public CharacterCard topCard()
    {
        if (cardsHere.Count <= 0)
        {
            return null;
        }
        return cardsHere[cardsHere.Count - 1];
    }
}
