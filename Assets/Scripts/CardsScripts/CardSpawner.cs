using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using UnityEngine.VFX;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class CardSpawner : MonoBehaviour
{
    [Range(1, 4)]
    public int cardAmount;
    public static EventSystem currentSelectingCards;
    [SerializeField] private float cardSpacing;

    [SerializeField] private GameObject cardPrefab;
    private int startIndex;
    private int currentCardIndex = 1;
    public object[,] onScreenCards;
    private int cardIndex = 0;
    private static bool firstTime = true;
    private int cardIndex3 = 0;
    private int cardIndex4 = 0;
    private int cardindex5 = 0;
    private int cardindex6 = 0;
    private bool cardChosen = false;
    private List<TarotCards> _playerCards;
    public bool encounterCleared;
    public GameObject canvas;
    private int arrayIndex;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCards());
        onScreenCards = new object[cardAmount, cardAmount];
    }


    public void HasClickedButton(int index, GameObject p)
    {
        TarotCards card = (TarotCards)onScreenCards[index, 1];
        card.ApplyEffect(p);
        Debug.Log("argggg card ahoy" + card.possibleMods);
        p.GetComponent<Player_movement>().stats.tarotCards.Add(card);
        _playerCards.Remove(card);
        cardChosen = true;

    }
    private void Update()
    {

    }
    private void DestroyCards()
    {
        for (int i = 0; i < onScreenCards.GetLength(0); i++)
        {
            Destroy((GameObject)onScreenCards[i, 0]);
        }
    }
    public void DisplayNextCard()
    {
       TarotCards card;
       for (int i = 0; i < onScreenCards.GetLength(0); i++)
        {
            if (_playerCards.Count == 3 + currentCardIndex)
            {
                return;
            }
            GameObject newCard = (GameObject)onScreenCards[i, 0];
            switch (i) 
            {
                case 0:
                    card = _playerCards[i + currentCardIndex + cardIndex];
                    break;
                case 1:
                    card = _playerCards[i + 1 + currentCardIndex];
                    break;
                case 2:
                    card = _playerCards[i  - 3 + currentCardIndex + cardIndex3];
                    cardIndex = 1;
                    if (currentCardIndex == 2)
                    {
                        cardIndex3 = 1;
                    }
                    break;
                case 3:
                    card = _playerCards[i + currentCardIndex];
                    break;
                default:
                    card = _playerCards[_playerCards.Count - 1];
                    break;
            }
            //TarotCards card = _playerCards[i + currentCardIndex];
            newCard.GetComponent<Image>().sprite = card.cardImage;
            newCard.GetComponent<DisplayDescription>().card = card;
            onScreenCards[i, 0] = newCard;
            onScreenCards[i, 1] = card;
        }
        currentCardIndex += 1;
    }
    public void DisplayPreviousCard()
    {
        TarotCards card;
        for (int i = 0; i < onScreenCards.GetLength(0); i++)
        {
            if (currentCardIndex == 1)
            {
                return;
            }
            if (currentCardIndex == 3)
            {
                cardindex5 = 1;
                cardIndex4 = 3;
                cardindex6 = 0;
            }
            if (currentCardIndex == 2)
            {
                cardIndex4 = 0;
                cardindex5 = 0;
            }
            if(currentCardIndex == 4)
            {
                cardIndex4 = 3;
                cardindex6 = 1;
            }
            GameObject newCard = (GameObject)onScreenCards[i, 0];
            switch (i)
            {
                case 0:
                    card = _playerCards[i + currentCardIndex - cardIndex - 1 + cardindex6];
                    break;
                case 1:
                    card = _playerCards[i + currentCardIndex - cardIndex - 1 + cardindex5 + cardindex6];
                    break;
                case 2:
                    card = _playerCards[i + currentCardIndex - cardIndex - 1 - cardIndex4];
                    cardIndex = 1;
                    if (currentCardIndex >= 3)
                    {
                        cardIndex4 = 4;
                    }
                    break;
                case 3:
                    card = _playerCards[i + currentCardIndex - 1 - cardIndex];
                    break;
                default:
                    card = _playerCards[_playerCards.Count - 1];
                    break;
            }

            //TarotCards card = _playerCards[i + currentCardIndex];
            newCard.GetComponent<Image>().sprite = card.cardImage;
            Debug.Log("card currently added" + card.possibleMods + " " + i);
            newCard.GetComponent<DisplayDescription>().card = card;
            onScreenCards[i, 0] = newCard;
            onScreenCards[i, 1] = card;
        }
        if (currentCardIndex == 2)
        {
            cardIndex = 0;
            cardIndex4 = 0;
            cardIndex3 = 0;
            cardindex5 = 0;
        }
        currentCardIndex -= 1;


    }
    private GameObject CreateNewCard(TarotCards card, GameObject p)
    {
        GameObject newCard;
        newCard = Instantiate(cardPrefab, gameObject.transform);
        newCard.GetComponent<Image>().sprite = card.cardImage;
        int index = arrayIndex;
        newCard.GetComponent<Button>().onClick.AddListener(() => { HasClickedButton(index, p); });
        newCard.GetComponent<DisplayDescription>().card = card;
        onScreenCards[arrayIndex, 1] = card;
        return newCard;
    }
    private void ChangeEventSystem()
    {
        if (currentSelectingCards != null) currentSelectingCards.enabled = false;
        currentSelectingCards = GameManager.instance.p2.GetComponent<EventSystem>();
        currentSelectingCards.enabled = true;
        currentSelectingCards.UpdateModules();
        GameManager.instance.p2.uiInputModule = GameManager.instance.p2.GetComponent<InputSystemUIInputModule>();
    }
    private bool HasSun(GameObject p)
    {
        foreach(TarotCards card in p.GetComponent<Player_movement>().stats.tarotCards)
        {
            if (card.possibleMods == TarotCards.possibleModifiers.TwoCards)
            {
                return true;
            }
        }
        return false;
    }
    private void CreateCards(GameObject p)
    {
        cardAmount = _playerCards.Count > 4 ? 4 : _playerCards.Count;
        onScreenCards = new object[cardAmount, cardAmount];
        // This makes the cards spawn on alternating sides.
        bool leftIndex = true;

        // Reference for modifying each card individually as it is spawned.
        GameObject newCard;

        // If there is an odd number of cards to be spawned then there will be one central card surrounded by
        // other cards, if there is an even number then there will be two that take up the middle space.
        //
        // This index is used so that there doesn't have to be duplicates of the same code for pretty much the
        // same function. By using it we can offset the below 'i' for loop by 1 we can make it so that cards spawn slightly
        // offset to the side making the cards spawn evenly across the screen.

        if (cardAmount % 2 == 0)
        {
            startIndex = 1;
        }
        else
        {
            startIndex = 0;
        }

        // This array is storing the cards currently shown on screen, allows us to destroy them easily if cards need rerolling.
        onScreenCards = new object[cardAmount, cardAmount];

        // The index is needed because the for loop 'i' doesn't always start at 0, and we need to sequentially add each card
        // to the array as we are creating them.
        arrayIndex = 0;
        for (int i = startIndex; i < cardAmount; i++)
        {
            TarotCards card = _playerCards[arrayIndex];
            // If the card amount is odd and it's the first card, then we can place that in the middle and then continue as
            // normal afterwards as if it was an even number of cards.
            if (i == 0 && startIndex == 0)
            {
                newCard = CreateNewCard(card, p);
                newCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                // We increment here because we are artificially completing a 'pair' with only one card already.
                i++;

                onScreenCards[arrayIndex, 0] = newCard;
                arrayIndex++;
            }
            // To be brutally honest I can't remember why. It's been a long day and if it's not here it doesn't work.
            else if (i % 2 == startIndex)
            {
                newCard = CreateNewCard(card, p);

                // Each card is set to either be the correct distance on the left or the right respectively using i.
                if (leftIndex)
                {
                    newCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(0 - (cardSpacing * i), 0);

                    i--;
                }
                else
                {
                    newCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(0 + (cardSpacing * i), 0);
                }

                // Add it to the array.
                onScreenCards[arrayIndex, 0] = newCard;

                // And all that's left to do is to increment the array index and swap the side we want the next card to spawn on
                // ready for the next one to spawn.
                arrayIndex++;
                leftIndex = !leftIndex;
            }
        }
    }
    // The cards are spawned in 'pairs', using the for loop to iterate through each cards position
    // and ensure the correct amount of cards in the correct arrangement are placed.
    public IEnumerator SpawnCards()
    {
        GameObject nextButton = canvas.transform.GetChild(2).gameObject;
        GameObject previousButton = canvas.transform.GetChild(3).gameObject;
        GameObject tutorialText = canvas.transform.GetChild(4).gameObject;
        while (true)
        {
            GameManager.instance.UpdateGameState(GameManager.GameState.normalPlay);
            nextButton.SetActive(false);
            previousButton.SetActive(false);
            tutorialText.SetActive(false);
            GameManager.instance.text.SetActive(false);
            yield return new WaitUntil(() => encounterCleared);
            if (GameManager.instance.p1 != null )
            {
                currentSelectingCards = GameManager.instance.p1.GetComponent<EventSystem>();
            }
            nextButton.SetActive(true);
            nextButton.SetActive(true);
            if (firstTime)
            {
                tutorialText.SetActive(true);
                firstTime = false;
            }
            GameManager.instance.p2.GetComponent<EventSystem>().enabled = false;
            encounterCleared = false;
            // disables the second input system
            //GameManager.instance.p2.enabled = false;
            foreach (GameObject p in GameManager.instance.playerInstances)
            {
                cardChosen = false;
                if (p == null)
                {
                    ChangeEventSystem();
                    continue;
                }
                _playerCards = p.GetComponent<TarotCardSelector>().cards;
                if (_playerCards.Count == 0)
                {
                    ChangeEventSystem();
                    continue;
                }
                CreateCards(p);
                // sets the selected game object to be the newly created tarot card.
                currentSelectingCards.SetSelectedGameObject((GameObject)onScreenCards[0, 0]);
                GameManager.instance.text.SetActive(true);
                yield return new WaitUntil(() => cardChosen);
                if (HasSun(p))
                {
                    cardChosen = false;
                    DestroyCards();
                    CreateCards(p);
                    currentSelectingCards.SetSelectedGameObject((GameObject)onScreenCards[0, 0]);
                    yield return new WaitUntil(() => cardChosen);
                }
                ChangeEventSystem();
                //GameManager.instance.p1.enabled = false;
                //GameManager.instance.p2.enabled = true;
                DestroyCards();
            }
        } 
    }
}
