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

    private int cardsToDraw;
    public Text deckText;
    public Text handText;

    void Start()
    {
        cardsToDraw = 2;
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
        Debug.Log(winner.GetFullName());
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
        DiscardHand();

        if (deck.GetCardCount() > 0)
        {
            hand = deck.Draw(cardsToDraw);
            handText.text = "";           

            for (int i = 0; i < hand.Count; i++)
            {
                if (hand[i] != null)
                    handText.text += hand[i].GetValueName() + " of " + hand[i].GetSuitName();

                if (i != hand.Count - 1)
                    handText.text += ", ";

                // display card
                CreateCardSprite(hand[i], i);
            }
            
            deckText.text = "Deck: " + deck.GetCardCount();

            // display winner
            DisplayWinner();
            
            // discard old cards
        }
        else
        {
            Debug.Log("out of cards !");            
            // refresh deck            
        }
    }
}

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

                // hearts 2x chance, ace of spades 3x chance
                int result = UnityEngine.Random.Range(1, 7);

                if (result == 1)
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
                else if (result == 2 || result == 3)
                {
                    // draw a random hearts card
                    drawnCard = FindCard(UnityEngine.Random.Range(1, 14), 'h');
                }
                else
                {
                    // draw the ace of spades
                    drawnCard = FindCard(1, 's');
                }

                if (drawnCard != null)
                {
                    hand.Add(drawnCard);
                    deck.Remove(drawnCard);
                    break;
                }
            }
        }

        return hand;
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

    public int GetDeckSize()
    {
        return deckSize;
    }

    public int GetCardCount()
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
        {
            if (deck[i].GetValue() == value && deck[i].GetSuit() == suit)
                return deck[i];            
        }

        return null;
    }

    private int CountCurrentSuit(char suit)
    {
        int suitCount = 0;

        for (int i = 0; i < deck.Count; i++)
        {
            if (deck[i].GetSuit().Equals(suit))
            {
                suitCount++;
            }
        }

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

public class Card
{
    private int value;
    private char suit;
    private Color color;
    private string valueName;
    private string suitName;
    private int suitValue;

    public Card(int cardValue, char cardSuit)
    {
        value = cardValue;
        suit = cardSuit;
        SetValueName();
        SetSuitName();
        SetSuitValue();
        SetColor();
    }

    public bool IsFaceCard()
    {
        return (value == 1 || value == 11 || value == 12 || value == 13);
    }

    public string GetFullName()
    {
        return valueName + " of " + suitName;
    }

    public int GetSuitValue()
    {
        return suitValue;
    }

    private void SetSuitValue()
    {
        switch (suit)
        {
            case 's':
                suitValue = 1;
                break;
            case 'h':
                suitValue = 2;
                break;
            case 'd':
                suitValue = 3;
                break;
            case 'c':
                suitValue = 4;
                break;            
            default:
                suitName = null;
                break;
        }
    }

    public string GetSuitName()
    {
        return suitName;
    }

    private void SetSuitName()
    {
        switch (suit)
        {
            case 's':
                suitName = "Spades";
                break;
            case 'h':
                suitName = "Hearts";
                break;
            case 'd':
                suitName = "Diamonds";
                break;
            case 'c':
                suitName = "Clubs";
                break;
            default:
                suitName = null;
                break;
        }
    }

    public string GetValueName()
    {
        return valueName;
    }

    private void SetValueName()
    {
        // aces = 1, jacks = 11, queens = 12, kings = 13
        switch (value)
        {
            case 1:
                valueName = "Ace";
                break;
            case 11:
                valueName = "Jack";
                break;
            case 12:
                valueName = "Queen";
                break;
            case 13:
                valueName = "King";
                break;
            default:
                valueName = value.ToString();
                break;
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

    public int GetValue()
    {
        return value;
    }

    public char GetSuit()
    {
        return suit;
    }
}