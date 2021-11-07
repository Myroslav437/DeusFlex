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
    public Text roomCountDisplayLabel;
    public Text countDownDisplay;
    public Text countDownDisplayLabel;
    public Text playersNicks;
    public Text chooseYourTeam;
    public Text WaitingRoomLabel;
    public Text GetReadyLabel;
    public Text BlueTeamInfo;
    public Text RedTeamInfo;
    public Text PlayersNickNames;


    public GameObject redSquare;
    public GameObject blueSquare;

    public GameObject cancelButton;

    public int multiplayerScene;
    public int menuScene;

    public int roomSize;
    public float timeToWait;
    //public float maxWaitTime;

    private int playerCount;
    private float countDownTime;

    private bool readyToStart;
    private bool startingMatchMaking;
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

        // Set nickNames
        GameObject avatar = LocalPlayerInfo.LPI.networkPlayer.GetComponent<PhotonPlayer>().myAvatar;
        int avatarID = avatar.GetComponent<PhotonView>().ViewID;
        PV.RPC("RPC_SetNickName", RpcTarget.AllBufferedViaServer, avatarID, LocalPlayerInfo.LPI.myNickName);
    }

    void Update()
    {
        WaitForMorePlayers();
        WaitForPlayersMatchMaking();
    }

    void WaitForMorePlayers()
    {
        if (readyToStart) { // staring countdown
            countDownTime -= Time.deltaTime;

            // Set squares
            redSquare.SetActive(true);
            blueSquare.SetActive(true);

            // Set "ChooseYourTeam"
            chooseYourTeam.gameObject.SetActive(false);

            // Set Cancel button:
            cancelButton.SetActive(false);

            // Set "PlayersCount"
            roomCountDisplay.gameObject.SetActive(false);
            roomCountDisplayLabel.gameObject.SetActive(false);

            // Set "Waiting room"
            WaitingRoomLabel.gameObject.SetActive(false);

            // Set "Get Ready"
            GetReadyLabel.gameObject.SetActive(true);

            // Set PlayersNickNames
            PlayersNickNames.gameObject.SetActive(false);

            // Set BlueTeamInfo and RedTeamInfo:
            BlueTeamInfo.gameObject.SetActive(true);
            RedTeamInfo.gameObject.SetActive(true);

            // Set "CountDown"
            countDownDisplay.gameObject.SetActive(true);
            countDownDisplayLabel.gameObject.SetActive(true);

            // update commands names
            updateRedTeamList();
            updateBlueTeamList();

        }
        else if (startingMatchMaking)  { // full room
            ResetTimer();

            // Set squares
            redSquare.SetActive(true);
            blueSquare.SetActive(true);

            // Set "ChooseYourTeam"
            chooseYourTeam.gameObject.SetActive(true);

            // Set Cancel button:
            cancelButton.SetActive(false);

            // Set "PlayersCount"
            roomCountDisplay.gameObject.SetActive(false);
            roomCountDisplayLabel.gameObject.SetActive(false);

            // Set "CountDown"
            countDownDisplay.gameObject.SetActive(false);
            countDownDisplayLabel.gameObject.SetActive(false);

            // Set "Waiting room"
            WaitingRoomLabel.gameObject.SetActive(false);

            // Set "Get Ready"
            GetReadyLabel.gameObject.SetActive(false);

            // Set PlayersNickNames
            PlayersNickNames.gameObject.SetActive(false);

            // Set BlueTeamInfo and RedTeamInfo:
            BlueTeamInfo.gameObject.SetActive(true);
            RedTeamInfo.gameObject.SetActive(true);

            // update commands names
            updateRedTeamList();
            updateBlueTeamList();
        }
        else {
            ResetTimer();

            // Set squares
            redSquare.SetActive(false);
            blueSquare.SetActive(false);

            // Set "ChooseYourTeam"
            chooseYourTeam.gameObject.SetActive(false);

            // Set Cancel button:
            cancelButton.SetActive(true);

            // Set "CountDown"
            countDownDisplay.gameObject.SetActive(false);
            countDownDisplayLabel.gameObject.SetActive(false);

            // Set "PlayersCount"
            roomCountDisplay.gameObject.SetActive(true);
            roomCountDisplayLabel.gameObject.SetActive(true);

            // Set PlayersNickNames
            PlayersNickNames.gameObject.SetActive(true);

            // Set "Waiting room"
            WaitingRoomLabel.gameObject.SetActive(true);

            // Set "Get Ready"
            GetReadyLabel.gameObject.SetActive(false);

            // Set BlueTeamInfo and RedTeamInfo:
            BlueTeamInfo.gameObject.SetActive(false);
            RedTeamInfo.gameObject.SetActive(false);
        }

        countDownDisplay.text = string.Format("{0:00}", countDownTime);

        if (countDownTime <= 0f) {
            if (startingGame) {
                return;
            }

            StartGame();
        }
    }

    void WaitForPlayersMatchMaking()
    {
        if (TeamController.TC.playersData.Count == roomSize)
        {
            int redTeamCount = 0, blueTeamCount = 0;
            foreach (KeyValuePair<int, TeamController.PlayerData> p in TeamController.TC.playersData)
            {
                if (p.Value.team == "Red")
                {
                    ++redTeamCount;
                }
                else if (p.Value.team == "Blue")
                {
                    ++blueTeamCount;
                }
            }

            readyToStart = (redTeamCount == (roomSize / 2) && blueTeamCount == (roomSize / 2));
        }
        else {
            readyToStart = false;
        }
    }

    private void updateRedTeamList() 
    {
        RedTeamInfo.text = "Red Team" + System.Environment.NewLine;
        string nicks = "";
        foreach (KeyValuePair<int, TeamController.PlayerData> pd in TeamController.TC.playersData) {
            if (pd.Value.team == "Red") {
                nicks += pd.Value.nickName + " " + pd.Value.playerPVID + " " + pd.Key + System.Environment.NewLine;
            }
        }

        RedTeamInfo.text += nicks;
    }

    private void updateBlueTeamList()
    {
        BlueTeamInfo.text = "Blue Team" + System.Environment.NewLine;
        string nicks = "";
        foreach (KeyValuePair<int, TeamController.PlayerData> pd in TeamController.TC.playersData) {
            if (pd.Value.team == "Blue") {
                nicks += pd.Value.nickName + " " + pd.Value.playerPVID + " " + pd.Key + System.Environment.NewLine;
            }
        }

        BlueTeamInfo.text += nicks;
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
            //Debug.Log("I am a master " + PhotonNetwork.NickName + " and " + newPlayer.NickName + " entered the room");
            Debug.Log("I am a master " + PhotonNetwork.LocalPlayer.ActorNumber + " and " + newPlayer.ActorNumber + " entered the room");
            PV.RPC("RPC_SetTimer", RpcTarget.Others, timeToWait);
            PV.RPC("RPC_PlayerCountUpdate", RpcTarget.AllViaServer);
        }
        else {
            Debug.Log("I am a player " + PhotonNetwork.LocalPlayer.ActorNumber + " and " + newPlayer.ActorNumber + " entered the room");
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

        TeamController.TC.RemovePlayerInfo(other.ActorNumber);
    }

    [PunRPC]
    void RPC_PlayerCountUpdate()  {
        playerCount = PhotonNetwork.PlayerList.Length;
        startingMatchMaking = (playerCount == roomSize);

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


    [PunRPC]
    void RPC_SetNickName(int playerViewID, string nickName)
    {
        Debug.Log("Executing RPC_SetNickName for " + nickName);

        GameObject obj = PhotonHelper.FindObjectViaPVID(playerViewID);

        NicknameLabel nl = obj.GetComponent<NicknameLabel>();
        nl.nickName = nickName;
        nl.UpdateText();
    }
}
