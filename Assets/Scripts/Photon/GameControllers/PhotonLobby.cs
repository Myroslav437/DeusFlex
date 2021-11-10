using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class PhotonLobby : MonoBehaviourPunCallbacks
{
    public static PhotonLobby lobby;
    public GameObject joinButton;
    public GameObject offlineButton;
    public GameObject hostGameButton;
    public GameObject cancelButtonPlayGame;
    public GameObject cancelButtonHostGame;
    public GameObject cancelButtonConnect;
    public GameObject cancelButtonEnterRoom;
    public GameObject EnterRoomButton;
    public GameObject ConnectRoomButton;
    public InputField nicknameInput;
    public InputField roomnameInput;

    public byte roomMaxPlayers;
    private int roomNameSize = 8;

    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsConnected) { 
            Debug.Log("Trying to connect to the Master server");
            PhotonNetwork.ConnectUsingSettings();   //Connect to the Master photon server
            PhotonNetwork.ConnectToRegion("eu");
        }

        offlineButton.SetActive(true);
        joinButton.SetActive(false);
        hostGameButton.SetActive(true);
        cancelButtonPlayGame.SetActive(false);
        cancelButtonHostGame.SetActive(false);
        cancelButtonConnect.SetActive(false);
        cancelButtonEnterRoom.SetActive(false);
        EnterRoomButton.SetActive(true);
        ConnectRoomButton.SetActive(false);
        nicknameInput.gameObject.SetActive(true);
        roomnameInput.gameObject.SetActive(false);
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Player has connected to the Photon Master server");
        PhotonNetwork.AutomaticallySyncScene = true;

        joinButton.SetActive(true);
        cancelButtonPlayGame.SetActive(false);
        offlineButton.SetActive(false);

        base.OnConnectedToMaster();
    }

    public void onJoinButtonClicked()
    {
        joinButton.SetActive(false);
        cancelButtonPlayGame.SetActive(true);

        if (LocalPlayerInfo.LPI != null)
        {
            LocalPlayerInfo.LPI.myNickName = GetNickName();
            PlayerPrefs.SetString("MyNickName", LocalPlayerInfo.LPI.myNickName);
        }

        PhotonRoom.room.isHostedRoom = false;
        PhotonNetwork.JoinRandomRoom();
    }

    public void onConnectRoomButtonClicked()
    {
        ConnectRoomButton.SetActive(false);
        cancelButtonConnect.SetActive(true);

        if (LocalPlayerInfo.LPI != null)
        {
            LocalPlayerInfo.LPI.myNickName = GetNickName();
            PlayerPrefs.SetString("MyNickName", LocalPlayerInfo.LPI.myNickName);
        }
        string roomName = roomnameInput.text;

        PhotonRoom.room.isHostedRoom = true;
        PhotonNetwork.JoinRoom(roomName);
    }

    public void onHostGamaButtonClicked()
    {
        hostGameButton.SetActive(false);
        cancelButtonHostGame.SetActive(true);

        if (LocalPlayerInfo.LPI != null)
        {
            LocalPlayerInfo.LPI.myNickName = GetNickName();
            PlayerPrefs.SetString("MyNickName", LocalPlayerInfo.LPI.myNickName);
        }

        string roomName = GenerateRoomName(roomNameSize);

        RoomOptions roomOps = new RoomOptions() { IsVisible = false, IsOpen = true, MaxPlayers = roomMaxPlayers };
        PhotonNetwork.CreateRoom(roomName, roomOps);
        PhotonRoom.room.isHostedRoom = true;
    }

    public void onEnterRoomButtonClicked()
    {
        EnterRoomButton.SetActive(false);
        roomnameInput.gameObject.SetActive(true);
        ConnectRoomButton.SetActive(true);
        cancelButtonEnterRoom.SetActive(true);
        cancelButtonConnect.SetActive(false);
    }

    public void onCancelButtonConnectClicked()
    {
        Debug.Log("Left the " + PhotonNetwork.CurrentRoom.Name);

        cancelButtonConnect.SetActive(false);
        ConnectRoomButton.SetActive(true);

        PhotonNetwork.LeaveRoom();
    }

    public void oncancelButtonEnterRoomClicked() 
    {
        EnterRoomButton.SetActive(true);
        roomnameInput.gameObject.SetActive(false);
        ConnectRoomButton.SetActive(false);
        cancelButtonConnect.SetActive(false);
        cancelButtonEnterRoom.SetActive(false);

        roomnameInput.text = "";
        roomnameInput.textComponent.color = Color.black;
    }

    private string GenerateRoomName(int size) {
        const string alphanumericCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ" + "abcdefghijklmnopqrstuvwxyz" + "0123456789";
        IEnumerable<char> characterSet = alphanumericCharacters;

        var characterArray = characterSet.Distinct().ToArray();
        if (characterArray.Length == 0)
            throw new ArgumentException("characterSet must not be empty", "characterSet");

        var bytes = new byte[size * 8];
        var result = new char[size];
        using (var cryptoProvider = new RNGCryptoServiceProvider()) {
            cryptoProvider.GetBytes(bytes);
        }
        for (int i = 0; i < size; i++) {
            ulong value = BitConverter.ToUInt64(bytes, i * 8);
            result[i] = characterArray[value % (uint)characterArray.Length];
        }

        return new string(result);
    }

    public override void OnJoinRoomFailed(short returnCode, string message) 
    {
        Debug.Log("Tried to join a random room but failed. Cannot find specified room");
        roomnameInput.textComponent.color = new Color(1, 0, 0);
        cancelButtonConnect.SetActive(false);
        ConnectRoomButton.SetActive(true);

        base.OnJoinRoomFailed(returnCode, message);
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

        int randomRoomName = UnityEngine.Random.Range(0, 10000);
        RoomOptions roomOps = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = roomMaxPlayers };
        PhotonNetwork.CreateRoom(GenerateRoomName(roomNameSize), roomOps);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to create room but failed. There must already be a room with the same name");
        createRandomRoom();
        base.OnCreateRoomFailed(returnCode, message);
    }

    public void onCancelButtonPlayGameClicked()
    {
        Debug.Log("Left the " + PhotonNetwork.CurrentRoom.Name);

        cancelButtonPlayGame.SetActive(false);
        joinButton.SetActive(true);

        PhotonNetwork.LeaveRoom();
    }

    public void onCancelButtonHostGameClicked() 
    {
        Debug.Log("Left the " + PhotonNetwork.CurrentRoom.Name);

        cancelButtonHostGame.SetActive(false);
        hostGameButton.SetActive(true);

        PhotonNetwork.LeaveRoom();
    }

    public void onExitButtonPressed() 
    {
        Application.Quit();
    }

    private void Awake()
    {
        lobby = this;   //Creates a singleton. Lives within the Main menu scene
    }

    string GetNickName()
    {
        string nick = nicknameInput.text;
        if (!CheckNickname(nick))
        {
            nick = "Player" + UnityEngine.Random.Range(0, 1000).ToString();
        }

        return nick;
    }

    public bool CheckNickname(string nick)
    {
        if (nick.Length == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
