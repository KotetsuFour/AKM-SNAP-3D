using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionState : MonoBehaviour
{
    public List<CharacterCard> cardsHere;
    public List<Transform> positions;
    public int maxCardsAllowed;
    public int myPlayer;

    // Start is called before the first frame update
    void Start()
    {
        cardsHere = new List<CharacterCard>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void updateCardPositions()
    {
        for (int q = 0; q < cardsHere.Count; q++)
        {
            cardsHere[q].transform.position = positions[q % positions.Count].position;
        }
    }

    public bool isEmpty()
    {
        return cardsHere.Count == 0;
    }
    public bool isFull()
    {
        return cardsHere.Count >= maxCardsAllowed;
    }
}
