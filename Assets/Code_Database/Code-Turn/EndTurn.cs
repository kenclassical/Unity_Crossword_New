using TMPro;
using UnityEngine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class EndTurn : MonoBehaviour{
    public GameObject buttonEnd;
    public GameObject buttonRandom;
    public GameObject buttonCancel;
    public GameObject buttonDumbWord;
    public GameObject Hand;
    public TMP_Text nameTurn;
    public int currentPlayerIndex = 0;
    public float TurnGame = 1.0f;
    private PhotonView PV;
    private Randomitem RandomCheck;
    private WordCheckGrid wordCheckGrid;
    private EndGame endGame;
    public TMP_Text ShowWordTurn;
    public List<string> AllShowWord;
    public List<string> AllShowWordPlayer1;
    public List<string> AllShowWordPlayer2;
    public int Turn;

    // Area
    public GameObject AreaGame;
    public GameObject AreaEndGame;

    private DumbWod dumbWord;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        RandomCheck = FindObjectOfType<Randomitem>();
        wordCheckGrid = FindObjectOfType<WordCheckGrid>();
        endGame = FindObjectOfType<EndGame>();
        dumbWord = FindObjectOfType<DumbWod>();
        AllShowWordPlayer1 = new List<string>();
        AllShowWordPlayer2 = new List<string>();
        AllShowWord = new List<string>();
    }
    void Start(){
        
        nameTurn.text = "Player: " + PhotonNetwork.PlayerList[currentPlayerIndex].NickName + " Turn: " + Mathf.RoundToInt(TurnGame) + "/5";

        buttonEnd.SetActive(false);
        buttonRandom.SetActive(false);
        buttonCancel.SetActive(false);
        buttonDumbWord.SetActive(false);

        if(PhotonNetwork.IsMasterClient){
            buttonCancel.SetActive(false);
            StartTurnPlayer();
        }
    }

    private void StartTurnPlayer(){
        if (PhotonNetwork.PlayerList[currentPlayerIndex].IsLocal){
            foreach(Transform child in Hand.transform){
                child.GetComponent<Image>().raycastTarget = true;
            }
            buttonEnd.SetActive(true);
            buttonRandom.SetActive(true);
            buttonDumbWord.SetActive(true);
        } else {
            foreach(Transform child in Hand.transform){
                child.GetComponent<Image>().raycastTarget = false;
            }
            buttonEnd.SetActive(false);
            buttonRandom.SetActive(false);
            buttonDumbWord.SetActive(false);
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
            if(!dumbWord.OnAndOff){
                dumbWord.Del();
            }
            RandomCheck.randomButton.image.color = RandomCheck.ColorAlphaButton;
            RandomCheck.textButton.color = RandomCheck.ColorAlphaText;
            currentPlayerIndex = index;
            nameTurn.text = "Player: " + PhotonNetwork.PlayerList[currentPlayerIndex].NickName + " Turn: " + Turn + "/5";
            if(ShowWord != ""){
                ShowWordTurn.text = ShowWord;
                if(currentPlayerIndex == 1){
                    AllShowWordPlayer1.Add(ShowWord);
                    Debug.Log("1");
                }else{
                    AllShowWordPlayer2.Add(ShowWord);
                    Debug.Log("2");
                }
            }
            StartTurnPlayer();
        }
        yield return new WaitForSeconds(0.1f);
    }
}