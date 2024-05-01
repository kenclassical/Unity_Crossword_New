using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Randomitem : MonoBehaviour
{
    public Button randomButton;
    private Button cancelBTN;
    public GameObject HandRandom;
    public GameObject Hand;
    public GameObject Area;
    public GameObject RandomText;
    [SerializeField] public TMP_Text textButton;
    [SerializeField] private GameObject cancelButton;
    [SerializeField] private GameObject endButton;
    private bool stateRandom;
    public bool buttonCheck = true;
    public charimage charImageScript;
    private DeckChater deckChaterInstance;
    private Color ColorAlpha50;
    public Color ColorAlphaButton;
    public Color ColorAlphaText;
    void Awake()
    {
        randomButton = GetComponent<Button>();
        cancelBTN = cancelButton.GetComponent<Button>();
        deckChaterInstance = FindObjectOfType<DeckChater>();
        ColorAlpha50.a = 0.5f;
        ColorAlphaButton = new Color(255f/255f, 212f/255f, 103f/255f);
        ColorAlphaText = new Color(255f/255f, 255f/255f, 255f/255f);
    }

    void Start()
    {
        randomButton.onClick.AddListener(() => {
            stateRandom = !stateRandom;
            if(stateRandom && buttonCheck)
            {
                StateRandom();
            }else if(!stateRandom && buttonCheck)
            {
                StateStatic();
            }
        });

        cancelBTN.onClick.AddListener(() => {
            stateRandom = false;
            Cancel();
        });
    }

    private void StateRandom()
    {
        textButton.text = "Confirm";
        HandRandom.SetActive(true);
        Area.SetActive(true);
        RandomText.SetActive(true);
        cancelButton.SetActive(true);
        endButton.SetActive(false);
    }

    private void StateStatic()
    {
        while (Hand.transform.childCount < 7)
        {
            Instantiate(deckChaterInstance.CradToHand, Hand.transform.position, Hand.transform.rotation, Hand.transform);
        }

        if(HandRandom != null && HandRandom.transform.childCount > 0){
            List<CharatImagedata> newData = new List<CharatImagedata>();
            foreach (Transform child in HandRandom.transform)
            {
                int id = child.GetComponent<charimage>().id;
                string charatletter = child.GetComponent<charimage>().charatletter;
                Sprite charImage = child.GetComponent<charimage>().charImage;
                int scorecharat = child.GetComponent<charimage>().scorecharat;

                newData.Add(new CharatImagedata(id, charatletter, scorecharat, charImage));    
                Destroy(child.gameObject);
            }
            deckChaterInstance.deck.AddRange(newData);
            randomButton.image.color = ColorAlpha50;
            textButton.color = ColorAlpha50;
            buttonCheck = false;
        }
        textButton.text = "Random";
        HandRandom.SetActive(false);
        Area.SetActive(false);
        RandomText.SetActive(false);
        cancelButton.SetActive(false);
        endButton.SetActive(true);
    }

    private void Cancel()
    {
        if(HandRandom != null && HandRandom.transform.childCount > 0){
            for(int i = 0; i <= HandRandom.transform.childCount+1; i++ ){
                foreach (Transform child in HandRandom.transform)
                {
                    child.SetParent(Hand.transform);
                }
            }
        }
        textButton.text = "Random";
        HandRandom.SetActive(false);
        Area.SetActive(false);
        RandomText.SetActive(false);
        cancelButton.SetActive(false);
        endButton.SetActive(true);
    }
    
}

