using TMPro;
using Photon.Pun;
using System.Collections;
using UnityEngine;
using Photon.Realtime;

public class GamePlay : MonoBehaviourPun
{
    public TMP_Text Playone;
    public TMP_Text Playtwo;
    public TMP_Text Sone;
    public TMP_Text Stwo;
    public GameObject Showleave;
    public GameObject EndGame;
    private bool isLeaving = false;
    private EndTurn endTurn;
    private void Awake()
    {
        endTurn = FindObjectOfType<EndTurn>();
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (!player.IsLocal)
            {
                Playtwo.text = player.NickName;
            }
            else 
            {
                Playone.text = player.NickName;
            }
        }
        endTurn.AllShowWordPlayer1.Add("Player 1:" + Playone.text);
        endTurn.AllShowWordPlayer2.Add("Player 2:" + Playtwo.text);
        if(PhotonNetwork.IsMasterClient){
            Sone.rectTransform.anchoredPosition = new Vector2(-270f, Sone.rectTransform.anchoredPosition.y);
            Stwo.rectTransform.anchoredPosition = new Vector2(270f, Sone.rectTransform.anchoredPosition.y);
        }else{
            Sone.rectTransform.anchoredPosition = new Vector2(270f, Sone.rectTransform.anchoredPosition.y);
            Stwo.rectTransform.anchoredPosition = new Vector2(-270f, Sone.rectTransform.anchoredPosition.y);
        }
    }

    private void Update() {
        int playerCount = PhotonNetwork.PlayerList.Length;
        if (playerCount < 2 && !isLeaving && !EndGame.activeSelf) {
            StartCoroutine(LeaveRoomProcess());
        }
    }

    private IEnumerator LeaveRoomProcess() {
        isLeaving = true;

        if (PhotonNetwork.InRoom && PhotonNetwork.NetworkClientState == ClientState.Joined) {
            PhotonNetwork.LeaveRoom(true);

            while (PhotonNetwork.InRoom || PhotonNetwork.NetworkClientState == ClientState.Leaving) {
                yield return null;
            }

            Showleave.SetActive(true);
            yield return new WaitForSeconds(5);

            PhotonNetwork.LoadLevel("Home");
        }

        isLeaving = false;
    }
}
