using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour
{
    [SerializeField] private Lane twoPlayerLeftLane;
    [SerializeField] private Lane twoPlayerMiddleLane;
    [SerializeField] private Lane twoPlayerRightLane;

    [SerializeField] private Lane threePlayerLeftLane;
    [SerializeField] private Lane threePlayerMiddleLane;
    [SerializeField] private Lane threePlayerRightLane;
    [SerializeField] private Lane threePlayerTopLane;

    public Lane leftLane;
    public Lane middleLane;
    public Lane rightLane;
    public Lane topLane;
    public Lane[] lanes;

    [SerializeField] private Deck deckPrefab;
    public List<Deck> decks;
    public List<PositionState> hands;
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
    private List<int> extraEnergies;

    [SerializeField] private GameObject cardExaminer;
    [SerializeField] private GameObject playUI;

    private SelectionMode selectionMode;
    // Start is called before the first frame update

    private PlayerConnector connector;
    private List<int> myActions;
    private List<CharacterCard> myCardsToReveal;
    private List<CharacterCard> myMovedCards;
    public List<CharacterCard> cardsToReveal;
    private int[] turnOrder;

    private int revealIdx;
    private int locationOngoingIdx;
    private int cardOngoingIdx;

    private LinkedList<GameNotification> actionQueue;

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
            foreach (Lane lane in lanes)
            {
                List<CharacterCard> playedCards = lane.getAllCardsHere();
                ret.AddRange(playedCards);
            }
        }
        return ret;
    }
    void Start()
    {
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
        } else if (StaticData.numPlayers == 3)
        {
            leftLane = Instantiate(threePlayerLeftLane);
            middleLane = Instantiate(threePlayerMiddleLane);
            rightLane = Instantiate(threePlayerRightLane);
            topLane = Instantiate(threePlayerTopLane);
            lanes = new Lane[] { leftLane, middleLane, rightLane, topLane };
        }

        initialize();
    }
    public void initialize()
    {
        StaticData.board = this;

        energies = new List<int>();
        extraEnergies = new List<int>();
        for (int q = 0; q < StaticData.numPlayers; q++)
        {
            energies.Add(0);
            extraEnergies.Add(0);
        }

        startOfTurn();

        GameNotification start = new GameNotification(GameNotification.Nature.GAME_START, false, null);
        actionQueue.AddLast(start);
        for (int q = 0; q < StaticData.numPlayers; q++)
        {
            for (int w = 0; w < 3; w++)
            {
                GameNotification draw = new GameNotification(GameNotification.Nature.RELOCATE_CARD, false, null);
                draw.setCards(new CharacterCard[] { decks[q].topCard() });
                draw.setPositions(new PositionState[] { decks[q], hands[q] });
                actionQueue.AddLast(draw);
            }
        }
    }
    public void startOfTurn()
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
    }
    // Update is called once per frame
    void Update()
    {
        //Handle notifications
        GameNotification currentAction = actionQueue.First.Value;
        if (currentAction.getStage() == GameNotification.Stage.PERMISSION)
        {
            if (currentAction.allow())
            {
                currentAction.act();
            }
        }
        else if (currentAction.getStage() == GameNotification.Stage.COMPLETED)
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
            foreach (GameNotification note in responses)
            {
                actionQueue.AddAfter(actionQueue.First, note);
            }
            processNotification();
            actionQueue.RemoveFirst();
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
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                    Vector2.zero, float.MaxValue, cardLayer);
                if (hit.collider != null)
                {
                    if (selectedCard != null)
                    {
                        selectedCard.GetComponent<SpriteRenderer>().color = Color.white;
                        selectedCard = null;
                    }
                    CharacterCard thisCard = hit.collider.GetComponent<CharacterCard>();
                    if (hands[StaticData.player].cardsHere.Contains(thisCard))
                    {
                        selectedCard = thisCard;
                        selectedCard.GetComponent<SpriteRenderer>().color = Color.cyan;
                    }
                }
                else
                {
                    hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                        Vector2.zero, float.MaxValue, positionLayer);
                    if (hit.collider != null && selectedCard != null)
                    {
                        Lane lane = hit.collider.GetComponent<Lane>();
                        if (lane.players.Contains(StaticData.player)
                            && lane.segments[StaticData.player].cardsHere.Count < lane.segments[StaticData.player].maxCardsAllowed
                            && energies[StaticData.player] >= selectedCard.getCost(this, lane))
                        {
                            List<NotificationHandler> checks = getAllReactors();
                            bool allowed = true;
                            foreach (NotificationHandler check in checks)
                            {
                                allowed = allowed && check.allowPlaceCard(selectedCard, lane.segments[StaticData.player]);
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
                                energies[StaticData.player] -= selectedCard.getCost(this, lane);
                                hands[StaticData.player].cardsHere.Remove(selectedCard);
                                selectedCard.turnPlayed = turn;
                            }
                        }
                        selectedCard.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                    else if (selectedCard != null)
                    {
                        selectedCard.GetComponent<SpriteRenderer>().color = Color.white;
                        selectedCard = null;
                    }
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
                Vector2.zero, float.MaxValue, cardLayer);
            if (hit.collider != null)
            {
                Debug.Log(hit.collider.name);
                CharacterCard thisCard = hit.collider.GetComponent<CharacterCard>();
                Debug.Log(thisCard);
                playUI.SetActive(false);
                cardExaminer.SetActive(true);
                StaticData.findDeepChild(cardExaminer.transform, "DisplayCard").GetComponent<Image>()
                    .sprite = thisCard.GetComponent<SpriteRenderer>().sprite;
                StaticData.findDeepChild(cardExaminer.transform, "Info1")
                    .GetComponent<TextMeshProUGUI>().text = $"Cost: {thisCard.getCost(this)} (Base: {thisCard.baseCost})" +
                    $"\nPower: {thisCard.getPower(this)} (Base: {thisCard.basePower})" +
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
            CharacterCard cardPlayed = segment.cardsHere[segment.cardsHere.Count - 1];
            lanes[lanePlayed].segments[StaticData.player].cardsHere.Remove(cardPlayed);
            hands[StaticData.player].cardsHere.Insert(handPos, cardPlayed);
            lanes[lanePlayed].segments[StaticData.player].updateCardPositions();
            hands[StaticData.player].updateCardPositions();
            cardPlayed.turnPlayed = 0;
        } else if (actionType == 1)
        {
            //TODO
            LaneSegment backFrom = lanes[lanePlayed].segments[StaticData.player];
            LaneSegment backTo = lanes[handPos / 4].segments[StaticData.player];
            CharacterCard cardPlayed = backFrom.cardsHere[backFrom.cardsHere.Count - 1];
            GameNotification.move(cardPlayed, backFrom, backTo);
        }
    }

    private void processNotification()
    {
        GameNotification note = actionQueue.First.Value;
        if (note.getNature() == GameNotification.Nature.GAME_START)
        {
            actionQueue.AddLast(new GameNotification(GameNotification.Nature.TURN_START, false, null));
        }
        else if (note.getNature() == GameNotification.Nature.TURN_START)
        {
            //Reveal locations
            if (StaticData.numPlayers == 2)
            {
                if (turn == 1 && leftLane.location == null)
                {
                    GameNotification reveal = new GameNotification(GameNotification.Nature.CHANGE_LOCATION, false, null);
                    reveal.setPositions(new PositionState[] { leftLane.segments[0] });
                    reveal.setLocations(new Location[] { StaticData.allLocations[Random.Range(0, StaticData.allLocations.Count)] });
                    actionQueue.AddLast(reveal);
                }
                else if (turn == 2 && middleLane.location == null)
                {
                    GameNotification reveal = new GameNotification(GameNotification.Nature.CHANGE_LOCATION, false, null);
                    reveal.setPositions(new PositionState[] { middleLane.segments[0] });
                    reveal.setLocations(new Location[] { StaticData.allLocations[Random.Range(0, StaticData.allLocations.Count)] });
                    actionQueue.AddLast(reveal);
                }
                else if (turn == 3 && rightLane.location == null)
                {
                    GameNotification reveal = new GameNotification(GameNotification.Nature.CHANGE_LOCATION, false, null);
                    reveal.setPositions(new PositionState[] { rightLane.segments[0] });
                    reveal.setLocations(new Location[] { StaticData.allLocations[Random.Range(0, StaticData.allLocations.Count)] });
                    actionQueue.AddLast(reveal);
                }
            }
            else if (StaticData.numPlayers == 3)
            {
                if (turn == 1)
                {
                    if (leftLane.location == null)
                    {
                        GameNotification reveal = new GameNotification(GameNotification.Nature.CHANGE_LOCATION, false, null);
                        reveal.setPositions(new PositionState[] { leftLane.segments[0] });
                        reveal.setLocations(new Location[] { StaticData.allLocations[Random.Range(0, StaticData.allLocations.Count)] });
                        actionQueue.AddLast(reveal);
                    }
                    if (rightLane.location == null)
                    {
                        GameNotification reveal = new GameNotification(GameNotification.Nature.CHANGE_LOCATION, false, null);
                        reveal.setPositions(new PositionState[] { rightLane.segments[0] });
                        reveal.setLocations(new Location[] { StaticData.allLocations[Random.Range(0, StaticData.allLocations.Count)] });
                        actionQueue.AddLast(reveal);
                    }
                    if (topLane.location == null)
                    {
                        GameNotification reveal = new GameNotification(GameNotification.Nature.CHANGE_LOCATION, false, null);
                        reveal.setPositions(new PositionState[] { topLane.segments[0] });
                        reveal.setLocations(new Location[] { StaticData.allLocations[Random.Range(0, StaticData.allLocations.Count)] });
                        actionQueue.AddLast(reveal);
                    }
                }
                else if (turn == 3 && middleLane.location == null)
                {
                    GameNotification reveal = new GameNotification(GameNotification.Nature.CHANGE_LOCATION, false, null);
                    reveal.setPositions(new PositionState[] { middleLane.segments[0] });
                    reveal.setLocations(new Location[] { StaticData.allLocations[Random.Range(0, StaticData.allLocations.Count)] });
                    actionQueue.AddLast(reveal);
                }
            }

            //Draw cards
            for (int q = 0; q < StaticData.numPlayers; q++)
            {
                GameNotification draw = new GameNotification(GameNotification.Nature.RELOCATE_CARD, true, null);
                actionQueue.AddLast(draw);
            }

            //Start play phase
            actionQueue.AddLast(new GameNotification(GameNotification.Nature.PLAY_PHASE, false, null));
        }
        else if (note.getNature() == GameNotification.Nature.PLAY_PHASE)
        {
            LinkedListNode<GameNotification> afterMove = actionQueue.Last;
            for (int q = 0; q < turnOrder.Length; q++)
            {
                if (turnOrder[q] == StaticData.player)
                {
                    foreach (CharacterCard card in myCardsToReveal)
                    {
                        GameNotification reveal = new GameNotification(GameNotification.Nature.REVEAL_CARD, true, null);
                        reveal.setCards(new CharacterCard[] { card });
                        actionQueue.AddLast(reveal);
                    }
                    //                    cardsToReveal.AddRange(myCardsToReveal);
                    //TODO unreveal cards
                    foreach (CharacterCard card in myMovedCards)
                    {
                        GameNotification move = new GameNotification(GameNotification.Nature.REGISTER_MOVE, false, null);
                        move.setCards(new CharacterCard[] { card });
                        actionQueue.AddLast(move);
                        afterMove = actionQueue.Last;
                    }
                    continue;
                }
                int[] actions = connector.playerActions[turnOrder[q]].Value;
                for (int w = 1; w < actions.Length; w += 3)
                {
                    if (actions[w] == 0)
                    {
                        CharacterCard card = hands[turnOrder[q]].cardsHere[actions[w + 1]];
                        GameNotification.move(card, hands[turnOrder[q]], lanes[actions[w + 2]].segments[turnOrder[q]]);
                        //TODO unreveal card
                        cardsToReveal.Add(card);
                        card.turnPlayed = turn;

                        GameNotification reveal = new GameNotification(GameNotification.Nature.REVEAL_CARD, true, null);
                        reveal.setCards(new CharacterCard[] { card });
                        actionQueue.AddLast(reveal);
                    }
                    else if (actions[w] == 1)
                    {
                        CharacterCard card = lanes[actions[w + 1] / 4]
                            .segments[turnOrder[q]].cardsHere[actions[w + 1] % 4];
                        GameNotification.move(card, lanes[actions[w + 1] / 4].segments[turnOrder[q]], lanes[actions[w + 2]].segments[turnOrder[q]]);

                        GameNotification move = new GameNotification(GameNotification.Nature.REGISTER_MOVE, false, null);
                        move.setCards(new CharacterCard[] { card });
                        actionQueue.AddAfter(afterMove, move);
                        afterMove = actionQueue.Last;
                    }
                }
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
    }
    public enum SelectionMode
    {
        PLAYING, WAITING, RESOLVING
    }
}
