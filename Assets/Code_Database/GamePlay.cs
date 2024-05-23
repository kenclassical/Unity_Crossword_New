using TMPro;
using Photon.Pun;

public class GamePlay : MonoBehaviourPun
{
    public TMP_Text Playone;
    public TMP_Text Playtwo;
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
        int  playerCount = PhotonNetwork.PlayerList.Length;
        if(playerCount < 2){
            PhotonNetwork.LeaveRoom(true);
            PhotonNetwork.LoadLevel("Home");
        }
    }  
}
