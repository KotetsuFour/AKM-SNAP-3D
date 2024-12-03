using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionState : MonoBehaviour
{
    public List<CharacterCard> cardsHere;
    public List<Transform> positions;
    public int maxCardsAllowed;
    public int myPlayer;
    public List<CharacterCard> tentativeCards;
    public List<CharacterCard> tentativelyRemovedCards;

    // Start is called before the first frame update
    void Start()
    {
        cardsHere = new List<CharacterCard>();
        tentativeCards = new List<CharacterCard>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void updateCardPositions()
    {
        for (int q = 0; q < cardsHere.Count; q++)
        {
            cardsHere[q].transform.position = positions[q % positions.Count].position;
            cardsHere[q].transform.rotation = positions[q % positions.Count].rotation;
        }
    }
    public virtual void updateTentativeCardPositions()
    {
        int skipped = 0;
        for (int q = 0; q < cardsHere.Count; q++)
        {
            CharacterCard card = cardsHere[q];
            if (tentativelyRemovedCards.Contains(card))
            {
                skipped++;
                continue;
            }
            card.transform.position = positions[(q - skipped) % positions.Count].position;
            card.transform.rotation = positions[(q - skipped) % positions.Count].rotation;
        }
        for (int q = 0; q < tentativeCards.Count; q++)
        {
            CharacterCard card = tentativeCards[q];
            card.transform.position = positions[(q + cardsHere.Count - tentativelyRemovedCards.Count) % positions.Count].position;
            card.transform.rotation = positions[(q + cardsHere.Count - tentativelyRemovedCards.Count) % positions.Count].rotation;
        }
    }

    public bool isEmpty()
    {
        return cardsHere.Count + tentativeCards.Count - tentativelyRemovedCards.Count == 0;
    }
    public bool isFull()
    {
        return cardsHere.Count + tentativeCards.Count - tentativelyRemovedCards.Count >= maxCardsAllowed;
    }

    public void addCard(CharacterCard card)
    {
        cardsHere.Add(card);
        card.positionState = this;
        updateCardPositions();
        card.updatePowerAndCostDisplay();
    }
    public void removeCard(CharacterCard card)
    {
        cardsHere.Remove(card);
        card.positionState = null;
        updateCardPositions();
    }
    public virtual void replaceCard(CharacterCard old, CharacterCard replacement)
    {
        int idx = cardsHere.IndexOf(old);
        cardsHere[idx] = replacement;
        updateCardPositions();
    }

    public virtual void addCardTentatively(CharacterCard card)
    {
        tentativeCards.Add(card);
        updateTentativeCardPositions();
    }

    public virtual void removeCardTentatively(CharacterCard card)
    {
        tentativelyRemovedCards.Add(card);
        updateTentativeCardPositions();
    }
    public virtual void undoTentativeAdd()
    {
        tentativeCards.RemoveAt(tentativeCards.Count - 1);
        updateTentativeCardPositions();
    }
    public virtual void undoTentativeRemove()
    {
        tentativelyRemovedCards.RemoveAt(tentativelyRemovedCards.Count - 1);
        updateTentativeCardPositions();
    }
    public virtual void finalizeTentatives()
    {
        cardsHere.AddRange(tentativeCards);
        foreach (CharacterCard card in tentativelyRemovedCards)
        {
            cardsHere.Remove(card);
        }
        tentativeCards = new List<CharacterCard>();
        tentativelyRemovedCards = new List<CharacterCard>();
        updateCardPositions();
    }
}
