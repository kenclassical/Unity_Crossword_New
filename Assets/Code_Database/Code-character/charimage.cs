using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using MySql.Data.MySqlClient;

public class charimage : MonoBehaviourPunCallbacks
{
    public List<CharatImagedata> displaydeck = new List<CharatImagedata>();
    public int id;
    public string charatletter;
    public Sprite charImage;
    public int scorecharat;

    public Image charter;

    public GameObject Hand;
    public int numberOFCardInDeck;
    private DeckChater deckChater;

    void Awake(){
        deckChater = FindObjectOfType<DeckChater>();
    }
    void Update()
    {
        id = displaydeck[0].id;
        scorecharat = displaydeck[0].scorecharat;
        charatletter = displaydeck[0].charatletter;
        charImage = displaydeck[0].charImage;

        charter.sprite = charImage;

        if (tag == "Clone")
        {
            DrawUniqueRandomCard();
            this.tag = "Untagged";
        }
    }

    void DrawUniqueRandomCard()
    {
        CharatImagedata newCard = null;
        if(deckChater.Letter.Count > 0)
        {
            int randomIndex = Random.Range(0, deckChater.Letter.Count);
            string selectedLetter = deckChater.Letter[randomIndex];
             for (int i = 0; i < DeckChater.startdeck.Count; i++)
            {
                CharatImagedata card = DeckChater.startdeck[i];
                if (card.charatletter.ToLower() == selectedLetter.ToLower())
                {
                    newCard = card;
                    break;
                }
            }

            if (newCard != null)
            {
                displaydeck[0] = newCard;

                deckChater.Letter.Remove(selectedLetter);
            }
        }else{
            int randomIndex = Random.Range(0, DeckChater.startdeck.Count);
            newCard = DeckChater.startdeck[randomIndex];

            if (newCard != null)
            {
                displaydeck[0] = newCard;
            }
        }
    }
}
