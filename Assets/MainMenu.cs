using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Transform menu;

    private List<int> currentDeck;

    private void Start()
    {
        StaticNetworking.initialize();
        addDefaultDecks();
        updateAvailableDecks();
    }
    private void addDefaultDecks()
    {
        StaticData.decks.Add(new List<int>(new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }));
    }

    public void startTwoPlayerGame()
    {
        if (StaticData.myDeck != null && StaticData.myDeck.Count == StaticData.NUM_CARDS_IN_DECK)
        {
            startGame(2);
        }
        else
        {
            StaticData.findDeepChild(menu, "Error").gameObject.SetActive(true);
        }
    }
    public void startThreePlayerGame()
    {
        if (StaticData.myDeck != null && StaticData.myDeck.Count == StaticData.NUM_CARDS_IN_DECK)
        {
            startGame(3);
        }
        else
        {
            StaticData.findDeepChild(menu, "Error").gameObject.SetActive(true);
        }
    }
    private void startGame(int numPlayers)
    {
        try
        {
            StaticData.numPlayers = numPlayers;
            StaticNetworking.createRelay(StaticData.findDeepChild(menu, "JoinCode").GetComponent<TextMeshProUGUI>());
            StaticData.findDeepChild(menu, "JoinCodeInput").gameObject.SetActive(false);
            StaticData.findDeepChild(menu, "Quit").gameObject.SetActive(true);
            switchToScreen("Lobby");
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
    public void switchToScreen(string screenName)
    {
        for (int q = 0; q < menu.childCount; q++)
        {
            menu.GetChild(q).gameObject.SetActive(false);
        }
        StaticData.findDeepChild(menu, screenName).gameObject.SetActive(true);
    }
    public void joinGame()
    {
        string code = StaticData.findDeepChild(menu, "JoinCodeInput").GetComponent<TMP_InputField>().text;
        code = code.Trim();
        code = code.ToUpper();
        if (!string.IsNullOrEmpty(code))
        {
            try
            {
                StaticNetworking.joinRelay(code);
                StaticData.findDeepChild(menu, "JoinCodeInput").gameObject.SetActive(false);
                StaticData.findDeepChild(menu, "Quit").gameObject.SetActive(true);
                StaticData.findDeepChild(menu, "JoinCode").GetComponent<TextMeshProUGUI>().text = "Joined!";
            }
            catch (Exception ex)
            {
                StaticData.findDeepChild(menu, "JoinCode").GetComponent<TextMeshProUGUI>().text = ex.Message;
                Debug.Log(ex);
            }
        }
    }
    public void cancelGame()
    {
        StaticNetworking.cancelConnection();
        switchToScreen("MainBackground");
    }

    public void toCardsAndDecksLibraries()
    {
        setupCardLibrary();
        setupDeckLibrary();
        switchToScreen("CardList");
    }

    public void backToMainMenu()
    {
        updateAvailableDecks();
        switchToScreen("MainBackground");
    }

    private void updateAvailableDecks()
    {
        TMP_Dropdown drop = StaticData.findDeepChild(menu, "DeckOptions").GetComponent<TMP_Dropdown>();
        drop.ClearOptions();
        for (int q = 0; q < StaticData.decks.Count; q++)
        {
            TMP_Dropdown.OptionData opt = new TMP_Dropdown.OptionData();
            opt.text = $"Deck {q}";
            drop.options.Add(opt);
        }
        if (StaticData.myDeck == null && StaticData.decks.Count > 0)
        {
            drop.value = 0;
            drop.RefreshShownValue();
            StaticData.myDeck = StaticData.decks[0];
        }
    }
    public void pickDeck(int myDeck)
    {
        StaticData.myDeck = StaticData.decks[myDeck];
    }
    private void setupCardLibrary()
    {
        Transform cardLib = StaticData.findDeepChild(menu, "CardsContent");
        Transform cardPrefab = StaticData.findDeepChild(menu, "CardButton");

        for (int q = 0; q < cardLib.childCount; q++)
        {
            Destroy(cardLib.GetChild(q).gameObject);
        }
        cardLib.DetachChildren();

        foreach (CharacterCard card in StaticData.allCards)
        {
            Button cardButton = Instantiate(cardPrefab, cardLib).GetComponent<Button>();
            cardButton.gameObject.SetActive(true);

            cardButton.GetComponent<Image>().sprite = card.faceImage;

            Button.ButtonClickedEvent examine = new Button.ButtonClickedEvent();
            examine.AddListener(delegate { examineCard(card); });
            cardButton.onClick = examine;
        }
    }

    private void setupDeckLibrary()
    {
        Transform deckLib = StaticData.findDeepChild(menu, "DecksContent");
        Transform deckPrefab = StaticData.findDeepChild(menu, "DeckButton");

        for (int q = 0; q < deckLib.childCount; q++)
        {
            Destroy(deckLib.GetChild(q).gameObject);
        }
        deckLib.DetachChildren();

        for (int q = 0; q < StaticData.decks.Count; q++)
        {
            Button deckButton = Instantiate(deckPrefab, deckLib).GetComponent<Button>();
            deckButton.gameObject.SetActive(true);

            StaticData.findDeepChild(deckButton.transform, "DeckButtonName").GetComponent<TextMeshProUGUI>()
                .text = $"Deck {q}";

            int deckNum = q;
            Button.ButtonClickedEvent seeDeck = new Button.ButtonClickedEvent();
            seeDeck.AddListener(delegate { setupDeckDisplay(deckNum); });
            deckButton.onClick = seeDeck;
        }

        Button addDeckButton = Instantiate(deckPrefab, deckLib).GetComponent<Button>();
        addDeckButton.gameObject.SetActive(true);

        StaticData.findDeepChild(addDeckButton.transform, "DeckButtonName").GetComponent<TextMeshProUGUI>()
            .text = $"+";

        Button.ButtonClickedEvent addDeck = new Button.ButtonClickedEvent();
        addDeck.AddListener(newDeck);
        addDeckButton.onClick = addDeck;
    }
    private void addCardToDeckDisplay(int cardId)
    {
        Transform deckLib = StaticData.findDeepChild(menu, "DecksContent");
        Transform deckContentPrefab = StaticData.findDeepChild(menu, "DeckButton");

        CharacterCard card = StaticData.allCards[cardId];
        Button cardInDeckButton = Instantiate(deckContentPrefab, deckLib).GetComponent<Button>();
        cardInDeckButton.gameObject.SetActive(true);

        StaticData.findDeepChild(cardInDeckButton.transform, "DeckButtonName").GetComponent<TextMeshProUGUI>()
            .text = card.characterName;

        Button.ButtonClickedEvent removeCard = new Button.ButtonClickedEvent();
        removeCard.AddListener(delegate { removeCardFromDeck(cardId, cardInDeckButton.transform); });
        cardInDeckButton.onClick = removeCard;
    }
    private void addCardToDeck(CharacterCard card)
    {
        currentDeck.Add(card.id);
        addCardToDeckDisplay(card.id);
    }
    private void removeCardFromDeck(int cardId, Transform itemInList)
    {
        currentDeck.Remove(cardId);
        itemInList.SetParent(null);
        Destroy(itemInList.gameObject);
    }
    private void examineCard(CharacterCard card)
    {
        Transform cardExaminer = StaticData.findDeepChild(menu, "CardExaminer");
        cardExaminer.gameObject.SetActive(true);
        StaticData.findDeepChild(cardExaminer.transform, "DisplayCard").GetComponent<Image>()
            .sprite = card.faceImage;
        StaticData.findDeepChild(cardExaminer.transform, "Cost").GetComponent<TextMeshProUGUI>()
            .text = $"{card.getCost()}";
        StaticData.findDeepChild(cardExaminer.transform, "Power").GetComponent<TextMeshProUGUI>()
            .text = $"{card.getPower()}";
        StaticData.findDeepChild(cardExaminer.transform, "Series").GetComponent<TextMeshProUGUI>()
            .text = $"Series:\n{card.series}";
        string attList = "Attributes:";
        for (int q = 0; q < card.attributes.Count; q++)
        {
            attList += "\n" + card.attributes[q];
        }
        StaticData.findDeepChild(cardExaminer.transform, "Attributes").GetComponent<TextMeshProUGUI>()
            .text = attList;
        StaticData.findDeepChild(cardExaminer.transform, "CardName").GetComponent<TextMeshProUGUI>()
            .text = card.characterName;
        StaticData.findDeepChild(cardExaminer.transform, "Ability").GetComponent<TextMeshProUGUI>()
            .text = card.abilityDescription;

        Button.ButtonClickedEvent addToDeck = new Button.ButtonClickedEvent();
        addToDeck.AddListener(delegate { addCardToDeck(card); exitExamineCardDisplay(); });
        StaticData.findDeepChild(cardExaminer, "AddCard").GetComponent<Button>().onClick = addToDeck;
        StaticData.findDeepChild(cardExaminer, "AddCard").GetComponent<Button>().interactable
             = currentDeck != null && currentDeck.Count < StaticData.NUM_CARDS_IN_DECK
             && !currentDeck.Contains(card.id);

        Button.ButtonClickedEvent back = new Button.ButtonClickedEvent();
        back.AddListener(delegate { exitExamineCardDisplay(); });
        StaticData.findDeepChild(cardExaminer, "Back").GetComponent<Button>().onClick = back;
    }
    public void exitExamineCardDisplay()
    {
        StaticData.findDeepChild(menu, "CardExaminer").gameObject.SetActive(false);
    }
    private void setupDeckDisplay(int deckNum)
    {
        currentDeck = StaticData.decks[deckNum];
        Transform deckLib = StaticData.findDeepChild(menu, "DecksContent");

        for (int q = 0; q < deckLib.childCount; q++)
        {
            Destroy(deckLib.GetChild(q).gameObject);
        }
        deckLib.DetachChildren();

        foreach (int cardId in currentDeck)
        {
            addCardToDeckDisplay(cardId);
        }
        StaticData.findDeepChild(menu, "BackToDecks").GetComponent<Button>().interactable = true;

    }
    private void newDeck()
    {
        StaticData.decks.Add(new List<int>());
        setupDeckDisplay(StaticData.decks.Count - 1);
    }
    public void backFromDeckDisplay()
    {
        currentDeck = null;
        setupDeckLibrary();
        StaticData.findDeepChild(menu, "BackToDecks").GetComponent<Button>().interactable = false;
    }
}
