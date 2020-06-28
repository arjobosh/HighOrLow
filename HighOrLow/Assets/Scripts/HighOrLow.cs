using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighOrLow : MonoBehaviour
{
    private Deck deck;
    private List<Card> hand;
    private List<GameObject> discards;
    private GameObject[] cardBacks;

    public GameObject cardBack1;
    public GameObject cardBack2;
    public GameObject cardBack3;
    public int cardsToDraw;
    public Text handText1;
    public Text handText2;
    public Text winnerText;
    public Button drawBtn;

    void Start()
    {
        deck = new Deck();
        deck.Shuffle();
        hand = new List<Card>();
        discards = new List<GameObject>();
        cardBacks = new GameObject[3] { cardBack1, cardBack2, cardBack3 };
    }

    private void DownsizeDeck()
    {        
        int interval = deck.GetDeckSize() / 3;
        int deckSize = deck.GetDeckSize();

        for (int i = 0; i < cardBacks.Length; i++)
        {
            if (deck.GetCurrentCardCount() < deckSize - interval && cardBacks[i].activeSelf)
                cardBacks[i].SetActive(false);

            deckSize -= interval;
        }
        //Debug.Log(deck.GetCurrentCardCount());        
    }

    private void DiscardHand()
    {
        for (int i = 0; i < hand.Count; i++)
            hand[i].DestroySprite();        
    }

    private void DisplayWinner()
    {
        Card winner = DetermineHighestCard();
        winnerText.text = winner.GetFullName() + "\nwins!";
    }

    private Card DetermineHighestCard()
    {
        Card higher = hand?[0];

        if (higher != null)
        {
            for (int i = 1; i < hand.Count; i++)
            {
                if (higher.GetValue() < hand[i].GetValue())
                {
                    higher = hand[i];
                }
                else if (higher.GetValue() == hand[i].GetValue())
                {
                    if (higher.GetSuitValue() < hand[i].GetSuitValue())
                    {
                        higher = hand[i];
                    }
                }
                else continue;
            }
        }

        return higher;
    }

    public void DrawTwoCards()
    {
        // destroy sprites of previous hand
        DiscardHand();
        handText1.text = handText2.text = winnerText.text = "";

        if (deck.GetCurrentCardCount() > 0)
        {
            // animate discard
            if (hand.Count > 0)
            {
                GameObject faceDown1 = Instantiate(cardBack3);
                GameObject faceDown2 = Instantiate(cardBack3);
                discards.Add(faceDown1);
                discards.Add(faceDown2);
                StartCoroutine(TranslateCard(faceDown1, new Vector3(-3.0f, -1.0f), new Vector3(-5.0f, 3.2f)));
                StartCoroutine(TranslateCard(faceDown2, new Vector3(3.0f, -1.0f), new Vector3(-5.0f, 3.2f)));
            }

            // draw twice for new hand
            hand = new List<Card>();
            hand.Add(deck.Draw());
            hand.Add(deck.Draw());

            // animate card draw
            GameObject card1 = Instantiate(cardBack3);
            GameObject card2 = Instantiate(cardBack3);
            StartCoroutine(TranslateCard(card1, card1.transform.position, new Vector3(-3.0f, -1.0f)));
            StartCoroutine(TranslateCard(card2, card2.transform.position, new Vector3(3.0f, -1.0f)));

            // reveal cards and update deck sprites
            StartCoroutine(RevealCardDraw(1.0f, card1, card2));
            DownsizeDeck();
        }
        else
        {
            // destroy discard pile
            ClearDiscards();

            // notify refresh
            winnerText.text = "Shuffling deck . . .";

            // refresh deck
            for (int i = 0; i < cardBacks.Length; i++)
                cardBacks[i].SetActive(true);
            
            Start();
        }
    }

    private void ClearDiscards()
    {
        if (discards.Count > 0)
        {
            for (int i = 0; i < discards.Count; i++)
            {
                Destroy(discards[i]);
            }
        }
    }

    private IEnumerator TranslateCard(GameObject card, Vector3 startPos, Vector3 endPos)
    {
        drawBtn.interactable = false;

        float time = 0f;
        float moveSpeed = 2.0f;

        while (time <= 1.0f)
        {
            if (card != null)
            {
                time += moveSpeed * Time.deltaTime;
                card.transform.position = Vector3.Lerp(startPos, endPos, time);
                yield return new WaitForSeconds(Time.deltaTime / moveSpeed);
            }
        }

        card.transform.position = endPos;
    }

    private IEnumerator RevealCardDraw(float time, GameObject first, GameObject second)
    {
        yield return new WaitForSeconds(time);

        Destroy(first);
        Destroy(second);

        hand[0].CreateSprite();
        hand[0].SetCardPosition(new Vector3(-3.0f, -1.0f));
        hand[1].CreateSprite();
        hand[1].SetCardPosition(new Vector3(3.0f, -1.0f));

        if (hand[0] != null)
            handText1.text = hand[0].GetFullName();

        if (hand[1] != null)
            handText2.text = hand[1].GetFullName();

        yield return new WaitForSeconds(time / 2.0f);
        
        DisplayWinner();
        drawBtn.interactable = true;
    }
}