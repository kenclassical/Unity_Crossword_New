using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class DeckChater : MonoBehaviour
{
    public List<CharatImagedata> deck = new List<CharatImagedata>();
    public static List<CharatImagedata> startdeck = new List<CharatImagedata>();
    public static int decksizenum;
    public int chardarw;
    public GameObject Hand;
    public GameObject CradToHand;
    public List<string> Letter = new List<string>();

    //SQL
    private MySqlConnection connection;
    private string connectionString = "Server=localhost;Database=userandpassword;User=root;Password='';SslMode=none;";
    void Start()
    {
        connection = new MySqlConnection(connectionString);
        connection.Open();
        decksizenum = 26;
        for(int i=0;i<decksizenum;i++){
            deck[i] = CharImageScriptTable.charatList[i];
        }
        StartCoroutine(StartGame());
        SelectStart();
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
            for (int i = Hand.transform.childCount; i < 7; i++)
            {
                Instantiate(CradToHand, Hand.transform.position, Hand.transform.rotation);
                if(!PhotonNetwork.IsMasterClient){
                    CradToHand.GetComponent<Image>().raycastTarget = false;
                }
                yield return new WaitForSeconds(0);
            }
        }
    }
    private void SelectStart(){
        var resultList = new List<string>();
        var sql = "SELECT English_word FROM vocabulary WHERE LENGTH(English_word) = 7;";
        using(var cmd = new MySqlCommand(sql, connection)){
            using (var reader = cmd.ExecuteReader()){
                while (reader.Read()){
                    string englishWord = reader.GetString(0);
                    resultList.Add(englishWord);
                }
            }
        }
        RandomStart(resultList);
    }

   public void RandomStart(List<string> resultList){
        List<string> selectedWords = new List<string>();
        int totalLength = 0;

        while (totalLength <= 7 && resultList.Count > 0)
        {
            int randomIndex = Random.Range(0, resultList.Count);
            string selectedWord = resultList[randomIndex];
            int wordLength = selectedWord.Length;

            if (totalLength + wordLength <= 7)
            {
                selectedWords.Add(selectedWord);
                totalLength += wordLength;

                if (totalLength == 6)
                {
                    break;
                }
            }
            else
            {
                resultList.RemoveAt(randomIndex);
            }
        }

        foreach (string word in selectedWords)
        {
            Debug.Log(word);
            foreach (char letter in word)
            {
                Letter.Add(letter.ToString());
            }
        }

        if (Letter.Count == 6)
        {
            int randomIndex = Random.Range(0, startdeck.Count);
            CharatImagedata randomCard = startdeck[randomIndex];
            Letter.Add(randomCard.charatletter);
        }
    }
}