using UnityEngine;
using Photon.Pun;
using TMPro;
using MySql.Data.MySqlClient;
using UnityEngine.UI;
using System;

public class EndGame : MonoBehaviour
{
    public TMP_Text Playone;
    public TMP_Text Playtwo;
    public TMP_Text ScorePlayerOneText;
    public TMP_Text ScorePlayerTwoText;
    public TMP_Text Sum;
    public int scorePlayerOne;
    public int scorePlayerTwo;
    public GameObject AreaShow;
    public GameObject TextVocabulary;
    public GameObject AreaText;
    private int Num;
    private EndTurn endTurn;
    string loggedInUsername;

    //SQL
    private MySqlConnection connection;
    private string connectionString = "Server=localhost;Database=userandpassword;User=root;Password='';SslMode=none;";

    void Awake()
    {
        endTurn = FindAnyObjectByType<EndTurn>();
        connection = new MySqlConnection(connectionString);
        connection.Open();
    }

    void Start()
    {
        loggedInUsername = PlayerPrefs.GetString("LoggedInUsername", "Guest");
        Debug.Log(loggedInUsername);
        Num = 1;     
    }

    public void Score(int ScorePlayerOne,int ScorePlayerTwo){
        scorePlayerOne = ScorePlayerOne;
        ScorePlayerOneText.text = scorePlayerOne.ToString();
        scorePlayerTwo = ScorePlayerTwo;
        ScorePlayerTwoText.text = scorePlayerTwo.ToString();
    }

    public void ShowEndGame(){
        Playone.text = PhotonNetwork.PlayerList[0].NickName;
        Playtwo.text = PhotonNetwork.PlayerList[1].NickName;
        foreach(string word in endTurn.AllShowWord)
        {
            GameObject M = Instantiate(TextVocabulary,AreaText.transform);
            TMP_Text vocabularyText = M.GetComponent<TMP_Text>();
            vocabularyText.text = word;
        }
        if (scorePlayerOne > scorePlayerTwo){
            Sum.text = "PLAYER: " + Playone.text + " WIN";
        }else if (scorePlayerOne < scorePlayerTwo){
            Sum.text = "PLAYER: " + Playtwo.text + " WIN";
        }else if (scorePlayerOne == scorePlayerTwo){
            Sum.text = "DRAW";  
        }
    }

    public void ShowVocabulary(){
        AreaShow.SetActive(true);
    }

    private void AddSQL(){
        Num = GetNextAvailableNum();
        var query = "INSERT INTO `history`(`NameOne`, `NameTwo`, `ScoreOne`, `ScoreTwo`, `Num`, `Sum`, `Word`,`username`) VALUES (@Playone,@Playtwo,@scorePlayerOne,@scorePlayerTwo,@Num,@Sum,@Word,@loggedInUsername)";
        foreach (string word in endTurn.AllShowWord)
        {
            MySqlCommand innerCmd = new MySqlCommand(query, connection);
            innerCmd.Parameters.AddWithValue("@Playone", Playone.text);
            innerCmd.Parameters.AddWithValue("@Playtwo", Playtwo.text);
            innerCmd.Parameters.AddWithValue("@scorePlayerOne", scorePlayerOne);
            innerCmd.Parameters.AddWithValue("@scorePlayerTwo", scorePlayerTwo);
            innerCmd.Parameters.AddWithValue("@Num", Num);
            innerCmd.Parameters.AddWithValue("@Sum", Sum.text);
            innerCmd.Parameters.AddWithValue("@Word", word);
            innerCmd.Parameters.AddWithValue("@loggedInUsername", loggedInUsername);
            innerCmd.ExecuteNonQuery();
        }
    }

    private int GetNextAvailableNum()
    {
        int num = 1;
        
        while (true)
        {
            var query = "SELECT COUNT(*) FROM history WHERE Num = @Num AND username = @loggedInUsername";
            
            using (MySqlCommand innerCmd = new MySqlCommand(query, connection))
            {
                innerCmd.Parameters.AddWithValue("@Num", num);
                innerCmd.Parameters.AddWithValue("@loggedInUsername", loggedInUsername);
                
                int count = Convert.ToInt32(innerCmd.ExecuteScalar());
                
                if (count == 0)
                {
                    return num;
                }
                
                num++;
            }
        }
    }


    public void ExitGame(){
        AddSQL();
        PhotonNetwork.LeaveRoom(true);
        PhotonNetwork.LoadLevel("Home");
    }

    public void CaneclShow(){
        AreaShow.SetActive(false);
    }
}
