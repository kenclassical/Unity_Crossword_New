using UnityEngine;
using Photon.Pun;
using TMPro;

public class EndGame : MonoBehaviour
{
    public TMP_Text Playone;
    public TMP_Text Playtwo;
    public TMP_Text ScorePlayerOneText;
    public TMP_Text ScorePlayerTwoText;
    public TMP_Text Sum;
    public int scorePlayerOne;
    public int scorePlayerTwo;

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
            Sum.text = "ALWAYS";  
        }
    }
}
