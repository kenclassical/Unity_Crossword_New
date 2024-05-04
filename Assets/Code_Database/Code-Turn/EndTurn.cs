using TMPro;
using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;

public class EndTurn : MonoBehaviour{
    public GameObject buttonEnd;
    public GameObject buttonRandom;
    public GameObject buttonCancel;
    public TMP_Text nameTurn;
    public int currentPlayerIndex = 0;
    private float TurnGame = 1.0f;
    private PhotonView PV;
    private Randomitem RandomCheck;
    private WordCheckGrid wordCheckGrid;
    private EndGame endGame;
    public TMP_Text ShowWordTurn;
    public List<string> AllShowWord;
    public int Turn;

    // Area
    public GameObject AreaGame;
    public GameObject AreaEndGame;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        RandomCheck = FindObjectOfType<Randomitem>();
        wordCheckGrid = FindObjectOfType<WordCheckGrid>();
        endGame = FindObjectOfType<EndGame>();
    }
    void Start(){
        AllShowWord = new List<string>();
        nameTurn.text = "Player: " + PhotonNetwork.PlayerList[currentPlayerIndex].NickName + " Turn: " + Mathf.RoundToInt(TurnGame);

        buttonEnd.SetActive(false);
        buttonRandom.SetActive(false);
        buttonCancel.SetActive(false);

        if(PhotonNetwork.IsMasterClient){
            buttonCancel.SetActive(false);
            StartTurnPlayer();
        }
    }

    private void StartTurnPlayer(){
        if (PhotonNetwork.PlayerList[currentPlayerIndex].IsLocal){
            buttonEnd.SetActive(true);
            buttonRandom.SetActive(true);
        } else {
            buttonEnd.SetActive(false);
            buttonRandom.SetActive(false);
        }
    }

    public void EndTurnPlayer(string ShowWord){
        currentPlayerIndex++;
        if (currentPlayerIndex >= PhotonNetwork.PlayerList.Length)
        {
            currentPlayerIndex = 0;
        }
        PV.RPC("DelayStartTurn", RpcTarget.AllBuffered,currentPlayerIndex,ShowWord);
    }

    [PunRPC]
    private IEnumerator DelayStartTurn(int index,string ShowWord)
    {
        TurnGame++;
        if(TurnGame > 10){
            if(wordCheckGrid.ShowWord != ""){
                ShowWordTurn.text = wordCheckGrid.ShowWord;
                AllShowWord.Add(wordCheckGrid.ShowWord);
            }
            AreaGame.SetActive(false);
            AreaEndGame.SetActive(true);
            endGame.ShowEndGame();
        }else{
            Turn = Mathf.CeilToInt(TurnGame / 2);
            RandomCheck.buttonCheck = true;
            RandomCheck.randomButton.image.color = RandomCheck.ColorAlphaButton;
            RandomCheck.textButton.color = RandomCheck.ColorAlphaText;
            currentPlayerIndex = index;
            nameTurn.text = "Player: " + PhotonNetwork.PlayerList[currentPlayerIndex].NickName + " Turn: " + Turn + "/5";
            if(ShowWord != ""){
                ShowWordTurn.text = ShowWord;
                AllShowWord.Add(ShowWord);
            }
            StartTurnPlayer();
        }
        yield return new WaitForSeconds(0.1f);
    }
}