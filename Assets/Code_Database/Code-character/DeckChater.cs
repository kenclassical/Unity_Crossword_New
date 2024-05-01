using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckChater : MonoBehaviour
{
    public List<CharatImagedata> deck = new List<CharatImagedata>();
    public static List<CharatImagedata> startdeck = new List<CharatImagedata>();
    public static int decksizenum;
    public int chardarw;
    public GameObject Hand;
    public GameObject CradToHand;
    void Start()
    {
        decksizenum = 26;
        for(int i=0;i<decksizenum;i++){
            deck[i] = CharImageScriptTable.charatList[i];
        }
        StartCoroutine(StartGame());
    }

    // Update is called once per frame
    void Update()
    {
        startdeck = deck;
    }
    
    IEnumerator StartGame()
    {
        if (Hand != null && Hand.transform.childCount < 7)
        {
            // สร้างการ์ดใหม่ในมือจนครบ 7 ในแต่ละเครื่องเล่น
            for (int i = Hand.transform.childCount; i < 7; i++)
            {
                Instantiate(CradToHand, Hand.transform.position, Hand.transform.rotation);
                yield return new WaitForSeconds(0);
            }
        }
    }
}