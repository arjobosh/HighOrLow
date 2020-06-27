using System;
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

    public Deck(int maxCardVal, int numCards)
    {
        // constructor for deck variants
        deckSize = numCards;
        deck = new List<Card>();
        InitializeDeck(maxCardVal, numCards);
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

    private void InitializeDeck(int maxVal, int numCards)
    {
        // variant deck build        
    }

    public List<Card> Draw(int numToDraw)
    {
        List<Card> hand = new List<Card>();

        for (int i = 0; i < numToDraw; i++)
        {
            while (true)
            {
                Card drawnCard;                
                int result = UnityEngine.Random.Range(1, 101);

                // non-special 1x chance, hearts 2x chance, ace of spades 3x chance
                if (result < 17)
                {
                    // draw a random non-special card
                    int randomSuit = UnityEngine.Random.Range(1, 4);

                    if (randomSuit == 1)
                        drawnCard = FindCard(UnityEngine.Random.Range(1, 14), 'd');
                    else if (randomSuit == 2)
                        drawnCard = FindCard(UnityEngine.Random.Range(1, 14), 'c');
                    else
                        drawnCard = FindCard(UnityEngine.Random.Range(2, 14), 's');
                }
                else if (result >= 17 && result < 49)
                {
                    // draw a random hearts card
                    drawnCard = FindCard(UnityEngine.Random.Range(1, 14), 'h');
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

    public Card TopDeck()
    {
        Card c = deck[0];
        deck.RemoveAt(0);
        return c;
    }

    public void Shuffle()
    {
        /*int r;
        for (int i = 0; i < deck.Count; i++)
        {
            r = Random.Range(1, deck.Count);
            Card temp = deck[i];
            deck[i] = deck[r];
            deck[r] = temp;
        }*/
        List<Card> shuffledDeck = new List<Card>();

        for (int i = 0; i < deck.Count; i++)
        {
            while (true)
            {
                // non-special 1x chance, hearts 2x chance, ace of spades 3x chance
                int nonSpecial = UnityEngine.Random.Range(1, 53);
                int hearts = UnityEngine.Random.Range(1, 27);
                int aceOfSpades = UnityEngine.Random.Range(1, 17);
                int result;

                if (nonSpecial == 1) result = nonSpecial;
                else if (hearts == 1) result = hearts;
                else if (aceOfSpades == 1) result = aceOfSpades;
                else continue;                
                
                if (result == nonSpecial)
                {
                    int randomSuit = UnityEngine.Random.Range(1, 4);

                    if (randomSuit == 1)
                    {
                        int r = UnityEngine.Random.Range(1, 14);
                        if (!IsInDeck(shuffledDeck, r, 'd'))
                        {
                            shuffledDeck.Add(new Card(r, 'd'));
                            break;
                        }
                    }
                    else if (randomSuit == 2)
                    {
                        int r = UnityEngine.Random.Range(1, 14);
                        if (!IsInDeck(shuffledDeck, r, 'c'))
                        {
                            shuffledDeck.Add(new Card(r, 'c'));
                            break;
                        }
                    }
                    else
                    {
                        int r = UnityEngine.Random.Range(2, 14);
                        if (!IsInDeck(shuffledDeck, r, 's'))
                        {
                            shuffledDeck.Add(new Card(r, 's'));
                            break;
                        }
                    }
                }
                else if (result == hearts)
                {
                    int r = UnityEngine.Random.Range(1, 14);
                    if (!IsInDeck(shuffledDeck, r, 'h'))
                    {
                        shuffledDeck.Add(new Card(r, 'h'));
                        break;
                    }
                }
                else if (result == aceOfSpades)
                {
                    if (!IsInDeck(shuffledDeck, 1, 's'))
                    {
                        shuffledDeck.Add(new Card(1, 's'));
                        break;
                    }
                }
                else
                {
                    continue;
                }                
            }
        }
        deck = shuffledDeck;
    }

    private bool IsInDeck(List<Card> buildingDeck, int value, char suit)
    {
        for (int i = 0; i < buildingDeck.Count; i++)
            if (buildingDeck[i].GetValue() == value && buildingDeck[i].GetSuit() == suit)
                return true;

        return false;
    }

    private Card FindCard(int value, char suit)
    {
        for (int i = 0; i < deck.Count; i++)
            if (deck[i].GetValue() == value && deck[i].GetSuit() == suit)
                return deck[i];

        return null;
    }

    public void SetDeckActive(bool status)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            deck[i].SetCardActive(status);
        }
    }

    public void SetDeckPosition(Vector3 pos)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            deck[i].SetCardPosition(pos);
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

    public int CountCurrentSuit(char suit)
    {
        int suitCount = 0;

        for (int i = 0; i < deck.Count; i++)
            if (deck[i].GetSuit().Equals(suit))
                suitCount++;
        
        return suitCount;
    }

    public int SumCurrentDeck()
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