using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


public class HomeCode : MonoBehaviourPunCallbacks
{
    public TMP_Text usernameText;
    public GameObject CrateroomCanvas;
    public GameObject JoinroomCanvas;
    public TMP_InputField CrateroomNameRoom;
    public TMP_InputField JoinNameRoom;
    private void Start()
    {
        string loggedInUsername = PlayerPrefs.GetString("LoggedInUsername", "Guest");
        usernameText.text = loggedInUsername;
        CrateroomCanvas.SetActive(false);
        JoinroomCanvas.SetActive(false);
        PhotonNetwork.NickName = usernameText.text;
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master Server");
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public void CheckUsername(){
        SceneManager.LoadScene("EditUsername");
    }

    public void JoinCanvas(){
        JoinroomCanvas.SetActive(true);
    }
    public void JoinRoom(){
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom(JoinNameRoom.text,roomOptions,TypedLobby.Default);
    }

    public void CreateCanvas(){
        CrateroomCanvas.SetActive(true);
    }
    public void CreateRoom(){
        if(CrateroomNameRoom.text.Length >= 1){
            PhotonNetwork.CreateRoom(CrateroomNameRoom.text,new RoomOptions(){MaxPlayers = 2},null);
        }
    }

    public void Cancelroom(){
        if (JoinroomCanvas.activeSelf == true) {
            JoinroomCanvas.SetActive(false);
        } else {
            CrateroomCanvas.SetActive(false);
        }
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("LobbyRoom");
    }

    public void Playhistory(){
        SceneManager.LoadScene("History");
    }

}
