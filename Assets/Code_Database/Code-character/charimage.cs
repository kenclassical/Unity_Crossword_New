using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

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
        if (DeckChater.startdeck.Count > 0)
        {
            int randomIndex = -1;
            CharatImagedata newCard;
            do
            {
                randomIndex = Random.Range(0, DeckChater.startdeck.Count);
                newCard = DeckChater.startdeck[randomIndex];
            } while (displaydeck.Exists(card => card.id == newCard.id));

            displaydeck[0] = newCard;
            DeckChater.startdeck.RemoveAt(randomIndex);
        }
    }
}
