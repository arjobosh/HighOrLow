using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HighOrLow : MonoBehaviour
{
    Deck deck;
    private int cardsToDraw;
    public Text cardText;

    // Start is called before the first frame update
    void Start()
    {
        cardsToDraw = 2;
        deck = new Deck();
        //deck.PrintDeck();        
        deck.Shuffle();

        List<Card> hand = deck.Draw(cardsToDraw);
        
        for (int i = 0; i < hand.Count; i++)
        {
            cardText.text += hand[i].GetSuit() + " of " + hand[i].GetValue() + ", ";
        }
        cardText.text += "; " + deck.GetCurrentDeckSize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class Deck
{
    private List<Card> deck;
    private int deckSize;

    public Deck()
    {
        deckSize = 52;
        deck = new List<Card>();
        Initialize();
    }

    public int GetCurrentDeckSize()
    {
        return deck.Count;
    }

    public void PrintDeck()
    {
        int b = 0;
        while (b < deck.Count)
        {
            Debug.Log(deck[b].GetValue() + " : " + deck[b].GetSuit());
            b++;
        }

        Debug.Log(deck.Count);
    }

    public int CountSuit(char suit)
    {
        int suitCount = 0;        

        for (int i = 0; i < deck.Count; i++)
        {
            if (deck[i].GetSuit().Equals(suit))
            {
                suitCount++;
            }
        }

        //Debug.Log("HEARTS = " + deck.CountSuit('h'));
        //Debug.Log("SPADES = " + deck.CountSuit('s'));
        //Debug.Log("DIAMONDS = " + deck.CountSuit('d'));
        //Debug.Log("CLUBS = " + deck.CountSuit('c'));

        return suitCount;
    }

    public int SumDeck()
    {
        int b = 0, sum = 0;
        while (b < deck.Count)
        {
            sum += deck[b].GetValue();
            b++;
        }

        //Debug.Log("SUM = " + deck.SumDeck());

        return sum;
    }

    private void Initialize()
    {
        int suitCount = 13;     // standard deck        

        for (int i = 1; i <= suitCount; i++)
        {
            Card hearts = new Card(i, 'h');            
            Card clubs = new Card(i, 'c');
            Card diamonds = new Card(i, 'd');
            Card spades = new Card(i, 's');

            deck.Add(hearts);
            deck.Add(clubs);
            deck.Add(diamonds);
            deck.Add(spades);
        }
    }
    
    public void Shuffle()
    {
        int r;
        for (int i = 0; i < deck.Count; i++)
        {
            r = UnityEngine.Random.Range(1, deck.Count);
            Card temp = deck[i];
            deck[i] = deck[r];
            deck[r] = temp;
        }                   
    }

    private Card FindCard(int value, char suit)
    {        
        for (int i = 0; i < deck.Count; i++)
        {
            if (deck[i].GetValue().Equals(value) && deck[i].GetSuit().Equals(suit))
            {                
                return deck[i];
            }
        }

        return null;
    }

    public List<Card> Draw(int numToDraw)
    {
        // aces = 1, jacks = 11, queens = 12, kings = 13
        // diamonds, clubs, spades excluding the ace of spades = 1 / 52
        // hearts = 2 / 52  = 1 / 26
        // ace of spades    = 3 / 52

        // P(non-special card) = 1/52
        // P(draw a heart) = 13/52 * 2
        // P(draw ace of spades) = 1/52 * 3

        List<Card> hand = new List<Card>();
        float baseChance = 1 / 52;

        for (int i = 0; i < numToDraw; i++)
        {

            // generate a random number between 0 and 1
            int result = UnityEngine.Random.Range(1, 7);
            Card card;

            while (true)
            {
                if (result == 1)
                {
                    // draw a random non-special card, if it exists                
                    int randomSuit = UnityEngine.Random.Range(1, 4);

                    if (randomSuit == 1)
                    {
                        int randomVal = UnityEngine.Random.Range(1, 14);
                        card = FindCard(randomVal, 'd');
                    }
                    else if (randomSuit == 2)
                    {
                        int randomVal = UnityEngine.Random.Range(1, 14);
                        card = FindCard(randomVal, 'c');
                    }
                    else
                    {
                        int randomVal = UnityEngine.Random.Range(2, 14);
                        card = FindCard(randomVal, 's');
                    }

                }
                else if (result > 1 && result <= 3)
                {
                    // draw a random heart, if it exists
                    int randomHeart = UnityEngine.Random.Range(1, 14);
                    card = FindCard(randomHeart, 'h');
                }
                else
                {
                    // draw the ace of spades, if it exists
                    card = FindCard(1, 's');
                }

                if (card == null)
                    continue;
                else
                {
                    hand.Add(card);
                    deck.Remove(card);
                    break;
                }
            }
        }

        return hand;
    }
}

public class Card
{
    private int value;
    private char suit;
    private Color color;
    private float probability;

    public Card(int cardValue, char cardSuit)
    {
        value = cardValue;
        suit = cardSuit;
        SetProbability();
        SetColor();
    }

    private void SetProbability()
    {
        float baseProb = 1 / 52;

        if (suit == 'h')
        {
            probability = baseProb * 2;
        }
        else if (suit == 's' && value == 1)
        {
            probability = baseProb * 3;
        }
        else
        {
            probability = baseProb;
        }
    }

    private void SetColor()
    {
        color = suit.Equals('h') || suit.Equals('d') ? Color.red : Color.black;
    }

    public Color GetColor()
    {
        return color;
    }

    public float GetProbability()
    {
        return probability;
    }

    public int GetValue()
    {
        return value;
    }

    public char GetSuit()
    {
        return suit;
    }
}