using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;


public class WaitingRoomController : MonoBehaviourPunCallbacks
{
    private static WaitingRoomController RC;
    private PhotonView PV;

    public Text roomCountDisplay;
    public Text countDownDisplay;
    public Text playersNicks;

    public int multiplayerScene;
    public int menuScene;

    public int roomSize;
    public float timeToWait;
    //public float maxWaitTime;

    private int playerCount;
    private float countDownTime;

    private bool readyToStart;
    private bool startingGame;


    private void Awake()
    {
        if (WaitingRoomController.RC == null) {
            WaitingRoomController.RC = this;
        }
        else if (WaitingRoomController.RC != this) {
            Destroy(WaitingRoomController.RC.gameObject);
            WaitingRoomController.RC = this;
        }

        // DontDestroyOnLoad(this.gameObject);
        PV = GetComponent<PhotonView>();
        countDownTime = timeToWait;
    }

    void Start()
    {
        PV.RPC("RPC_PlayerCountUpdate", RpcTarget.MasterClient);
        PlayerNicknamesUpdate();

        if (PhotonNetwork.IsMasterClient) {
            Debug.Log("I am a master client");
        }
    }

    void Update()
    {
        WaitForMorePlayers();
    }

    void WaitForMorePlayers()
    {
        if (readyToStart)  { // full room
            countDownTime -= Time.deltaTime;
        }
        else {
            ResetTimer();
        }

        countDownDisplay.text = string.Format("{0:00}", countDownTime);

        if (countDownTime <= 0f) {
            if (startingGame) {
                return;
            }
            StartGame();
        }
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("WRController: Joined the " + PhotonNetwork.CurrentRoom.Name);

        PlayerNicknamesUpdate();
        PV.RPC("PRC_PlayerNicknamesSet", RpcTarget.AllBufferedViaServer, playersNicks.text);

        base.OnJoinedRoom();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (PhotonNetwork.IsMasterClient) {
            Debug.Log("I am a master " + PhotonNetwork.NickName + " and " + newPlayer.NickName + " entered the room");
            PV.RPC("RPC_SetTimer", RpcTarget.Others, timeToWait);
            PV.RPC("RPC_PlayerCountUpdate", RpcTarget.AllViaServer);
        }
        else {
            Debug.Log("I am a player" + PhotonNetwork.NickName  + " and " + newPlayer.NickName + " entered the room");
        }

        base.OnPlayerEnteredRoom(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player other)
    {

        if (PhotonNetwork.IsMasterClient) {
            Debug.Log("I am a master " + PhotonNetwork.NickName + " and " + other.NickName + " left the room");
            PV.RPC("RPC_PlayerCountUpdate", RpcTarget.AllViaServer);
        }
        PhotonNetwork.DestroyPlayerObjects(other);


        PV.RPC("RPC_PlayerCountUpdate", RpcTarget.AllViaServer);
        PlayerNicknamesUpdate();
        PV.RPC("PRC_PlayerNicknamesSet", RpcTarget.AllBufferedViaServer, playersNicks.text);
    }

    [PunRPC]
    void RPC_PlayerCountUpdate()  {
        playerCount = PhotonNetwork.PlayerList.Length;
        readyToStart = (playerCount == roomSize);

        roomCountDisplay.text = playerCount.ToString() + ":" + roomSize.ToString();
    }


    [PunRPC]
    void PRC_PlayerNicknamesSet(string names)
    {
        playersNicks.text = names;
    }

    void PlayerNicknamesUpdate() {
        string tmp = "";

        foreach (Player p in PhotonNetwork.PlayerList) {
            string nick = p.NickName;
            tmp += nick + System.Environment.NewLine;
        }

        playersNicks.text = tmp;
    }

    [PunRPC]
    private void RPC_SetTimer(float timeIn) 
    {
        countDownTime = timeIn;
    }

    void ResetTimer()
    {
        countDownTime = timeToWait;
    }

    void StartGame()
    {
        startingGame = true;

        if (!PhotonNetwork.IsMasterClient) {
            return;
        }

        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(multiplayerScene);
    }

    public void OnCancelButtonPressed() 
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(menuScene);
    }
}
