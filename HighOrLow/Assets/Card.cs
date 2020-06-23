using UnityEngine;

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