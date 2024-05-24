using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine;

public class DumbWod : MonoBehaviour
{
    public GameObject DumbWodPrefab;
    public GameObject DumbWordButton;
    public GameObject Hand;
    public GameObject DumbWordShowAll;
    public GameObject TextVocabulary;

    private WordCheckGrid wordCheckGrid;
    private EndTurn endTurn;

    public List<string> dumbWordShow;
    private List<string> AllWord;
    public string letter;

    public List<string> AllWordRandom;

    public bool OnAndOff = true;

    //SQL
    private MySqlConnection connection;
    private string connectionString;
    void Awake()
    {
        dumbWordShow = new List<string>();
        AllWord = new List<string>();
        AllWordRandom = new List<string>();
        wordCheckGrid = FindAnyObjectByType<WordCheckGrid>();
        endTurn = FindAnyObjectByType<EndTurn>();
        connectionString = "Server=10.50.16.95;Database=userandpassword;User=root;Password='';SslMode=none;";
        connection = new MySqlConnection(connectionString);
        connection.Open();
    }
    public void OnDumb(){
        if(OnAndOff == true){
            foreach (Transform child in Hand.transform)
            {
                string charatletter = child.GetComponent<charimage>().charatletter;
                letter += charatletter;
            }
            CheckedWord(letter.ToLower());
            foreach(string word in dumbWordShow){
                GameObject M = Instantiate(TextVocabulary,DumbWordShowAll.transform);
                TMP_Text vocabularyText = M.GetComponent<TMP_Text>();
                vocabularyText.text = word;
            }
            OnAndOff = false;
            Debug.Log(OnAndOff);
        }
        DumbWodPrefab.SetActive(true);
        DumbWordButton.SetActive(false);
    }

    public void OffDumb(){
        DumbWodPrefab.SetActive(false);
        DumbWordButton.SetActive(true);
    }

    public void Del(){
        foreach(Transform child in DumbWordShowAll.transform){
            Destroy(child.gameObject);
        }
        letter = "";
        dumbWordShow = new List<string>();
        OnAndOff = true;
    }

    private List<string> SelectAll(){
        var resultList = new List<string>();
        var sql = "SELECT English_word FROM vocabulary WHERE LENGTH(English_word) <= 7 AND LENGTH(English_word) > 1;";
        using(var cmd = new MySqlCommand(sql, connection)){
            using (var reader = cmd.ExecuteReader()){
                while (reader.Read()){
                    string englishWord = reader.GetString(0);
                    resultList.Add(englishWord.ToLower());
                }
            }
        }
        return resultList;
    }

    private void CheckedWord(string lettersToCheck){
        if (AllWordRandom.Count == 0)
        {
            Debug.Log("1");
            AllWord = SelectAll();

            HashSet<string> matchingWords = new HashSet<string>(AllWord.Where(word => CanFormWord(lettersToCheck, word.ToLower())));

            foreach (string word in matchingWords)
            {
                dumbWordShow.Add(word);
            }
        }
        else
        {
            HashSet<string> matchingWords = new HashSet<string>(AllWordRandom.Where(word => CanFormWord(lettersToCheck, word.ToLower())));

            foreach (string word in matchingWords)
            {
                dumbWordShow.Add(word);
            }
        }
    }

    private static bool CanFormWord(string lettersToCheck, string word)
    {
        Dictionary<char, int> letterCount = lettersToCheck.GroupBy(c => c).ToDictionary(g => g.Key, g => g.Count());

        foreach (char c in word)
        {
            if (!letterCount.ContainsKey(c) || letterCount[c] == 0)
            {
                return false;
            }
            letterCount[c]--;
        }
        return true;
    }
}
