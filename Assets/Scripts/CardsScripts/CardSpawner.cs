using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardSpawner : MonoBehaviour
{
    [Range(1, 4)]
    public int cardAmount;

    [SerializeField] private float cardSpacing;

    [SerializeField] private GameObject cardPrefab;

    private int startIndex;

    public GameObject[] onscreenCards;


    private bool cardChosen = false;
    private List<TarotCards> _playerCards;
    public bool encounterCleared;
    public GameObject Canvas;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnCards());
        onscreenCards = new GameObject[cardAmount];
    }


    public void HasClickedButton(TarotCards card, GameObject p)
    {
        
        cardChosen = true;
        Debug.Log(card.effectValue);
        Debug.Log(card.description);
        card.ApplyEffect(p);
        p.GetComponent<Player_movement>().stats.tarotCards.Add(card);

    }
    private void Update()
    {

    }
    private void DestroyCards()
    {
        foreach (GameObject card in onscreenCards)
        {
            Destroy(card);
        }
    }

    // The cards are spawned in 'pairs', using the for loop to iterate through each cards position
    // and ensure the correct amount of cards in the correct arrangement are placed.
    public IEnumerator SpawnCards()
    {
        while (true)
        {
            GameManager.instance.UpdateGameState(GameManager.GameState.normalPlay);
            yield return new WaitUntil(() => encounterCleared);
            encounterCleared = false;
            // disables the second input system
            GameManager.instance.p2.enabled = false;
            foreach (GameObject p in GameManager.instance.playerInstances)
            {
                cardChosen = false;
                _playerCards = p.GetComponent<TarotCardSelector>().cards;
                if (_playerCards.Count == 0)
                {
                    continue;
                }
                cardAmount = _playerCards.Count > 4 ? 4 : _playerCards.Count;
                onscreenCards = new GameObject[cardAmount];
                foreach (TarotCards card in _playerCards)
                {
                    Debug.Log(card);
                }

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
                onscreenCards = new GameObject[cardAmount];

                // The index is needed because the for loop 'i' doesn't always start at 0, and we need to sequentially add each card
                // to the array as we are creating them.
                int arrayIndex = 0;
                for (int i = startIndex; i < cardAmount; i++)
                {
                    TarotCards card = _playerCards[arrayIndex];
                    // If the card amount is odd and it's the first card, then we can place that in the middle and then continue as
                    // normal afterwards as if it was an even number of cards.
                    if (i == 0 && startIndex == 0)
                    {
                        newCard = Instantiate(cardPrefab, gameObject.transform);
                        newCard.GetComponent<Image>().sprite = card.cardImage;
                        newCard.GetComponent<Button>().onClick.AddListener(() => { HasClickedButton(card, p); });
                        newCard.GetComponent<DisplayDescription>().card = card;
                        newCard.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
                        // We increment here because we are artificially completing a 'pair' with only one card already.
                        i++;

                        onscreenCards[0] = newCard;
                        arrayIndex++;
                    }
                     // To be brutally honest I can't remember why. It's been a long day and if it's not here it doesn't work.
                    else if (i % 2 == startIndex)
                    {
                        newCard = Instantiate(cardPrefab, gameObject.transform);
                        newCard.GetComponent<Image>().sprite = card.cardImage;
                        newCard.GetComponent<Button>().onClick.AddListener(() => { HasClickedButton(card, p); });
                        newCard.GetComponent<DisplayDescription>().card = card;

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
                        onscreenCards[arrayIndex] = newCard;

                        // And all that's left to do is to increment the array index and swap the side we want the next card to spawn on
                        // ready for the next one to spawn.
                        arrayIndex++;
                        leftIndex = !leftIndex;
                    }
                }
                Debug.Log(onscreenCards[0].name + "ere");
                // sets the selected game object to be the newly created tarot card.
                GameManager.instance._events.SetSelectedGameObject(onscreenCards[0]);
                yield return new WaitUntil(() => cardChosen);
                DestroyCards();
            }
        } 
    }
}
