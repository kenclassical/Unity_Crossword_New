using TMPro;
using Photon.Pun;
using System.Collections;
using UnityEngine;
using Photon.Realtime;

public class GamePlay : MonoBehaviourPun
{
    public TMP_Text Playone;
    public TMP_Text Playtwo;
    public GameObject Showleave;
    private bool isLeaving = false;
    private void Awake()
    {
        // foreach (Player player in PhotonNetwork.PlayerList)
        // {
        //     if (!player.IsLocal)
        //     {
        //         Playtwo.text = player.NickName;
        //     }else 
        //     {
        //         Playone.text = player.NickName;
        //     }
        // }
        Playone.text = PhotonNetwork.PlayerList[0].NickName;
        Playtwo.text = PhotonNetwork.PlayerList[1].NickName;
    }

    private void Update() {
        int playerCount = PhotonNetwork.PlayerList.Length;
        if (playerCount < 2 && !isLeaving) {
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
