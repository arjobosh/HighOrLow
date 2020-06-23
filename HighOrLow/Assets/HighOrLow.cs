using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HighOrLow : MonoBehaviour
{
    private Deck deck;
    private List<Card> hand;
    private List<GameObject> sprites;

    public int cardsToDraw;
    public Text deckText;
    public Text handText1;
    public Text handText2;
    public Text winnerText;

    void Start()
    {
        deck = new Deck();
        hand = new List<Card>();
        sprites = new List<GameObject>();
    }

    private string GenerateAssetPath(int value, string suit, string name, bool isFace)
    {
        string path = "Assets/CardArt/" + suit + "/";

        path += (value < 10) ? "0" : "";
        path += value.ToString() + "_";
        path += (isFace) ? name[0].ToString() : value.ToString();
        path += "_" + suit[0].ToString();
        path += ".png";

        return path;
    }

    private void CreateCardSprite(Card card, int handIndex)
    {
        string spritePath = GenerateAssetPath(card.GetValue(), card.GetSuitName(), card.GetValueName(), card.IsFaceCard());

        GameObject cardSprite = new GameObject();

        cardSprite.name = "Card" + (handIndex + 1).ToString();

        cardSprite
            .AddComponent<SpriteRenderer>()
            .GetComponent<SpriteRenderer>().sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);

        cardSprite.GetComponent<Transform>().localScale = new Vector3(0.5f, 0.5f);

        float startPosX = -3.0f;
        float startPosY = -1.0f;

        cardSprite.GetComponent<Transform>().localPosition = (handIndex == 0) ? 
            new Vector3(startPosX, startPosY) : new Vector3(-startPosX, startPosY);

        sprites.Add(cardSprite);
    }

    private void DiscardHand()
    {
        for (int i = 0; i < sprites.Count; i++)
        {
            Destroy(sprites[i]);
        }
    }

    private void DisplayWinner()
    {
        Card winner = DetermineHigherCard();
        winnerText.text = winner.GetFullName() + " wins!";
        //Debug.Log(winner.GetFullName());
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
        

        if (deck.GetCardCount() > 0)
        {
            hand = deck.Draw(cardsToDraw);
            handText1.text = "";
            handText2.text = "";

            /*for (int i = 0; i < hand.Count; i++)
            {                                
                if (hand[i] != null)
                    handText1.text += hand[i].GetValueName() + " of " + hand[i].GetSuitName();

                if (i != hand.Count - 1)
                    handText1.text += ", ";

                // display card
                CreateCardSprite(hand[i], i);
            }*/

            if (hand[0] != null)
                handText1.text = hand[0].GetFullName();

            if (hand[1] != null)
                handText2.text = hand[1].GetFullName();

            // display cards
            CreateCardSprite(hand[0], 0);
            CreateCardSprite(hand[1], 1);

            deckText.text = "Deck: " + deck.GetCardCount();

            // announce winner
            DisplayWinner();
        }
        else
        {         
            // refresh deck
            Start();
        }
        Debug.Log(deck.CountCurrentSuit('h'));
    }
}