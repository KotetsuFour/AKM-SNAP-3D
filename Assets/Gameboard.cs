using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Gameboard : MonoBehaviour
{
    [SerializeField] private Camera cam;

    [SerializeField] private Lane twoPlayerLeftLane;
    [SerializeField] private Lane twoPlayerMiddleLane;
    [SerializeField] private Lane twoPlayerRightLane;

    [SerializeField] private Lane threePlayerLeftLane;
    [SerializeField] private Lane threePlayerMiddleLane;
    [SerializeField] private Lane threePlayerRightLane;
    [SerializeField] private Lane threePlayerTopLane;

    [SerializeField] private UnrevealedLocation unrevealed;

    public Lane leftLane;
    public Lane middleLane;
    public Lane rightLane;
    public Lane topLane;
    public Lane[] lanes;

    [SerializeField] private Deck deckPrefab;
    public List<Deck> decks;
    public List<Hand> hands;
    public List<PositionState> destroyedCardPiles;
    public List<PositionState> discardPiles;

    [SerializeField] private LayerMask cardLayer;
    [SerializeField] private LayerMask positionLayer;

    public int turn;
    public static int lastTurn = 6;

    public static float cardWidth = 1.35f;
    public static float cardHeight = 1.75f;

    private CharacterCard selectedCard;

    public List<int> energies;
    public List<int> extraEnergies;

    [SerializeField] private GameObject cardExaminer;
    [SerializeField] private GameObject playUI;

    private SelectionMode selectionMode;
    // Start is called before the first frame update

    private bool donePlaying;
    private PlayerConnector connector;
    private List<int> myActions;
    private int[] turnOrder;

    private LinkedList<GameNotification> actionQueue;

    public GameArchive archive;

    public bool endPlayPhase;

    public List<NotificationHandler> getAllReactors()
    {
        List<NotificationHandler> ret = new List<NotificationHandler>();
        foreach (Lane lane in lanes)
        {
            if (lane.location != null)
            {
                ret.Add(lane.location);
            }
        }
        for (int q = 0; q < turnOrder.Length; q++)
        {
            ret.AddRange(decks[turnOrder[q]].cardsHere);
            ret.AddRange(hands[turnOrder[q]].cardsHere);
            ret.AddRange(destroyedCardPiles[turnOrder[q]].cardsHere);
            ret.AddRange(discardPiles[turnOrder[q]].cardsHere);
        }
        foreach (Lane lane in lanes)
        {
            List<CharacterCard> playedCards = lane.getAllCardsHere();
            ret.AddRange(playedCards);
        }
        return ret;
    }
    public List<NotificationHandler> getAllPermissionNeeded()
    {
        List<NotificationHandler> ret = new List<NotificationHandler>();
        foreach (Lane lane in lanes)
        {
            if (lane.location != null)
            {
                ret.Add(lane.location);
            }
        }
        foreach (Lane lane in lanes)
        {
            List<CharacterCard> playedCards = lane.getAllCardsHere();
            ret.AddRange(playedCards);
        }
        return ret;
    }
    public void setMyDeck()
    {
        for (int q = 0; q < StaticData.myDeck.Count; q++)
        {
            addCardToDeck(decks[0], StaticData.myDeck[q]);
        }
    }
    public void setDeck(int player, NetworkLogic.DeckPackage deckPack)
    {
        addCardToDeck(decks[player], deckPack.card0);
        addCardToDeck(decks[player], deckPack.card1);
        addCardToDeck(decks[player], deckPack.card2);
        addCardToDeck(decks[player], deckPack.card3);
        addCardToDeck(decks[player], deckPack.card4);
        addCardToDeck(decks[player], deckPack.card5);
        addCardToDeck(decks[player], deckPack.card6);
        addCardToDeck(decks[player], deckPack.card7);
        addCardToDeck(decks[player], deckPack.card8);
        addCardToDeck(decks[player], deckPack.card9);
        addCardToDeck(decks[player], deckPack.card10);
        addCardToDeck(decks[player], deckPack.card11);
    }
    private void addCardToDeck(Deck deck, int cardIdx)
    {
        CharacterCard card = Instantiate(StaticData.allCards[cardIdx]);
        deck.addCard(card);
    }
    void Start()
    {
        StaticData.board = this;

        setMyDeck();

        GameObject found = GameObject.Find("Player Connector");
        if (found == null)
        {
            found = GameObject.Find("Player Connector(Clone)");
        }
        connector = found.GetComponent<PlayerConnector>();

        if (StaticData.numPlayers == 2)
        {
            leftLane = Instantiate(twoPlayerLeftLane);
            middleLane = Instantiate(twoPlayerMiddleLane);
            rightLane = Instantiate(twoPlayerRightLane);
            lanes = new Lane[] { leftLane, middleLane, rightLane };
        }
        else if (StaticData.numPlayers == 3)
        {
            leftLane = Instantiate(threePlayerLeftLane);
            middleLane = Instantiate(threePlayerMiddleLane);
            rightLane = Instantiate(threePlayerRightLane);
            topLane = Instantiate(threePlayerTopLane);
            lanes = new Lane[] { leftLane, middleLane, rightLane, topLane };
        }

        energies = new List<int>();
        extraEnergies = new List<int>();
        for (int q = 0; q < StaticData.numPlayers; q++)
        {
            energies.Add(0);
            extraEnergies.Add(0);
        }

        archive = new GameArchive();

        GameNotification start = new GameNotification(GameNotification.Nature.GAME_START, false, null);
        actionQueue.AddLast(start);

    }
    public Lane oneLaneToTheRight(Lane lane)
    {
        if (StaticData.numPlayers == 2)
        {
            if (lane == twoPlayerLeftLane)
            {
                return twoPlayerMiddleLane;
            }
            else if (lane == twoPlayerMiddleLane)
            {
                return twoPlayerRightLane;
            }
            else if (lane == twoPlayerRightLane)
            {
                return null;
            }

        } else if (StaticData.numPlayers == 3)
        {
            //TODO
        }
        return null;
    }
    public Lane oneLaneToTheLeft(Lane lane)
    {
        if (StaticData.numPlayers == 2)
        {
            if (lane == twoPlayerLeftLane)
            {
                return null;
            }
            else if (lane == twoPlayerMiddleLane)
            {
                return twoPlayerLeftLane;
            }
            else if (lane == twoPlayerRightLane)
            {
                return twoPlayerMiddleLane;
            }

        }
        else if (StaticData.numPlayers == 3)
        {
            //TODO
        }
        return null;
    }
    // Update is called once per frame
    void Update()
    {
        //Handle notifications
        GameNotification currentAction = actionQueue.First.Value;
        if (currentAction.getStage() == GameNotification.Stage.PERMISSION)
        {
            currentAction.allow();
        }
        else if (currentAction.getStage() == GameNotification.Stage.ANIMATING)
        {
            currentAction.animate();
        }
        else if (currentAction.getStage() == GameNotification.Stage.ACTING)
        {
            currentAction.act();
        }
        else if (currentAction.getStage() == GameNotification.Stage.DENIED)
        {
            actionQueue.RemoveFirst();
        }
        else if (currentAction.getStage() == GameNotification.Stage.COMPLETED)
        {
            if (currentAction.getNature() == GameNotification.Nature.ON_REVEAL
                || currentAction.getNature() == GameNotification.Nature.ONGOING
                || currentAction.getNature() == GameNotification.Nature.LOCATION_EFFECT)
            {
                List<GameNotification> responses = currentAction.getCause().getResponse(currentAction);
                LinkedListNode<GameNotification> after = actionQueue.First;
                foreach (GameNotification note in responses)
                {
                    actionQueue.AddAfter(after, note);
                    after = after.Next;
                }
                actionQueue.RemoveFirst();
            }
            else
            {
                List<GameNotification> responses = new List<GameNotification>();
                List<NotificationHandler> responders = getAllReactors();
                foreach (NotificationHandler card in responders)
                {
                    List<GameNotification> response = card.getResponse(currentAction);
                    if (response != null)
                    {
                        responses.AddRange(response);
                    }
                }
                LinkedListNode<GameNotification> after = actionQueue.First;
                foreach (GameNotification note in responses)
                {
                    actionQueue.AddAfter(after, note);
                    after = after.Next;
                }
                processNotification();
                actionQueue.RemoveFirst();
            }
        }

        //Update energy display
        StaticData.findDeepChild(playUI.transform, "Energy").GetComponent<TextMeshProUGUI>()
            .text = "" + energies[StaticData.player];

        //Handle player input
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (actionQueue.First.Value.getNature() == GameNotification.Nature.PLAY_PHASE)
            {
                playUI.SetActive(true);
                cardExaminer.SetActive(false);

                //If you clicked a card, it becomes the selectedCard
                RaycastHit hit;
                if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, float.MaxValue, cardLayer))
                {
                    if (selectedCard != null)
                    {
                        selectedCard.GetComponent<SpriteRenderer>().color = Color.white;
                        selectedCard = null;
                    }
                    selectedCard = hit.collider.GetComponent<CharacterCard>();
                    selectedCard.GetComponent<SpriteRenderer>().color = Color.cyan;
                    //TODO reveal the examine button
                }
                //If you clicked a lane
                else if (Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out hit, float.MaxValue, positionLayer))
                {
                    if (selectedCard != null)
                    {
                        selectedCard.GetComponent<SpriteRenderer>().color = Color.white;
                        if (hit.collider != null)
                        {
                            Lane lane = hit.collider.GetComponent<Lane>();
                            //Check to play card
                            if (!donePlaying
                                && hands[StaticData.player].cardsHere.Contains(selectedCard)
                                && lane.players.Contains(StaticData.player)
                                && !lane.segments[StaticData.player].isFull()
                                && lane.segments[StaticData.player] != selectedCard.positionState
                                && energies[StaticData.player] >= selectedCard.getCost())
                            {
                                List<NotificationHandler> checks = getAllPermissionNeeded();
                                bool allowed = true;
                                foreach (NotificationHandler check in checks)
                                {
                                    if (!check.allowPlaceCard(selectedCard, lane.segments[StaticData.player]))
                                    {
                                        allowed = false;
                                        break;
                                    }
                                }
                                if (allowed)
                                {
                                    myActions.Add(0);
                                    myActions.Add(hands[StaticData.player].cardsHere.IndexOf(selectedCard));
                                    for (int q = 0; q < lanes.Length; q++)
                                    {
                                        if (lanes[q] == lane)
                                        {
                                            myActions.Add(q);
                                            break;
                                        }
                                    }
                                    energies[StaticData.player] -= selectedCard.getCost();
                                    hands[StaticData.player].removeCardTentatively(selectedCard);
                                    lane.segments[StaticData.player].addCardTentatively(selectedCard);
                                    selectedCard.turnPlayed = turn;
                                }
                            }
                            //Check to move card
                            else if (!donePlaying
                                && selectedCard.positionState is LaneSegment
                                && selectedCard.myPlayer == StaticData.player
                                && selectedCard.canMove
                                && !lane.segments[StaticData.player].isFull()
                                && lane.segments[StaticData.player] != selectedCard.positionState)
                            {
                                List<NotificationHandler> checks = getAllPermissionNeeded();
                                bool allowed = true;
                                foreach (NotificationHandler check in checks)
                                {
                                    if (!check.allowMoveCard(selectedCard, (LaneSegment)selectedCard.positionState, lane.segments[StaticData.player]))
                                    {
                                        allowed = false;
                                        break;
                                    }
                                }
                                if (allowed)
                                {
                                    myActions.Add(1);
                                    for (int q = 0; q < lanes.Length; q++)
                                    {
                                        if (lanes[q] == ((LaneSegment)selectedCard.positionState))
                                        {
                                            myActions.Add((q * StaticData.numCardsPerLane)
                                                + selectedCard.positionState.cardsHere.Count
                                                + selectedCard.positionState.tentativeCards.Count);
                                            break;
                                        }
                                    }
                                    for (int q = 0; q < lanes.Length; q++)
                                    {
                                        if (lanes[q] == lane)
                                        {
                                            myActions.Add(q);
                                            break;
                                        }
                                    }
                                    selectedCard.positionState.removeCardTentatively(selectedCard);
                                    lane.segments[StaticData.player].addCardTentatively(selectedCard);
                                }
                            }
                        }
                    }
                }
                else
                {
                    selectedCard = null;
                    //TODO hide examine button
                }
            }
        }
    }

    public void examine()
    {
        if (selectedCard != null)
        {
            CharacterCard thisCard = selectedCard;
            Debug.Log(thisCard);
            playUI.SetActive(false);
            cardExaminer.SetActive(true);
            StaticData.findDeepChild(cardExaminer.transform, "DisplayCard").GetComponent<Image>()
                .sprite = thisCard.GetComponent<SpriteRenderer>().sprite;
            StaticData.findDeepChild(cardExaminer.transform, "Info1")
                .GetComponent<TextMeshProUGUI>().text = $"Cost: {thisCard.getCost()} (Base: {thisCard.baseCost})" +
                $"\nPower: {thisCard.getPower()} (Base: {thisCard.basePower})" +
                $"\n\nSeries: {thisCard.series}";
            string attList = "Attributes:";
            for (int q = 0; q < thisCard.attributes.Count; q++)
            {
                attList += " " + thisCard.attributes[q];
                if (q < thisCard.attributes.Count - 1)
                {
                    attList += ",";
                }
            }
            StaticData.findDeepChild(cardExaminer.transform, "Info2").GetComponent<TextMeshProUGUI>()
                .text = attList;
            StaticData.findDeepChild(cardExaminer.transform, "CardName").GetComponent<TextMeshProUGUI>()
                .text = thisCard.characterName;
            StaticData.findDeepChild(cardExaminer.transform, "Ability").GetComponent<TextMeshProUGUI>()
                .text = thisCard.abilityDescription;
        }
    }

    public void undo()
    {
        if (myActions.Count == 0)
        {
            return;
        }
        int actionType = myActions[myActions.Count - 3];
        int handPos = myActions[myActions.Count - 2];
        int lanePlayed = myActions[myActions.Count - 1];
        myActions.RemoveAt(myActions.Count - 1);
        myActions.RemoveAt(myActions.Count - 1);
        myActions.RemoveAt(myActions.Count - 1);
        if (actionType == 0)
        {
            LaneSegment segment = lanes[lanePlayed].segments[StaticData.player];
            CharacterCard cardPlayed = segment.tentativeCards[segment.tentativeCards.Count - 1];
            segment.undoTentativeAdd();
            hands[StaticData.player].undoTentativeRemove();
            cardPlayed.turnPlayed = 0;
        } else if (actionType == 1)
        {
            LaneSegment backFrom = lanes[lanePlayed].segments[StaticData.player];
            LaneSegment backTo = lanes[handPos / StaticData.numCardsPerLane].segments[StaticData.player];
            backFrom.undoTentativeAdd();
            backTo.undoTentativeRemove();
        }
    }

    private void processNotification()
    {
        GameNotification note = actionQueue.First.Value;
        if (note.getNature() == GameNotification.Nature.GAME_START)
        {
            for (int q = 0; q < StaticData.numPlayers; q++)
            {
                for (int w = 0; w < 3; w++)
                {
                    GameNotification draw = new GameNotification(GameNotification.Nature.RELOCATE_CARD, false, null);
                    draw.setCards(new CharacterCard[] { decks[q].cardsHere[decks[q].cardsHere.Count - (1 + w)] });
                    draw.setPositions(new PositionState[] { decks[q], hands[q] });
                    actionQueue.AddLast(draw);
                }
            }
            actionQueue.AddLast(new GameNotification(GameNotification.Nature.TURN_START, false, null));
        }
        else if (note.getNature() == GameNotification.Nature.TURN_START)
        {
            //Increment the turn and refresh actions
            turn++;
            myActions = new List<int>();
            myActions.Add(turn);

            //Set energy
            for (int q = 0; q < StaticData.numPlayers; q++)
            {
                energies[q] = turn + extraEnergies[q];
                extraEnergies[q] = 0;
            }

            //Draw cards
            for (int q = 0; q < StaticData.numPlayers; q++)
            {
                GameNotification draw = new GameNotification(GameNotification.Nature.RELOCATE_CARD, true, null);
                draw.setCards(new CharacterCard[] { decks[q].topCard() });
                draw.setPositions(new PositionState[] { decks[q], hands[q] });
                actionQueue.AddLast(draw);
            }

            //Start play phase
            actionQueue.AddLast(new GameNotification(GameNotification.Nature.PLAY_PHASE, false, null));
        }
        else if (note.getNature() == GameNotification.Nature.PLAY_PHASE)
        {
            GameNotification finalize = new GameNotification(GameNotification.Nature.FINALIZE_PLAY_PHASE, false, null);
            actionQueue.AddLast(finalize);
            LinkedListNode<GameNotification> afterFin = actionQueue.Last;
            LinkedListNode<GameNotification> beforeFin = actionQueue.First;
            for (int q = 0; q < turnOrder.Length; q++)
            {
                List<CharacterCard> unrevealed = new List<CharacterCard>();
                int currentPlayer = turnOrder[q];

                int[] actions = connector.playerActions[currentPlayer].Value;
                for (int w = 1; w < actions.Length; w += 3)
                {
                    if (actions[w] == 0)
                    {
                        CharacterCard card = hands[currentPlayer].cardsHere[actions[w + 1]];
                        LaneSegment dest = lanes[actions[w + 2]].segments[currentPlayer];
                        GameNotification move = new GameNotification(GameNotification.Nature.PLAY_CARD, false, null);
                        move.setCards( new CharacterCard[] { card });
                        if (currentPlayer != StaticData.player)
                        {
                            hands[currentPlayer].removeCardTentatively(card);
                            dest.addCardTentatively(card);
                        }
                        actionQueue.AddAfter(beforeFin, move);
                        beforeFin = beforeFin.Next;
                        GameNotification reveal = new GameNotification(GameNotification.Nature.REVEAL_CARD, true, null);
                        actionQueue.AddAfter(afterFin, reveal);
                        afterFin = afterFin.Next;
                        card.turnPlayed = turn;
                        unrevealed.Add(card);
                    }
                    else if (actions[w] == 1)
                    {
                        CharacterCard card = lanes[actions[w + 1] / 4]
                            .segments[turnOrder[q]].cardsHere[actions[w + 1] % 4];
                        LaneSegment moveFrom = lanes[actions[w + 1] / 4].segments[turnOrder[q]];
                        LaneSegment moveTo = lanes[actions[w + 2]].segments[turnOrder[q]];
                        GameNotification move = new GameNotification(GameNotification.Nature.REGISTER_MOVE, false, null);
                        move.setCards(new CharacterCard[] { card });
                        if (currentPlayer != StaticData.player)
                        {
                            moveFrom.removeCardTentatively(card);
                            moveTo.addCardTentatively(card);
                        }
                        actionQueue.AddAfter(beforeFin, move);
                        beforeFin = beforeFin.Next;
                    }
                }

                foreach (Lane lane in lanes)
                {
                    foreach (CharacterCard check in lane.segments[currentPlayer].cardsHere)
                    {
                        if (!check.revealed && !unrevealed.Contains(check))
                        {
                            GameNotification reveal = new GameNotification(GameNotification.Nature.REVEAL_CARD, true, null);
                            reveal.setCards(new CharacterCard[] { check });
                            actionQueue.AddAfter(beforeFin, reveal);
                        }
                    }
                }
            }
            foreach (Lane lane in lanes)
            {
                foreach (LaneSegment seg in lane.segments)
                {
                    seg.finalizeTentatives();
                }
            }
            foreach (PositionState hand in hands)
            {
                hand.finalizeTentatives();
            }
            actionQueue.AddLast(new GameNotification(GameNotification.Nature.TURN_END, false, null));
        }
        else if (note.getNature() == GameNotification.Nature.TURN_END)
        {
            if (turn >= lastTurn)
            {
                actionQueue.AddLast(new GameNotification(GameNotification.Nature.GAME_END, false, null));
            }
            else
            {
                actionQueue.AddLast(new GameNotification(GameNotification.Nature.TURN_START, false, null));
            }
        }
        else if (note.getNature() == GameNotification.Nature.GAME_END)
        {
            actionQueue.AddLast(new GameNotification(GameNotification.Nature.FINISH, false, null));
        }
        else if (note.getNature() == GameNotification.Nature.FINISH)
        {
            //TODO Setup end screen
            actionQueue.AddLast(new GameNotification(GameNotification.Nature.STANDBY, false, null));
        }
        else if (note.getNature() == GameNotification.Nature.PLAY_CARD)
        {
            archive.playedCards[note.getCharacterCards()[0].myPlayer].Add(note.getCharacterCards()[0]);
        }
        else if (note.getNature() == GameNotification.Nature.REVEAL_CARD)
        {
            GameNotification reveal = new GameNotification(GameNotification.Nature.ON_REVEAL, true, null);
            reveal.setCards(new CharacterCard[] { note.getCharacterCards()[0] });
            actionQueue.AddAfter(actionQueue.First, reveal);
        }
        else if (note.getNature() == GameNotification.Nature.REGISTER_MOVE)
        {
            if (note.getCause() is CharacterCard)
            {
                if (note.getPositions()[0] is LaneSegment && note.getPositions()[1] is LaneSegment)
                {
                    archive.movedCards[((CharacterCard)note.getCause()).myPlayer]++;
                }
                else if (destroyedCardPiles.Contains(note.getPositions()[1]))
                {
                    archive.destroyedCards[((CharacterCard)note.getCause()).myPlayer]++;
                }
                else if (discardPiles.Contains(note.getPositions()[1]))
                {
                    archive.destroyedCards[((CharacterCard)note.getCause()).myPlayer]++;
                }
            }
        }
        else if (note.getNature() == GameNotification.Nature.RELOCATE_CARD)
        {
            if (note.getPositions()[1] is LaneSegment && !(note.getPositions()[0] is LaneSegment))
            {
                GameNotification reveal = new GameNotification(GameNotification.Nature.REVEAL_CARD, true, null);
                reveal.setCards(new CharacterCard[] { note.getCharacterCards()[0] });
                actionQueue.AddAfter(actionQueue.First, reveal);
            }
        }
    }
    public void calculateScores()
    {
        foreach (Lane lane in lanes)
        {
            foreach (LaneSegment seg in lane.segments)
            {
                seg.calculatePowers();
            }
        }
    }
    public enum SelectionMode
    {
        PLAYING, WAITING, RESOLVING
    }
}
