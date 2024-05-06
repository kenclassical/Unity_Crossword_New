using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class History : MonoBehaviour
{
    //GameObject
    public GameObject ShowArea;
    public GameObject Area;
    string loggedInUsername;
    public GameObject ShowWordHistory;
    public GameObject AreaText;
    public GameObject TextVocabulary;

    //SQL
    private MySqlConnection connection;
    private string connectionString = "Server=localhost;Database=userandpassword;User=root;Password='';SslMode=none;";
    void Awake()
    {
        loggedInUsername = PlayerPrefs.GetString("LoggedInUsername", "Guest");
        connection = new MySqlConnection(connectionString);
        connection.Open();
        ShowAll();
    }
    public void ToBack(){
        SceneManager.LoadScene("Home");
    }

    private void ShowAll(){
        int NumWhile = CountDistinctNum();
        int Num = 1;
        for(int i = 0 ; i < NumWhile ; i++){
            var sql = "SELECT DISTINCT * FROM history WHERE Num = @Num AND username = @loggedInUsername";
            using (var command = new MySqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Num", Num);
                command.Parameters.AddWithValue("@loggedInUsername", loggedInUsername);
                using (var reader = command.ExecuteReader())
                {
                    if(reader.Read()){
                        string p1 = reader.GetString("NameOne");
                        string s1 = reader.GetString("ScoreOne");
                        string p2 = reader.GetString("NameTwo");
                        string s2 = reader.GetString("ScoreTwo");
                        string sum = reader.GetString("Sum");
                        ShowAllHistory(p1, p2, s1, s2, sum,Num);
                    }
                }
            }
            Num++;
        }
    }

    private int CountDistinctNum()
    {
        int num = 0;
        var sql = "SELECT COUNT(DISTINCT Num) AS NumCount FROM history WHERE username = @loggedInUsername";
        using (var command = new MySqlCommand(sql, connection))
        {
            command.Parameters.AddWithValue("@loggedInUsername", loggedInUsername);
            using (var reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    num = reader.GetInt32("NumCount");
                }
            }

        }
        return num;
    }

    private void ShowAllHistory(string Player1, string Player2, string Score1, string Score2,string SumAll,int Num){
        GameObject S = Instantiate(ShowArea,Area.transform);
        TMP_Text p1Text = S.transform.Find("P1").GetComponent<TMP_Text>();
        TMP_Text p2Text = S.transform.Find("P2").GetComponent<TMP_Text>();
        TMP_Text s1Text = S.transform.Find("S1").GetComponent<TMP_Text>();
        TMP_Text s2Text = S.transform.Find("S2").GetComponent<TMP_Text>();
        TMP_Text sumText = S.transform.Find("SUM").GetComponent<TMP_Text>();
        TMP_Text NumText = S.transform.Find("Num").GetComponent<TMP_Text>();

        p1Text.text = Player1;
        p2Text.text = Player2;
        s1Text.text = Score1;
        s2Text.text = Score2;
        sumText.text = SumAll;
        NumText.text = Num.ToString();
        NumText.enabled = false;
    }

    public void CancelShow(){
        foreach(Transform child in AreaText.transform){
            Destroy(child.gameObject);
        }
        ShowWordHistory.SetActive(false);
    }

}
