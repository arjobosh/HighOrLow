using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighOrLow : MonoBehaviour
{
    Deck deck;

    // Start is called before the first frame update
    void Start()
    {
        deck = new Deck();
        deck.PrintDeck();        
        deck.Shuffle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class Deck
{
    // aces = 1, jacks = 11, queens = 12, kings = 13
    List<Card> deck;
    int deckSize;

    public Deck()
    {
        deckSize = 52;
        deck = new List<Card>();
        Initialize();
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

    public void Draw(int numToDraw)
    {
        
    }
}

public class Card
{
    private int value;
    private char suit;
    
    public Card(int cardValue, char cardSuit)
    {
        value = cardValue;
        suit = cardSuit;
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