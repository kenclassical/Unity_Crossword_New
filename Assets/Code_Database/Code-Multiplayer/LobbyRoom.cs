using UnityEngine;
using Photon.Pun;
using TMPro;

public class LobbyRoom : MonoBehaviour
{
    public TMP_Text lobbyStatusText;

    private void Update()
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            if (playerCount == 2)
            {
                // ถ้ามีผู้เล่นอย่างน้อย 2 คนในห้อง
                lobbyStatusText.text = "Game is starting...";
                PhotonNetwork.LoadLevel("Scenesgame"); // เริ่มโหมดเกมของคุณ
            }
            else
            {
                // ถ้ามีน้อยกว่า 2 คนในห้อง
                lobbyStatusText.text = "Waiting for players1/2...";
            }
        }
    }
}
