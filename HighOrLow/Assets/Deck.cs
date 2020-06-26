using System.Collections.Generic;
using UnityEngine;

public class Deck
{
    private List<Card> deck;
    private int deckSize;

    public Deck()
    {
        // standard deck constructor
        deckSize = 52;
        deck = new List<Card>();
        InitializeDeck();
    }

    public Deck(int numCards)
    {
        // constructor for deck variants
        deckSize = numCards;
        deck = new List<Card>();
        InitializeDeck(numCards);
    }

    public List<Card> Draw(int numToDraw)
    {
        List<Card> hand = new List<Card>();

        for (int i = 0; i < numToDraw; i++)
        {
            while (true)
            {
                Card drawnCard;                
                int result = Random.Range(1, 101);

                // non-special 1x chance, hearts 2x chance, ace of spades 3x chance
                if (result < 17)
                {
                    // draw a random non-special card
                    int randomSuit = Random.Range(1, 4);

                    if (randomSuit == 1)
                        drawnCard = FindCard(Random.Range(1, 14), 'd');
                    else if (randomSuit == 2)
                        drawnCard = FindCard(Random.Range(1, 14), 'c');
                    else
                        drawnCard = FindCard(Random.Range(2, 14), 's');
                }
                else if (result >= 17 && result < 49)
                {
                    // draw a random hearts card
                    drawnCard = FindCard(Random.Range(1, 14), 'h');
                }
                else if (result >= 49 && result < 97)
                {
                    // draw the ace of spades
                    drawnCard = FindCard(1, 's');
                }
                else
                {
                    drawnCard = null;
                }

                if (drawnCard != null)
                {
                    hand.Add(drawnCard);
                    deck.Remove(drawnCard);
                    break;
                }
            }
        }

        //Debug.Log("Hearts: " + CountCurrentSuit('h') + "/" + GetCurrentCardCount());
        return hand;
    }

    public void Shuffle()
    {
        int r;
        for (int i = 0; i < deck.Count; i++)
        {
            r = Random.Range(1, deck.Count);
            Card temp = deck[i];
            deck[i] = deck[r];
            deck[r] = temp;
        }
    }

    public int GetDeckSize()
    {
        return deckSize;
    }

    public int GetCurrentCardCount()
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

        //Debug.Log(deck.Count);
    }

    private void InitializeDeck()
    {
        int maxCardVal = 13;

        for (int i = 1; i <= maxCardVal; i++)
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

    private void InitializeDeck(int maxVal)
    {
        // variant deck build        
    }

    private Card FindCard(int value, char suit)
    {
        for (int i = 0; i < deck.Count; i++)
            if (deck[i].GetValue() == value && deck[i].GetSuit() == suit)
                return deck[i];

        return null;
    }

    public int CountCurrentSuit(char suit)
    {
        int suitCount = 0;

        for (int i = 0; i < deck.Count; i++)
            if (deck[i].GetSuit().Equals(suit))
                suitCount++;
        
        return suitCount;
    }

    private int SumCurrentDeck()
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
}