using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HighOrLow : MonoBehaviour
{
    private Deck deck;
    private List<Card> hand;
    private List<GameObject> sprites;

    public GameObject cardBack;
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
        string spritePath = GenerateAssetPath(card.GetValue(), 
            card.GetSuitName(), card.GetValueName(), card.IsFaceCard());

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
        
        if (deck.GetCardCount() > 0)
        {
            // get new hand
            hand = deck.Draw(cardsToDraw);
            handText1.text = "";
            handText2.text = "";
            winnerText.text = "";

            // animate card draw            
            GameObject copy1 = Instantiate(cardBack);
            GameObject copy2 = Instantiate(cardBack);
            StartCoroutine(AnimateCardDraw(copy1, copy2));            

            // display cards and announce winner after animation
            StartCoroutine(RevealCardDraw(1.0f, copy1, copy2));
            
        }
        else
        {         
            // notify refresh


            // refresh deck
            Start();
        }
        
        //Debug.Log(deck.CountCurrentSuit('h'));
    }

    private IEnumerator RevealCardDraw(float time, GameObject first, GameObject second)
    {
        yield return new WaitForSeconds(time);
        
        Destroy(first);
        Destroy(second);

        CreateCardSprite(hand[0], 0);
        CreateCardSprite(hand[1], 1);

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