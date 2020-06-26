﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighOrLow : MonoBehaviour
{
    private Deck deck;
    private List<Card> hand;
    private List<GameObject> sprites;

    public GameObject cardBack1;
    public GameObject cardBack2;
    public GameObject cardBack3;
    public int cardsToDraw;
    public Text deckText;
    public Text handText1;
    public Text handText2;
    public Text winnerText;
    public Button drawBtn;

    void Start()
    {
        deck = new Deck();
        hand = new List<Card>();
        sprites = new List<GameObject>();        
    }

    private string GenerateAssetPath(int value, string suit, string name, bool isFace)
    {
        string path = "CardArt/" + suit + "/";

        path += (value < 10) ? "0" : "";
        path += value.ToString() + "_";
        path += (isFace) ? name[0].ToString() : value.ToString();
        path += "_" + suit[0].ToString();
        //path += ".png";

        return path;
    }

    private void DownsizeDeck()
    {
        if (deck.GetCurrentCardCount() < deck.GetDeckSize() - (deck.GetDeckSize() / 3) && cardBack1.activeSelf)
        {
            cardBack1.SetActive(false);
        }
        
        if (deck.GetCurrentCardCount() < (deck.GetDeckSize() / 3) && cardBack2.activeSelf)
        {
            cardBack2.SetActive(false);
        }

        if (deck.GetCurrentCardCount() == 0 && cardBack3.activeSelf)
        {
            cardBack3.SetActive(false);
        }
    }

    private void CreateCardSprite(Card card, int handIndex, Vector3 cardPos)
    {        
        string spritePath = 
            GenerateAssetPath(card.GetValue(), card.GetSuitName(), card.GetValueName(), card.IsFaceCard());

        GameObject cardSprite = new GameObject();

        cardSprite.name = "Card" + (handIndex + 1).ToString();

        cardSprite
            .AddComponent<SpriteRenderer>()
            .GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(spritePath);

        cardSprite.GetComponent<Transform>().localScale = new Vector3(0.5f, 0.5f);
        cardSprite.GetComponent<Transform>().localPosition = cardPos;

        sprites.Add(cardSprite);
    }

    private void DiscardHand()
    {
        for (int i = 0; i < sprites.Count; i++)
            Destroy(sprites[i]);        
    }

    private void DisplayWinner()
    {
        Card winner = DetermineHigherCard();
        winnerText.text = winner.GetFullName() + "\nwins!";        
    }

    private Card DetermineHigherCard()
    {
        // assumes two cards to compare
        if (hand[0].GetValue() > hand[1].GetValue())
            return hand[0];
        else if (hand[0].GetValue() < hand[1].GetValue())
            return hand[1];
        else
        {
            if (hand[0].GetSuitValue() > hand[1].GetSuitValue())
                return hand[0];
            else
                return hand[1];
        }
    }

    public void DrawCards()
    {
        // discard previous hand
        DiscardHand();
        StartCoroutine(AnimateDiscard());
        handText1.text = handText2.text = winnerText.text = "";

        if (deck.GetCurrentCardCount() > 0)
        {
            // get new hand
            hand = deck.Draw(cardsToDraw);

            // animate card draw
            GameObject copy1 = Instantiate(cardBack3);
            GameObject copy2 = Instantiate(cardBack3);
            StartCoroutine(AnimateCardDraw(copy1, copy2));            

            // reveal cards and announce winner
            StartCoroutine(RevealCardDraw(1.0f, copy1, copy2));
            DownsizeDeck();
        }
        else
        {
            // notify refresh
            winnerText.text = "Shuffling deck . . .";

            // refresh deck
            cardBack1.SetActive(true);
            cardBack2.SetActive(true);
            cardBack3.SetActive(true);
            Start();
        }
    }

    private IEnumerator AnimateDiscard()
    {
        GameObject card1 = Instantiate(cardBack3);
        GameObject card2 = Instantiate(cardBack3);

        card1.transform.position = new Vector3(-3.0f, -1.0f);
        card2.transform.position = new Vector3(3.0f, -1.0f);

        Vector3 discardPos = new Vector3(5.0f, 1.0f);
        Vector3 ogPos1 = card1.transform.position;
        Vector3 ogPos2 = card2.transform.position;
        float time = 0f;
        float moveSpeed = 2.0f;

        while (time <= 1.0f)
        {
            time += moveSpeed * Time.deltaTime;
            card1.transform.position = Vector3.Lerp(ogPos1, discardPos, time);
            card2.transform.position = Vector3.Lerp(ogPos2, discardPos, time);
            yield return new WaitForSeconds(Time.deltaTime / moveSpeed);
        }

        card1.transform.position = discardPos;
        card2.transform.position = discardPos;

        yield return new WaitForSeconds(Time.deltaTime / moveSpeed);
    }

    private IEnumerator RevealCardDraw(float time, GameObject first, GameObject second)
    {
        yield return new WaitForSeconds(time);
        
        Destroy(first);
        Destroy(second);

        CreateCardSprite(hand[0], 0, new Vector3(-3.0f, -1.0f));
        CreateCardSprite(hand[1], 1, new Vector3(3.0f, -1.0f));

        if (hand[0] != null)
            handText1.text = hand[0].GetFullName();

        if (hand[1] != null)
            handText2.text = hand[1].GetFullName();

        yield return new WaitForSeconds(time / 2.0f);

        DisplayWinner();
        drawBtn.interactable = true;
    }

    private IEnumerator AnimateCardDraw(GameObject card1, GameObject card2)
    {
        drawBtn.interactable = false;

        float time = 0f;
        float moveSpeed = 2.0f;
        Vector3 ogPos1 = card1.transform.position;
        Vector3 ogPos2 = card2.transform.position;
        Vector3 endPos1 = new Vector3(-3.0f, -1.0f);
        Vector3 endPos2 = new Vector3(3.0f, -1.0f);
        
        while (time <= 1.0f)
        {
            time += moveSpeed * Time.deltaTime;
            card1.transform.position = Vector3.Lerp(ogPos1, endPos1, time);
            card2.transform.position = Vector3.Lerp(ogPos2, endPos2, time);
            yield return new WaitForSeconds(Time.deltaTime / moveSpeed);
        }

        card1.transform.position = endPos1;
        card2.transform.position = endPos2;
    }


}