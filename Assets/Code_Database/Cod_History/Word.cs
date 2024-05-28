using System;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine;

public class Word : MonoBehaviour
{
    public TMP_Text Number;
    private History history;
    private string loggedInUsername;
    public List<string> AddWord;

    //SQL
    private MySqlConnection connection;
    private string connectionString = "Server=192.168.1.163;Database=userandpassword;User=root;Password='';SslMode=none;";

    void Awake()
    {
        loggedInUsername = PlayerPrefs.GetString("LoggedInUsername", "Guest");
        history = FindAnyObjectByType<History>();
        connection = new MySqlConnection(connectionString);
        connection.Open();
    }
    public void WordAll(){
        AddWord = ShowAll(Number.text);
        foreach(string word in AddWord)
        {
            GameObject M = Instantiate(history.TextVocabulary,history.AreaText.transform);
            TMP_Text vocabularyText = M.GetComponent<TMP_Text>();
            vocabularyText.text = word;
        }
    }

    private List<string> ShowAll(string numValue){
        List<string> AddWordShow = new List<string>();
        var sql = "SELECT Word FROM history WHERE Num = @Num AND username = @loggedInUsername";
        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@Num", numValue);
            command.Parameters.AddWithValue("@loggedInUsername", loggedInUsername);
            using (var reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    string word = reader.GetString("Word");
                    AddWordShow.Add(word);
                }
            }
        }
        return AddWordShow;
    }

    public void ShowVocabulary(){
        history.ShowWordHistory.SetActive(true);
    }
}
