using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;
    public GameObject joinButton;
    public GameObject cancelButton;

    public byte roomMaxPlayers;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Trying to connect to the Master server");
        PhotonNetwork.ConnectUsingSettings();   //Connect to the Master photon server
        PhotonNetwork.ConnectToRegion("eu");
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
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = roomMaxPlayers };
        PhotonNetwork.CreateRoom("Room" + randomRoomName, roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create room but failed. There must already be a room with the same name");
        createRandomRoom();
        base.OnCreateRoomFailed(returnCode, message);
    }

    public void onCancelButtonClicked()
    {
        Debug.Log("Left the " + PhotonNetwork.CurrentRoom.Name);

        cancelButton.SetActive(false);
        joinButton.SetActive(true);

        PhotonNetwork.LeaveRoom();
    }

    private void Awake()
    {
        lobby = this;   //Creates a singleton. Lives within the Main menu scene
    }
}
