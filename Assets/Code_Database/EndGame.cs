using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using UnityEditor.VersionControl;

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

    private EndTurn endTurn;

    void Awake()
    {
        endTurn = FindAnyObjectByType<EndTurn>();
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
        foreach(string word in endTurn.AllShowWord)
        {
            GameObject M = Instantiate(TextVocabulary,AreaText.transform);
            TMP_Text vocabularyText = M.GetComponent<TMP_Text>();
            vocabularyText.text = word;
        }
    }

    public void ExitGame(){
        PhotonNetwork.LeaveRoom(true);
        PhotonNetwork.LoadLevel("Home");
    }

    public void CaneclShow(){
        AreaShow.SetActive(false);
    }
}
