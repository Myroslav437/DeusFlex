using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonLobby : MonoBehaviourPunCallbacks
{

    public static PhotonLobby lobby;
    public GameObject joinButton;
    public GameObject cancelButton;

    private void Awake()
    {
        lobby = this;   //Creates a singleton. Lives within the Main menu scene
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Trying to connect to the Master server");
        PhotonNetwork.ConnectUsingSettings();   //Connect to the Master photon server
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the Photon Master server");
        PhotonNetwork.AutomaticallySyncScene = true;

        joinButton.SetActive(true);
        base.OnConnectedToMaster();
    }

    public void onJoinButtonClicked() 
    {
        joinButton.SetActive(false);
        cancelButton.SetActive(true);

        PhotonNetwork.JoinRandomRoom();
    }

    public void onCancelButtonClicked() 
    {
        Debug.Log("Left the " + PhotonNetwork.CurrentRoom.Name);

        cancelButton.SetActive(false);
        joinButton.SetActive(true);

        PhotonNetwork.LeaveRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join a random room but failed. There must be no active games avaliable");
        createRandomRoom();
        base.OnJoinRandomFailed(returnCode, message);
    }

    void createRandomRoom() 
    {
        Debug.Log("Creating a new random room");

        int randomRoomName = Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 10 };
        PhotonNetwork.CreateRoom("room" + randomRoomName, roomOps);
    }



    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create room but failed. There must already be a room with the same name");
        createRandomRoom();
        base.OnCreateRoomFailed(returnCode, message);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
