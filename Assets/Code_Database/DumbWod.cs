using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using UnityEngine;

public class DumbWod : MonoBehaviour
{
    public GameObject DumbWodPrefab;
    public GameObject DumbWordButton;
    public GameObject Hand;
    public List<string> HandLetter;
    public List<string> GridLetter;

    //SQL
    private MySqlConnection connection;
    private string connectionString;
    void Awake()
    {
        HandLetter = new List<string>();
        GridLetter = new List<string>();
        connectionString = "Server=localhost;Database=userandpassword;User=root;Password='';SslMode=none;";
        connection = new MySqlConnection(connectionString);
        connection.Open();
    }
    public void OnDumb(){
        foreach(Transform child in Hand.transform){
            HandLetter.Add(child.GetComponent<charimage>().charatletter);
        }
        GridLetter = Select();
        DumbWodPrefab.SetActive(true);
        DumbWordButton.SetActive(false);
    }

    public void OffDumb(){
        HandLetter.Clear();
        DumbWodPrefab.SetActive(false);
        DumbWordButton.SetActive(true);
    }

    private List<string> Select(){
        List<string> WordAll = new List<string>();
        string sqlQuery = "SELECT * FROM vocabulary WHERE ";
        foreach (string letter in HandLetter)
        {
            sqlQuery += "English_word LIKE '%" + letter + "%' AND ";
            Debug.Log(sqlQuery);
        }
        // ตรวจสอบว่ามีตัวเชื่อม "AND " อยู่ด้านท้ายของคำสั่ง SQL หรือไม่
        if (HandLetter.Count > 0)
        {
            sqlQuery = sqlQuery.Remove(sqlQuery.Length - 5);
            Debug.Log("2");
        }

        using (var command = new MySqlCommand(sqlQuery, connection))
        {   
            var reader = command.ExecuteReader();
            while (reader.Read()){
                string word = reader["English_word"].ToString();
                WordAll.Add(word);
                Debug.Log("1");
            }
            reader.Close();
        }
        return WordAll;
    }


}
